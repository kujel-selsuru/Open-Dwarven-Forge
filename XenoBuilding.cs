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
    /// XenoBuilding class
    /// </summary>
    public class XenoBuilding : XenoSprite
    {
        //protected 
        protected int tileWidth;
        protected int tileHeight;

        //public
        /// <summary>
        /// XenoBuilding constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width</param>
        /// <param name="height">Heihgt</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="tileWidth">Tile width</param>
        /// <param name="tileHeight">Tile Height</param>
        public XenoBuilding(Texture2D source, float x, float y, int width, int height, int numFrames, int tileWidth = 32, int tileHeight = 32) : 
            base(source, x, y, width, height, numFrames)
        {
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
        }
        /// <summary>
        /// XenoBuilding copy constructor
        /// </summary>
        /// <param name="obj">XenoBuilding reference</param>
        public XenoBuilding(XenoBuilding obj) : base(obj)
        {
            tileWidth = obj.tileWidth;
            tileHeight = obj.tileHeight;
        }
        /// <summary>
        /// XenoBuilding constructor
        /// </summary>
        /// <param name="source">Name of source graphic</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width</param>
        /// <param name="height">Heihgt</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="tileWidth">Tile width</param>
        /// <param name="tileHeight">Tile Height</param>
        public XenoBuilding(string source, float x, float y, int width, int height, int numFrames, int tileWidth = 32, int tileHeight = 32) :
            base(source, x, y, width, height, numFrames)
        {
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
        }
        /// <summary>
        /// XenoBuilding from file constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="sr">StreamReader reference</param>
        public XenoBuilding(Texture2D source, StreamReader sr) : base(source, sr)
        {
            sr.ReadLine();
            tileWidth = Convert.ToInt32(sr.ReadLine());
            tileHeight = Convert.ToInt32(sr.ReadLine());
        }
        /// <summary>
        /// XenoBuilding from file constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public XenoBuilding(StreamReader sr) : base(sr)
        {
            sr.ReadLine();
            tileWidth = Convert.ToInt32(sr.ReadLine());
            tileHeight = Convert.ToInt32(sr.ReadLine());
        }
        /// <summary>
        /// XenoBuilding save data
        /// </summary>
        /// <param name="sw"></param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======XenoBuilding Data======");
            sw.WriteLine(tileWidth);
            sw.WriteLine(tileHeight);
        }
        /// <summary>
        /// Updates the mapGraph based on size of XenoBuilding
        /// </summary>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="shiftRight">Shift right value</param>
        /// <param name="shiftDown">Shift down value</param>
        /// <param name="interior">interior flag value</param>
        public virtual void updateMG(MapGraph mg, int shiftRight, int shiftDown, bool interior)
        {
            if (tileWidth != 0 && tileHeight != 0)
            {
                if (interior)
                {
                    //interior cell's have no offset
                    for (int tx = ((int)X) / tileWidth; tx < ((int)X) / tileWidth + (int)W / tileWidth; tx++)
                    {
                        for (int ty = ((int)Y) / tileHeight; ty < ((int)Y) / tileHeight + (int)H / tileHeight; ty++)
                        {
                            mg.setCell(tx, ty, false);
                        }
                    }
                }
                else
                {
                    //x position + shiftRight to get relative leftside of XenoBuilding
                    //y position + shiftDown tp get relative topSide of XenoBuilding
                    for (int tx = ((int)X / tileWidth) + shiftRight; tx < ((int)X / tileWidth) + shiftRight + (int)W / tileWidth; tx++)
                    {
                        for (int ty = ((int)Y / tileHeight) + shiftDown; ty < ((int)Y / tileHeight) + shiftDown + (int)H / tileHeight; ty++)
                        {
                            mg.setCell(tx, ty, false);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Updates the mapGraph based on size of XenoBuilding (only sets the lower half)
        /// </summary>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="shiftRight">Shift right value</param>
        /// <param name="shiftDown">Shift down value</param>
        /// <param name="interior">interior flag value</param>
        public virtual void updateMGLower(MapGraph mg, int shiftRight, int shiftDown, bool interior)
        {
            if (tileWidth != 0 && tileHeight != 0)
            {
                if (interior)
                {
                    //interior cell's have no offset
                    for (int tx = ((int)X) / tileWidth; tx < ((int)X) / tileWidth + (int)W / tileWidth; tx++)
                    {
                        for (int ty = (((int)Y) / tileHeight) + (((int)H / 2) / tileHeight); ty < ((int)Y) / tileHeight + (int)H / tileHeight; ty++)
                        {
                            mg.setCell(tx, ty, false);
                        }
                    }
                }
                else
                {
                    int shiftx = 0;
                    int shifty = 0;
                    if(shiftRight > 0)
                    {
                        shiftx = 100;
                    }
                    if (shiftDown > 0)
                    {
                        shifty = 100;
                    }
                    //x position - mgLeft to get relative leftside of XenoBuilding
                    //y position - mgTop tp get relative topSide of XenoBuilding
                    for (int tx = ((int)X / tileWidth) + shiftx; tx < ((int)X / tileWidth) + shiftx + (int)W / tileWidth; tx++)
                    {
                        for (int ty = (((int)Y / tileHeight) + shifty) + (((int)H / 2) / tileHeight); ty < ((int)Y / tileHeight) + shifty + (int)H / tileHeight; ty++)
                        {
                            mg.setCell(tx, ty, false);
                        }
                    }
                }
            }
        }
    }
}
