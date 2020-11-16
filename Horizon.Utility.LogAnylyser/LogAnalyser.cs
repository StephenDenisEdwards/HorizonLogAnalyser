using System;
using System.Collections;
using System.Collections.Generic;

namespace Horizon.Utility.LogAnylyser
{
    public class LogAnalyser : IEnumerable<Dictionary<string, string>>, IDisposable
    {
        private List<Tuple<ILogFieldList, ILogStreamReader>> _logFiles;
        public LogAnalyser()
        {
            _logFiles = new List<Tuple<ILogFieldList, ILogStreamReader>>();
        }
        public LogAnalyser(LogFactory logFactory) : this()
        {
            Add(logFactory.Create());
        }
        public LogAnalyser(IEnumerable<LogFactory> logFactories) : this()
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
        public IEnumerator<Dictionary<string, string>> GetEnumerator()
        {
            foreach (Tuple<ILogFieldList, ILogStreamReader> logFile in GetLogFiles())
            {
                while (!logFile.Item2.EndOfStream)
                {
                    string line = logFile.Item2.ReadLine();

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
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
        ~LogAnalyser()
        {
            ReleaseUnmanagedResources();
        }
    }
}