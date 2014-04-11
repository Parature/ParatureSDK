using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ParaLogger
{
    internal static class TextLogManager
    {
        /// <summary>
        /// Provides the log file directory path
        /// </summary>
        static string LogFileDirectory()
        {

            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Remove(0, 8));
            if (path.Substring(path.Length - 3).ToLower() == "bin")
            {
                path = path.Substring(0, path.Length - 3);
            }
            path = path.Replace(@"\bin\Debug", "");
            path = path.Replace(@"\bin\Release", "");
            // 1/29 Added for Linux/MONO Support
            if (path.StartsWith("home"))
            {
                path = "/" + path;
                path = path.Replace("%20", " ");
            }

            return path + "/logs/";// +DateTime.UtcNow.Month.ToString() + "-" + DateTime.UtcNow.Year.ToString() + ".txt";

        }
        /// <summary>
        /// Provides the Log file path.
        /// </summary>        
        public static string LogFileAddress(String FileNameOverride)
        {
            if (LogManager.logginEnvironment == ParaLoggerEnums.LoggingEnvironment.Mono)
            {
                return DateTime.UtcNow.Month.ToString() + "-" + DateTime.UtcNow.Year.ToString() + ".txt";
            }
            else
            {
                if (string.IsNullOrEmpty(FileNameOverride) == false)
                {
                    return LogFileDirectory() + FileNameOverride + ".txt";
                }
                else
                {
                    //return LogFileDirectory() + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    return LogFileDirectory() + DateTime.Now.ToString("MM-dd-yyyy") + ".txt";
                }
            }

        }
        /// <summary>
        /// Will check if the file exceeded the size limit or not.
        /// </summary>        
        static void CheckLogFile(string FilePath)
        {
            if (System.IO.Directory.Exists(LogFileDirectory()) == false)
            {
                System.IO.Directory.CreateDirectory(LogFileDirectory());
            }
            if (System.IO.File.Exists(FilePath) == false)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(FilePath);
                sw.Close();
            }
        }

        public static void TextFileInsertLog(String logMessage, ParaLoggerEnums.EventType EventType, String FileNameOverride)
        {
            string FilePath = LogFileAddress(FileNameOverride);
            CheckLogFile(FilePath);
            using (StreamWriter logger = File.AppendText(FilePath))
            {
                DateTime now = DateTime.Now;
                logger.Write("[{0}-{1}]", DateTime.Now.ToString("MM/dd/yyyy-HH:mm:ss' GMT'z"), EventType.ToString());
                logger.WriteLine(":{0}", logMessage);
                logger.WriteLine("----------");
                logger.Flush();
                logger.Close();
            }
        }
    }
}
