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
    /// a drop down list of options that returns a string for the selected value.
    /// </summary>
    public class DropDownList
    {
        //protected
        protected SDL.SDL_Color backColor;
        protected SDL.SDL_Color textColor;
        protected SDI2 input;
        protected int index;
        protected Rectangle box;
        protected Rectangle clickBox;
        protected Rectangle selectBox;
        protected Point2D topLeft;
        protected Point2D temp;
        protected Point2D tip;
        protected bool active;
        protected List<Rectangle> testBoxes;

        //public
        /// <summary>
        /// DropDownList constructor
        /// </summary>
        /// <param name="backColor">Background colour</param>
        /// <param name="textColor">Text colour</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width of menu</param>
        /// <param name="height">Height of each option in menu</param>
        /// <param name="delay">Input delay</param>
        public DropDownList(SDL.SDL_Color backColor, SDL.SDL_Color textColor, int x, int y, int width, int height, int delay = 5)
        {
            this.backColor = backColor;
            this.textColor = textColor;
            topLeft = new Point2D(x, y);
            box = new Rectangle(x, y, width, height);
            clickBox = new Rectangle(x, y, 1, 1);
            selectBox = new Rectangle(x, y, width, 32);
            input = new SDI2(delay);
            index = 0;
            tip = new Point2D(0, 0);
            active = false;
            temp = new Point2D(x, y);
            testBoxes = new List<Rectangle>();
        }
        /// <summary>
        /// Updates internal state (depricated)
        /// </summary>
        /// <param name="options">Arrary of options to display</param>
        /// <param name="cursorBox">Cursor Rectangle reference</param>
        /// <param name="mlb">Mouse left button depression flag</param>
        /// <param name="w">Width of menu options</param>
        /// <param name="h">Height of menu options</param>
        /// <returns>String</returns>
        public string update(string[] options, Rectangle cursorBox, bool mlb, int w = 300, int h = 25)
        {
            if(testBoxes.Count != options.Length)
            {
                testBoxes = new List<Rectangle>();
                for(int i = 0; i < options.Length; i++)
                {
                    testBoxes.Add(new Rectangle(topLeft.X, topLeft.Y + i * h, w, h));
                }
            }
            for(int i = 0; i < testBoxes.Count; i++)
            {
                if(cursorBox.intersects(testBoxes[i]))
                {
                    index = i;
                    if(mlb)
                    {
                        return options[i];
                    }
                    break;
                }
            }
            return "";
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        /// <param name="options">Array of options</param>
        /// <param name="shift">Shift value (depricated)</param>
        /// <returns>String</returns>
        public string update(string[] options, int shift = 15)
        {
            //Rectangle clickBox = new Rectangle(avatar.DestBox.X, avatar.DestBox.Y - shift, 5, 5);
            //Rectangle selectBox = new Rectangle(topLeft.X, topLeft.Y, box.Width, (int)font.MeasureString("i").Y);
            tip.X = MouseHandler.getMouseX();
            tip.Y = MouseHandler.getMouseY();
            selectBox.X = topLeft.X;
            selectBox.Y = topLeft.Y;
            index = 0;
            for(int i = 0; i < options.Length; i++)
            {
                if(selectBox.pointInRect(tip) == true)
                {
                    index = i;
                    if(MouseHandler.getLeft())
                    {
                        active = false;
                        return options[i];
                    }
                    break;
                }
                selectBox.X = topLeft.X;
                selectBox.Y = topLeft.Y + (i * selectBox.Height);
            }
            return "";
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        /// <param name="options">Array of options</param>
        /// <param name="tip">Pointer tip Point2D</param>
        /// <returns>String</returns>
        public string update(string[] options, Point2D tip)
        {
            selectBox.X = topLeft.X;
            selectBox.Y = topLeft.Y;
            index = 0;
            for(int i = 0; i < options.Length; i++)
            {
                if(selectBox.pointInRect(tip) == true)
                {
                    index = i;
                    if(MouseHandler.getLeft() == true)
                    {
                        active = false;
                        if(i == 0)
                        {
                            return options[0];
                        }
                        return options[i - 1];
                    }
                    break;
                }
                selectBox.Y = topLeft.Y + (i * selectBox.Height);
            }
            return "";
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        /// <param name="options">Arrary of options</param>
        /// <returns>String</returns>
        public string update(string[] options)
        {
            inputState inps = input.update();
            switch(inps)
            {
                case inputState.lu:
                    if (index > 0)
                    {
                        index--;
                        selectBox.Y = (selectBox.Height * index) + topLeft.Y;
                    }
                    break;
                case inputState.ld:
                    if (index < options.Length - 1)
                    {
                        index++;
                        selectBox.Y = (selectBox.Height * index) + topLeft.Y;
                    }
                    break;
                case inputState.a:
                    active = false;
                    return options[index];
                case inputState.b:
                    active = false;
                    break;
            }
            return "";
        }
        /// <summary>
        /// Draws DropDownMenu
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="options">Array of options</param>
        public void draw(IntPtr renderer, string[] options)
        {
            temp.Y = topLeft.Y;
            box.X = topLeft.X;
            box.Y = topLeft.Y;
            temp.X = topLeft.X;
            SDL.SDL_Rect tmpDest;
            SDL.SDL_Color oldColour;
            //store old colour
            SDL.SDL_GetRenderDrawColor(renderer, out oldColour.r, out oldColour.g, out oldColour.b, out oldColour.a);
            box.Height = options.Length * selectBox.Height;
            tmpDest = box.Rect;
            //draw background
            SDL.SDL_SetRenderDrawColor(renderer, backColor.r, backColor.g, backColor.b, backColor.a);
            SDL.SDL_RenderFillRect(renderer, ref tmpDest);
            SDL.SDL_SetRenderDrawColor(renderer, oldColour.r, oldColour.g, oldColour.b, oldColour.a);
            //draw test for each option
            for (int i = 0; i < options.Length; i++)
            {                
                temp.Y = TopLeft.Y + (i * selectBox.Height);
                SimpleFont.DrawString(renderer, options[i], (int)temp.X, (int)temp.Y, textColor);
            }
            /*
            //draw selectBox
            tmpDest = hoverBox.Rect;
            SDL.SDL_SetRenderDrawColor(renderer, 255, 255, 255, 1);
            SDL.SDL_RenderDrawRect(renderer, ref tmpDest);
            SDL.SDL_SetRenderDrawColor(renderer, oldColour.r, oldColour.g, oldColour.b, oldColour.a);
            */
        }
        /// <summary>
        /// Tests if a point is in the selectBox
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Boolean</returns>
        public bool pointInBox(int x, int y)
        {
            if (x >= selectBox.X & x < selectBox.X + selectBox.Width)
            {
                if (y >= selectBox.Y & x < selectBox.Y + selectBox.Height)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Activates menu, resets index to zero if already active
        /// </summary>
        public void activate()
        {
            if (active)
            {
                active = false;
            }
            else
            {
                active = true;
                index = 0;
            }
        }
        /// <summary>
        /// TopLEft property
        /// </summary>
        public Point2D TopLeft
        {
            get { return topLeft; }
            set { topLeft = value; }
        }
        /// <summary>
        /// Active property
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }
        /// <summary>
        /// KeyBinder property
        /// </summary>
        public KeyBinder KeyBinder
        {
            get { return input.KB; }
        }
    }
}
