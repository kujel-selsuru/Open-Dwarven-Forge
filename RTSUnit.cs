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
public enum UNITTYPE
    {
        ut_ground = 0, ut_air, ut_gndWorker, ut_airWorker, ut_gndHarvester,
        ut_airHarvester, ut_gndMiner, ut_airMiner, ut_gndSpecial, ut_airSpecial, ut_gndHero, ut_airHero,
        ut_gndMedic, ut_airMedic, ut_airCarrier, ut_gndCarrier, ut_airAPC, ut_gndAPC
    };
    public class RTSUnit : RTSObject
    {
        //protected
        protected UNITTYPE ut;
        protected int tank;
        protected int tankSize;
        protected string resourceName;
        protected RTSUnit host;
        protected int hostIndex;

        //public
        /// <summary>
        /// builds a RTS Unit instance, sprite details parsed from sprite name
        /// </summary>
        /// <param name="spriteName">Sprite sheet name in graphics bank</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        /// <param name="numFrames">number of frames</param>
        /// <param name="maxHP">Max HP</param>
        /// <param name="name">Name of unit</param>
        /// <param name="owner">Owning RTS commander</param>
        public RTSUnit(string spriteName, float x, float y, int w, int h, int numFrames, int maxHP, string name, RTSCommander owner = null)
            : base(spriteName, x, y, w, h, numFrames, maxHP, OBJECTTYPE.ot_unit, name, owner)
        {
            ut = UNITTYPE.ut_ground;
            tank = 0;
            tankSize = 0;
            tempObjective = null;
            resourceName = "";
            host = null;
            hostIndex = -1;
        }
        /// <summary>
        /// RTSUnit copy constructor
        /// </summary>
        /// <param name="obj">RTSUnit instance to copy</param>
        public RTSUnit(RTSUnit obj) : base(obj)
        {
            ut = obj.UT;
            tank = obj.Tank;
            tankSize = obj.TankSize;
            tempObjective = obj.TempObjective;
            resourceName = obj.resourceName;
            host = obj.Host;
            hostIndex = HostIndex;
        }
        /// <summary>
        /// RTSUnit constructor from file, set host reference outside of constructor
        /// </summary>
        /// <param name="sr">Stream reader reference</param>
        public RTSUnit(System.IO.StreamReader sr)
            : base(sr)
        {
            sr.ReadLine();
            ut = (UNITTYPE)(Convert.ToInt32(sr.ReadLine()));
            tank = Convert.ToInt32(sr.ReadLine());
            tankSize = Convert.ToInt32(sr.ReadLine());
            resourceName = sr.ReadLine();
            hostIndex = Convert.ToInt32(sr.ReadLine());
            host = null;
        }
        /// <summary>
        /// Save object instance data
        /// </summary>
        /// <param name="sw">stream writer reference</param>
        public override void saveData(System.IO.StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======RTSUnit Data======");
            sw.WriteLine((int)ut);
            sw.WriteLine(tank);
            sw.WriteLine(tankSize);
            sw.WriteLine(resourceName);
            sw.WriteLine(hostIndex);
        }
        /// <summary>
        /// load object instance data, set host reference outside of loadData
        /// </summary>
        /// <param name="sr">stream reader reference</param>
        /// <param name="gb">graphic bank</param>
        public override void loadData(System.IO.StreamReader sr)
        {
            base.loadData(sr);
            sr.ReadLine();
            ut = (UNITTYPE)(Convert.ToInt32(sr.ReadLine()));
            tank = Convert.ToInt32(sr.ReadLine());
            tankSize = Convert.ToInt32(sr.ReadLine());
            resourceName = sr.ReadLine();
            hostIndex = Convert.ToInt32(sr.ReadLine());
            host = null;
        }
        /// <summary>
        /// Unit type property
        /// </summary>
        public UNITTYPE UT
        {
            get { return ut; }
            set { ut = value; }
        }
        /// <summary>
        /// Tank property
        /// </summary>
        public int Tank
        {
            get { return tank; }
            set { tank = value; }
        }
        /// <summary>
        /// Tank size property
        /// </summary>
        public int TankSize
        {
            get { return tankSize; }
            set { tankSize = value; }
        }
        /// <summary>
        /// Resource name property
        /// </summary>
        public string ResourceName
        {
            get { return resourceName; }
            set { resourceName = value; }
        }
        /// <summary>
        /// Host property
        /// </summary>
        public RTSUnit Host
        {
            get { return host; }
            set { host = value; }
        }
        /// <summary>
        /// Host index property
        /// </summary>
        public int HostIndex
        {
            get { return hostIndex; }
            set { hostIndex = value; }
        }
    }
}
