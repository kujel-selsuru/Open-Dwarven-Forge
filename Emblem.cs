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
    /// Emblem class
    /// </summary>
    public class Emblem
    {
        //protected
        protected Texture2D layer1;
        protected Texture2D layer2;
        protected SDL.SDL_Color colour1;
        protected SDL.SDL_Color colour2;
        protected Rectangle box;
        protected Rectangle box2;

        //public
        /// <summary>
        /// Emblem constructor
        /// </summary>
        /// <param name="layer1">Texture2D reference</param>
        /// <param name="layer2">Texture2D reference</param>
        /// <param name="colour1">SDL_Color</param>
        /// <param name="colour2">SDL_Color</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        public Emblem(Texture2D layer1, Texture2D layer2, SDL.SDL_Color colour1, SDL.SDL_Color colour2, int x, int y, int w, int h)
        {
            this.layer1 = layer1;
            this.layer2 = layer2;
            this.colour1 = colour1;
            this.colour2 = colour2;
            box = new Rectangle(0, 0, w, h);
            box2 = new Rectangle(x, y, w, h);
        }
        /// <summary>
        /// Draw Emblem
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            SimpleDraw.draw(renderer, layer1, box, box2);
            //sb.Draw(layer1, box, colour1);
            SimpleDraw.draw(renderer, layer2, box, box2);
            //sb.Draw(layer2, box, colour2);
            //sb.End();
        }
        /// <summary>
        /// Layer1 property
        /// </summary>
        public Texture2D Layer1
        {
            get { return layer1; }
            set { layer1 = value; }
        }
        /// <summary>
        /// Layer2 property
        /// </summary>
        public Texture2D Layer2
        {
            get { return layer2; }
            set { layer2 = value; }
        }
        /// <summary>
        /// Colour1 property
        /// </summary>
        public SDL.SDL_Color Colour1
        {
            get { return colour1; }
            set { colour1 = value; }
        }
        /// <summary>
        /// Colour2 property
        /// </summary>
        public SDL.SDL_Color Colour2
        {
            get { return colour2; }
            set { colour2 = value; }
        }
        /// <summary>
        /// X property
        /// </summary>
        public float X
        {
            get { return box.X; }
            set { box.X = value; }
        }
        /// <summary>
        /// Y property
        /// </summary>
        public float Y
        {
            get { return box.Y; }
            set { box.Y = value; }
        }
    }
}
