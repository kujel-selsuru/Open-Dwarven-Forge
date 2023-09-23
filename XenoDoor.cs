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
    /// XenoDoor class
    /// </summary>
    public class XenoDoor : XenoBuilding
    {
        //protected
        protected bool open;
        protected int lockStrength;

        //public
        /// <summary>
        /// XenoDoor constructor
        /// </summary>
        /// <param name="source">Texture2D</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="tileWidth">Tile width</param>
        /// <param name="tileHeight">Tile Height</param>
        /// <param name="lockStrength">Strength of lock value</param>
        public XenoDoor(Texture2D source, float x, float y, int width, int height, int numFrames, int tileWidth, int tileHeight, int lockStrength) : 
            base(source, x, y, width, height, numFrames, tileWidth, tileHeight)
        {
            this.lockStrength = lockStrength;
            open = false;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
        }
        /// <summary>
        /// XenoDoor from file constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="sr">StreamReader reference</param>
        public XenoDoor(Texture2D source, StreamReader sr) : base(source, sr)
        {
            sr.ReadLine();//discard testing data
            lockStrength = Convert.ToInt32(sr.ReadLine());
            open = Convert.ToBoolean(sr.ReadLine());
            tileWidth = Convert.ToInt32(sr.ReadLine());
            tileHeight = Convert.ToInt32(sr.ReadLine());
        }
        /// <summary>
        /// XenoDoor save data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======XenoDoor Data======");
            sw.WriteLine(lockStrength);
            sw.WriteLine(open);
            sw.WriteLine(tileWidth);
            sw.WriteLine(tileHeight);
        }
        /// <summary>
        /// XenoDoor update MapGraph
        /// </summary>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="mgLeft">MapGraph left side</param>
        /// <param name="mgTop">MapGraph top side</param>
        /// <param name="interior">Interior flag value</param>
        public override void updateMG(MapGraph mg, int mgLeft, int mgTop, bool interior)
        {
            if (!open)
            {
                if (tileWidth != 0 && tileHeight != 0)
                {
                    for (int tx = ((int)X) / tileWidth; tx < ((int)X) / tileWidth + (int)W / tileWidth; tx++)
                    {
                        for (int ty = ((int)Y) / tileHeight; ty < ((int)Y) / tileHeight + (int)H / tileHeight; ty++)
                        {
                            mg.setCell(tx, ty, false);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// XenoDoor draw
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        public override void draw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            srcRect.x = srcRect.w * frame;
            srcRect.y = (int)direct * srcRect.h;
            destRect.x = (int)hitBox.X - winx;
            destRect.y = (int)hitBox.Y - winy;
            SimpleDraw.draw(renderer, source, srcRect, destRect);
            if (open)
            {
                if (frameDelay.tick())
                {
                    if (frame < numFrames)
                    {
                        frame++;
                    }
                }
            }
        }
        /// <summary>
        /// Attempts to pick lock provided a skill input, returns success or failure state
        /// Lock picking skill value 15 points or more below lock strength is a garunteed fail
        /// </summary>
        /// <param name="skill">Lock picking skill value</param>
        /// <returns>Boolean</returns>
        public bool pickLock(int skill)
        {
            int diff;
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            if(skill < lockStrength - 15)
            {
                return false;
            }
            else
            {
                diff = lockStrength - skill;
                int value = rand.Next(30, 100);
                if(value - diff >= lockStrength)
                {
                    open = true;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Open property
        /// </summary>
        public bool Open
        {
            get { return open; }
            set { open = value; }
        }
    }
}
