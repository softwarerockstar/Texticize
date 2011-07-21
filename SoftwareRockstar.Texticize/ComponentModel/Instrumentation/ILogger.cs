using System;
namespace SoftwareRockstar.ComponentModel.Instrumentation
{     
    public interface ILogger
    {   
        //void LogDebug(Func<FormatMessageHandler, string> formatCallback);
        //void LogDebug(string format, params object[] args);
        //void LogDebug(string message);
        
        void LogError(Exception ex);
        void LogError(Func<FormatMessageHandler, string> formatCallback);
        void LogError(string format, params object[] args);
        void LogError(string message);
        
        void LogInfo(Func<FormatMessageHandler, string> formatCallback);
        void LogInfo(string format, params object[] args);
        void LogInfo(string message);
        
        void LogWarning(Func<FormatMessageHandler, string> formatCallback);
        void LogWarning(string format, params object[] args);
        void LogWarning(string message);

        IFormatProvider FormatProvider { get; }        
        string LoggerName { get; }
        string MethodName { get; }
    }
}
