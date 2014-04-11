using System;
using System.Collections.Generic;
using System.Text;

namespace ParaLogger
{
    public static class ParaLoggerEnums
    {
        public enum LogMode
        {
            TextFile,
            Email//,
            //DataBase,
            //ParatureApis,
            
        }
        public enum EventType
        {
            Event=1,
            Warning=2,
            Exception=3
        }

        /// <summary>
        /// Indicates whether this is a mono or Windows environment, to help determine the way log files are saved.
        /// </summary>
        public enum LoggingEnvironment
        {
            Windows=1,
            Mono=2
        }
    }
}
