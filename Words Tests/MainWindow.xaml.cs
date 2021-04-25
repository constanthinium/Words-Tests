using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Words_Tests.Pages;

namespace Words_Tests
{
    public partial class MainWindow : Window
    {
        public static Window Instance;
        public static Frame MainFrameInstance;

        public MainWindow()
        {
            InitializeComponent();

            Instance = this;
            MainFrameInstance = MainFrame;
            MainFrame.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseBack, (sender, args) => { }));
            MainFrame.Navigating += MainWindow_OnClosing;
            MainFrame.Navigated += (sender, args) =>
                BackButton.Visibility = MainFrame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
            BackButton.Click += (sender, args) => MainFrame.GoBack();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (MainFrame.Content is EditTestPage editTestPage && !editTestPage.IsTestSaved)
            {
                e.Cancel = MessageBox.Show("You have unsaved changes. Exit?", "Unsaved changes",
                    MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel;
            }
        }
    }
}
