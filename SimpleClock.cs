//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenoLib
{
    public class SimpleClock
    {
        //protected
        protected int framesPerSecond;
        protected int frames;
        protected int seconds;
        protected int minutes;
        protected int hours;
        protected Point2D position;
        //public
        /// <summary>
        /// SimpleClock constructor
        /// </summary>
        /// <param name="framesPerSecond">Base measure of time internally in SimpleClock</param>
        public SimpleClock(int framesPerSecond = 60)
        {
            this.framesPerSecond = framesPerSecond;
            frames = 0;
            seconds = 0;
            minutes = 0;
            hours = 0;
            position = new Point2D(0, 0);
        }
        /// <summary>
        /// SimpleClock copy constructor
        /// </summary>
        /// <param name="sc">Simple clock reference</param>
        public SimpleClock(SimpleClock sc)
        {
            framesPerSecond = sc.FramesPerSecond;
            frames = sc.Frames;
            seconds = sc.Seconds;
            minutes = sc.Minutes;
            hours = sc.Hours;
            position = new Point2D(0, 0);
        }
        /// <summary>
        /// SimpleClock from file constructor
        /// </summary>
        /// <param name="sr">String writer reference</param>
        public SimpleClock(System.IO.StreamReader sr)
        {
            sr.ReadLine();
            framesPerSecond = Convert.ToInt32(sr.ReadLine());
            frames = Convert.ToInt32(sr.ReadLine());
            seconds = Convert.ToInt32(sr.ReadLine());
            minutes = Convert.ToInt32(sr.ReadLine());
            hours = Convert.ToInt32(sr.ReadLine());
            position = new Point2D(0, 0);
        }
        /// <summary>
        /// Update internal state
        /// </summary>
        public virtual void update()
        {
            frames++;
            if (frames >= framesPerSecond)
            {
                frames = 0;
                seconds++;
                if (seconds >= 59)
                {
                    seconds = 0;
                    minutes++;
                    if (minutes >= 59)
                    {
                        hours++;
                    }
                }
            }
        }
        /// <summary>
        /// Draw the clock's hour and minute count
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="colour">Colour of text</param>
        /// <param name="renderer">renderer reference</param>
        /// <param name="scaler">text scaling value</param>
        public virtual void draw(int x, int y, string colour, IntPtr renderer, float scaler = 1)
        {
            position.X = x;
            position.Y = 0;
            //sb.DrawString(font, hours + ":" + minutes, position, colour);
            SimpleFont.drawColourString(renderer, hours + ": " + minutes, x, y, colour);
        }
        /// <summary>
        /// Draw the clock's hour, minute and second count
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="colour">Colour of text</param>
        /// <param name="renderer">renderer reference</param>
        /// <param name="scaler">text scaling value</param>
        public virtual void draw2(int x, int y, string colour, IntPtr renderer, float scaler = 1)
        {
            position.X = x;
            position.Y = 0;
            //sb.DrawString(font, hours + ":" + minutes, position, colour);
            SimpleFont.drawColourString(renderer, hours + ": " + minutes + ": " + seconds, x, y, colour);
        }
        /// <summary>
        /// Save data
        /// </summary>
        /// <param name="sw">String writer reference</param>
        public virtual void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======SimpleClock Data======");
            sw.WriteLine(framesPerSecond);
            sw.WriteLine(frames);
            sw.WriteLine(seconds);
            sw.WriteLine(minutes);
            sw.WriteLine(hours);
        }
        /// <summary>
        /// FramesPerSecond property
        /// </summary>
        public int FramesPerSecond
        {
            get { return framesPerSecond; }
            set { framesPerSecond = value; }
        }
        /// <summary>
        /// FramesPerSecond property
        /// </summary>
        public int Frames
        {
            get { return frames; }
            set { frames = value; }
        }
        /// <summary>
        /// Seconds property
        /// </summary>
        public int Seconds
        {
            get { return seconds; }
            set { seconds = value; }
        }
        /// <summary>
        /// Minutes property
        /// </summary>
        public int Minutes
        {
            get { return minutes; }
            set { minutes = value; }
        }
        /// <summary>
        /// Hours property
        /// </summary>
        public int Hours
        {
            get { return hours; }
            set { hours = value; }
        }
    }
}
