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
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Graphics;
//using GLIB;

namespace XenoLib
{
    /// <summary>
    /// ListMenu class
    /// </summary>
    public class ListMenu
    {
        //protected
        protected string[] options;
        protected Rectangle[] boxes;
        protected Point2D topLeft;
        protected Point2D ms;
        protected CoolDown delay;
        protected bool active;
        protected int w;
        protected int h;

        //public
        /// <summary>
        /// ListMenu constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public ListMenu(int x, int y)
        {
            topLeft = new Point2D(x, y);
            delay = new CoolDown(3);
            active = false;
            ms = new Point2D();
        }
        /// <summary>
        /// Update internal state
        /// </summary>
        /// <param name="sx">Shift x value</param>
        /// <param name="sy">Shift y value</param>
        /// <returns>String</returns>
        public string update(int sx = 9, int sy = 9)
        {
            ms.X = MouseHandler.getMouseX();
            ms.Y = MouseHandler.getMouseY();
            //MouseState ms = Mouse.GetState();
            if (!delay.Active)
            {
                if (MouseHandler.getLeft())
                {
                    delay.activate();
                    Rectangle tar = new Rectangle(ms.X + sx, ms.Y + sy, 3, 3);
                    for (int i = 0; i < boxes.Length; i++)
                    {
                        if(boxes[i].intersects(tar))
                        {
                            active = false;
                            return options[i];
                        }
                    }
                }
            }
            delay.update();
            return " ";
        }
        /// <summary>
        /// Activates list and populates list of options
        /// </summary>
        /// <param name="opts">String Arrary</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width of ListMenu</param>
        /// <param name="h">Height of option boxes</param>
        /// <param name="gap">Gap between boxes</param>
        public void activate(string[] opts, int x, int y, int w = 250, int h = 24, int gap = 6)
        {
            active = true;
            options = new string[opts.Length];
            boxes = new Rectangle[opts.Length];
            topLeft.X = x;
            topLeft.Y = y;
            this.w = w;
            this.h = h;
            for (int i = 0; i < opts.Length; i++)
            {
                options[i] = opts[i];
                boxes[i] = new Rectangle(x, y + (h * i) + (i * gap), w, h);
            }
        }
        /// <summary>
        /// Draws ListMenu
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            SDL.SDL_Color oldColour;
            SDL.SDL_Color black;
            black.r = 0;
            black.g = 0;
            black.b = 0;
            black.a = 1;
            for (int i = 0; i < options.Length; i++)
            {
                SDL.SDL_Rect tmp = boxes[i].Rect;
                
                SDL.SDL_GetRenderDrawColor(renderer, out oldColour.r, out oldColour.g, out oldColour.b, out oldColour.a);
                SDL.SDL_SetRenderDrawColor(renderer, 127, 127, 127, 1);
                SDL.SDL_RenderDrawRect(renderer, ref tmp);
                SDL.SDL_SetRenderDrawColor(renderer, oldColour.r, oldColour.g, oldColour.b, oldColour.a);
                //sb.Draw(pixel, boxes[i], Color.LightGray);
                SpriteFont.draw(renderer, options[i], (int)boxes[i].X, (int)boxes[i].X, black);
                //sb.DrawString(font, options[i], new Point2D(boxes[i].X, boxes[i].Y), Color.Black);
            }
        }
        /// <summary>
        /// Deactivate ListMenu
        /// </summary>
        public void deactivate()
        {
            active = false;
        }
        /// <summary>
        /// Active property
        /// </summary>
        public bool Active
        {
            get { return active; }
        }
    }
}
