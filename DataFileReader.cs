//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// DataFileReader, reads file line by line and outputs to console window
    /// </summary>
    public static class DataFileReader
    {
        /// <summary>
        /// Reads file provided a path/fileName
        /// </summary>
        /// <param name="fileName"></param>
        public static void readFileData(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            while(!sr.EndOfStream)
            {
                Console.WriteLine(sr.ReadLine());
            }
            sr.Close();
        }
    }
}
