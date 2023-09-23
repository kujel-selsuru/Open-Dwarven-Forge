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
    /// RadioButtonPanel class
    /// </summary>
    public class RadioButtonPanel
    {
        //protected
        protected Texture2D selected;
        protected Texture2D unselected;
        protected Rectangle srcBox;
        protected Rectangle destBox;
        protected Rectangle[] choices;
        protected string[] names;
        protected SimpleButton2 doneButton;
        protected int option;
        protected int numOptions;
        protected Point2D pos;

        //public
        /// <summary>
        /// RadioButtonPanel constructor
        /// </summary>
        /// <param name="circleButtonPressed">Texture2D referecne</param>
        /// <param name="circleButtonDepressed">Texture2D referecne</param>
        /// <param name="doneButtonPressed">Texture2D referecne</param>
        /// <param name="doneButtonDepressed">Texture2D referecne</param>
        /// <param name="numOptions">Number of optons</param>
        /// <param name="w">Width of radio buttons</param>
        /// <param name="h">Height of radio buttons</param>
        /// <param name="x">X positoin</param>
        /// <param name="y">Y position</param>
        /// <param name="names">String arrary of options names</param>
        public RadioButtonPanel(Texture2D circleButtonPressed, Texture2D circleButtonDepressed, Texture2D doneButtonPressed, 
            Texture2D doneButtonDepressed, int numOptions, int w, int h, int x, int y, string[] names)
        {
            selected = circleButtonPressed;
            unselected = circleButtonDepressed;
            srcBox = new Rectangle(0, 0, w, h);
            destBox = new Rectangle(x, x, w, h);
            option = -1;
            doneButton = new SimpleButton2(doneButtonDepressed, doneButtonPressed, x + w, y + h, "done");
            this.numOptions = numOptions;
            choices = new Rectangle[numOptions];
            this.names = new string[numOptions];
            for (int i = 0; i < numOptions; i++)
            {
                choices[i] = new Rectangle(x, y + (i * 32), 32, 32);
                this.names[i] = names[i];
            }
            pos = new Point2D(x, y);
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        /// <param name="cursor">SimpleCursor reference</param>
        /// <param name="eve">SDL_Event referecne</param>
        /// <returns>String</returns>
        public string update(SimpleCursor cursor)
        {
            if (cursor.getMBS() == MBS.left)
            {
                for (int i = 0; i < numOptions; i++)
                {
                    if (choices[i].intersects(cursor.Box))
                    {
                        option = i;
                        break;
                    }
                }
            }
            return doneButton.click(cursor.DestBox, cursor.getMBS());
        }
        /// <summary>
        /// Draws RadioButtonPanel
        /// </summary>
        /// <param name="renderer">Renderer referecne</param>
        /// <param name="textColor">Colour of text</param>
        public void draw(IntPtr renderer, SDL.SDL_Color textColor)
        {
            //sb.Draw(back, backBox, colour);
            for (int i = 0; i < numOptions; i++)
            {
                if (i == option)
                {
                    destBox.Y = pos.Y + (destBox.Height * i);
                    SimpleDraw.draw(renderer, selected, srcBox, destBox);
                    //sb.Draw(selected, choices[i], Color.White);
                }
                else
                {
                    destBox.Y = pos.Y + (destBox.Height * i);
                    SimpleDraw.draw(renderer, unselected, srcBox, destBox);
                    //sb.Draw(unselected, choices[i], Color.White);
                }
                SpriteFont.draw(renderer, names[i], choices[i].X + 32, choices[i].Y, textColor);
                //sb.DrawString(font, names[i], new Vector2(choices[i].X + 32, choices[i].Y), textColor);
            }
            //doneButton.draw(sb);
        }
        /// <summary>
        /// Draws RadioButtonPanel + done button
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="textColor">Colour of text</param>
        public void draw2(IntPtr renderer, SDL.SDL_Color textColor)
        {
            draw(renderer, textColor);
            doneButton.draw(renderer);
        }
        /// <summary>
        /// Option property
        /// </summary>
        public int Option
        {
            get { return option; }
            set { option = value; }
        }
        /// <summary>
        /// Names property
        /// </summary>
        public string[] Names
        {
            get { return names; }
        }
        /// <summary>
        /// Choice property
        /// </summary>
        public string Choice
        {
            get
            {
                if (option != -1)
                {
                    return names[option];
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// Pos property
        /// </summary>
        public Point2D Pos
        {
            get { return pos; }
        }
    }

    /// <summary>
    /// RadioButton class for stand alone radio buttons
    /// </summary>
    public class RadioButton
    {
        //protected
        protected Texture2D src;
        protected bool state;
        protected string name;
        protected Rectangle srcRect;
        protected Rectangle destRect;
        protected CoolDown delay;
        protected Rectangle clickBox;

        //public
        /// <summary>
        /// RadioButton constructor
        /// </summary>
        /// <param name="src">Texture2D reference</param>
        /// <param name="initState">Initial state of button</param>
        /// <param name="name">Name of button</param>
        /// <param name="x">X position in pixels</param>
        /// <param name="y">Y position in pixels</param>
        /// <param name="w">Width in pixels</param>
        /// <param name="h">Height in pixels</param>
        /// <param name="delay">Input delay value</param>
        public RadioButton(Texture2D src, bool initState, string name, int x, int y, int w, int h, int delay = 5)
        {
            this.src = src;
            state = initState;
            this.name = name;
            srcRect = new Rectangle(0, 0, w, h);
            destRect = new Rectangle(x, y, w, h);
            this.delay = new CoolDown(delay);
            clickBox = new Rectangle(0, 0, 1, 1);
        }
        /// <summary>
        /// Updates Radio/button internal state
        /// </summary>
        /// <param name="mBox">Mouse box rectangle reference</param>
        /// <param name="mlb">Mouse left button state</param>
        public void update(Rectangle mBox, bool mlb)
        {
            delay.update();
            if (mlb == true)
            {
                if (destRect.intersects(mBox))
                {
                    if (delay.Active == false)
                    {
                        if (state == false)
                        {
                            state = true;
                        }
                        else
                        {
                            state = false;
                        }
                        delay.activate();
                    }
                }
            }
        }
        /// <summary>
        /// Updates Radio/button internal state
        /// </summary>
        public void update()
        {
            delay.update();
            clickBox.IX = MouseHandler.getMouseX();
            clickBox.IY = MouseHandler.getMouseY();
            if (MouseHandler.getLeft() == true)
            {
                if (destRect.intersects(clickBox))
                {
                    if (delay.Active == false)
                    {
                        if (state == false)
                        {
                            state = true;
                        }
                        else
                        {
                            state = false;
                        }
                        delay.activate();
                    }
                }
            }
        }
        /// <summary>
        /// Draws just the RadioButton
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            if(state)
            {
                srcRect.X = 0;
            }
            else
            {
                srcRect.X = srcRect.Width;
            }
            SimpleDraw.draw(renderer, src, srcRect, destRect);
        }
        /// <summary>
        /// Draws the RadioButton name to right hand side
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="scaler">Scaling value for text</param>
        public void drawName(IntPtr renderer, float scaler = 1)
        {
            SimpleFont.DrawString(renderer, name, destRect.X + destRect.Width, destRect.Y, scaler);
        }
        /// <summary>
        /// State property
        /// </summary>
        public bool State
        {
            get { return state; }
            set { state = value; }
        }
        /// <summary>
        /// Name property
        /// </summary>
        public string Name
        {
            get { return name; }
        }
    }
}
