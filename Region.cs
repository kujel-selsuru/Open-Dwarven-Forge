//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework;

namespace XenoLib
{
    /// <summary>
    /// Region class
    /// </summary>
    public class Region
    {
        //protected
        protected int width;
        protected int height;
        protected int winx;
        protected int winy;
        protected int winwidth;
        protected int winheight;
        protected Texture2D background;
        protected Rectangle box;
        protected Rectangle destRect;
        protected Rectangle sourceRect;
        protected Point2D scale;

        //public
        /// <summary>
        /// Region constructor
        /// </summary>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="winx">Window x initial position</param>
        /// <param name="winy">Window y initial position</param>
        /// <param name="winwidth">Window width in pixels</param>
        /// <param name="winheight">Window height in pixels</param>
        /// <param name="background">Texture2D reference</param>
        public Region(int width, int height, int winx, int winy, int winwidth, int winheight, 
            Texture2D background)
        {
            this.width = width;
            this.height = height;
            this.winx = winx;
            this.winy = winy;
            this.winwidth = winwidth;
            this.winheight = winheight;
            this.background = background;
            scale = new Point2D(width/background.width, height/background.height);
            box = new Rectangle(winx, winy, winwidth, winheight);
            destRect = new Rectangle(0, 0, winwidth, winheight);
            sourceRect = new Rectangle(0, 0, scale.X, scale.Y);
        }
        /// <summary>
        /// move the window by the value of x and y, 
        /// where x and y can either be positive or negative values
        /// </summary>
        /// <param name="x">change in x value</param>
        /// <param name="y">change in y value</param>
        public void move_win(int x, int y)
        {
            if (winx + winwidth + x <= width && winx + x >= 0)
            {
                winx += x;
            }
            if (winy + winheight + y <= height && winy + y >= 0)
            {
                winy += y;
            }
            box.X = winx;
            box.Y = winy;
            sourceRect.X = scale.X * winx;
            sourceRect.Y = scale.Y * winy;
        }
        /// <summary>
        /// Winx Property
        /// </summary>
        public int Winx
        {
            get { return winx; }
            set
            {
                if (winx + winwidth < width)
                {
                    winx = value;
                }
                else
                {
                    winx = width - winwidth;
                }
            }
        }
        /// <summary>
        /// Winy Property
        /// </summary>
        public int Winy
        {
            get { return winy; }
            set
            {
                if (winy + winheight < height)
                {
                    winy = value;
                }
                else
                {
                    winy = height - winheight;
                }
            }
        }
        /// <summary>
        /// Width Property
        /// </summary>
        public int Width
        {
            get{ return width; }
        }
        /// <summary>
        /// Height Property
        /// </summary>
        public int Height
        {
            get { return height; }
        }
        /// <summary>
        /// WinWidth Property
        /// </summary>
        public int Winwidth
        {
            get { return winwidth; }
            set { winwidth = value; }
        }
        /// <summary>
        /// WinHeight Property
        /// </summary>
        public int Winheight
        {
            get { return winheight; }
            set { winheight = value; }
        }
        /// <summary>
        /// Box Property
        /// </summary>
        public Rectangle Box
        {
            get { return box; }
            set { box = value; }
        }
        /// <summary>
        /// test if an object with a box is inside the region's window
        /// </summary>
        /// <param name="rect">Rectangle reference</param>
        /// <returns>Boolean</returns>
        public bool collides(Rectangle rect)
        {
            if (box.intersects(rect))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// draws the portion of the background in the window 
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void drawbase(IntPtr renderer)
        {
            SimpleDraw.draw(renderer, background, sourceRect, destRect);
            //draw the background before anything else
            //spriteBatch.Begin(SpriteSortMode.BackToFront, null);
            //spriteBatch.Draw(background, destRect, sourceRect, Color.White);
            //spriteBatch.End();
        }
        /// <summary>
        /// Checks if a point is inside the bounds of the visible window
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Boolean</returns>
        public bool isPointInWindow(int x, int y)
        {
            if ((x >= winx && x <= winx + winwidth) && (y >= winy && y <= winy + winheight))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Scale property
        /// </summary>
        public Point2D Scale
        {
            get { return scale; }
            //set { scale = value; }
        }
    }
}
