//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XenoLib
{
    /// <summary>
    /// XenoDoodad class
    /// </summary>
    public class XenoDoodad : XenoSprite
    {
        //protected

        //public
        /// <summary>
        /// XenoDoodad constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="name">Name of doodad and source graphic</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="numFrames">Number of frames</param>
        public XenoDoodad(Texture2D source, string name, float x, float y, int width, int height, int numFrames) : base(source, x, y, width, height, numFrames, 100, 5, name)
        {
            //this.name = name;
            //source = TextureBank.getTexture(name);
        }
        /// <summary>
        /// XenoDoodad from fie constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="sr">StreamReader reference</param>
        public XenoDoodad(Texture2D source, StreamReader sr) : base(source, sr)
        {
            //source = TextureBank.getTexture(name);
        }
        /// <summary>
        /// XenoDoodad save data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
        }
    }
}
