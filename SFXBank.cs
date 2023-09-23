//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// SFXBank class
    /// </summary>
    public static class SFXBank
    {
        //private
        static Dictionary<string, string> sfxs;

        //public
        /// <summary>
        /// SFXBank constructor
        /// </summary>
        static SFXBank()
        {
            sfxs = new Dictionary<string, string>();
        }
        /// <summary>
        /// Names property
        /// </summary>
        public static List<string> Names
        {
            get{ return sfxs.Keys.ToList<string>(); }
        }
        /// <summary>
        /// Count property
        /// </summary>
        public static int Count
        {
            get { return sfxs.Keys.Count; }
        }
        /// <summary>
        /// Returns a SFX filePath or "" if none found
        /// </summary>
        /// <param name="name">Name/key of SFX</param>
        /// <returns>string</returns>
        public static string getSFX(string name)
        {
            string value;
            if(sfxs.TryGetValue(name, out value))
            {
                return value;
            }
            return "";
        }
        /// <summary>
        /// Adds a SFX filePath to SFXBank 
        /// </summary>
        /// <param name="name">Name/key</param>
        /// <param name="filePath">SFX filePath</param>
        public static void addSFX(string name, string filePath)
        {
            sfxs.Add(name, filePath);
        }
        /// <summary>
        /// Returns a list of all keys/names in SFXBank
        /// </summary>
        /// <returns>List of strings</returns>
        public static List<string> getKeys()
        {
            return sfxs.Keys.ToList();
        }
        /// <summary>
        /// Checks for a provided key in sfxbank returns true if found else false
        /// </summary>
        /// <param name="key">Key value to serach</param>
        /// <returns>Boolean</returns>
        public static bool containsKey(string key)
        {
            if(sfxs.Keys.Contains(key) == true)
            {
                return true;
            }
            return false;
        }
    }
}
