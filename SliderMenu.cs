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
    /// Slider class
    /// </summary>
    public class Slider
    {
        //protected
        protected Texture2D knob;
        protected Rectangle knobBox;
        protected Rectangle knobDestBox;
        protected int p;
        protected int length;
        protected int thickness;
        protected int speed;
        protected Point2D position;
        protected Point2D position2;

        //public
        /// <summary>
        /// Slider constructor
        /// </summary>
        /// <param name="knob">Texture2D</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="length">Length</param>
        public Slider(Texture2D knob, int x, int y, int length)
        {
            this.knob = knob;
            knobBox = new Rectangle(0, 0, knob.width, knob.height);
            knobDestBox = new Rectangle(x, y - knob.height / 2, knob.width, knob.height);
            p = 0;
            this.length = length;
            position = new Point2D(x, y);
            position = new Point2D(x + length, y);
            speed = length / 100;
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        /// <param name="inps">InputState enumeration</param>
        public void update(inputState inps)
        {
            switch(inps)
            {
                case inputState.none:
                    break;
                case inputState.ll:
                    if (p > 0)
                    {
                        p--;
                        knobBox.X -= speed;
                    }
                    break;
                case inputState.lr:
                    if(p < 99)
                    {
                        p++;
                        knobBox.X += speed;
                    }
                    break;
            }
        }
        /// <summary>
        /// Draws Slider
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="colour">SDL_Color</param>
        public void draw(IntPtr renderer, SDL.SDL_Color colour)
        {
            DrawLine.draw(renderer, position, position2, colour);
            //Line.Draw(sb, pixel, position, new GLIB.Point(position.X + length, position.Y), colour, thickness);
            SimpleDraw.draw(renderer, knob, knobBox, knobDestBox);
            //sb.Draw(knob, knobBox, Color.White);
        }
        /// <summary>
        /// P Property
        /// </summary>
        public int P
        {
            get { return p; }
            set
            {
                if (value < -1 & value > 99)
                {
                    p = 0;
                }
                else
                {
                    p = value;
                }
                    }
        }
    }
    /// <summary>
    /// SliderMenu class
    /// </summary>
    public class SliderMenu
    {
        //protected
        protected List<string> names;
        protected List<Slider> slides;
        protected SDI input;
        protected int index;
        protected bool locked;
        protected int space;
        protected int length; 
        protected Point2D topLeft;
        protected SDL.SDL_Color blue;
        protected SDL.SDL_Color gray;
        protected SDL.SDL_Color white;
        protected SDL.SDL_Color black;

        //public
        /// <summary>
        /// SliderMenu constructor
        /// </summary>
        /// <param name="names">Arrary of strings</param>
        /// <param name="knob">Texture2D reference</param>
        /// <param name="space">Space between sliders</param>
        /// <param name="length">Length of sliders</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public SliderMenu(string[] names, Texture2D knob, int space, int length, int x, int y)
        {
            for (int i = 0; i < names.Length; i++)
            {
                this.names.Add(names[i]);
                slides.Add(new Slider(knob, x, y + (i * space), length));
                this.space = space;
                topLeft = new Point2D(x, y);
                this.length = length;
                input = new SDI();
                locked = false;
                index = 0;
            }
            defineColours();
        }
        /// <summary>
        /// Defines a set of colours
        /// </summary>
        public void defineColours()
        {
            blue.r = 0;
            blue.g = 0;
            blue.b = 255;
            blue.a = 1;
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
        /// <returns>Boolean</returns>
        public bool update()
        {
            inputState inps = input.update();
            switch(inps)
            {
                case inputState.ll:
                    if (locked)
                    {
                        if (slides[index].P > 0)
                        {
                            slides[index].P--;
                        }
                    }
                    break;
                case inputState.lr:
                    if (locked)
                    {
                        if (slides[index].P < 99)
                        {
                            slides[index].P++;
                        }
                    }
                    break;
                case inputState.lu:
                    if (!locked)
                    {
                        if (index > 0)
                        {
                            index--;
                        }
                    }
                    break;
                case inputState.ld:
                    if (!locked)
                    {
                        if (index < slides.Count - 1)
                        {
                            index++;
                        }
                    }
                    break;
                case inputState.a:
                    if (locked)
                    {
                        locked = false;
                    }
                    else
                    {
                        locked = true;
                    }
                    break;
                case inputState.b:
                    return true;
                case inputState.none:
                    break;
            }
            return false;
        }
        /// <summary>
        /// Draws SliderMenu
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="shift">Shift value</param>
        public void draw(IntPtr renderer, int shift = 15)
        {
            Point2D pos1 = new Point2D(topLeft.X + length, topLeft.Y);
            Point2D pos2 = new Point2D(topLeft.X - shift, topLeft.Y);
            for (int i = 0; i < slides.Count; i++ )
            {
                if (i == index)
                {
                    slides[i].draw(renderer, blue);
                }
                else
                {
                    slides[i].draw(renderer, gray);
                }
                pos1.Y = i * space - (space / 2);
                pos2.Y = i * space;
                SimpleFont.DrawString(renderer, names[i], pos1.X, pos1.Y, white);
                //sb.DrawString(font, names[i], pos1, Color.White);
                SimpleFont.DrawString(renderer, slides[i].ToString(), pos2.X, pos2.Y, white);
                //sb.DrawString(font, slides[i].P.ToString(), pos2, Color.White);
            }
        }
        /// <summary>
        /// Names property
        /// </summary>
        public List<string> Names
        {
            get { return names; }
        }
        /// <summary>
        /// Slides property
        /// </summary>
        public List<Slider> Slides
        {
            get { return slides; }
        }
    }
}
