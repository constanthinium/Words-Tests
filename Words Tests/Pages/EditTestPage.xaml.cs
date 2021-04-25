﻿using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Words_Tests.Pages
{
    public partial class EditTestPage : Page
    {
        private readonly string _testFilePath;
        private readonly ObservableCollection<QuestionAnswer> _pairs = new ObservableCollection<QuestionAnswer>();

        public EditTestPage(int initialQuestionsCount)
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

            for (var i = 0; i < initialQuestionsCount; i++)
            {
                _pairs.Add(new QuestionAnswer());
            }
        }

        public EditTestPage(string testFilePath, IEnumerable<QuestionAnswer> questions) : this(0)
        {
            _testFilePath = testFilePath;

            foreach (var questionAnswer in questions)
            {
                _pairs.Add(questionAnswer);
            }
        }

        private void AddQuestion(object sender, RoutedEventArgs e)
        {
            _pairs.Add(new QuestionAnswer());
        }

        private void SaveTest_Click(object sender, RoutedEventArgs e)
        {
            if (_pairs.Any(q => string.IsNullOrWhiteSpace(q.Question) || string.IsNullOrWhiteSpace(q.Answer)))
            {
                MessageBox.Show("You cannot save empty questions and answers",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (_testFilePath != null)
            {
                SaveTest(_testFilePath, _pairs);
            }
            else
            {
                var testName = Interaction.InputBox("Введите имя теста");
                if (testName == "") return;
                var newTestFilePath = testName + ".xml";

                if (File.Exists(newTestFilePath))
                {
                    MessageBox.Show("Тест с таким названием уже существует, попробуйте другое имя",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    SaveTest(newTestFilePath, _pairs);
                }
            }
        }

        private static void SaveTest(string saveFilePath, IReadOnlyCollection<QuestionAnswer> pairs)
        {
            using (var file = File.Create(saveFilePath))
            {
                App.Serializer.Serialize(file, pairs);
            }

            MessageBox.Show("Тест сохранен", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            MainWindow.MainFrameInstance.GoBack();
        }

        private void RemoveQuestion(object sender, RoutedEventArgs e)
        {
            _pairs.RemoveAt(QuestionsDataGrid.SelectedIndex);
        }
    }
}
