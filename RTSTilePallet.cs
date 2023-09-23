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
    /// tile stamp for transfering data from tile pallet
    /// </summary>
    public class TileStamp
    {
        //public
        /// <summary>
        /// Tile stamp for placing tiles based on selected tile in pallet
        /// </summary>
        /// <param name="sx">tile source x</param>
        /// <param name="sy">tile source y</param>
        /// <param name="tileWidth">tile width</param>
        /// <param name="tileHeight">tile height</param>
        /// <param name="sourceName">tile source name</param>
        /// <param name="source">tile source texture</param>
        /// <param name="terrainValue">value of tile's terrain</param>
        public TileStamp(int sx, int sy, int tileWidth, int tileHeight, string sourceName, Texture2D source, int terrainValue = 0)
        {
            this.sx = sx;
            this.sy = sy;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.sourceName = sourceName;
            this.source = source;
            this.terrainValue = terrainValue;
        }
        public int sx;
        public int sy;
        public int tileWidth;
        public int tileHeight;
        public string sourceName;
        public Texture2D source;
        public int terrainValue;
    };
    /// <summary>
    /// handles tile source image and terrain values
    /// </summary>
    public class RTSTilePallet
    {
        //protected
        protected int width;
        protected int height;
        protected int tileWidth;
        protected int tileHeight;
        protected int tilesX;
        protected int tilesY;
        protected int stampX;
        protected int stampY;
        protected int[,] terrainValues;
        protected Texture2D source;
        protected Texture2D pixel;
        protected string sourceName;
        protected Rectangle srcRect;
        protected Rectangle destRect;
        protected int sourceWidth;
        protected int sourceHeight;
        protected Rectangle box;
        protected TileStamp stamp;
        //public
        /// <summary>
        /// tile pallet constructor
        /// </summary>
        /// <param name="x">pallet x position</param>
        /// <param name="y">pallet y position</param>
        /// <param name="width">pallet width in pixels</param>
        /// <param name="height">pallet height in pixels</param>
        /// <param name="tileWidth">tile width in pixels</param>
        /// <param name="tileHeight">tile height in pixels</param>
        /// <param name="source">source texture</param>
        /// <param name="pixel">pixel texture</param>
        /// <param name="sourceName">source name</param>
        /// <param name="sourceWidth">source width in pixels</param>
        /// <param name="sourceHeight">source height in pixels</param>
        public RTSTilePallet(int x, int y, int width, int height, int tileWidth, int tileHeight,
            Texture2D source, Texture2D pixel, string sourceName, int sourceWidth, int sourceHeight)
        {
            this.width = width;
            this.height = height;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.source = source;
            this.pixel = pixel;
            this.sourceName = sourceName;
            this.sourceWidth = sourceWidth;
            this.sourceHeight = sourceHeight;
            srcRect.X = 0;
            srcRect.Y = 0;
            srcRect.Width = width;
            srcRect.Height = height;
            destRect.X = x;
            destRect.Y = y;
            destRect.Width = width;
            destRect.Height = height;
            terrainValues = new int[width / tileWidth, height / tileHeight];
            tilesX = width / tileWidth;
            tilesY = height / tileHeight;
            stampX = 0;
            stampY = 0;
            box = new Rectangle(x, y, width, height);
            stamp = new TileStamp(0, 0, tileWidth, tileHeight, sourceName, source);
        }
        /// <summary>
        /// handles clicking the tile pallet
        /// </summary>
        public void clicked()
        {
            if (KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_LCTRL) == true)
            {
                if (MouseHandler.getLeft() == true)
                {
                    if (terrainValues[(int)(MouseHandler.getMouseX() - destRect.X) / tileWidth, (int)(MouseHandler.getMouseY() - destRect.Y) / tileHeight] < 10)
                    {
                        terrainValues[(int)(MouseHandler.getMouseX() - destRect.X) / tileWidth, (int)(MouseHandler.getMouseY() - destRect.Y) / tileHeight]++;
                    }

                }
                else if (MouseHandler.getRight() == true)
                {
                    if (terrainValues[(int)(MouseHandler.getMouseX() - destRect.X) / tileWidth, (int)(MouseHandler.getMouseY() - destRect.Y) / tileHeight] > -1)
                    {
                        terrainValues[(int)(MouseHandler.getMouseX() - destRect.X) / tileWidth, (int)(MouseHandler.getMouseY() - destRect.Y) / tileHeight]--;
                    }
                }
            }
            else if (MouseHandler.getLeft() == true)
            {
                stampX = (int)((MouseHandler.getMouseX() - destRect.X) / tileWidth) * tileWidth;
                stampY = (int)((MouseHandler.getMouseY() - destRect.Y) / tileHeight) * tileHeight;
                stamp.sx = stampX;
                stamp.sy = stampY;
                stamp.terrainValue = terrainValues[stamp.sx / tileWidth, stamp.sy / tileHeight];
            }
        }
        /// <summary>
        /// draws the tile pallet
        /// </summary>
        /// <param name="renderer">renderer reference</param>
        /// <param name="scaler">text scaler value</param>
        public void draw(IntPtr renderer, float scaler)
        {
            Rectangle selectedBox = new Rectangle(destRect.X + stampX, destRect.Y + stampY, tileWidth, tileHeight);
            SimpleDraw.draw(renderer, source, srcRect, destRect);
            DrawRects.drawRect(renderer, selectedBox, ColourBank.getColour(XENOCOLOURS.MAGENTA), false);
            for (int tx = 0; tx < tilesX; tx++)
            {
                for (int ty = 0; ty < tilesY; ty++)
                {
                    SimpleFont.DrawString(renderer, terrainValues[tx, ty].ToString(),
                        destRect.X + (tx * tileWidth), destRect.Y + (ty * tileHeight), scaler);
                }
            }
        }
        /// <summary>
        /// source name property
        /// </summary>
        public string SourceName
        {
            get { return sourceName; }
            set { sourceName = value; }
        }
        /// <summary>
        /// source property
        /// </summary>
        public Texture2D Source
        {
            get { return source; }
            set { source = value; }
        }
        /// <summary>
        /// stamp property
        /// </summary>
        public TileStamp Stamp
        {
            get { return stamp; }
        }
        /// <summary>
        /// box property
        /// </summary>
        public Rectangle Box
        {
            get { return box; }
        }
    }
}
