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
    /// Pocket class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pocket<T>
    {
        //private
        T[] stack;
        int max;
        int index;

        //public
        /// <summary>
        /// Pocket constructor
        /// </summary>
        /// <param name="max">Max number of items in pocket</param>
        public Pocket(int max = 5)
        {
            stack = new T[max];
            this.max = max;
            index = 0;
        }
        /// <summary>
        /// Peek at an item in pocket
        /// </summary>
        /// <param name="i">Item index</param>
        /// <returns>Object of type T</returns>
        public T peek(int i)
        {
            return stack[i];
        }
        /// <summary>
        /// Push an new item into pocket if there is space
        /// </summary>
        /// <param name="item">Item to push into pocket</param>
        /// <returns>Boolean</returns>
        public bool push(T item)
        {
            if (item == null)
            {
                return false;
            }
            if (index < max - 1)
            {
                if (index == 0)
                {
                    stack[0] = item;
                    index++;
                }
                else
                {
                    if (index == 1)
                    {
                        stack[1] = stack[0];
                    }
                    else
                    {
                        for (int i = index - 1; i > 0; i--)
                        {
                            stack[i + 1] = stack[i];
                        }
                    }
                    stack[0] = item;
                    index++;
                    /*
                    index++;
                    for (int i = index; i > 0; i--)
                    {
                        stack[i] = stack[i - 1];
                    }
                    stack[0] = item;
                     */ 
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Pop out the top item in pocket
        /// </summary>
        /// <returns></returns>
        public T pop()
        {
            if (index > 0)
            {
                T temp = stack[0];
                stack[0] = default(T);
                for (int i = 0; i < index; i++)
                {
                    stack[i] = stack[i + 1];
                }
                index--;
                return temp;
            }
            else
            {
                T temp = stack[0];
                stack[0] = default(T);
                return temp;
            }
            //return default(T);
        }
        /// <summary>
        /// Remove an item at index from pocket and return it
        /// </summary>
        /// <param name="i">Index of item</param>
        /// <returns>Object of type T</returns>
        public T remove(int i)
        {
            if (i < 0 | i > max - 1)
            {
                return default(T);
            }
            else
            {
                T temp = stack[i];
                if (i <= max - 1)
                {
                    for (int k = i; k < max - 1; k++)
                    {
                        if (k != max - 1)
                        {
                            stack[k] = stack[k + 1];
                        }
                        else
                        {
                            stack[k] = default(T);
                        }
                    }
                }
                index--;
                return temp;
            }
        }
        /// <summary>
        /// Count property
        /// </summary>
        public int Count
        {
            get { return index; }
        }
        /// <summary>
        /// Max property
        /// </summary>
        public int Max
        {
            get { return max; }
        }
    }
}
