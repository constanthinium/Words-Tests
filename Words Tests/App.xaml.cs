using System.Collections.ObjectModel;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace Words_Tests
{
    public partial class App
    {
        public static readonly XmlSerializer Serializer = new XmlSerializer(typeof(ObservableCollection<QuestionAnswer>));

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
                Title = "Program error occurred. Please send this text to developer.",
                Width = 800,
                Height = 450,
                WindowStyle = WindowStyle.ToolWindow,
                ResizeMode = ResizeMode.NoResize,
                Owner = Current.MainWindow
            };

            window.Loaded += (o, args) => SystemSounds.Hand.Play();
            window.ShowDialog();
        }
    }
}
