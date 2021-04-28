using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Words_Tests
{
    public partial class QuestionImageWindow : Window
    {
        public QuestionImageWindow(string imageQuery)
        {
            InitializeComponent();
            QuestionTextBlock.Text = $"Select \"{imageQuery}\" Image";
            DisplayImages(imageQuery);
        }

        private async void DisplayImages(string imageQuery)
        {
            var uri = new Uri($"https://www.google.com/search?q={imageQuery}&tbm=isch");
            var pattern = new Regex("<img.+?src=[\"'](.+?)[\"'].*?>");
            var client = new WebClient();
            var response = await client.DownloadStringTaskAsync(uri);
            var matches = pattern.Matches(response);

            for (var i = 1; i <= 3; i++)
            {
                var imageAddress = matches[i].Groups[1].Value;
                var imageData = client.DownloadData(imageAddress);
                var imageView = new Image { Source = BytesToImage(imageData) };
                var imageButton = new Button { Content = imageView };
                ImagesGrid.Children.Add(imageButton);
                Grid.SetColumn(imageButton, i - 1);
            }
        }

        private static ImageSource BytesToImage(byte[] imageData)
        {
            var bitmapImage = new BitmapImage();
            var imageMemory = new MemoryStream(imageData);
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = imageMemory;
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
