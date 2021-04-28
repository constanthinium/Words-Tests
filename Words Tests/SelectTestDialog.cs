using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace Words_Tests
{
    internal class SelectTestDialog
    {
        public ObservableCollection<QuestionAnswer> Questions;
        public string TestFilePath;

        public bool Show()
        {
            var mainWindow = Application.Current.MainWindow;

            var window = new Window
            {
                Title = "Выберите тест",
                WindowStyle = WindowStyle.None,
                Width = mainWindow.ActualWidth / 2,
                Height = mainWindow.ActualHeight / 2,
                Owner = mainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false
            };

            window.KeyDown += (sender, args) =>
            {
                if (args.Key == Key.Escape)
                {
                    window.Close();
                }
            };

            var listBox = new ListBox();
            window.Closing += (sender, args) => mainWindow.Effect = null;
            window.Content = listBox;

            listBox.MouseDoubleClick += (sender, args) =>
            {
                if (listBox.SelectedIndex > -1)
                {
                    window.DialogResult = true;
                }
            };

            var testFiles = Directory.GetFiles(".", "*.xml");

            if (testFiles.Length == 0)
            {
                MessageBox.Show("Похоже, у вас нет ни одного теста, который можно было бы пройти или отредактировать.",
                    "Нет тестов", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            foreach (var file in testFiles)
            {
                listBox.Items.Add(Path.GetFileNameWithoutExtension(file));
            }

            mainWindow.Effect = new BlurEffect { Radius = 16 };
            if (window.ShowDialog() == false)
            {
                return false;
            }

            try
            {
                TestFilePath = testFiles[listBox.SelectedIndex];

                using (var testFile = File.OpenRead(TestFilePath))
                {
                    Questions = (ObservableCollection<QuestionAnswer>) App.Serializer.Deserialize(testFile);
                }

                return true;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Тест, который вы пытаетесь открыть, либо не является тестом, либо поврежден.",
                    "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
