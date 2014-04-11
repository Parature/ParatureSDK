using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
namespace ParaLogger
{

    public static class LogManager
    {
        private static Int32 phAccid = 4184;
        private static Int32 phDepid = 4634;
        private static string phToken = "5yzXXV@l5W/AWfw6ra2qOyT39KXL@fRBjKNL91xG79CGHkn8bsuQjp5fqMqa0MxHKDYB@nu@aZz0Tp4gfT@/VA==";
        private static ParaConnect.Paraenums.ServerFarm phServerFarm = ParaConnect.Paraenums.ServerFarm.SCO;

        public static ParaLoggerEnums.LoggingEnvironment logginEnvironment = ParaLoggerEnums.LoggingEnvironment.Windows;

        /// <summary>
        /// The Ticket number related to this project, for the Parature Phone Home feature.
        /// </summary>
        public static Int64 phTicketNumber = 0;

        private static Int64 phActionid = 41779;

        public static void LogEvent(string Message, ParaLoggerEnums.EventType EventType, ParaLoggerEnums.LogMode LogMode)
        {
            try
            {

                switch (LogMode)
                {
                    case ParaLoggerEnums.LogMode.Email:

                        break;
                    case ParaLoggerEnums.LogMode.TextFile:
                        //urrent.Server.MapPath("log\\foo.xml");
                        TextLogManager.TextFileInsertLog(Message, EventType, "");
                        break;
                }

            }

            catch (Exception ex)
            {
                // need to do something here.
            }
        }

        public static void LogEventToFile(string message)
        {
            LogEvent(message, ParaLoggerEnums.EventType.Event, ParaLoggerEnums.LogMode.TextFile);
        }

        public static void LogWarningToFile(string message)
        {
            LogEvent(message, ParaLoggerEnums.EventType.Warning, ParaLoggerEnums.LogMode.TextFile);
        }

        public static void LogExceptionToFile(string message)
        {
            LogEvent(message, ParaLoggerEnums.EventType.Exception, ParaLoggerEnums.LogMode.TextFile);
        }

        public static void LogExceptionToFile(string message, string FileName)
        {
            LogEvent(message, ParaLoggerEnums.EventType.Exception, FileName);
        }

        /// <summary>
        /// Log an event to a text file with a name of your choice.
        /// </summary>
        /// <param name="Message">
        /// The message to log.
        /// </param>        
        /// <param name="FileName">
        ///  The name of the log file, without extension. Just the file name.
        /// </param>
        public static void LogEvent(string Message, ParaLoggerEnums.EventType EventType, string FileName)
        {
            try
            {

                TextLogManager.TextFileInsertLog(Message, EventType, FileName);
            }

            catch (Exception ex)
            {
                // need to do something here.
            }
        }


        /// <summary>
        /// Allows to post an internal comment action to the appropriate ticket for this project.
        /// </summary>
        public static void PhoneHome(string message)
        {
            if (phTicketNumber > 0)
            {
                try
                {
                    ParaConnect.ParaCredentials pc = new ParaConnect.ParaCredentials(phToken, phServerFarm, ParaConnect.Paraenums.ApiVersion.v1, phAccid, phDepid, "'1J2*Ll~+?uuE*^e43tFGWf%|t#QD", "6b5,DK!cmw,u6`=iLl-`FP.Tcf+/F");
                    ParaConnect.ParaObjects.Action action = new ParaConnect.ParaObjects.Action();
                    action.ActionID = phActionid;
                    action.Comment = message;
                    action.VisibleToCustomer = false;
                    ParaConnect.ApiHandler.Ticket.ActionRun(phTicketNumber, action, pc);
                }
                catch (Exception e)
                {

                }
            }
        }
        public static string getLogPath(string FileName)
        {
            return TextLogManager.LogFileAddress(FileName);
        }

    }
}
