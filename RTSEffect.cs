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
public class RTSEffect : RTSObject
    {
        //protected

        //public
        /// <summary>
        /// RTSEffect constructor
        /// </summary>
        /// <param name="spriteName">Sprite name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        /// <param name="numFrames">number of frames</param>
        /// <param name="name">Name of effect</param>
        public RTSEffect(string spriteName, float x, float y, int w, int h, int numFrames, string name)
            : base(spriteName, x, y, w, h, numFrames, 10, OBJECTTYPE.ot_effect, name)
        {

        }
        /// <summary>
        /// RTSEffect copy constructor
        /// </summary>
        /// <param name="obj">RTSEffect object referecne</param>
        public RTSEffect(RTSEffect obj)
            : base(obj)
        {

        }
        /// <summary>
        /// RTSEffect from file constructor 
        /// </summary>
        /// <param name="sr"></param>
        public RTSEffect(System.IO.StreamReader sr) :
            base(sr)
        {

        }
        /// <summary>
        /// RTSEffect update override
        /// </summary>
        public override void update()
        {
            base.update();
            if (frame >= numFrames - 1)
            {
                dead = true;
            }
        }
    }
}
