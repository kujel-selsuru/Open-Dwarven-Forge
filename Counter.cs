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
    /// Counter class
    /// </summary>
    public class Counter
    {
        //protected
        protected int count;
        protected int max;

        //public
        /// <summary>
        /// Counter constructor
        /// </summary>
        /// <param name="max">Max number of ticks</param>
        public Counter(int max = 100)
        {
            count = 0;
            this.max = max;
        }
        /// <summary>
        /// Tick up by one and return true if max while resetting to zero else return false
        /// </summary>
        /// <returns>Boolean</returns>
        public bool tick()
        {
            count++;
            if (count < max)
            {
                return false;
            }
            else
            {
                count = 0;
                return true;
            }
        }
        /// <summary>
        /// Returns a string representation of current state
        /// </summary>
        /// <returns></returns>
        public string display()
        {
            string s = "Max count: " + max + " Count: " + count;
            return s;
        }
        /// <summary>
        /// Resets value and allows changing max ticks
        /// </summary>
        /// <param name="value"></param>
        public void reset(int value = 100)
        {
            max = value;
            count = 0;
        }
        /// <summary>
        /// Sets current ticks to zero
        /// </summary>
        public void zero()
        {
            count = 0;
        }
        /// <summary>
        /// Max property
        /// </summary>
        public int Max
        {
            get { return max; }
        }
        /// <summary>
        /// Currnet ticks property
        /// </summary>
        public int Current
        {
            get { return count; }
        }
    }
}
