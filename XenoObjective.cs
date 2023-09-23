//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenoLib
{
    public enum PHAZE { one = 0, two, three, four, five, six, seven, eight, nine, finished }

    //base class for objectives
    public class XenoObjective
    {
        //protected
        protected PHAZE phaze;
        protected XenoSprite body;
        protected Random rand;
        protected Counter wait;
        protected XenoSprite target;
        protected bool done;

        //public
        /// <summary>
        /// XenoObjective constructor
        /// </summary>
        /// <param name="rand">Random reference</param>
        /// <param name="ticks">Wait value</param>
        public XenoObjective(Random rand = null, int ticks = 10)
        {
            phaze = PHAZE.one;
            this.body = null;
            if (rand != null)
            {
                this.rand = rand;
            }
            else
            { 
                rand = new Random((int)System.DateTime.Now.Ticks);
            }
            wait = new Counter(ticks);
            target = null;
            done = false;
        }
        /// <summary>
        /// Updates internal state, impliment in child class
        /// <param name="actions">List of XenoSprite objects to add to</param>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="spf">SimplePathFinder reference</param>
        /// <param name="mgLeft">Left side of MapGraph</param>
        /// <param name="mgTop">Top side of MapGraph</param>
        /// <param name="range">Minimum desired range to target in tiles</param>
        /// <param name="scaler">Scaling value for calculating range</param>
        /// <param name="rangeMod">Hyponinuse of tiles</param>
        public virtual void update(List<XenoSprite> actions, MapGraph mg, SimplePathFinder spf, 
            int mgLeft, int mgTop, int range, int scaler = 32, int rangeMod = 45)
        {

        }
        /// <summary>
        /// Updates internal state, impliment in child class
        /// <param name="actions">List of XenoSprite objects to add to</param>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="spf">SimplePathFinder reference</param>
        /// <param name="mgLeft">Left side of MapGraph</param>
        /// <param name="mgTop">Top side of MapGraph</param>
        /// <param name="range">Minimum desired range to target in tiles</param>
        /// <param name="cell">OpenWorldCell reference</param>
        /// <param name="scaler">Scaling value for calculating range</param>
        /// <param name="rangeMod">Hyponinuse of tiles</param>
        public virtual void update(List<XenoSprite> actions, MapGraph mg, SimplePathFinder spf,
            int mgLeft, int mgTop, int range, OpenWorldCell cell, int scaler = 32, int rangeMod = 45)
        {

        }
        /// <summary>
        /// Updates internal state, impliment in child class
        /// <param name="actions">List of XenoSprite objects to add to</param>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="spf">SimplePathFinder reference</param>
        /// <param name="mgLeft">Left side of MapGraph</param>
        /// <param name="mgTop">Top side of MapGraph</param>
        /// <param name="range">Minimum desired range to target in tiles</param>
        /// <param name="cell">OpenWorldCell reference</param>
        /// <param name="quad">Sprite quadrent value</param>
        /// <param name="scaler">Scaling value for calculating range</param>
        /// <param name="rangeMod">Hyponinuse of tiles</param>
        public virtual void update(List<XenoSprite> actions, MapGraph mg, SimplePathFinder spf,
            int mgLeft, int mgTop, int range, OpenWorldCell cell, int quad, int scaler = 32, int rangeMod = 45)
        {

        }
        /// <summary>
        /// Phaze property
        /// </summary>
        public PHAZE Stage
        {
            get { return phaze; }
            set { phaze = value; }
        }
        /// <summary>
        /// Body/host property
        /// </summary>
        public XenoSprite Body
        {
            get { return body; }
            set { body = value; }
        }
        /// <summary>
        /// Target property
        /// </summary>
        public XenoSprite Target
        {
            get { return target; }
            set { target = value; }
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
