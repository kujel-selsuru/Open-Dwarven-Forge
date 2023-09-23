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
    /// SimpleMenuV3
    /// </summary>
    public class SimpleMenuV3
    {
        //protected
        protected Texture2D background;
        protected Texture2D rect;
        protected Rectangle backBox;
        protected Rectangle rectBox;
        protected Rectangle clickBox;
        protected int index;
        protected int middle;
        protected int shift;
        protected int spacer;
        protected string[] options;
        protected CoolDown delay;
        protected Point2D mousePos; 

        //public
        /// <summary>
        /// SimpleMenuV3 constructor
        /// </summary>
        /// <param name="background">Texture2D</param>
        /// <param name="rect">Texture2D</param>
        /// <param name="options">Arrary of strings</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="spacer"></param>
        public SimpleMenuV3(Texture2D background, Texture2D rect, string[] options, int x = 50, int y = 50, int w = 500, int h = 250, int spacer = 10)
        {
            this.background = background;
            this.rect = rect;
            backBox = new Rectangle(x, y, w, h);
            rectBox = new Rectangle(x, y, w, h);
            clickBox = new Rectangle(x, y, w, h);
            index = 0;
            middle = (int)(backBox.X + (backBox.Width / 2));
            this.options = new string[options.Length];
            for (int i = 0; i < options.Length; i++)
            {
                this.options[i] = options[i];
            }
            mousePos = new Point2D(0, 0);
            delay = new CoolDown(4);
            this.spacer = spacer;
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        /// <returns>string</returns>
        public string update()
        {
            mousePos.X = MouseHandler.getMouseX();
            mousePos.Y = MouseHandler.getMouseY();
            if (!delay.Active)
            {
                if (MouseHandler.getLeft())
                {
                    Point2D pos = new Point2D(0, 0);
                    Point2D dimen = new Point2D(0, 32);
                    for (int i = 0; i < options.Length; i++)
                    {
                        dimen.X = options[i].Length * 16;
                        clickBox.Width = (int)dimen.X;
                        clickBox.Height = (int)dimen.Y;
                        clickBox.X = middle - (int)(dimen.X / 2);
                        clickBox.Y = backBox.Y + 5 + (i * (int)dimen.Y) + (i * spacer);
                        if (new Rectangle(mousePos.X, mousePos.Y, 3, 3).intersects(clickBox))
                        {
                            return options[i];
                        }
                    }
                    delay.activate();
                }
            }
            if (!delay.Active)
            {
                if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_UP))
                {
                    if (index > 0)
                    {
                        index--;
                    }
                    else
                    {
                        index = options.Length - 1;
                    }
                    delay.activate();
                }
                if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_DOWN))
                {
                    if (index < options.Length - 1)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }
                    delay.activate();
                }
                if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_RETURN))
                {
                    delay.activate();
                    return options[index];
                }
                if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_ESCAPE))
                {
                    delay.activate();
                    return "cancel";
                }
            }
            delay.update();
            return "";
        }
        /// <summary>
        /// Draw SimpleMenuV3
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="colour">SDL_Color</param>
        public void draw(IntPtr renderer, SDL.SDL_Color colour)
        {
            SimpleDraw.draw(renderer, background, backBox);
            //sb.Draw(background, backBox, Color.White);
            Point2D pos = new Point2D(0, 0);
            Point2D dimen = new Point2D(0, 32);
            dimen.X = options[index].Length * 16;
            rectBox.X = middle - ((int)dimen.X / 2);
            rectBox.Y = backBox.Y + 5 + (index * (int)dimen.Y) + (index * spacer);
            rectBox.Width = (int)dimen.X;
            rectBox.Height = (int)dimen.Y;
            SimpleDraw.draw(renderer, rect, rectBox);
            //sb.Draw(rect, rectBox, Color.White);
            for (int i = 0; i < options.Length; i++)
            {
                dimen.X = options[i].Length * 16;
                pos.X = middle - (dimen.X / 2);
                pos.Y = backBox.Y + 5 + (i * dimen.Y) + (i * spacer);
                SimpleFont.DrawString(renderer, options[i], pos.X, pos.Y, colour);
                //sb.DrawString(font, options[i], pos, colour);
            }
            
        }
    }
}

