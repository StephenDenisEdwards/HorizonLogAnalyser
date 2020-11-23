using System;
using System.IO;
using System.Threading.Tasks;

namespace Horizon.Utility.LogAnylyser
{
    public class TextLogStreamReader : ILogStreamReader
    {
        private readonly StreamReader _streamReader;

        public TextLogStreamReader(StreamReader streamReader)
        {
            _streamReader = streamReader ?? throw new ArgumentNullException(nameof(streamReader));
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