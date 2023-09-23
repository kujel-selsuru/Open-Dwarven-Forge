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
    /// SimpleTimer class
    /// </summary>
    public class SimpleTimer
    {
        //protected
        protected int seconds;
        protected int minutes;
        protected int hours;
        protected Counter delay;

        //public
        /// <summary>
        /// SimpleTimer constructor
        /// </summary>
        /// <param name="seconds">Seconds value</param>
        /// <param name="minutes">Minutes value</param>
        /// <param name="hours">Hours value</param>
        /// <param name="delay">Delay value</param>
        public SimpleTimer(int seconds = 0, int minutes = 0, int hours = 0, int delay = 30)
        {
            this.seconds = seconds;
            this.minutes = minutes;
            this.hours = hours;
            this.delay = new Counter(delay);
        }
        /// <summary>
        /// Updates intneral state
        /// </summary>
        public void update()
        {
            if (delay.tick())
            {
                seconds++;
                if (seconds > 59)
                {
                    seconds = 0;
                    minutes++;
                }
                if (minutes > 58)
                {
                    minutes = 0;
                    hours++;
                }
            }
        }
        /// <summary>
        /// Draws SimpleTimer
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="colour">SDL_Color</param>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        public void display(IntPtr renderer, int x, int y, SDL.SDL_Color colour, int winx = 0, int winy = 0)
        {
            SimpleFont.DrawString(renderer, hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString(), x - winx, y - winy, colour);
            //sb.DrawString(font, hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString(), new Vector2(x, y), colour);
        }
        /// <summary>
        /// Seconds property
        /// </summary>
        public int Seconds
        {
            get { return seconds; }
        }
        /// <summary>
        /// Minutes property
        /// </summary>
        public int Minutes
        {
            get { return minutes; }
        }
        /// <summary>
        /// Hours property
        /// </summary>
        public int Hours
        {
            get { return hours; }
        }
        /// <summary>
        /// Delay property
        /// </summary>
        public int Delay
        {
            get { return delay.Max; }
            set { delay.reset(value); }
        }
    }
}
