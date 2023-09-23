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
    /// Animations class
    /// </summary>
    public class Animation
    {
        //protected
        protected int x;
        protected int y;
        protected int frames;
        protected int currFrame;
        protected Texture2D source;
        protected SDL.SDL_Rect sourceBox;
        protected SDL.SDL_Rect destBox;
        protected bool loops;
        protected bool done;

        //public
        /// <summary>
        /// Animation constructor
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="frames">Number of frames</param>
        /// <param name="source">Source Texture2D reference</param>
        /// <param name="loops">Loops flag</param>
        /// <param name="inflate">Inflation scaler</param>
        public Animation(int x, int y, int frames, Texture2D source, bool loops = false, float inflate = 1)
        {
            this.x = x;
            this.y = y;
            this.frames = frames;
            this.source = source;
            currFrame = 0;
            sourceBox.x = 0;
            sourceBox.y = 0;
            sourceBox.w = source.width / frames;
            sourceBox.h = source.height;
            destBox.x = x;
            destBox.y = y;
            destBox.w = (int)(inflate * (source.width / frames));
            destBox.h = (int)(inflate * source.height);
            this.loops = loops;
            done = false;
        }
        /// <summary>
        /// Draws animation 
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        public void draw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            destBox.x = x - winx;
            destBox.y = y - winy;
            SDL.SDL_RenderCopy(renderer, source.texture, ref sourceBox, ref destBox);
            if (currFrame < frames)
            {
                if (currFrame < frames)
                {
                    currFrame++;
                }
                else if (loops)
                {
                    currFrame = 0;
                }
                else
                {
                    done = true;
                }
            }
            sourceBox.x = currFrame * (source.width / frames);
        }
        /// <summary>
        /// Advances the frame of animation without rendering anything
        /// </summary>
        public void advance()
        {
            if (currFrame < frames)
            {
                if (currFrame < frames)
                {
                    currFrame++;
                }
                else if (loops)
                {
                    currFrame = 0;
                }
                else
                {
                    done = true;
                }
            }
        }
        /// <summary>
        /// Done property
        /// </summary>
        public bool Done
        {
            get { return done; }
            set { done = value; }
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
    }
}
