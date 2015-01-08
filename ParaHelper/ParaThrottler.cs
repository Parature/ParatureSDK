using System;
using System.IO;
using System.Reflection;

namespace ParatureAPI.ParaHelper
{
    internal class ParaThrottler
    {


        /// <summary>
        /// Returns the path of the application
        /// </summary>
        static string GetAppPath()
        {
            // Returns the application path...
            System.Reflection.Module[] modules = Assembly.GetExecutingAssembly().GetModules();

            string aPath = Path.GetDirectoryName(modules[0].FullyQualifiedName);

            if ((aPath != "") && (aPath[aPath.Length - 1] != '\\'))
            {
                aPath += '\\';
            }
            return aPath;
        }
        static String LoadLastAccessed()
        {
            if (File.Exists(StampFilePath()) == false)
            {
                SaveLastAccessed(ToUtcTime(DateTime.UtcNow));
            }
            System.IO.StreamReader objStreamReader = new System.IO.StreamReader(StampFilePath());
            //Now, read the entire file into a string
            String contents = objStreamReader.ReadToEnd();
            objStreamReader.Close();
            File.Delete(StampFilePath());
            return contents;
            //return DateTime.Parse(contents);
        }
        static void SaveLastAccessed(string stamp)
        {
            if (File.Exists(StampFilePath()) == true)
            {
                File.Delete(StampFilePath());
            }
            System.IO.StreamWriter objStreamWriter = new System.IO.StreamWriter(StampFilePath());
            objStreamWriter.WriteLine(stamp);
            objStreamWriter.Close();
        }
        private static string StampFilePath()
        {
            return GetAppPath() + "config\\thrott.txt";
        }
        /// <summary>
        /// To Zulu (UTC) time format.
        /// </summary>
        /// <param name="date"> The datetime.</param>
        /// <returns> Returns the datetime in zulu string format.</returns>
        private static string ToUtcTime(DateTime date)
        {
            //return date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            return date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

    }
}