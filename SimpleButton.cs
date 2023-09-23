//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// SimpleButton class
    /// </summary>
    public class SimpleButton
    {
        //protected
        protected Texture2D depressed;
        protected Texture2D pressed;
        protected Rectangle box;
        protected Rectangle box2;
        protected string name;
        protected CoolDown cool;

        //public
        /// <summary>
        /// SimpleButton constructor
        /// </summary>
        /// <param name="depressed">Texture2D reference</param>
        /// <param name="pressed">Texture2D reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="name">Button name</param>
        public SimpleButton(Texture2D depressed, Texture2D pressed, int x, int y, string name)
        {
            this.depressed = depressed;
            this.pressed = pressed;
            this.name = name;
            box = new Rectangle(0, 0, depressed.width, depressed.height);
            box2 = new Rectangle(x, y, depressed.width, depressed.height);
            cool = new CoolDown(5);
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        public void update()
        {
            cool.update();
        }
        /// <summary>
        /// Checks if button has been clicked
        /// </summary>
        /// <param name="mBox">Rectangle reference</param>
        /// <param name="state">MBS</param>
        /// <returns>String</returns>
        public string click(Rectangle mBox, MBS state)
        {
            if (state == MBS.left)
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
        /// Draws SimpleButton
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            if (cool.Active)
            {
                SimpleDraw.draw(renderer, pressed, box, box2);
                //sb.Draw(pressed, box, Color.White);
            }
            else
            {
                SimpleDraw.draw(renderer, depressed, box, box2);
                //sb.Draw(depressed, box, Color.White);
            }
        }
    }
}
