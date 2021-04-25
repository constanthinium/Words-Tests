using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Words_Tests.Pages
{
    public partial class EditTestPage : Page
    {
        private readonly string _testFilePath;
        private readonly ObservableCollection<Pair> _pairs = new ObservableCollection<Pair>();

        public EditTestPage(int questionsCount)
        {
            InitializeComponent();
            QuestionsDataGrid.ItemsSource = _pairs;
            QuestionsDataGrid.AutoGeneratingColumn += (sender, args) => 
                args.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            _pairs.CollectionChanged += (sender, args) =>
            {
                QuestionsDataGrid.Items.Refresh();
                QuestionsCountTextBlock.Text = "Questions Count: " + _pairs.Count;
            };

            for (var i = 0; i < questionsCount; i++)
            {
                _pairs.Add(new Pair());
            }
        }

        public EditTestPage(string testFilePath, IEnumerable<(string question, string answer)> questions) : this(0)
        {
            _testFilePath = testFilePath;

            foreach (var (question, answer) in questions)
            {
                _pairs.Add(new Pair(question, answer));
            }
        }

        private void AddQuestion(object sender, RoutedEventArgs e)
        {
            _pairs.Add(new Pair());
        }

        private void SaveTest_Click(object sender, RoutedEventArgs e)
        {
            if (_testFilePath != null)
            {
                SaveTest(_testFilePath, _pairs);
                MessageBox.Show("Этот тест изменен", "Тест изменен", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var testName = Interaction.InputBox("Введите имя теста");
                if (testName == "") return;
                var newTestFilePath = Path.Combine(App.testsDir, testName) + ".xml";

                if (File.Exists(newTestFilePath))
                {
                    MessageBox.Show("Тест с таким названием уже существует, попробуйте другое имя",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    SaveTest(newTestFilePath, _pairs);
                    MessageBox.Show("Тест сохранен", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void SaveTest(string saveFilePath, IReadOnlyCollection<Pair> pairs)
        {
            throw new NotImplementedException();
        }

        private void RemoveQuestion(object sender, RoutedEventArgs e)
        {
            _pairs.RemoveAt(QuestionsDataGrid.SelectedIndex);
        }
    }
}
