//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;

namespace XenoLib
{
    /// <summary>
    /// SimpleMenu class
    /// </summary>
    public class SimpleMenu
    {
        //protected
        protected Texture2D back;
        protected Rectangle box;
        protected int index;
        protected SDI input;
        protected SDL.SDL_Color gray;
        protected SDL.SDL_Color white;
        protected SDL.SDL_Color black;

        //public
        public SimpleMenu(Texture2D back, int x, int y, int delay = 6)
        {
            this.back = back;
            box = new Rectangle(x, y, back.width, back.height);
            index = 0;
            input = new SDI(delay);
            gray.r = 126;
            gray.g = 126;
            gray.b = 126;
            gray.a = 1;
            white.r = 255;
            white.g = 255;
            white.b = 255;
            white.a = 1;
            black.r = 0;
            black.g = 0;
            black.b = 0;
            black.a = 1;
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        /// <param name="options">Arrary of strings representing options</param>
        /// <returns>String</returns>
        public string update(string[] options)
        {
            string opt = "";
            inputState inps = input.update();

            switch(inps)
            {
                case inputState.lu:
                    if (index == 0)
                    {
                        index = options.Length - 1;
                    }
                    else
                    {
                        index--;
                    }
                    break;
                case inputState.ld:
                    if (index == options.Length - 1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }
                    break;
                case inputState.a:
                    opt = options[index];
                    break;
            }
            return opt;
        }
        /// <summary>
        /// Draws SimpleMenu
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="options">Arrary of strings</param>
        /// <param name="shift">Shift value</param>
        public void draw(IntPtr renderer, string[] options, int shift = 20)
        {
            //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            DrawRects.drawRect(renderer, box, gray, true);
            //sb.Draw(back, box, Color.White);
            for (int i = 0; i < options.Length; i++ )
            {
                if (i == index)
                {
                    SimpleFont.DrawString(renderer, options[i], box.X, box.Y + shift + (shift * i), black);
                    //sb.DrawString(font, options[i], new Point2D(box.X, box.Y + shift + (shift * i)), Color.Gray);
                }
                else
                {
                    SimpleFont.DrawString(renderer, options[i], box.X, box.Y + shift + (shift * i), white);
                    //sb.DrawString(font, options[i], new Vector2(box.X, box.Y + shift + (shift * i)), Color.White);
                }
            }
            //sb.End();
        }
    }
    /// <summary>
    /// KeyboardMenu class
    /// </summary>
    public class KeyboardMenu
    {
        //protected
        protected Texture2D back;
        protected Rectangle box;
        protected int index;
        protected CoolDown delay;
        protected SDL.SDL_Color gray;
        protected SDL.SDL_Color white;
        protected SDL.SDL_Color black;

        //public
        /// <summary>
        /// KeyBoardMenu constructor
        /// </summary>
        /// <param name="back">Texture2D</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="delay">Input delay value</param>
        public KeyboardMenu(Texture2D back, int x, int y, int delay = 6)
        {
            this.back = back;
            box = new Rectangle(x, y, back.width, back.height);
            index = 0;
            this.delay = new CoolDown(delay);
            gray.r = 126;
            gray.g = 126;
            gray.b = 126;
            gray.a = 1;
            white.r = 255;
            white.g = 255;
            white.b = 255;
            white.a = 1;
            black.r = 0;
            black.g = 0;
            black.b = 0;
            black.a = 1;
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        /// <param name="options">String arrary of options</param>
        /// <returns>String</returns>
        public string update(string[] options)
        {
            string opt = "";
            if (!delay.Active)
            {
                if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_UP))
                {
                    if (index == 0)
                    {
                        index = options.Length - 1;
                    }
                    else
                    {
                        index--;
                    }
                    delay.activate();
                }
                if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_DOWN))
                {
                    if (index == options.Length - 1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }
                    delay.activate();
                }
                if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_RETURN))
                {
                    opt = options[index];
                    delay.activate();
                }
            }
            delay.update();
            return opt;
        }
        /// <summary>
        /// Draws KeyboardMenu
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="options">Arrary of strings</param>
        /// <param name="shift">Shift value</param>
        public void draw(IntPtr renderer, string[] options, int shift = 20)
        {
            //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            DrawRects.drawRect(renderer, box, gray, true);
            //sb.Draw(back, box, Color.White);
            for (int i = 0; i < options.Length; i++)
            {
                if (i == index)
                {
                    SimpleFont.DrawString(renderer, options[i], box.X, box.Y + shift + (shift * i), black);
                    //sb.DrawString(font, options[i], new Vector2(box.X, box.Y + shift + (shift * i)), Color.Gray);
                }
                else
                {
                    SimpleFont.DrawString(renderer, options[i], box.X, box.Y + shift + (shift * i), white);
                    //sb.DrawString(font, options[i], new Vector2(box.X, box.Y + shift + (shift * i)), Color.White);
                }
            }
            //sb.End();
        }
    }
}
