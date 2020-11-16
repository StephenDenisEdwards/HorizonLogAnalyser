using System;
using System.Threading.Tasks;

namespace Horizon.Utility.LogAnylyser
{
    public interface ILogStreamReader : IDisposable
    {
        public string? ReadLine();
        public Task<string?> ReadLineAsync();
        public bool EndOfStream { get; }
    }
}