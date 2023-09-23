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
public enum SCRIPTSTATE { ss_running = 0, ss_failed, ss_succeeded }

    public class RTSScript
    {
        //protected
        protected WorldScene world;
        protected string name;
        protected Counter delay;
        protected SCRIPTSTATE scriptState;
        //public
        /// <summary>
        /// Base script constructor
        /// </summary>
        /// <param name="world">WorldScene reference</param>
        /// <param name="name">Script name</param>
        /// <param name="name">Script internal delay</param>
        public RTSScript(WorldScene world, string name = "script", int delay = 60)
        {
            this.world = world;
            this.name = name;
            this.delay = new Counter(delay);
        }
        /// <summary>
        /// Base script from file constructor
        /// </summary>
        /// <param name="world">WorldScene reference</param>
        /// <param name="name">Stream reader reference</param>
        public RTSScript(WorldScene world, System.IO.StreamReader sr)
        {
            this.world = world;
            this.name = sr.ReadLine();
            this.delay = new Counter(Convert.ToInt32(sr.ReadLine()));
        }
        /// <summary>
        /// RTSScript copy constructor
        /// </summary>
        /// <param name="obj">Object reference</param>
        public RTSScript(RTSScript obj)
        {
            world = obj.World;
            name = obj.Name;
            delay = new Counter(obj.Delay);
        }
        /// <summary>
        /// Initializes a script for use
        /// </summary>
        /// <param name="world">Worldscene reference</param>
        public virtual void initializer(WorldScene world)
        {

        }
        /// <summary>
        /// Updates internal state and is only a blank virtual function
        /// </summary>
        public virtual void update()
        {

        }
        /// <summary>
        /// Save script data
        /// </summary>
        /// <param name="sw">Stream writer reference</param>
        public virtual void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======RTS Script Data======");
            sw.WriteLine(name);
            sw.WriteLine(delay.Max);
        }
        /// <summary>
        /// Name property
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// Delay property
        /// </summary>
        public int Delay
        {
            get { return delay.Max; }
            set { delay.reset(value); }
        }
        /// <summary>
        /// World property
        /// </summary>
        public WorldScene World
        {
            get { return world; }
            set { world = value; }
        }
        /// <summary>
        /// ScriptState property
        /// </summary>
        public SCRIPTSTATE ScriptState
        {
            get { return scriptState; }
        }
    }
}
