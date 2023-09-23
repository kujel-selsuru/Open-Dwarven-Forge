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
    /// MessageBoard class
    /// </summary>
    public class MessageBoard
    {
        //protected
        protected Point2D topLeft;
        protected Point2D line;
        protected List<string> msgs;
        protected int max;
        protected Counter delay;

        //public
        /// <summary>
        /// MessageBoard constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="max">Max displayed messages</param>
        /// <param name="ticks">Delay between removing last message</param>
        public MessageBoard(int x, int y, int max = 15, int ticks = 210)
        {
            topLeft = new Point2D(x, y);
            line = new Point2D(x, y);
            msgs = new List<string>();
            this.max = max;
            delay = new Counter(ticks);
        }
        /// <summary>
        /// Add a new message to the list
        /// </summary>
        /// <param name="msg">Message string to add</param>
        public void addMessage(string msg)
        {
            if (msgs.Count == 0)
            {
                delay.reset(delay.Max);
            }
            msgs.Add(msg);
            if (msgs.Count > max)
            {
                msgs.RemoveAt(msgs.Count - 1);
            }
        }
        /// <summary>
        /// Update internal state
        /// </summary>
        public void update()
        {
            if (delay.tick())
            {
                if (msgs.Count > 0)
                {
                    msgs.RemoveAt(0);
                }
            }
        }
        /// <summary>
        /// Draw MessageBoard
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="colour"></param>
        public void draw(IntPtr renderer, SDL.SDL_Color colour)
        {
            line.Y = topLeft.Y;
            for (int i = 0; i < msgs.Count; i++)
            {
                SimpleFont.DrawString(renderer, msgs[i], line.X, line.Y, colour);
                line.Y += 20;
            }
        }
        /// <summary>
        /// Reset message delay counter
        /// </summary>
        /// <param name="ticks">Max ticks</param>
        public void resetDelay(int ticks = 210)
        {
            delay.reset(ticks);
        }
        /// <summary>
        /// Set the position of MessageBoard
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public void setPosition(int x, int y)
        {
            topLeft.X = x;
            topLeft.Y = y;
            line.X = x;
            line.Y = y;
        }
        /// <summary>
        /// Set max messages in board at once
        /// </summary>
        /// <param name="max"></param>
        public void setMax(int max = 10)
        {
            this.max = max;
        }
    }
}
