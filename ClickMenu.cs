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
    /// ClickMenu class
    /// </summary>
    public class ClickMenu
    {
        protected Rectangle[] boxes;
        protected string[] names;
        protected Texture2D source;
        protected SDL.SDL_Rect sourceBox;
        //protected SpriteFont font;
        protected Point2D topLeft;
        protected SDL.SDL_Color colour;
        /// <summary>
        /// ClickMenu constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="names">Arrary of names (as strings)</param>
        /// <param name="colour">Colour of rendered text</param>
        public ClickMenu(Texture2D source, int x, int y, string[] names, SDL.SDL_Color colour)
        {
            this.source = source;
            SDL.SDL_Rect sourceBox;
            sourceBox.x = 0;
            sourceBox.y = 0;
            sourceBox.w = source.width;
            sourceBox.h = source.height;
            //this.font = font;
            topLeft = new Point2D(x, y);
            boxes = new Rectangle[names.Length];
            this.names = new string[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                this.names[i] = names[i];
                boxes[i] = new Rectangle(x + 10, y + i * source.height, source.width, source.height);
            }
            this.colour = colour;
        }
        /// <summary>
        /// Update internal state
        /// </summary>
        /// <param name="rect">Mouse pointer rectangle</param>
        /// <param name="clicked">State of left mouse button depression</param>
        /// <returns>String value of clicked option or "none" if no option clicked</returns>
        public string update(Rectangle rect, bool clicked)
        {
            if (!clicked)
            {
                return "none";
            }
            else
            {
                for (int i = 0; i < boxes.Length; i++)
                {
                    if (boxes[i].intersects(rect))
                    {
                        return names[i];
                    }
                }
                return "none";
            }
        }
        /// <summary>
        /// Draws ClickMenu
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                //sb.Draw(source, boxes[i], Color.White);
                SDL.SDL_Rect dest = boxes[i].Rect;
                SDL.SDL_RenderCopy(renderer, source.texture, ref sourceBox, ref dest);
                SimpleFont.DrawString(renderer, names[i], (int)(topLeft.X + 5), (int)(topLeft.Y + (i * source.height)), colour);
                //sb.DrawString(font, names[i], new Vector2(topLeft.X + 5, topLeft.Y + i * source.Height), Color.White);
            }
        }
        /*
        public SpriteFont Font
        {
            get { return font; }
        }
        */
    }
}
