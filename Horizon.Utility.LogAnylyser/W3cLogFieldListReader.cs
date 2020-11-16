using System;
using System.Collections;

namespace Horizon.Utility.LogAnylyser
{
    public class W3cLogFieldListReader : ILogFieldList
    {
        private ILogStreamReader _logStreamReader;
        private ArrayList _fieldList = new ArrayList();

        public W3cLogFieldListReader(ILogStreamReader logStreamReader)
        {
            _logStreamReader = logStreamReader;

            bool found = false;

            while (!found && !_logStreamReader.EndOfStream)
            {
                string line = _logStreamReader.ReadLine();

                if (line.StartsWith("#Fields: "))
                {
                    found = true;
                    var splitLine = line.Split();

                    for (int i = 1; i < splitLine.Length; i++)
                    {
                        if(!string.IsNullOrEmpty(splitLine[i]))
                        {
                            _fieldList.Add(splitLine[i].Trim());
                        }
                    }
                }
            }

            if (!found)
            {
                throw new Exception("W3C #Fields directive not found.");
            }
        }
        public string this[int i] => (string) _fieldList[i];
        public string[] Values(string line)
        {
            return line.Split(' ');
        }
    }
}