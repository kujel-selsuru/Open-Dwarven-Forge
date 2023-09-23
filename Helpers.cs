//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Diagnostics;
using SDL2;
//using Microsoft.Xna.Framework;

namespace XenoLib
{
    /// <summary>
    /// Helper functions class
    /// </summary>
    public static class Helpers<T>
    {
        /// <summary>
        /// Returns a number divisible by probided scaler value less than provided value
        /// </summary>
        /// <param name="value">Value to reduce</param>
        /// <param name="scale">Scaling value</param>
        /// <returns>Int</returns>
        public static int forceDivisible(int value, double scale)
        {
            int temp = value;
            while(scale % value != 0)
            {
                value--;
            }
            return temp;
        }
        /// <summary>
        /// Tests if a position is in domain of specified ranges
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <returns>Boolean</returns>
        public static bool inDomain(int x, int y, int w, int h)
        {
            if (x < 0 || x > w)
            {
                return false;
            }
            if (y < 0 || y > h)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Gets a list of points generating a circle centered around a point
        /// on a data grid
        /// </summary>
        /// <param name="grid">DataGrid reference</param>
        /// <param name="center">Center of circle</param>
        /// <param name="radius">Radius of circle</param>
        /// <returns>List of Point2D objects</returns>
        public static List<Point2D> getCircleArea<T>(DataGrid<T> grid, Point2D center, float radius)
        {
            List<Point2D> points = new List<Point2D>();
            int width = (int)(radius * 2) + 1;
            int height = (int)(radius * 2) + 1;
            Point2D p = null;
            for (int x = (int)(center.X - radius); x < (int)(center.X - radius - 1) + width; x++)
            {
                for (int y = (int)(center.Y - radius); y < (int)(center.Y - radius - 1) + height; y++)
                {
                    if (inDomain(x, y, grid.Width, grid.Height) == true)
                    {
                        p = new Point2D(x, y);
                        if (insideCircle(center, p, radius) == true)
                        {
                            points.Add(p);
                        }
                    }
                }
            }
            //remove furthest left
            int tmp = 0;
            for(int i = 1; i < points.Count; i++)
            {
                if(points[i].X < points[tmp].X)
                {
                    tmp = i;
                }
            }
            points.RemoveAt(tmp);
            //remove furthest up
            tmp = 0;
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].Y < points[tmp].Y)
                {
                    tmp = i;
                }
            }
            points.RemoveAt(tmp);
            return points;
        }
        /// <summary>
        /// Aids getCircleArea
        /// </summary>
        /// <param name="center">Center of circle</param>
        /// <param name="p">Point to be checked</param>
        /// <param name="radius">Radius of circle</param>
        /// <returns>Boolean</returns>
        static bool insideCircle(Point2D center, Point2D p, float radius)
        {
            float dx = center.X - p.X;
            float dy = center.Y - p.Y;
            float dist = (dx * dx) + (dy * dy);
            if (dist <= radius * radius)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns a list of valid points in radius for x and y positions
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="radius">Radius around x, y position</param>
        /// <param name="curve">How shallow or sharp the curve is</param>
        /// <param name="w">Width of grid</param>
        /// <param name="h">Height of grid</param>
        /// <returns>List of Point2D objects</returns>
        public static List<Point2D> getAreaIn2DGrid(int x, int y, int radius, int curve, int w, int h)
        {
            List<Point2D> temp = new List<Point2D>();
            if (radius < 3)
            {
                radius = 3;
            }
            int tempW = curve + 1;
            //int tempShift = 1;
            int txs = x - curve;
            for (int ty = y - radius; ty < y * 2 + 1; ty++)
            {
                for (int tx = txs; tx < txs + tempW; tx++)
                {
                    if (inDomain(tx, ty, w, h))
                    {
                        temp.Add(new Point2D(tx, ty));
                    }
                }
                if (ty < y - 1)
                {
                    txs -= curve;
                    tempW += (curve * 2);
                }
                else if (ty > y + 1)
                {
                    txs += curve;
                    tempW -= (curve * 2);
                }
            }
            return temp;
        }
        /// <summary>
        /// Scans a DataGrid of type K objects for type T objects
        /// </summary>
        /// <typeparam name="T">Object type to scan for</typeparam>
        /// <typeparam name="K">Object type stored in DataGrid</typeparam>
        /// <param name="grid">DataGrid reference</param>
        /// <returns>List of Point2D objects</returns>
        public static List<Point2D> scanDataGrid<T, K>(DataGrid<K> grid)
        {
            List<Point2D> temp = new List<Point2D>();
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    if (grid.Grid[x, y] is T)
                    {
                        temp.Add(new Point2D(x, y));
                    }
                }
            }
            return temp;
        }
        /// <summary>
        /// Scales up a provided value by provided scaler
        /// </summary>
        /// <param name="value">Value to scale up</param>
        /// <param name="scale">Scaler value</param>
        /// <returns>Int</returns>
        public static int scaleUp(int value, double scale)
        {
            return (int)(value * scale);
        }
        /// <summary>
        /// Scales down a provided value by provided scaler
        /// </summary>
        /// <param name="value">Value to scale down</param>
        /// <param name="scale">Scaler value</param>
        /// <returns>Int</returns>
        public static int scaleDwon(int value, double scale)
        {
            return (int)(value / scale);
        }
        /// <summary>
        /// Calculates number of radians per slice provided
        /// </summary>
        /// <param name="numSlices">Number of slices</param>
        /// <returns>Double</returns>
        public static double radiansPerSlice(int numSlices)
        {
            return (2*Math.PI) / numSlices;
        }
        /// <summary>
        /// Calculate facing between two points
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>Facing8</returns>
        public static facing8 p1FaceP2(Point2D p1, Point2D p2)
        {
            float x = p1.X - p2.X;
            float y = p1.Y - p2.Y;
            double angle = Math.Atan(y / x);
            return (facing8)(int)(angle / ((2 * Math.PI) / 8));
        }
        /// <summary>
        /// Convert degrees to radians
        /// </summary>
        /// <param name="value">Degree value to convert</param>
        /// <returns>Double</returns>
        public static double degreesToRadians(double value)
        {
            return value * (PI / 180);
        }
        /// <summary>
        /// Convert radians to degrees
        /// </summary>
        /// <param name="value">Radian value to convert</param>
        /// <returns>Double</returns>
        public static double RadiansToDegrees(double value)
        {
            return value * (180 / PI);
        }
        /// <summary>
        /// Return an approximation of PI
        /// </summary>
        public static double PI
        {
            //3.14159265
            get { return Math.PI; }
        }
        /// <summary>
        /// Takes an angle in degrees and limits it to -360 to 360
        /// </summary>
        /// <param name="angle">Angle to limit</param>
        /// <returns>Double</returns>
        public static double limitRangeTo360(double angle)
        {
            double tmp = 0;
            if (angle > 360)
            {
                tmp = angle - 360;
                while(tmp > 360)
                {
                    tmp = tmp - 360;
                }
            }
            else if(angle < -360)
            {
                tmp = angle + 360;
                while (tmp < 360)
                {
                    tmp = tmp + 360;
                }
            }
            else
            {
                tmp = angle;
            }
            return tmp;
        }
        /// <summary>
        /// Calculate one position on x axis in specified direction
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="angle">Angle of movement</param>
        /// <param name="scaler">Scaling value ie: tile width</param>
        /// <returns>X value</returns>
        public static float getXValue(float x, double angle, int scaler)
        {
            return x + ((float)Math.Cos(degreesToRadians(angle)) * (float)scaler);
        }
        /// <summary>
        /// Calculate one position on x axis in specified direction
        /// </summary>
        /// <param name="y">Y position</param>
        /// <param name="angle">Angle of movement</param>
        /// <param name="scaler">Scaling value ie: tile width</param>
        /// <returns>Y value</returns>
        public static float getYValue(float y, double angle, int scaler)
        {
            return y + ((float)Math.Sin(degreesToRadians(angle)) * (float)scaler);
        }
        /// <summary>
        /// Extracts a file name minus extention and file path
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>String</returns>
        public static string extractFileName(string filePath)
        {
            char[] dot = {'.'};
            char[] slash = {'\\'};
            string[] strs = StringParser.parse(filePath, dot);
            string[] strs2 = StringParser.parse(strs[0], slash);
            string str = strs2[strs2.Length - 1];
            return str;
        }
        /// <summary>
        /// Extracts a file's extention
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>String</returns>
        public static string extractExtention(string filePath)
        {
            char[] dot = { '.' };
            string[] strs = StringParser.parse(filePath, dot);
            return strs[strs.Length - 1];
        }
        /// <summary>
        /// Returns a file name plus extention provided a full file path
        /// </summary>
        /// <param name="filePath">Path of file to extract proper name of</param>
        /// <returns>String</returns>
        public static string extractFileNameProper(string filePath)
        {
            char[] slash = { '\\' };
            string[] strs = StringParser.parse(filePath, slash);
            return strs[strs.Length - 1];
        }
        /// <summary>
        /// Returns names of all files containing the specified 
        /// extention in the format of ".extention" at specified 
        /// folder path
        /// </summary>
        /// <param name="folderPath">Path of folder to search</param>
        /// <param name="ext">Extention of files to return</param>
        /// <returns>List of files names minus extention</returns>
        public static List<string> getFilesWithExtention(string folderPath, string ext)
        {
            string[] tmp1 = System.IO.Directory.GetFiles(folderPath);
            string[] tmp2 = new string[tmp1.Length];
            string[] tmp3 = null;
            List<string> tmp4 = new List<string>();
            char[] slash = { '\\' };
            char[] dot = { '.' };
            for (int i = 0; i < tmp1.Length; i++)
            {
                tmp3 = StringParser.parse(tmp1[i], slash);
                tmp2[i] = tmp3[tmp3.Length - 1];
            }
            for (int i = 0; i < tmp2.Length; i++)
            {
                if(tmp2[i].Contains(ext) == true)
                {
                    tmp4.Add(StringParser.parse(tmp2[i], dot)[0]);
                }
            }
            return tmp4;
        }
        /// <summary>
        /// Checks if a string contains any numbers
        /// </summary>
        /// <param name="str">String to check</param>
        /// <returns>Boolean</returns>
        public static bool containsNumbers(string str)
        {
            for(int i = 0; i < str.Length; i++)
            {
                switch(str[i])
                {
                    case '0':
                        return true;
                    case '1':
                        return true;
                    case '2':
                        return true;
                    case '3':
                        return true;
                    case '4':
                        return true;
                    case '5':
                        return true;
                    case '6':
                        return true;
                    case '7':
                        return true;
                    case '8':
                        return true;
                    case '9':
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if a string contains only numbers
        /// </summary>
        /// <param name="str">String to check</param>
        /// <returns>Boolean</returns>
        public static bool containsOnlyNumbers(string str)
        {
            bool found = false;//found a number at position in string
            for (int i = 0; i < str.Length; i++)
            {
                found = false;//start of each new cycle set found to false
                switch (str[i])
                {
                    case '0':
                        found = true;
                        break;
                    case '1':
                        found = true;
                        break;
                    case '2':
                        found = true;
                        break;
                    case '3':
                        found = true;
                        break;
                    case '4':
                        found = true;
                        break;
                    case '5':
                        found = true;
                        break;
                    case '6':
                        found = true;
                        break;
                    case '7':
                        found = true;
                        break;
                    case '8':
                        found = true;
                        break;
                    case '9':
                        found = true;
                        break;
                }
                if(found == false)//A number was not found at postion
                {
                    return false;
                }
            }
            return true;//Pass through and only found numbers
        }
        /// <summary>
        /// Copys files from one folder to another folder (excluding directories)
        /// </summary>
        /// <param name="currentDir">File path of folder to copy from</param>
        /// <param name="targetDir">File path of folder to copy to</param>
        public static void copyFolder(string currentDir, string targetDir)
        {
            //copy all cell files from current dir to save dir
            string[] fileNames = Directory.GetFiles(currentDir);
            for (int f = 0; f < fileNames.Length; f++)
            {
                File.Copy(fileNames[f], targetDir + extractFileNameProper(fileNames[f]), true);
            }
        }
        /// <summary>
        /// Copys files from one folder to another folder (including directories)
        /// </summary>
        /// <param name="currentDir">File path of folder to copy from</param>
        /// <param name="targetDir">File path of folder to copy to</param>
        public static void copyFolder2(string currentDir, string targetDir)
        {
            //copy all cell files from current dir to save dir
            string[] fileNames = Directory.GetFiles(currentDir);
            string[] dirNames = Directory.GetDirectories(currentDir);
            fileNames = mergLists(dirNames, fileNames);
            if(fileNames.Length > 0)
            {
                string[] temp = new string[fileNames.Length - 1];
                for(int f = 0; f < fileNames.Length - 1; f++)
                {
                    temp[f] = fileNames[f];
                }
                fileNames = temp;
            }
            if(fileNames.Length > 0)
            {
                for(int f = 0; f < fileNames.Length - 1; f++)
                {
                    if(fileNames[f] != null)
                    {
                        //identify folders, make a new folder at destination and copy contents to target else just copy file
                        if(System.IO.Directory.Exists(fileNames[f]) == true)
                        {
                            System.IO.Directory.CreateDirectory(targetDir + "\\" + extractFileName(fileNames[f]));
                            copyFolder2(fileNames[f], targetDir + "\\" + extractFileName(fileNames[f]));
                        }
                        else
                        {
                            System.IO.File.Copy(fileNames[f], targetDir + "\\" + extractFileNameProper(fileNames[f]), true);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Copys files from one folder to another folder
        /// </summary>
        /// <param name="currentDir">File path of folder to copy from</param>
        /// <param name="targetDir">File path of folder to copy to</param>
        /// <param name="exclusions">Array of strings of files to exclude</param>
        public static void copyFolder(string currentDir, string targetDir, string[] exclusions)
        {
            //copy all cell files from current dir to save dir
            string[] fileNames = Directory.GetFiles(currentDir);
            string[] dirNames = Directory.GetDirectories(currentDir);
            if(dirNames.Length > 0)
            {
                fileNames = mergLists(dirNames, fileNames);
            }
            char[] slash = { '\\' };
            string[] tmp = null;
            //go through all names and remove the names that match the exclusions list
            int count = 0;
            if(fileNames.Length > 0)
            {
                for(int f = 0; f < fileNames.Length - 1; f++)
                {
                    if(fileNames[f] != null)
                    {
                        tmp = StringParser.parse(fileNames[f], slash);
                        for(int e = 0; e < exclusions.Length - 1; e++)
                        {
                            if(tmp[tmp.Length - 1] == exclusions[e])
                            {
                                for(int s = f; s < fileNames.Length - (count + 1); s++)
                                {
                                    fileNames[s] = fileNames[s + 1];
                                }
                                count++;
                            }
                        }
                    }
                }
            }
            if(fileNames.Length > 0)
            {
                string[] temp = new string[fileNames.Length - count];
                for(int f = 0; f < fileNames.Length - (count + 1); f++)
                {
                    temp[f] = fileNames[f];
                }
                fileNames = temp;
                for(int f = 0; f < fileNames.Length; f++)
                {
                    if(fileNames[f] != null)
                    {
                        //identify folders, make a new folder at destination and copy contents to target else just copy file
                        if(System.IO.Directory.Exists(fileNames[f]) == true)
                        {
                            System.IO.Directory.CreateDirectory(targetDir + "\\" + extractFileName(fileNames[f]));
                            copyFolder2(fileNames[f], targetDir + "\\" + extractFileName(fileNames[f]));
                        }
                        else
                        {
                            System.IO.File.Copy(fileNames[f], targetDir + "\\" + extractFileNameProper(fileNames[f]), true);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Takes two string arrays and returns a single merged one, starting with the first array and
        /// fallowed by second array
        /// </summary>
        /// <param name="list1">String array 1 reference</param>
        /// <param name="list2">String array 2 reference</param>
        /// <returns>String array</returns>
        public static string[] mergLists(string[] list1, string[] list2)
        {
            int num = list1.Length + list2.Length;
            int num2 = list1.Length;
            string[] temp = new string[num];
            for(int f = 0; f < list1.Length - 1; f++)
            {
                temp[f] = list1[f];
            }
            for(int f = 0; f < list2.Length - 1; f++)
            {
                temp[f + num2] = list2[f];
            }
            return temp;
        }
        /// <summary>
        /// Fast copys folder to folder
        /// </summary>
        /// <param name="SolutionDirectory">Folder to copy from</param>
        /// <param name="TargetDirectory">Folder to copy to</param>
        public static void fastCopyFolder(string SolutionDirectory, string TargetDirectory)
        {
            // Use ProcessStartInfo class

            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.CreateNoWindow = false;

            startInfo.UseShellExecute = false;

            //Give the name as Xcopy

            startInfo.FileName = "xcopy";

            //make the window Hidden

            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            //Send the Source and destination as Arguments to the process

            startInfo.Arguments = "\"" + SolutionDirectory + "\"" + " " + "\"" + TargetDirectory + "\"" + @" /e /y /I";

            try

            {
                // Start the process with the info we specified.

                // Call WaitForExit and then the using statement will close.

                using (Process exeProcess = Process.Start(startInfo))

                {
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception exp)

            {
                throw exp;
            }
        }
        /// <summary>
        /// Extracts graphics dimesions from properly formatted file names
        /// graphics file names ending in _width_height
        /// </summary>
        /// <param name="name">string value</param>
        /// <returns>Point2D</returns>
        public static Point2D extractSpriteSize(string name)
        {
            try
            {
                string[] temp;
                char[] dot = { '.' };
                temp = StringParser.parse(name, dot);
                char[] under = { '_' };
                temp = StringParser.parse(temp[0], under);
                return new Point2D(Convert.ToInt32(temp[1]), Convert.ToInt32(temp[2]));
            }
            catch (Exception)
            {

            }
            return new Point2D(0, 0);
        }
        /// <summary>
        /// Extracts a source image's TextureBank key from name
        /// </summary>
        /// <param name="name">Source graphic path</param>
        /// <returns>String</returns>
        public static string extractSourceKey(string name)
        {
            try
            {
                string[] temp;
                char[] dot = { '.' };
                temp = StringParser.parse(name, dot);
                char[] under = { '_' };
                temp = StringParser.parse(temp[0], under);
                char[] slash = { '\\' };
                string[] temp2 = StringParser.parse(temp[0], slash);
                return temp2[temp2.Length - 1];
            }
            catch(Exception)
            {

            }
            return "";
        }
        /// <summary>
        /// Strinks path by one folder each call
        /// </summary>
        /// <param name="path">File path as string</param>
        /// <returns>String</returns>
        public static string shrinkPathOne(string path)
        {
            char[] splitter = new char[] { '.' };
            char[] splitter2 = new char[] { '\\' };
            string name3 = StringParser.parse(path, splitter)[0];
            string[] name4 = StringParser.parse(name3, splitter2);
            string assetPath = "";
            int size = name4.Length - 1;
            if (name4[size] == "")
            {
                size--;
            }
            for (int i = 0; i < size; i++)
            {
                if (name4[i] != "")
                {
                    assetPath += name4[i] + "\\";
                }
            }
            return assetPath;
        }
        /// <summary>
        /// Adds data alphabetically by name (useable in any pairing of a list of strings and a list of T objects)
        /// </summary>
        /// /// <param name="names">List of strings reference</param>
        /// <param name="datas">List of T objects reference</param>
        /// <param name="name">Name value</param>
        /// <param name="data">T obj reference</param>
        public static void alphaAdd(List<string> names, List<T> datas, string name, T data)
        {
            int chars = 0;
            bool found = false;
            char[] space = { ' ' };
            string str1 = "";
            string str2 = "";
            string[] temp = StringParser.parse(name, space);
            for(int s = 0; s < temp.Length; s++)
            {
                str1 += temp[s];
            }
            for(int i = 0; i < names.Count; i++)
            {
                temp = StringParser.parse(names[i], space);
                for (int s = 0; s < temp.Length; s++)
                {
                    str2 += temp[s];
                }
                if (str1.Length < str2.Length)
                {
                    for(int c = 0; c < str1.Length; c++)
                    {
                        if (str1[chars] == space[0])
                        {
                            chars++;
                            c++;
                            if (c >= str1.Length)
                            {
                                break;
                            }
                        }
                        if (charGreater(str1[chars], str2[chars]) == true)
                        {
                            if(i < names.Count - 1)
                            {
                                i++;
                            }
                            break;
                        }
                        else
                        {
                            chars++;
                        }
                        if(charLesser(str1[chars], str2[chars]) == true)
                        {
                            found = true;
                            break;
                        }
                    }
                    if(found == true)
                    {
                        datas.Insert(i, data);
                        names.Insert(i, name);
                    }
                }
                else
                {
                    for(int c = 0; c < str2.Length; c++)
                    {
                        if (str1[chars] == space[0])
                        {
                            chars++;
                            c++;
                            if(c >= str2.Length)
                            {
                                break;
                            }
                        }
                        if (charGreater(str1[chars], str2[chars]) == true)
                        {
                            if(i < names.Count - 1)
                            {
                                i++;
                            }
                            break;
                        }
                        else
                        {
                            chars++;
                        }
                        if(charLesser(str1[chars], str2[chars]) == true)
                        {
                            found = true;
                        }
                    }
                    if(found == true)
                    {
                        datas.Insert(i, data);
                        names.Insert(i, name);
                    }
                }
            }
            if(found == false)
            {
                datas.Add(data);
                names.Add(name);
            }
        }
        /// <summary>
        /// Compares two chars and if a is lesser ASCII value than b return true else return false
        /// </summary>
        /// <param name="a">Char value</param>
        /// <param name="b">Char Value</param>
        /// <returns>Boolean</returns>
        private static bool charLesser(char a, char b)
        {
            int asc1, asc2;
            asc1 = Convert.ToInt32(a);
            asc2 = Convert.ToInt32(b);
            if (a < b)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Compares two chars and if a is greater ASCII value than b return true else return false
        /// </summary>
        /// <param name="a">Char value</param>
        /// <param name="b">Char Value</param>
        /// <returns>Boolean</returns>
        private static bool charGreater(char a, char b)
        {
            int asc1, asc2;
            asc1 = Convert.ToInt32(a);
            asc2 = Convert.ToInt32(b);
            if (a > b)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Automatically loads all png files ending in _widthValue_heightValue with magenta as the transparent color.
        /// Returns a list of source key values for calling graphics files from the TextureBank.
        /// </summary>
        /// <param name="srcPath">Source graphics folder path</param>
        /// <param name="renderer">Renderer reference</param>
        /// <returns>List of strings</returns>
        public static List<string> autoLoadGraphics(string srcPath, IntPtr renderer)
        {
            List<string> srcKeys = new List<string>();
            if(System.IO.Directory.Exists(srcPath) == true)
            {
                Point2D dimensions = new Point2D(0, 0);
                string[] fileNames = System.IO.Directory.GetFiles(srcPath);
                char[] dot = { '.' };
                string[] temp;
                string srcKey = "";
                for(int i = 0; i < fileNames.Length; i++)
                {
                    temp = StringParser.parse(fileNames[i], dot);
                    if(temp[temp.Length - 1] == "png")
                    {
                        try
                        {
                            dimensions = extractSpriteSize(fileNames[i]);
                            srcKey = extractSourceKey(fileNames[i]);
                            TextureBank.addTexture(srcKey, TextureLoader.autoLoad(fileNames[i], renderer));
                            srcKeys.Add(srcKey);
                        }
                        catch(Exception)
                        {

                        }
                    }
                }
            }
            return srcKeys;
        }
        /// <summary>
        /// Takes an angle -in degrees- and a speed as a float and returns a vector as a 
        /// Point2D object representing movement values in x and y axis
        /// </summary>
        /// <param name="angle">Direction of meovement in degrees</param>
        /// <param name="speed">Speed or magnitude</param>
        /// <returns>Point2D</returns>
        public static Point2D movementVector(double angle, float speed)
        {
            Point2D p = new Point2D(0, 0);
            p.X = (float)Math.Cos((double)degreesToRadians(angle)) * speed;
            p.Y = (float)Math.Sin((double)degreesToRadians(angle)) * speed;
            return p;
        }
        /// <summary>
        /// Returns a float of the x axis movement provided a direction of movement
        /// -in degrees- and a speed in float
        /// </summary>
        /// <param name="angle">Direction of movement in degrees</param>
        /// <param name="speed">Speed or magnitude</param>
        /// <returns>Float</returns>
        public static float movementX(double angle, float speed)
        {
            return (float)Math.Cos((double)degreesToRadians(angle)) * speed;
        }
        /// <summary>
        /// Returns a float of the y axis movement provided a direction of movement
        /// -in degrees- and a speed in float
        /// </summary>
        /// <param name="angle">Direction of movement in degrees</param>
        /// <param name="speed">Speed or magnitude</param>
        /// <returns>Float</returns>
        public static float movementY(double angle, float speed)
        {
            return (float)Math.Cos((double)degreesToRadians(angle)) * speed;
        }
        /// <summary>
        /// Takes a file name and file path and outputs a compressed file of specified extension
        /// </summary>
        /// <param name="filePathIn">File folder path to compress from</param>
        /// <param name="filePathOut">File folder path to comress to</param>
        /// <param name="fileName">File name of compressed/to be compressed file</param>
        /// <param name="ext">Input extension</param>
        /// <param name="outExt">Output extension</param>
        public static void compressData(string filePathIn, string filePathOut, string fileName, string ext = "txt", string outExt = "data")
        {
            //file to read in
            StreamReader sr = new StreamReader( filePathIn + fileName + "." + ext);
            //Part A: Write string to temporary file
            string temp = Path.GetTempFileName();
            File.WriteAllText(temp, sr.ReadToEnd());
            //Part B: Read file into byte array
            byte[] b;
            using(FileStream f = new FileStream(temp, FileMode.Open))
            {
                b = new byte[f.Length];
                f.Read(b, 0, (int)f.Length);
            }
            //Part C: Us GZipStream to write compressed bytes to target file
            using(FileStream f2 = new FileStream(filePathOut + fileName + "." + outExt, FileMode.Create))
            {

                using(GZipStream gz = new GZipStream(f2, CompressionMode.Compress, false))
                {
                    gz.Write(b, 0, b.Length);
                }
            }
        }
        /// <summary>
        /// Decompresses a compressed file an outputs as a text file
        /// </summary>
        /// <param name="filePathIn">File folder path of file to decompress</param>
        /// <param name="filePathOut">File folder path of output file</param>
        /// <param name="fileName">File name of decompressed/compressed file</param>
        /// <param name="ext">Input extension</param>
        /// <param name="outExt">Output extension</param>
        public static void decompressData(string filePathIn, string filePathOut, string fileName, string ext = "txt", string outExt = "data")
        {
            //read the data from copressed file
            string readData = "";
            GZipStream inStream = new GZipStream(File.OpenRead(filePathIn + fileName + "." + ext), CompressionMode.Decompress);
            StreamReader sr = new StreamReader(inStream);
            readData = sr.ReadToEnd();
            //now write decompressed data into an output file
            StreamWriter sw = new StreamWriter(filePathOut + fileName + "." + outExt);
            int stringIndex = 0;
            string str = "";
            char buffer = ' ';
            while(stringIndex < readData.Length - 1)
            {
                buffer = readData[stringIndex];
                if(buffer == '\n')
                {
                    sw.WriteLine(str);
                    str = "";
                }
                else
                {
                    str += buffer;
                }
            }

        }
    }
}
