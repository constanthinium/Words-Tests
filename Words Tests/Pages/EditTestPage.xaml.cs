using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using Words_Tests.Windows;

namespace Words_Tests.Pages
{
    public partial class EditTestPage
    {
        public bool IsTestSaved;
        private QuestionAnswer deletedQuesion;
        private readonly string _testFilePath;
        private readonly ObservableCollection<QuestionAnswer> _pairs = new ObservableCollection<QuestionAnswer>();

        public EditTestPage(int initialQuestionsCount)
        {
            InitializeComponent();
            QuestionsDataGrid.ItemsSource = _pairs;
            RemoveSelectionMenuItem.Click += (sender, args) => QuestionsDataGrid.UnselectAllCells();

            _pairs.CollectionChanged += (sender, args) =>
            {
                QuestionsDataGrid.Items.Refresh();
                QuestionsCountTextBlock.Text = "Questions Count: " + _pairs.Count;
                IsTestSaved = false;
                if (args.Action != NotifyCollectionChangedAction.Add) return;
                var addedQuestion = (QuestionAnswer)args.NewItems[0];
                addedQuestion.PropertyChanged += (o, eventArgs) => IsTestSaved = false;
            };

            for (var i = 0; i < initialQuestionsCount; i++)
            {
                _pairs.Add(new QuestionAnswer());
            }

            IsTestSaved = true;
        }

        public EditTestPage(string testFilePath, IEnumerable<QuestionAnswer> questions) : this(0)
        {
            _testFilePath = testFilePath;

            foreach (var questionAnswer in questions)
            {
                _pairs.Add(questionAnswer);
            }

            IsTestSaved = true;
        }

        private void AddQuestion(object sender, RoutedEventArgs e)
        {
            _pairs.Add(new QuestionAnswer());
        }

        private void SaveTestClick(object sender, RoutedEventArgs e)
        {
            if (_pairs.Any(q => string.IsNullOrWhiteSpace(q.Question) || string.IsNullOrWhiteSpace(q.Answer)))
            {
                MessageBox.Show("You cannot save empty questions and answers",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void SaveTest(string saveFilePath, IReadOnlyCollection<QuestionAnswer> pairs)
        {
            using (var file = File.Create(saveFilePath))
            {
                App.Serializer.Serialize(file, pairs);
            }

            IsTestSaved = true;
            MessageBox.Show("Тест сохранен", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            MainWindow.MainFrameInstance.GoBack();
        }

        private void RemoveQuestion(object sender, RoutedEventArgs e)
        {
            int index = QuestionsDataGrid.SelectedIndex;
            deletedQuesion = _pairs[index];
            _pairs.RemoveAt(index);
            RestoreLastDeletedQuestion.IsEnabled = true;
        }

        private void AddImage(object sender, RoutedEventArgs e)
        {
            var imageQuery = ((QuestionAnswer)QuestionsDataGrid.SelectedItem).Answer;

            if (string.IsNullOrWhiteSpace(imageQuery))
            {
                MessageBox.Show("You need to enter question first", "Enter Question",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var imageWindow = new QuestionImageWindow(imageQuery)
                { Owner = Application.Current.MainWindow };

                if (imageWindow.ShowDialog() == true)
                {
                    _pairs[QuestionsDataGrid.SelectedIndex].SetImageBytesFromBitmapSource(imageWindow.SelectedImage);
                }
            }
        }

        private void RestoreLastDeletedQuestion_Click(object sender, RoutedEventArgs e)
        {
            _pairs.Add(deletedQuesion);
            RestoreLastDeletedQuestion.IsEnabled = false;
        }
    }
}
