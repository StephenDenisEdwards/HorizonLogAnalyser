namespace Horizon.Utility.LogAnylyser
{
    public interface ILogFieldList
    {
        string this[int index] { get; }

        string[] Values(string line);
    }
}