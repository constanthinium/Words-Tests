using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace Words_Tests
{
    public partial class App
    {
        public static readonly XmlSerializer Serializer = new XmlSerializer(typeof(ObservableCollection<QuestionAnswer>));
        public static readonly WebClient Client = new WebClient();
        public static readonly string CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        private static readonly string ProgramName = AppDomain.CurrentDomain.FriendlyName;

        public App()
        {
            Startup += (sender, args) => CheckForUpdates();
            Client.Headers.Add(HttpRequestHeader.UserAgent, ProgramName);
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            var textBox = new TextBox
            {
                Text = e.Exception.ToString(),
                IsReadOnly = true,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            var window = new Window
            {
                Content = textBox,
                Title = "A program error has occurred. Send this text to the developer.",
                Width = 800,
                Height = 450,
                WindowStyle = WindowStyle.ToolWindow,
                ResizeMode = ResizeMode.NoResize,
                Topmost = true
            };

            window.Loaded += (o, args) => SystemSounds.Hand.Play();
            window.ShowDialog();
        }

        private static void CheckForUpdates()
        {
            const string releasesUrl = "https://api.github.com/repos/constanthinium/Words-Tests/releases";
            var releasesJson = Client.DownloadString(releasesUrl);
            var serializer = new DataContractJsonSerializer(typeof(Release[]));
            var jsonStream = new MemoryStream(Encoding.ASCII.GetBytes(releasesJson));
            var releases = (Release[])serializer.ReadObject(jsonStream);
            var latestRelease = releases[0];
            var latestVersion = latestRelease.TagName.TrimStart('v');
            if (CurrentVersion == latestVersion) Debug.WriteLine("latest version");
            else
            {
                Debug.WriteLine("outdated version");
                Update(latestRelease.Assets[0].DownloadUrl);
            }
        }

        private static void Update(string downloadUrl)
        {
            var downloadedFilePath = $"{GetDownloadsFolder()}\\{ProgramName}";
            Client.DownloadFile(downloadUrl, downloadedFilePath);
            var appLocation = Assembly.GetExecutingAssembly().Location;

            var replaceBatch =
                "@echo off\n" +
                ":loop\n" +
                "tasklist | find \"Words Tests.exe\" > nul\n" +
                "if errorlevel 1 goto continue else\n" +
                "timeout /nobreak 1 > nul\n" +
                "goto loop\n" +
                ":continue\n" +
                $"move /y \"{downloadedFilePath}\", \"{appLocation}\"\n" +
                $"\"{appLocation}\"";

            var tempName = Path.GetTempFileName();
            tempName = Path.ChangeExtension(tempName, "bat");
            File.WriteAllText(tempName, replaceBatch);
            var startInfo = new ProcessStartInfo(tempName)
            { WindowStyle = ProcessWindowStyle.Hidden };
            Process.Start(startInfo);
            Current.Shutdown();
        }

        [DllImport("shell32", CharSet = CharSet.Unicode)]
        private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)]
            Guid rfid, uint dwFlags, IntPtr hToken, out string ppszPath);

        private static string GetDownloadsFolder()
        {
            SHGetKnownFolderPath(new Guid("{374DE290-123F-4565-9164-39C4925E467B}"),
                0, IntPtr.Zero, out var outPath);
            return outPath;
        }

        [DataContract]
        private struct Release
        {
            [DataMember(Name = "tag_name")]
            public string TagName;

            [DataMember(Name = "assets")]
            public Asset[] Assets;

            [DataContract]
            public struct Asset
            {
                [DataMember(Name = "browser_download_url")]
                public string DownloadUrl;
            }
        }
    }
}
