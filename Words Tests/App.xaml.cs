using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Net;
using System.Reflection;
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

        public App() => Startup += (sender, args) => CheckForUpdates();

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
            var client = new WebClient();
            client.Headers.Add(HttpRequestHeader.UserAgent, AppDomain.CurrentDomain.FriendlyName);
            var releasesJson = client.DownloadString(releasesUrl);
            var serializer = new DataContractJsonSerializer(typeof(Release[]));
            var jsonStream = new MemoryStream(Encoding.ASCII.GetBytes(releasesJson));
            var releases = (Release[])serializer.ReadObject(jsonStream);
            var latestRelease = releases[0];
            var latestVersion = latestRelease.TagName.Remove(0, 1);
            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            if (currentVersion == latestVersion) return;
            var result = MessageBox.Show("You are not using the latest version. Do you want to download it?",
                "Check for updates", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result != MessageBoxResult.OK) return;
            var downloadUrl = latestRelease.Assets[0].DownloadUrl;
            Process.Start(downloadUrl);
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
