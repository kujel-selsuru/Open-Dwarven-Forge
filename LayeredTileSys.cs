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
    /// LayeredTileSys class
    /// </summary>
    public class LayeredTileSys
    {
        /// <summary>
        /// Tile class
        /// </summary>
        protected class Tile
        {
            //protected Point
            protected Rectangle sourceBox;
            protected SDL.SDL_Rect srcRect;
            protected Rectangle destBox;
            protected SDL.SDL_Rect destRect;
            protected Rectangle hitBox;
            protected Texture2D source;
            protected bool passible;

            //public
            /// <summary>
            /// Tile constructor
            /// </summary>
            /// <param name="source">Texture2D reference</param>
            /// <param name="x">X position</param>
            /// <param name="y">Y position</param>
            /// <param name="w">Width</param>
            /// <param name="h">Height</param>
            /// <param name="sx">Source X value</param>
            /// <param name="sy">Source Y value</param>
            /// <param name="passible">Passability flag</param>
            public Tile(Texture2D source, int x, int y, int w, int h, int sx, int sy, bool passible)
            {
                this.source = source;
                hitBox = new Rectangle(x, y, w, h);
                sourceBox = new Rectangle(sx * w, sy * h, w, h);
                destBox = new Rectangle(0, 0, w, h);
                this.passible = passible;
            }
            /// <summary>
            /// draws tile at designated grid index
            /// </summary>
            /// <param name="renderer">Renderer reference</param>
            /// <param name="x">Window x offset</param>
            /// <param name="y">Window y offset</param>
            public void draw(IntPtr renderer, int x, int y, int winx = 0, int winy = 0)
            {
                destBox.X = (x * destBox.Width) - winx;
                destBox.Y = (y * destBox.Height) - winy;
                srcRect = sourceBox.Rect;
                destRect = destBox.Rect;
                SDL.SDL_RenderCopy(renderer, source.texture, ref srcRect, ref destRect);
                //sb.Draw(source, destBox, sourceBox, Color.White);
            }
            /// <summary>
            /// Render tile
            /// </summary>
            /// <param name="renderer">Renderer reference</param>
            /// <param name="winx">Window x offset</param>
            /// <param name="winy">Window y offset</param>
            public void render(IntPtr renderer, int winx = 0, int winy = 0)
            {
                destBox.X = hitBox.X - winx;
                destBox.Y = hitBox.Y - winy;
                srcRect = sourceBox.Rect;
                destRect = destBox.Rect;
                SDL.SDL_RenderCopy(renderer, source.texture, ref srcRect, ref destRect);
                //sb.Draw(source, destBox, sourceBox, Color.White);
            }
            /// <summary>
            /// Passible property
            /// </summary>
            public bool Passible
            {
                get { return passible; }
            }
            /// <summary>
            /// Sets passibility flag
            /// </summary>
            /// <param name="passibility">Passibility flag value</param>
            public void setPassibility(bool passibility)
            {
                passible = passibility;
            }
            /// <summary>
            /// HitBox property
            /// </summary>
            public Rectangle HitBox
            {
                get { return hitBox; }
            }
            /// <summary>
            /// X property
            /// </summary>
            public float X
            {
                get { return hitBox.X; }
            }
            /// <summary>
            /// Y property
            /// </summary>
            public float Y
            {
                get { return hitBox.Y; }
            }
            /// <summary>
            /// SX property
            /// </summary>
            public float SX
            {
                get { return sourceBox.X; }
            }
            /// <summary>
            /// SY property
            /// </summary>
            public float SY
            {
                get { return sourceBox.Y; }
            }
        }

        protected Texture2D source;
        protected int winWidth;
        protected int winHeight;
        protected int winx;
        protected int winy;
        protected int width;
        protected int height;
        protected int tileWidth;
        protected int tileHeight;
        protected Tile[,] layer1;
        protected Tile[,] layer2;
        protected Tile[,] layer3;
        protected MapGraph mg;
        protected Point2D window;
        /// <summary>
        /// Handles all functionality of a region of tiles
        /// </summary>
        /// <param name="source">Tile sheet</param>
        /// <param name="width">Width of region in tiles</param>
        /// <param name="height">Height of region in tiles</param>
        /// <param name="winWidth">Width of window</param>
        /// <param name="winHeight">Height of window</param>
        /// <param name="winx">Window x start</param>
        /// <param name="winy">Window y start</param>
        /// <param name="tilew">Tile width</param>
        /// <param name="tileh">Tile height</param>
        /// <param name="fill">Fill the first layer with tiles at (0, 0) in tile sheet</param>
        public LayeredTileSys(Texture2D source, int width, int height, int winWidth, int winHeight, int winx, int winy, int tilew, int tileh, bool fill = false)
        {
            this.source = source;
            this.width = width;
            this.height = height;
            this.winWidth = winWidth;
            this.winHeight = winHeight;
            this.winx = winx;
            this.winy = winy;
            tileWidth = tilew;
            tileHeight = tileh;
            window = new Point2D(winx * tilew, winy * tileh);
            layer1 = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    layer1[x, y] = null;// new Tile(source, x, y, tilew, tileh, 0, 0, true);
                }
            }
            layer2 = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    layer2[x, y] = null;// new Tile(source, x, y, tilew, tileh, 0, 0, true);
                }
            }
            layer3 = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    layer3[x, y] = null;// new Tile(source, x, y, tilew, tileh, 0, 0, true);
                }
            }
            if (fill)
            {
                fillLayer(1, 0, 0);
            }
            mg = new MapGraph(width, height);
        }
        /// <summary>
        /// Handles all functionality of a region of tiles.
        /// this constructor builds from a file instead of defualt builds
        /// </summary>
        /// <param name="source">Tile sheet</param>
        /// <param name="width">Width of region in tiles</param>
        /// <param name="height">Height of region in tiles</param>
        /// <param name="winWidth">Width of window</param>
        /// <param name="winHeight">Height of window</param>
        /// <param name="winx">Window x start</param>
        /// <param name="winy">Window y start</param>
        /// <param name="tilew">Tile width</param>
        /// <param name="tileh">Tile height</param>
        public LayeredTileSys(Texture2D source, int width, int height, int winWidth, int winHeight, int winx, int winy, int tilew, int tileh, System.IO.StreamReader sr)
        {
            this.source = source;
            this.width = width;
            this.height = height;
            this.winWidth = winWidth;
            this.winHeight = winHeight;
            tileWidth = tilew;
            tileHeight = tileh;
            this.winx = winx;
            this.winy = winy;
            window = new Point2D(winx * tilew, winy * tileh);
            //layer 1
            sr.ReadLine();//discard testing data
            layer1 = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    string temp = sr.ReadLine();
                    if (temp != "null")
                    {
                        string temp2 = sr.ReadLine();
                        layer1[x, y] = new Tile(source, x * tilew, y * tileh, tilew, tileh, Convert.ToInt32(temp), Convert.ToInt32(temp2), true);
                    }
                    else
                    {
                        layer1[x, y] = null;
                    }
                }
            }
            //layer 2
            sr.ReadLine();//discard testing data
            layer2 = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    string temp = sr.ReadLine();
                    if (temp != "null")
                    {
                        layer2[x, y] = new Tile(source, x * tilew, y * tileh, tilew, tileh, Convert.ToInt32(temp), Convert.ToInt32(sr.ReadLine()), true);
                    }
                    else
                    {
                        layer2[x, y] = null;
                    }
                }
            }
            //layer 3
            sr.ReadLine();//discard testing data
            layer3 = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    string temp = sr.ReadLine();
                    if (temp != "null")
                    {
                        layer3[x, y] = new Tile(source, x * tilew, y * tileh, tilew, tileh, Convert.ToInt32(temp), Convert.ToInt32(sr.ReadLine()), true);
                    }
                    else
                    {
                        layer3[x, y] = null;
                    }
                }
            }
            mg = new MapGraph(1, 1);
            sr.ReadLine();//discard testing data
            mg.load(sr);
        }
        /// <summary>
        /// Handles all functionality of a region of tiles.
        /// this constructor builds from a file instead of defualt builds
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="winWidth">Window width</param>
        /// <param name="winHeight">Window height</param>
        /// <param name="sr">StreamReader reference</param>
        public LayeredTileSys(Texture2D source, int winWidth, int winHeight, System.IO.StreamReader sr)
        {
            this.source = source;
            this.winWidth = winWidth;
            this.winHeight = winHeight;
            sr.ReadLine();//discard testing data
            tileWidth = Convert.ToInt32(sr.ReadLine());//tilew;
            tileHeight = Convert.ToInt32(sr.ReadLine());//tileh;
            this.width = Convert.ToInt32(sr.ReadLine());//width;
            this.height = Convert.ToInt32(sr.ReadLine());//height;         
            this.winx = Convert.ToInt32(sr.ReadLine());//winx;
            this.winy = Convert.ToInt32(sr.ReadLine());//winy;
            window = new Point2D(winx * tileWidth, winy * tileHeight);
            //layer 1
            sr.ReadLine();//discard testing data
            layer1 = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    string temp = sr.ReadLine();
                    if (temp != "null")
                    {
                        string temp2 = sr.ReadLine();
                        layer1[x, y] = new Tile(source, x * tileWidth, y * tileHeight, tileWidth, tileHeight, Convert.ToInt32(temp), Convert.ToInt32(temp2), true);
                    }
                    else
                    {
                        layer1[x, y] = null;
                    }
                }
            }
            //layer 2
            sr.ReadLine();//discard testing data
            layer2 = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    string temp = sr.ReadLine();
                    if (temp != "null")
                    {
                        layer2[x, y] = new Tile(source, x * tileWidth, y * tileHeight, tileWidth, tileHeight, Convert.ToInt32(temp), Convert.ToInt32(sr.ReadLine()), true);
                    }
                    else
                    {
                        layer2[x, y] = null;
                    }
                }
            }
            //layer 3
            sr.ReadLine();//discard testing data
            layer3 = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    string temp = sr.ReadLine();
                    if (temp != "null")
                    {
                        layer3[x, y] = new Tile(source, x * tileWidth, y * tileHeight, tileWidth, tileHeight, Convert.ToInt32(temp), Convert.ToInt32(sr.ReadLine()), true);
                    }
                    else
                    {
                        layer3[x, y] = null;
                    }
                }
            }
            mg = new MapGraph(1, 1);
            sr.ReadLine();//discard testing data
            mg.load(sr);
        }
        /// <summary>
        /// Draws a window of region
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="windx">Window x position</param>
        /// <param name="windy">Window y position</param>
        /// <param name="shiftx">Shifit X value</param>
        /// <param name="shifty">Shift y value</param>
        public void draw(IntPtr renderer, int windx, int windy, int shiftx = 0, int shifty = 0)
        {
            //drawLayer(sb, 1, tileWidth, windx, windy);
            //drawLayer(sb, 2, tileWidth, windx, windy);
            //drawLayer(sb, 3, tileWidth, windx, windy);
            
            //layer1
            int i = 0;
            int k = 0;
            //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            for (int x = winx / tileWidth; x < winx / tileWidth + winWidth; x++)
            {
                for (int y = winy / tileHeight; y < winy / tileHeight + winHeight; y++)
                {
                    if (layer1[x, y] != null)
                    {
                        layer1[x, y].draw(renderer, x, y, windx - shiftx, windy - shifty);
                    }
                    k++;//screen x
                }
                k = 0;
                i++;//screen y
            }
            //sb.End();
            //layer2
            i = 0;
            k = 0;
            //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            for (int x = winx / tileWidth; x < winx / tileWidth + winWidth; x++)
            {
                for (int y = winy / tileHeight; y < winy / tileHeight + winHeight; y++)
                {
                    if (layer2[x, y] != null)
                    {
                        layer2[x, y].draw(renderer, x, y, windx - shiftx, windy - shifty);
                    }
                    k++;
                }
                k = 0;
                i++;
            }
            //sb.End();
            //layer3
            i = 0;
            k = 0;
            //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            for (int x = winx / tileWidth; x < winx / tileWidth + winWidth; x++)
            {
                for (int y = winy / tileHeight; y < winy / tileHeight + winHeight; y++)
                {
                    if (layer3[x, y] != null)
                    {
                        layer3[x, y].draw(renderer, x, y, windx - shiftx, windy - shifty);
                    }
                    k++;
                }
                k = 0;
                i++;
            }
            //sb.End();
             
        }
        /// <summary>
        /// Draws a specified layer
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="layer">Layer to draw</param>
        /// <param name="scale">Scaler value</param>
        /// <param name="shiftx">Shift X value</param>
        /// <param name="shifty">Shift y value</param>
        public void drawLayer(IntPtr renderer, int layer, int scale = 32, int shiftx = 0, int shifty = 0)
        {
            int i = 0;
            int k = 0;
            int startx = (winx / scale);
            if (startx < 0)
            {
                startx = 0;
            }
            int starty = (winy / scale);
            if (starty < 0)
            {
                starty = 0;
            }
            int endx = startx + winWidth;
            if (endx > width)
            {
                endx = width;
            }
            
            int endy = starty + winHeight;
            if (endy > height)
            {
                endy = height;
            }
            switch(layer)
            {
                case 1:
                    i = 0;
                    k = 0;
                    for (int y = starty; y < endy; y++)//int x = winx
                    {
                        for (int x = startx; x < endx; x++)//int y = winy
                        {
                            if (layer1[x, y] != null)
                            {
                                layer1[x, y].render(renderer, winx - shiftx, winy - shifty);
                            }
                            k++;//screen x
                        }
                        k = 0;
                        i++;//screen y
                    }
                    break;
                case 2:
                    i = 0;
                    k = 0;
                    for (int y = starty; y < endy; y++)//int x = winx
                    {
                        for (int x = startx; x < endx; x++)//int y = winy
                        {
                            if (layer2[x, y] != null)
                            {
                                layer2[x, y].render(renderer, winx - shiftx, winy - shifty);
                            }
                        k++;
                        }
                        k = 0;
                        i++;
                    }
                    break;
                case 3:
                    i = 0;
                    k = 0;
                    for (int y = starty; y < endy; y++)//int x = winx
                    {
                        for (int x = startx; x < endx; x++)//int y = winy
                        {
                            if (layer3[x, y] != null)
                            {
                                layer3[x, y].render(renderer, winx - shiftx, winy - shifty);
                            }
                        k++;
                        }
                        k = 0;
                        i++;
                    }
                    break;
            }
        }
        /// <summary>
        /// Draws a windowed section of specified layer
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="layer">Layer to draw</param>
        /// <param name="ww">Window width</param>
        /// <param name="wh">Window height</param>
        /// <param name="scale">Scaler value</param>
        /// <param name="shiftx">Shift x value</param>
        /// <param name="shifty">Shift y value</param>
        public void drawWindowLayer(IntPtr renderer, int layer, int ww = 12, int wh = 12, int scale = 32, int shiftx = 0, int shifty = 0)
        {
            int i = 0;
            int k = 0;
            int startx = (winx / scale);
            if (startx < 0)
            {
                startx = 0;
            }
            int starty = (winy / scale);
            if (starty < 0)
            {
                starty = 0;
            }
            int endx = startx + ww;
            if (endx > width)
            {
                endx = width;
            }

            int endy = starty + wh;
            if (endy > height)
            {
                endy = height;
            }
            switch (layer)
            {
                case 1:
                    i = 0;
                    k = 0;
                    for (int y = starty; y < endy; y++)//int x = winx
                    {
                        for (int x = startx; x < endx; x++)//int y = winy
                        {
                            if (layer1[x, y] != null)
                            {
                                layer1[x, y].render(renderer, winx - shiftx, winy - shifty);
                            }
                            k++;//screen x
                        }
                        k = 0;
                        i++;//screen y
                    }
                    break;
                case 2:
                    i = 0;
                    k = 0;
                    for (int y = starty; y < endy; y++)//int x = winx
                    {
                        for (int x = startx; x < endx; x++)//int y = winy
                        {
                            if (layer2[x, y] != null)
                            {
                                layer2[x, y].render(renderer, winx - shiftx, winy - shifty);
                            }
                            k++;
                        }
                        k = 0;
                        i++;
                    }
                    break;
                case 3:
                    i = 0;
                    k = 0;
                    for (int y = starty; y < endy; y++)//int x = winx
                    {
                        for (int x = startx; x < endx; x++)//int y = winy
                        {
                            if (layer3[x, y] != null)
                            {
                                layer3[x, y].render(renderer, winx - shiftx, winy - shifty);
                            }
                            k++;
                        }
                        k = 0;
                        i++;
                    }
                    break;
            }
        }
        /// <summary>
        /// Draws windowed section
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="scale">Scaler value</param>
        /// <param name="winx">Window x position</param>
        /// <param name="winy">Window y position</param>
        /// <param name="down">Shift down position</param>
        public void render(IntPtr renderer, int scale = 32, int winx = 0, int winy = 0, int down = 0)
        {
            //layer1
            int i = 0;
            int k = 0;
            int startx = (winx / scale);
            int starty = (winy / scale);
            int endx = startx + winWidth;
            if (endx > width)
            {
                endx = width;
            }
            int endy = starty + winHeight;
            if (endy > height)
            {
                endy = height;
            }
            //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            for (int x = startx; x < endx; x++)//int x = winx
            {
                for (int y = starty; y < endy; y++)//int y = winy
                {
                    if (layer1[x, y] != null)
                    {
                        layer1[x, y].render(renderer, winx, winy + down);
                    }
                    k++;//screen x
                }
                k = 0;
                i++;//screen y
            }
            //sb.End();
            //layer2
            i = 0;
            k = 0;
            //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            for (int x = startx; x < endx; x++)//int x = winx
            {
                for (int y = starty; y < endy; y++)//int y = winy
                {
                    if (layer2[x, y] != null)
                    {
                        layer2[x, y].render(renderer, winx, winy + down);
                    }
                    k++;
                }
                k = 0;
                i++;
            }
            //sb.End();
            //layer3
            i = 0;
            k = 0;
            //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            for (int x = startx; x < endx; x++)//int x = winx
            {
                for (int y = starty; y < endy; y++)//int y = winy
                {
                    if (layer3[x, y] != null)
                    {
                        layer3[x, y].render(renderer, winx, winy + down);
                    }
                    k++;
                }
                k = 0;
                i++;
            }
            //sb.End();
        }
        /// <summary>
        /// Move window (depricated)
        /// </summary>
        /// <param name="shiftx">Shift x value</param>
        /// <param name="shifty">Shift y value</param>
        public void moveWindow(int shiftx, int shifty)
        {
            if ((winx + shiftx >= 0) & (winx + winWidth + shiftx < width))
            {
                winx += shiftx;
            }
            if((winy + shifty >= 0) & (winy + winHeight + shifty < height))
            {
                winy += shifty;
            }
        }
        /// <summary>
        /// Shift window
        /// </summary>
        /// <param name="shiftx">Shift x value</param>
        /// <param name="shifty">Shift y value</param>
        public void shiftWindow(int shiftx, int shifty)
        {
            if (winx + shiftx >= 0 & winx + (winWidth * tileWidth) + shiftx <= (width * tileWidth))
            {
                winx += shiftx;
            }
            if (winy + shifty >= 0 & winy + (winHeight * tileHeight) + shifty <= (height * tileHeight))
            {
                winy += shifty;
            }
        }
        /// <summary>
        /// Test a position is in region domain
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <returns></returns>
        public bool inDomain(int x, int y)
        {
            if (x >= 0 & x < width)
            {
                if (y >= 0 & y < height)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Set a tile at speficied location
        /// </summary>
        /// <param name="layer">Layer of new tile</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="nsx">New sx value</param>
        /// <param name="nsy">New sy value</param>
        /// <param name="passible">Passiblity flag value</param>
        public void setTile(int layer, int x, int y, int nsx, int nsy, bool passible)
        {
            if (inDomain(x, y))
            {
                switch (layer)
                {
                    case 1:
                        layer1[x, y] = new Tile(source, x * tileWidth, y * tileHeight, tileWidth, tileHeight, nsx, nsy, passible);
                        //mg.setCell(x, y, passible);
                        break;
                    case 2:
                        layer2[x, y] = new Tile(source, x * tileWidth, y * tileHeight, tileWidth, tileHeight, nsx, nsy, passible);
                        //mg.setCell(x, y, passible);
                        break;
                    case 3:
                        layer3[x, y] = new Tile(source, x * tileWidth, y * tileHeight, tileWidth, tileHeight, nsx, nsy, passible);
                        mg.setCell(x, y, passible);
                        break;
                }
            }
        }
        /// <summary>
        /// Erase a tile at specified location
        /// </summary>
        /// <param name="layer">Layer of tile</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public void eraseTile(int layer, int x, int y)
        {
            if (inDomain(x, y))
            {
                switch (layer)
                {
                    case 1:
                        layer1[x, y] = null;
                        break;
                    case 2:
                        layer2[x, y] = null;
                        break;
                    case 3:
                        layer3[x, y] = null;
                        break;
                }
            }
        }
        /// <summary>
        /// Fill a layer with a specified tile
        /// </summary>
        /// <param name="layer">Layer to fill</param>
        /// <param name="nsx">Tile sx value</param>
        /// <param name="nsy">Tile sy value</param>
        /// <param name="passible">Tile passibility flag value</param>
        public void fillLayer(int layer, int nsx, int nsy, bool passible = true)
        {
            switch(layer)
            {
                case 1:
                    for (int i = 0; i < width; i++)
                    {
                        for (int k = 0; k < height; k++)
                        {
                            layer1[i, k] = new Tile(source, i * tileWidth, k * tileHeight, tileWidth, tileHeight, nsx, nsy, passible);
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i < width; i++)
                    {
                        for (int k = 0; k < height; k++)
                        {
                            layer2[i, k] = new Tile(source, i * tileWidth, k * tileHeight, tileWidth, tileHeight, nsx, nsy, passible);
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < width; i++)
                    {
                        for (int k = 0; k < height; k++)
                        {
                            layer3[i, k] = new Tile(source, i * tileWidth, k * tileHeight, tileWidth, tileHeight, nsx, nsy, passible);
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Fill an area from a position (layer 1)
        /// </summary>
        /// <param name="px">Position x</param>
        /// <param name="py">Position y</param>
        /// <param name="nsx">Tile sx value</param>
        /// <param name="nsy">Tile sy value</param>
        /// <param name="passible">Tile passibility flag value</param>
        public void fillAreaL1(int px, int py, int nsx, int nsy, bool passible = true)
        {
            Point2D start = new Point2D(px / tileWidth, py / tileHeight);
            int l = 0;
            int u = 0;
            int r = 0;
            int d = 0;
            //left fins
            l = 0;//reset l
            do
            {
                if (inDomain((int)(start.X - l), (int)start.Y))
                {
                    if (layer1[(int)(start.X - l), (int)start.Y] == null)
                    {
                        //fill upward
                        u = 0;//reset u
                        do
                        {
                            if (inDomain((int)(start.X - l), (int)(start.Y - u)))
                            {
                                if (layer1[(int)(start.X - l), (int)(start.Y - u)] == null)
                                {
                                    layer1[(int)(start.X - l), (int)(start.Y - u)] = new Tile(TextureBank.getTexture("tiles"), (int)((start.X - l) * tileWidth), (int)((start.Y - u) * tileHeight), tileWidth, tileHeight, nsx, nsy, true);
                                    u++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        } while (true);
                        //fill downward
                        d = 1;//reset d
                        do
                        {
                            if (inDomain((int)(start.X - l), (int)(start.Y + d)))
                            {
                                if (layer1[(int)(start.X - l), (int)(start.Y + d)] == null)
                                {
                                    layer1[(int)(start.X - l), (int)(start.Y + d)] = new Tile(TextureBank.getTexture("tiles"), (int)((start.X - l) * tileWidth), (int)((start.Y + d) * tileHeight), tileWidth, tileHeight, nsx, nsy, true);
                                    d++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        } while (true);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                l++;
            } while (true);
            //right fins
            r = 1;//reset r
            do
            {
                if (inDomain((int)(start.X + r), (int)(start.Y)))
                {
                    if (layer1[(int)(start.X + r), (int)(start.Y)] == null)
                    {
                        //fill upward
                        u = 0;//reset u
                        do
                        {
                            if (inDomain((int)(start.X + r), (int)(start.Y - u)))
                            {
                                if (layer1[(int)(start.X + r), (int)(start.Y - u)] == null)
                                {
                                    layer1[(int)(start.X + r), (int)(start.Y - u)] = new Tile(TextureBank.getTexture("tiles"), (int)((start.X + r) * tileWidth), (int)((start.Y - u) * tileHeight), tileWidth, tileHeight, nsx, nsy, true);
                                    u++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        } while (true);
                        //fill downward
                        d = 1;//reset d
                        do
                        {
                            if (inDomain((int)(start.X + r), (int)(start.Y + d)))
                            {
                                if (layer1[(int)(start.X + r), (int)(start.Y + d)] == null)
                                {
                                    layer1[(int)(start.X + r), (int)(start.Y + d)] = new Tile(TextureBank.getTexture("tiles"), (int)((start.X + r) * tileWidth), (int)((start.Y + d) * tileHeight), tileWidth, tileHeight, nsx, nsy, true);
                                    d++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        } while (true);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                r++;
            } while (true);

        }
        /// <summary>
        /// Fill an area from a position (layer 2)
        /// </summary>
        /// <param name="px">Position x</param>
        /// <param name="py">Position y</param>
        /// <param name="nsx">Tile sx value</param>
        /// <param name="nsy">Tile sy value</param>
        /// <param name="passible">Tile passibility flag value</param>
        public void fillAreaL2(int px, int py, int nsx, int nsy, bool passible = true)
        {
            Point2D start = new Point2D(px / tileWidth, py / tileHeight);
            int l = 0;
            int u = 0;
            int r = 0;
            int d = 0;
            //left fins
            l = 0;//reset l
            do
            {
                if (inDomain((int)(start.X - l), (int)start.Y))
                {
                    if (layer2[(int)(start.X - l), (int)start.Y] == null)
                    {
                        //fill upward
                        u = 0;//reset u
                        do
                        {
                            if (inDomain((int)(start.X - l), (int)(start.Y - u)))
                            {
                                if (layer2[(int)(start.X - l), (int)(start.Y - u)] == null)
                                {
                                    layer2[(int)(start.X - l), (int)(start.Y - u)] = new Tile(TextureBank.getTexture("tiles"), (int)((start.X - l) * tileWidth), (int)((start.Y - u) * tileHeight), tileWidth, tileHeight, nsx, nsy, true);
                                    u++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        } while (true);
                        //fill downward
                        d = 1;//reset d
                        do
                        {
                            if (inDomain((int)(start.X - l), (int)(start.Y + d)))
                            {
                                if (layer2[(int)(start.X - l), (int)(start.Y + d)] == null)
                                {
                                    layer2[(int)(start.X - l), (int)(start.Y + d)] = new Tile(TextureBank.getTexture("tiles"), (int)((start.X - l) * tileWidth), (int)((start.Y + d) * tileHeight), tileWidth, tileHeight, nsx, nsy, true);
                                    d++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        } while (true);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                l++;
            } while (true);
            //right fins
            r = 1;//reset r
            do
            {
                if (inDomain((int)(start.X + r), (int)start.Y))
                {
                    if (layer2[(int)(start.X + r), (int)start.Y] == null)
                    {
                        //fill upward
                        u = 0;//reset u
                        do
                        {
                            if (inDomain((int)(start.X + r), (int)(start.Y - u)))
                            {
                                if (layer2[(int)(start.X + r), (int)(start.Y - u)] == null)
                                {
                                    layer2[(int)(start.X + r), (int)(start.Y - u)] = new Tile(TextureBank.getTexture("tiles"), (int)((start.X + r) * tileWidth), (int)((start.Y - u) * tileHeight), tileWidth, tileHeight, nsx, nsy, true);
                                    u++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        } while (true);
                        //fill downward
                        d = 1;//reset d
                        do
                        {
                            if (inDomain((int)(start.X + r), (int)(start.Y + d)))
                            {
                                if (layer2[(int)(start.X + r), (int)(start.Y + d)] == null)
                                {
                                    layer2[(int)(start.X + r), (int)(start.Y + d)] = new Tile(TextureBank.getTexture("tiles"), (int)((start.X + r) * tileWidth), (int)((start.Y + d) * tileHeight), tileWidth, tileHeight, nsx, nsy, true);
                                    d++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        } while (true);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                r++;
            } while (true);

        }
        /// <summary>
        /// Fill an area from a position (layer 3)
        /// </summary>
        /// <param name="px">Position x</param>
        /// <param name="py">Position y</param>
        /// <param name="nsx">Tile sx value</param>
        /// <param name="nsy">Tile sy value</param>
        /// <param name="passible">Tile passibility flag value</param>
        public void fillAreaL3(int px, int py, int nsx, int nsy, bool passible = true)
        {
            Point2D start = new Point2D(px / tileWidth, py / tileHeight);
            int l = 0;
            int u = 0;
            int r = 0;
            int d = 0;
            //left fins
            l = 0;//reset l
            do
            {
                if (inDomain((int)(start.X - l), (int)start.Y))
                {
                    if (layer3[(int)(start.X - l), (int)start.Y] == null)
                    {
                        //fill upward
                        u = 0;//reset u
                        do
                        {
                            if (inDomain((int)(start.X - l), (int)(start.Y - u)))
                            {
                                if (layer3[(int)(start.X - l), (int)(start.Y - u)] == null)
                                {
                                    layer3[(int)(start.X - l), (int)(start.Y - u)] = new Tile(TextureBank.getTexture("tiles"), (int)((start.X - l) * tileWidth), (int)((start.Y - u) * tileHeight), tileWidth, tileHeight, nsx, nsy, true);
                                    u++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        } while (true);
                        //fill downward
                        d = 1;//reset d
                        do
                        {
                            if (inDomain((int)(start.X - l), (int)(start.Y + d)))
                            {
                                if (layer3[(int)(start.X - l), (int)(start.Y + d)] == null)
                                {
                                    layer3[(int)(start.X - l), (int)(start.Y + d)] = new Tile(TextureBank.getTexture("tiles"), (int)((start.X - l) * tileWidth), (int)((start.Y + d) * tileHeight), tileWidth, tileHeight, nsx, nsy, true);
                                    d++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        } while (true);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                l++;
            } while (true);
            //right fins
            r = 1;//reset r
            do
            {
                if (inDomain((int)(start.X + r), (int)start.Y))
                {
                    if (layer3[(int)(start.X + r), (int)start.Y] == null)
                    {
                        //fill upward
                        u = 0;//reset u
                        do
                        {
                            if (inDomain((int)(start.X + r), (int)(start.Y - u)))
                            {
                                if (layer3[(int)(start.X + r), (int)(start.Y - u)] == null)
                                {
                                    layer3[(int)(start.X + r), (int)(start.Y - u)] = new Tile(TextureBank.getTexture("tiles"), (int)((start.X + r) * tileWidth), (int)((start.Y - u) * tileHeight), tileWidth, tileHeight, nsx, nsy, true);
                                    u++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        } while (true);
                        //fill downward
                        d = 1;//reset d
                        do
                        {
                            if (inDomain((int)(start.X + r), (int)(start.Y + d)))
                            {
                                if (layer3[(int)(start.X + r), (int)(start.Y + d)] == null)
                                {
                                    layer3[(int)(start.X + r), (int)(start.Y + d)] = new Tile(TextureBank.getTexture("tiles"), (int)((start.X + r) * tileWidth), (int)((start.Y + d) * tileHeight), tileWidth, tileHeight, nsx, nsy, true);
                                    d++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        } while (true);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                r++;
            } while (true);

        }
        /// <summary>
        /// Erase all tiles in a layer
        /// </summary>
        /// <param name="layer">Layer to erase tiles in</param>
        public void eraseLayer(int layer = 1)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    switch (layer)
                    {
                        case 1:
                            layer1[x, y] = null;
                            break;
                        case 2:
                            layer2[x, y] = null;
                            break;
                        case 3:
                            layer3[x, y] = null;
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Formats board and sets new dimensions
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="width">New width of region</param>
        /// <param name="height">New height of region</param>
        /// <param name="winx">Window x position</param>
        /// <param name="winy">Window y position</param>
        public void formatTileBoard(Texture2D source, int width, int height, int winx = 0, int winy = 0)
        {
            this.source = source;
            this.width = width;
            this.height = height;
            this.winx = winx;
            this.winy = winy;
            window = new Point2D(winx * this.tileWidth, winy * this.tileHeight);
            layer1 = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    layer1[x, y] = null;
                }
            }
            layer2 = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    layer2[x, y] = null;
                }
            }
            layer3 = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    layer3[x, y] = null;
                }
            }
            mg = new MapGraph(width, height);
        }
        /// <summary>
        /// Save region's data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("####### map setup #######");
            sw.WriteLine(width.ToString());
            sw.WriteLine(height.ToString());
            sw.WriteLine(tileWidth.ToString());
            sw.WriteLine(tileHeight.ToString());
            sw.WriteLine(winx.ToString());
            sw.WriteLine(winy.ToString());
            sw.WriteLine("####### layer1 #######");
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (layer1[x, y] == null)
                    {
                        sw.WriteLine("null");
                    }
                    else
                    {
                        sw.WriteLine(layer1[x, y].SX.ToString());
                        sw.WriteLine(layer1[x, y].SY.ToString());
                    }
                }
            }
            sw.WriteLine("####### layer2 #######");
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (layer2[x, y] == null)
                    {
                        sw.WriteLine("null");
                    }
                    else
                    {
                        sw.WriteLine(layer2[x, y].SX.ToString());
                        sw.WriteLine(layer2[x, y].SY.ToString());
                    }
                }
            }
            sw.WriteLine("####### layer3 #######");
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (layer3[x, y] == null)
                    {
                        sw.WriteLine("null");
                    }
                    else
                    {
                        sw.WriteLine(layer3[x, y].SX.ToString());
                        sw.WriteLine(layer3[x, y].SY.ToString());
                    }
                }
            }
            sw.WriteLine("####### Map Graph #######");
            mg.save(sw);
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
        public int WinWidth
        {
            get { return winWidth; }
            set { winWidth = value; }
        }
        /// <summary>
        /// WinHeight property
        /// </summary>
        public int WinHeight
        {
            get { return winHeight; }
            set { winHeight = value; }
        }
        /// <summary>
        /// window x in pixels
        /// </summary>
        public int Winx
        {
            get { return winx; }
            set { winx = value; }
        }
        /// <summary>
        /// window y in pixels
        /// </summary>
        public int Winy
        {
            get { return winy; }
            set { winy = value; }
        }
        /// <summary>
        /// MapGraph property
        /// </summary>
        public MapGraph MG
        {
            get { return mg; }
            set { mg = value; }
        }
        /// <summary>
        /// TileWidth property
        /// </summary>
        public int TileWidth
        {
            get { return tileWidth; }
        }
        /// <summary>
        /// TileHeight property
        /// </summary>
        public int TileHeight
        {
            get { return tileHeight; }
        }
        /// <summary>
        /// returns a point containing the tile source coordinates
        /// </summary>
        /// <param name="layer">layer to get from</param>
        /// <param name="x">x tile</param>
        /// <param name="y">y tile</param>
        /// <param name="tilew">tile width</param>
        /// <param name="tileh">tile height</param>
        /// <returns>Point2D</returns>
        public Point2D tileAt(int layer, int x, int y, int tilew, int tileh)
        {
            if (x >= 0 & x < width)
            {
                if(y >= 0 & y < height)
                {
                    if (layer == 1)
                    {
                        if (layer1[x, y] != null)
                        {
                            return new Point2D(layer1[x, y].SX / tilew, layer1[x, y].SY / tileh);
                        }
                    }
                    else if (layer == 2)
                    {
                        if (layer2[x, y] != null)
                        {
                            return new Point2D(layer2[x, y].SX / tilew, layer2[x, y].SY / tileh); ;
                        }
                    }
                    else if(layer == 3)
                    {
                        if (layer3[x, y] != null)
                        {
                            return new Point2D(layer3[x, y].SX / tilew, layer3[x, y].SY / tileh);
                        }
                    }
                }
            }
            return new Point2D(0, 0);
        }
        /// <summary>
        /// returns true if contains no tile else returns false indicating a tile present
        /// </summary>
        /// <param name="layer">layer to get from</param>
        /// <param name="x">x tile</param>
        /// <param name="y">y tile</param>
        /// <param name="tilew">tile width</param>
        /// <param name="tileh">tile height</param>
        /// <returns>Boolean</returns>
        public bool noTileAt(int layer, int x, int y, int tilew, int tileh)
        {
            if (x >= 0 & x < width)
            {
                if (y >= 0 & y < height)
                {
                    if (layer == 1)
                    {
                        if (layer1[x, y] != null)
                        {
                            return false;
                        }
                    }
                    else if (layer == 2)
                    {
                        if (layer2[x, y] != null)
                        {
                            return false;
                        }
                    }
                    else if (layer == 3)
                    {
                        if (layer3[x, y] != null)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Source property
        /// </summary>
        public Texture2D Source
        {
            get { return source; }
        }
    }
}
