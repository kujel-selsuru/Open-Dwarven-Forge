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
//**************************************************************************************************
    //List of all assumed graphics names and pixel dimensions
    //bp 96 x 32
    //bdp 96 x 32
    //basic button 96 x 32
    //basic 96 x 32
    //ubp 96 x 32
    //ubd 96 x 32
    //dbp 32 x 32
    //dbd 32 x 32
    //bbp 96 x 32
    //bbd 96 x 32
    //pixel 1 x 1
    //white pixel 1 x 1
    //tiles 224 x 320
    //up 32 x 32
    //left 32 x 32
    //right 32 x 32
    //down 32 x 32
    //
    //**************************************************************************************************

    //useful primes for games
    public enum PRIMES
    {
        pri_one = 1, pri_two = 2, pri_three = 3, pri_five = 5, pri_seven = 7,
        pri_eleven = 11, pri_therteen = 13, pri_seventeen = 17, pri_nineteen = 19, pri_twentythree = 23
    };
    /// <summary>
    /// a path node class used in path finding
    /// </summary>
    public class RTSPathNode
    {
        //public
        public int x;
        public int y;
        public RTSPathNode parent;
        public int distence;
        public int steps;
        public int terrainValue;
        public RTSPathNode(int x, int y, int steps, int distence, int terrainValue, RTSPathNode parent = null)
        {
            this.x = x;
            this.y = y;
            this.parent = parent;
        }
        public int finalCost()
        {
            return distence + steps + terrainValue;
        }
    }
    /// <summary>
    /// world tile class used to build world scenes
    /// </summary>
    public class WorldTile
    {
        //protected
        protected Rectangle srcRect;
        protected Rectangle destRect;
        protected Texture2D source;
        protected Point2D position;
        protected int terrainValue;
        protected bool occupied;
        //public
        public WorldTile(int sx, int sy, int x, int y, int w, int h, Texture2D source, int terrainValue)
        {
            position = new Point2D(x, y);
            srcRect = new Rectangle(sx, sy, w, h);
            destRect = new Rectangle(x, y, w, h);
            this.source = source;
            this.terrainValue = terrainValue;
            occupied = false;
        }
        /// <summary>
        /// Loading contructor
        /// </summary>
        /// <param name="sr">stream reader reference</param>
        /// <param name="texture">World scene's tile texture reference</param>
        public WorldTile(System.IO.StreamReader sr, Texture2D texture)
        {
            source = texture;
            position = new Point2D(Convert.ToInt32(sr.ReadLine()), Convert.ToInt32(sr.ReadLine()));
            destRect.X = (int)position.X;
            destRect.Y = (int)position.Y;
            srcRect.X = Convert.ToInt32(sr.ReadLine());
            srcRect.Y = Convert.ToInt32(sr.ReadLine());
            srcRect.Width = Convert.ToInt32(sr.ReadLine());
            srcRect.Height = Convert.ToInt32(sr.ReadLine());
            destRect.Width = srcRect.Width;
            destRect.Height = srcRect.Height;
            terrainValue = Convert.ToInt32(sr.ReadLine());
            occupied = Convert.ToBoolean(sr.ReadLine());
        }
        /// <summary>
        /// position property
        /// </summary>
        public Point2D Position
        {
            get { return position; }
            set { position = value; }
        }
        /// <summary>
        /// terrain value property
        /// </summary>
        public int TerrainValue
        {
            get { return terrainValue; }
            set { terrainValue = value; }
        }
        /// <summary>
        /// occupied property
        /// </summary>
        public bool Occupied
        {
            get { return occupied; }
            set { occupied = value; }
        }
        /// <summary>
        /// draws the tile
        /// </summary>
        /// <param name="renderer">renderer reference</param>
        /// <param name="winx">window x offset</param>
        /// <param name="winy">window y offset</param>
        public void draw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            destRect.X = (int)(position.X - winx);
            destRect.Y = (int)(position.Y - winy);
            //sb.Draw(source, destRect, srcRect, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            SimpleDraw.draw(renderer, source, destRect);
        }
        /// <summary>
        /// saves the tile object's data for WorldScene to load later
        /// </summary>
        /// <param name="sw">StreamWriter object for writing data to</param>
        public void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine(position.X);
            sw.WriteLine(position.Y);
            sw.WriteLine(srcRect.X);
            sw.WriteLine(srcRect.Y);
            sw.WriteLine(srcRect.Width);
            sw.WriteLine(srcRect.Height);
            sw.WriteLine(terrainValue);
            sw.WriteLine(occupied);
        }
    }
    /// <summary>
    /// world scene object used to build worlds
    /// </summary>
    public class WorldScene
    {
        //protected
        protected WorldTile[,] tiles;
        protected int worldWidth;
        protected int worldHeight;
        protected int worldPixelsW;
        protected int worldPixelsH;
        protected int winWidth;
        protected int winHeight;
        protected int tileWidth;
        protected int tileHeight;
        protected int winx;
        protected int winy;
        protected Rectangle windowFrame;
        protected Rectangle windowDeadZone;
        protected string sourceName;
        protected Texture2D source;
        protected PriorityQueue<RTSPathNode> open;
        protected List<RTSPathNode> closed;
        protected int[,] sectors;
        protected int sectorWidth;
        protected int sectorHeight;
        protected int sectorRows;
        protected int sectorColumns;
        protected RTSResourceGrid ores;
        protected List<RTSCommander> cmdrs;
        protected List<RTSScript> scripts;
        protected MapGraph mg;

        /// <summary>
        /// checks the closed list for duplicates and shorter routes, if none found returns -1
        /// </summary>
        /// <param name="curr">current path node</param>
        /// <returns>boolean</returns>
        protected bool checkClosed(RTSPathNode tempNode)
        {
            for (int i = 0; i < closed.Count; i++)
            {
                if (tempNode.x == closed[i].x && tempNode.y == closed[i].y)
                {
                    if (tempNode.finalCost() > closed[i].finalCost())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// add's nodes to open list for path finding
        /// </summary>
        /// <param name="curr">current node</param>
        /// <param name="end">end point of search</param>
        protected void addNode(RTSPathNode curr, Point2D end)
        {
            RTSPathNode tempNode = null;
            Point2D tempPoint = null;
            if (inDomain(curr.x, curr.y - 1))
            {
                if (tiles[curr.x, curr.y - 1].TerrainValue > -1 && !tiles[curr.x, curr.y - 1].Occupied)
                {
                    tempPoint = new Point2D(curr.x, curr.y - 1);
                    tempNode = new RTSPathNode(curr.x, curr.y - 1, curr.steps + 1, tiles[(int)tempPoint.X, (int)tempPoint.Y].TerrainValue, (int)Point2D.calculateDistance(tempPoint, end), curr);
                    if (!checkClosed(tempNode))
                    {
                        open.enqueue(tempNode, tempNode.finalCost());
                    }
                }
            }
            if (inDomain(curr.x + 1, curr.y - 1))
            {
                if (tiles[curr.x + 1, curr.y - 1].TerrainValue > -1 && !tiles[curr.x + 1, curr.y - 1].Occupied)
                {
                    tempPoint = new Point2D(curr.x + 1, curr.y - 1);
                    tempNode = new RTSPathNode(curr.x + 1, curr.y - 1, curr.steps + 1, tiles[(int)tempPoint.X, (int)tempPoint.Y].TerrainValue, (int)Point2D.calculateDistance(tempPoint, end), curr);
                    if (!checkClosed(tempNode))
                    {
                        open.enqueue(tempNode, tempNode.finalCost());
                    }
                }
            }
            if (inDomain(curr.x + 1, curr.y))
            {
                if (tiles[curr.x + 1, curr.y].TerrainValue > -1 && !tiles[curr.x + 1, curr.y].Occupied)
                {
                    tempPoint = new Point2D(curr.x + 1, curr.y);
                    tempNode = new RTSPathNode(curr.x + 1, curr.y, curr.steps + 1, tiles[(int)tempPoint.X, (int)tempPoint.Y].TerrainValue, (int)Point2D.calculateDistance(tempPoint, end), curr);
                    if (!checkClosed(tempNode))
                    {
                        open.enqueue(tempNode, tempNode.finalCost());
                    }
                }
            }
            if (inDomain(curr.x + 1, curr.y + 1))
            {
                if (tiles[curr.x + 1, curr.y + 1].TerrainValue > -1 && !tiles[curr.x + 1, curr.y + 1].Occupied)
                {
                    tempPoint = new Point2D(curr.x + 1, curr.y + 1);
                    tempNode = new RTSPathNode(curr.x + 1, curr.y + 1, curr.steps + 1, tiles[(int)tempPoint.X, (int)tempPoint.Y].TerrainValue, (int)Point2D.calculateDistance(tempPoint, end), curr);
                    if (!checkClosed(tempNode))
                    {
                        open.enqueue(tempNode, tempNode.finalCost());
                    }
                }
            }
            if (inDomain(curr.x, curr.y + 1))
            {
                if (tiles[curr.x, curr.y + 1].TerrainValue > -1 && !tiles[curr.x, curr.y + 1].Occupied)
                {
                    tempPoint = new Point2D(curr.x, curr.y + 1);
                    tempNode = new RTSPathNode(curr.x, curr.y + 1, curr.steps + 1, tiles[(int)tempPoint.X, (int)tempPoint.Y].TerrainValue, (int)Point2D.calculateDistance(tempPoint, end), curr);
                    if (!checkClosed(tempNode))
                    {
                        open.enqueue(tempNode, tempNode.finalCost());
                    }
                }
            }
            if (inDomain(curr.x - 1, curr.y + 1))
            {
                if (tiles[curr.x - 1, curr.y + 1].TerrainValue > -1 && !tiles[curr.x - 1, curr.y + 1].Occupied)
                {
                    tempPoint = new Point2D(curr.x - 1, curr.y + 1);
                    tempNode = new RTSPathNode(curr.x - 1, curr.y + 1, curr.steps + 1, tiles[(int)tempPoint.X, (int)tempPoint.Y].TerrainValue, (int)Point2D.calculateDistance(tempPoint, end), curr);
                    if (!checkClosed(tempNode))
                    {
                        open.enqueue(tempNode, tempNode.finalCost());
                    }
                }
            }
            if (inDomain(curr.x - 1, curr.y))
            {
                if (tiles[curr.x - 1, curr.y].TerrainValue > -1 && !tiles[curr.x - 1, curr.y].Occupied)
                {
                    tempPoint = new Point2D(curr.x - 1, curr.y);
                    tempNode = new RTSPathNode(curr.x - 1, curr.y, curr.steps + 1, tiles[(int)tempPoint.X, (int)tempPoint.Y].TerrainValue, (int)Point2D.calculateDistance(tempPoint, end), curr);
                    if (!checkClosed(tempNode))
                    {
                        open.enqueue(tempNode, tempNode.finalCost());
                    }
                }
            }
            if (inDomain(curr.x - 1, curr.y - 1))
            {
                if (tiles[curr.x - 1, curr.y - 1].TerrainValue > -1 && !tiles[curr.x - 1, curr.y - 1].Occupied)
                {
                    tempPoint = new Point2D(curr.x - 1, curr.y - 1);
                    tempNode = new RTSPathNode(curr.x - 1, curr.y - 1, curr.steps + 1, tiles[(int)tempPoint.X, (int)tempPoint.Y].TerrainValue, (int)Point2D.calculateDistance(tempPoint, end), curr);
                    if (!checkClosed(tempNode))
                    {
                        open.enqueue(tempNode, tempNode.finalCost());
                    }
                }
            }
        }
        /// <summary>
        /// create a path after calcualting course
        /// </summary>
        /// <param name="curr">current path node</param>
        /// <returns>list of points</returns>
        protected List<Point2D> createPath(RTSPathNode curr)
        {
            List<Point2D> path = new List<Point2D>();
            while (curr.parent != null)
            {
                path.Insert(0, new Point2D(curr.x, curr.y));
                curr = curr.parent;
            }
            return path;
        }
        //public
        /// <summary>
        /// builds a defualt world scene object
        /// </summary>
        /// <param name="width">width in tiles of world</param>
        /// <param name="height">height in tiles of world</param>
        /// <param name="tileWidth">width of tiles in pixels</param>
        /// <param name="tileHeight">height of tiles in pixels</param>
        /// <param name="source">source Texture2D</param>
        /// <param name="sourceName">name of source Texture2D</param>
        public WorldScene(int width, int height, int tileWidth, int tileHeight,
            Texture2D source, string sourceName)
        {
            worldWidth = width;
            worldHeight = height;
            worldPixelsW = width * tileWidth;
            worldPixelsH = height * tileHeight;
            winWidth = 15;
            winHeight = 15;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            winx = 0;
            winy = 0;
            this.sourceName = sourceName;
            this.source = source;
            tiles = new WorldTile[worldWidth, worldHeight];
            for (int wx = 0; wx < worldWidth; wx++)
            {
                for (int wy = 0; wy < worldHeight; wy++)
                {
                    tiles[wx, wy] = new WorldTile(0, 0, wx * tileWidth, wy * tileHeight, tileWidth,
                        tileHeight, source, 0);
                }
            }
            windowFrame = new Rectangle(winx, winy, winWidth * tileWidth, winHeight * tileHeight);
            windowDeadZone = new Rectangle(winx - tileWidth, winy - tileHeight, winWidth * tileWidth +
                (2 * tileWidth), winHeight * tileHeight + (2 * tileHeight));
            sectorWidth = tileWidth * 4;
            sectorHeight = tileHeight * 4;
            sectorColumns = (worldWidth * tileWidth) / sectorWidth;
            sectorRows = (worldHeight * tileHeight) / sectorHeight;
            sectors = new int[sectorColumns, sectorRows];
            int ii = 0;
            for (int ix = 0; ix < sectorColumns; ix++)
            {
                for (int iy = 0; iy < sectorRows; iy++)
                {
                    sectors[ix, iy] = ii;
                    ii++;
                }
            }
            cmdrs = new List<RTSCommander>();
            scripts = new List<RTSScript>();
            ores = new RTSResourceGrid(worldWidth, worldHeight, tileWidth, tileHeight);
        }
        /// <summary>
        /// world scene constructor that builds from a file stream
        /// </summary>
        /// <param name="sr">stream reader</param>
        public WorldScene(System.IO.StreamReader sr)
        {
            sr.ReadLine();
            worldWidth = Convert.ToInt32(sr.ReadLine());
            worldHeight = Convert.ToInt32(sr.ReadLine());
            worldPixelsW = Convert.ToInt32(sr.ReadLine());
            worldPixelsH = Convert.ToInt32(sr.ReadLine());
            winWidth = Convert.ToInt32(sr.ReadLine());
            winHeight = Convert.ToInt32(sr.ReadLine());
            tileWidth = Convert.ToInt32(sr.ReadLine());
            tileHeight = Convert.ToInt32(sr.ReadLine());
            winx = Convert.ToInt32(sr.ReadLine());
            winy = Convert.ToInt32(sr.ReadLine());
            sourceName = sr.ReadLine();
            source = TextureBank.getTexture(sourceName);
            ores = new RTSResourceGrid(sr);
            int num = Convert.ToInt32(sr.ReadLine());
            cmdrs = new List<RTSCommander>();
            for (int c = 0; c < num; c++)
            {
                RTSCommander cmdr = new RTSCommander(sr, this);
                cmdrs.Add(cmdr);
            }
            windowFrame = new Rectangle(winx, winy, winWidth * tileWidth, winHeight * tileHeight);
            windowDeadZone = new Rectangle(winx - tileWidth, winy - tileHeight, winWidth * tileWidth +
                (2 * tileWidth), winHeight * tileHeight + (2 * tileHeight));
            sectorWidth = tileWidth * 4;
            sectorHeight = tileHeight * 4;
            sectorColumns = (worldWidth * tileWidth) / sectorWidth;
            sectorRows = (worldHeight * tileHeight) / sectorHeight;
            sectors = new int[sectorColumns, sectorRows];
            int ii = 0;
            for (int ix = 0; ix < sectorColumns; ix++)
            {
                for (int iy = 0; iy < sectorRows; iy++)
                {
                    sectors[ix, iy] = ii;
                    ii++;
                }
            }
            scripts = new List<RTSScript>();
            num = (int)Convert.ToInt32(sr.ReadLine());
            for (int i = 0; i < num; i++)
            {
                loadCustomScripts(sr);
            }
            mg = new MapGraph(worldWidth, worldHeight);
        }
        /// <summary>
        /// Loads a game's custom scripts
        /// </summary>
        /// <param name="sr">Stream reader reference</param>
        public virtual void loadCustomScripts(System.IO.StreamReader sr)
        {
            switch (sr.ReadLine())
            {
                case "script":
                    scripts.Add(new RTSScript(this, sr));
                    break;
            }
        }
        /// <summary>
        /// sets the current position of the window
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        public void setWindow(int x, int y)
        {
            winx = x;
            if (winx < 0)
            {
                winx = 0;
            }
            if (winx + (winWidth * tileWidth) > worldWidth * tileWidth)
            {
                winx = (worldWidth * tileWidth) - (winWidth * tileWidth);
            }
            winy = y;
            if (winy < 0)
            {
                winy = 0;
            }
            if (winy + (winHeight * tileHeight) > worldHeight * tileHeight)
            {
                winy = (worldHeight * tileHeight) - (winHeight * tileHeight);
            }
            windowFrame.setPos(winx, winy);
            windowDeadZone.setPos(winx - tileWidth, winy - tileHeight);
        }
        /// <summary>
        /// shift the world window
        /// </summary>
        /// <param name="shiftx">how much to shift the x axis</param>
        /// <param name="shifty">how much to shift the y axis</param>
        public void shiftWindow(int shiftx, int shifty)
        {
            if (winx + shiftx >= 0 && winx + (winWidth * tileWidth) + shiftx <= (worldWidth * tileWidth))
            {
                winx += shiftx;
                windowFrame.move(shiftx, 0);
                windowDeadZone.move(shiftx, 0);
            }
            if (winy + shifty >= 0 && winy + (winHeight * tileHeight) + shifty <= (worldHeight * tileHeight))
            {
                winy += shifty;
                windowFrame.move(0, shifty);
                windowDeadZone.move(0, shifty);
            }
        }
        /// <summary>
        /// draws the tiles of the world in the window
        /// </summary>
        /// <param name="renderer">renderer reference</param>
        /// <param name="shiftx">shift render position along x axis</param>
        /// <param name="shifty">shift render position along y axis</param>
        public void draw(IntPtr renderer, int shiftx = 0, int shifty = 0)
        {
            for (int tx = winx / tileWidth; tx < (winx / tileWidth) + winWidth; tx++)
            {
                for (int ty = winy / tileHeight; ty < (winy / tileHeight) + winHeight; ty++)
                {
                    if (tiles[tx, ty] != null)
                    {
                        tiles[tx, ty].draw(renderer, (winx - shiftx), (winy - shifty));
                    }
                }
            }
        }
        /// <summary>
        /// Draws tile column and row numbers 
        /// </summary>
        /// <param name="renderer">SpriteBatch reference</param>
        /// <param name="colour">text rendering colour</param>
        /// <param name="shiftx">shift x value</param>
        /// <param name="shifty">shift y value</param>
        /// <param name="scaler">text scaling value</param>
        public void drawTileNumbers(IntPtr renderer, string colour, int shiftx = 0, int shifty = 0, float scaler = 1)
        {
            int i = 0;
            for (int tx = winx / tileWidth; tx < (winx / tileWidth) + winWidth; tx++)
            {
                //sb.DrawString(font, tx.ToString(), new Vector2(shiftx + (i * tileWidth), shifty - tileHeight), colour);
                SimpleFont.drawColourString(renderer, tx.ToString(), shiftx + (i * tileWidth), shifty - tileHeight, colour, scaler);
                i++;
            }
            i = 0;
            for (int ty = winy / tileHeight; ty < (winy / tileHeight) + winHeight; ty++)
            {
                //sb.DrawString(font, ty.ToString(), new Vector2(shiftx - tileWidth, shifty + (i * tileHeight)), colour);
                SimpleFont.drawColourString(renderer, ty.ToString(), shiftx - tileWidth, shifty + (i * tileHeight), colour, scaler);
                i++;
            }
        }
        /// <summary>
        /// Calculates closest multiple of the tile width given a value
        /// </summary>
        /// <param name="val">Value to provide</param>
        /// <returns>A multiple of tile width</returns>
        public int multipleTW(int val)
        {
            return (val / tileWidth) * tileWidth;
        }
        /// <summary>
        /// Calculates closest multiple of the tile height given a value
        /// </summary>
        /// <param name="val">Value to provide</param>
        /// <returns>A multiple of tile height</returns>
        public int multipleTH(int val)
        {
            return (val / tileHeight) * tileHeight;
        }
        /// <summary>
        /// tests if a provided position is inside the bonds of the world
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <returns>boolean</returns>
        public bool inDomain(int x, int y)
        {
            if (x >= 0 && x < worldWidth)
            {
                if (y >= 0 && y < worldHeight)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// returns a tile at location if a tile exists and null if position is empty
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <returns>returns tile at position or null if empty</returns>
        public WorldTile getTile(int x, int y)
        {
            if (inDomain(x, y))
            {
                return tiles[x, y];
            }
            return null;
        }
        /// <summary>
        /// Sets a tile position to new tile of parameters
        /// </summary>
        /// <param name="x">X position in pixels</param>
        /// <param name="y">Y position in pixels</param>
        /// <param name="sx">Tile source x</param>
        /// <param name="sy">Tile source Y</param>
        /// <param name="gb">Graphics bank reference</param>
        /// <param name="tilesName">Tile source name</param>
        public void setTile(int x, int y, int sx, int sy, string tilesName, int terrainValue = 0)
        {
            if (inDomain(x / tileWidth, y / tileHeight))
            {
                tiles[x / tileWidth, y / tileHeight] = new WorldTile(sx, sy,
                    (x / tileWidth) * tileWidth, (y / tileHeight) * tileHeight, tileWidth, tileHeight,
                    TextureBank.getTexture(tilesName), terrainValue);
            }
        }
        /// <summary>
        /// Erase a tile at the specified position
        /// </summary>
        /// <param name="x">X position in pixels</param>
        /// <param name="y">Y position in pixels</param>
        public void eraseTile(int x, int y)
        {
            if (inDomain(x / tileWidth, y / tileHeight))
            {
                tiles[x / tileWidth, y / tileHeight] = null;
            }
        }
        /// <summary>
        /// calculate a path through the world
        /// </summary>
        /// <param name="start">start of path</param>
        /// <param name="end">end of path</param>
        /// <param name="maxSearches"> the maximum number of search loops</param>
        /// <returns>returns a list of points</returns>
        public List<Point2D> findPath(Point2D start, Point2D end, int maxSearches = 1000)
        {
            open = new PriorityQueue<RTSPathNode>();
            closed = new List<RTSPathNode>();
            RTSPathNode curr = new RTSPathNode((int)start.X, (int)start.Y, 0, tiles[(int)start.X, (int)start.Y].TerrainValue, Point2D.calculateDistance(start, end), null);
            int searches = 0;
            Point2D currPoint = new Point2D(curr.x, curr.y);
            do
            {
                addNode(curr, end);
                closed.Add(curr);
                curr = open.dequeue();
                searches++;
            }
            while (currPoint != end && searches < maxSearches);
            return createPath(curr);
        }
        /// <summary>
        /// Save World scene data
        /// </summary>
        /// <param name="sw">Stream writer reference</param>
        public virtual void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======WorldScene Data======");
            sw.WriteLine(worldWidth);
            sw.WriteLine(worldHeight);
            sw.WriteLine(worldPixelsW);
            sw.WriteLine(worldPixelsH);
            sw.WriteLine(winWidth);
            sw.WriteLine(winHeight);
            sw.WriteLine(tileWidth);
            sw.WriteLine(tileHeight);
            sw.WriteLine(winx);
            sw.WriteLine(winy);
            sw.WriteLine(sourceName);
            ores.saveData(sw);
            sw.WriteLine(cmdrs.Count);
            for (int c = 0; c < cmdrs.Count; c++)
            {
                cmdrs[c].saveData(sw);
            }
            sw.WriteLine(scripts.Count);
            for (int i = 0; i < scripts.Count; i++)
            {
                scripts[i].saveData(sw);
            }
        }
        /// <summary>
        /// Collects a list of points given the perameters of an area
        /// </summary>
        /// <param name="x">Area center x value</param>
        /// <param name="y">Area center y value</param>
        /// <param name="r">Area redius</param>
        /// <param name="curve">Value to control curve of area collected</param>
        /// <param name="passability">Flag to toggle whether or not passability counts</param>
        /// <returns>Returns null if incorrect perameters entered else a list of valid point</returns>
        public List<Point2D> collectArea(int x, int y, int r, int curve, bool passability = false)
        {
            if (curve >= r)
            {
                return null;
            }
            int start = y - r;
            int end = y + r;
            int tsx = x - curve;
            int tex = x + curve;
            int tsy1 = y - r + curve;
            int tsy2 = y + r - curve;
            if (curve < tsy1 - start)
            {
                return null;
            }
            List<Point2D> points = new List<Point2D>();
            for (int v = start; v < end; v++)
            {
                for (int h = tsx; h < tex; h++)
                {
                    if (inDomain(h, v))
                    {
                        if (tiles[h, v] != null)
                        {
                            if (passability)
                            {
                                if (!tiles[h, v].Occupied)
                                {
                                    points.Add(new Point2D(h, v));
                                }
                            }
                            else
                            {
                                points.Add(new Point2D(h, v));
                            }
                        }
                    }
                }
                if (v < tsy1)
                {
                    tsx -= 1;
                    tex += 1;
                }
                if (v > tsy2)
                {
                    tsx += 1;
                    tex -= 1;
                }
            }
            return points;
        }
        /// <summary>
        /// sets all tiles to unoccupied
        /// </summary>
        public void clearOccupiedSpaces()
        {
            for (int x = 0; x < worldWidth; x++)
            {
                for (int y = 0; y < worldHeight; y++)
                {
                    if (tiles[x, y] != null)
                    {
                        tiles[x, y].Occupied = false;
                    }
                }
            }
        }
        /// <summary>
        /// returns the sector a given position is in the world
        /// </summary>
        /// <param name="tx">x position</param>
        /// <param name="ty">y position</param>
        /// <returns>returns an integar or -1 if outside bounds</returns>
        public int getSector(int tx, int ty)
        {
            if (tx >= 0 && tx <= worldPixelsW)
            {
                if (ty >= 0 && ty <= worldPixelsH)
                {
                    return sectors[tx / sectorWidth, ty / sectorHeight];
                }
            }
            return -1;
        }
        /// <summary>
        /// Tests a space to identify if it is clear of objects or not
        /// </summary>
        /// <param name="tx">X position in pixels</param>
        /// <param name="ty">Y position in pixels</param>
        /// <param name="w">Width in tiles</param>
        /// <param name="h">Height in tiles</param>
        /// <returns></returns>
        public bool checkSpace(int tx, int ty, int w, int h)
        {
            for (int col = tx / tileWidth; col < (tx / tileWidth) + w; col++)
            {
                for (int row = ty / tileHeight; row < (ty / tileHeight) + h; row++)
                {
                    if (inDomain(col, row))
                    {
                        if (tiles[col, row].Occupied)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Tests if a position is on screen
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>True if point on screen else returns false</returns>
        public bool onScreen(int x, int y)
        {
            if (x >= winx * tileWidth && x < (winx * tileWidth) + (winWidth * tileWidth))
            {
                if (y >= winy * tileHeight && x < (winy * tileHeight) + (winHeight * tileHeight))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Converts a radian value into degrees
        /// </summary>
        /// <param name="val">Value to convert</param>
        /// <returns>Returns result as a double</returns>
        public double radiansToDegree(double val)
        {
            return val * (180 / Math.PI);
        }
        /// <summary>
        /// Converts a degrees value into radians
        /// </summary>
        /// <param name="val">Value to convert</param>
        /// <returns>Returns result as a double</returns>
        public double degreesToRadians(double val)
        {
            return val * (Math.PI / 180);
        }
        /// <summary>
        /// World width in pixels property
        /// </summary>
        public int WorldPixelsW
        {
            get { return worldPixelsW; }
        }
        /// <summary>
        /// World height in pixels property
        /// </summary>
        public int WorldPixelsH
        {
            get { return worldPixelsH; }
        }
        /// <summary>
        /// Tile width property
        /// </summary>
        public int TileWidth
        {
            get { return tileWidth; }
        }
        /// <summary>
        /// tile height property
        /// </summary>
        public int TileHeight
        {
            get { return tileHeight; }
        }
        /// <summary>
        /// Ores property
        /// </summary>
        public RTSResourceGrid Ores
        {
            get { return ores; }
        }
        /// <summary>
        /// Commanders property
        /// </summary>
        public List<RTSCommander> CMDRS
        {
            get { return cmdrs; }
        }
        /// <summary>
        /// WinX property
        /// </summary>
        public int Winx
        {
            get { return winx; }
        }
        /// <summary>
        /// WinY property
        /// </summary>
        public int Winy
        {
            get { return winy; }
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
        /// World width property
        /// </summary>
        public int WorldWidth
        {
            get { return worldWidth; }
        }
        /// <summary>
        /// World height property
        /// </summary>
        public int WorldHeight
        {
            get { return worldHeight; }
        }
        /// <summary>
        /// Source name property
        /// </summary>
        public string SourceName
        {
            get { return sourceName; }
        }
        /// <summary>
        /// Scripts property
        /// </summary>
        public List<RTSScript> Scripts
        {
            get { return scripts; }
        }
        /// <summary>
        /// MG property
        /// </summary>
        public MapGraph MG
        {
            get { return mg; }
        }
    }
}
