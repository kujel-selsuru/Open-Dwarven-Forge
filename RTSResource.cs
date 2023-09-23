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
    public enum RESOURCETYPE { rt_static = 0, rt_dynamic, rt_mined };
    public class RTSResource
    {
        //protected
        protected Texture2D source;
        protected string sourceName;
        protected string name;
        protected Rectangle srcRect;
        protected Rectangle destRect;
        protected int worth;
        protected int maxWorth;
        protected RESOURCETYPE rt;
        protected int growthRate;
        protected int growthTicks;
        protected Point2D position;
        protected bool impassable;
        //public
        /// <summary>
        /// RTS resource constructor
        /// </summary>
        /// <param name="source">source texture</param>
        /// <param name="sourceName">source textxre name</param>
        /// <param name="name">resource name</param>
        /// <param name="width">x position</param>
        /// <param name="width">y position</param>
        /// <param name="width">image width</param>
        /// <param name="height">image height</param>
        /// <param name="maxWorth">max value of resource</param>
        /// <param name="rt">resource type</param>
        /// <param name="rt">growth rate of resource</param>
        /// <param name="rt">impassability</param>
        public RTSResource(Texture2D source, string sourceName, string name, float x, float y, float width,
            float height, int maxWorth, RESOURCETYPE rt, int growthRate, bool impassable)
        {
            this.source = source;
            this.sourceName = sourceName;
            this.name = name;
            destRect.X = x;
            destRect.Y = y;
            destRect.Width = width;
            destRect.Height = height;
            srcRect.X = 0;
            srcRect.Y = 0;
            srcRect.Width = width;
            srcRect.Height = height;
            this.maxWorth = maxWorth;
            worth = maxWorth;
            this.rt = rt;
            this.growthRate = growthRate;
            growthTicks = 0;
            position = new Point2D(x, y);
            this.impassable = impassable;
        }
        /// <summary>
        /// Constructor from stream reader
        /// </summary>
        /// <param name="sr">Stream reader reference</param>
        /// <param name="gb">Graphics bank referecne</param>
        public RTSResource(System.IO.StreamReader sr)
        {
            sr.ReadLine();
            sourceName = sr.ReadLine();
            source = TextureBank.getTexture(sourceName);
            name = sr.ReadLine();
            destRect.X = Convert.ToInt32(sr.ReadLine());
            destRect.Y = Convert.ToInt32(sr.ReadLine());
            destRect.Width = Convert.ToInt32(sr.ReadLine());
            destRect.Height = Convert.ToInt32(sr.ReadLine());
            srcRect.X = 0;
            srcRect.Y = 0;
            srcRect.Width = destRect.Width;
            srcRect.Height = destRect.Height;
            worth = Convert.ToInt32(sr.ReadLine());
            maxWorth = Convert.ToInt32(sr.ReadLine());
            rt = (RESOURCETYPE)(Convert.ToInt32(sr.ReadLine()));
            growthRate = Convert.ToInt32(sr.ReadLine());
            growthTicks = Convert.ToInt32(sr.ReadLine());
            position = new Point2D(destRect.X, destRect.Y);
            impassable = Convert.ToBoolean(sr.ReadLine());
        }
        /// <summary>
        /// RTSResource copy constructor
        /// </summary>
        /// <param name="obj">RTSResource instance to copy</param>
        public RTSResource(RTSResource obj)
        {
            source = obj.Source;
            sourceName = obj.SourceName;
            name = obj.Name;
            destRect.X = obj.X;
            destRect.Y = obj.Y;
            destRect.Width = obj.Width;
            destRect.Height = obj.Height;
            srcRect.X = 0;
            srcRect.Y = 0;
            srcRect.Width = obj.Width;
            srcRect.Height = obj.Height;
            maxWorth = obj.MaxWorth;
            worth = obj.Worth;
            rt = obj.RT;
            growthRate = obj.GrowthRate;
            growthTicks = obj.GrowthTicks;
            this.impassable = obj.Impassable;
        }
        /// <summary>
        /// Source Property
        /// </summary>
        public Texture2D Source
        {
            get { return source; }
            set { source = value; }
        }
        /// <summary>
        /// Source name property
        /// </summary>
        public string SourceName
        {
            get { return sourceName; }
            set { sourceName = value; }
        }
        /// <summary>
        /// Name property
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// Width Property
        /// </summary>
        public float Width
        {
            get { return destRect.Width; }
            set
            {
                destRect.Width = value;
                srcRect.Width = value;
            }
        }
        /// <summary>
        /// Height Property
        /// </summary>
        public float Height
        {
            get { return destRect.Height; }
            set
            {
                destRect.Height = value;
                srcRect.Height = value;
            }
        }
        /// <summary>
        /// X Property
        /// </summary>
        public float X
        {
            get { return destRect.X; }
            set { destRect.X = value; }
        }
        /// <summary>
        /// Y Property
        /// </summary>
        public float Y
        {
            get { return destRect.Y; }
            set { destRect.Y = value; }
        }
        /// <summary>
        /// Worth Property
        /// </summary>
        public int Worth
        {
            get { return worth; }
            set { worth = value; }
        }
        /// <summary>
        /// Max worth Property
        /// </summary>
        public int MaxWorth
        {
            get { return maxWorth; }
            set { maxWorth = value; }
        }
        /// <summary>
        /// Resource type Property
        /// </summary>
        public RESOURCETYPE RT
        {
            get { return rt; }
            set { rt = value; }
        }
        /// <summary>
        /// Growth rate property
        /// </summary>
        public int GrowthRate
        {
            get { return growthRate; }
            set { growthRate = value; }
        }
        /// <summary>
        /// growth ticks property
        /// </summary>
        public int GrowthTicks
        {
            get { return growthTicks; }
            set { growthTicks = value; }
        }
        /// <summary>
        /// harvests upto the scooped amount 
        /// </summary>
        /// <param name="scoop">how much of the resource to remove from object</param>
        /// <returns>returns how much of the resource actually harvested</returns>
        public int harvest(int scoop)
        {
            int temp = 0;
            if (worth >= scoop)
            {
                temp = scoop;
                worth -= scoop;
            }
            else
            {
                temp = worth;
                worth = 0;
            }

            return temp;
        }
        /// <summary>
        /// grows the internal value of a RTS resource object
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public bool grow(int rate)
        {
            growthTicks += rate;
            if (growthTicks >= growthRate)
            {
                growthTicks = 0;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Position property
        /// </summary>
        public Point2D Position
        {
            get { return position; }
            set
            {
                position = value;
                destRect.X = (int)position.X;
                destRect.Y = (int)position.Y;
            }
        }
        /// <summary>
        /// impassable property
        /// </summary>
        public bool Impassable
        {
            get { return impassable; }
            set { impassable = value; }
        }
        /// <summary>
        /// draws the resource
        /// </summary>
        /// <param name="renderer">renderer reference</param>
        /// <param name="winx">window x offset</param>
        /// <param name="winy">window y offset</param>
        public void draw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            //sb.Draw(source, destRect, srcRect, Color.White);
            SimpleDraw.draw(renderer, source, srcRect, destRect);
        }
        /// <summary>
        /// RTS resource save data
        /// </summary>
        /// <param name="sw">stream writer reference</param>
        public virtual void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======RTSResource Data======");
            sw.WriteLine(sourceName);
            sw.WriteLine(name);
            sw.WriteLine(destRect.X);
            sw.WriteLine(destRect.Y);
            sw.WriteLine(destRect.Width);
            sw.WriteLine(destRect.Height);
            sw.WriteLine(maxWorth);
            sw.WriteLine(worth);
            sw.WriteLine((int)rt);
            sw.WriteLine(growthRate);
            sw.WriteLine(growthTicks);
            sw.WriteLine(impassable);
        }
    }
    public class RTSResourceGrid
    {
        //protected
        protected RTSResource[,] grid;
        protected int width;
        protected int height;
        protected int tileWidth;
        protected int tileHeight;
        protected Random rand;

        //public
        /// <summary>
        /// RTS resource grid constructor
        /// </summary>
        /// <param name="width">width of grid</param>
        /// <param name="height">height of grid</param>
        /// <param name="tile width">widh of tiles</param>
        /// <param name="tile height">height of tiles</param>
        public RTSResourceGrid(int width, int height, int tileWidth, int tileHeight)
        {
            grid = new RTSResource[width, height];
            this.width = width;
            this.height = height;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            rand = new Random((int)System.DateTime.Today.Ticks);
            for (int column = 0; column < width; column++)
            {
                for (int row = 0; row < height; row++)
                {
                    grid[column, row] = null;
                }
            }
        }
        /// <summary>
        /// RTS Resource constructor from file
        /// </summary>
        /// <param name="sr">Stream reader reference</param>
        /// <param name="gb">Graphics bank</param>
        public RTSResourceGrid(System.IO.StreamReader sr)
        {
            sr.ReadLine();
            width = Convert.ToInt32(sr.ReadLine());
            height = Convert.ToInt32(sr.ReadLine());
            grid = new RTSResource[width, height];
            tileWidth = Convert.ToInt32(sr.ReadLine());
            tileHeight = Convert.ToInt32(sr.ReadLine());
            rand = new Random((int)System.DateTime.Today.Ticks);
            for (int column = 0; column < width; column++)
            {
                for (int row = 0; row < height; row++)
                {
                    if (sr.ReadLine() == "data")
                    {
                        grid[column, row] = new RTSResource(sr);
                    }
                    else
                    {
                        grid[column, row] = null;
                    }
                }
            }
        }
        /// <summary>
        /// RTS resource grid copy constructor 
        /// </summary>
        /// <param name="obj"></param>
        public RTSResourceGrid(RTSResourceGrid obj)
        {
            grid = new RTSResource[obj.Width, obj.Height];
            this.width = obj.Width;
            this.height = obj.Height;
            this.tileWidth = obj.TileWidth;
            this.tileHeight = obj.TileHeight;
            rand = new Random((int)System.DateTime.Today.Ticks);
            for (int column = 0; column < width; column++)
            {
                for (int row = 0; row < height; row++)
                {
                    grid[column, row] = obj.atPosition(column, row);
                }
            }
        }
        /// <summary>
        /// tests if a provided position is within the grid
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <returns></returns>
        public bool inDomain(int x, int y)
        {
            if (x >= 0 && x < width)
            {
                if (y >= 0 && y < height)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// returns the RTS Resoruce if the position is not empty
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <returns></returns>
        public RTSResource atPosition(int x, int y)
        {
            if (inDomain(x, y))
            {
                return grid[x, y];
            }
            return null;
        }
        /// <summary>
        /// attempts to drow dynamic resources in grid
        /// </summary>
        /// <param name="world">world scene</param>
        /// <param name="rate">rate at which resource objects increase internal value</param>
        public void updateGrid(WorldScene world, int rate)
        {
            RTSResource temp = null;
            //search for dynamic resource objects
            for (int column = 0; column < width; column++)
            {
                for (int row = 0; row < height; row++)
                {
                    if (grid[column, row].Impassable)
                    {
                        world.getTile(column, row - 1).Occupied = true;
                    }
                    if (grid[column, row].RT == RESOURCETYPE.rt_dynamic)
                    {
                        //if a dynamic resource grow it and if it reaches its growth rate 
                        //randomly tries to expand into an empty space
                        if (grid[column, row].grow(rate))
                        {
                            temp = grid[column, row];
                            int val = rand.Next(1, 800);
                            if (val < 100)
                            {
                                //up
                                if (world.inDomain((column * world.TileWidth), (row * world.TileHeight) - world.TileHeight))
                                {
                                    if (!world.getTile(column, row - 1).Occupied && grid[column, row - 1] == null)//tile is clear so safe to grow into
                                    {
                                        grid[column, row - 1] = new RTSResource(temp.Source,
                                            temp.SourceName, temp.SourceName,
                                            (column * world.TileWidth),
                                            (row * world.TileHeight) - world.TileHeight,
                                            temp.Width, temp.Height, temp.MaxWorth, temp.RT,
                                            temp.GrowthRate, temp.Impassable);
                                    }
                                }
                            }
                            else if (val >= 100 && val < 200)
                            {
                                //up right
                                if (world.inDomain((column * world.TileWidth) + world.TileWidth, (row * world.TileHeight) - world.TileHeight))
                                {
                                    if (!world.getTile(column + 1, row - 1).Occupied && grid[column + 1, row - 1] == null)//tile is clear so safe to grow into
                                    {
                                        grid[column + 1, row - 1] = new RTSResource(temp.Source,
                                            temp.SourceName, temp.SourceName, (column * world.TileWidth) + world.TileWidth,
                                            (row * world.TileHeight) - world.TileHeight,
                                            temp.Width, temp.Height, temp.MaxWorth, temp.RT,
                                            temp.GrowthRate, temp.Impassable);
                                    }
                                }
                            }
                            else if (val >= 200 && val < 300)
                            {
                                //right
                                if (world.inDomain((column * world.TileWidth) + world.TileWidth, (row * world.TileHeight)))
                                {
                                    if (!world.getTile(column + 1, row).Occupied && grid[column + 1, row] == null)//tile is clear so safe to grow into
                                    {
                                        grid[column + 1, row] = new RTSResource(temp.Source,
                                            temp.SourceName, temp.SourceName,
                                            (column * world.TileWidth) + world.TileWidth,
                                            (row * world.TileHeight),
                                            temp.Width, temp.Height, temp.MaxWorth, temp.RT,
                                            temp.GrowthRate, temp.Impassable);
                                    }
                                }
                            }
                            else if (val >= 300 && val < 400)
                            {
                                //down right
                                if (world.inDomain((column * world.TileWidth) + world.TileWidth, (row * world.TileHeight) + world.TileHeight))
                                {
                                    if (!world.getTile(column + 1, row + 1).Occupied && grid[column + 1, row + 1] == null)//tile is clear so safe to grow into
                                    {
                                        grid[column + 1, row + 1] = new RTSResource(temp.Source,
                                            temp.SourceName, temp.SourceName, (column * world.TileWidth) + world.TileWidth,
                                            (row * world.TileHeight) + world.TileHeight,
                                            temp.Width, temp.Height, temp.MaxWorth, temp.RT,
                                            temp.GrowthRate, temp.Impassable);
                                    }
                                }
                            }
                            else if (val >= 400 && val < 500)
                            {
                                //down
                                if (world.inDomain((column * world.TileWidth), (row * world.TileHeight) + world.TileHeight))
                                {
                                    if (!world.getTile(column, row + 1).Occupied && grid[column, row + 1] == null)//tile is clear so safe to grow into
                                    {
                                        grid[column, row + 1] = new RTSResource(temp.Source,
                                            temp.SourceName, temp.SourceName,
                                            (column * world.TileWidth),
                                            (row * world.TileHeight) + world.TileHeight,
                                            temp.Width, temp.Height, temp.MaxWorth, temp.RT,
                                            temp.GrowthRate, temp.Impassable);
                                    }
                                }
                            }
                            else if (val >= 500 && val < 600)
                            {
                                //down left
                                if (world.inDomain((column * world.TileWidth) - world.TileWidth, (row * world.TileHeight) + world.TileHeight))
                                {
                                    if (!world.getTile(column - 1, row + 1).Occupied && grid[column - 1, row + 1] == null)//tile is clear so safe to grow into
                                    {
                                        grid[column - 1, row + 1] = new RTSResource(temp.Source,
                                            temp.SourceName, temp.SourceName,
                                            (column * world.TileWidth) - world.TileWidth,
                                            (row * world.TileHeight) + world.TileHeight,
                                            temp.Width, temp.Height, temp.MaxWorth, temp.RT,
                                            temp.GrowthRate, temp.Impassable);
                                    }
                                }
                            }
                            else if (val >= 600 && val < 700)
                            {
                                //left
                                if (world.inDomain((column * world.TileWidth) - world.TileWidth, (row * world.TileHeight)))
                                {
                                    if (!world.getTile(column - 1, row).Occupied && grid[column - 1, row] == null)//tile is clear so safe to grow into
                                    {
                                        grid[column - 1, row] = new RTSResource(temp.Source,
                                            temp.SourceName, temp.SourceName,
                                            (column * world.TileWidth) - world.TileWidth,
                                            (row * world.TileHeight),
                                            temp.Width, temp.Height, temp.MaxWorth, temp.RT,
                                            temp.GrowthRate, temp.Impassable);
                                    }
                                }
                            }
                            else if (val >= 700)
                            {
                                //up left
                                if (world.inDomain((column * world.TileWidth) - world.TileWidth, (row * world.TileHeight) - world.TileHeight))
                                {
                                    if (!world.getTile(column - 1, row - 1).Occupied && grid[column - 1, row - 1] == null)//tile is clear so safe to grow into
                                    {
                                        grid[column - 1, row - 1] = new RTSResource(temp.Source,
                                            temp.SourceName, temp.SourceName,
                                            (column * world.TileWidth) - world.TileWidth,
                                            (row * world.TileHeight) - world.TileHeight,
                                            temp.Width, temp.Height, temp.MaxWorth, temp.RT,
                                            temp.GrowthRate, temp.Impassable);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// draws the resource grid space currently in the game window 
        /// </summary>
        /// <param name="renderer">renderer reference</param>
        /// <param name="winx">window x position</param>
        /// <param name="winy">window y position</param>
        /// <param name="w">window width in tiles</param>
        /// <param name="h">window hieght in tiles</param>
        public void draw(IntPtr renderer, int winx, int winy, int w, int h)
        {
            //add 2 spaces in both dimension for dead zone on game window
            for (int column = (winx / tileWidth) - 1; column < w + 1; column++)
            {
                for (int row = (winy / tileHeight) - 1; row < h + 1; row++)
                {
                    if (inDomain(column, row))
                    {
                        grid[column, row].draw(renderer, (column * tileWidth) - winx, (row * tileHeight) - winy);
                    }
                }
            }
        }
        /// <summary>
        /// Adds a RTS resource object to the specified location if it is empty
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="rsc">RTS resource object to place</param>
        public void addResource(int x, int y, RTSResource rsc)
        {
            if (inDomain(x / tileWidth, y / tileHeight))
            {
                if (grid[x / tileWidth, y / tileHeight] == null)
                {
                    grid[x / tileWidth, y / tileHeight] = rsc;
                }
            }
        }
        /// <summary>
        /// Erases a resource at a specified position
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y poistion</param>
        public void eraseResource(int x, int y)
        {
            if (inDomain(x / tileWidth, y / tileHeight))
            {
                grid[x / tileWidth, y / tileHeight] = null;
            }
        }
        /// <summary>
        /// Save world scene data
        /// </summary>
        /// <param name="sw">Stream writer reference</param>
        public virtual void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======RTSResourceGrid Data======");
            sw.WriteLine(width);
            sw.WriteLine(height);
            sw.WriteLine(tileWidth);
            sw.WriteLine(tileHeight);
            for (int column = 0; column < width; column++)
            {
                for (int row = 0; row < height; row++)
                {
                    if (grid[column, row] != null)
                    {
                        sw.WriteLine("data");
                        grid[column, row].saveData(sw);
                    }
                    else
                    {
                        sw.WriteLine("null");
                    }
                }
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
        /// Hegiht property
        /// </summary>
        public int Height
        {
            get { return height; }
        }
        /// <summary>
        /// Tile width property
        /// </summary>
        public int TileWidth
        {
            get { return TileWidth; }
        }
        /// <summary>
        /// Tile hegiht property
        /// </summary>
        public int TileHeight
        {
            get { return TileHeight; }
        }
    }
}
