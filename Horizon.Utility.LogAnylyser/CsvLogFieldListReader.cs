using System;
using System.Collections;

namespace Horizon.Utility.LogAnylyser
{
    public class CsvLogFieldListReader : ILogFieldList
    {
        private ILogStreamReader _logStreamReader;
        private ArrayList _fieldList = new ArrayList();

        public CsvLogFieldListReader(ILogStreamReader logStreamReader)
        {
            _logStreamReader = logStreamReader;

            if (_logStreamReader.EndOfStream)
            {
                throw new Exception("SCV file is empty.");
            }

            string line = _logStreamReader.ReadLine();

            var splitLine = line.Split(',');

            for (int i = 0; i < splitLine.Length; i++)
            {
                _fieldList.Add(splitLine[i].Trim());
            }
        }
        public string this[int i]
        {
            get { return (string)_fieldList[i]; }
        }
        public string[] Values(string line)
        {
            return line.Split(',');
        }
    }
}