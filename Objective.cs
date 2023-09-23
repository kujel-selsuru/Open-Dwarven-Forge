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
    public enum Phaze { one = 0, two, three, four, five, six, seven, eight, nine, finished }

    //base class for objectives
    public class Objective
    {
        //protected
        protected Phaze phaze;
        protected SimpleSprite body;
        protected Random rand;
        protected Counter wait;
        protected bool done;

        //public
        /// <summary>
        /// Objective constructor
        /// </summary>
        /// <param name="ticks">Wait value</param>
        public Objective(int ticks = 10)
        {
            phaze = Phaze.one;
            this.body = null;
            rand = new Random(new Guid().GetHashCode());
            wait = new Counter(ticks);
            done = false;
        }
        /// <summary>
        /// Updates internal state, impliment in child class
        /// </summary>
        public virtual void update()
        {

        }
        /// <summary>
        /// Phaze property
        /// </summary>
        public Phaze Stage
        {
            get { return phaze; }
            set { phaze = value; }
        }
        /// <summary>
        /// Body/host property
        /// </summary>
        public SimpleSprite Body
        {
            get { return body; }
            set { body = value; }
        }
        /// <summary>
        /// Done property
        /// </summary>
        public bool Done
        {
            get { return done; }
        }
    }

}
