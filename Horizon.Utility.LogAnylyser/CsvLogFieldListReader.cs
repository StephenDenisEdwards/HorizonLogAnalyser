using System;
using System.Collections;

namespace Horizon.Utility.LogAnylyser
{
    public class CsvLogFieldListReader : ILogFieldList
    {
        private readonly ArrayList _fieldList = new ArrayList();
        private readonly ILogStreamReader _logStreamReader;

        public CsvLogFieldListReader(ILogStreamReader logStreamReader)
        {
            _logStreamReader = logStreamReader ?? throw new ArgumentNullException(nameof(logStreamReader));

            if (_logStreamReader.EndOfStream) throw new Exception("SCV file is empty.");

            var line = _logStreamReader.ReadLine();

            var splitLine = line.Split(',');

            for (var i = 0; i < splitLine.Length; i++) _fieldList.Add(splitLine[i].Trim());
        }

        public string this[int i] => (string) _fieldList[i];

        public string[] Values(string line)
        {
            return line.Split(',');
        }
    }
}