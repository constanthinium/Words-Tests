using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Words_Tests
{
    public class QuestionAnswer : INotifyPropertyChanged
    {
        private string _question;
        private string _answer;
        private byte[] _image;

        public event PropertyChangedEventHandler PropertyChanged;

        [XmlAttribute]
        public string Question
        {
            get => _question;
            set
            {
                _question = value; 
                OnPropertyChanged("Question");
            }
        }

        [XmlAttribute]
        public string Answer
        {
            get => _answer;
            set
            {
                _answer = value; 
                OnPropertyChanged("Answer");
            }
        }

        public byte[] Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged("Image");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetImageBytesFromBitmapSource(ImageSource imageSource)
        {
            var bitmapSource = (BitmapSource)imageSource;
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            var stream = new MemoryStream();
            encoder.Save(stream);
            Image = stream.ToArray();
        }

        public ImageSource GetImageSourceOrReturnNull()
        {
            if (_image == null) return null;
            var image = new BitmapImage();
            var stream = new MemoryStream(_image);
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }
    }
}
