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
    public enum facing4 {u, r, d, l};
    /// <summary>
    /// class to handle sprite sheet with four frames of animation and four facings 
    /// </summary>
    public class Sprite4x4
    {
        //varibles
        Texture2D image;
        int frame;
        int width;
        int height;
        SDL.SDL_Rect box1;//destination rectangle
        SDL.SDL_Rect box2;//source rectangle
        bool still;
        Counter delay;
        /// <summary>
        /// Sprite4x4 construtor
        /// </summary>
        /// <param name="img">Texture2D reference</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="delayTime">Delay time between frame updates</param>
        public Sprite4x4(Texture2D img, int w, int h, int x = 0, int y = 0, int delayTime = 0)
        {
            image = img;
            width = w;
            height = h;
            frame = 0;
            box1.x = x;
            box1.y = y;
            box1.w = width / 4;
            box1.h = height / 4;
            box2.x = 0;
            box2.y = 0;
            box2.w = width / 4;
            box2.h = height / 4; 
            still = false;
            delay = new Counter(delayTime);
        }
        /// <summary>
        /// draws 1/16 of source image, does not calculate relative to window
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="facing">Sprite facing</param>
        public void draw(IntPtr renderer, int x, int y, facing4 facing = facing4.u)
        {
            box1.x = x;
            box1.y = y;
            if (delay.tick())
            {
                box2.x = frame * (width / 4);
            }
            box2.y = (int)facing * (height / 4);
            SDL.SDL_RenderCopy(renderer, image.texture, ref box2, ref box1);
            if (!still)
            {
                if (frame < 3)
                {
                    frame++;
                }
                else
                {
                    frame = 0;
                }
            }
        }
        /// <summary>
        /// draws the entire sprite source
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public void draw(IntPtr renderer, int x, int y)
        {
            box1.x = x;
            box1.y = y;
            SDL.SDL_RenderCopy(renderer, image.texture, ref box2, ref box1);
            if (!still)
            {
                if (frame < 3)
                {
                    frame++;
                }
                else
                {
                    frame = 0;
                }
            }
        }
        /// <summary>
        /// Still property
        /// </summary>
        public bool Still
        {
            get { return still; }
            set { still = value; }
        }
        /// <summary>
        /// Width property
        /// </summary>
        public int Width
        {
            get { return image.width / 4; }
        }
        /// <summary>
        /// Height property
        /// </summary>
        public int Height
        {
            get { return image.height / 4; }
        }
    }
}
