using System.Windows;
using System.Windows.Controls;

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
        }
    }
}
