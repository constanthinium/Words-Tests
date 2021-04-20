using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Words_Tests
{
    public partial class PassTestWindow : Window
    {
        private int score;
        private int currentPairId;
        private readonly List<(string question, string answer)> pairs;
        private readonly int questionCount;
        private readonly Random rnd = new Random();

        private const string stringScatterLetters = "Рассыпать буквы (-2 балла)";
        private const string stringMixLetters = "Перемешать буквы";

        public PassTestWindow(List<(string question, string answer)> pairs)
        {
            InitializeComponent();
            SizeChanged += (s, e) => MixLetters();
            this.pairs = pairs;
            questionCount = pairs.Count;
            currentPairId = rnd.Next(pairs.Count);
            textBlockQuestion.Text = pairs[currentPairId].question;
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            if (textBoxAnswer.Text.Length == 0)
                return;
            if (textBoxAnswer.Text == pairs[currentPairId].answer)
            {
                score++;
                score +=
                    Convert.ToInt32(buttonShowFirstLetter.IsEnabled) +
                    Convert.ToInt32(buttonShowLength.IsEnabled) +
                    Convert.ToInt32(((TextBlock)((StackPanel)buttonScatterLetters.Content).Children[1]).Text == stringScatterLetters) * 2;
                MessageBox.Show("Верно", "Верно", MessageBoxButton.OK, MessageBoxImage.Information);
                buttonShowFirstLetter.IsEnabled = buttonShowLength.IsEnabled = true;
                ((TextBlock)((StackPanel)buttonScatterLetters.Content).Children[1]).Text = stringScatterLetters;
            }
            else
            {
                MessageBox.Show($"Неверно. Верный ответ: {pairs[currentPairId].answer}", "Неверно", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            pairs.RemoveAt(currentPairId);
            if (pairs.Count == 0)
            {
                Close();
                MessageBox.Show($"Тест пройден. Очки: {score} из {questionCount * 5}", "Тест пройден", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            currentPairId = rnd.Next(pairs.Count);
            textBlockQuestion.Text = pairs[currentPairId].question;
            textBoxAnswer.Clear();
        }

        private void TextBoxAnswer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Submit(null, null);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new MainWindow
            {
                Left = Left,
                Top = Top
            }.Show();
        }

        private void ShowFirstLetter(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Первая буква: {pairs[currentPairId].answer[0]}", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Information);
            ((Button)sender).IsEnabled = false;
        }

        private void ShowLength(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Длина слова: {pairs[currentPairId].answer.Length}", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Information);
            ((Button)sender).IsEnabled = false;
        }

        private void ScatterLetters(object sender, RoutedEventArgs e)
        {
            if (((TextBlock)((StackPanel)buttonScatterLetters.Content).Children[1]).Text == stringScatterLetters)
            {
                textBoxAnswer.IsEnabled = buttonDone.IsEnabled = false;
                foreach (char letter in pairs[currentPairId].answer)
                {
                    Button buttonLetter = new Button
                    {
                        Content = letter,
                        Padding = new Thickness(10),
                    };
                    buttonLetter.Loaded += (s, ea) => SetRandomCanvasPos(buttonLetter);
                    buttonLetter.Click += delegate
                    {
                        if (pairs[currentPairId].answer[textBoxAnswer.Text.Length] == (char)buttonLetter.Content)
                        {
                            textBoxAnswer.AppendText(buttonLetter.Content.ToString());
                            canvasScatter.Children.Remove(buttonLetter);
                            if (textBoxAnswer.Text == pairs[currentPairId].answer)
                            {
                                Submit(null, null);
                                textBoxAnswer.IsEnabled = buttonDone.IsEnabled = true;
                            }
                        }
                        else MessageBox.Show("Неверно");
                    };
                    canvasScatter.Children.Add(buttonLetter);
                }
                ((TextBlock)((StackPanel)buttonScatterLetters.Content).Children[1]).Text = stringMixLetters;
            }
            else if (((TextBlock)((StackPanel)buttonScatterLetters.Content).Children[1]).Text == stringMixLetters)
                MixLetters();
        }

        private void MixLetters()
        {
            foreach (Button buttonLetter in canvasScatter.Children)
                SetRandomCanvasPos(buttonLetter);
        }

        private void SetRandomCanvasPos(Button buttonLetter)
        {
            Canvas.SetLeft(buttonLetter, rnd.Next(0, (int)(canvasScatter.ActualWidth - buttonLetter.ActualWidth)));
            Canvas.SetTop(buttonLetter, rnd.Next(0, (int)(canvasScatter.ActualHeight - buttonLetter.ActualHeight)));
        }
    }
}
