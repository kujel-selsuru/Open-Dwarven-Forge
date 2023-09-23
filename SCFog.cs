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
    public class SCFog
    {
        //protected
        protected int hexWidth;
        protected int hexHeight;
        protected int winx;
        protected int winy;
        protected DataGrid<bool> grid;
        protected Rectangle window;
        protected Texture2D source;
        protected Rectangle srcRect;
        protected Rectangle destRect;
        protected string sourceName;

        //public
        /// <summary>
        /// SCFog constructor
        /// </summary>
        /// <param name="sourceName">Source graphic name</param>
        /// <param name="hexWidth">Hex width in pixels</param>
        /// <param name="hexHeight">Hex height in pixels</param>
        /// <param name="width">Width in hexes</param>
        /// <param name="height">Height in hexes</param>
        /// <param name="winx">Winx in hexes</param>
        /// <param name="winy">Winy in hexes</param>
        /// <param name="winWidth">WinWidth in hexes</param>
        /// <param name="winHeight">WinHeight in hexes</param>
        public SCFog(string sourceName, int hexWidth, int hexHeight, 
            int width, int height, int winx, int winy, int winWidth, 
            int winHeight)
        {
            this.hexWidth = hexWidth;
            this.hexHeight = hexHeight;
            winx = 0;
            winy = 0;
            grid = new DataGrid<bool>(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid.Grid[x, y] = true;
                }
            }
            window = new Rectangle(0, 0, winWidth, winHeight);
            this.sourceName = sourceName;
            source = TextureBank.getTexture(sourceName);
            srcRect = new Rectangle(0, 0, 36, 36);
            destRect = new Rectangle(0, 0, 36, 36);
        }
        /// <summary>
        /// SCFog from file constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public SCFog(StreamReader sr)
        {
            sr.ReadLine();
            sourceName = sr.ReadLine();
            source = TextureBank.getTexture(sourceName);
            hexWidth = Convert.ToInt32(sr.ReadLine());
            hexHeight = Convert.ToInt32(sr.ReadLine());
            string buffer = "";
            grid = new DataGrid<bool>(Convert.ToInt32(sr.ReadLine()),
                Convert.ToInt32(sr.ReadLine()));
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    buffer = sr.ReadLine();
                    if (buffer == "TRUE")
                    {
                        grid.Grid[x, y] = true;
                    }
                    else
                    {
                        grid.Grid[x, y] = false;
                    }
                }
            }
            window = new Rectangle(Convert.ToInt32(sr.ReadLine()),
                Convert.ToInt32(sr.ReadLine()), Convert.ToInt32(sr.ReadLine()),
                Convert.ToInt32(sr.ReadLine()));
            winx = 0;
            winy = 0;
            srcRect = new Rectangle(0, 0, 36, 36);
            destRect = new Rectangle(0, 0, 36, 36);
        }
        /// <summary>
        /// SCFog copy constructor
        /// </summary>
        /// <param name="obj">Fog reference</param>
        public SCFog(SCFog obj)
        {
            hexWidth = obj.hexWidth;
            hexHeight = obj.hexHeight;
            winx = 0;
            winy = 0;
            source = obj.source;
            grid = new DataGrid<bool>(obj.FogGrid.Width, obj.FogGrid.Height);
            for (int x = 0; x < obj.FogGrid.Width; x++)
            {
                for (int y = 0; y < obj.FogGrid.Height; y++)
                {
                    grid.Grid[x, y] = obj.FogGrid.Grid[x, y];
                }
            }
            window = new Rectangle(0, 0, obj.window.Width, obj.window.Height);
            sourceName = obj.sourceName;
            source = TextureBank.getTexture(sourceName);
            srcRect = new Rectangle(0, 0, 36, 36);
            destRect = new Rectangle(0, 0, 36, 36);
        }
        /// <summary>
        /// Save SCFog data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveData(StreamWriter sw)
        {
            sw.WriteLine("======SCFog Data======");
            sw.WriteLine(sourceName);
            sw.WriteLine(hexWidth);
            sw.WriteLine(hexHeight);
            sw.WriteLine(grid.Width);
            sw.WriteLine(grid.Height);
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    if(grid.Grid[x, y] == true)
                    {
                        sw.WriteLine("TRUE");
                    }
                    else
                    {
                        sw.WriteLine("FALSE");
                    }
                }
            }
            sw.WriteLine(window.X);
            sw.WriteLine(window.Y);
            sw.WriteLine(window.Width);
            sw.WriteLine(window.Height);
        }
        /// <summary>
        /// Sets window position
        /// </summary>
        /// <param name="x">X position in hexes</param>
        /// <param name="y">Y position in hexes</param>
        public void setWindow(int x, int y)
        {
            window.X = x;
            if (window.X < -(window.Width - 1))
            {
                window.X = -(window.Width - 1);
            }
            if (window.X > (grid.Width - 1) + window.Width)
            {
                window.X = (grid.Width - 1) + window.Width;
            }
            window.Y = y;
            if (window.Y < -window.Height)
            {
                window.Y = -window.Height;
            }
            if (window.Y > (grid.Height - 1) + window.Height)
            {
                window.Y = (grid.Height - 1) + window.Height;
            }
        }
        /// <summary>
        /// Moves window
        /// </summary>
        /// <param name="x">X shift value in pixels</param>
        /// <param name="y">Y shift value in pixels</param>
        public void moveWindow(float x, float y)
        {
            window.X += x;
            window.Y += y;
        }
        /// <summary>
        /// Draws SCFog
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            for(int x = (int)window.X; x < (int)window.X + window.Width; x++)
            {
                for(int y = (int)window.Y; y < (int)window.Y + window.Height; y++)
                {
                    if(grid.inDomain(x, y) == true)
                    {
                        if(grid.Grid[x, y] == true)
                        {
                            destRect.X = ((int)window.X * 32) - 2;
                            destRect.Y = ((int)window.Y * 32) - 2;
                            SimpleDraw.draw(renderer, source, srcRect, destRect);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Defogs a space
        /// </summary>
        /// <param name="hx">Center hex X value</param>
        /// <param name="hy">Center hex Y value</param>
        /// <param name="stamp">Stamp reference</param>
        public void defog(int hx, int hy, SCStamp stamp)
        {
            //center tile
            if (grid.inDomain(hx, hy) == true)
            {
                grid.Grid[hx, hy] = false;
            }
            //one tile up
            if (grid.inDomain(hx, hy - 1) == true)
            {
                
                grid.Grid[hx, hy - 1] = false;
            }
            //one tile down
            if (grid.inDomain(hx, hy + 1) == true)
            {
                grid.Grid[hx, hy + 1] = false;
            }
            //one tile left
            if (grid.inDomain(hx - 1, hy) == true)
            {
                grid.Grid[hx - 1, hy] = false;
            }
            //one tile right
            if (grid.inDomain(hx + 1, hy) == true)
            {
                grid.Grid[hx + 1, hy] = false;
            }
            //one tile left, down one
            if (grid.inDomain(hx - 1, hy + 1) == true)
            {
                grid.Grid[hx - 1, hy + 1] = false;
            }
            //one tile right, down one
            if (grid.inDomain(hx + 1, hy + 1) == true)
            {
                grid.Grid[hx + 1, hy + 1] = false;
            }
            //one tile left, up one
            if (grid.inDomain(hx - 1, hy - 1) == true)
            {
                grid.Grid[hx - 1, hy + 1] = false;
            }
            //one tile right, up one
            if (grid.inDomain(hx + 1, hy - 1) == true)
            {
                grid.Grid[hx + 1, hy + 1] = false;
            }
        }
        /// <summary>
        /// FogGrid property
        /// </summary>
        public DataGrid<bool> FogGrid
        {
            get { return grid; }
        }
    }
}
