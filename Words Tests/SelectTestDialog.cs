using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace Words_Tests
{
    internal class SelectTestDialog
    {
        public List<(string question, string answer)> Questions;
        public string TestFilePath;

        public bool Show(Window parent)
        {
            var window = new Window
            {
                Title = "Выберите тест",
                WindowStyle = WindowStyle.None,
                Width = parent.ActualWidth / 2,
                Height = parent.ActualHeight / 2,
                Owner = parent,
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

            window.Closing += (sender, args) => parent.Effect = null;
            var listBox = new ListBox();
            window.Content = listBox;
            listBox.MouseDoubleClick += (sender, args) => window.DialogResult = true;
            var testFiles = Directory.GetFiles(App.testsDir, "*.xml");

            if (!Directory.Exists(App.testsDir) || testFiles.Length == 0)
            {
                MessageBox.Show("Похоже, у вас нет ни одного теста, который можно было бы пройти или отредактировать.",
                    "Нет тестов", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            foreach (var file in testFiles)
            {
                listBox.Items.Add(Path.GetFileNameWithoutExtension(file));
            }

            parent.Effect = new BlurEffect { Radius = 16 };
            if (window.ShowDialog() == false)
            {
                return false;
            }

            try
            {
                TestFilePath = testFiles[listBox.SelectedIndex];

                using (var testFile = File.OpenRead(TestFilePath))
                {
                    Questions = (List<(string question, string answer)>)App.serializer.Deserialize(testFile);
                }

                return true;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Тест, который вы пытаетесь открыть, либо не является тестом, либо поврежден.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
