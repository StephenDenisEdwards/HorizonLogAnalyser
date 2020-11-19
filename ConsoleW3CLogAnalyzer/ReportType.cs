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

    public record Person
    {
    public string LastName { get; }
    public string FirstName { get; }

    public Person(string first, string last) => (FirstName, LastName) = (first, last);
    }
}