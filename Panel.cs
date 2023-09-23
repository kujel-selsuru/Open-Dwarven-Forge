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
    /// Panel8slots enumeration
    /// </summary>
    public enum panel8slots { one = 0, two, three, four, five, six, seven, eight, none}

    /// <summary>
    /// Panel2x4 class
    /// </summary>
    public class Panel2x4
    {
        //protected
        //2 x 4 buttons
        protected Rectangle box;
        protected Rectangle buttonSrcBox;
        protected Rectangle[,] buttons;
        protected panel8slots[,] slots;
        protected Texture2D[,] buttonSources;
        protected int w;
        protected int h;

        //public
        /// <summary>
        /// Panel2x4 constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="sources">Texture2D reference array</param>
        /// <param name="bw">Button width</param>
        /// <param name="bh">Button height</param>
        public Panel2x4(int x, int y, Texture2D[] sources, int bw = 80, int bh = 80)//, int w = 2, int h = 4)
        {
            box = new Rectangle(x, y, bw * 2, bh * 4);
            buttons = new Rectangle[2, 4];
            this.w = 2;
            this.h = 4;
            for (int i = 0; i < w; i++)
            {
                for (int k = 0; k < h; k++)
                {
                    buttons[i, k] = new Rectangle(x + i * bw, y + k * bh, bw, bh);
                }
            }
            buttonSources = new Texture2D[w, h];
            int count = 0;
            for (int i = 0; i < w; i++)
            {
                for (int k = 0; k < h; k++)
                {
                    buttonSources[i, k] = sources[count++];
                }
            }
            slots = new panel8slots[w, h];
            slots[0, 0] = panel8slots.one;
            slots[0, 1] = panel8slots.two;
            slots[0, 2] = panel8slots.three;
            slots[0, 3] = panel8slots.four;
            slots[1, 0] = panel8slots.five;
            slots[1, 1] = panel8slots.six;
            slots[1, 2] = panel8slots.seven;
            slots[1, 3] = panel8slots.eight;
        }
        /// <summary>
        /// Draw Panel2x4
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            for (int i = 0; i < w; i++)
            {
                for (int k = 0; k < h; k++)
                {
                    SimpleDraw.draw(renderer, buttonSources[i, k], buttonSrcBox, buttons[i, k]);
                    //sb.Draw(buttonSources[i,k], buttons[i,k], Color.White);
                }
            }
        }
        /// <summary>
        /// checks if rectangle is over a panel
        /// </summary>
        /// <param name="msb">Rectangle reference</param>
        /// <returns>Panel8slots enumeration</returns>
        public panel8slots attemptClickButton(Rectangle msb)
        {
            if(box.intersects(msb))
            {
                for(int i = 0; i < w; i++)
                {
                    for (int k = 0; k < h; k++)
                    {
                        if (buttons[i, k].intersects(msb))
                        {
                            return slots[i, k];
                        }
                    }
                }
            }
            return panel8slots.none;
        }
    }
}
