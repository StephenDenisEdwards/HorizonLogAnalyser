namespace ConsoleW3CLogAnalyzer
{
    public class ReportType
    {
        public string Value { get; set; }
        public int Rank { get; set; }
        public int Hits { get; set; }

        public override string ToString()
        {
            return $"Value:{Value}, Rank:{Rank}, Hits:{Hits}";
        }
    }
}