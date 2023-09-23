//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using SDL2;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;

namespace XenoLib
{
    /// <summary>
    /// A class for handling animations displayed on screen
    /// </summary>
    public static class AnimationEngine
    {
        //private
        static List<Animation> animations;
        //public
        public static Rectangle window;

        //private
        static AnimationEngine()
        {
            animations = new List<Animation>();
            window = new Rectangle(0, 0, 640, 480);
        }
        //public
        /// <summary>
        /// Add a animation to be run
        /// </summary>
        /// <param name="image">Animation call name</param>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="inflate">Inflation scaler value</param>
        /// <param name="frames">Number of frames</param>
        /// <param name="loops">Loops flag</param>
        /// <param name="sound">Sound call name</param>
        public static void addAnimation(string image, int x = 0, int y = 0, int inflate = 1, int frames = 8, bool loops = false, string sound = "none")
        {
            animations.Add(new Animation(x, y, frames, TextureBank.getTexture(image), loops, inflate));
            if(sound != "none")
            {
                SFXEngine.addSound(sound);
            }
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        public static void update()
        {
            for (int i = 0; i < animations.Count; i++)
            {
                if (animations[i].Done)
                {
                    animations.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// Draws all active animations
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public static void draw(IntPtr renderer)
        {
            for (int i = 0; i < animations.Count; i++)
            {
                animations[i].draw(renderer, (int)window.X, (int)window.Y);
            }
        }
        /// <summary>
        /// Draws all active animations
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window x value</param>
        /// <param name="winy">Window y value</param>
        /// <param name="winw">Window width in pixels</param>
        /// <param name="winh">Window height in pixels</param>
        /// <param name="tilew">Tile width in pixels</param>
        /// <param name="tileh">Tile height in pixels</param>
        public static void draw(IntPtr renderer, int winx, int winy, int winw, int winh, int tilew = 32, int tileh = 32)
        {
            for (int i = 0; i < animations.Count; i++)
            {
                //make sure animation is within window
                if (animations[i].X >= winx - tilew && animations[i].X < winx + winw + tilew)
                {
                    if (animations[i].Y >= winy - tileh && animations[i].Y < winy + winh + tileh)
                    {
                        animations[i].draw(renderer, winx, winy);
                    }
                    else
                    {
                        animations[i].advance();
                    }
                }
            }
        }
        /// <summary>
        /// Checks if a position is within window domain
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <returns></returns>
        public static bool inWindow(int x, int y)
        {
            if (x >= window.X & x <= window.Width)
            {
                if(y >= window.Y & y <= window.Height)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Window X property
        /// </summary>
        public static int WinX
        {
            get { return (int)window.X; }
            set { window.X = value; }
        }
        /// <summary>
        /// Window Y property
        /// </summary>
        public static int WinY
        {
            get { return (int)window.Y; }
            set { window.Y = value; }
        }
        /// <summary>
        /// Window Width property
        /// </summary>
        public static int WinWidth
        {
            get { return (int)window.Width; }
            set { window.Width = value; }
        }
        /// <summary>
        /// Window Height property
        /// </summary>
        public static int WinHeight
        {
            get { return (int)window.Height; }
            set { window.Height = value; }
        }
    }
}
