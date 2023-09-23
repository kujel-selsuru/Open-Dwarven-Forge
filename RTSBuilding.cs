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
public enum BUILDINGTYPE
    {
        bt_command = 0, bt_tech, bt_factory, bt_turret, bt_support,
        bt_generator, bt_mine, bt_refinory, bt_foundation
    };
    public enum CONSTRUCTIONTYPE { ct_grows = 0, ct_built, ct_constructing, ct_growing };
    public class RTSBuilding : RTSObject
    {
        //protected
        protected BUILDINGTYPE bt;
        protected CONSTRUCTIONTYPE ct;
        //public
        /// <summary>
        /// builds a RTS Building instance, sprite details parsed from sprite name
        /// </summary>
        /// <param name="spriteName">Sprite sheet name in graphics bank</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        /// <param name="numFrames">number of frames</param>
        /// <param name="maxHP">Max HP</param>
        /// <param name="name">Name of building</param>
        /// <param name="owner">Owning RTS commander</param>
        public RTSBuilding(string spriteName, float x, float y, int w, int h, int numFrames, int maxHP, string name, RTSCommander owner)
            : base(spriteName, x, y, w, h, numFrames, maxHP, OBJECTTYPE.ot_building, name, owner)
        {
            bt = BUILDINGTYPE.bt_support;
        }
        /// <summary>
        /// RTSBuilding copy constructor
        /// </summary>
        /// <param name="obj">RTSBuilding instance to copy</param>
        public RTSBuilding(RTSBuilding obj) : base(obj)
        {
            bt = obj.BT;
        }
        /// <summary>
        /// RTSBuilding constructor from file
        /// </summary>
        /// <param name="sr">Stream reader reference</param>
        public RTSBuilding(System.IO.StreamReader sr) : base(sr)
        {
            sr.ReadLine();
            bt = (BUILDINGTYPE)(Convert.ToInt32(sr.ReadLine()));
            ct = (CONSTRUCTIONTYPE)(Convert.ToInt32(sr.ReadLine()));
        }
        /// <summary>
        /// Save object sintance data
        /// </summary>
        /// <param name="sw">stream writer reference</param>
        public override void saveData(System.IO.StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======RTSBuilding Data======");
            sw.WriteLine((int)bt);
            sw.WriteLine((int)ct);
        }
        /// <summary>
        /// Load object instance data
        /// </summary>
        /// <param name="sr">stream reader reference</param>
        /// <param name="gb">graphics bank</param>
        public override void loadData(System.IO.StreamReader sr)
        {
            base.loadData(sr);
            sr.ReadLine();
            bt = (BUILDINGTYPE)(Convert.ToInt32(sr.ReadLine()));
            ct = (CONSTRUCTIONTYPE)(Convert.ToInt32(sr.ReadLine()));
        }
        /// <summary>
        /// Building type property
        /// </summary>
        public BUILDINGTYPE BT
        {
            get { return bt; }
            set { bt = value; }
        }
        /// <summary>
        /// Construction type property
        /// </summary>
        public CONSTRUCTIONTYPE CT
        {
            get { return ct; }
            set { ct = value; }
        }
    }
}
