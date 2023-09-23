//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// Timer class
    /// </summary>
    public class Timer
    {
        //protected
        protected int hours;
        protected int minutes;
        protected int seconds;
        protected DateTime start;

        //public
        /// <summary>
        /// Timer constructor
        /// </summary>
        /// <param name="hours">Hours</param>
        /// <param name="minutes">Minutes</param>
        /// <param name="seconds">Seconds</param>
        public Timer(int hours = 0, int minutes = 0, int seconds = 0)
        {
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
            start = DateTime.Now;
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        public void update()
        {
            DateTime temp = DateTime.Now;
            if(temp.Second != start.Second)
            {
                if (temp.Second + seconds > 59)
                {
                    minutes++;
                    seconds = (temp.Second + seconds) - 59;
                }
                else
                {
                    seconds += temp.Second;
                }
                if (minutes > 59)
                {
                    hours++;
                    minutes = minutes - 59;
                }
                start = DateTime.Now;//set the start point to now
            }
        }
        /// <summary>
        /// Draws Timer
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="colour">SDL_Color</param>
        public void draw(IntPtr renderer, int x, int y, SDL.SDL_Color colour)
        {
            string str = hours.ToString() + ": " + minutes.ToString() + ": " + seconds.ToString();
            SpriteFont.draw(renderer, str, x, y, colour);
        }
        /// <summary>
        /// Save data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void save(System.IO.StreamWriter sw)
        {
            sw.WriteLine(hours.ToString());
            sw.WriteLine(minutes.ToString());
            sw.WriteLine(seconds.ToString());
        }
        /// <summary>
        /// Hours property
        /// </summary>
        public int Hours
        {
            get { return hours; }
            set
            {
                if (value > 999)
                {
                    hours = 999;
                }
                else if (value < 0)
                {
                    hours = 0;
                }
                else
                {
                    hours = value;
                }
            }
        }
        /// <summary>
        /// Minutes property
        /// </summary>
        public int Minutes
        {
            get { return minutes; }
            set
            {
                if (value > 999)
                {
                    minutes = 59;
                }
                else if (value < 0)
                {
                    minutes = 0;
                }
                else
                {
                    minutes = value;
                }
            }
        }
        /// <summary>
        /// Seconds property
        /// </summary>
        public int Seconds
        {
            get { return seconds; }
            set
            {
                if (value > 59)
                {
                    Seconds = 59;
                }
                else if (value < 0)
                {
                    Seconds = 0;
                }
                else
                {
                    Seconds = value;
                }
            }
        }
    }
}
