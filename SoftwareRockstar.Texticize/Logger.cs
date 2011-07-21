using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareRockstar.ComponentModel.Instrumentation;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace SoftwareRockstar.Texticize
{
    public static class Logger
    {
        public static void LogMethodStartup()
        {
            var logger = GetLogger();
            logger.LogInfo("********** Start Method {0}() **********", logger.MethodName);
        }

        public static void LogMethodEnd()
        {
            var logger = GetLogger();
            logger.LogInfo("********** End Method {0}() **********", logger.MethodName);            
        }

        public static void LogInfo(string message)
        {
            var logger = GetLogger();
            logger.LogInfo(message);            
        }

        public static void LogInfo(Func<FormatMessageHandler, string> formatCallback)
        {
            var logger = GetLogger();
            logger.LogInfo(formatCallback);            
        }

        public static void LogInfo(string format, params object[] args)
        {
            var logger = GetLogger();
            logger.LogInfo(format, args);
        }

        public static void LogWarning(string message)
        {
            var logger = GetLogger();
            logger.LogWarning(message);
        }

        public static void LogWarning(Func<FormatMessageHandler, string> formatCallback)
        {
            var logger = GetLogger();
            logger.LogWarning(formatCallback);
        }

        public static void LogWarning(string format, params object[] args)
        {
            var logger = GetLogger();
            logger.LogWarning(format, args);
        }

        public static void LogError(string message)
        {
            var logger = GetLogger();
            logger.LogError(message);
        }

        public static void LogError(Func<FormatMessageHandler, string> formatCallback)
        {
            var logger = GetLogger();
            logger.LogError(formatCallback);
        }

        public static void LogError(string format, params object[] args)
        {
            var logger = GetLogger();
            logger.LogError(format, args);
        }

        public static void LogError(Exception ex)
        {
            var logger = GetLogger();
            logger.LogError(ex);
        }

        private static ILogger GetLogger()
        {
            var callingMethod = new StackTrace().GetFrame(2).GetMethod();

            return LogManager.GetLogger(
                "Texticize",
                callingMethod.DeclaringType.FullName,
                callingMethod.Name,
                CultureInfo.InvariantCulture);
        }
    }
}
