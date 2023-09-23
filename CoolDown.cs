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
    /// CoolDown class
    /// </summary>
    public class CoolDown
    {
        //public
        protected int ticks;
        protected int maxTicks;
        protected bool active;
        //public
        /// <summary>
        /// CoolDown constructor
        /// </summary>
        /// <param name="maxTicks">Max number of ticks</param>
        public CoolDown(int maxTicks)
        {
            this.maxTicks = maxTicks;
            active = false;
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        public void update()
        {
            if (active)
            {
                ticks++;
                if (ticks >= maxTicks)
                {
                    active = false;
                    ticks = 0;
                }
            }
        }
        /// <summary>
        /// Activates CoolDown
        /// </summary>
        public void activate()
        {
            active = true;
        }
        /// <summary>
        /// Returns the percentage of cool down complete 
        /// </summary>
        public int Percentage
        {
            get { return (int)(((double)ticks/(double)maxTicks) * 100);}
        }
        /// <summary>
        /// Reset CoolDown internal state
        /// </summary>
        public void reset()
        {
            ticks = 0;
            active = false;
        }
        /// <summary>
        /// Reset CoolDown internal state
        /// </summary>
        public void reset(int t)
        {
            maxTicks = t;
            ticks = 0;
            active = false;
        }
        /// <summary>
        /// Max ticks property
        /// </summary>
        public int MaxTicks
        {
            get { return maxTicks; }
        }
        /// <summary>
        /// Current ticks property
        /// </summary>
        public int Ticks
        {
            get { return ticks; }
        }
        /// <summary>
        /// Active property
        /// </summary>
        public bool Active
        {
            get { return active; }
            //set { active = value; }
        }
    }
}
