using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MYVote;

namespace MyQuizBackend
{
    class Logger
    {
        #region LOGGER

        public static void writeToLogFile(string textToLog)
        {
            var pathToLogFile = Path.Combine(Startup._iHostingEnv.ContentRootPath, @"Logs\ExceptionLogFile.txt");

            if (File.Exists(pathToLogFile))
            {
                try
                {
                    if (deleteLogFileIfTooBig(pathToLogFile))
                    {
                        writeToLogFile("LOGGER: Logfile exceeded 20MB and got resettet.");
                    }
                    File.AppendAllText(pathToLogFile, Environment.NewLine + "-----NEW ENTRY-----" + Environment.NewLine + DateTime.Now + Environment.NewLine);
                    File.AppendAllText(pathToLogFile, textToLog);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                Console.WriteLine("LOGGER: Invalid Path to Logfile!");
            }
        }

        private static bool deleteLogFileIfTooBig(string pathToLogFile)
        {
            if (File.Exists(pathToLogFile) && new FileInfo(pathToLogFile).Length > 2e+7) //20 Megabyte
            {
                File.Delete(pathToLogFile);
                Console.WriteLine("LOGGER: Logfile exceeded 20MB and got resettet.");
                return true;
            }
            return false;
        }
        #endregion LOGGER
    }
}