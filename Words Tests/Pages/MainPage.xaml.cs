using System.Windows;
using System.Windows.Controls;

namespace Words_Tests.Pages
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void CreateTest(object sender, RoutedEventArgs e)
        {
            var createTestPage = new EditTestPage();
            MainWindow.MainFrameInstance.Navigate(createTestPage);
        }

        private void PassTest(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectTestDialog();
            if (!dialog.Show(MainWindow.Instance)) return;

            var passTestPage = new PassTestPage(dialog.Questions);
            MainWindow.MainFrameInstance.Navigate(passTestPage);
        }

        private void EditTest(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectTestDialog();
            if (!dialog.Show(MainWindow.Instance)) return;

            var createTestPage = new EditTestPage(dialog.TestFilePath, dialog.Questions);
            MainWindow.MainFrameInstance.Navigate(createTestPage);
        }

    }
}
