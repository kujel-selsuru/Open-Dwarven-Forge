//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using GLIB;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// Tabs enumeration
    /// </summary>
    public enum TABS { one = 0, two, three, four, five, none }
    /// <summary>
    /// TabPanel class
    /// </summary>
    public class TabPanel
    {
        //protected
        protected Texture2D source;
        protected Rectangle srcBox;
        protected Rectangle[] boxes;
        protected int numTabs;
        protected Point2D topLeft;
        protected string[] names;
        protected TABS lastSelected;
        protected SDL.SDL_Color white;

        //public
        /// <summary>
        /// TabsPanel constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="names">Names arrary of strings</param>
        /// <param name="numTabs">Number of tabs</param>
        public TabPanel(Texture2D source, int x, int y, string[] names, int numTabs = 5)
        {
            this.source = source;
            topLeft = new Point2D(x, y);
            this.numTabs = numTabs;
            srcBox = new Rectangle(0, 0, source.width, source.height);
            boxes = new Rectangle[numTabs];
            for (int i = 0; i < numTabs; i++)
            {
                boxes[i] = new Rectangle(x + i * source.width, y, source.width, source.height);
            }
            this.names = new string[names.Length];
            for (int i = 0; i < numTabs; i++)
            {
                this.names[i] = names[i];
            }
            lastSelected = TABS.none;
            white.r = 255;
            white.g = 255;
            white.b = 255;
            white.a = 1;
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        /// <param name="rect">Rectangle referecne</param>
        /// <param name="clicked">Boolean for if left mouse button is pressed</param>
        /// <returns></returns>
        public TABS update(Rectangle rect, bool clicked)
        {
            if (!clicked)
            {
                lastSelected = TABS.none;
                return TABS.none;
            }
            else
            {
                for (int i = 0; i < boxes.Length; i++)
                {
                    if (boxes[i].intersects(rect))
                    {
                        lastSelected = (TABS)i;
                        return (TABS)i;
                    }
                }
                lastSelected = TABS.none;
                return TABS.none;
            }
        }
        /// <summary>
        /// Draws TabsPanel
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="charSize">CharSize</param>
        public void draw(IntPtr renderer, int charSize = 16)
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                //int shift = source.width - ((names[i].Length * charSize) / 2);
                SimpleDraw.draw(renderer, source, srcBox, boxes[i]);
                int mid = SimpleFont.stringRenderWidth(names[i], 1) / 2;
                SimpleFont.DrawString(renderer, names[i], boxes[i].X + ((boxes[i].Width / 2) - mid), boxes[i].Y, 1);
            }
        }
        /// <summary>
        /// LastSelected property
        /// </summary>
        public TABS LastSelected
        {
            get { return lastSelected; }
            set { lastSelected = value; }
        }
    }
}
