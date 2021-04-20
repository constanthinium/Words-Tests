using System.Windows;

namespace Words_Tests
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateTest(object sender, RoutedEventArgs e)
        {
            GoToWindow(new CreateTestWindow());
        }

        private void PassTest(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectTestDialog();
            if (!dialog.Show()) return;

            var passTestWindow = new PassTestWindow(dialog.Questions);
            GoToWindow(passTestWindow);
        }

        private void GoToWindow(Window destinationWindow)
        {
            destinationWindow.Left = Left;
            destinationWindow.Top = Top;
            destinationWindow.Show();
            Close();
        }

        private void EditTestButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectTestDialog();
            if (!dialog.Show()) return;

            GoToWindow(new CreateTestWindow(dialog.TestFilePath, dialog.Questions));
        }
    }
}
