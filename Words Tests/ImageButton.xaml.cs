using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Words_Tests
{
    public partial class ImageButton
    {
        public string Text { get; set; }

        public ImageSource Image { get; set; }

        public ImageButton()
        {
            InitializeComponent();
            DataContext = this;
        }
    }

    public class ImageButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dValue = (double)value;
            if (targetType == typeof(Thickness))
                return dValue / 2;
            if (targetType == typeof(double))
                return dValue * 1.5f;
            throw new ArgumentException("What are you doing");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
