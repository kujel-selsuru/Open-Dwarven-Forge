//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

namespace XenoLib
{
    /// <summary>
    /// DataLineGraph class
    /// </summary>
    public class DataLineGraph
    {
        //protected
        protected int scalex;
        protected int scaley;
        protected Texture2D pixel;
        protected List<Point2D> data;
        protected Rectangle box;
        protected SDL.SDL_Rect boxSrc;

        //public
        /// <summary>
        /// DataLineGraph constructor
        /// </summary>
        /// <param name="scalex">Scaler X value</param>
        /// <param name="scaley">Scaler Y value</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Background width</param>
        /// <param name="height">Background height</param>
        /// <param name="pixel">Background Texture2D reference</param>
        /// <param name="dataFile">Data file path/name to build graph from</param>
        public DataLineGraph(int scalex, int scaley, int x, int y, int width, int height, Texture2D pixel, string dataFile)
        {
            this.scalex = scalex;
            this.scaley = scaley;
            box = new Rectangle(x, y, width, height);
            boxSrc.x = x;
            boxSrc.y = y;
            boxSrc.x = width;
            boxSrc.y = height;
            this.pixel = pixel;
            data = new List<Point2D>();
            System.IO.StreamReader sr = new System.IO.StreamReader(dataFile);
            int entries = Convert.ToInt32(sr.ReadLine());
            for (int e = 0; e < entries; e++)
            {
                data.Add(new Point2D(Convert.ToInt32(sr.ReadLine(), Convert.ToInt32(sr.ReadLine()))));
            }
            tweakData();
        }
        /// <summary>
        /// Scales data to fit graph
        /// </summary>
        protected void tweakData()
        {
            for (int d = 0; d < data.Count; d++)
            {
                data[d].X = box.X + (data[d].X * scalex);
                data[d].Y = box.Y + (data[d].Y * scaley);
            }
        }
        /// <summary>
        /// Draws DataLineGraph
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="colour">Line colour</param>
        public void draw(IntPtr renderer, SDL.SDL_Color colour)
        {
            SDL.SDL_Rect boxDest = box.Rect;
            SDL.SDL_RenderCopy(renderer, pixel.texture, ref boxSrc, ref boxDest);
            for (int d = 1; d < data.Count; d++)
            {
                if (d == 1)
                {
                    DrawLine.draw(renderer, data[0], data[d], colour);
                }
                else
                {
                    DrawLine.draw(renderer, data[d - 1], data[d], colour);
                }
            }
        }
    }
}
