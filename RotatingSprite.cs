//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework;

namespace XenoLib
{
    /// <summary>
    /// draws a top down view style sprite, also handles hit box and can rotate the output frame;
    /// </summary>
    public class RotatingSprite : SimpleSprite
    {
        //protected
        protected float angle;
        protected Point2D boxCenter;
        
        //public
        /// <summary>
        /// RotatingSprite constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="still">Still flag value</param>
        /// <param name="angle">Initial angle value</param>
        /// <param name="delay">Frame delay value</param>
        public RotatingSprite(Texture2D source, int x, int y, int numFrames, bool still, float angle, int delay = 6)
            : base(source, x, y, numFrames, simpleFacing.up, still, delay)
        {
            this.angle = angle;
            sourceBox.h = source.height;
            destBox.h = source.height;
            hitBox.h = source.height;
            height = source.height;
            boxCenter = new Point2D(destBox.w/2, destBox.h/2);
        }
        /// <summary>
        /// Draw RotatingSprite
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        public new void draw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            destBox.x = hitBox.x - winx;
            destBox.y = hitBox.y - winy;
            SimpleDraw.draw(renderer, source, sourceBox, destBox, angle, boxCenter, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
            //sb.Draw(source, destBox, sourceBox, Color.White, angle, boxCenter, SpriteEffects.None, 0f);
            if (!still)
            {
                if (frameDelay.tick())
                {
                    if (frame < numFrames - 1)
                    {
                        frame++;
                    }
                    else
                    {
                        frame = 0;
                    }
                }
            }
            sourceBox.x = sourceBox.w * frame;
        }
        /// <summary>
        /// Angle property
        /// </summary>
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }
    }
}
