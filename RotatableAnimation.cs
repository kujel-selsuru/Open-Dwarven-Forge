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
    /// RotatableAnimation
    /// </summary>
    public class RotatableAnimation
    {
        //protected
        protected Texture2D[] sources;
        protected float angle;
        protected int frame;
        protected int maxFrames;
        protected Rectangle box;
        protected Rectangle destination;
        protected Point2D origin;
        protected bool done;

        //public
        /// <summary>
        /// RotatableAnimation constructor
        /// </summary>
        /// <param name="sourceTexs">array of Texture2D references</param>
        /// <param name="angle">angle of rotation</param>
        /// <param name="maxFrames">Max frames</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public RotatableAnimation(Texture2D[] sourceTexs, float angle, int maxFrames, int x, int y)
        {
            sources = new Texture2D[sourceTexs.Length];
            for (int i = 0; i < sourceTexs.Length; i++)
            {
                sources[i] = sourceTexs[i];
            }
            this.angle = angle;
            this.maxFrames = maxFrames;
            frame = 0;
            box = new Rectangle(0, 0, sources[0].width, sources[0].height);
            destination = new Rectangle(x, y, sources[0].width, sources[0].height);
            origin = new Point2D(sources[0].width / 2,sources[0].height / 2);
            done = false;
        }
        /// <summary>
        /// Draws RotatableAnimation
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        public void draw(IntPtr renderer, int x, int y, int winx = 0, int winy = 0)
        {
            destination.X = x - winx;
            destination.Y = y - winy;
            SimpleDraw.draw(renderer, sources[frame], box, destination, angle, origin, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
            //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //sb.Draw(sources[frame], destination, box, Color.White, angle, origin, SpriteEffects.None, 0f);
            //sb.End();
            if (frame < maxFrames)
            {
                frame++;
            }
            else
            {
                done = true;
            }
        }
        /// <summary>
        /// add value to current angle
        /// </summary>
        /// <param name="angle">Angle value to add</param>
        public void rotate(float angle)
        {
            this.angle += angle;
        }
        /// <summary>
        /// Resets value to zero
        /// </summary>
        public void resetAngle()
        {
            angle = 0;
        }
        /// <summary>
        /// Done property
        /// </summary>
        public bool Done
        {
            get { return done; }
        }
        /// <summary>
        /// X property
        /// </summary>
        public float X
        {
            get { return destination.X; }
            set { destination.X = value; }
        }
        /// <summary>
        /// Y property
        /// </summary>
        public float Y
        {
            get { return destination.Y; }
            set { destination.Y = value; }
        }
    }
}
