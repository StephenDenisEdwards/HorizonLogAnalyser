using System.IO;
using System.Threading.Tasks;

namespace Horizon.Utility.LogAnylyser
{
    public class TextLogStreamReader : ILogStreamReader
    {
        private StreamReader _streamReader;

        public TextLogStreamReader(StreamReader streamReader)
        {
            _streamReader = streamReader;
        }

        public void Dispose()
        {
            _streamReader.Dispose();
        }

        public string? ReadLine()
        {
            return _streamReader.ReadLine();
        }

        public Task<string?> ReadLineAsync()
        {
            return _streamReader.ReadLineAsync();
        }

        public bool EndOfStream => _streamReader.EndOfStream;
    }
}