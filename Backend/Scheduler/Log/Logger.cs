using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Log
{
    public enum LogLevel
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }

    public static class Logger
    {
        private static readonly NLog.ILogger _instance;

        static Logger()
        {
            _instance = NLog.LogManager.GetLogger("MAIN_LOGGER");
        }

        public static void Log(string message, LogLevel level = LogLevel.Trace)
        {
            _instance.Log(level.ConvertToNLog(), message);
        }

        public static void Log(Exception exception, LogLevel level = LogLevel.Error, string message = "")
        {
            _instance.Log(level.ConvertToNLog(), exception, message);
        }
    }

    static class LogLevelExtension
    {
        public static NLog.LogLevel ConvertToNLog(this LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace:
                    return NLog.LogLevel.Trace;
                case LogLevel.Debug:
                    return NLog.LogLevel.Debug;
                case LogLevel.Info:
                    return NLog.LogLevel.Info;
                case LogLevel.Warn:
                    return NLog.LogLevel.Warn;
                case LogLevel.Error:
                    return NLog.LogLevel.Error;
                case LogLevel.Fatal:
                    return NLog.LogLevel.Fatal;
                default:
                    return NLog.LogLevel.Trace;
            }
        }
    }
}
