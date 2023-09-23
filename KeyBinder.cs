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
    /// KeyBinder class
    /// </summary>
    public class KeyBinder
    {
        //protected
        protected inputState[] keys;

        //public
        /// <summary>
        /// KeyBinder constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public KeyBinder(StreamReader sr = null)
        {
            keys = new inputState[22];
            if (sr == null)
            {
                for (int i = 0; i < 22; i++)
                {
                    keys[i] = (inputState)i;
                }
            }
            else
            {
                for (int i = 0; i < 22; i++)
                {
                    keys[i] = (inputState)Convert.ToInt16(sr.ReadLine());
                }
            }
        }
        /// <summary>
        /// Process a key input
        /// </summary>
        /// <param name="key">InputState to process</param>
        /// <returns>InputState</returns>
        public inputState processKey(inputState key)
        {
            if (key != inputState.none)
            {
                return keys[(int)key];
            }
            else
            {
                return inputState.none;
            }
        }
        /// <summary>
        /// Swap two inputs
        /// </summary>
        /// <param name="alpha">InputState Alpha</param>
        /// <param name="beta">InputState Beta</param>
        public void swap(inputState alpha, inputState beta)
        {
            inputState temp = alpha;
            alpha = beta;
            beta = temp;
        }
        /// <summary>
        /// Swap two key bindings in internal arrarys
        /// </summary>
        /// <param name="alpha">InputState index Alpha</param>
        /// <param name="beta">InputState index Beta</param>
        public void swap(int alpha, int beta)
        {
            inputState temp = keys[alpha];
            keys[alpha] = keys[beta];
            keys[beta] = temp;
        }
        /// <summary>
        /// Save inputState bindings to file
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveBindings(StreamWriter sw)
        {
            for (int i = 0; i < 20; i++)
            {
                sw.WriteLine(keys[i].ToString());
            }
        }
        /// <summary>
        /// Keys property
        /// </summary>
        public inputState[] Keys
        {
            get { return keys; }
        }
    }
}
