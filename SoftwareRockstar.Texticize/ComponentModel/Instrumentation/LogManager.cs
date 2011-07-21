using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;

namespace SoftwareRockstar.ComponentModel.Instrumentation
{
    public delegate string FormatMessageHandler(string format, params object[] args);

    public enum LogType
    {
        Error,
        Waring,
        Info
    }

    public static class LogManager
    {
        static Dictionary<string, TraceLogger> _cache = new Dictionary<string,TraceLogger>();

        public static ILogger GetLogger(string traceSourceName, string loggerName, string methodName = null, IFormatProvider formatProvider = null)
        {
            TraceLogger logger = null;

            if (_cache.ContainsKey(loggerName))
                logger = _cache[loggerName];
            else
            {
                logger = new TraceLogger(traceSourceName);
                logger.LoggerName = loggerName;
                _cache[loggerName] = logger;
            }
            
            logger.MethodName = methodName;
            logger.FormatProvider = formatProvider ?? CultureInfo.InvariantCulture;

            return logger;
        }
    }
}
