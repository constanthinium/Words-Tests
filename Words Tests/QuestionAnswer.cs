using System;
using System.ComponentModel;
using System.Diagnostics;
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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            Debug.WriteLine("PropertyChanged: " + propertyName);
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
    }
}
