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

namespace XenoLib
{
    /// <summary>
    /// SimpleStringBuilder class
    /// </summary>
    public class SimpleStringBuilder
    {
        //protected
        protected string sequence;
        protected CoolDown delay;
        protected Point2D point;

        //public
        /// <summary>
        /// SimpleStringBuilder constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="ticks">Input delay value</param>
        public SimpleStringBuilder(int x = 100, int y = 100, int ticks = 60)
        {
            delay = new CoolDown(ticks);
            point = new Point2D(x, y);
            sequence = "";
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        public void update()
        {
            if (!delay.Active)
            {
                if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_LSHIFT) | KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_RSHIFT))
                {
                    if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_a))
                    {
                        sequence += "A";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_b))
                    {
                        sequence += "B";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_c))
                    {
                        sequence += "C";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_d))
                    {
                        sequence += "D";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_e))
                    {
                        sequence += "E";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_f))
                    {
                        sequence += "F";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_g))
                    {
                        sequence += "G";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_h))
                    {
                        sequence += "H";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_i))
                    {
                        sequence += "I";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_j))
                    {
                        sequence += "J";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_k))
                    {
                        sequence += "K";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_l))
                    {
                        sequence += "L";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_m))
                    {
                        sequence += "M";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_n))
                    {
                        sequence += "N";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_o))
                    {
                        sequence += "O";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_p))
                    {
                        sequence += "P";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_q))
                    {
                        sequence += "Q";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_r))
                    {
                        sequence += "R";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_s))
                    {
                        sequence += "S";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_t))
                    {
                        sequence += "T";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_w))
                    {
                        sequence += "W";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_u))
                    {
                        sequence += "U";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_v))
                    {
                        sequence += "V";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_x))
                    {
                        sequence += "X";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_y))
                    {
                        sequence += "Y";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_z))
                    {
                        sequence += "Z";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_1))
                    {
                        sequence += "!";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_2))
                    {
                        sequence += "@";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_3))
                    {
                        sequence += "#";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_4))
                    {
                        sequence += "$";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_5))
                    {
                        sequence += "%";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_6))
                    {
                        sequence += "^";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_7))
                    {
                        sequence += "&";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_8))
                    {
                        sequence += "*";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_9))
                    {
                        sequence += "(";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_0))
                    {
                        sequence += ")";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_QUOTE))
                    {
                        sequence += "\"";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_PERIOD))
                    {
                        sequence += ">";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_COMMA))
                    {
                        sequence += "<";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_BACKSLASH))
                    {
                        sequence += "?";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_SLASH))
                    {
                        sequence += "|";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_MINUS))
                    {
                        sequence += "_";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_EQUALS))
                    {
                        sequence += "+";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_SEMICOLON))
                    {
                        sequence += ":";
                        delay.activate();
                    }
                }
                else
                {
                    if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_a))
                    {
                        sequence += "a";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_b))
                    {
                        sequence += "b";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_c))
                    {
                        sequence += "c";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_d))
                    {
                        sequence += "d";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_e))
                    {
                        sequence += "e";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_f))
                    {
                        sequence += "f";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_g))
                    {
                        sequence += "g";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_h))
                    {
                        sequence += "h";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_i))
                    {
                        sequence += "i";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_j))
                    {
                        sequence += "j";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_k))
                    {
                        sequence += "k";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_l))
                    {
                        sequence += "l";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_m))
                    {
                        sequence += "m";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_n))
                    {
                        sequence += "n";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_o))
                    {
                        sequence += "o";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_p))
                    {
                        sequence += "p";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_q))
                    {
                        sequence += "q";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_r))
                    {
                        sequence += "r";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_s))
                    {
                        sequence += "s";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_t))
                    {
                        sequence += "t";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_w))
                    {
                        sequence += "w";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_u))
                    {
                        sequence += "u";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_v))
                    {
                        sequence += "v";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_x))
                    {
                        sequence += "x";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_y))
                    {
                        sequence += "y";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_z))
                    {
                        sequence += "z";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_1))
                    {
                        sequence += "1";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_2))
                    {
                        sequence += "2";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_3))
                    {
                        sequence += "3";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_4))
                    {
                        sequence += "4";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_5))
                    {
                        sequence += "5";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_6))
                    {
                        sequence += "6";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_7))
                    {
                        sequence += "7";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_8))
                    {
                        sequence += "8";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_9))
                    {
                        sequence += "9";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_0))
                    {
                        sequence += "0";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_SPACE))
                    {
                        sequence += " ";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_QUOTE))
                    {
                        sequence += "\"";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_PERIOD))
                    {
                        sequence += ".";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_COMMA))
                    {
                        sequence += ",";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_BACKSLASH))
                    {
                        sequence += "\\";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_SLASH))
                    {
                        sequence += "/";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_RETURN))
                    {
                        sequence += "\n";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_MINUS))
                    {
                        sequence += "-";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_EQUALS))
                    {
                        sequence += "=";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_SEMICOLON))
                    {
                        sequence += ";";
                        delay.activate();
                    }
                }
                if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_BACKSPACE))
                {
                    string temp = "";
                    for (int i = 0; i < sequence.Length - 1; i++)
                    {
                        temp += sequence[i];
                    }
                    sequence = temp;
                    delay.activate();
                }
            }
            delay.update();
        }
        /// <summary>
        /// Sequence property
        /// </summary>
        public string Sequence
        {
            get { return sequence; }
            set { sequence = value; }
        }
        /// <summary>
        /// Draws contents of sequence (deprecated)
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="colour">SDL_Colour reference</param>
        public void draw(IntPtr renderer, SDL.SDL_Color colour)
        {
            SpriteFont.draw(renderer, sequence, (int)point.X, (int)point.Y, colour);
            //sb.DrawString(font, sequence, new Point2D(point.X, point.Y), colour);
        }
        /// <summary>
        /// Draws contents of sequence
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="scaler">scaler value</param>
        public void draw(IntPtr renderer, float scaler = 1)
        {
            SimpleFont.DrawString(renderer, sequence, point.X, point.Y, scaler);
            //sb.DrawString(font, sequence, new Point2D(point.X, point.Y), colour);
        }
        /// <summary>
        /// Clears contents of sqeuence
        /// </summary>
        public void clear()
        {
            sequence = "";
        }
        /// <summary>
        /// Resets delay value
        /// <param name="num">Number of ticks value</param>
        /// </summary>
        public void reset(int num)
        {
            delay.reset(num);
        }
        /// <summary>
        /// Delay property
        /// </summary>
        public int Delay
        {
            get { return delay.MaxTicks; }
        }
    }
    /// <summary>
    /// SimpleNumberBuilder (only builds integars)
    /// </summary>
    public class SimpleNumberBuilder
    {
        //protected
        protected string sequence;
        protected CoolDown delay;
        protected Point2D point;

        //public
        /// <summary>
        /// SimpleNumberBuilder constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="ticks"></param>
        public SimpleNumberBuilder(int x = 100, int y = 100, int ticks = 4)
        {
            delay = new CoolDown(ticks);
            point = new Point2D(x, y);
            sequence = "0";
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        public void update()
        {
            
            if (!delay.Active)
            {
                if (sequence.Length < 9)
                {
                    if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_1))
                    {
                        sequence += "1";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_2))
                    {
                        sequence += "2";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_3))
                    {
                        sequence += "3";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_4))
                    {
                        sequence += "4";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_5))
                    {
                        sequence += "5";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_6))
                    {
                        sequence += "6";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_7))
                    {
                        sequence += "7";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_8))
                    {
                        sequence += "8";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_9))
                    {
                        sequence += "9";
                        delay.activate();
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_0))
                    {
                        sequence += "0";
                        delay.activate();
                    }
                }
                if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_BACKSPACE))
                {
                    string temp = "";
                    for (int i = 0; i < sequence.Length - 1; i++)
                    {
                        temp += sequence[i];
                    }
                    sequence = temp;
                    delay.activate();
                }
            }
            delay.update();
        }
        /// <summary>
        /// Draws the contents of internal sequence (deprecated)
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="colour">Name of colour</param>
        public void draw(IntPtr renderer, string colour)
        {
            SimpleFont.drawColourString(renderer, sequence, (int)point.X, (int)point.Y, colour);
            //sb.DrawString(font, sequence, new Vector2(point.X, point.Y), colour);
        }
        /// <summary>
        /// Draws contents of sequence
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="scaler">scaler value</param>
        public void draw(IntPtr renderer, float scaler = 1)
        {
            SimpleFont.DrawString(renderer, sequence, point.X, point.Y, scaler);
            //sb.DrawString(font, sequence, new Point2D(point.X, point.Y), colour);
        }
        /// <summary>
        /// Clears the internal sequence
        /// </summary>
        public void clear()
        {
            sequence = "";
        }
        /// <summary>
        /// Number property
        /// </summary>
        public int Number
        {
            get
            {
                if(sequence == "")
                {
                    return 0;
                }
                return (int)Convert.ToUInt32(sequence);
            }
            set { sequence = Convert.ToString(value); }
        }
        /// <summary>
        /// Sequence property (only returns)
        /// </summary>
        public string Sequence
        {
            get { return sequence; }
        }
    }
}
