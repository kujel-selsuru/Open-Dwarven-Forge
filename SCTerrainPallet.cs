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
using XenoLib;

namespace XenoLib
{
    public class SCStamp
    {
        //protected
        protected Point2D srcValue;
        protected int tv;

        //public
        /// <summary>
        /// SCStamp constructor
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        public SCStamp(int x, int y)
        {
            srcValue = new Point2D(x, y);
            tv = 0;
        }
        /// <summary>
        /// X property
        /// </summary>
        public int X
        {
            get { return srcValue.IX; }
            set { srcValue.X = value; }
        }
        /// <summary>
        /// Y property
        /// </summary>
        public int Y
        {
            get { return srcValue.IY; }
            set { srcValue.Y = value; }
        }
        /// <summary>
        /// TV property
        /// </summary>
        public int TV
        {
            get { return tv; }
            set { tv = value; }
        }
    }

    public class SCTerrainPallet
    {
        //protected
        protected SCStamp stamp;
        protected Rectangle tileDestRect;
        protected Rectangle tileSrcRect;
        protected Rectangle stampRect;
        protected RadioButton tilesButton;
        protected Point2D pos;
        protected Texture2D source;
        protected string sourceName;
        protected CoolDown delay;
        protected DataGrid<int> terrainValues;
        protected ScrollBar scrollBar;

        //public
        /// <summary>
        /// SCTerrainPallet
        /// </summary>
        /// <param name="x">X poition</param>
        /// <param name="y">Y position</param>
        /// <param name="source">Texture2D reference</param>
        public SCTerrainPallet(int x, int y, string sourceName)
        {
            pos = new Point2D(x, y);
            tileSrcRect = new Rectangle(0, 0, 224, 576);
            tileDestRect = new Rectangle(x, y + 32, 224, 576);
            stampRect = new Rectangle(x, y + 32, 32, 32);
            tilesButton = new RadioButton(TextureBank.getTexture("raido button 32 x 32"), 
                false, "tiles", x, y, 32, 32, 60);
            this.source = TextureBank.getTexture(sourceName);
            delay = new CoolDown(60);
            terrainValues = new DataGrid<int>(7, 18);
            stamp = new SCStamp(0, 0);
            stamp.TV = terrainValues.Grid[0, 0];
            scrollBar = new ScrollBar(x + 256, y, 960, 16);
        }
        /// <summary>
        /// SCTerrainPallet from file constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public SCTerrainPallet(StreamReader sr)
        {
            sr.ReadLine();
            sourceName = sr.ReadLine();
            source = TextureBank.getTexture(sourceName);
            terrainValues = new DataGrid<int>(Convert.ToInt32(sr.ReadLine()),
                Convert.ToInt32(sr.ReadLine()));
            for (int x = 0; x < terrainValues.Width; x++)
            {
                for (int y = 0; y < terrainValues.Height; y++)
                {
                    terrainValues.Grid[x, y] = Convert.ToInt32(sr.ReadLine());
                }
            }
        }
        /// <summary>
        /// Save SCTerrainPallet data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveData(StreamWriter sw)
        {
            sw.WriteLine("======SCTerrainPallet Data======");
            sw.WriteLine(sourceName);
            sw.WriteLine(terrainValues.Width);
            sw.WriteLine(terrainValues.Height);
            for(int x = 0; x < terrainValues.Width; x++)
            {
                for (int y = 0; y < terrainValues.Height; y++)
                {
                    sw.WriteLine(terrainValues.Grid[x, y].ToString());
                }
            }
        }
        /// <summary>
        /// Updates SCTerrainPallet internal state
        /// </summary>
        /// <param name="pointer">SimplePointer reference</param>
        public void update(SimplePointer pointer)
        {
            tilesButton.update(pointer.HitBox, MouseHandler.getLeft());
            if(tileDestRect.pointInRect(new Point2D(pointer.X, 
                pointer.Y)) == true)
            {
                if (tilesButton.State == false)
                {
                    if (MouseHandler.getLeft() == true)
                    {
                        if (delay.Active == false)
                        {
                            delay.activate();
                            stamp.X = ((pointer.X - (int)tileDestRect.X) / 32) * 32;
                            stamp.Y = ((pointer.Y - (int)tileDestRect.Y) / 32) * 32;
                            stampRect.X = (((pointer.X - (int)tileDestRect.X) / 32) * 32) + pos.X;
                            stampRect.Y = (((pointer.Y - (int)tileDestRect.Y) / 32) * 32) + pos.Y + 32;
                        }
                    }
                }
                else
                {
                    int tx = ((pointer.X - (int)tileDestRect.X) / 32);
                    int ty = ((pointer.Y - (int)tileDestRect.Y) / 32);
                    if (MouseHandler.getLeft() == true)
                    {
                        if (delay.Active == false)
                        {
                            delay.activate();
                            if (terrainValues.Grid[tx, ty] < 10)
                            {
                                terrainValues.Grid[tx, ty]++;
                            }
                        }
                    }
                    if (MouseHandler.getRight() == true)
                    {
                        if (delay.Active == false)
                        {
                            delay.activate();
                            if (terrainValues.Grid[tx, ty] > -1)
                            {
                                terrainValues.Grid[tx, ty]--;
                            }
                        }
                    }
                }
            }
            //scrollBar.update();
            delay.update();
        }
        /// <summary>
        /// Draws SCTerrainPallet
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            tilesButton.draw(renderer);
            SimpleDraw.draw(renderer, source, tileSrcRect, tileDestRect);
            DrawRects.drawRect(renderer, stampRect, ColourBank.getColour(XENOCOLOURS.BLUE));
            if(tilesButton.State == true)
            {
                for(int x = 0; x < terrainValues.Width; x++)
                {
                    for (int y = 0; y < terrainValues.Height; y++)
                    {
                        string str = terrainValues.Grid[x, y].ToString();
                        int renderWidth = SimpleFont.stringRenderWidth(str, 0.5f);
                        SimpleFont.drawColourString(renderer, str,
                            pos.IX + (32 * x) + (16 - (renderWidth / 2)), 
                            pos.IY + (32 * y) + 16 + 32, "red", 0.5f);
                    }
                }
            }
            //scrollBar.draw(renderer);
        }
        /// <summary>
        /// SCStamp property
        /// </summary>
        public SCStamp Stampp
        {
            get { return stamp; }
        }

    }
}
