using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Words_Tests
{
    internal class SelectTestDialog
    {
        public List<(string question, string answer)> Questions;
        public string TestFilePath;

        public bool Show()
        {
            var cursorPos = System.Windows.Forms.Cursor.Position;

            var form = new System.Windows.Forms.Form
            {
                Text = "Выберите тест",
                ShowIcon = false,
                MinimizeBox = false,
                MaximizeBox = false,
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog,
                Left = cursorPos.X,
                Top = cursorPos.Y,
            };

            var listBox = new System.Windows.Forms.ListBox
            { Dock = System.Windows.Forms.DockStyle.Fill };

            listBox.SelectedValueChanged += (s, ea) =>
            {
                form.DialogResult = System.Windows.Forms.DialogResult.OK;
                form.Close();
            };

            if (!Directory.Exists(App.testsDir))
            {
                DisplayNoTestsMessage();
                return false;
            }

            var testFiles = Directory.GetFiles(App.testsDir, "*.xml");

            if (testFiles.Length == 0)
            {
                DisplayNoTestsMessage();
                return false;
            }

            foreach (var file in testFiles)
            {
                listBox.Items.Add(Path.GetFileNameWithoutExtension(file));
            }

            form.Controls.Add(listBox);

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return false;
            }

            try
            {
                TestFilePath = testFiles[listBox.SelectedIndex];

                using (var testFile = File.OpenRead(TestFilePath))
                { Questions = (List<(string question, string answer)>)App.serializer.Deserialize(testFile); }

                return true;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Тест, который вы пытаетесь открыть, либо не является тестом, либо поврежден", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private static void DisplayNoTestsMessage()
        {
            MessageBox.Show("Похоже, у вас нет ни одного теста, который можно было бы пройти или отредактировать.", "Нет тестов",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
