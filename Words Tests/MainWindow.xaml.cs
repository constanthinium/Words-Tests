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
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (MainFrame.Content is EditTestPage editTestPage && !editTestPage.IsTestSaved)
            {
                e.Cancel = MessageBox.Show("You have unsaved changes. Close?", "Unsaved changes",
                    MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel;
            }
        }
    }
}
