//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// GameLogger class
    /// </summary>
    public static class GameLogger
    {
        //public
        /// <summary>
        /// Writes a log of data to a specified file for later analysis
        /// </summary>
        /// <param name="path">File path of log file</param>
        /// <param name="logName">Name of log file</param>
        /// <param name="l1">Log 1</param>
        /// <param name="l2">Log 2</param>
        /// <param name="l3">Log 3</param>
        /// <param name="l4">Log 4</param>
        /// <param name="l5">Log 5</param>
        /// <param name="l6">Log 6</param>
        /// <param name="l7">Log 7</param>
        /// <param name="l8">Log 8</param>
        /// <param name="l9">Log 9</param>
        public static void writeLog(string path = "C:\\Game Logs\\", string logName = "log", string l1 = " ", string l2 = " ", string l3 = " ", string l4 = " ", string l5 = " ", string l6 = " ", string l7 = " ", string l8 = " ", string l9 = " ")
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            if (!System.IO.File.Exists(path + logName + ".txt"))
            {
                System.IO.StreamWriter s = new System.IO.StreamWriter(path + logName + ".txt");
                s.Close();
            }
            System.IO.StreamWriter sw = new System.IO.StreamWriter(path + logName + ".txt", true);
            if (l1 != " ")
            {
                sw.WriteLine(l1);
            }
            if (l2 != " ")
            {
                sw.WriteLine(l2);
            }
            if (l3 != " ")
            {
                sw.WriteLine(l3);
            }
            if (l4 != " ")
            {
                sw.WriteLine(l4);
            }
            if (l5 != " ")
            {
                sw.WriteLine(l5);
            }
            if (l6 != " ")
            {
                sw.WriteLine(l6);
            }
            if (l7 != " ")
            {
                sw.WriteLine(l7);
            }
            if (l8 != " ")
            {
                sw.WriteLine(l8);
            }
            if (l9 != " ")
            {
                sw.WriteLine(l9);
            }
            sw.Close();
        }
    }
}
