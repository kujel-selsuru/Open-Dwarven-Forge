//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
using System.IO;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// A base class for games using a 2d tiled map system
    /// </summary>
    public class TiledRegion
    {
        //protected
        protected LayeredTileSys tiles;
        protected DataGrid<SimpleSprite> actors;
        protected int tilew;
        protected int tileh;
        protected int windx;
        protected int windy;
        protected int down;
        protected Point2D shift;

        //public
        /// <summary>
        /// TiledRegion constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="width">Width in tiles</param>
        /// <param name="height">Height in tiles</param>
        /// <param name="winWidth">Window width</param>
        /// <param name="winHeight">Window height</param>
        /// <param name="winx">Window x position</param>
        /// <param name="winy">Window y position</param>
        /// <param name="tilew">Tile width in pixels</param>
        /// <param name="tileh">Tile height in pixels</param>
        /// <param name="fill">Fill bottom layer flag value</param>
        /// <param name="down">Shift down value</param>
        public TiledRegion(Texture2D source, int width, int height, int winWidth, int winHeight, int winx, int winy, int tilew, int tileh, bool fill = false, int down = 0)
        {
            tiles = new LayeredTileSys(source, width, height, winWidth, winHeight, winx, winy, tilew, tileh, fill);
            tiles.MG.setAllTrue();
            actors = new DataGrid<SimpleSprite>(width, height);
            this.tilew = tilew;
            this.tileh = tileh;
            this.down = down;
            shift = new Point2D(0, 0);
        }
        /// <summary>
        /// TiledRegion from file constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="winWidth">Window width</param>
        /// <param name="winHeight">Window height</param>
        /// <param name="sr">StreamReader reference</param>
        /// <param name="down">Shift down value</param>
        public TiledRegion(Texture2D source, int winWidth, int winHeight, StreamReader sr, int down = 0)
        {
            sr.ReadLine();//discard testing data
            this.tilew = Convert.ToInt32(sr.ReadLine().ToString());
            this.tileh = Convert.ToInt32(sr.ReadLine().ToString());
            int tempw = Convert.ToInt32(sr.ReadLine().ToString());
            int temph = Convert.ToInt32(sr.ReadLine().ToString());
            int wx = Convert.ToInt32(sr.ReadLine().ToString());
            int wy = Convert.ToInt32(sr.ReadLine().ToString());
            tiles = new LayeredTileSys(source, tempw, temph, winWidth, winHeight, 0, 0, tilew, tileh, sr);
            tiles.Winx = wx; 
            tiles.Winy = wy; 
            windx = wx;
            windy = wy;
            allocateActors(tempw, temph);
            this.down = down;
            shift = new Point2D(0, 0);
        }
        /// <summary>
        /// Creates an empty data grid in the actor member variable
        /// </summary>
        /// <param name="tempw">DataGrid width</param>
        /// <param name="temph">DataGrid height</param>
        protected void allocateActors(int tempw, int temph)
        {
            actors = new DataGrid<SimpleSprite>(tempw, temph);
            for (int x = 0; x < tempw; x++)
            {
                for (int y = 0; y < temph; y++)
                {
                    actors.Grid[x, y] = null;
                }
            }
        }
        /// <summary>
        /// has unresolved bugs use render instead
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            tiles.draw(renderer, windx, windy);
            int startx = tiles.Winx / tiles.TileWidth;
            int starty = tiles.Winy / tiles.TileHeight;
            int endx = startx + tiles.WinWidth;
            if (endx > tiles.Width)
            {
                endx = tiles.Width;
            }
            int endy = starty + tiles.WinHeight;
            if (endy > tiles.Height)
            {
                endy = tiles.Height;
            }
            for (int x = startx; x < endx; x++)
            {
                for (int y = tiles.Winy; y < endy; y++)
                {
                    //SimpleSprite temp = actors.dataAt(x, y);
                    if (actors.Grid[x, y] != null)
                    {
                        actors.Grid[x, y].draw(renderer, tiles.Winx, tiles.Winy + down);
                    }
                }
            }
        }
        /// <summary>
        /// renders all tiles and actors in a given region
        /// </summary>
        /// <param name="sb">Renderer reference</param>
        /// <param name="scale">used to calculate window correctley</param>
        public void render(IntPtr renderer, int scale = 32)
        {
            tiles.render(renderer, scale, (int)(windx - shift.X), (int)(windy - shift.Y) + down);
            int startx = (windx / scale);
            int starty = (windy / scale);
            //int i = 0;
            //int k = 0;
            int endx = startx + tiles.WinWidth;
            if (endx > tiles.Width)
            {
                endx = tiles.Width;
            }
            int endy = starty + tiles.WinHeight;
            if (endy > tiles.Height)
            {
                endy = tiles.Height;
            }
            for (int x = startx; x < endx; x++)
            {
                for (int y = starty; y < endy; y++)
                {
                    if(actors.Grid[x, y] != null)
                    {
                        actors.Grid[x, y].draw(renderer, (int)(windx - shift.X), (int)(windy - shift.Y) + down);
                    }
                }
            }
        }
        /// <summary>
        /// Draws specified layer
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="layer">Layer to draw</param>
        /// <param name="scale">Scaler value</param>
        public void drawlayer(IntPtr renderer, int layer, int scale = 32)
        {
            if (layer < 4)
            {
                tiles.drawLayer(renderer, layer, scale, (int)shift.X, (int)shift.Y);
            }
            else
            {
                int startx = (windx / scale);
                int starty = (windy / scale);
                //int i = 0;
                //int k = 0;
                int endx = startx + tiles.WinWidth;
                if (endx > tiles.Width)
                {
                    endx = tiles.Width;
                }
                int endy = starty + tiles.WinHeight;
                if (endy > tiles.Height)
                {
                    endy = tiles.Height;
                }
                for (int x = startx; x < endx; x++)
                {
                    for (int y = starty; y < endy; y++)
                    {
                        if (actors.Grid[x, y] != null)
                        {
                            actors.Grid[x, y].draw(renderer, (int)(tiles.Winx + shift.X), (int)(tiles.Winy + shift.Y + down));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Draws windowed area of spcified layer
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="layer">Layer to draw</param>
        /// <param name="px">Position x value</param>
        /// <param name="py">Position y value</param>
        /// <param name="ww">Window width</param>
        /// <param name="wh">Window height</param>
        /// <param name="scale">scaler value</param>
        public void drawWindowlayer(IntPtr renderer, int layer, int px = 0, int py = 0, int ww = 12, int wh = 12, int scale = 32)
        {
            if (layer < 4)
            {
                tiles.drawWindowLayer(renderer, layer, ww, wh, scale, (int)(shift.X + px), (int)(shift.Y + py));
            }
            else
            {
                int startx = (windx / scale);
                int starty = (windy / scale);
                //int i = 0;
                //int k = 0;
                int endx = startx + ww;
                if (endx > tiles.Width)
                {
                    endx = tiles.Width;
                }
                int endy = starty + wh;
                if (endy > tiles.Height)
                {
                    endy = tiles.Height;
                }
                for (int x = startx; x < endx; x++)
                {
                    for (int y = starty; y < endy; y++)
                    {
                        if (actors.Grid[x, y] != null)
                        {
                            actors.Grid[x, y].draw(renderer, (int)(px + tiles.Winx + shift.X), (int)(py + tiles.Winy + shift.Y + down));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Draws a row of tiles
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="y">Row of tiles to draw</param>
        /// <param name="scale">Scaler value</param>
        /// <param name="shift">Shift down value</param>
        public void drawRow(IntPtr renderer, int y, int scale, int shift = 48)
        {
            int startx = (windx / scale);
            int endx = startx + tiles.WinWidth;
            if (endx > tiles.Width)
            {
                endx = tiles.Width;
            }
            int row = (y + shift) / scale;
            for (int x = startx; x < endx; x++)
            {
                if (actors.Grid[x, row] != null)
                {
                    actors.Grid[x, row].draw(renderer, tiles.Winx, tiles.Winy + down);
                }
            }
        }
        /// <summary>
        /// Returns a List of SDL_Rects for collision detection (8 SDL_Rects max)
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>List of SDL_Rects</returns>
        public List<SDL.SDL_Rect> getBoxes(int x, int y)
        {
            Point2D p = new Point2D(x/tilew, y/tileh);
            List<SDL.SDL_Rect> temp = new List<SDL.SDL_Rect>();
            if (actors.inDomain((int)p.X, (int)p.Y))
            {
                if (actors.Grid[(int)p.X, (int)p.Y] != null)
                {
                    temp.Add(actors.Grid[x, y].HitBox);
                }
            }
            if (actors.inDomain((int)p.X, (int)p.Y - 1))
            {
                if (actors.Grid[(int)p.X, (int)p.Y - 1] != null)
                {
                    temp.Add(actors.Grid[x, y - 1].HitBox);
                }
            }
            if (actors.inDomain((int)p.X + 1, (int)p.Y - 1))
            {
                if (actors.Grid[(int)p.X + 1, (int)p.Y - 1] != null)
                {
                    temp.Add(actors.Grid[x + 1, y - 1].HitBox);
                }
            }
            if (actors.inDomain((int)p.X + 1, (int)p.Y))
            {
                if (actors.Grid[(int)p.X + 1, (int)p.Y] != null)
                {
                    temp.Add(actors.Grid[x + 1, y].HitBox);
                }
            }
            if (actors.inDomain((int)p.X + 1, (int)p.Y + 1))
            {
                if (actors.Grid[(int)p.X + 1, (int)p.Y + 1] != null)
                {
                    temp.Add(actors.Grid[x + 1, y + 1].HitBox);
                }
            }
            if (actors.inDomain((int)p.X, (int)p.Y + 1))
            {
                if (actors.Grid[(int)p.X, (int)p.Y + 1] != null)
                {
                    temp.Add(actors.Grid[x, y + 1].HitBox);
                }
            }
            if (actors.inDomain((int)p.X - 1, (int)p.Y + 1))
            {
                if (actors.Grid[(int)p.X - 1, (int)p.Y + 1] != null)
                {
                    temp.Add(actors.Grid[x - 1, y + 1].HitBox);
                }
            }
            if (actors.inDomain((int)p.X - 1, (int)p.Y))
            {
                if (actors.Grid[(int)p.X - 1, (int)p.Y] != null)
                {
                    temp.Add(actors.Grid[x - 1, y].HitBox);
                }
            }
            if (actors.inDomain((int)p.X - 1, (int)p.Y - 1))
            {
                if (actors.Grid[(int)p.X - 1, (int)p.Y - 1] != null)
                {
                    temp.Add(actors.Grid[x - 1, y - 1].HitBox);
                }
            }
            return temp;
        }
        /// <summary>
        /// Fills specified layer with a tile choice
        /// </summary>
        /// <param name="layer">Layer to fill</param>
        /// <param name="nsx">Tile SX value</param>
        /// <param name="nsy">Tile SY value</param>
        /// <param name="passible">PAssiblity flag</param>
        public void fillLayer(int layer, int nsx, int nsy, bool passible = true)
        {
            tiles.fillLayer(layer, nsx, nsy, passible);
        }
        /// <summary>
        /// Erase a specified tile
        /// </summary>
        /// <param name="layer">Layer of tile</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public void eraseTile(int layer, int x, int y)
        {
            tiles.eraseTile(layer, x, y);
        }
        /// <summary>
        /// Tiles property
        /// </summary>
        public LayeredTileSys Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }
        /// <summary>
        /// Actors property
        /// </summary>
        public DataGrid<SimpleSprite> Actors
        {
            get { return actors; }
        }
        /// <summary>
        /// Add SimpleSprite object to actors layer
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="actor">SimpleSprite object to add</param>
        public void addActor(int x, int y, SimpleSprite actor)
        {
            if(actors.inDomain(x, y))
            {
                actors.Grid[x,y] = actor;
                tiles.MG.setCell(x, y, false);
            }
        }
        /// <summary>
        /// Save data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveData(StreamWriter sw)
        {
            sw.WriteLine("####### map setup #######");
            sw.WriteLine(this.tilew.ToString());
            sw.WriteLine(this.tileh.ToString());
            sw.WriteLine(this.tiles.Width.ToString());
            sw.WriteLine(this.tiles.Height.ToString());
            sw.WriteLine(Convert.ToInt32(tiles.Winx));
            sw.WriteLine(Convert.ToInt32(tiles.Winy));
            //layer 1
            sw.WriteLine("####### layer1 #######");
            for (int x = 0; x < tiles.Width; x++)
            {
                for (int y = 0; y < tiles.Height; y++)
                {
                    Point2D temp = tiles.tileAt(1, x, y, tilew, tileh);
                    if (temp != null)
                    {
                        sw.WriteLine(temp.X);
                        sw.WriteLine(temp.Y);
                    }
                    else
                    {
                        sw.WriteLine("null");
                    }
                }
            }
            //layer 2
            sw.WriteLine("####### layer2 #######");
            for (int x = 0; x < tiles.Width; x++)
            {
                for (int y = 0; y < tiles.Height; y++)
                {
                    Point2D temp = tiles.tileAt(2, x, y, tilew, tileh);
                    if (temp != null)
                    {
                        sw.WriteLine(temp.X);
                        sw.WriteLine(temp.Y);
                    }
                    else
                    {
                        sw.WriteLine("null");
                    }
                }
            }
            //layer 3
            sw.WriteLine("####### layer3 #######");
            for (int x = 0; x < tiles.Width; x++)
            {
                for (int y = 0; y < tiles.Height; y++)
                {
                    Point2D temp = tiles.tileAt(3, x, y, tilew, tileh);
                    if (temp != null)
                    {
                        sw.WriteLine(temp.X);
                        sw.WriteLine(temp.Y);
                    }
                    else
                    {
                        sw.WriteLine("null");
                    }
                }
            }
            //map graph
            sw.WriteLine("map graph");
            tiles.MG.save(sw);
            //actors
            /*
            for (int x = 0; x < tiles.Width; x++)
            {
                for (int y = 0; y < tiles.Height; y++)
                {
                    if (actors.Grid[x, y] != null)
                    {
                        sw.WriteLine("barrel");
                    }
                    else
                    {
                        sw.WriteLine("null");
                    }
                }
            }
            */
        }
        /// <summary>
        /// Populates a list of SDL_Rects with potential collisions
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="rects">List to  populate</param>
        public void findCollsions(int x, int y, List<SDL.SDL_Rect> rects)
        {
            rects.Clear();
            Point2D temp = new Point2D(x / 32, y / 32);
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
            temp.Y -= 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
            temp.X += 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
            temp.Y += 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
            temp.Y += 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
            temp.X -= 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
            temp.X -= 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
            temp.Y -= 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
            temp.Y -= 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
        }
        /// <summary>
        /// Populates a list of SDL_Rects with potential collisions
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="rects">List to  populate</param>
        public void findCollsions(int x, int y, ref List<SDL.SDL_Rect> rects)
        {
            //rects.Clear();
            Point2D temp = new Point2D(x / 32, y / 32);
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
            temp.Y -= 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
            temp.X += 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
            temp.Y += 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
            temp.Y += 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
            temp.X -= 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
            temp.X -= 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
            temp.Y -= 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
            temp.Y -= 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                rects.Add(actors.Grid[(int)temp.X, (int)temp.Y].HitBox);
            }
        }
        /// <summary>
        /// Populates a list of SimpleSprites with potential collisions
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="rects">List to  populate</param>
        public void findCollsions(int x, int y, int scale, List<SimpleSprite> sprites)
        {
            sprites.Clear();
            Point2D temp = new Point2D(x / scale, y / scale);
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                sprites.Add(actors.Grid[(int)temp.X, (int)temp.Y]);
            }
            temp.Y -= 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                sprites.Add(actors.Grid[(int)temp.X, (int)temp.Y]);
            }
            temp.X += 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                sprites.Add(actors.Grid[(int)temp.X, (int)temp.Y]);
            }
            temp.Y += 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                sprites.Add(actors.Grid[(int)temp.X, (int)temp.Y]);
            }
            temp.Y += 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                sprites.Add(actors.Grid[(int)temp.X, (int)temp.Y]);
            }
            temp.X -= 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                sprites.Add(actors.Grid[(int)temp.X, (int)temp.Y]);
            }
            temp.X -= 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                sprites.Add(actors.Grid[(int)temp.X, (int)temp.Y]);
            }
            temp.Y -= 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                sprites.Add(actors.Grid[(int)temp.X, (int)temp.Y]);
            }
            temp.Y -= 1;
            if (testForRect((int)temp.X, (int)temp.Y))
            {
                sprites.Add(actors.Grid[(int)temp.X, (int)temp.Y]);
            }
        }
        /// <summary>
        /// Returns a SimpleSprite object at specified loaction
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>SimpleSprite</returns>
        public SimpleSprite findActor(int x, int y)
        {
            Point2D temp = new Point2D(x / tilew, y / tileh);
            if(actors.Grid[(int)temp.X, (int)temp.Y] != null)
            {
                return actors.Grid[(int)temp.X, (int)temp.Y];
            }
            temp.Y -= 1;//up
            if (actors.Grid[(int)temp.X, (int)temp.Y] != null)
            {
                return actors.Grid[(int)temp.X, (int)temp.Y];
            }
            temp.X += 1;//up right
            if (actors.Grid[(int)temp.X, (int)temp.Y] != null)
            {
                return actors.Grid[(int)temp.X, (int)temp.Y];
            }
            temp.Y += 1;//right
            if (actors.Grid[(int)temp.X, (int)temp.Y] != null)
            {
                return actors.Grid[(int)temp.X, (int)temp.Y];
            }
            temp.Y += 1;//right down
            if (actors.Grid[(int)temp.X, (int)temp.Y] != null)
            {
                return actors.Grid[(int)temp.X, (int)temp.Y];
            }
            temp.X -= 1;//down
            if (actors.Grid[(int)temp.X, (int)temp.Y] != null)
            {
                return actors.Grid[(int)temp.X, (int)temp.Y];
            }
            temp.X -= 1;//left down
            if (actors.Grid[(int)temp.X, (int)temp.Y] != null)
            {
                return actors.Grid[(int)temp.X, (int)temp.Y];
            }
            temp.Y -= 1;//left
            if (actors.Grid[(int)temp.X, (int)temp.Y] != null)
            {
                return actors.Grid[(int)temp.X, (int)temp.Y];
            }
            temp.Y -= 1;//left up
            if (actors.Grid[(int)temp.X, (int)temp.Y] != null)
            {
                return actors.Grid[(int)temp.X, (int)temp.Y];
            }
            return null;
        }
        /// <summary>
        /// Checks if a position on the actors layer has an object in it
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Boolean</returns>
        protected bool testForRect(int x, int y)
        {
            if (tiles.inDomain(x, y))
            {
                if (actors.Grid[x, y] != null)
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
        /// Formats window position to keep it inside TiledRegion
        /// </summary>
        public void formatWinPos()
        {
            if (windx < 0)
            {
                windx = 0;
                Winx = 0;
            }
            else if (windx + (tiles.TileWidth * tiles.WinWidth) > actors.Width * (tiles.TileWidth))
            {
                windx = (actors.Width * tiles.TileWidth) - (tiles.TileWidth * tiles.WinWidth);
                Winx = (actors.Width * tiles.TileWidth) - (tiles.TileWidth * tiles.WinWidth);
            }
            if (windy < 0)
            {
                windy = 0;
                Winy = 0;
            }
            else if (windy + (tiles.TileHeight * tiles.WinHeight) > actors.Height * (tiles.TileHeight))
            {
                windy = (actors.Height * tiles.TileHeight) - (tiles.TileHeight * tiles.WinHeight);
                Winy = (actors.Height * tiles.TileHeight) - (tiles.TileHeight * tiles.WinHeight);
            }
        }
        /// <summary>
        /// Shifts window
        /// </summary>
        /// <param name="shiftx">Shift x value</param>
        /// <param name="shifty">Shiift y value</param>
        public void shiftWindow(int shiftx, int shifty)
        {
            
            if (windx + shiftx >= 0 & windx + (tiles.TileWidth * tiles.WinWidth) + shiftx <= actors.Width * (tiles.TileWidth)) 
            {
                windx += shiftx;
                tiles.Winx += shiftx;
            }
            if (windy + shifty >= 0 & windy + (tiles.TileHeight * tiles.WinHeight) + shifty <= actors.Height * (tiles.TileHeight))
            {
                windy += shifty;
                tiles.Winy += shifty;
            }
        }
        /// <summary>
        /// Shifts window
        /// </summary>
        /// <param name="shiftx">Shift x value</param>
        /// <param name="shifty">Shift y value</param>
        public void shiftWindow2(int shiftx, int shifty)
        {

            if (windx + shiftx >= 0 & windx + (tiles.TileWidth * tiles.WinWidth) + shiftx <= (actors.Width + 12) * (tiles.TileWidth))
            {
                windx += shiftx;
                tiles.Winx += shiftx;
            }
            if (windy + shifty >= 0 & windy + (tiles.TileHeight * tiles.WinHeight) + shifty <= (actors.Height + 18) * (tiles.TileHeight))
            {
                windy += shifty;
                tiles.Winy += shifty;
            }
        }
        /// <summary>
        /// Formats Tiles and Actors 
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="x">New width value</param>
        /// <param name="y">New height value</param>
        public void formatTileBoard(Texture2D source, int x, int y)
        {
            tiles.formatTileBoard(source, x, y);
            actors = new DataGrid<SimpleSprite>(x, y);
        }
        /// <summary>
        /// unresolved bugs don't use
        /// </summary>
        /// <param name="x">x point in tiles</param>
        /// <param name="y">y point in tiles</param>
        public void setWindow(int x, int y)
        {
            windx = x;
            Winx = x;
            windy = y;
            Winy = y;
            formatWinPos();  
        }
        /// <summary>
        /// Windx returns the window position in pixels not tiles
        /// </summary>
        public int Windx
        {
            get { return windx; }
            set { windx = value; }
        }
        /// <summary>
        /// Windy returns the window position in pixels not tiles
        /// </summary>
        public int Windy
        {
            get { return windy; }
            set { windy = value; }
        }
        /// <summary>
        /// TileWidth property
        /// </summary>
        public int TileWidth
        {
            get { return tiles.TileWidth; }
        }
        /// <summary>
        /// TileHeight property
        /// </summary>
        public int TileHeight
        {
            get { return tiles.TileHeight; }
        }
        /// <summary>
        /// Width property
        /// </summary>
        public int Width
        {
            get { return tiles.Width; }
        }
        /// <summary>
        /// Height property
        /// </summary>
        public int Height
        {
            get { return tiles.Height; }
        }
        /// <summary>
        /// WinWidth property
        /// </summary>
        public int WinWidth
        {
            get { return tiles.WinWidth; }
            set { tiles.WinWidth = value; }
        }
        /// <summary>
        /// WinHeight property
        /// </summary>
        public int WinHeight
        {
            get { return tiles.WinHeight; }
            set { tiles.WinHeight = value; }
        }
        /// <summary>
        /// Shift property
        /// </summary>
        public Point2D Shift
        {
            get { return shift; }
            set { shift = value; }
        }
        /// <summary>
        /// Winx property
        /// </summary>
        public int Winx
        {
            get { return tiles.Winx; }
            set { tiles.Winx = value; }
        }
        /// <summary>
        /// Winy property
        /// </summary>
        public int Winy
        {
            get { return tiles.Winy; }
            set { tiles.Winy = value; }
        }
        /// <summary>
        /// WidthInPixels property
        /// </summary>
        public int WidthInPixels
        {
            get { return tiles.Width * tilew; }
        }
        /// <summary>
        /// HeightInPixels property
        /// </summary>
        public int HeightInPixels
        {
            get { return tiles.Height * tileh; }
        }
    }
}
