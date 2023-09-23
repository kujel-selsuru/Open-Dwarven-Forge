//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// SimpleSlider class for scrolling values
    /// </summary>
    public class SimpleSlider
    {
        //protected
        protected Rectangle bar;
        protected Rectangle handle;
        protected SimpleButton4 upButton;
        protected SimpleButton4 downButton;
        protected float percentage;
        protected int max;
        protected bool vert;
        protected int state;
        protected CoolDown inputDelay;

        //public
        /// <summary>
        /// SimpleSlider
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="max">Max meassured value</param>
        /// <param name="blankButton">Blank 1/10th the length/height of the slider</param>
        /// <param name="vert">Verticle flag value</param>
        public SimpleSlider(float x, float y, int w, int h, int max, string blankButton, bool vert = false)
        {
            bar = new Rectangle(x, y, w, h);
            if (vert == true)
            {
                handle = new Rectangle(x, y, w, h / 10);
                upButton = new SimpleButton4(TextureBank.getTexture(blankButton), TextureBank.getTexture(blankButton), (int)x, (int)(y - handle.Height), "up");
                downButton = new SimpleButton4(TextureBank.getTexture(blankButton), TextureBank.getTexture(blankButton), (int)x, (int)(y + handle.Height), "down");
            }
            else
            {
                handle = new Rectangle(x, y, w / 10, h);
                upButton = new SimpleButton4(TextureBank.getTexture(blankButton), TextureBank.getTexture(blankButton), (int)(x - handle.Width), (int)y, "up");
                downButton = new SimpleButton4(TextureBank.getTexture(blankButton), TextureBank.getTexture(blankButton), (int)(x + handle.Width), (int)y, "down");
            }
            this.vert = vert;
            this.max = max;
            percentage = 0;
            state = 0;
            inputDelay = new CoolDown(15);
        }
        /// <summary>
        /// Updates SimpleSlider's internal state
        /// </summary>
        public void update()
        {
            upButton.update();
            downButton.update();
            inputDelay.update();
            if(upButton.click() == "up")
            {
                if(state > 0)
                {
                    state--;
                    percentage = (float)state / max;
                }
            }
            if (downButton.click() == "down")
            {
                if (state < max)
                {
                    state++;
                    percentage = (float)state / max;
                }
            }

            if (MouseHandler.getLeft() == true)
            {
                if (inputDelay.Active == false)
                {
                    Point2D tmp = new Point2D(MouseHandler.getMouseX(), MouseHandler.getMouseY());
                    if (bar.pointInRect(tmp))
                    {
                        inputDelay.activate();
                        float tmpPercent = 0;
                        if(vert == true)
                        {
                            tmpPercent = (tmp.Y - bar.Y) / bar.Height;
                        }
                        else
                        {
                            tmpPercent = (tmp.X - bar.X) / bar.Width;
                        }
                        state = (int)((float)max * tmpPercent);
                        percentage = tmpPercent;
                    }
                }
            }
        }
        /// <summary>
        /// Draws SimpleSlider
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="barColour">Bar colour</param>
        /// <param name="handleColour">Handle colour</param>
        public void draw(IntPtr renderer, XENOCOLOURS barColour, XENOCOLOURS handleColour)
        {
            DrawRects.drawRect(renderer, bar, ColourBank.getColour(barColour), false);
            DrawRects.drawRect(renderer, handle, ColourBank.getColour(handleColour), true);
            upButton.draw(renderer);
            downButton.draw(renderer);
        }
        /// <summary>
        /// State property
        /// </summary>
        public int State
        {
            get { return state; }
            set
            {
                if (value >= 0 && value < max)
                {
                    state = value;
                }
                else
                {
                    state = 0;
                }
            }
        }
        /// <summary>
        /// Percentage property
        /// </summary>
        public float Percentage
        {
            get { return percentage; }
        }
        /// <summary>
        /// Max property
        /// </summary>
        public int Max
        {
            get { return max; }
            set { max = value; }
        }
        /// <summary>
        /// Vert property
        /// </summary>
        public bool Vert
        {
            get { return vert; }
        }
    }
}
