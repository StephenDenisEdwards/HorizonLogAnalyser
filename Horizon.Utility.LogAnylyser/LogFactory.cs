using System;

namespace Horizon.Utility.LogAnylyser
{
    public abstract class LogFactory
    {
        public abstract Tuple<ILogFieldList, ILogStreamReader> Create();
    }
}