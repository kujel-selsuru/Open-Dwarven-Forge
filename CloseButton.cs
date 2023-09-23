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
    /// CloseButton class
    /// </summary>
    public class CloseButton
    {
        //protected
        protected Texture2D source;
        protected Rectangle box;
        protected SDL.SDL_Rect destRect;
        protected SDL.SDL_Rect srcRect;

        //public 
        /// <summary>
        /// CloseButton constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public CloseButton(Texture2D source, int x, int y)
        {
            this.source = source;
            box = new Rectangle(x, y, source.width, source.height);
            srcRect.x = 0;
            srcRect.y = 0;
            srcRect.w = source.width;
            srcRect.h = source.height;
        }
        /// <summary>
        /// Update internal state
        /// </summary>
        /// <param name="rect">Mouse pointer Rectangle</param>
        /// <param name="mlb">State of left mouse button depression</param>
        /// <returns>Boolean</returns>
        public bool update(Rectangle rect, bool mlb)
        {
            if (mlb)
            {
                if(box.intersects(rect))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Draw CloseButton
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            SDL.SDL_Rect destRect = box.Rect;
            SDL.SDL_RenderCopy(renderer, source.texture, ref srcRect, ref destRect);
            //sb.Draw(source, box, Color.White);
        }
    }
}
