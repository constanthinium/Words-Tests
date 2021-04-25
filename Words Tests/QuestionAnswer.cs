using System.Xml.Serialization;

namespace Words_Tests
{
    public class QuestionAnswer
    {
        public QuestionAnswer()
        {
            
        }

        public QuestionAnswer(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }

        [XmlAttribute]
        public string Question { get; set; }

        [XmlAttribute]
        public string Answer { get; set; }
    }
}
