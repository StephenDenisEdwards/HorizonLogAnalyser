using System;
using System.IO;

namespace Horizon.Utility.LogAnylyser
{
    public class CsvLogFileFactory : LogFactory
    {
        private readonly string _logFilePathName;

        public CsvLogFileFactory(string logFilePathName)
        {
            _logFilePathName = logFilePathName ?? throw new ArgumentNullException(nameof(logFilePathName));
        }

        public override Tuple<ILogFieldList, ILogStreamReader> Create()
        {
            var streamReader = new StreamReader(_logFilePathName);
            ILogStreamReader logStreamReader = new TextLogStreamReader(streamReader);
            ILogFieldList fieldList = new CsvLogFieldListReader(logStreamReader);
            return new Tuple<ILogFieldList, ILogStreamReader>(fieldList, logStreamReader);
        }
    }
}