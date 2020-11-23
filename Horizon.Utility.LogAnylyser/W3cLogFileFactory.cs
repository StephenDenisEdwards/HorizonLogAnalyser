using System;
using System.IO;

namespace Horizon.Utility.LogAnylyser
{
    public class W3cLogFileFactory : LogFactory
    {
        private readonly string _logFilePathName;

        public W3cLogFileFactory(string logFilePathName)
        {
            _logFilePathName = logFilePathName ?? throw new ArgumentNullException(nameof(logFilePathName));
        }

        public override Tuple<ILogFieldList, ILogStreamReader> Create()
        {
            var streamReader = new StreamReader(_logFilePathName);
            ILogStreamReader logStreamReader = new TextLogStreamReader(streamReader);
            ILogFieldList fieldList = new W3cLogFieldListReader(logStreamReader);
            return new Tuple<ILogFieldList, ILogStreamReader>(fieldList, logStreamReader);
        }
    }
}