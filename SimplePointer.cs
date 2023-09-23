//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenoLib
{
    /// <summary>
    /// SimplePointer class, just provides a visual representation of pointer on screen
    /// </summary>
    public class SimplePointer
    {
        //protected
        protected Texture2D source;
        protected Rectangle srcRect;
        protected Rectangle destRect;

        //public
        /// <summary>
        /// SimplePointer constructor
        /// </summary>
        /// <param name="source">Texture2D referecne</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        public SimplePointer(Texture2D source, int x, int y, int w, int h)
        {
            this.source = source;
            srcRect = new Rectangle(0, 0, w, h);
            destRect = new Rectangle(x, y, w, h);
        }
        /// <summary>
        /// Updates SimplePointer position
        /// </summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        public void update(int px, int py)
        {
            destRect.X = px;
            destRect.Y = py;
        }
        /// <summary>
        /// Draws SimplePointer
        /// </summary>
        /// <param name="renderer">renderer reference</param>
        public void draw(IntPtr renderer)
        {
            SimpleDraw.draw(renderer, source, srcRect, destRect);
        }
        /// <summary>
        /// HitBox property
        /// </summary>
        public Rectangle HitBox
        {
            get { return destRect; }
        }
        /// <summary>
        /// X property
        /// </summary>
        public int X
        {
            get { return (int)destRect.X; }
            set { destRect.X = value; }
        }
        /// <summary>
        /// Y property
        /// </summary>
        public int Y
        {
            get { return (int)destRect.Y; }
            set { destRect.Y = value; }
        }
        /// <summary>
        /// Width property
        /// </summary>
        public int W
        {
            get { return (int)destRect.Width;  }
        }
        /// <summary>
        /// Height property
        /// </summary>
        public int H
        {
            get { return (int)destRect.Height; }
        }
        /// <summary>
        /// Tip property (top left corner)
        /// </summary>
        public Point2D Tip
        {
            get { return new Point2D(destRect.X, destRect.Y); }
        }
    }
}
