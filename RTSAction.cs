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
    public enum RTSACTIONTYPE
    {
        at_g2g = 0, at_g2a, at_a2a, at_a2g, at_buff, at_debuff,
        at_heal, at_special, at_harvest, at_refine, at_repair, at_build, at_mine
    };
    public class RTSAction : RTSObject
    {
        //protected
        protected RTSACTIONTYPE at;
        protected int timeSpan;
        protected int timeSpent;
        protected int dam;
        //public
        /// <summary>
        /// builds a RTS Action instance, sprite details parsed from sprite name
        /// </summary>
        /// <param name="spriteName">Sprite sheet name in graphics bank</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        /// <param name="numFrames">number of frames</param>
        /// <param name="name">Name of action</param>
        /// <param name="damage">Damage value</param>
        /// <param name="timeSpan">Time span of action</param>
        public RTSAction(string spriteName, float x, float y, int w, int h, int numFrames, string name, int damage = 100, int timeSpan = 300)
            : base(spriteName, x, y, w, h, numFrames, 0, OBJECTTYPE.ot_action, name)
        {
            at = RTSACTIONTYPE.at_g2g;
            timeSpent = 0;
            this.dam = damage;
            this.TimeSpan = timeSpan;
        }
        /// <summary>
        /// RTSAction copy constructor
        /// </summary>
        /// <param name="obj">RTSAction instance to copy</param>
        public RTSAction(RTSAction obj) : base(obj)
        {
            at = obj.AT;
            timeSpan = obj.timeSpan;
            dam = obj.Damage;
            timeSpent = 0;
        }
        /// <summary>
        /// RTSAction constructor from file
        /// </summary>
        /// <param name="sr">Stream reader reference</param>
        public RTSAction(System.IO.StreamReader sr) : base(sr)
        {
            sr.ReadLine();
            at = (RTSACTIONTYPE)(Convert.ToInt32(sr.ReadLine()));
            dam = Convert.ToInt32(sr.ReadLine());
            timeSpan = Convert.ToInt32(sr.ReadLine());
            timeSpent = 0;
        }
        /// <summary>
        /// Save object instance data
        /// </summary>
        /// <param name="sw">stream writer reference</param>
        public override void saveData(System.IO.StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======RTSAction Data======");
            sw.WriteLine((int)at);
            sw.WriteLine(dam.ToString());
            sw.WriteLine(timeSpan.ToString());
        }
        /// <summary>
        /// Load object instance data
        /// </summary>
        /// <param name="sr">stream reader reference</param>
        public override void loadData(System.IO.StreamReader sr)
        {
            base.loadData(sr);
            sr.ReadLine();
            at = (RTSACTIONTYPE)(Convert.ToInt32(sr.ReadLine()));
            dam = Convert.ToInt32(sr.ReadLine());
            timeSpan = Convert.ToInt32(sr.ReadLine());
            timeSpent = 0;
        }
        /// <summary>
        /// Pure virtual method for handling specials in special derived classes
        /// </summary>
        /// <param name="world">WorldScene referecne</param>
        public virtual void special(WorldScene world)
        {
            //implement in child classes
        }
        /// <summary>
        /// Loads a new set of values into action and resets counters
        /// </summary>
        /// <param name="name">Name of new action to load</param>
        /// <param name="x">X position to load</param>
        /// <param name="y">Y position to load</param>
        /// <param name="actDB">RTSAction database to load from</param>
        public void reload(string name, float x, float y, GenericBank<RTSAction> actDB)
        {
            RTSAction obj = actDB.getData(name);
            if (obj != null)
            {
                hitBox = new Rectangle(x, y, obj.W, obj.H);
                destRect.w = (int)obj.W;
                destRect.h = (int)obj.H;
                srcRect.w = (int)obj.W;
                srcRect.h = (int)obj.H;
                hitBox.Width = obj.W;
                hitBox.Height = obj.H;
                at = obj.AT;
                speed = obj.Speed;
                dead = false;
                timeSpan = obj.TimeSpan;
                timeSpent = 0;
            }
        }
        /// <summary>
        /// Override of RTSObject's update function
        /// </summary>
        public override void update()
        {
            timeSpent++;
            if (timeSpent >= timeSpan)
            {
                dead = true;
            }
        }
        /// <summary>
        /// Action type property
        /// </summary>
        public RTSACTIONTYPE AT
        {
            get { return at; }
            set { at = value; }
        }
        /// <summary>
        /// Time span property
        /// </summary>
        public int TimeSpan
        {
            get { return timeSpan; }
            set { timeSpan = value; }
        }
        /// <summary>
        /// Time spent property
        /// </summary>
        public int TimeSpent
        {
            get { return timeSpent; }
            set { timeSpent = value; }
        }
        /// <summary>
        /// Damage property
        /// </summary>
        public int Damage
        {
            get { return dam; }
            set { dam = value; }
        }
    }
}
