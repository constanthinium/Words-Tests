using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Words_Tests
{
    public partial class ImageButton
    {
        public string Text { get; set; }

        public ImageSource Source { get; set; }

        public ImageButton()
        {
            InitializeComponent();
            DataContext = this;
        }
    }

    public class ImageHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * 1.5;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
