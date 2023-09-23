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
    /// A class that handles drawing x by y tiles from a single source texture by deviding 
    /// the source texture up into width by height tiles of size tw by th pixels. This class 
    /// does not enforce a source texture size.
    /// </summary>
    public class TileSys
    {
        /// <summary>
        /// Tile Class
        /// </summary>
        public class Tile
        {
            //protected
            protected int x, y, width, height, sx, sy;

            //public 
            /// <summary>
            /// Tile constructor
            /// </summary>
            /// <param name="x">X position</param>
            /// <param name="y">Y position</param>
            /// <param name="w">Width</param>
            /// <param name="h">Height</param>
            /// <param name="sx">Source X</param>
            /// <param name="sy">Source Y</param>
            public Tile(int x, int y, int w, int h, int sx = 0, int sy = 0)
            {
                this.x = x;
                this.y = y;
                width = w;
                height = h;
                this.sx = sx;
                this.sy = sy;
            }
            /// <summary>
            /// X property
            /// </summary>
            public int X
            {
                get { return x; }
            }
            /// <summary>
            /// Y property
            /// </summary>
            public int Y
            {
                get { return y; }
            }
            /// <summary>
            /// SX property
            /// </summary>
            public int SX
            {
                get { return sx; }
                set { sx = value; }
            }
            /// <summary>
            /// SY property
            /// </summary>
            public int SY
            {
                get { return sy; }
                set { sy = value; }
            }
        }
        
        protected int tw, th, width, height, winwidth, winheight, winx, winy;
        protected Tile[,] tiles;
        protected Texture2D source;

        //public
        /// <summary>
        /// TileSys class
        /// </summary>
        /// <param name="tw">Tile width</param>
        /// <param name="th">Tile height</param>
        /// <param name="width">Width in tiles</param>
        /// <param name="height">Height in tiles</param>
        /// <param name="winwidth">Window width in tiles</param>
        /// <param name="winheight">Window height in tiles</param>
        /// <param name="winx">Window x position</param>
        /// <param name="winy">Window y position</param>
        /// <param name="source">Tile source Texture2D reference</param>
        public TileSys(int tw, int th, int width, int height, int winwidth, int winheight, int winx, int winy, Texture2D source)
        {
            this.tw = tw;
            this.th = th;
            this.width = width;
            this.height = height;
            this.winwidth = winwidth;
            this.winheight = winheight;
            this.winx = winx;
            this.winy = winy;
            this.source = source;
            tiles = new Tile[width, height];
            
            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    tiles[i, k] = new Tile(i*tw, k*th, tw, th);
                }
            }
        }
        /// <summary>
        /// Draw TileSys
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            //sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            for(int i = winx; i < winx+winwidth; i++)
            {
                for (int k = winy; k < winy + winheight; k++)
                {
                    SimpleDraw.draw(renderer, source, new Rectangle((i * tw) - (winx * tw), (k * th) - (winy * th), tw, th), new Rectangle(tiles[i, k].SX, tiles[i, k].SY, tw, th));
                    //sb.Draw(source, new Rectangle((i * tw)-(winx*tw), (k * th)-(winy*th), tw, th), new Rectangle(tiles[i, k].SX, tiles[i, k].SY, tw, th), Color.White);
                }
            }
            //sb.End();
        }
        /// <summary>
        /// Winx property
        /// </summary>
        public int Winx
        {
            get { return winx; }
            set
            {
                winx = value;
                if (winx < 0)
                    winx = 0;
                if (winx + winwidth > width)
                    winx = width - winwidth;
            }
        }
        /// <summary>
        /// Winy property
        /// </summary>
        public int Winy
        {
            get { return winy; }
            set 
            { 
                winy = value;
                if (winy < 0)
                    winy = 0;
                if (winy + winheight > height)
                    winy = height - winheight;
            }
        }
        /// <summary>
        /// Width property
        /// </summary>
        public int Width
        {
            get { return width; }
        }
        /// <summary>
        /// Height property
        /// </summary>
        public int Height
        {
            get { return height; }
        }
        /// <summary>
        /// WinWidth property
        /// </summary>
        public int Winwidth
        {
            get { return winwidth; }
        }
        /// <summary>
        /// WinHeight property
        /// </summary>
        public int Winheight
        {
            get { return winheight; }
        }
        /// <summary>
        /// Set a tile 
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="nsx">New source x value</param>
        /// <param name="nsy">New source y value</param>
        public void SetTile(int x, int y, int nsx, int nsy)
        {
            tiles[x, y].SX = nsx;
            tiles[x, y].SY = nsy;
        }
    }
}
