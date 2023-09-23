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
    /// WireSlidingPallet
    /// </summary>
    public class WireSlidingPallet
    {
        //protected
        protected Texture2D source;
        protected int scroll;
        protected Point2D stamp;
        protected Rectangle window;
        protected SimpleWireButton up;
        protected SimpleWireButton down;
        protected Rectangle srcBox;
        protected Rectangle destBox;
        protected Rectangle box;
        protected int maxScroll;
        protected int scale;
        protected SDL.SDL_Color white;

        //public
        /// <summary>
        /// WireSlidingPallet
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="pw">Pallet width in tiles</param>
        /// <param name="ph">Pallet height in tiles</param>
        /// <param name="scale">Scaler value/tile dimensions</param>
        public WireSlidingPallet(Texture2D source, int x, int y, int pw = 7, int ph = 15, int scale = 32)
        {
            this.source = source;
            scroll = 0;
            stamp = new Point2D(0, 0);
            window = new Rectangle(0, 0, pw * scale, ph * scale);
            up = new SimpleWireButton(x + (pw * scale), y, 32, 32, "up");
            down = new SimpleWireButton(x + (pw * scale), (ph + 1) * scale, 32, 32, "down");
            destBox = new Rectangle(0, 0, pw * scale, ph * scale);
            destBox = new Rectangle(x, y, pw * scale, ph * scale);
            destBox = new Rectangle(x, y, scale, scale);
            maxScroll = (int)((source.height - window.Height) / scale);
            this.scale = scale;
            white.r = 255;
            white.g = 255;
            white.b = 255;
            white.a = 1;
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        /// <param name="cursor">SimpleCursor reference</param>
        public void update(SimpleCursor cursor)
        {
            if (up.click(cursor.DestBox) == "up")
            {
                if (scroll > 0)
                {
                    scroll--;
                }
            }
            else if (down.click(cursor.DestBox) == "down")
            {
                if (scroll < maxScroll)
                {
                    scroll++;
                }
            }
            else if (cursor.getMBS() == MBS.left)
            {
                if (destBox.intersects(cursor.Box))
                {
                    stamp.X = (cursor.DestBox.X - destBox.X) / 32;
                    stamp.Y = (cursor.DestBox.Y - destBox.Y) / 32;
                }
            }
            window.Y = scroll * 32;
        }
        /// <summary>
        /// Update buttons
        /// </summary>
        /// <param name="cursor">SimpleCursor reference</param>
        public void updateButtons(SimpleCursor cursor)
        {
            up.update();
            down.update();
            if (up.click(cursor.DestBox) == "up")
            {
                if (scroll > 0)
                {
                    scroll--;
                }
            }
            else if (down.click(cursor.DestBox) == "down")
            {
                if (scroll < maxScroll)
                {
                    scroll++;
                }
            }
        }
        /// <summary>
        /// Draw WireFramePallet
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            SimpleDraw.draw(renderer, source, srcBox, destBox);
            //sb.Draw(source, destBox, window, white);
            DrawRects.drawRect(renderer, box, white, false);
            //Line.drawSquare(sb, pixel, Box.X + stamp.X * scale, Box.Y + stamp.Y * scale, scale, scale, Color.White, 1);
            up.draw(renderer, white);
            down.draw(renderer, white);
        }
        /// <summary>
        /// Source property
        /// </summary>
        public Texture2D Source
        {
            get { return source; }
            set
            {
                source = value;
                maxScroll = (int)((source.height - window.Height) / scale);
                scroll = 0;
            }
        }
        /// <summary>
        /// Stamp property
        /// </summary>
        public Point2D Stamp
        {
            get { return stamp; }
        }
        /// <summary>
        /// DestBox property
        /// </summary>
        public Rectangle DestBox
        {
            get { return destBox; }
        }
        /// <summary>
        /// SrcBox property
        /// </summary>
        public Rectangle SrcBox
        {
            get { return srcBox; }
        }
        /// <summary>
        /// Box property
        /// </summary>
        public Rectangle Box
        {
            get { return Box; }
        }
    }
}
