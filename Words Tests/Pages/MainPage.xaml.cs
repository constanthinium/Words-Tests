using System.Windows;
using Words_Tests.Windows;

namespace Words_Tests.Pages
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            VersionTextBlock.Text = $"Version: {App.CurrentVersion}";
        }

        private void CreateTest(object sender, RoutedEventArgs e)
        {
            var createTestPage = new EditTestPage(5);
            MainWindow.MainFrameInstance.Navigate(createTestPage);
        }

        private void PassTest(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectTestDialog();
            if (!dialog.Show()) return;

            var passTestPage = new PassTestPage(dialog.Questions);
            MainWindow.MainFrameInstance.Navigate(passTestPage);
        }

        private void EditTest(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectTestDialog();
            if (!dialog.Show()) return;

            var createTestPage = new EditTestPage(dialog.TestFilePath, dialog.Questions);
            MainWindow.MainFrameInstance.Navigate(createTestPage);
        }
    }
}
