//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XenoLib
{
    /// <summary>
    /// A generic bank to store and retrive data types, by defualt 600 data objects can be stored. 
    /// Warning with more objects stored the time required to retrive the desired object goes up!
    /// </summary>
    public class GenericBank<T>
    {
        //protected
        protected List<string> names;
        protected List<T> datas;
        protected int index;
        protected int max;

        //public
        /// <summary>
        /// Generic Bank constructor
        /// </summary>
        public GenericBank(int max = 6000)
        {
            names = new List<string>();
            datas = new List<T>();
            index = 0;
            this.max = max;
        }
        /// <summary>
        /// Add a data item to bank
        /// </summary>
        /// <param name="name">Data item key</param>
        /// <param name="data">Data item object reference</param>
        /// <returns>Boolean</returns>
        public bool addData(string name, T data)
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (names[i] == name)
                {
                    return false;
                }
            }
            if (index < max)
            {
                names.Add(name);
                datas.Add(data);
                index++;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Adds data alphabetically by name
        /// </summary>
        /// <param name="name">Name value</param>
        /// <param name="data">T obj reference</param>
        public void alphaAdd(string name, T data)
        {
            Helpers<T>.alphaAdd(names, datas, name, data);
        }
        /// <summary>
        /// Get a data item provided a key/name
        /// </summary>
        /// <param name="name">Key/name</param>
        /// <returns>T object reference</returns>
        public T getData(string name)
        {
            for (int i = 0; i < index; i++)
            {
                if (names[i] == name)
                {
                    return datas[i];
                }
            }
            return default(T);
        }
        /// <summary>
        /// Get a data item provided an index
        /// </summary>
        /// <param name="i">index of item</param>
        /// <returns>T object reference</returns>
        public T getData(int i)
        {
            if(i >= 0 && i < index)
            {
                return datas[i];
            }
            return default(T);
        }
        /// <summary>
        /// Removes a data item from list
        /// </summary>
        /// <param name="name">Key/name</param>
        /// <returns>Boolean</returns>
        public bool removeData(string name)
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (names[i] == name)
                {
                    for (int k = i; i < names.Count; k++)
                    {
                        names[k - 1] = names[k];
                        datas[k - 1] = datas[k];
                    }
                    index--;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Replaces data with name, returns true if 
        /// successful else returns false
        /// </summary>
        /// <param name="name">name of data to replace</param>
        /// <param name="data">Data to add</param>
        /// <returns>Boolean</returns>
        public bool replaceData(string name, T data)
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (names[i] == name)
                {
                    datas[i] = data;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Replaces data at index, returns true if 
        /// successful else returns false
        /// </summary>
        /// <param name="index">Index value</param>
        /// <param name="data">Data to add</param>
        /// <returns>Boolean</returns>
        public bool replaceData(int index, T data)
        {
            if (index >= 0 && index < datas.Count - 1)
            {
                datas[index] = data;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Removes a data item from list
        /// </summary>
        /// <param name="i">index of item</param>
        /// <returns>Boolean</returns>
        public bool removeData(int i)
        {
            if (i < index)
            {
                for (int k = i; k < index; k++)
                {
                    names[k] = names[k + 1];
                    datas[k] = datas[k + 1];
                }
                index--;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Names property
        /// </summary>
        public List<string> Names
        {
            get { return names; }
        }
        /// <summary>
        /// index provides the the index value of the next data space to be added
        /// </summary>
        public int Index
        {
            get { return index; }
        }
        /// <summary>
        /// Max property
        /// </summary>
        public int Max
        {
            get { return max; }
            set { max = value; }
        }
        /// <summary>
        /// Access an item at index location in bank
        /// </summary>
        /// <param name="i">Item index value</param>
        /// <returns>T object reference</returns>
        public T at(int i) 
        {
            if (i >= 0 && i < index)
            {
                return datas[i];
            }
            else
            {
                return default(T);
            }
        }
        /// <summary>
        /// Checks if GenericBank contains key
        /// </summary>
        /// <param name="key">Name/Key of value</param>
        /// <returns>Boolean</returns>
        public bool containsKey(string key)
        {
            for(int i = 0; i < names.Count; i++)
            {
                if(names[i] == key)
                {
                    return true;
                }
            }
            return false;
        }
    }
    /// <summary>
    /// Variable size and global GenericBank class
    /// </summary>
    /// <typeparam name="T">T object reference</typeparam>
    public static class GBank<T>
    {
        //private
        private static Dictionary<String, T> bank;

        /// <summary>
        /// GBank constructor
        /// </summary>
        static GBank()
        {
            bank = new Dictionary<string, T>();
        }
        /// <summary>
        /// Adds a data type T object to bank
        /// </summary>
        /// <param name="name">Key/name</param>
        /// <param name="data">Data T object reference</param>
        /// <returns>Boolean</returns>
        public static bool addData(string name, T data)
        {
            if (bank.ContainsKey(name))
            {
                return false;
            }
            else
            {
                bank.Add(name, data);
                return true;
            }
        }
        /// <summary>
        /// Returns a data T object reference provided a name
        /// </summary>
        /// <param name="name">Key/name</param>
        /// <returns>T object reference</returns>
        public static T getData(string name)
        {
            T temp;
            bank.TryGetValue(name, out temp);
            return temp;
        }
        /// <summary>
        /// Checks if GBank contians key/name provided
        /// </summary>
        /// <param name="name">Key/name</param>
        /// <returns>Boolean</returns>
        public static bool contains(string name)
        {
            return bank.Keys.Contains(name);
        }
    }
}
