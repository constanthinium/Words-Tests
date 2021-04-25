namespace Words_Tests
{
    internal class Pair
    {
        public Pair()
        {
            
        }

        public Pair(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }

        public string Question { get; set; }

        public string Answer { get; set; }
    }
}
