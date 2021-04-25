using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace Words_Tests
{
    public partial class App : Application
    {
        public static readonly XmlSerializer Serializer = new XmlSerializer(typeof(ObservableCollection<QuestionAnswer>));

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            var textBox = new TextBox
            {
                IsReadOnly = true,
                Text = e.Exception.ToString()
            };
            var window = new Window
            {
                Content = textBox,
                Title = "Program error",
                Width = 800,
                Height = 450,
                WindowStyle = WindowStyle.ToolWindow,
                ResizeMode =  ResizeMode.NoResize,
                Topmost = true
            };
            window.Loaded += (o, args) =>
                MessageBox.Show("Program error occurred. Please send this information to developer.",
                    "Program error", MessageBoxButton.OK, MessageBoxImage.Error);
            window.ShowDialog();
        }
    }
}
