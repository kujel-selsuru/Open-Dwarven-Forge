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
    /// BulletCache class (stores used bullet objects)
    /// </summary>
    /// <typeparam name="T">Data of type T objects</typeparam>
    public class BulletCache<T>
    {
        //protected
        protected List<T> data;

        //public
        /// <summary>
        /// BulletCache constructor
        /// </summary>
        public BulletCache()
        {
            data = new List<T>();
        }
        /// <summary>
        /// Push a object of type T into bullet cache
        /// </summary>
        /// <param name="data">Object of type T</param>
        public void push(T data)
        {
            this.data.Add(data);
        }
        /// <summary>
        /// Pop out the first object of type T
        /// </summary>
        /// <returns>Object of type T</returns>
        public T pop()
        {
            T temp = default(T);
            if (!IsEmpty)
            {
                temp = data[0];
                data.RemoveAt(0);
            }
            return temp;
        }
        /// <summary>
        /// Checks if cache is empty
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (data.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
