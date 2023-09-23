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
    /// ScrollingList class
    /// </summary>
    public class ScrollingList
    {
        //protected
        protected List<string> options;
        protected List<Rectangle> testBoxes;
        protected int start;
        protected int range;
        protected SimpleButton up;
        protected SimpleButton down;
        protected Rectangle box;
        protected int index;
        protected int boxW;
        protected int boxH;
        protected SDL.SDL_Color white;
        protected SDL.SDL_Color black;
        protected SDL.SDL_Color gray;

        //public
        /// <summary>
        /// ScrollingList constructor
        /// </summary>
        /// <param name="dbp">Up button pressed Texture2D reference</param>
        /// <param name="dbd">Up button depressed Texture2D reference</param>
        /// <param name="ubp">Down button pressed Texture2D reference</param>
        /// <param name="ubd">Down button pressed Texture2D reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="boxW">Box width</param>
        /// <param name="boxH">Box height</param>
        /// <param name="range">Range of displayed options</param>
        public ScrollingList(Texture2D dbp, Texture2D dbd, Texture2D ubp, Texture2D ubd, int x, int y, int width, int height, int boxW, int boxH, int range)
        {
            options = new List<string>();
            testBoxes = new List<Rectangle>();
            start = 0;
            this.range = range;
            up = new SimpleButton(ubd, ubp, x + width, y, "up");
            down = new SimpleButton(dbp, dbd, x + width, y + height, "down");
            box = new Rectangle(0, 0, width, height);
            index = 0;
            this.boxW = boxW;
            this.boxH = boxH;
            white.r = 255;
            white.g = 255;
            white.b = 255;
            white.a = 1;
            black.r = 0;
            black.g = 0;
            black.b = 0;
            black.a = 1;
            gray.r = 155;
            gray.g = 155;
            gray.b = 155;
            gray.a = 1;
        }
        /// <summary>
        /// Add an option to list
        /// </summary>
        /// <param name="option">Option as string to add</param>
        public void addOption(string option)
        {
            if (!contains(option))
            {
                options.Add(option);
                testBoxes.Add(new Rectangle(box.X, box.Y + options.Count * boxH, boxW, boxH));
            }
        }
        /// <summary>
        /// Checks if the list contains provided string
        /// </summary>
        /// <param name="option">Option to check</param>
        /// <returns>Boolean</returns>
        public bool contains(string option)
        {
            for (int i = 0; i < options.Count; i++)
            {
                if(options[i] == option)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Updates ScrollingList's internal state
        /// </summary>
        /// <param name="state">MBS</param>
        /// <param name="mBox">Rectangle reference</param>
        /// <returns></returns>
        public string update(MBS state, Rectangle mBox)
        {
            if (up.click(mBox, state) == "up")
            {
                if (start > 0)
                {
                    start--;
                }
            }
            else if (down.click(mBox, state) == "down")
            {
                if (options.Count > range)
                {
                    if (start + range < options.Count)
                    {
                        start++;
                    }
                }
            }
            up.update();
            down.update();
            if(state == MBS.left)
            {
                for (int i = 0; i < testBoxes.Count; i++)
                {
                    if (testBoxes[i].intersects(mBox))
                    {
                        return options[i];
                    }
                }

            }
            return "";
        }
        /// <summary>
        /// Draws ScrollingList
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            DrawRects.drawRect(renderer, box, white, true);
            //sb.Draw(pixel, box, Color.White);
            up.draw(renderer);
            down.draw(renderer);
            Point2D temp = new Point2D(box.X, box.Y);
            if (options.Count >= range)
            {
                if (start + range > options.Count)
                {
                    for (int i = start; i < options.Count; i++)
                    {
                        temp.Y = testBoxes[i].Y;
                        SimpleFont.DrawString(renderer, options[i], (int)temp.X, (int)temp.Y, black);
                        //sb.DrawString(font, options[i], temp, Color.Black);
                    }
                }
                else
                {
                    for (int i = start; i < start + range; i++)
                    {
                        temp.Y = testBoxes[i].Y;
                        SimpleFont.DrawString(renderer, options[i], (int)temp.X, (int)temp.Y, black);
                        //sb.DrawString(font, options[i], temp, Color.Black);
                    }
                }
            }
            else
            {
                for (int i = 0; i < options.Count; i++)
                {
                    temp.Y = testBoxes[i].Y;
                    SpriteFont.draw(renderer, options[i], temp.X, temp.Y, black);
                    //sb.DrawString(font, options[i], temp, Color.Black);
                }
            }
        }
    }
}
