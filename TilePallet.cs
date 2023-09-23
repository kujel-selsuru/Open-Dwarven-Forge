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
    /// TilePallet class
    /// </summary>
    public class TilePallet
    {
        //protected
        protected Rectangle destBox;
        protected Rectangle tileBox;
        protected Rectangle stamp;
        protected Texture2D source; //32 x 32 each tile, 5 tiles by 7 tiles
        protected SDL.SDL_Color white;

        //public
        /// <summary>
        /// TilePallet constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="source">Texture2D referecne</param>
        /// <param name="tilew">Tile Width</param>
        /// <param name="tileh">Tile Height</param>
        /// <param name="width">Width in tiles</param>
        /// <param name="height">Height in tiles</param>
        public TilePallet(int x, int y, Texture2D source, int tilew = 32, int tileh = 32, int width = 5, int height = 7)
        {
            destBox = new Rectangle(x, y, source.width, source.height);
            tileBox = new Rectangle(0, 0, tilew, tileh);
            stamp = new Rectangle(0, 0, tilew, tileh);
            this.source = source;
            white.r = 255;
            white.g = 255;
            white.b = 255;
            white.a = 1;
        }
        /// <summary>
        /// Checks if mouse over pallet and sets stamp values
        /// </summary>
        /// <param name="msx">Mouse x value</param>
        /// <param name="msy">mouse y value</param>
        /// <param name="stamp">Rectangle reference</param>
        public void attmeptMouseClick(int msx, int msy, out Rectangle stamp)
        {
            Rectangle temp = new Rectangle(msx, msy, 3, 3);
            if (destBox.intersects(temp))
            {
                stamp = getTileBox(msx, msy);
            }
            else
            {
                stamp = new Rectangle(tileBox.X, tileBox.Y, 0, 0);
            }

        }
        /// <summary>
        /// Returns a Rectangle object provided x and y values
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <returns>Rectangle</returns>
        public Rectangle getTileBox(int x, int y)
        {
            tileBox.X = x - destBox.X;
            //format possition into a multiple of the width
            if (tileBox.X % tileBox.Width > 0)
            {
                int temp = (int)(tileBox.X / tileBox.Width);
                tileBox.X = tileBox.Width * temp;
            }
            //format tilebox.x into grid value
            tileBox.X = tileBox.X / tileBox.Width;
            tileBox.Y = y - destBox.Y;
            if (tileBox.Y % tileBox.Height > 0)
            {
                int temp = (int)(tileBox.Y / tileBox.Height);
                tileBox.Y = tileBox.Height * temp;
            }
            tileBox.Y = tileBox.Y / tileBox.Height;
            return tileBox;
        }
        /// <summary>
        /// Draws TilePallet
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            SimpleDraw.draw(renderer, source, destBox);
            //sb.Draw(source, destBox, Color.White);
            Rectangle box = new Rectangle((tileBox.X * 32) + destBox.X, (tileBox.Y * 32) +destBox.Y, tileBox.Width, tileBox.Height);
            DrawRects.drawRect(renderer, box, white);
            //Line.drawSquare(sb, pixel, (tileBox.X * 32) + destBox.X, (tileBox.Y * 32) + destBox.Y, tileBox.Width, tileBox.Height, Color.White, 2);
        }
        /// <summary>
        /// Box poperty
        /// </summary>
        public Rectangle Box
        {
            get { return destBox; }
        }
        /// <summary>
        /// Stamp property
        /// </summary>
        public Rectangle Stamp
        {
            get { return stamp; }
            set { stamp = value; }
        }
        /// <summary>
        /// Source property
        /// </summary>
        public Texture2D Source
        {
            get { return source; }
            set { source = value; }
        }
    }
}
