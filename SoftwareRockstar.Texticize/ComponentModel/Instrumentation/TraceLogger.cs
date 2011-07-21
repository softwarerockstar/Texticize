using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;

namespace SoftwareRockstar.ComponentModel.Instrumentation
{   
    public class TraceLogger : ILogger
    {
        TraceSource _traceSource;

        internal TraceLogger(string traceSourceName) 
        {
            _traceSource = new TraceSource(traceSourceName);
        }

        public string LoggerName { get; internal set; }
        public string MethodName { get; internal set; }
        public IFormatProvider FormatProvider { get; internal set; }        

        public void LogError(string message)
        {
            if (_traceSource.Switch.ShouldTrace(TraceEventType.Error))
                CreateLog(message, TraceEventType.Error);
        }

        public void LogError(string format, params object[] args)
        {
            if (_traceSource.Switch.ShouldTrace(TraceEventType.Error))
                LogError(FormatMessage(format, args));
        }    

        public void LogError(Exception ex)
        {
            if (_traceSource.Switch.ShouldTrace(TraceEventType.Error))
                LogError(ex.ToString());
        }

        public void LogError(Func<FormatMessageHandler, string> formatCallback)
        {
            if (_traceSource.Switch.ShouldTrace(TraceEventType.Error))
            {
                var result = formatCallback(new FormatMessageHandler(FormatMessage));
                LogError(result);
            }
        }

        public void LogWarning(string message)
        {
            if (_traceSource.Switch.ShouldTrace(TraceEventType.Warning))
                CreateLog(message, TraceEventType.Warning);
        }

        public void LogWarning(string format, params object[] args)
        {
            if (_traceSource.Switch.ShouldTrace(TraceEventType.Warning))
                LogWarning(FormatMessage(format, args));
        }

        public void LogWarning(Func<FormatMessageHandler, string> formatCallback)
        {
            if (_traceSource.Switch.ShouldTrace(TraceEventType.Warning))
            {
                var result = formatCallback(new FormatMessageHandler(FormatMessage));
                LogWarning(result);
            }
        }

        public void LogInfo(string message)
        {
            if (_traceSource.Switch.ShouldTrace(TraceEventType.Information))
                CreateLog(message, TraceEventType.Information);
        }

        public void LogInfo(string format, params object[] args)
        {
            if (_traceSource.Switch.ShouldTrace(TraceEventType.Information))
                LogInfo(FormatMessage(format, args));
        }

        public void LogInfo(Func<FormatMessageHandler, string> formatCallback)
        {
            if (_traceSource.Switch.ShouldTrace(TraceEventType.Information))
            {
                var result = formatCallback(new FormatMessageHandler(FormatMessage));
                LogInfo(result);
            }
        }

        private string FormatMessage(string format, params object[] args)
        {
            return String.Format(FormatProvider, format, args);
        }

        private void CreateLog(string message, TraceEventType type)
        {
            if (_traceSource != null)
            {
                string toLog = FormatMessage(
                    "{0}\t{1}\t{2}{3}\n\t{4}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Thread.CurrentThread.ManagedThreadId,
                    this.LoggerName,
                    "::" + this.MethodName,
                    message);

                _traceSource.TraceEvent(type, 0, toLog);
                _traceSource.Flush();
            }             
        }
    }
}
