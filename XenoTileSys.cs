//====================================================
//Written by Kujel Selsuru
//Last Updated 22/12/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// XenoTileSys class
    /// </summary>
    public class XenoTileSys
    {
        /// <summary>
        /// XenoTile class
        /// </summary>
        public class XenoTile
        {
            //protected 
            protected Point2D pos;
            protected SDL.SDL_Rect srcRect;
            protected SDL.SDL_Rect destRect;
            protected Texture2D source;

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
            public XenoTile(Texture2D source, int x, int y, int w, int h, int sx, int sy)
            {
                this.source = source;
                pos = new Point2D(x, y);
                srcRect.x = sx;
                srcRect.y = sy;
                srcRect.w = w;
                srcRect.h = h;
                destRect.x = x;
                destRect.y = x;
                destRect.w = w;
                destRect.h = h;


            }
            /// <summary>
            /// Tile copy constructor
            /// </summary>
            /// <param name="obj">XenoTile reference</param>
            public XenoTile(XenoTile obj)
            {
                source = obj.source;
                pos = new Point2D(obj.pos.X, obj.pos.Y);
                srcRect.x = obj.srcRect.x;
                srcRect.y = obj.srcRect.y;
                srcRect.w = obj.srcRect.w;
                srcRect.h = obj.srcRect.h;
                destRect.x = obj.destRect.x;
                destRect.y = obj.destRect.y;
                destRect.w = obj.destRect.w;
                destRect.h = obj.destRect.h;
            }
            /// <summary>
            /// draws tile at designated grid index
            /// </summary>
            /// <param name="renderer">Renderer reference</param>
            /// <param name="x">Window x offset</param>
            /// <param name="y">Window y offset</param>
            public void draw(IntPtr renderer, int winx = 0, int winy = 0)
            {
                destRect.x = pos.IX - winx;
                destRect.y = pos.IY - winy;
                SDL.SDL_RenderCopy(renderer, source.texture, ref srcRect, ref destRect);
                //sb.Draw(source, destBox, sourceBox, Color.White);
            }
            /// <summary>
            /// X property
            /// </summary>
            public float X
            {
                get { return pos.X; }
                set { pos.X = value; }
            }
            /// <summary>
            /// Y property
            /// </summary>
            public float Y
            {
                get { return pos.Y; }
                set { pos.Y = value; }
            }
            /// <summary>
            /// SX property
            /// </summary>
            public int SX
            {
                get { return srcRect.x; }
                set { srcRect.x = value; }
            }
            /// <summary>
            /// SY property
            /// </summary>
            public int SY
            {
                get { return srcRect.y; }
                set { srcRect.y = value; }
            }
        }

        protected Texture2D source;
        protected int winx;
        protected int winy;
        protected int winWidth;
        protected int winHeight;
        protected int width;
        protected int height;
        protected int tileWidth;
        protected int tileHeight;
        protected XenoTile[,] layer1;
        protected XenoTile[,] layer2;
        protected XenoTile[,] layer3;
        protected Rectangle srcRect;
        protected Rectangle destRect;

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
        /// <param name="fill">Fill the first layer with tiles at (random sx, sy) in tile sheet</param>
        /// <param name="sourceW">Source Width in tiles</param>
        /// <param name="sourceH">Source Height in tiles</param>
        public XenoTileSys(Texture2D source, int width, int height, int winWidth, int winHeight, int winx, int winy, int tilew, int tileh, bool fill = false, bool randomize = false, int sourceW = 5, int sourceH = 7)
        {
            this.source = source;
            this.width = width;
            this.height = height;
            this.winx = winx;
            this.winy = winy;
            this.winWidth = winWidth;
            this.winHeight = winHeight;
            tileWidth = tilew;
            tileHeight = tileh;
            srcRect = new Rectangle(0, 0, tileWidth, tileHeight);
            destRect = new Rectangle(0, 0, tileWidth, tileHeight);
            layer1 = new XenoTile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    layer1[x, y] = null;// new Tile(source, x, y, tilew, tileh, 0, 0, true);
                }
            }
            layer2 = new XenoTile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    layer2[x, y] = null;// new Tile(source, x, y, tilew, tileh, 0, 0, true);
                }
            }
            layer3 = new XenoTile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    layer3[x, y] = null;// new Tile(source, x, y, tilew, tileh, 0, 0, true);
                }
            }
            if (fill == true && randomize == false)
            {
                fillLayer(1, 0, 0);
            }
            if (fill == false && randomize == true)
            {
                randomFillLayer(1, sourceW, sourceH);
            }
        }
        /// <summary>
        /// XenoTileSys copy construcotr
        /// </summary>
        /// <param name="obj">XenoTileSys reference</param>
        public XenoTileSys(XenoTileSys obj)
        {
            this.source = obj.Source;
            this.width = obj.Width;
            this.height = obj.Height;
            this.winx = obj.winx;
            this.winy = obj.winy;
            this.winWidth = obj.WinWidth;
            this.winHeight = obj.WinHeight;
            tileWidth = obj.TileWidth;
            tileHeight = obj.TileHeight;
            srcRect = new Rectangle(obj.srcRect.X, obj.srcRect.Y, obj.srcRect.Width, obj.srcRect.Height);
            destRect = new Rectangle(obj.destRect.X, obj.destRect.Y, obj.destRect.Width, obj.destRect.Height);
            layer1 = new XenoTile[obj.Width, obj.Height];
            for (int x = 0; x < obj.Width; x++)
            {
                for (int y = 0; y < obj.Height; y++)
                {
                    layer1[x, y] = null;// new Tile(source, x, y, tilew, tileh, 0, 0, true);
                }
            }
            layer2 = new XenoTile[obj.Width, obj.Height];
            for (int x = 0; x < obj.Width; x++)
            {
                for (int y = 0; y < obj.Height; y++)
                {
                    layer2[x, y] = null;// new Tile(source, x, y, tilew, tileh, 0, 0, true);
                }
            }
            layer3 = new XenoTile[obj.Width, obj.Height];
            for (int x = 0; x < obj.Width; x++)
            {
                for (int y = 0; y < obj.Height; y++)
                {
                    layer3[x, y] = null;// new Tile(source, x, y, tilew, tileh, 0, 0, true);
                }
            }
        }
        /// <summary>
        /// Handles all functionality of a region of tiles.
        /// this constructor builds from a file instead of defualt builds
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="winWidth">Window width</param>
        /// <param name="winHeight">Window height</param>
        /// <param name="sr">StreamReader reference</param>
        public XenoTileSys(Texture2D source, int winWidth, int winHeight, System.IO.StreamReader sr)
        {
            this.source = source;
            this.winWidth = winWidth;
            this.winHeight = winHeight;
            sr.ReadLine();//discard testing data
            width = Convert.ToInt32(sr.ReadLine());//tilew;
            height = Convert.ToInt32(sr.ReadLine());//tileh;
            tileWidth = Convert.ToInt32(sr.ReadLine());//width;
            tileHeight = Convert.ToInt32(sr.ReadLine());//height;
            srcRect = new Rectangle(0, 0, tileWidth, tileHeight);
            destRect = new Rectangle(0, 0, tileWidth, tileHeight);
            //layer 1
            sr.ReadLine();//discard testing data
            layer1 = new XenoTile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    string temp = sr.ReadLine();
                    if (temp != "null")
                    {
                        string temp2 = sr.ReadLine();
                        layer1[x, y] = new XenoTile(source, x * tileWidth, y * tileHeight, tileWidth, tileHeight, Convert.ToInt32(temp), Convert.ToInt32(temp2));
                    }
                    else
                    {
                        layer1[x, y] = null;
                    }
                }
            }
            //layer 2
            sr.ReadLine();//discard testing data
            layer2 = new XenoTile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    string temp = sr.ReadLine();
                    if (temp != "null")
                    {
                        layer2[x, y] = new XenoTile(source, x * tileWidth, y * tileHeight, tileWidth, tileHeight, Convert.ToInt32(temp), Convert.ToInt32(sr.ReadLine()));
                    }
                    else
                    {
                        layer2[x, y] = null;
                    }
                }
            }
            //layer 3
            sr.ReadLine();//discard testing data
            layer3 = new XenoTile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    string temp = sr.ReadLine();
                    if (temp != "null")
                    {
                        layer3[x, y] = new XenoTile(source, x * tileWidth, y * tileHeight, tileWidth, tileHeight, Convert.ToInt32(temp), Convert.ToInt32(sr.ReadLine()));
                    }
                    else
                    {
                        layer3[x, y] = null;
                    }
                }
            }
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
        }
        /// <summary>
        /// Draws a window of region
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">World window x value</param>
        /// <param name="winy">World window y value</param>
        /// <param name="shiftx">Shifit x value</param>
        /// <param name="shifty">Shift y value</param>
        public void draw(IntPtr renderer, int winx, int winy, int shiftx = 0, int shifty = 0)
        {
            //layer1
            //win(x/y) is tile (x/y) to draw so remove win(x/y) to shift to zero, shift(x/y) is window offset and mod(x/y) is pixel offset
            //int modx = winx % (tileWidth - 1);
            //int mody = winy % (tileHeight - 1);
            int offsetX = winx + shiftx;// + modx;
            int offsetY = winy + shifty;// + mody;
            for (int x = winx / tileWidth; x < winx / tileWidth + winWidth; x++)
            {
                for (int y = winy / tileHeight; y < winy / tileHeight + winHeight; y++)
                {
                    if (inDomain(x, y))
                    {
                        if (layer1[x, y] != null)
                        {

                            layer1[x, y].draw(renderer, offsetX, offsetY);
                        }
                    }
                }
            }
            //layer2
            for (int x = winx / tileWidth; x < winx / tileWidth + winWidth; x++)
            {
                for (int y = winy / tileHeight; y < winy / tileHeight + winHeight; y++)
                {
                    if (inDomain(x, y))
                    {
                        if (layer2[x, y] != null)
                        {
                            layer2[x, y].draw(renderer, offsetX, offsetY);
                        }
                    }
                }
            }
            //layer3
            for (int x = winx / tileWidth; x < winx / tileWidth + winWidth; x++)
            {
                for (int y = winy / tileHeight; y < winy / tileHeight + winHeight; y++)
                {
                    if (inDomain(x, y))
                    {
                        if (layer3[x, y] != null)
                        {
                            layer3[x, y].draw(renderer, offsetX, offsetY);
                        }
                    }
                }
            }

        }
        /// <summary>
        /// Draws a specified layer
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window X value</param>
        /// <param name="winy">Window y value</param>
        /// <param name="layer">Layer to draw</param>
        /// <param name="scale">Scaler value</param>
        /// <param name="shiftx">Shift X value</param>
        /// <param name="shifty">Shift y value</param>
        public void drawLayer(IntPtr renderer, int winx, int winy, int layer, int scale = 32, int shiftx = 0, int shifty = 0)
        {
            //win(x/y) is tile (x/y) to draw so remove win(x/y) to shift to zero, shift(x/y) is window offset and mod(x/y) is pixel offset
            //int modx = winx % (tileWidth - 1);
            //int mody = winy % (tileHeight - 1);
            int offsetX = winx + shiftx;// + modx;
            int offsetY = winy + shifty;// + mody;
            int startx = (winx / scale);
            int starty = (winy / scale);
            int endx = startx + winWidth;
            int endy = starty + winHeight;

            switch (layer)
            {
                case 1:
                    for (int y = starty; y < endy; y++)
                    {
                        for (int x = startx; x < endx; x++)
                        {
                            if (inDomain(x, y))
                            {
                                if (layer1[x, y] != null)
                                {
                                    layer1[x, y].draw(renderer, offsetX, offsetY);
                                }
                            }
                        }
                    }
                    break;
                case 2:
                    for (int y = starty; y < endy; y++)
                    {
                        for (int x = startx; x < endx; x++)
                        {
                            if (inDomain(x, y))
                            {
                                if (layer2[x, y] != null)
                                {
                                    layer2[x, y].draw(renderer, offsetX, offsetY);
                                }
                            }
                        }
                    }
                    break;
                case 3:
                    for (int y = starty; y < endy; y++)
                    {
                        for (int x = startx; x < endx; x++)
                        {
                            if (inDomain(x, y))
                            {
                                if (layer3[x, y] != null)
                                {
                                    layer3[x, y].draw(renderer, offsetX, offsetY);
                                }
                            }
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// checks if the tile space at layer and position is null
        /// </summary>
        /// <param name="layer">Layer value</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Boolean</returns>
        public bool noTile(int layer, int x, int y)
        {
            if(inDomain(x, y))
            {
                switch(layer)
                {
                    case 1:
                        if (layer1[x, y] == null)
                        {
                            return true;
                        }
                        break;
                    case 2:
                        if (layer2[x, y] == null)
                        {
                            return true;
                        }
                        break;
                    case 3:
                        if (layer3[x, y] == null)
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }
        /// <summary>
        /// Sets window position
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">y position</param>
        public void setWindow(int x, int y)
        {
            winx = x;
            winy = y;
        }
        /// <summary>
        /// Test a position is in region domain
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <returns>Boolean</returns>
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
        /// <param name="x">X position in tiles</param>
        /// <param name="y">Y position in tiles</param>
        /// <param name="nsx">New sx value in pixels</param>
        /// <param name="nsy">New sy value in pixels</param>
        public void setTile(int layer, int x, int y, int nsx, int nsy)
        {
            if (inDomain(x, y))
            {
                switch (layer)
                {
                    case 1:
                        layer1[x, y] = new XenoTile(source, x * tileWidth, y * tileHeight, tileWidth, tileHeight, nsx, nsy);
                        //mg.setCell(x, y, passible);
                        break;
                    case 2:
                        layer2[x, y] = new XenoTile(source, x * tileWidth, y * tileHeight, tileWidth, tileHeight, nsx, nsy);
                        //mg.setCell(x, y, passible);
                        break;
                    case 3:
                        layer3[x, y] = new XenoTile(source, x * tileWidth, y * tileHeight, tileWidth, tileHeight, nsx, nsy);
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
        public void fillLayer(int layer, int nsx, int nsy)
        {
            switch (layer)
            {
                case 1:
                    for (int i = 0; i < width; i++)
                    {
                        for (int k = 0; k < height; k++)
                        {
                            layer1[i, k] = new XenoTile(source, i * tileWidth, k * tileHeight, tileWidth, tileHeight, nsx, nsy);
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i < width; i++)
                    {
                        for (int k = 0; k < height; k++)
                        {
                            layer2[i, k] = new XenoTile(source, i * tileWidth, k * tileHeight, tileWidth, tileHeight, nsx, nsy);
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < width; i++)
                    {
                        for (int k = 0; k < height; k++)
                        {
                            layer3[i, k] = new XenoTile(source, i * tileWidth, k * tileHeight, tileWidth, tileHeight, nsx, nsy);
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Fill a layer with a randomized tiles
        /// </summary>
        /// <param name="layer">Layer to fill</param>
        /// <param name="sourceW">Source width in tiles</param>
        /// <param name="sourceH">Source height in tiles</param>
        public void randomFillLayer(int layer, int sourceW = 5, int sourceH = 7)
        {
            Random rand = new Random((int)System.DateTime.Today.Ticks);
            int sx = 0;
            int sy = 0;
            switch (layer)
            {
                case 1:
                    for (int i = 0; i < width; i++)
                    {
                        for (int k = 0; k < height; k++)
                        {
                            sx = (rand.Next(0, (sourceW * 100)) / 100) * tileWidth;
                            sy = (rand.Next(0, (sourceH * 100)) / 100) * tileHeight;
                            layer1[i, k] = new XenoTile(source, i * tileWidth, k * tileHeight, tileWidth, tileHeight, sx, sy);
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i < width; i++)
                    {
                        for (int k = 0; k < height; k++)
                        {
                            sx = rand.Next(0, (sourceW * 100)) / 100;
                            sy = rand.Next(0, (sourceH * 100)) / 100;
                            layer2[i, k] = new XenoTile(source, i * tileWidth, k * tileHeight, tileWidth, tileHeight, sx, sy);
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < width; i++)
                    {
                        for (int k = 0; k < height; k++)
                        {
                            sx = rand.Next(0, (sourceW * 100)) / 100;
                            sy = rand.Next(0, (sourceH * 100)) / 100;
                            layer3[i, k] = new XenoTile(source, i * tileWidth, k * tileHeight, tileWidth, tileHeight, sx, sy);
                        }
                    }
                    break;
            }
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
        public void formatTileBoard(Texture2D source, int width, int height)
        {
            this.source = source;
            this.width = width;
            this.height = height;
            //window = new Point2D(winx * this.tileWidth, winy * this.tileHeight);
            layer1 = new XenoTile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    layer1[x, y] = null;
                }
            }
            layer2 = new XenoTile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    layer2[x, y] = null;
                }
            }
            layer3 = new XenoTile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    layer3[x, y] = null;
                }
            }
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
                if (y >= 0 & y < height)
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
                    else if (layer == 3)
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
        /// Tests if a rectangle intersects with any tiles int layer
        /// </summary>
        /// <param name="rect">Rectangle reference</param>
        /// <returns>Boolean</returns>
        public bool intersectLayer1Rect(Rectangle rect)
        {
            if (rect.Width / tileWidth > 1)
            {
                if (rect.Height / tileHeight > 1)
                {
                    //width and height are greater than one tile
                    for (int x = (int)rect.X / tileWidth; x < (int)rect.X / tileWidth + (int)rect.Width / tileWidth; x++)
                    {
                        for (int y = (int)rect.Y / tileHeight; y < (int)rect.Y / tileHeight + (int)rect.Height / tileHeight; y++)
                        {
                            if (layer1[x, y] != null)
                            {
                                return true;
                            }
                        }
                    }
                }
                else//height is not greater than one tile but width is
                {
                    for (int x = (int)rect.X / tileWidth; x < (int)rect.X / tileWidth + (int)rect.Width / tileWidth; x++)
                    {
                        if (layer1[x, (int)rect.Y / tileHeight] != null)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (rect.Height / tileHeight > 1)
                {
                    //width is not greater than one tile but height is
                    for (int y = (int)rect.Y / tileHeight; y < (int)rect.Y / tileHeight + (int)rect.Height / tileHeight; y++)
                    {
                        if (layer1[(int)rect.X / tileWidth, y] != null)
                        {
                            return true;
                        }
                    }
                }
            }
            //rect is one tile or smaller
            if (layer1[(int)rect.X / tileWidth, (int)rect.Y / tileHeight] != null)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Tests if a rectangle intersects with any tiles int layer
        /// </summary>
        /// <param name="rect">Rectangle reference</param>
        /// <returns>Boolean</returns>
        public bool intersectLayer2Rect(Rectangle rect)
        {
            if (rect.Width / tileWidth > 1)
            {
                if (rect.Height / tileHeight > 1)
                {
                    //width and height are greater than one tile
                    for (int x = (int)rect.X / tileWidth; x < (int)rect.X / tileWidth + (int)rect.Width / tileWidth; x++)
                    {
                        for (int y = (int)rect.Y / tileHeight; y < (int)rect.Y / tileHeight + (int)rect.Height / tileHeight; y++)
                        {
                            if (layer2[x, y] != null)
                            {
                                return true;
                            }
                        }
                    }
                }
                else//height is not greater than one tile but width is
                {
                    for (int x = (int)rect.X / tileWidth; x < (int)rect.X / tileWidth + (int)rect.Width / tileWidth; x++)
                    {
                        if (layer2[x, (int)rect.Y / tileHeight] != null)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (rect.Height / tileHeight > 1)
                {
                    //width is not greater than one tile but height is
                    for (int y = (int)rect.Y / tileHeight; y < (int)rect.Y / tileHeight + (int)rect.Height / tileHeight; y++)
                    {
                        if (layer2[(int)rect.X / tileWidth, y] != null)
                        {
                            return true;
                        }
                    }
                }
            }
            //rect is one tile or smaller
            if (layer2[(int)rect.X / tileWidth, (int)rect.Y / tileHeight] != null)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Tests if a rectangle intersects with any tiles int layer
        /// </summary>
        /// <param name="rect">Rectangle reference</param>
        /// <returns>Boolean</returns>
        public bool intersectLayer3Rect(Rectangle rect)
        {
            if (rect.Width / tileWidth > 1)
            {
                if (rect.Height / tileHeight > 1)
                {
                    //width and height are greater than one tile
                    for (int x = (int)rect.X / tileWidth; x < (int)rect.X / tileWidth + (int)rect.Width / tileWidth; x++)
                    {
                        for (int y = (int)rect.Y / tileHeight; y < (int)rect.Y / tileHeight + (int)rect.Height / tileHeight; y++)
                        {
                            if (layer3[x, y] != null)
                            {
                                return true;
                            }
                        }
                    }
                }
                else//height is not greater than one tile but width is
                {
                    for (int x = (int)rect.X / tileWidth; x < (int)rect.X / tileWidth + (int)rect.Width / tileWidth; x++)
                    {
                        if (layer3[x, (int)rect.Y / tileHeight] != null)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (rect.Height / tileHeight > 1)
                {
                    //width is not greater than one tile but height is
                    for (int y = (int)rect.Y / tileHeight; y < (int)rect.Y / tileHeight + (int)rect.Height / tileHeight; y++)
                    {
                        if (layer3[(int)rect.X / tileWidth, y] != null)
                        {
                            return true;
                        }
                    }
                }
            }
            //rect is one tile or smaller
            if (layer3[(int)rect.X / tileWidth, (int)rect.Y / tileHeight] != null)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Tests if a rectangle intersects a given layer of tiles
        /// </summary>
        /// <param name="layer">Layer value</param>
        /// <param name="rect">Rectangle reference</param>
        /// <returns></returns>
        public bool intersectRect(int layer, Rectangle rect)
        {
            switch(layer)
            {
                case 1:
                    return intersectLayer1Rect(rect);
                case 2:
                    return intersectLayer2Rect(rect);
                case 3:
                    return intersectLayer3Rect(rect);
            }
            return false;
        }
        /// <summary>
        /// Returns a tile srcRect
        /// </summary>
        /// <param name="layer">Layer value</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Rectangle</returns>
        public Rectangle getTileSrcRect(int layer, int x, int y)
        {
            Rectangle rect = null;
            switch (layer)
            {
                case 1:
                    rect = new Rectangle(layer1[x, y].SX, layer1[x, y].SY, tileWidth, tileHeight);
                    break;
                case 2:
                    rect = new Rectangle(layer2[x, y].SX, layer2[x, y].SY, tileWidth, tileHeight);
                    break;
                case 3:
                    rect = new Rectangle(layer3[x, y].SX, layer3[x, y].SY, tileWidth, tileHeight);
                    break;
            }
            return rect;
        }
        /// <summary>
        /// Returns a tile at specified layer and position, returns null
        /// if no tile at location or outside domain
        /// </summary>
        /// <param name="layer">Tile layer value</param>
        /// <param name="x">X position in tiles</param>
        /// <param name="y">Y position in tiles</param>
        /// <returns>XenoTile reference</returns>
        public XenoTile getTile(int layer, int x, int y)
        {
            if (inDomain(x, y) == true)
            {
                switch (layer)
                {
                    case 1:
                        return layer1[x, y];
                    case 2:
                        return layer2[x, y];
                    case 3:
                        return layer3[x, y];
                }
            }
            return null;
        }
        /// <summary>
        /// Returns the outer perimeter of tile points
        /// </summary>
        /// <param name="x">X position in tiles</param>
        /// <param name="y">Y position in tiles</param>
        /// <param name="layer">Layer value</param>
        /// <returns>List of Point2D objects</returns>
        public List<Point2D> getOuterPerimeter(int x, int y, int layer = 1)
        {
            List<Point2D> open = new List<Point2D>();
            List<Point2D> perimeter = new List<Point2D>();
            XenoTile t1;
            XenoTile t2;
            if (x >= 0 && x < width)
            {
                if (y >= 0 && y < height)
                {
                    t1 = layer1[x, y];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            open.Add(new Point2D(x, y));
            Point2D p = null;
            while (open.Count > 0)
            {
                p = open[0];
                open.RemoveAt(0);
                switch (layer)
                {
                    case 1:
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY - 1 >= 0 && p.IY - 1 < height)
                            {
                                t2 = layer1[p.IX, p.IY - 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX, p.IY - 1));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY - 1));
                                }
                            }
                        }
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY + 1 >= 0 && p.IY + 1 < height)
                            {
                                t2 = layer1[p.IX, p.IY + 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX, p.IY + 1));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY + 1));
                                }
                            }
                        }
                        if (p.IX - 1 >= 0 && p.IX - 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer1[p.IX - 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX - 1, p.IY));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX - 1, p.IY));
                                }
                            }
                        }
                        if (p.IX + 1 >= 0 && p.IX + 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer1[p.IX + 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX + 1, p.IY));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX + 1, p.IY));
                                }
                            }
                        }
                        break;
                    case 2:
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY - 1 >= 0 && p.IY - 1 < height)
                            {
                                t2 = layer2[p.IX, p.IY - 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX, p.IY - 1));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY - 1));
                                }
                            }
                        }
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY + 1 >= 0 && p.IY + 1 < height)
                            {
                                t2 = layer2[p.IX, p.IY + 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX, p.IY + 1));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY + 1));
                                }
                            }
                        }
                        if (p.IX - 1 >= 0 && p.IX - 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer2[p.IX - 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX - 1, p.IY));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX - 1, p.IY));
                                }
                            }
                        }
                        if (p.IX + 1 >= 0 && p.IX + 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer2[p.IX + 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX + 1, p.IY));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX + 1, p.IY));
                                }
                            }
                        }
                        break;
                    case 3:
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY - 1 >= 0 && p.IY - 1 < height)
                            {
                                t2 = layer3[p.IX, p.IY - 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX, p.IY - 1));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY - 1));
                                }
                            }
                        }
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY + 1 >= 0 && p.IY + 1 < height)
                            {
                                t2 = layer3[p.IX, p.IY + 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX, p.IY + 1));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY + 1));
                                }
                            }
                        }
                        if (p.IX - 1 >= 0 && p.IX - 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer3[p.IX - 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX - 1, p.IY));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX - 1, p.IY));
                                }
                            }
                        }
                        if (p.IX + 1 >= 0 && p.IX + 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer3[p.IX + 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX + 1, p.IY));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX + 1, p.IY));
                                }
                            }
                        }
                        break;
                }
            }
            //remove duplicates before returning list
            for(int i = 0; i < perimeter.Count - 1; i++)
            {
                for (int k = i; k < perimeter.Count - 1; k++)
                {
                    if(perimeter[i].IX == perimeter[k].IX &&
                        perimeter[i].IY == perimeter[k].IY)
                    {
                        perimeter.RemoveAt(k);
                        break;
                    }
                }
            }
            return perimeter;
        }
        /// <summary>
        /// Returns the inner perimeter of tile points
        /// </summary>
        /// <param name="x">X position in tiles</param>
        /// <param name="y">Y position in tiles</param>
        /// <param name="layer">Layer value</param>
        /// <returns>List of Point2D objects</returns>
        public List<Point2D> getInnerPerimeter(int x, int y, int layer = 1)
        {
            List<Point2D> open = new List<Point2D>();
            List<Point2D> perimeter = new List<Point2D>();
            XenoTile t1;
            XenoTile t2;
            if (x >= 0 && x < width)
            {
                if (y >= 0 && y < height)
                {
                    t1 = layer1[x, y];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            open.Add(new Point2D(x, y));
            Point2D p = null;
            while (open.Count > 0)
            {
                p = open[0];
                open.RemoveAt(0);
                switch (layer)
                {
                    case 1:
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY - 1 >= 0 && p.IY - 1 < height)
                            {
                                t2 = layer1[p.IX, p.IY - 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX, p.IY - 1));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY));
                                }
                            }
                        }
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY + 1 >= 0 && p.IY + 1 < height)
                            {
                                t2 = layer1[p.IX, p.IY + 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX, p.IY + 1));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY));
                                }
                            }
                        }
                        if (p.IX - 1 >= 0 && p.IX - 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer1[p.IX - 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX - 1, p.IY));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY));
                                }
                            }
                        }
                        if (p.IX + 1 >= 0 && p.IX + 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer1[p.IX + 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX + 1, p.IY));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY));
                                }
                            }
                        }
                        break;
                    case 2:
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY - 1 >= 0 && p.IY - 1 < height)
                            {
                                t2 = layer2[p.IX, p.IY - 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX, p.IY - 1));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY));
                                }
                            }
                        }
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY + 1 >= 0 && p.IY + 1 < height)
                            {
                                t2 = layer2[p.IX, p.IY + 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX, p.IY + 1));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY));
                                }
                            }
                        }
                        if (p.IX - 1 >= 0 && p.IX - 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer2[p.IX - 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX - 1, p.IY));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY));
                                }
                            }
                        }
                        if (p.IX + 1 >= 0 && p.IX + 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer2[p.IX + 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX + 1, p.IY));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY));
                                }
                            }
                        }
                        break;
                    case 3:
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY - 1 >= 0 && p.IY - 1 < height)
                            {
                                t2 = layer3[p.IX, p.IY - 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX, p.IY - 1));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY));
                                }
                            }
                        }
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY + 1 >= 0 && p.IY + 1 < height)
                            {
                                t2 = layer3[p.IX, p.IY + 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX, p.IY + 1));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY));
                                }
                            }
                        }
                        if (p.IX - 1 >= 0 && p.IX - 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer3[p.IX - 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX - 1, p.IY));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY));
                                }
                            }
                        }
                        if (p.IX + 1 >= 0 && p.IX + 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer3[p.IX + 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    open.Add(new Point2D(p.IX + 1, p.IY));
                                }
                                else
                                {
                                    perimeter.Add(new Point2D(p.IX, p.IY));
                                }
                            }
                        }
                        break;
                }
            }
            //remove duplicates before returning list
            for (int i = 0; i < perimeter.Count - 1; i++)
            {
                for (int k = i; k < perimeter.Count - 1; k++)
                {
                    if (perimeter[i].IX == perimeter[k].IX &&
                        perimeter[i].IY == perimeter[k].IY)
                    {
                        perimeter.RemoveAt(k);
                        break;
                    }
                }
            }
            return perimeter;
        }
        /// <summary>
        /// Fills an area of tiles with a specified tile
        /// </summary>
        /// <param name="x">X position in tiles</param>
        /// <param name="y">Y position in tiles</param>
        /// <param name="sx">Source X position in tiles</param>
        /// <param name="sy">Source Y position in tiles</param>
        /// <param name="layer">Layer value</param>
        public void fillArea(int x, int y, int sx, int sy, int layer = 1)
        {
            List<Point2D> open = new List<Point2D>();
            XenoTile t1;
            XenoTile t2;
            if (x >= 0 && x < width)
            {
                if (y >= 0 && y < height)
                {
                    t1 = layer1[x, y];
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
            open.Add(new Point2D(x, y));
            Point2D p = null;
            while (open.Count > 0)
            {
                p = open[0];
                open.RemoveAt(0);
                switch (layer)
                {
                    case 1:
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY - 1 >= 0 && p.IY - 1 < height)
                            {
                                t2 = layer1[p.IX, p.IY - 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    layer1[p.IX, p.IY - 1] = new XenoTile(source, (p.IX) / tileWidth, (p.IY - 1) / tileHeight,
                                        tileWidth, tileHeight, sx * tileWidth, sy * tileHeight);
                                    open.Add(new Point2D(p.IX, p.IY - 1));
                                }
                            }
                        }
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY + 1 >= 0 && p.IY + 1 < height)
                            {
                                t2 = layer1[p.IX, p.IY + 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    layer1[p.IX, p.IY + 1] = new XenoTile(source, (p.IX) / tileWidth, (p.IY + 1) / tileHeight,
                                        tileWidth, tileHeight, sx * tileWidth, sy * tileHeight);
                                    open.Add(new Point2D(p.IX, p.IY + 1));
                                }
                            }
                        }
                        if (p.IX - 1 >= 0 && p.IX - 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer1[p.IX - 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    layer1[p.IX - 1, p.IY] = new XenoTile(source, (p.IX) / tileWidth, (p.IY) / tileHeight,
                                        tileWidth, tileHeight, sx * tileWidth, sy * tileHeight);
                                    open.Add(new Point2D(p.IX - 1, p.IY));
                                }
                            }
                        }
                        if (p.IX + 1 >= 0 && p.IX + 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer1[p.IX + 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    layer1[p.IX + 1, p.IY] = new XenoTile(source, (p.IX + 1) / tileWidth, (p.IY) / tileHeight,
                                        tileWidth, tileHeight, sx * tileWidth, sy * tileHeight);
                                    open.Add(new Point2D(p.IX + 1, p.IY));
                                }
                            }
                        }
                        break;
                    case 2:
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY - 1 >= 0 && p.IY - 1 < height)
                            {
                                t2 = layer2[p.IX, p.IY - 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    layer2[p.IX, p.IY - 1] = new XenoTile(source, (p.IX) / tileWidth, (p.IY - 1) / tileHeight,
                                        tileWidth, tileHeight, sx, sy);
                                    open.Add(new Point2D(p.IX, p.IY - 1));
                                }
                            }
                        }
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY + 1 >= 0 && p.IY + 1 < height)
                            {
                                t2 = layer2[p.IX, p.IY + 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    layer2[p.IX, p.IY + 1] = new XenoTile(source, (p.IX) / tileWidth, (p.IY + 1) / tileHeight,
                                        tileWidth, tileHeight, sx, sy);
                                    open.Add(new Point2D(p.IX, p.IY + 1));
                                }
                            }
                        }
                        if (p.IX - 1 >= 0 && p.IX - 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer2[p.IX - 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    layer2[p.IX - 1, p.IY] = new XenoTile(source, (p.IX - 1) / tileWidth, (p.IY) / tileHeight,
                                        tileWidth, tileHeight, sx, sy);
                                    open.Add(new Point2D(p.IX - 1, p.IY));
                                }
                            }
                        }
                        if (p.IX + 1 >= 0 && p.IX + 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer2[p.IX + 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    layer2[p.IX + 1, p.IY] = new XenoTile(source, (p.IX + 1) / tileWidth, (p.IY) / tileHeight,
                                        tileWidth, tileHeight, sx, sy);
                                    open.Add(new Point2D(p.IX + 1, p.IY));
                                }
                            }
                        }
                        break;
                    case 3:
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY - 1 >= 0 && p.IY - 1 < height)
                            {
                                t2 = layer3[p.IX, p.IY - 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    layer3[p.IX, p.IY - 1] = new XenoTile(source, (p.IX) / tileWidth, (p.IY - 1) / tileHeight,
                                        tileWidth, tileHeight, sx, sy);
                                    open.Add(new Point2D(p.IX, p.IY - 1));
                                }
                            }
                        }
                        if (p.IX >= 0 && p.IX < width)
                        {
                            if (p.IY + 1 >= 0 && p.IY + 1 < height)
                            {
                                t2 = layer3[p.IX, p.IY + 1];
                                if (compareTiles(t1, t2) == true)
                                {
                                    layer3[p.IX, p.IY + 1] = new XenoTile(source, (p.IX) / tileWidth, (p.IY + 1) / tileHeight,
                                        tileWidth, tileHeight, sx, sy);
                                    open.Add(new Point2D(p.IX, p.IY + 1));
                                }
                            }
                        }
                        if (p.IX - 1 >= 0 && p.IX - 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer3[p.IX - 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    layer3[p.IX - 1, p.IY] = new XenoTile(source, (p.IX + 1) / tileWidth, (p.IY) / tileHeight,
                                        tileWidth, tileHeight, sx, sy);
                                    open.Add(new Point2D(p.IX - 1, p.IY));
                                }
                            }
                        }
                        if (p.IX + 1 >= 0 && p.IX + 1 < width)
                        {
                            if (p.IY >= 0 && p.IY < height)
                            {
                                t2 = layer3[p.IX + 1, p.IY];
                                if (compareTiles(t1, t2) == true)
                                {
                                    layer3[p.IX + 1, p.IY] = new XenoTile(source, (p.IX + 1) / tileWidth, (p.IY) / tileHeight,
                                        tileWidth, tileHeight, sx, sy);
                                    open.Add(new Point2D(p.IX + 1, p.IY));
                                }
                            }
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Compares two tiles and returns true if they are same source values
        /// </summary>
        /// <param name="t1">XenoTile reference</param>
        /// <param name="t2">XenoTile reference</param>
        /// <returns>Boolean</returns>
        public bool compareTiles(XenoTile t1, XenoTile t2)
        {
            if (t1 == null && t2 == null)
            {
                return true;
            }
            else if (t1 != null && t2 != null)
            {
                if (t1.SX == t2.SX)
                {
                    if (t1.SY == t2.SY)
                    {
                        return true;
                    }
                }
            }
            return false;
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
        /// Source property
        /// </summary>
        public Texture2D Source
        {
            get { return source; }
        }
        /// <summary>
        /// Layer1 property
        /// </summary>
        public XenoTile[,] Layer1
        {
            get { return layer1; }
        }
        /// <summary>
        /// Layer2 property
        /// </summary>
        public XenoTile[,] Layer2
        {
            get { return layer2; }
        }
        /// <summary>
        /// Layer3 property
        /// </summary>
        public XenoTile[,] Layer3
        {
            get { return layer3; }
        }
    }
}
