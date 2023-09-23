//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace XenoLib
{
    public class ScrollBar
    {
        //protected
        protected Rectangle bar;
        protected Rectangle tab;
        protected SDL.SDL_Color colour1;
        protected SDL.SDL_Color colour2;
        protected int maxNum;
        protected int currNum;

        //public
        /// <summary>
        /// ScrollBar constructor
        /// </summary>
        /// <param name="x">X position in pixels</param>
        /// <param name="y">Y position in pixels</param>
        /// <param name="maxNum">Max number of items to scroll</param>
        /// <param name="height">Height of ScrollBar</param>
        public ScrollBar(int x, int y, int maxNum, int height)
        {
            if(height < 64)
            {
                height = 64;
            }
            bar = new Rectangle(x, y, 32, height + 32);
            tab = new Rectangle(x, y, 32, 32);
            colour1 = ColourBank.getColour(XENOCOLOURS.DIM_GRAY);
            colour2 = ColourBank.getColour(XENOCOLOURS.GRAY);
            this.maxNum = maxNum;
            currNum = 0;
        }
        /// <summary>
        /// Draws ScrollBar
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            DrawRects.drawRect(renderer, bar, colour1, true);
            DrawRects.drawRect(renderer, tab, colour2, true);
        }
        /// <summary>
        /// Returns the percentage of scrolled bar
        /// </summary>
        /// <returns>Percentage as float value</returns>
        public float scrollPercent()
        {
            float percent = 0;
            percent = (tab.Y / (bar.Height - 32)) * 100;
            return percent;
        }
        /// <summary>
        /// Returns the relative position of tab to scrolled value
        /// </summary>
        /// <returns>Integer</returns>
        public int scrollPosition()
        {
            return (int)(maxNum * scrollPercent());
        }
        /// <summary>
        /// Updates the tab position on ScrollBar
        /// </summary>
        public void update()
        {
            Point2D p = new Point2D(0, 0);
            if(MouseHandler.getLeft() == true)
            {
                p.X = MouseHandler.getMouseX();
                p.Y = MouseHandler.getMouseY();
                if (bar.pointInRect(p) == true)
                {
                    tab.Y = p.Y - bar.Y;
                }
            }
        }
        /// <summary>
        /// MaxNum property
        /// </summary>
        public int MaxNum
        {
            get { return maxNum; }
            set { maxNum = value; }
        }
    }
}
