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
    /// MouseCursor class
    /// </summary>
    public class MouseCursor
    {
        //protected
        protected Texture2D source;
        protected Rectangle box;
        protected Rectangle srcBox;

        //public
        /// <summary>
        /// MouseCursor constructor
        /// </summary>
        /// <param name="source">Texture2D referecne</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public MouseCursor(Texture2D source, int width = 0, int height = 0)
        {
            this.source = source;
            if (width < 1 | height < 1)
            {
                box = new Rectangle(0, 0, source.width, source.height);
            }
            else
            {
                box = new Rectangle(0, 0, width, height);
            }
            srcBox = new Rectangle(0, 0, source.width, source.height);
        }
        /// <summary>
        /// Update cursor position on screen
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void update(int x, int y)
        {
            box.X = x;
            box.Y = y;
        }
        /// <summary>
        /// Draw MouseCursor
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            SimpleDraw.draw(renderer, source, srcBox, box);
            //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //sb.Draw(source, box, Color.White);
            //sb.End();
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
        /// <summary>
        /// Box property
        /// </summary>
        public Rectangle Box
        {
            get { return box; }
        }
    }
}
