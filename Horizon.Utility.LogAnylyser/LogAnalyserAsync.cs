using System;
using System.Collections.Generic;
using System.Threading;

namespace Horizon.Utility.LogAnylyser
{
    public class LogAnalyserAsync : IAsyncEnumerable<Dictionary<string, string>>, IDisposable
    {
        private List<Tuple<ILogFieldList, ILogStreamReader>> _logFiles;
        public LogAnalyserAsync()
        {
            _logFiles = new List<Tuple<ILogFieldList, ILogStreamReader>>();
        }
        public LogAnalyserAsync(LogFactory logFactory) : this()
        {
            Add(logFactory.Create());
        }
        public LogAnalyserAsync(IEnumerable<LogFactory> logFactories) : this()
        {
            foreach (var logFactory in logFactories)
            {
                Add(logFactory.Create());
            }
        }
        protected IEnumerable<Tuple<ILogFieldList, ILogStreamReader>> GetLogFiles()
        {
            return _logFiles;
        }
        public async IAsyncEnumerator<Dictionary<string, string>> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (Tuple<ILogFieldList, ILogStreamReader> logFile in GetLogFiles())
            {
                while (!logFile.Item2.EndOfStream)
                {
                    string line = await logFile.Item2.ReadLineAsync();

                    var fieldValues = logFile.Item1.Values(line);

                    Dictionary<string, string> dict = new Dictionary<string, string>();

                    for (int i = 0; i < fieldValues.Length; i++)
                    {
                        dict.Add(logFile.Item1[i], fieldValues[i]);
                    }

                    yield return dict;
                }
            }
        }
        public void Add(Tuple<ILogFieldList, ILogStreamReader> tuple)
        {
            _logFiles.Add(tuple);
        }
        public void Add(ILogFieldList logFieldList, ILogStreamReader logStreamReader)
        {
            _logFiles.Add(new Tuple<ILogFieldList, ILogStreamReader>(logFieldList, logStreamReader));
        }
        private void ReleaseUnmanagedResources()
        {
            foreach (var item in _logFiles)
            {
                item.Item2.Dispose();
            }
        }
        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }
        ~LogAnalyserAsync()
        {
            ReleaseUnmanagedResources();
        }
    }
}