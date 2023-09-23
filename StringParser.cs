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
    /// StringParser class
    /// </summary>
    public static class StringParser
    {
        /// <summary>
        /// Splits strings up by white spaces
        /// </summary>
        /// <param name="str">String to split up</param>
        /// <returns>String array</returns>
        public static string[] parse(string str)
        {
            return str.Split(' ');
        }
        /// <summary>
        /// Splits strings by char value 
        /// </summary>
        /// <param name="str">String to split up</param>
        /// <param name="ident">Char value to split by</param>
        /// <returns>String arrary</returns>
        public static string[] parse(string str, char[] ident)
        {
            return str.Split(ident);
        }
        /// <summary>
        /// Spits string two char values, first by the ident1 and than splitting the substrings by indet2
        /// </summary>
        /// <param name="str">String to split up</param>
        /// <param name="ident1">Char value to split by first</param>
        /// <param name="ident2">Char value to split by second</param>
        /// <returns>2D string arrary</returns>
        public static string[][] doubleParse(string str, char[] ident1, char[] ident2)
        {
            string[] temp = parse(str, ident1);
            string[][] temp2 = new string[temp.Length][];
            for (int i = 0; i < temp.Length; i++)
            {
                temp2[i] = parse(temp[i], ident2);
            }
            return temp2;
        }
        /// <summary>
        /// Splits up string by char value and returns only the sub string at index i
        /// </summary>
        /// <param name="str">String to split up</param>
        /// <param name="ident">Char value to split by</param>
        /// <param name="i">Index of sub string to return</param>
        /// <returns>String</returns>
        public static string parseIndex(string str, char[] ident, int i)
        {
            string[] temp = parse(str, ident);
            return temp[i];
        }
    }
}
