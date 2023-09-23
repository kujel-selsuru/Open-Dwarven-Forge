//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// LTimer class
    /// </summary>
    public class LTimer
    {
        //protected
        protected uint mStartTicks;
        protected uint mPausedTicks;
        protected bool mStarted;
        protected bool mPaused;

        //public
        /// <summary>
        /// LTimer constructor
        /// </summary>
        public LTimer()
        {
            mStartTicks = 0;
            mPausedTicks = 0;
            mStarted = false;
            mPaused = false;
        }
        /// <summary>
        /// Start timer
        /// </summary>
        public void start()
        {
            mStarted = true;
            mPaused = false;
            mStartTicks = SDL.SDL_GetTicks();
            mPausedTicks = 0;
        }
        /// <summary>
        /// Pause timer
        /// </summary>
        public void pause()
        {
            if (mStarted && !mPaused)
            {
                mPaused = true;
                mPausedTicks = SDL.SDL_GetTicks() - mStartTicks;
                mStartTicks = 0;
            }
        }
        /// <summary>
        /// Unpause timer
        /// </summary>
        public void unpause()
        {
            if (mPaused && mStarted)
            {
                mPaused = false;
                mStartTicks = SDL.SDL_GetTicks() - mPausedTicks;
                mPausedTicks = 0;
            }
        }
        /// <summary>
        /// Stop timer
        /// </summary>
        public void stop()
        {
            mStarted = false;
            mPaused = false;
            mStartTicks = 0;
            mPausedTicks = 0;
        }
        /// <summary>
        /// Returns timer ticks
        /// </summary>
        /// <returns>Unit</returns>
        public uint getTicks()
        {
            uint time = 0;
            if (mStarted)
            {
                if (mPaused)
                {
                    time = mPausedTicks;
                }
                else
                {
                    time = SDL.SDL_GetTicks() - mStartTicks;
                }
            }
            return time;
        }
        /// <summary>
        /// IsStarted property
        /// </summary>
        /// <returns></returns>
        public bool IsStarted
        {
            get { return mStarted; }
        }
        /// <summary>
        /// IsPaused property
        /// </summary>
        public bool IsPaused
        {
            get { return mPaused; }
        }
    }
}
