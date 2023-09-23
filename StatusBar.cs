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
    /// StatusBar class
    /// </summary>
    public class StatusBar
    {
        //protected 
        protected Texture2D source;
        protected int max;
        protected int current;
        protected float percent;
        protected int length;
        protected int x;
        protected int y;

        //public
        /// <summary>
        /// StatusBar constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="max">Max value</param>
        /// <param name="current">Current value</param>
        /// <param name="length">Length of status bar</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public StatusBar(Texture2D source, int max, int current, int length, int x, int y)
        {
            this.source = source;
            this.max = max;
            this.current = current;
            percent = 1.0f;
            this.length = length;
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// Update internal state
        /// </summary>
        /// <param name="nCurrent">New current value</param>
        public void update(int nCurrent)
        {
            current = nCurrent;
            percent = (float)current / (float)max;
        }
        /// <summary>
        /// Draw StatusBar
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="colour">Colour of bar</param>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        public void draw(IntPtr renderer, SDL.SDL_Color colour, int winx = 0, int winy = 0)
        {
            float temp = percent * length;
            DrawRects.drawRect(renderer, new Rectangle(x - winx, y - winy, temp, 3), colour, true);
            //DrawLine.draw(source.texture, new Point2D(x - winx, y - winy), new Point2D((int)(x + temp) - winx, y - winy), colour);
            //DrawLine.draw(source.texture, new Point2D(x - winx, y - winy + 1), new Point2D((int)(x + temp) - winx, y - winy + 1), colour);
            //DrawLine.draw(source.texture, new Point2D(x - winx, y - winy + 2), new Point2D((int)(x + temp) - winx, y - winy + 2), colour);
            //Line.Draw(sb, source, new GLIB.Point(x - winx, y - winy), new GLIB.Point((int)(x + temp) - winx, y - winy), colour, width);
        }
        /// <summary>
        /// Sets a new max and current back to zero
        /// </summary>
        public void reset(int max = 100)
        {
            this.max = max;
            current = 0;
        }
        /// <summary>
        /// X property
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        /// <summary>
        /// Y property
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        /// <summary>
        /// Max property
        /// </summary>
        public int Max
        {
            get { return max; }
            set { max = value; }
        }
        /// <summary>
        /// Length property
        /// </summary>
        public int Length
        {
            get { return length; }
            set { length = value; }
        }
    }

    /// <summary>
    /// StatusBar2 class (does vertical and horizontal)
    /// </summary>
    public class StatusBar2
    {
        //protected 
        protected SDL.SDL_Color colour;
        protected int max;
        protected int current;
        protected float percent;
        protected int length;
        protected int x;
        protected int y;
        protected bool vertical;
        protected Rectangle box;

        //public
        /// <summary>
        /// StatusBar constructor
        /// </summary>
        /// <param name="max">Max value</param>
        /// <param name="current">Current value</param>
        /// <param name="length">Length of status bar</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="colour">Colour of status bar</param>
        /// <param name="vertical">Vertical flag value</param>
        public StatusBar2(int max, int current, int length, int x, int y, SDL.SDL_Color colour, bool vertical)
        {
            this.colour = colour;
            this.max = max;
            this.current = current;
            percent = 1.0f;
            this.length = length;
            this.x = x;
            this.y = y;
            this.vertical = vertical;
            box = new Rectangle(x, y, 3, 3);
        }
        /// <summary>
        /// Update internal state
        /// </summary>
        /// <param name="nCurrent">New current value</param>
        public void update(int nCurrent)
        {
            current = nCurrent;
            percent = (float)current / (float)max;
        }
        /// <summary>
        /// Draw StatusBar2
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="thickness">Thickness of statusbar</param>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        public void draw(IntPtr renderer, int thickness = 3, int winx = 0, int winy = 0)
        {
            float temp = percent * length;
            if(vertical)
            {
                box.Height = temp;
                box.Width = thickness;
                box.X = x - winx;
                box.Y = y - winy + (length - (percent * length));
                DrawRects.drawRect(renderer, box, colour, true);
            }
            else
            {
                box.Width = temp;
                box.Height = thickness;
                box.X = x - winx;
                box.Y = y - winy;
                DrawRects.drawRect(renderer, box, colour, true);
            }
            //DrawLine.draw(source.texture, new Point2D(x - winx, y - winy), new Point2D((int)(x + temp) - winx, y - winy), colour);
            //DrawLine.draw(source.texture, new Point2D(x - winx, y - winy + 1), new Point2D((int)(x + temp) - winx, y - winy + 1), colour);
            //DrawLine.draw(source.texture, new Point2D(x - winx, y - winy + 2), new Point2D((int)(x + temp) - winx, y - winy + 2), colour);
            //Line.Draw(sb, source, new GLIB.Point(x - winx, y - winy), new GLIB.Point((int)(x + temp) - winx, y - winy), colour, width);
        }
        /// <summary>
        /// X property
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        /// <summary>
        /// Y property
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        /// <summary>
        /// Max property
        /// </summary>
        public int Max
        {
            get { return max; }
            set { max = value; }
        }
        /// <summary>
        /// Length property
        /// </summary>
        public int Length
        {
            get { return length; }
            set { length = value; }
        }
    }
}
