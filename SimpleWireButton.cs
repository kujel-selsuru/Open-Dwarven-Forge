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
    /// SimpleWireButton class
    /// </summary>
    public class SimpleWireButton
    {
        //protected
        protected Rectangle box;
        protected string name;
        protected CoolDown cool;
        protected SDL.SDL_Color white;

        //public
        /// <summary>
        /// SimpleWireButton constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="name">Name</param>
        public SimpleWireButton(int x, int y, int w, int h, string name)
        {
            this.name = name;
            box = new Rectangle(x, y, w, h);
            cool = new CoolDown(7);
            white.r = 255;
            white.g = 255;
            white.b = 255;
            white.a = 1;
        }
        /// <summary>
        /// Update internal state
        /// </summary>
        public void update()
        {
            cool.update();
        }
        /// <summary>
        /// Checks if button was clicked
        /// </summary>
        /// <param name="mBox">Rectangle reference</param>
        /// <returns>String</returns>
        public string click(Rectangle mBox)
        {
            if (MouseHandler.getLeft())
            {
                if (!cool.Active)
                {
                    if (box.intersects(mBox))
                    {
                        cool.activate();
                        return name;
                    }
                }
            }
            return "";
        }
        /// <summary>
        /// Draws button
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="colour">Colour of button frame</param>
        public void draw(IntPtr renderer, SDL.SDL_Color colour)
        {
            //sb.Draw(wp, box, colour);
            DrawRects.drawRect(renderer, box, colour, false);
            //Line.drawSquare(sb, wp, box.X, box.Y, box.Width, box.Height, colour, 2);
        }
        /// <summary>
        /// Draws name of button
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void darwName(IntPtr renderer)
        {
            SpriteFont.draw(renderer, name, box.X + 2, box.Y + 2, white);
            //sb.DrawString(font, name, new Vector2(X + 2, Y + 2), Color.White);
        }
        /// <summary>
        /// Name property
        /// </summary>
        public string Name
        {
            get { return name; }
        }
        /// <summary>
        /// X property
        /// </summary>
        public float X
        {
            get { return box.X; }
            set { box.X = value; }
        }
        /// <summary>
        /// Y property
        /// </summary>
        public float Y
        {
            get { return box.Y; }
            set { box.Y = value; }
        }
    }
}
