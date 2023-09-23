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
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// A XenoSprite derivitive with PointFrame and SnowManBox
    /// collsion features added, collider boxes are 33% dimensions
    /// of normal hitBox
    /// </summary>
    public class XenoExplorer : XenoSprite
    {
        //protected
        protected PointFrame pFrame;
        protected SnowManBox colliders;
        //public 
        /// <summary>
        /// XenoExplorer constructor
        /// </summary>
        /// <param name="name">Sprite name and source Texture2D key</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y positon</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="hp">HitPoints value</param>
        /// <param name="delay">Frame delay value</param>
        public XenoExplorer(string name, float x, float y, int width, int height, int numFrames, int hp = 100, int delay = 5) : 
            base(name, x, y, width, height, numFrames, hp, delay)
        {
            pFrame = new PointFrame(Center.X, Center.Y, width, height);
            colliders = new SnowManBox(Center.X, Center.Y, width * 0.33f, height * 0.33f );
        }
        /// <summary>
        /// From file XenoExplorer constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        /// <param name="delay">Frame delay value</param>
        public XenoExplorer(StreamReader sr, int delay = 5) : 
            base(sr, delay)
        {
            pFrame = new PointFrame(sr);
            colliders = new SnowManBox(sr);
        }
        /// <summary>
        /// Move override
        /// </summary>
        /// <param name="angle">Direction of movement (in degrees)</param>
        /// <param name="speed">Speed of movement</param>
        public override void move(double angle, float speed)
        {
            base.move(angle, speed);
            pFrame.move(angle, speed);
            colliders.move(angle, speed);
        }
        /// <summary>
        /// Move override
        /// </summary>
        /// <param name="x">X direction value</param>
        /// <param name="y">Y direction value</param>
        public override void move(float x, float y)
        {
            base.move(x, y);
            pFrame.move(x, y);
            colliders.move(x, y);
        }
        /// <summary>
        /// PFrame property
        /// </summary>
        public PointFrame PFrame
        {
            get { return pFrame; }
        }
        /// <summary>
        /// Colliders property
        /// </summary>
        public SnowManBox Colliders
        {
            get { return colliders; }
        }
    }
}
