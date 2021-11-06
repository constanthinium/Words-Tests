using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Words_Tests.Windows;

namespace Words_Tests.Pages
{
    public partial class PassTestPage
    {
        private int _score;
        private int _currentQuestionIndex;
        private readonly ObservableCollection<QuestionAnswer> _pairs;
        private readonly int _questionCount;
        private readonly Random _random = new Random();

        public PassTestPage(ObservableCollection<QuestionAnswer> pairs)
        {
            InitializeComponent();
            _pairs = pairs;
            _questionCount = pairs.Count;
            NextQuestion();
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            if (AnswerTextBox.Text == "") return;

            if (AnswerTextBox.Text == _pairs[_currentQuestionIndex].Answer)
            {
                _score += 5;
                ApplyPenalty(ref _score);
                MessageBox.Show("Верно", "Верно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Неверно. Верный ответ: {_pairs[_currentQuestionIndex].Answer}",
                    "Неверно", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _pairs.RemoveAt(_currentQuestionIndex);

            if (_pairs.Count == 0)
            {
                MessageBox.Show($"Тест пройден. Очки: {_score} из {_questionCount * 5}",
                    "Тест пройден", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow.MainFrameInstance.GoBack();
            }
            else
            {
                AnswerTextBox.Clear();
                RestoreHints();
                NextQuestion();
            }
        }

        private void TextBoxAnswer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Submit(null, null);
            }
        }

        private void ShowFirstLetter(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            MessageBox.Show("The first letter - " + _pairs[_currentQuestionIndex].Answer[0],
                "Подсказка", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowLength(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            MessageBox.Show("The word length - " + _pairs[_currentQuestionIndex].Answer.Length,
                "Подсказка", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ScatterLetters(object sender, RoutedEventArgs e)
        {
            AnswerTextBox.IsReadOnly = true;
            DoneButton.Visibility = Visibility.Collapsed;
            ScatterLettersButton.Visibility = Visibility.Collapsed;

            foreach (var buttonLetter in _pairs[_currentQuestionIndex].Answer.Select(letter => new Button
            {
                Content = letter,
                Padding = new Thickness(10)
            }))
            {
                buttonLetter.Loaded += (o, args) => SetRandomCanvasPos(buttonLetter);
                buttonLetter.Click += LetterClick;
                ScatterCanvas.Children.Add(buttonLetter);
            }
        }

        private void LetterClick(object sender, RoutedEventArgs e)
        {
            var letterButton = (Button)sender;

            if (_pairs[_currentQuestionIndex].Answer[AnswerTextBox.Text.Length] == (char)letterButton.Content)
            {
                AnswerTextBox.AppendText(letterButton.Content.ToString());
                ScatterCanvas.Children.Remove(letterButton);
                if (AnswerTextBox.Text != _pairs[_currentQuestionIndex].Answer) return;
                Submit(null, null);
                AnswerTextBox.IsEnabled = DoneButton.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Неверно");
            }
        }

        private void SetRandomCanvasPos(FrameworkElement buttonLetter)
        {
            Canvas.SetLeft(buttonLetter, _random.Next(0, (int)(ScatterCanvas.ActualWidth - buttonLetter.ActualWidth)));
            Canvas.SetTop(buttonLetter, _random.Next(0, (int)(ScatterCanvas.ActualHeight - buttonLetter.ActualHeight)));
        }

        private void MixLetters(object sender, RoutedEventArgs e)
        {
            foreach (Button buttonLetter in ScatterCanvas.Children)
            {
                SetRandomCanvasPos(buttonLetter);
            }
        }

        private void ApplyPenalty(ref int score)
        {
            if (!ShowFirstLetterButton.IsEnabled)
            {
                score--;
            }

            if (!ShowLengthButton.IsEnabled)
            {
                score--;
            }

            if (!ScatterLettersButton.IsVisible)
            {
                score -= 2;
            }
        }

        private void RestoreHints()
        {
            ShowFirstLetterButton.IsEnabled = true;
            ShowLengthButton.IsEnabled = true;
            ScatterLettersButton.Visibility = Visibility.Visible;
            DoneButton.Visibility = Visibility.Visible;
            AnswerTextBox.IsReadOnly = false;
        }

        private void NextQuestion()
        {
            _currentQuestionIndex = _random.Next(_pairs.Count);
            var questionAnswer = _pairs[_currentQuestionIndex];
            QuestionTextBlock.Text = questionAnswer.Question;
            QuestionImage.Source = questionAnswer.GetImageSourceOrReturnNull();
        }
    }
}
