//====================================================
//Written by Kujel Selsuru
//Last Updated 26/03/24
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenoLib
{
    public class XenoObject : XenoSprite
    {
        //protected
        protected List<XenoComponent> parts;
        protected XenoCrate crate;

        //public
        /// <summary>
        /// XenoObject constructor
        /// </summary>
        /// <param name="name">Source graphic name/object name</param>
        /// <param name="x">X position in pixels</param>
        /// <param name="y">Y position in pixels</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="hp">Hit point</param>
        /// <param name="delay">Frame delay value</param>
        public XenoObject(string name, float x, float y, int width, int height, int numFrames, 
            int hp = 100, int delay = 5) : 
            base(TextureBank.getTexture(name), x, y, width, height, numFrames, hp, delay)
        {
            this.name = name;
            parts = new List<XenoComponent>();
            crate = new XenoCrate(name + " crate");
        }
        /// <summary>
        /// XenoObject from file constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public XenoObject(System.IO.StreamReader sr) : base(sr)
        {
            sr.ReadLine();
            name = sr.ReadLine();
            int num = Convert.ToInt32(sr.ReadLine());
            parts = new List<XenoComponent>();
            XenoComponent tmp = null;
            for(int i = 0; i < num; i++)
            {
                tmp = new XenoComponent(sr);
                parts.Add(tmp);
            }
            crate = new XenoCrate(sr);
        }
        /// <summary>
        /// XenoObject copy constructor
        /// </summary>
        /// <param name="obj">XenoObject reference</param>
        public XenoObject(XenoObject obj) : base(obj)
        {
            name = obj.Name;
            parts = new List<XenoComponent>();
            XenoComponent tmp = null;
            for(int i = 0; i < obj.Parts.Count; i++)
            {
                tmp = new XenoComponent(parts[i]);
                parts.Add(tmp);
            }
            crate = new XenoCrate(obj.Crate);
        }
        /// <summary>
        /// Overrides saveData function
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======XenoObject Data======");
            sw.WriteLine(name);
            sw.WriteLine(parts.Count);
            for(int i = 0; i < parts.Count; i++)
            {
                parts[i].saveData(sw);
            }
            crate.saveData(sw);
        }
        /// <summary>
        /// Parts property
        /// </summary>
        public List<XenoComponent> Parts
        {
            get { return parts; }
        }
        /// <summary>
        /// Crate property
        /// </summary>
        public XenoCrate Crate
        {
            get { return crate; }
        }
    }
}
