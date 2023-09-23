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
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Graphics;

namespace XenoLib
{
    /// <summary>
    /// WireFrameMap class
    /// </summary>
    public class WireFrameMap
    {
        //protected
        protected Point2D scale;
        protected Point2D pos;
        protected Rectangle boarder;
        protected Rectangle window;
        protected SDL.SDL_Color white;

        //public
        /// <summary>
        /// WireFrameMap constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="ww">Window width</param>
        /// <param name="wh">Window height</param>
        /// <param name="rw">Region width</param>
        /// <param name="rh">Region height</param>
        public WireFrameMap(int x, int y, int w, int h, int ww, int wh, int rw, int rh)
        {
            scale = new Point2D(rw / w, rh / h);
            pos = new Point2D((boarder.X - window.X) * scale.X, (boarder.Y - window.Y) * scale.Y);
            boarder = new Rectangle(x, y, w, h);
            window = new Rectangle(x, y, ww, wh);
            white.r = 255;
            white.g = 255;
            white.b = 255;
            white.a = 1;
        }
        /// <summary>
        /// update internal state
        /// </summary>
        /// <param name="mBox">Rectangle reference</param>
        public void update(Rectangle mBox)
        {
            if (MouseHandler.getLeft())
            {
                if (boarder.intersects(mBox))
                {
                    window.X = mBox.X;
                    if (window.X < boarder.X)
                    {
                        window.X = boarder.X;
                    }
                    if (window.X + window.Width > boarder.X + boarder.Width)
                    {
                        window.X = boarder.X + boarder.Width - window.Width;
                    }
                    window.Y = mBox.Y;
                    if (window.Y < boarder.Y)
                    {
                        window.Y = boarder.Y;
                    }
                    if (window.Y + window.Height > boarder.Y + boarder.Height)
                    {
                        window.Y = boarder.Y + boarder.Height - window.Height;
                    }
                    pos.X = (window.X - boarder.X) * scale.X;
                    pos.Y = (window.Y - boarder.Y) * scale.Y;
                }
            }
        }
        /// <summary>
        /// Draws WireFrameMap
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            DrawRects.drawRect(renderer, boarder, white, false);
            //Line.drawSquare(sb, pixel, boarder.X, boarder.Y, boarder.Width, boarder.Height, Color.White, 2);
            DrawRects.drawRect(renderer, window, white, false);
            //Line.drawSquare(sb, pixel, window.X, window.Y, window.Width, window.Height, Color.White, 2);
        }
        /// <summary>
        /// Shifts window
        /// </summary>
        /// <param name="x">X shift value</param>
        /// <param name="y">Y shift value</param>
        public void shiftWindow(int x, int y)
        {
            window.X += x;
            window.Y += y;
            pos.X = (window.X - boarder.X) * scale.X;
            pos.Y = (window.Y - boarder.Y) * scale.Y;
        }
        /// <summary>
        /// Pos property
        /// </summary>
        public Point2D Pos
        {
            get { return pos; }
            set { pos = value; }
        }
        /// <summary>
        /// Window width property
        /// </summary>
        public float WW
        {
            get { return window.Width; }
        }
        /// <summary>
        /// Window height property
        /// </summary>
        public float WH
        {
            get { return window.Height; }
        }
        /// <summary>
        /// Boarder width property
        /// </summary>
        public float BW
        {
            get { return boarder.Width; }
        }
        /// <summary>
        /// Boarder height property
        /// </summary>
        public float BH
        {
            get { return boarder.Height; }
        }
        /// <summary>
        /// Scale property
        /// </summary>
        public Point2D Scale
        {
            get { return scale; }
        }
    }
}
