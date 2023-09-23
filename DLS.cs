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
    /// duel layer sprite
    /// </summary>
    public class DLS : SimpleSprite
    {
        //protected
        protected Texture2D topImg;
        protected Rectangle top;
        protected Rectangle topBox;
        protected int shift;

        /// <summary>
        /// DuelLayerSprite constructor
        /// </summary>
        /// <param name="source">Source Texture2D reference</param>
        /// <param name="topSource">TopSource Texture2D reference</param>
        /// <param name="shift">Top image shift value</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="numFrames">Number of frames</param>
        public DLS(Texture2D source, Texture2D topSource, int shift, int x, int y, int numFrames)
            : base(source, x, y, numFrames, simpleFacing.up, true)
        {
            topImg = topSource;
            top = new Rectangle(x, y - shift, topSource.width / numFrames, topSource.height / 4);
            topBox = new Rectangle(0, 0, topSource.width / numFrames, topSource.height / 4);
            this.shift = shift;
        }
        /// <summary>
        /// Draws DuelLayerSprite
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        public new void draw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            topBox.X = frame * top.Width;
            base.draw(renderer, winx, winy);
            topBox.Y = (((int)facing) * top.Height);
            top.X = hitBox.x - winx;
            top.Y = hitBox.y - winy - shift;
            SDL.SDL_Rect topSrcBox = top.Rect;
            SDL.SDL_Rect topDestBox = topBox.Rect;
            SDL.SDL_RenderCopy(renderer, topImg.texture, ref topSrcBox, ref topDestBox);
            //sb.Draw(topImg, top, topBox, Color.White);
        }
    }
}
