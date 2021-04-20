using System.IO;
using System.Windows;
using Forms = System.Windows.Forms;
using Microsoft.Win32;
using System.Collections.Generic;
using System;

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
            new CreateTestWindow
            {
                Left = Left,
                Top = Top
            }.Show();
            Close();
        }

        private void PassTest(object sender, RoutedEventArgs e)
        {
            var form = new Forms.Form
            {
                Text = "Выберите тест",
                ShowIcon = false,
                MinimizeBox = false,
                MaximizeBox = false,
                FormBorderStyle = Forms.FormBorderStyle.FixedDialog,
                Left = Forms.Cursor.Position.X,
                Top = Forms.Cursor.Position.Y,
            };
            var listBox = new Forms.ListBox
            {
                Dock = Forms.DockStyle.Fill
            };
            listBox.SelectedValueChanged += (s, ea) =>
            {
                form.DialogResult = Forms.DialogResult.OK;
                form.Close();
            };
            if (!Directory.Exists(App.testsDir))
            {
                NoTestsMessage();
                return;
            }
            string[] testFiles = Directory.GetFiles(App.testsDir, "*.xml");
            if (testFiles.Length == 0)
            {
                NoTestsMessage();
                return;
            }
            foreach (string file in testFiles)
            {
                listBox.Items.Add(Path.GetFileNameWithoutExtension(file));
            }
            form.Controls.Add(listBox);
            if (form.ShowDialog() == Forms.DialogResult.Cancel)
                return;

            try
            {
                new PassTestWindow((List<(string question, string answer)>)App.serializer.Deserialize(File.OpenRead(testFiles[listBox.SelectedIndex])))
                {
                    Left = Left,
                    Top = Top
                }.Show();
                Close();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Тест, который вы пытаетесь открыть, либо не является тестом, либо повреждён", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Import(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XML Test Files (*.xml)|*.xml",
            };
            if (openFileDialog.ShowDialog() == true)
            {
                if (Path.GetDirectoryName(openFileDialog.FileName) != App.testsDir)
                {
                    File.Copy(openFileDialog.FileName, Path.Combine(App.testsDir, openFileDialog.SafeFileName));
                    MessageBox.Show("Тест импортирован. Теперь его можно пройти.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Нельзя импортировать уже готовый тест.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void NoTestsMessage()
        {
            MessageBox.Show("Похоже, у вас нет ни одного теста, который можно было бы пройти.", "Нет тестов", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
