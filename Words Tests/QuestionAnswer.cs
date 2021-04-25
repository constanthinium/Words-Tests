using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Words_Tests
{
    public class QuestionAnswer : INotifyPropertyChanged
    {
        private string _question;
        private string _answer;

        public QuestionAnswer()
        {
            
        }

        public QuestionAnswer(string question, string answer)
        {
            _question = question;
            _answer = answer;
        }

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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Debug.WriteLine("PropertyChanged: " + propertyName);
        }
    }
}
