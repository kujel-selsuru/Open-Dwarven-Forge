//====================================================
//Written by Kujel Selsuru
//Last Updated 20/12/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// Chamber direction enumeration
    /// </summary>
    public enum CHAMBER { UP = 0, RIGHT, DOWN, LEFT };

    /// <summary>
    /// OpenWorldCell class
    /// </summary>
    public class OpenWorldCell
    {
        //protected 
        protected XenoTileSys tiles;
        protected AutoTileSys autoTiles;
        protected SectorGraph64 sg;
        protected string name;
        protected Texture2D source;
        protected Texture2D autoSource;
        protected Texture2D interSource;
        protected Texture2D interAutoSource;
        protected int cellx;//cell's x position in cells
        protected int celly;//cell's y position in cells
        protected int worldx;//cell's x position in pixels
        protected int worldy;//cell's y position in pixels
        protected bool isInteriorCell;
        protected int cellLeftSide;
        protected int cellTopSide;
        protected int cellRightSide;
        protected int cellBottomSide;
        protected List<XenoPortal> portals;
        protected List<XenoBuilding> buildings;
        protected List<XenoDoodad> doodads;
        protected bool spawnable;
        protected Random rand;
        protected MapGraph localMG;

        //public
        /// <summary>
        /// OpenWorldCell constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="autoSource">Texture2D reference</param>
        /// <param name="width">Width in tiles</param>
        /// <param name="height">Height in tiles</param>
        /// <param name="tileWidth">Tile Width</param>
        /// <param name="tileHeight">Tile Height</param>
        /// <param name="winWidth">Window width in tiles</param>
        /// <param name="winHeight">Window height in tiles</param>
        /// <param name="winx">Window x value</param>
        /// <param name="winy">Window y value</param>
        /// <param name="cellx">Cell's x position in cells</param>
        /// <param name="celly">Cell's y position in cells</param>
        /// <param name="name">Cell name</param>
        /// <param name="spawnable">Spawnable flag</param>
        /// <param name="fill">Fill flag</param>
        /// <param name="randomize">Randomize flag</param>
        /// <param name="sourceW">Source width in tiles</param>
        /// <param name="sourceH">Source height in tiles</param>
        public OpenWorldCell(Texture2D source, Texture2D autoSource, int width, int height, int tileWidth, 
            int tileHeight, int winWidth, int winHeight, int winx, int winy, int cellx, int celly, 
            string name, bool spawnable = true, bool fill = true, bool randomize = false, int sourceW = 5, int sourceH = 7)
        {
            this.source = source;
            tiles = new XenoTileSys(source, width, height, winWidth, winHeight, winx, winy, tileWidth, tileHeight, fill, randomize, sourceW, sourceH);
            autoTiles = new AutoTileSys(autoSource, width, height, tileWidth, tileHeight);
            sg = new SectorGraph64(width * tileWidth, height * tileHeight);
            this.name = name;
            this.cellx = cellx;
            this.celly = celly;
            this.worldx = cellx * (tileWidth * width);
            this.worldy = celly * (tileHeight * height);
            isInteriorCell = false;
            cellLeftSide = cellx * (tileWidth * width);
            cellTopSide = celly * (tileHeight * height);
            cellRightSide = cellLeftSide + (tileWidth * width);
            cellBottomSide = cellTopSide + (tileHeight * height);
            portals = new List<XenoPortal>();
            buildings = new List<XenoBuilding>();
            doodads = new List<XenoDoodad>();
            this.spawnable = spawnable;
            interSource = source;
            interAutoSource = autoSource;
            rand = new Random((int)System.DateTime.Now.Ticks);
            localMG = new MapGraph(width, height);
            localMG.setAllTrue();
        }
        /// <summary>
        /// OpenWorld from file constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="autoSource">Texture2D reference</param>
        /// <param name="winWidth">Window width in tiles</param>
        /// <param name="winHeight">Window height in tiles</param>
        /// <param name="sr">StreamReader reference</param>
        public OpenWorldCell(Texture2D source, Texture2D autoSource, int winWidth, int winHeight, StreamReader sr)
        {
            this.source = source;
            this.autoSource = autoSource;
            sr.ReadLine();//discard testing data
            name = sr.ReadLine();
            cellx = Convert.ToInt32(sr.ReadLine());
            celly = Convert.ToInt32(sr.ReadLine());
            worldx = Convert.ToInt32(sr.ReadLine());
            worldy = Convert.ToInt32(sr.ReadLine());
            spawnable = Convert.ToBoolean(sr.ReadLine());
            tiles = new XenoTileSys(source, winWidth, winHeight, sr);
            autoTiles = new AutoTileSys(autoSource, sr);
            sg = new SectorGraph64(tiles.Width * tiles.TileWidth, tiles.Height * tiles.TileHeight);
            isInteriorCell = Convert.ToBoolean(sr.ReadLine());
            sr.ReadLine();//discard testing data
            int num = Convert.ToInt32(sr.ReadLine());
            portals = new List<XenoPortal>();
            for(int p = 0; p < num; p++)
            {
                //portals use generic graphics for easier loading
                portals.Add(new XenoPortal(TextureBank.getTexture("portal"), sr));
            }
            sr.ReadLine();//discard testing data
            num = Convert.ToInt32(sr.ReadLine());
            buildings = new List<XenoBuilding>();
            for (int p = 0; p < num; p++)
            {
                //buildings use generic graphics for easier loading (can internally change reference)
                buildings.Add(new XenoBuilding(sr));
            }
            sr.ReadLine();//discard testing data
            num = Convert.ToInt32(sr.ReadLine());
            doodads = new List<XenoDoodad>();
            for (int p = 0; p < num; p++)
            {
                //doodads use generic graphics for easier loading (can internally change reference)
                doodads.Add(new XenoDoodad(TextureBank.getTexture("doodad"), sr));
            }
            cellLeftSide = cellx * (Width * tiles.TileWidth);
            cellTopSide = celly * (Height * tiles.TileHeight);
            cellRightSide = cellLeftSide + (tiles.TileWidth * tiles.Width);
            cellBottomSide = cellTopSide + (tiles.TileHeight * tiles.Height);
            interSource = source;
            interAutoSource = autoSource;
            rand = new Random((int)System.DateTime.Now.Ticks);
            localMG = new MapGraph(tiles.TileWidth, tiles.TileHeight);
            localMG.setAllTrue();
        }
        /// <summary>
        /// Uninitialized constructor, used for loading custom constructors
        /// </summary>
        public OpenWorldCell()
        {
            this.source = default(Texture2D);
            this.autoSource = default(Texture2D);
            name = "";
            cellx = 0;
            celly = 0;
            worldx = 0;
            worldy = 0;
            spawnable = false;
            tiles = null;
            autoTiles = null;
            sg = null;
            isInteriorCell = false;
            portals = null;
            buildings = null;
            doodads = null;
            cellLeftSide = 0;
            cellTopSide = 0;
            cellRightSide = 0;
            cellBottomSide = 0;
            rand = new Random((int)System.DateTime.Now.Ticks);
            localMG = new MapGraph(1, 1);
            localMG.setAllTrue();
        }
        /// <summary>
        /// Saves data
        /// </summary>
        /// <param name="filePath">File path to save cell to</param>
        /// <param name="saveAuto">Close fileStream when done flag value</param>
        public virtual void saveData(string filePath, string cellName = "", bool saveAuto = false)
        {
            if (cellName == "")
            {
                filePath += "cell_" + cellx + "_" + celly + ".owc";
            }
            else
            {
                filePath += "cell_" + cellName + ".owc";
            }
            StreamWriter sw = new StreamWriter(filePath);
            sw.WriteLine("======OpenWorldCell Data======");
            sw.WriteLine(name);
            sw.WriteLine(cellx);
            sw.WriteLine(celly);
            sw.WriteLine(worldx);
            sw.WriteLine(worldy);
            sw.WriteLine(spawnable);
            tiles.saveData(sw);
            autoTiles.saveData(sw);
            sw.WriteLine(isInteriorCell);
            sw.WriteLine("======Portals Data======");
            sw.WriteLine(portals.Count);
            for(int p = 0; p < portals.Count; p++)
            {
                portals[p].saveData(sw);
            }
            sw.WriteLine("======Buildings Data======");
            sw.WriteLine(buildings.Count);
            for (int b = 0; b < buildings.Count; b++)
            {
                buildings[b].saveData(sw);
            }
            sw.WriteLine("======Doodads Data======");
            sw.WriteLine(doodads.Count);
            for (int d = 0; d < doodads.Count; d++)
            {
                doodads[d].saveData(sw);
            }
            localMG.save(sw);
            if(saveAuto)
            {
                sw.Close();
            }
        }
        /// <summary>
        /// Saves data
        /// </summary>
        /// <param name="sw">File stream to save to</param>
        /// <param name="saveAuto">Close fileStream when done flag value</param>
        public virtual void saveData(StreamWriter sw, bool saveAuto = false)
        {
            sw.WriteLine("======OpenWorldCell Data======");
            sw.WriteLine(name);
            sw.WriteLine(cellx);
            sw.WriteLine(celly);
            sw.WriteLine(worldx);
            sw.WriteLine(worldy);
            sw.WriteLine(spawnable);
            tiles.saveData(sw);
            autoTiles.saveData(sw);
            sw.WriteLine(isInteriorCell);
            sw.WriteLine("======Portals Data======");
            sw.WriteLine(portals.Count);
            for (int p = 0; p < portals.Count; p++)
            {
                portals[p].saveData(sw);
            }
            sw.WriteLine("======Buildings Data======");
            sw.WriteLine(buildings.Count);
            for (int b = 0; b < buildings.Count; b++)
            {
                buildings[b].saveData(sw);
            }
            sw.WriteLine("======Doodads Data======");
            sw.WriteLine(doodads.Count);
            for (int d = 0; d < doodads.Count; d++)
            {
                doodads[d].saveData(sw);
            }
            //localMG.save(sw);
            if (saveAuto)
            {
                sw.Close();
            }
        }
        /// <summary>
        /// Draws OpenWorldCell
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window x position</param>
        /// <param name="winy">Window y position</param>
        /// <param name="shiftx">Shift x value</param>
        /// <param name="shifty">Shift y value</param>
        public virtual void draw(IntPtr renderer, int winx, int winy, int shiftx = 0, int shifty = 0)
        {
            tiles.draw(renderer, winx - (cellx * (tiles.Width * tiles.TileWidth)), winy - (celly * (tiles.Height * tiles.TileHeight)), shiftx, shifty);
            autoTiles.draw(renderer, winx - (cellx * (tiles.Width * tiles.TileWidth)), winy - (celly * (tiles.Height * tiles.TileHeight)), tiles.WinWidth, tiles.WinHeight, shiftx, shifty);
            for (int b = 0; b < buildings.Count; b++)
            {
                buildings[b].draw(renderer, winx - (cellx * (tiles.Width * tiles.TileWidth)) + shiftx, winy - (celly * (tiles.Height * tiles.TileHeight)) + shifty);
            }
            for (int d = 0; d < doodads.Count; d++)
            {
                doodads[d].draw(renderer, winx - (cellx * (tiles.Width * tiles.TileWidth)) + shiftx, winy - (celly * (tiles.Height * tiles.TileHeight)) + shifty);
            }
            for (int p = 0; p < portals.Count; p++)
            {
                portals[p].draw(renderer, winx - (cellx * (tiles.Width * tiles.TileWidth)) + shiftx, winy - (celly * (tiles.Height * tiles.TileHeight)) + shifty);
            }
        }
        /// <summary>
        /// Draws the local MapGraph (doesn't work correctly)
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        /// <param name="shiftx">Shift x offset</param>
        /// <param name="shifty">shift y offset</param>
        public void drawLocalMG(IntPtr renderer, int winx, int winy, int shiftx = 0, int shifty = 0)
        {
            Rectangle rect = new Rectangle(0, 0, tiles.TileWidth, tiles.TileHeight);
            int wx = (winx - (cellx * (tiles.Width * tiles.TileWidth))) / tiles.TileWidth;
            int wy = (winy - (celly * (tiles.Height * tiles.TileHeight))) / tiles.TileHeight;
            for (int x = wx; x < wx + tiles.WinWidth; x++)
            {
                for (int y = wy; y < wy + tiles.WinHeight; y++)
                {
                    if(localMG.getCell(x, y) == false)
                    {
                        rect.X = (x * tiles.TileWidth) - Math.Abs(((cellx * (tiles.Width * tiles.TileWidth)) - winx)) + shiftx;
                        if(rect.X < 0)
                        {
                            rect.X = -rect.X;
                        }
                        rect.Y = (y * tiles.TileHeight) - Math.Abs(((celly * (tiles.Height * tiles.TileHeight)) - winy)) + shifty;
                        if(rect.Y < 0)
                        {
                            rect.Y = -rect.Y;
                        }
                        DrawRects.drawRect(renderer, rect, ColourBank.getColour(XENOCOLOURS.BLUE), true);
                    }
                }
            }
        }
        /// <summary>
        /// Loads an alternative tile source from TextureBank
        /// </summary>
        /// <param name="altTileSrc">Name of alt tile source</param>
        public void loadAltTileSrc(string altTileSrc)
        {
            source = TextureBank.getTexture(altTileSrc);
        }
        /// <summary>
        /// Loads an alternative autoTile source from TextureBank
        /// </summary>
        /// <param name="altTileSrc">Name of alt tile source</param>
        public void loadAltAutoTileSrc(string altTileSrc)
        {
            autoSource = TextureBank.getTexture(altTileSrc);
        }
        /// <summary>
        /// Calculates a position's quadrent within cell
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Int</returns>
        public int getQuadrent(float x, float y)
        {
            if(x < (tiles.TileWidth * tiles.Width) / 2)//on left side of cell
            {
                if(y < (tiles.TileHeight * tiles.Height) / 2)//on top half of cell
                {
                    return 1;
                }
                else//on bottom half of cell
                {
                    return 4;
                }
            }
            else//on right side of cell
            {
                if (y < (tiles.TileHeight * tiles.Height) / 2)//on top half of cell
                {
                    return 2;
                }
                else//on bottom half of cell
                {
                    return 3;
                }
            }
        }
        /// <summary>
        /// Check if a position in within cell
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Boolean</returns>
        public bool inCell(float x, float y)
        {
            if(x >= cellLeftSide && x < cellRightSide)
            {
                if (y >= cellTopSide && y < cellBottomSide)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if there is a autoTile at location
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns></returns>
        public bool isAutoTile(int x, int y)
        {
            if(autoTiles.isTile(x, y))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Updates MapGraph for XenoBuildings and XenoDoodads
        /// </summary>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="shiftRight">Shift right value</param>
        /// <param name="shiftDown">Shift down value</param>
        /// <param name="interior">Is an interior cell</param>
        public virtual void updateMG(MapGraph mg, int shiftRight, int shiftDown, bool interior)
        {
            for (int b = 0; b < buildings.Count; b++)
            {
                buildings[b].updateMG(mg, shiftRight, shiftDown, interior);
            }
            for (int d = 0; d < doodads.Count; d++)
            {
                if (interior)
                {
                    mg.setCell((int)doodads[d].X / tiles.TileWidth, (int)doodads[d].Y / tiles.TileHeight, false);
                }
                else
                {
                    mg.setCell((int)doodads[d].X / tiles.TileWidth + shiftRight, (int)doodads[d].Y / tiles.TileHeight + shiftDown, false);
                }
            }
        }
        /// <summary>
        /// Updates MapGraph for XenoBuildings and XenoDoodads (only sets lower half)
        /// </summary>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="shiftRight">Shift right value</param>
        /// <param name="shiftDown">Shift down value</param>
        /// <param name="interior">Is an interior cell</param>
        public virtual void updateMGLower(MapGraph mg, int shiftRight, int shiftDown, bool interior)
        {
            for (int b = 0; b < buildings.Count; b++)
            {
                buildings[b].updateMGLower(mg, shiftRight, shiftDown, interior);
            }
            for (int d = 0; d < doodads.Count; d++)
            {
                if (interior)
                {
                    mg.setCell((int)doodads[d].X / tiles.TileWidth, (int)doodads[d].Y / tiles.TileHeight,false);
                }
                else
                {
                    mg.setCell((int)doodads[d].X / tiles.TileWidth + (shiftRight / tiles.Width), (int)doodads[d].Y / tiles.TileHeight + (shiftDown / tiles.Height), false);
                }
            }
        }
        /// <summary>
        /// Updates the local MapGraph only
        /// </summary>
        public virtual void updateLocalMG()
        {
            localMG.setAllTrue();
            for(int tx = 0; tx < Width; tx++)
            {
                for (int ty = 0; ty < Height; ty++)
                {
                    if(autoTiles.isTile(tx, ty))
                    {
                        localMG.setCell(tx, ty, false);
                    }
                }
            }
        }
        /// <summary>
        /// Tiles property
        /// </summary>
        public XenoTileSys Tiles
        {
            get { return tiles; }
        }
        /// <summary>
        /// AutoTiles property
        /// </summary>
        public AutoTileSys AutoTiles
        {
            get { return autoTiles; }
        }
        /// <summary>
        /// Adds a series of chambers to cell
        /// </summary>
        /// <param name="depth">Depth of recursive calls</param>
        /// <param name="size">Size of room to build</param>
        /// <param name="dir">Direction of room entrence</param>
        /// <param name="x">X position of entrence</param>
        /// <param name="y">Y position of entrence</param>
        /// <param name="isStart">Flag value for starting chamber</param>
        public virtual void addChamber(int depth, int size, CHAMBER dir, int x, int y, bool isStart = false)
        {
            int numChambers;
            int nextSize;
            int haz = size / 2;
            switch (dir)
            {
                case CHAMBER.UP://going up
                    //add entrance way
                    autoTiles.removeTile(x, y);
                    autoTiles.removeTile(x - 1, y);
                    autoTiles.removeTile(x, y - 1);
                    autoTiles.removeTile(x - 1, y - 1);
                    //carve out actual chamber
                    for (int tx = x - haz; tx < x + (haz + 1); tx++)
                    {
                        for (int ty = y - (size + 1); ty < y; ty++)
                        {
                            autoTiles.removeTile(tx, ty);
                        }
                    }
                    if (depth > 0)
                    {
                        //try to add upto 4 chambers if space allows it
                        numChambers = rand.Next(1000, 4000) / 1000;
                        for (int i = 0; i < numChambers; i++)
                        {
                            switch ((CHAMBER)(int)(rand.Next(1000, 4000) / 1000))
                            {
                                case CHAMBER.UP:
                                    if (y > 20)
                                    {
                                        nextSize = ((rand.Next(0, 4000) / 1000) * 2) + 8;
                                        if (noChamber(x, y - (size + 4), nextSize ,CHAMBER.UP))
                                        {
                                            addChamber(depth - 1, nextSize, CHAMBER.UP, x, y - (size + 2));
                                        }
                                    }
                                    break;
                                case CHAMBER.RIGHT:
                                    if (x < tiles.Width - 20)
                                    {
                                        nextSize = ((rand.Next(0, 4000) / 1000) * 2) + 8;
                                        if (noChamber(x + haz, y - (haz + 2), nextSize, CHAMBER.RIGHT))
                                        {
                                            addChamber(depth - 1, nextSize, CHAMBER.RIGHT, x + haz, y - (haz + 2));
                                        }
                                    }
                                    break;
                                case CHAMBER.DOWN:
                                    break;
                                case CHAMBER.LEFT:
                                    if (x > 20)
                                    {
                                        nextSize = ((rand.Next(0, 4000) / 1000) * 2) + 8;
                                        if (noChamber(x - (haz + 1), y - (haz + 2), nextSize, CHAMBER.LEFT))
                                        {
                                            addChamber(depth - 1, nextSize, CHAMBER.LEFT, x - (haz + 1), y - (haz + 2));
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case CHAMBER.RIGHT://going right
                    //add entrance way
                    autoTiles.removeTile(x, y);
                    autoTiles.removeTile(x + 1, y);
                    autoTiles.removeTile(x, y + 1);
                    autoTiles.removeTile(x + 1, y + 1);
                    //carve out actual chamber
                    for (int tx = x + 2; tx < x + (size + 2); tx++)
                    {
                        for (int ty = y - (haz - 1); ty < y + (haz + 1); ty++)
                        {
                            autoTiles.removeTile(tx, ty);
                        }
                    }
                    if (depth > 0)
                    {
                        //try to add upto 4 chambers if space allows it
                        numChambers = rand.Next(1000, 4000) / 1000;
                        for (int i = 0; i < numChambers; i++)
                        {
                            switch ((CHAMBER)(int)(rand.Next(1000, 4000) / 1000))
                            {
                                case CHAMBER.UP:
                                    if (y > 20)
                                    {
                                        nextSize = ((rand.Next(0, 4000) / 1000) * 2) + 8;
                                        if (noChamber(x + (haz + 2), y - (haz + 2), nextSize, CHAMBER.UP))
                                        {
                                            addChamber(depth - 1, nextSize, CHAMBER.UP, x + (haz + 2), y - haz);
                                        }
                                    }
                                    break;
                                case CHAMBER.RIGHT:
                                    if (x < tiles.Width - 20)
                                    {
                                        nextSize = ((rand.Next(0, 4000) / 1000) * 2) + 8;
                                        if (noChamber(x + (size + 4), y, nextSize, CHAMBER.RIGHT))
                                        {
                                            addChamber(depth - 1, nextSize, CHAMBER.RIGHT, x + (size + 2), y);
                                        }
                                    }
                                    break;
                                case CHAMBER.DOWN:
                                    if (y < tiles.Height - 20)
                                    {
                                        nextSize = ((rand.Next(0, 4000) / 1000) * 2) + 8;
                                        if (noChamber(x + (haz + 2), y + (haz + 3), nextSize, CHAMBER.DOWN))
                                        {
                                            addChamber(depth - 1, nextSize, CHAMBER.DOWN, x + (haz + 2), y + haz);
                                        }
                                    }
                                    break;
                                case CHAMBER.LEFT:
                                    break;
                            }
                        }
                    }
                    break;
                case CHAMBER.DOWN://going down
                    //add entrance way
                    autoTiles.removeTile(x, y);
                    autoTiles.removeTile(x - 1, y);
                    autoTiles.removeTile(x, y + 1);
                    autoTiles.removeTile(x - 1, y + 1);
                    //carve out actual chamber
                    for (int tx = x - haz; tx < x + haz; tx++)
                    {
                        for (int ty = y + 2; ty < y + (size + 2); ty++)
                        {
                            autoTiles.removeTile(tx, ty);
                        }
                    }
                    if (depth > 0)
                    {
                        //try to add upto 4 chambers if space allows it
                        numChambers = rand.Next(1000, 4000) / 1000;
                        for (int i = 0; i < numChambers; i++)
                        {
                            switch ((CHAMBER)(int)(rand.Next(1000, 4000) / 1000))
                            {
                                case CHAMBER.UP:
                                    break;
                                case CHAMBER.RIGHT:
                                    if (x < tiles.Width - 20)
                                    {
                                        nextSize = ((rand.Next(0, 4000) / 1000) * 2) + 8;
                                        if (noChamber(x + (haz + 2), y + (haz + 1), nextSize, CHAMBER.RIGHT))
                                        {
                                            addChamber(depth - 1, nextSize, CHAMBER.RIGHT, x + haz, y + (haz + 1));
                                        }
                                    }
                                    break;
                                case CHAMBER.DOWN:
                                    if (y < tiles.Height - 20)
                                    {
                                        nextSize = ((rand.Next(0, 4000) / 1000) * 2) + 8;
                                        if (noChamber(x, y + (size + 4), nextSize, CHAMBER.DOWN))
                                        {
                                            addChamber(depth - 1, nextSize, CHAMBER.DOWN, x, y + (size + 2));
                                        }
                                    }
                                    break;
                                case CHAMBER.LEFT:
                                    if (x > 4)
                                    {
                                        nextSize = ((rand.Next(0, 4000) / 1000) * 2) + 8;
                                        if (noChamber(x - (haz + 3), y + (haz + 1), nextSize, CHAMBER.LEFT))
                                        {
                                            addChamber(depth - 1, nextSize, CHAMBER.LEFT, x - (haz + 1), y + (haz + 1));
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case CHAMBER.LEFT://going left
                    //add entrance way
                    autoTiles.removeTile(x, y);
                    autoTiles.removeTile(x - 1, y);
                    autoTiles.removeTile(x, y + 1);
                    autoTiles.removeTile(x - 1, y + 1);
                    //carve out actual chamber
                    for (int tx = x - 2; tx > x - (size + 2); tx--)
                    {
                        for (int ty = y - (haz - 2); ty < y + haz; ty++)
                        {
                            autoTiles.removeTile(tx, ty);
                        }
                    }
                    if (depth > 0)
                    {
                        //try to add upto 4 chambers if space allows it
                        numChambers = rand.Next(1000, 4000) / 1000;
                        for (int i = 0; i < numChambers; i++)
                        {
                            switch ((CHAMBER)(int)(rand.Next(1000, 4000) / 1000))
                            {
                                case CHAMBER.UP:
                                    if (y > size + 4)
                                    {
                                        nextSize = ((rand.Next(0, 4000) / 1000) * 2) + 8;
                                        if (noChamber(x - (haz + 1), y - (haz + 2), nextSize, CHAMBER.UP))
                                        {
                                            addChamber(depth - 1, nextSize, CHAMBER.UP, x - (haz + 1), y - haz);
                                        }
                                    }
                                    break;
                                case CHAMBER.RIGHT:
                                    break;
                                case CHAMBER.DOWN:
                                    if (y < tiles.Height - (size + 4))
                                    {
                                        nextSize = ((rand.Next(0, 4000) / 1000) * 2) + 8;
                                        if (noChamber(x - (haz + 1), y + (haz + 3), nextSize, CHAMBER.DOWN))
                                        {
                                            addChamber(depth - 1, nextSize, CHAMBER.DOWN, x - (haz + 1), y + haz);
                                        }
                                    }
                                    break;
                                case CHAMBER.LEFT:
                                    if (x > size + 4)
                                    {
                                        nextSize = ((rand.Next(0, 4000) / 1000) * 2) + 8;
                                        if (checkChamberSpace(x - (size + 4), y, CHAMBER.LEFT, CHAMBER.LEFT, nextSize))
                                        {
                                            addChamber(depth - 1, nextSize, CHAMBER.LEFT, x - (size + 2), y);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Checks if a desired space has no chamber already there
        /// </summary>
        /// <param name="x">X position of entry from chamber</param>
        /// <param name="y">Y position of entry from chamber</param>
        /// <param name="dirFrom">Direction of entry from chamber</param>
        /// <param name="dir">Desired direction of chamber</param>
        /// <param name="size">desired size of chamber</param>
        /// <returns>Boolean</returns>
        public bool checkChamberSpace(int x, int y, CHAMBER dirFrom, CHAMBER dir, int size)
        {
            int haz = size / 2;
            switch (dirFrom)
            {
                case CHAMBER.UP://from up
                    switch (dir)
                    {
                        case CHAMBER.UP:
                            if (noChamber(x, y - (size + 4), size, dirFrom))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        case CHAMBER.RIGHT:
                            if (noChamber(x + haz, y - (haz + 2) , size, dirFrom))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        case CHAMBER.DOWN:
                            return false;
                        case CHAMBER.LEFT:
                            if (noChamber(x - (haz + 1), y - (haz + 2), size, dirFrom))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                    }
                    break;
                case CHAMBER.RIGHT://from right
                    switch (dir)
                    {
                        case CHAMBER.UP:
                            if (noChamber(x + (haz + 2), y - haz, size, dirFrom))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        case CHAMBER.RIGHT:
                            if (noChamber(x + (size + 4), y, size, dirFrom))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        case CHAMBER.DOWN:
                            if (noChamber(x + (haz + 2), y + (haz + 1), size, dirFrom))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        case CHAMBER.LEFT:
                            return false;
                    }
                    break;
                case CHAMBER.DOWN://from down
                    switch (dir)
                    {
                        case CHAMBER.UP:
                            return false;
                        case CHAMBER.RIGHT:
                            if (noChamber(x + haz, y + (haz + 1), size, dirFrom))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        case CHAMBER.DOWN:
                            if (noChamber(x, y + (size + 4), size, dirFrom))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        case CHAMBER.LEFT:
                            if (noChamber(x - (haz + 1), y + (haz + 1), size, dirFrom))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                    }
                    break;
                case CHAMBER.LEFT://from left
                    switch (dir)
                    {
                        case CHAMBER.UP:
                            if (noChamber(x - (haz + 1), y - haz, size, dirFrom))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        case CHAMBER.RIGHT:
                            return false;
                            
                        case CHAMBER.DOWN:
                            if (noChamber(x + (haz + 1), y + (haz + 1), size, dirFrom))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        case CHAMBER.LEFT:
                            if (noChamber(x + (size + 4), y, size, dirFrom))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                    }
                    break;
            }
            return false;//no chamber found
        }
        /// <summary>
        /// Checks if there are any open spaces in target area meaning a chamber exists there
        /// Checks from top center of chamber position
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="size">Size of chamber to check for</param>
        /// <returns>Booleans</returns>
        public bool noChamber(int x, int y, int size, CHAMBER going)
        {
            int haz = size / 2;
            switch (going)
            {
                case CHAMBER.UP:
                    for (int tx = x - (haz + 1); tx < x + haz; tx++)
                    {
                        for (int ty = y - (size + 2); ty < y - 1; ty++)
                        {
                            if (autoTiles.inDomain(tx, ty))
                            {
                                if (!autoTiles.isTile(tx, ty))
                                {
                                    return false;//there is an open space so there is a chamber overlap
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    break;
                case CHAMBER.RIGHT:
                    for (int tx = x + 1; tx < x + (size + 2); tx++)
                    {
                        for (int ty = y - haz; ty < y + (haz + 1); ty++)
                        {
                            if (autoTiles.inDomain(tx, ty))
                            {
                                if (!autoTiles.isTile(tx, ty))
                                {
                                    return false;//there is an open space so there is a chamber overlap
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    break;
                case CHAMBER.DOWN:
                    for (int tx = x - (haz + 1); tx < x + haz; tx++)
                    {
                        for (int ty = y + 1; ty < y + (size + 2); ty++)
                        {
                            if (autoTiles.inDomain(tx, ty))
                            {
                                if (!autoTiles.isTile(tx, ty))
                                {
                                    return false;//there is an open space so there is a chamber overlap
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    break;
                case CHAMBER.LEFT:
                    for (int tx = x - 1; tx > x - (size + 4); tx--)
                    {
                        for (int ty = y - haz; ty < y + (haz + 1); ty++)
                        {
                            if (autoTiles.inDomain(tx, ty))
                            {
                                if (!autoTiles.isTile(tx, ty))
                                {
                                    return false;//there is an open space so there is a chamber overlap
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    break;
            }
            return true;
        }
        /// <summary>
        /// On 1 in 4 calls add a door at entry way into chamber
        /// </summary>
        /// <param name="x">X position of entry way</param>
        /// <param name="y">Y position of entry way</param>
        /// <param name="dir">Direction of facing</param>
        public virtual void addDoor(int x, int y, CHAMBER dir)
        {
            XenoBuilding temp = null;
            if(rand.Next(0, 400) < 100)
            {
                switch(dir)
                {
                    case CHAMBER.UP:
                        temp = new XenoDoor(TextureBank.getTexture("door"), (x - 1) * tiles.TileWidth, (y + 1) * tiles.TileHeight,
                            2 * tiles.TileWidth, 2 * tiles.TileHeight, 30, tiles.TileWidth, tiles.TileHeight,
                            rand.Next(300, 1000) / 10);
                        temp.Direct = DIRECT.UP;
                        buildings.Add(temp);
                        break;
                    case CHAMBER.RIGHT:
                        temp = new XenoDoor(TextureBank.getTexture("door"), (x - 1) * tiles.TileWidth, y * tiles.TileHeight,
                            2 * tiles.TileWidth, 2 * tiles.TileHeight, 30, tiles.TileWidth, tiles.TileHeight,
                            rand.Next(300, 1000) / 10);
                        temp.Direct = DIRECT.RIGHT;
                        buildings.Add(temp);
                        break;
                    case CHAMBER.DOWN:
                        temp = new XenoDoor(TextureBank.getTexture("door"), (x - 1) * tiles.TileWidth, (y - 1) * tiles.TileHeight,
                            2 * tiles.TileWidth, 2 * tiles.TileHeight, 30, tiles.TileWidth, tiles.TileHeight,
                            rand.Next(300, 1000) / 10);
                        temp.Direct = DIRECT.DOWN;
                        buildings.Add(temp);
                        break;
                    case CHAMBER.LEFT:
                        temp = new XenoDoor(TextureBank.getTexture("door"), (x + 1) * tiles.TileWidth, y * tiles.TileHeight,
                            2 * tiles.TileWidth, 2 * tiles.TileHeight, 30, tiles.TileWidth, tiles.TileHeight,
                            rand.Next(300, 1000) / 10);
                        temp.Direct = DIRECT.LEFT;
                        buildings.Add(temp);
                        break;
                }
            }
        }
        /// <summary>
        /// Adds a random number of XenoDoodads to chamber based on size of chamber
        /// </summary>
        /// <param name="size">Size of chamber</param>
        /// <param name="x">X position of entry way</param>
        /// <param name="y">Y position of entry way</param>
        /// <param name="dir">Direction of entry into chamber</param>
        public virtual void addDoodads(int size, int x, int y, CHAMBER dir)
        {
            int tx = 0;
            int ty = 0;
            DIRECT face = 0;
            XenoDoodad temp = null;
            int choice = 0;
            bool found = false;
            int haz = size / 2;

            int num = rand.Next((size / 4) * 1000, (size / 2) * 1000) / 1000;
            for (int i = 0; i < num; i++)
            {
                switch (dir)
                {
                    case CHAMBER.UP:
                        if (rand.Next(1, 100) > 50)//under on right side and over on left side
                        {
                            tx = x - haz;
                            face = DIRECT.LEFT;
                        }
                        else
                        {
                            tx = x + (haz);
                            face = DIRECT.RIGHT;
                        }
                        if (rand.Next(1, 100) > 50)
                        {
                            ty = y - (rand.Next(1000, haz * 1000) / 1000);
                        }
                        else
                        {
                            ty = y - (haz + 2) + (rand.Next(1000, haz * 1000) / 1000);
                        }
                        break;
                    case CHAMBER.RIGHT:
                        if (rand.Next(1, 100) > 50)//under on right side and over on left side
                        {
                            ty = y - (haz - 1);
                            face = DIRECT.UP;
                        }
                        else
                        {
                            ty = y + (haz);
                            face = DIRECT.DOWN;
                        }
                        if (rand.Next(1, 100) > 50)
                        {
                            tx = x + (rand.Next(1000, haz * 1000) / 1000);
                        }
                        else
                        {
                            tx = x + (haz + 2) + (rand.Next(1000, haz * 1000) / 1000);
                        }
                        break;
                    case CHAMBER.DOWN:
                        if (rand.Next(1, 100) > 50)//under on right side and over on left side
                        {
                            tx = x - haz;
                            face = DIRECT.LEFT;
                        }
                        else
                        {
                            tx = x + (haz - 1);
                            face = DIRECT.RIGHT;
                        }
                        if (rand.Next(1, 100) > 50)
                        {
                            ty = y + 1 + (rand.Next(1000, haz * 1000) / 1000);
                        }
                        else
                        {
                            ty = y + (haz + 2) + (rand.Next(1000, haz * 1000) / 1000);
                        }
                        break;
                    case CHAMBER.LEFT:
                        if (rand.Next(1, 100) > 50)//under on right side and over on left side
                        {
                            ty = y - (haz - 2);
                            face = DIRECT.UP;
                        }
                        else
                        {
                            ty = y + (haz + 2);
                            face = DIRECT.DOWN;
                        }
                        if (rand.Next(1, 100) > 50)
                        {
                            tx = x - (rand.Next(1000, haz * 1000) / 1000);
                        }
                        else
                        {
                            tx = x - (haz + 2) + (rand.Next(1000, haz * 1000) / 1000);
                        }
                        break;
                }
                for (int k = 0; k < doodads.Count; k++)
                {
                    if (doodads[k].X / tiles.TileWidth == tx && doodads[k].Y / tiles.TileHeight == ty)
                    {
                        found = true;
                        break;
                    }
                }
                //make sure position is against a wall
                switch (face)
                {
                    case DIRECT.UP:
                        if (isAutoTile(tx, ty))
                        {
                            found = true;
                        }
                        break;
                    case DIRECT.RIGHT:
                        if (isAutoTile(tx - 1, ty))
                        {
                            found = true;
                        }
                        break;
                    case DIRECT.DOWN:
                        if (isAutoTile(tx, ty))
                        {
                            found = true;
                        }
                        break;
                    case DIRECT.LEFT:
                        if (isAutoTile(tx + 1, ty))
                        {
                            found = true;
                        }
                        break;
                }
                //make sure not over an autotile
                if (isAutoTile(tx, ty))
                {
                    found = true;
                }
                if (!found)
                {
                    choice = rand.Next(1000, 3500) / 1000;
                    switch (choice)
                    {
                        case 1:
                            temp = new XenoDoodad(TextureBank.getTexture("doodad 1"), "doodad 1", tx * tiles.TileWidth, ty * tiles.TileHeight, tiles.TileWidth, tiles.TileHeight, 4);
                            temp.Direct = face;
                            doodads.Add(temp);
                            break;
                        case 2:
                            temp = new XenoDoodad(TextureBank.getTexture("doodad 2"), "doodad 2", tx * tiles.TileWidth, ty * tiles.TileHeight, tiles.TileWidth, tiles.TileHeight, 4);
                            temp.Direct = face;
                            doodads.Add(temp);
                            break;
                        case 3:
                            temp = new XenoDoodad(TextureBank.getTexture("doodad 3"), "doodad 3", tx * tiles.TileWidth, ty * tiles.TileHeight, tiles.TileWidth, tiles.TileHeight, 4);
                            temp.Direct = face;
                            doodads.Add(temp);
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Returns a list of valid points in radius for x and y positions
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="radius">Radius around x, y position</param>
        /// <param name="curve">How shallow or sharp the curve is</param>
        /// <returns>List of Point2D objects</returns>
        public List<Point2D> getArea(int x, int y, int radius, int curve = 1)
        {
            List<Point2D> temp = new List<Point2D>();
            if (radius < 3)
            {
                radius = 3;
            }
            int tempW = curve + 1;
            int txs = x - curve;
            for (int ty = y - radius; ty < y + (radius * 2) + 1; ty++)
            {
                for (int tx = txs; tx < txs + tempW; tx++)
                {
                    if (tiles.inDomain(tx, ty))
                    {
                        temp.Add(new Point2D(tx, ty));
                    }
                }
                if (ty < y - 1)
                {
                    txs -= curve;
                    tempW += (2 * curve);
                }
                else if (ty > y + 1)
                {
                    txs += curve;
                    tempW -= (2 * curve);
                }
            }
            return temp;
        }
        /// <summary>
        /// Returns a list of Point2D objects representing a ring of valid positions
        /// Returns null if outer radius is less than or equal to the inner radius
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="radius">Outer radius</param>
        /// <param name="innerRadius">Inner radius</param>
        /// <param name="curve">How shallow or sharp the curve is</param>
        /// <returns>List of Point2D objects</returns>
        public List<Point2D> getRingedArea(int x, int y, int radius, int innerRadius, int curve = 1)
        {
            //return null if outer radius is less than or equal the inner radius
            if (radius <= innerRadius)
            {
                return null;
            }
            //get the area of two circles remove the Point2D objects that overlap 
            List<Point2D> temp = getArea(x, y, radius, curve);
            List<Point2D> temp2 = getArea(x, y, innerRadius, curve);
            for (int r = 0; r < temp.Count; r++)
            {
                for (int ir = 0; ir < temp2.Count; ir++)
                {
                    if(temp[r].X == temp2[ir].X && temp[r].Y == temp2[ir].Y)
                    {
                        temp.RemoveAt(r);
                        break;
                    }
                }
            }
            return temp;
        }
        /// <summary>
        /// Spreads out evenly in all clear directions creating a list of points
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="magnitude">Magnitude (value * (8^2))</param>
        /// <returns>List of Point2D objects</returns>
        public List<Point2D> getRadius(float x, float y, int magnitude)
        {
            List<Point2D> open = new List<Point2D>();
            List<Point2D> closed = new List<Point2D>();
            Point2D tmp = new Point2D(x, y);
            int depth = magnitude * (int)Math.Pow(8, 2);
            while(open.Count > 0 && depth > 0)
            {
                addPointsToOpen(tmp.X, tmp.Y, open, closed);
                closed.Add(tmp);
                tmp = open[0];
                open.RemoveAt(0);
                depth--;
            }
            return closed;
        }
        /// <summary>
        /// Aids getRadius in filling out the open list
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="open">Open list reference</param>
        /// <param name="closed">Closed list reference</param>
        protected void addPointsToOpen(float x, float y, List<Point2D> open, List<Point2D> closed)
        {
            int tx = (int)(x / tiles.TileWidth);
            int ty = (int)(y / tiles.TileHeight);
            //add points in 8 directions, checking the open and closed 
            //lists for duplicates
            //Add spots clockwise
            //up
            Point2D tmp = new Point2D(tx, ty - 1);
            bool found = false;
            for(int i = 0; i < open.Count; i++)
            {
                if(tmp.X == open[i].X && tmp.Y == open[i].Y)
                {
                    found = true;
                    break;
                }
            }
            if(found == false)
            {
                for (int i = 0; i < closed.Count; i++)
                {
                    if (tmp.X == closed[i].X && tmp.Y == closed[i].Y)
                    {
                        found = true;
                        break;
                    }
                }
            }
            if(found == false && tiles.inDomain(tx, ty) == true)
            {
                closed.Add(new Point2D(tmp.X, tmp.Y));
            }
            //up right
            tmp = new Point2D(tx + 1, ty - 1);
            found = false;
            for (int i = 0; i < open.Count; i++)
            {
                if (tmp.X == open[i].X && tmp.Y == open[i].Y)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                for (int i = 0; i < closed.Count; i++)
                {
                    if (tmp.X == closed[i].X && tmp.Y == closed[i].Y)
                    {
                        found = true;
                        break;
                    }
                }
            }
            if (found == false && tiles.inDomain(tx, ty) == true)
            {
                closed.Add(new Point2D(tmp.X, tmp.Y));
            }
            //right
            tmp = new Point2D(tx + 1, ty);
            found = false;
            for (int i = 0; i < open.Count; i++)
            {
                if (tmp.X == open[i].X && tmp.Y == open[i].Y)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                for (int i = 0; i < closed.Count; i++)
                {
                    if (tmp.X == closed[i].X && tmp.Y == closed[i].Y)
                    {
                        found = true;
                        break;
                    }
                }
            }
            if (found == false && tiles.inDomain(tx, ty) == true)
            {
                closed.Add(new Point2D(tmp.X, tmp.Y));
            }
            //down right
            tmp = new Point2D(tx + 1, ty + 1);
            found = false;
            for (int i = 0; i < open.Count; i++)
            {
                if (tmp.X == open[i].X && tmp.Y == open[i].Y)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                for (int i = 0; i < closed.Count; i++)
                {
                    if (tmp.X == closed[i].X && tmp.Y == closed[i].Y)
                    {
                        found = true;
                        break;
                    }
                }
            }
            if (found == false && tiles.inDomain(tx, ty) == true)
            {
                closed.Add(new Point2D(tmp.X, tmp.Y));
            }
            //down
            tmp = new Point2D(tx, ty + 1);
            found = false;
            for (int i = 0; i < open.Count; i++)
            {
                if (tmp.X == open[i].X && tmp.Y == open[i].Y)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                for (int i = 0; i < closed.Count; i++)
                {
                    if (tmp.X == closed[i].X && tmp.Y == closed[i].Y)
                    {
                        found = true;
                        break;
                    }
                }
            }
            if (found == false && tiles.inDomain(tx, ty) == true)
            {
                closed.Add(new Point2D(tmp.X, tmp.Y));
            }
            //down left
            tmp = new Point2D(tx - 1, ty + 1);
            found = false;
            for (int i = 0; i < open.Count; i++)
            {
                if (tmp.X == open[i].X && tmp.Y == open[i].Y)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                for (int i = 0; i < closed.Count; i++)
                {
                    if (tmp.X == closed[i].X && tmp.Y == closed[i].Y)
                    {
                        found = true;
                        break;
                    }
                }
            }
            if (found == false && tiles.inDomain(tx, ty) == true)
            {
                closed.Add(new Point2D(tmp.X, tmp.Y));
            }
            //left
            tmp = new Point2D(tx - 1, ty);
            found = false;
            for (int i = 0; i < open.Count; i++)
            {
                if (tmp.X == open[i].X && tmp.Y == open[i].Y)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                for (int i = 0; i < closed.Count; i++)
                {
                    if (tmp.X == closed[i].X && tmp.Y == closed[i].Y)
                    {
                        found = true;
                        break;
                    }
                }
            }
            if (found == false && tiles.inDomain(tx, ty) == true)
            {
                closed.Add(new Point2D(tmp.X, tmp.Y));
            }
            //up left
            tmp = new Point2D(tx - 1, ty - 1);
            found = false;
            for (int i = 0; i < open.Count; i++)
            {
                if (tmp.X == open[i].X && tmp.Y == open[i].Y)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                for (int i = 0; i < closed.Count; i++)
                {
                    if (tmp.X == closed[i].X && tmp.Y == closed[i].Y)
                    {
                        found = true;
                        break;
                    }
                }
            }
            if (found == false && tiles.inDomain(tx, ty) == true)
            {
                closed.Add(new Point2D(tmp.X, tmp.Y));
            }
        }
        /// <summary>
        /// Gets a list of points generating a circle centered around a point
        /// </summary>
        /// <param name="center">Center of circle</param>
        /// <param name="radius">Radius of circle</param>
        /// <returns>List of Point2D objects</returns>
        public List<Point2D> getCircleArea(Point2D center, float radius)
        {
            List<Point2D> points = new List<Point2D>();
            int width = (int)(radius * 2) + 1;
            int height = (int)(radius * 2) + 1;
            Point2D p = null;
            for(int x = (int)(center.X - radius); x < width; x++)
            {
                for (int y = (int)(center.Y - radius); y < height; y++)
                {
                    if (tiles.inDomain(x, y) == true)
                    {
                        p = new Point2D(x, y);
                        if (insideCircle(center, p, radius) == true)
                        {
                            points.Add(p);
                        }
                    }
                }
            }
            return points;
        }
        /// <summary>
        /// Aids getCircleArea
        /// </summary>
        /// <param name="center">Center of circle</param>
        /// <param name="p">Point to be checked</param>
        /// <param name="radius">Radius of circle</param>
        /// <returns>Boolean</returns>
        protected bool insideCircle(Point2D center, Point2D p, float radius)
        {
            float dx = center.X - p.X;
            float dy = center.Y - p.Y;
            float dist = (dx * dx) + (dy * dy);
            if(dist <= radius * radius)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Intersects avatar with portals
        /// </summary>
        /// <param name="avatar">XenoSprite reference</param>
        /// <param name="world">OpenWorld reference</param>
        public void intersectPortal(XenoSprite avatar, OpenWorld world)
        {
            Rectangle tempBox = new Rectangle(avatar.HitBox.X - (cellx * Tiles.Width * tiles.TileWidth), avatar.HitBox.Y - (celly * Tiles.Height * tiles.TileHeight), avatar.HitBox.Width, avatar.HitBox.Height);
            for (int p = 0; p < portals.Count; p++)
            {
                if (portals[p].HitBox.intersects(tempBox))
                {
                    world.enterExit(portals[p].CellName, world.SaveFolderPath, portals[p].CX, portals[p].CY, portals[p].TX, portals[p].TY, portals[p].ToInterior);
                }
            }
        }
        /// <summary>
        /// Removes autotiles to create a straight tunnel, provided a start and end in tiles
        /// </summary>
        /// <param name="x1">Start x in tiles value</param>
        /// <param name="y1">Start y in tiles value</param>
        /// <param name="x2">End x in tiles value</param>
        /// <param name="y2">End x in tiles value</param>
        /// <param name="radius">Radius in tiles value</param>
        public void addTunnel(int x1, int y1, int x2, int y2, int radius)
        {
            int range = Point2D.calculateDistance(new Point2D(x1, y1), new Point2D(x2, y2)) / 2;
            int dist1 = range;
            int dist2 = 0;
            double angle = Point2D.CalcAngle(new Point2D(x1, y1), new Point2D(x2, y2));
            List<Point2D> area = getArea(x1, y1, radius);
            int tx = x1;
            int ty = y1;
            for(int r = 0; r < range; r++)
            {
                for(int a = 0; a < area.Count; a++)
                {
                    autoTiles.removeTile(area[a].IX, area[a].IY);
                }
                tx = (int)((Math.Cos(Helpers<int>.degreesToRadians(angle))) * radius) + tx;
                ty = (int)((Math.Sin(Helpers<int>.degreesToRadians(angle))) * radius) + ty;
                area = getArea(tx, ty, radius);
                dist2 = Point2D.calculateDistance(new Point2D(tx, ty), new Point2D(x2, y2)) / 2;
                if (dist1 < dist2)
                {
                    break;
                }
                dist1 = dist2;
            }
        }
        /// <summary>
        /// Randomly sets all layer 1 tiles to a tile from the tile source
        /// </summary>
        public void generateRandomFlooring()
        {
            for(int tx = 0; tx < tiles.Width; tx++)
            {
                for (int ty = 0; ty < tiles.Height; ty++)
                {
                    int sx, sy;
                    sx = (rand.Next(0, 500) / 100) * 32;
                    sy = (rand.Next(0, 700) / 100) * 32;
                    tiles.setTile(1, tx, ty, sx, sy);
                }
            }
        }
        /// <summary>
        /// Sets all autotiles in cell to a tile
        /// </summary>
        public void fillAllAutoTiles()
        {
            for (int tx = 0; tx < tiles.Width; tx++)
            {
                for (int ty = 0; ty < tiles.Height; ty++)
                {
                    if(autoTiles.isTile(tx, ty) == false)
                    {
                        autoTiles.addTile(tx, ty);
                    }
                }
            }
                }
        /// <summary>
        /// Removes doodads from Alpha cell in 3 radius sizes ("small", "medium", "large")
        /// </summary>
        /// <param name="world">OpenWorld reference</param>
        /// <param name="x">X position in tiles</param>
        /// <param name="y">Y position in tiles</param>
        /// <param name="size">Size of radius ("small", "medium", "large")</param>
        public void clearDoodadRadius(int x, int y, string size)
        {
            List<Point2D> points = null;
            Point2D center = new Point2D(x, y);
            switch (size)
            {
                case "small":
                    points = getCircleArea(center, 7);
                    break;
                case "medium":
                    points = getCircleArea(center, 10);
                    break;
                case "large":
                    points = getCircleArea(center, 13);
                    break;
            }
            for (int i = 0; i < points.Count - 1; i++)
            {
                for (int k = 0; k < doodads.Count - 1; k++)
                {
                    if (doodads[k].IX == points[i].IX &&
                        doodads[k].IY == points[i].IY)
                    {
                        doodads.RemoveAt(k);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Gets a list of points generating a circle centered around a point
        /// </summary>
        /// <param name="center">Center of circle</param>
        /// <param name="radius">Radius of circle</param>
        /// <returns>List of Point2D objects</returns>
        public List<Point2D> getCircleArea<T>(Point2D center, float radius)
        {
            List<Point2D> points = new List<Point2D>();
            int width = (int)(radius * 2) + 1;
            int height = (int)(radius * 2) + 1;
            Point2D p = null;
            for (int x = (int)(center.X - radius); x < (int)(center.X - radius - 1) + width; x++)
            {
                for (int y = (int)(center.Y - radius); y < (int)(center.Y - radius - 1) + height; y++)
                {
                    if (Helpers<int>.inDomain(x, y, tiles.Width, tiles.Height) == true)
                    {
                        p = new Point2D(x, y);
                        if (insideCir(center, p, radius) == true)
                        {
                            points.Add(p);
                        }
                    }
                }
            }
            //remove furthest left
            int tmp = 0;
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].X < points[tmp].X)
                {
                    tmp = i;
                }
            }
            points.RemoveAt(tmp);
            //remove furthest up
            tmp = 0;
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].Y < points[tmp].Y)
                {
                    tmp = i;
                }
            }
            points.RemoveAt(tmp);
            return points;
        }
        /// <summary>
        /// Aids getCircleArea
        /// </summary>
        /// <param name="center">Center of circle</param>
        /// <param name="p">Point to be checked</param>
        /// <param name="radius">Radius of circle</param>
        /// <returns>Boolean</returns>
        protected bool insideCir(Point2D center, Point2D p, float radius)
        {
            float dx = center.X - p.X;
            float dy = center.Y - p.Y;
            float dist = (dx * dx) + (dy * dy);
            if (dist <= radius * radius)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns true if a space with a one tile radius is clear
        /// </summary>
        /// <param name="x">X position in tiles</param>
        /// <param name="y">Y position in tiles</param>
        /// <param name="w">Width in tiles</param>
        /// <param name="h">Height in tiles</param>
        /// <returns>Boolean</returns>
        public bool spaceClear(int x, int y, int w, int h)
        {
            int w2 = w + 2;
            int h2 = h + 2;
            int x2 = x - (w / 2) - 1;
            int y2 = y - (h / 2) - 1;
            for(int xx = x2; xx < x2 + w2; xx++)
            {
                for(int yy = y2; yy < y2 + h2; yy++)
                {
                    if(localMG.getCell(xx, yy) == true)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Cellx property
        /// </summary>
        public int CellX
        {
            get { return cellx; }
        }
        /// <summary>
        /// Celly property
        /// </summary>
        public int CellY
        {
            get { return celly; }
        }
        /// <summary>
        /// Worldx property
        /// </summary>
        public int WorldX
        {
            get { return worldx; }
        }
        /// <summary>
        /// Worldy property
        /// </summary>
        public int WorldY
        {
            get { return worldy; }
        }
        /// <summary>
        /// WinWidth property
        /// </summary>
        public int WinWidth
        {
            get { return Tiles.WinWidth; }
        }
        /// <summary>
        /// WinHeight property
        /// </summary>
        public int WinHeight
        {
            get { return Tiles.WinHeight; }
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
        /// IsInteriorCell property
        /// </summary>
        public bool IsInteriorCell
        {
            get { return isInteriorCell; }
            set { isInteriorCell = value; }
        }
        /// <summary>
        /// Portals property
        /// </summary>
        public List<XenoPortal> Portals
        {
            get { return portals; }
        }
        /// <summary>
        /// Buildings property
        /// </summary>
        public List<XenoBuilding> Buildings
        {
            get { return buildings; }
        }
        /// <summary>
        /// CellLeftSide property
        /// </summary>
        public int CellLeftSide
        {
            get { return cellLeftSide; }
        }
        /// <summary>
        /// CellTopSide property
        /// </summary>
        public int CellTopSide
        {
            get { return cellTopSide; }
        }
        /// <summary>
        /// CellRightSide property
        /// </summary>
        public int CellRightSide
        {
            get { return cellRightSide; }
        }
        /// <summary>
        /// CellBottomSide property
        /// </summary>
        public int CellBottomSide
        {
            get { return cellBottomSide; }
        }
        /// <summary>
        /// SG property
        /// </summary>
        public SectorGraph64 SG
        {
            get { return sg; }
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
        /// Spawnable property
        /// </summary>
        public bool Spawnable
        {
            get { return spawnable; }
            set { spawnable = value; }
        }
        /// <summary>
        /// Source property
        /// </summary>
        public Texture2D Source
        {
            get { return source; }
            set { source = value; }
        }
        /// <summary>
        /// AutoSource property
        /// </summary>
        public Texture2D AutoSource
        {
            get { return autoSource; }
            set { autoSource = value; }
        }
        /// <summary>
        /// InterSource property
        /// </summary>
        public Texture2D InterSource
        {
            get { return interSource; }
            set { interSource = value; }
        }
        /// <summary>
        /// InterAutoSource property
        /// </summary>
        public Texture2D InterAutoSource
        {
            get { return interAutoSource; }
            set { interAutoSource = value; }
        }
        /// <summary>
        /// LocalMG property
        /// </summary>
        public MapGraph LocalMG
        {
            get { return localMG; }
        }
        /// <summary>
        /// Doodads property
        /// </summary>
        public List<XenoDoodad> Doodads
        {
            get { return doodads; }
        }
    }
    /// <summary>
    /// OpenWorld class
    /// </summary>
    public class OpenWorld
    {
        //protected
        protected OpenWorldCell alpha;
        protected OpenWorldCell beta;
        protected OpenWorldCell delta;
        protected OpenWorldCell gamma;
        protected OpenWorldCell tempA;
        protected OpenWorldCell tempB;
        protected OpenWorldCell tempD;
        protected OpenWorldCell tempG;
        protected MapGraph mg;
        protected SimplePathFinder spf;
        protected List<XenoSprite> creatures;
        protected List<XenoSprite> npcs;
        protected XenoSprite avatar;
        protected Texture2D source;
        protected Texture2D autoSource;
        protected Texture2D interSource;
        protected Texture2D interAutoSource;
        protected int winx;//window's world x position in pixels
        protected int winy;//window's world y position in pixels
        protected int modx;
        protected int mody;
        protected int currentQuadrent;
        protected int cellWidth;
        protected int cellHeight;
        protected int tileWidth;
        protected int tileHeight;
        protected int winWidth;
        protected int winHeight;
        protected int worldWidth;//world's width in cells
        protected int worldHeight;//world's height in cells
        protected int worldLeftSide; 
        protected int worldTopSide;
        protected int worldRightSide;
        protected int worldBottomSide;
        protected CoolDown inputDelay;
        protected Random rand;
        protected Counter delay1;
        protected Counter delay2;
        protected Counter delay3;
        protected Counter delay4;
        protected Counter delay5;
        protected Counter delay6;
        protected int mgLeft;
        protected int mgTop;
        protected int shiftRight;
        protected int shiftDown;
        protected string saveFolderPath;
        protected string planet;
        protected bool isPlanet;
        //interior variables \/\/\/
        protected int interX;
        protected int interY;
        protected int interWinX;
        protected int interWinY;
        protected bool interior;
        //interior variables /\/\/\

        //public
        /// <summary>
        /// OpenWorld constructor
        /// </summary>
        /// <param name="source">Tile source Texture2D reference</param>
        /// <param name="autoSource">AutoTile source Texture2D reference</param>
        /// <param name="cellWidth">Cell width in tiles</param>
        /// <param name="cellHeight">Cell height in tiles</param>
        /// <param name="winx">Window X position in pixels</param>
        /// <param name="winy">Window Y position in pixels</param>
        /// <param name="winWidth">Window width in tiles</param>
        /// <param name="winHeight">Window height in tiles</param>
        /// <param name="tileWidth">Tile width in pixels</param>
        /// <param name="tileHeight">Tile height in pixels</param>
        /// <param name="worldWidth">World width in cells</param>
        /// <param name="worldHeight">World height in cells</param>
        /// <param name="inputDelay">Input delay value</param>
        /// <param name="planet">Planet name</param>
        /// <param name="isPlanet">IsPlanet flag value</param>
        public OpenWorld(Texture2D source, Texture2D autoSource, int cellWidth, int cellHeight, int winx, int winy, 
            int winWidth, int winHeight, int tileWidth, int tileHeight, int worldWidth, int worldHeight, int inputDelay = 1, 
            string planet = "", bool isPlanet = false)
        {
            alpha = null;
            beta = null;
            delta = null;
            gamma = null;
            mg = new MapGraph(cellWidth * 2, cellHeight * 2);//4 cells worth of space
            spf = new SimplePathFinder(tileWidth, 1000, mg, 1);//may need updating parameters
            creatures = new List<XenoSprite>();
            npcs = new List<XenoSprite>();
            this.source = source;
            this.autoSource = autoSource;
            avatar = null;
            this.winx = winx;
            this.winy = winy;
            currentQuadrent = 1;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.winWidth = winWidth;
            this.winHeight = winHeight;
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            worldLeftSide = 0;
            worldTopSide = 0;
            worldRightSide = worldWidth * (cellWidth * tileWidth);
            worldBottomSide = worldHeight * (cellHeight * tileHeight);
            this.inputDelay = new CoolDown(inputDelay);
            interX = 0;
            interY = 0;
            interWinX = 0;
            interWinY = 0;
            interior = false;
            rand = new Random((int)System.DateTime.Today.Ticks);
            delay1 = new Counter(17);
            delay2 = new Counter(19);//bullet collsion checks
            delay3 = new Counter(37);//mapGraph updates
            delay4 = new Counter(47);
            delay5 = new Counter(97);
            delay6 = new Counter(137);
            mgLeft = 0;
            mgTop = 0;
            shiftRight = 0;
            shiftDown = 0;
            saveFolderPath = "";
            interSource = source;
            interAutoSource = autoSource;
            this.planet = planet;
            this.isPlanet = isPlanet;
        }
        /// <summary>
        /// Draws Openworld
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="shiftx">Shift x value</param>
        /// <param name="shifty">Shift y value</param>
        public void draw(IntPtr renderer, int shiftx, int shifty)
        {
            if(alpha != null)
            {
                alpha.draw(renderer, winx, winy, shiftx, shifty);// - (winx % (tileWidth - 1)), winy - (winy % (tileHeight - 1)), shiftx, shifty);
            }
            if (beta != null)
            {
                beta.draw(renderer, winx, winy, shiftx, shifty);// - (winx % (tileWidth - 1)), winy - (winy % (tileHeight - 1)), shiftx, shifty);
            }
            if (delta != null)
            {
                delta.draw(renderer, winx, winy, shiftx, shifty);// - (winx % (tileWidth - 1)), winy - (winy % (tileHeight - 1)), shiftx, shifty);
            }
            if (gamma != null)
            {
                gamma.draw(renderer, winx, winy, shiftx, shifty);// - (winx % (tileWidth - 1)), winy - (winy % (tileHeight - 1)), shiftx, shifty);
            }
        }
        /// <summary>
        /// Draws only one cell and is used for interior cells
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="shiftx">Shift x value</param>
        /// <param name="shifty">Shift y value</param>
        public void drawInterior(IntPtr renderer, int shiftx, int shifty)
        {
            if (alpha != null)
            {
                alpha.draw(renderer, interWinX, interWinY, shiftx, shifty);
            }
        }
        /// <summary>
        /// Draws the currrent mapGraph
        /// </summary>
        /// <param name="renderer">Renderer referecne</param>
        public void drawMG(IntPtr renderer)
        {
            SDL.SDL_Color red;
            red.r = 255;
            red.g = 0;
            red.b = 0;
            red.a = 156;
            Rectangle rect = new Rectangle(0, 0, tileWidth, tileHeight);

            if (interior)
            {
                for (int wx = winx / tileWidth; wx < (winx / tileWidth) + alpha.WinWidth; wx++)
                {
                    for (int wy = winy / tileHeight; wy < (winy / tileHeight) + alpha.WinHeight; wy++)
                    {
                        if (mg.getCell(wx, wy) == false)
                        {
                            rect.X = wx * tileWidth - winx;
                            rect.Y = wy * tileHeight - winy;
                            DrawRects.drawRect(renderer, rect, red, true);
                        }
                    }
                }
            }
            else
            {
                int q = calculateCurrentQuadrent();
                int tmpx = 0;
                int tmpy = 0;
                switch (q)
                {
                    case 1:
                        for (int wx = winx / tileWidth; wx < (winx / tileWidth) + alpha.WinWidth; wx++)
                        {
                            for (int wy = winy / tileHeight; wy < (winy / tileHeight) + alpha.WinHeight; wy++)
                            {
                                tmpx = wx - (alpha.CellX * alpha.Width);
                                tmpy = wy - (alpha.CellY * alpha.Height);
                                if (mg.getCell(tmpx + alpha.Width, tmpy + alpha.Height) == false)
                                {
                                    rect.X = wx * tileWidth - winx;
                                    rect.Y = wy * tileHeight - winy;
                                    DrawRects.drawRect(renderer, rect, red, true);
                                }
                            }
                        }
                        break;
                    case 2:
                        for (int wx = winx / tileWidth; wx < (winx / tileWidth) + alpha.WinWidth; wx++)
                        {
                            for (int wy = winy / tileHeight; wy < (winy / tileHeight) + alpha.WinHeight; wy++)
                            {
                                tmpx = wx - (alpha.CellX * alpha.Width);
                                tmpy = wy - (alpha.CellY * alpha.Height);
                                if (mg.getCell(tmpx, tmpy + alpha.Height) == false)
                                {
                                    rect.X = wx * tileWidth - winx;
                                    rect.Y = wy * tileHeight - winy;
                                    DrawRects.drawRect(renderer, rect, red, true);
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int wx = winx / tileWidth; wx < (winx / tileWidth) + alpha.WinWidth; wx++)
                        {
                            for (int wy = winy / tileHeight; wy < (winy / tileHeight) + alpha.WinHeight; wy++)
                            {
                                tmpx = wx - (alpha.CellX * alpha.Width);
                                tmpy = wy - (alpha.CellY * alpha.Height);
                                if (mg.getCell(tmpx, tmpy) == false)
                                {
                                    rect.X = wx * tileWidth - winx;
                                    rect.Y = wy * tileHeight - winy;
                                    DrawRects.drawRect(renderer, rect, red, true);
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int wx = winx / tileWidth; wx < (winx / tileWidth) + alpha.WinWidth; wx++)
                        {
                            for (int wy = winy / tileHeight; wy < (winy / tileHeight) + alpha.WinHeight; wy++)
                            {
                                tmpx = wx - (alpha.CellX * alpha.Width);
                                tmpy = wy - (alpha.CellY * alpha.Height);
                                if (mg.getCell(tmpx + alpha.Width, tmpy) == false)
                                {
                                    rect.X = wx * tileWidth - winx;
                                    rect.Y = wy * tileHeight - winy;
                                    DrawRects.drawRect(renderer, rect, red, true);
                                }
                            }
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Draws the local mapGraph
        /// </summary>
        /// <param name="renderer">Renderer referecne</param>
        /// <param name="shiftx">Shift x offset</param>
        /// <param name="shifty">Shift y offset</param>
        public void drawLocalMG(IntPtr renderer, int shiftx = 0, int shifty = 0)
        {
            if (alpha != null)
            {
                alpha.drawLocalMG(renderer, winx, winy, shiftx, shifty);
            }
        }
        /// <summary>
        /// Calculate the current quadrent
        /// </summary>
        /// <returns>Boolean</returns>
        public int calculateCurrentQuadrent()
        {
            //avatar's current cell
            int cellx = (int)avatar.Center.X / (cellWidth * tileWidth);
            int celly = (int)avatar.Center.Y / (cellHeight * tileHeight);
            //avatar's current cell center
            int hazx = (cellx * (cellWidth * tileWidth)) + ((cellWidth / 2) * tileWidth);
            int hazy = (celly * (cellHeight * tileHeight)) + ((cellHeight / 2) * tileHeight);
            if (avatar.Center.IX >= hazx)//right side of cell
            {
                if(avatar.Center.IY >= hazy)
                {
                    return 3;//bottom right side of cell
                }
                else
                {
                    return 2;//top right side of cell
                }
            }
            else//left side of cell
            {
                if (avatar.Center.IY >= hazy)
                {
                    return 4;//bottom left side of cell
                }
                else
                {
                    return 1;//top left side of cell
                }
            }
        }
        /// <summary>
        /// Calculate the current quadrent of a point
        /// </summary>
        /// <returns>Boolean</returns>
        public int calculateCurrentQuadrent(float x, float y)
        {
            //avatar's current cell
            int cellx = (int)x / (cellWidth * tileWidth);
            int celly = (int)y / (cellHeight * tileHeight);
            //avatar's current cell center
            int hazx = (cellx * (cellWidth * tileWidth)) + ((cellWidth / 2) * tileWidth);
            int hazy = (celly * (cellHeight * tileHeight)) + ((cellHeight / 2) * tileHeight);
            if (avatar.Center.IX >= hazx)//right side of cell
            {
                if (avatar.Center.IY >= hazy)
                {
                    return 3;//bottom right side of cell
                }
                else
                {
                    return 2;//top right side of cell
                }
            }
            else//left side of cell
            {
                if (avatar.Center.IY >= hazy)
                {
                    return 4;//bottom left side of cell
                }
                else
                {
                    return 1;//top left side of cell
                }
            }
        }
        /// <summary>
        /// Updates active cells
        /// </summary>
        public void updateCells()
        {
            if(saveFolderPath == "")
            {
                saveFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            }
            //save cells in temp variables (and save cell data)
            //add saving cell data later
            tempA = alpha;
            //alpha.saveData(saveFolderPath);
            tempB = beta;
            //beta.saveData(saveFolderPath);
            tempD = delta;
            //delta.saveData(saveFolderPath);
            tempG = gamma;
            //gamma.saveData(saveFolderPath);

            //identifiy which cell avatar is in
            if (tempA.inCell(avatar.Center.X, avatar.Center.Y))
            {
                //update alpha
                alpha = tempA;
                tempA = null;
                //based on quadrent update beta, delta and gamma
                switch (currentQuadrent)
                {
                    case 1:
                        beta = getCellAt(alpha.CellX - 1, alpha.CellY);
                        delta = getCellAt(alpha.CellX - 1, alpha.CellY - 1);
                        gamma = getCellAt(alpha.CellX, alpha.CellY - 1);
                        break;
                    case 2:
                        beta = getCellAt(alpha.CellX, alpha.CellY - 1);
                        delta = getCellAt(alpha.CellX + 1, alpha.CellY - 1);
                        gamma = getCellAt(alpha.CellX + 1, alpha.CellY);
                        break;
                    case 3:
                        beta = getCellAt(alpha.CellX + 1, alpha.CellY);
                        delta = getCellAt(alpha.CellX + 1, alpha.CellY + 1);
                        gamma = getCellAt(alpha.CellX, alpha.CellY + 1);
                        break;
                    case 4:
                        beta = getCellAt(alpha.CellX, alpha.CellY + 1);
                        delta = getCellAt(alpha.CellX - 1, alpha.CellY + 1);
                        gamma = getCellAt(alpha.CellX - 1, alpha.CellY);
                        break;
                }
            }
            else if (tempB != null && tempB.inCell(avatar.Center.X, avatar.Center.Y))
            {
                //update alpha
                alpha = tempB;
                tempB = null;
                //based on quadrent update beta, delta and gamma
                switch (currentQuadrent)
                {
                    case 1:
                        beta = getCellAt(alpha.CellX - 1, alpha.CellY);
                        delta = getCellAt(alpha.CellX - 1, alpha.CellY - 1);
                        gamma = getCellAt(alpha.CellX, alpha.CellY - 1);
                        break;
                    case 2:
                        beta = getCellAt(alpha.CellX, alpha.CellY - 1);
                        delta = getCellAt(alpha.CellX + 1, alpha.CellY - 1);
                        gamma = getCellAt(alpha.CellX + 1, alpha.CellY);
                        break;
                    case 3:
                        beta = getCellAt(alpha.CellX + 1, alpha.CellY);
                        delta = getCellAt(alpha.CellX + 1, alpha.CellY + 1);
                        gamma = getCellAt(alpha.CellX, alpha.CellY + 1);
                        break;
                    case 4:
                        beta = getCellAt(alpha.CellX, alpha.CellY + 1);
                        delta = getCellAt(alpha.CellX - 1, alpha.CellY + 1);
                        gamma = getCellAt(alpha.CellX - 1, alpha.CellY);
                        break;
                }
            }
            else if (tempD != null && tempD.inCell(avatar.Center.X, avatar.Center.Y))
            {
                //update alpha
                alpha = tempD;
                tempD = null;
                //based on quadrent update beta, delta and gamma
                switch (currentQuadrent)
                {
                    case 1:
                        beta = getCellAt(alpha.CellX - 1, alpha.CellY);
                        delta = getCellAt(alpha.CellX - 1, alpha.CellY - 1);
                        gamma = getCellAt(alpha.CellX, alpha.CellY - 1);
                        break;
                    case 2:
                        beta = getCellAt(alpha.CellX, alpha.CellY - 1);
                        delta = getCellAt(alpha.CellX + 1, alpha.CellY - 1);
                        gamma = getCellAt(alpha.CellX + 1, alpha.CellY);
                        break;
                    case 3:
                        beta = getCellAt(alpha.CellX + 1, alpha.CellY);
                        delta = getCellAt(alpha.CellX + 1, alpha.CellY + 1);
                        gamma = getCellAt(alpha.CellX, alpha.CellY + 1);
                        break;
                    case 4:
                        beta = getCellAt(alpha.CellX, alpha.CellY + 1);
                        delta = getCellAt(alpha.CellX - 1, alpha.CellY + 1);
                        gamma = getCellAt(alpha.CellX - 1, alpha.CellY);
                        break;
                }
            }
            else if (tempG != null && tempG.inCell(avatar.Center.X, avatar.Center.Y))
            {
                //update alpha
                alpha = tempG;
                tempG = null;
                //based on quadrent update beta, delta and gamma
                switch (currentQuadrent)
                {
                    case 1:
                        beta = getCellAt(alpha.CellX - 1, alpha.CellY);
                        delta = getCellAt(alpha.CellX - 1, alpha.CellY - 1);
                        gamma = getCellAt(alpha.CellX, alpha.CellY - 1);
                        break;
                    case 2:
                        beta = getCellAt(alpha.CellX, alpha.CellY - 1);
                        delta = getCellAt(alpha.CellX + 1, alpha.CellY - 1);
                        gamma = getCellAt(alpha.CellX + 1, alpha.CellY);
                        break;
                    case 3:
                        beta = getCellAt(alpha.CellX + 1, alpha.CellY);
                        delta = getCellAt(alpha.CellX + 1, alpha.CellY + 1);
                        gamma = getCellAt(alpha.CellX, alpha.CellY + 1);
                        break;
                    case 4:
                        beta = getCellAt(alpha.CellX, alpha.CellY + 1);
                        delta = getCellAt(alpha.CellX - 1, alpha.CellY + 1);
                        gamma = getCellAt(alpha.CellX - 1, alpha.CellY);
                        break;
                }
            }
            if(beta == null)
            {
                switch(currentQuadrent)
                {
                    case 1:
                        beta = getCellAt(alpha.CellX - 1, alpha.CellY);
                        break;
                    case 2:
                        beta = getCellAt(alpha.CellX, alpha.CellY - 1);
                        break;
                    case 3:
                        beta = getCellAt(alpha.CellX + 1, alpha.CellY);
                        break;
                    case 4:
                        beta = getCellAt(alpha.CellX, alpha.CellY + 1);
                        break;
                }
            }
            if (delta == null)
            {
                switch (currentQuadrent)
                {
                    case 1:
                        delta = getCellAt(alpha.CellX - 1, alpha.CellY - 1);
                        break;
                    case 2:
                        delta = getCellAt(alpha.CellX + 1, alpha.CellY - 1);
                        break;
                    case 3:
                        delta = getCellAt(alpha.CellX + 1, alpha.CellY + 1);
                        break;
                    case 4:
                        delta = getCellAt(alpha.CellX - 1, alpha.CellY + 1);
                        break;
                }
            }
            if (gamma == null)
            {
                switch (currentQuadrent)
                {
                    case 1:
                        gamma = getCellAt(alpha.CellX, alpha.CellY - 1);
                        break;
                    case 2:
                        gamma = getCellAt(alpha.CellX + 1, alpha.CellY);
                        break;
                    case 3:
                        gamma = getCellAt(alpha.CellX, alpha.CellY + 1);
                        break;
                    case 4:
                        gamma = getCellAt(alpha.CellX - 1, alpha.CellY);
                        break;
                }
            }
        }
        /// <summary>
        /// Check if avatar has changed quadrents and update currentQuadrent if there is a change
        /// </summary>
        /// <returns>Boolean</returns>
        public bool hasChangedQuadrents()
        {
            int tempQuadrent = calculateCurrentQuadrent();
            if (currentQuadrent != tempQuadrent)
            {
                currentQuadrent = tempQuadrent;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if active cells need updating and updates if so
        /// </summary>
        public void doCellsNeedUpdating()
        {
            if(hasChangedQuadrents())
            {
                updateCells();
            }//If quadrent is fine but avatar is not in alpha cell updateCells
            else if(alpha.inCell(avatar.Center.X, avatar.Center.Y) == false)
            {
                updateCells();
            }
        }
        /// <summary>
        /// Sets the window position
        /// </summary>
        /// <param name="x">Window x position</param>
        /// <param name="y">Window y position</param>
        public void setWindow(int x, int y)
        {
            winx = x;
            winy = y;
        }
        /// <summary>
        /// Returns the cell at position cx, cy
        /// </summary>
        /// <param name="cx">Cellx value</param>
        /// <param name="cy">Celly value</param>
        /// <returns>OpenWorldCell</returns>
        public virtual OpenWorldCell getCellAt(int cx, int cy)
        {
            //check tempA, tempB, tempD and tempG for cell else try to load from file
            bool found = false;
            if(tempA != null)
            {
                if(tempA.CellX == cx && tempA.CellY == cy)
                {
                    found = true;
                    return tempA;
                }
            }
            if (!found && tempB != null)
            {
                if (tempB.CellX == cx && tempB.CellY == cy)
                {
                    found = true;
                    return tempB;
                }
            }
            if (!found && tempD != null)
            {
                if (tempD.CellX == cx && tempD.CellY == cy)
                {
                    found = true;
                    return tempD;
                }
            }
            if (!found && tempG != null)
            {
                if (tempG.CellX == cx && tempG.CellY == cy)
                {
                    found = true;
                    return tempG;
                }
            }
             //load a cell file if it's in world domain
            if(!found && inWorld(cx, cy))
            {
                return loadCell(cx, cy, saveFolderPath);
            }
            //no cell in world so return null
            return null;
        }
        /// <summary>
        /// Checks a space on map to see if clear;
        /// </summary>
        /// <param name="x">X position in tiles</param>
        /// <param name="y">Y position in tiles</param>
        /// <returns>Boolean</returns>
        public bool isPositionClear(int x, int y)
        {
            bool temp = false;
            int cx = x - (alpha.CellX * alpha.Width);
            int cy = y - (alpha.CellY * alpha.Height);
            if (interior)
            {
                temp = mg.getCell(x, y);
            }
            else
            {
                switch (currentQuadrent)
                {
                    case 1:
                        temp = mg.getCell(cx + alpha.Width, cy + alpha.Height);
                        break;
                    case 2:
                        temp = mg.getCell(cx, cy + alpha.Height);
                        break;
                    case 3:
                        temp = mg.getCell(cx, cy);
                        break;
                    case 4:
                        temp = mg.getCell(cx + alpha.Width, cy);
                        break;
                }
            }
            return temp;
        }
        /// <summary>
        /// Moves player via keyboard input
        /// </summary>
        public void updateAvatarPos()
        {
            if (!inputDelay.Active)
            {
                if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_w))
                {
                    avatar.Still = false;
                    if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_a))
                    {
                        if (interior)
                        {
                            if (avatar.Left - 4 >= alpha.CellLeftSide && avatar.Top - 4 >= alpha.CellTopSide)
                            {
                                if(isPositionClear((int)(avatar.Left - 4) / tileWidth, (int)(avatar.Waist - 4) / tileHeight) &&
                                    isPositionClear((int)(avatar.Left - 4) / tileWidth, (int)(avatar.Bottom) / tileHeight))
                                {
                                    avatar.move(-4, -4);
                                }
                            }
                        }
                        else
                        {
                            if (avatar.Left - 4 >= worldLeftSide && avatar.Top - 4 >= worldTopSide)
                            {
                                if (isPositionClear((int)(avatar.Left - 4) / tileWidth, (int)(avatar.Waist - 4) / tileHeight) &&
                                    isPositionClear((int)(avatar.Left - 4) / tileWidth, (int)(avatar.Bottom) / tileHeight))
                                {
                                    avatar.move(-4, -4);
                                }
                            }
                        }
                        avatar.Direct = DIRECT.UPLEFT;
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_d))
                    {
                        if (interior)
                        {
                            if (avatar.Right + 4 < alpha.CellRightSide && avatar.Top - 4 >= alpha.CellTopSide)
                            {
                                if (isPositionClear((int)(avatar.Right + 4) / tileWidth, (int)(avatar.Waist - 4) / tileWidth) &&
                                    isPositionClear((int)(avatar.Right + 4) / tileWidth, (int)(avatar.Bottom) / tileWidth))
                                {
                                    avatar.move(4, -4);
                                }
                            }
                        }
                        else
                        {
                            if (avatar.Right + 4 < worldRightSide && avatar.Top - 4 >= worldTopSide)
                            {
                                if (isPositionClear((int)(avatar.Right + 4) / tileWidth, (int)(avatar.Waist - 4) / tileHeight) &&
                                    isPositionClear((int)(avatar.Right + 4) / tileWidth, (int)(avatar.Bottom) / tileHeight))
                                {
                                    avatar.move(4, -4);
                                }
                            }
                        }
                        avatar.Direct = DIRECT.UPRIGHT;
                    }
                    else if (!KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_d) &&
                        !KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_a))
                    {
                        if (interior)
                        {
                            if (avatar.Top - 4 >= alpha.CellTopSide)
                            {
                                if (isPositionClear((int)(avatar.Left) / tileWidth, (int)(avatar.Waist - 4) / tileHeight) &&
                                    isPositionClear((int)(avatar.Left) / tileWidth, (int)(avatar.Bottom) / tileHeight))
                                {
                                    avatar.move(0, -4);
                                }
                            }
                        }
                        else
                        {
                            if (avatar.Top - 4 >= worldTopSide)
                            {
                                if (isPositionClear((int)(avatar.Left) / tileWidth, (int)(avatar.Waist - 4) / tileHeight) &&
                                    isPositionClear((int)(avatar.Right) / tileWidth, (int)(avatar.Waist - 4) / tileHeight))
                                {
                                    avatar.move(0, -4);
                                }
                            }
                        }
                        avatar.Direct = DIRECT.UP;
                    }
                    inputDelay.activate();
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_s))
                {
                    avatar.Still = false;
                    if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_a))
                    {
                        if (interior)
                        {
                            if (avatar.Left - 4 >= alpha.CellLeftSide && avatar.Bottom + 4 < alpha.CellBottomSide)
                            {
                                if (isPositionClear((int)(avatar.Left - 4) / tileWidth, (int)(avatar.Bottom + 4) / tileHeight) &&
                                    isPositionClear((int)(avatar.Left - 4) / tileWidth, (int)(avatar.Waist) / tileHeight))
                                {
                                    avatar.move(-4, 4);
                                }
                            }
                        }
                        else
                        {
                            if (avatar.Left - 4 >= worldLeftSide && avatar.Bottom + 4 < worldBottomSide)
                            {
                                if (isPositionClear((int)(avatar.Left - 4) / tileWidth, (int)(avatar.Bottom + 4) / tileHeight) &&
                                    isPositionClear((int)(avatar.Left - 4) / tileWidth, (int)(avatar.Waist) / tileHeight))
                                {
                                    avatar.move(-4, 4);
                                }
                            }
                        }
                        avatar.Direct = DIRECT.DOWNLEFT;
                    }
                    else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_d))
                    {
                        if (interior)
                        {
                            if (avatar.Right + 4 < alpha.CellRightSide && avatar.Bottom + 4 < alpha.CellBottomSide)
                            {
                                if (isPositionClear((int)(avatar.Right + 4) / tileWidth, (int)(avatar.Bottom + 4) / tileHeight) &&
                                    isPositionClear((int)(avatar.Right + 4) / tileWidth, (int)(avatar.Waist) / tileHeight))
                                {
                                    avatar.move(4, 4);
                                }
                            }
                        }
                        else
                        {
                            if (avatar.Right + 4 < worldRightSide && avatar.Bottom + 4 < worldBottomSide)
                            {
                                if (isPositionClear((int)(avatar.Right + 4) / tileWidth, (int)(avatar.Bottom + 4) / tileHeight) &&
                                    isPositionClear((int)(avatar.Right + 4) / tileWidth, (int)(avatar.Waist) / tileHeight))
                                {
                                    avatar.move(4, 4);
                                }
                            }
                        }
                        avatar.Direct = DIRECT.DOWNRIGHT;
                    }
                    else if (!KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_d) &&
                        !KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_a))
                    {
                        if (interior)
                        {
                            if (avatar.Bottom + 4 < alpha.CellBottomSide)
                            {
                                if (isPositionClear((int)(avatar.Left) / tileWidth, (int)(avatar.Bottom + 4) / tileHeight) &&
                                    isPositionClear((int)(avatar.Right) / tileWidth, (int)(avatar.Bottom + 4) / tileHeight))
                                {
                                    avatar.move(0, 4);
                                }
                            }
                        }
                        else
                        {
                            if (avatar.Bottom + 4 < worldBottomSide)
                            {
                                if (isPositionClear((int)(avatar.Left) / tileWidth, (int)(avatar.Bottom + 4) / tileHeight) &&
                                    isPositionClear((int)(avatar.Right) / tileWidth, (int)(avatar.Bottom + 4) / tileHeight))
                                {
                                    avatar.move(0, 4);
                                }
                            }
                        }
                        avatar.Direct = DIRECT.DOWN;
                    }
                    inputDelay.activate();
                }
                if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_d))
                {
                    if (!KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_s) &&
                        !KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_w))
                    {
                        avatar.Still = false;
                        if (interior)
                        {
                            if (avatar.Right + 4 < alpha.CellRightSide)
                            {
                                if (isPositionClear((int)(avatar.Right + 4) / tileWidth, (int)(avatar.Bottom) / tileWidth) &&
                                    isPositionClear((int)(avatar.Left) / tileWidth, (int)(avatar.Top) / tileWidth))
                                {
                                    avatar.move(4, 0);
                                }
                            }
                        }
                        else
                        {
                            if (avatar.Right + 4 < worldRightSide)
                            {
                                if (isPositionClear((int)(avatar.Right + 4) / tileWidth, (int)(avatar.Bottom) / tileWidth) &&
                                    isPositionClear((int)(avatar.Left) / tileWidth, (int)(avatar.Top) / tileWidth))
                                {
                                    avatar.move(4, 0);
                                }
                            }
                        }
                        avatar.Direct = DIRECT.RIGHT;
                    }
                    inputDelay.activate();
                }
                else if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_a))
                {
                    if (!KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_w) &&
                        !KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_s))
                    {
                        avatar.Still = false;
                        if (interior)
                        {
                            if (avatar.Left - 4 >= alpha.CellLeftSide)
                            {
                                if (isPositionClear((int)(avatar.Left - 4) / tileWidth, (int)(avatar.Bottom) / tileWidth) &&
                                    isPositionClear((int)(avatar.Left - 4) / tileWidth, (int)(avatar.Top) / tileWidth))
                                {
                                    avatar.move(-4, 0);
                                }
                            }
                        }
                        else
                        {
                            if (avatar.Left - 4 >= worldLeftSide)
                            {
                                if (isPositionClear((int)(avatar.Left - 4) / tileWidth, (int)(avatar.Bottom) / tileWidth) &&
                                    isPositionClear((int)(avatar.Left - 4) / tileWidth, (int)(avatar.Top) / tileWidth))
                                {
                                    avatar.move(-4, 0);
                                }
                            }
                        }
                        avatar.Direct = DIRECT.LEFT;
                    }
                    inputDelay.activate();
                }
                if (!KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_w) &&
                    !KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_d) &&
                    !KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_s) &&
                    !KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_a))
                {
                    avatar.Still = true;
                }
            }
            inputDelay.update();
        }
        /// <summary>
        /// Attempt to interact with an object in front of player avatar
        /// </summary>
        public virtual void avatarInteract()
        {
            if (KeyboardHandler.getKeyState(SDL.SDL_Keycode.SDLK_e))
            {
                int tx = 0;
                int ty = 0;
                switch (avatar.Direct)
                {
                    case DIRECT.UP:
                        tx = avatar.Center.IX;
                        ty = avatar.Center.IY - (int)(avatar.H);
                        break;
                    case DIRECT.RIGHT:
                        tx = avatar.Center.IX + (int)(avatar.W);
                        ty = avatar.Center.IY;
                        break;
                    case DIRECT.DOWN:
                        tx = avatar.Center.IX;
                        ty = avatar.Center.IY + (int)(avatar.H);
                        break;
                    case DIRECT.LEFT:
                        tx = avatar.Center.IX - (int)(avatar.W);
                        ty = avatar.Center.IY;
                        break;
                }
                for (int n = 0; n < npcs.Count; n++)
                {
                    //talk to npc
                }
            }
        }
        /// <summary>
        /// Empties the event queue
        /// </summary>
        public void emptyEventQueue(out SDL.SDL_Event eve)
        {
            while (SDL.SDL_PollEvent(out eve) != 0)
            {
                //do nothing event queue is just being cleared
            }
        }
        /// <summary>
        /// Load a specified cell
        /// </summary>
        /// <param name="cx">Cell's cx value</param>
        /// <param name="cy">Cell's cy value</param>
        /// <param name="path">Folder path of cell to load</param>
        /// <returns>OpenWordCell</returns>
        public virtual OpenWorldCell loadCell(int cx, int cy, string path = "")
        {
            if(path == "")
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }
            StreamReader sr;
            if(isPlanet == true)
            {
                path += planet + "\\" + "cell_" + cx + "_" + cy + ".owc";
                sr = new StreamReader(path);
            }
            else
            {
                path += "cell_" + cx + "_" + cy + ".owc";
                sr = new StreamReader(path);
            }
            OpenWorldCell cell = new OpenWorldCell(source, autoSource, winWidth, winHeight, sr);
            sr.Close();
            //cell name is "cell_(cx value)_(cy value).owc"
            return cell;
        }
        /// <summary>
        /// Loads an interior cell
        /// </summary>
        /// <param name="name">Cell name</param>
        /// <param name="path">Folder path for cell to load</param>
        /// <returns>OpenWorldCell</returns>
        public virtual OpenWorldCell loadInteriorCell(string name, string path = "")
        {
            if (path == "")
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }
            StreamReader sr;
            if (isPlanet == true)
            {
                path += planet + "\\" + "cell_" + name + ".owc";
                sr = new StreamReader(path);
            }
            else
            {
                sr = new StreamReader(path);
            }
            OpenWorldCell cell = new OpenWorldCell(interSource, interAutoSource, winWidth, winHeight, sr);
            sr.Close();
            //cell name is "cell_(name).owc"
            cell.IsInteriorCell = true;
            interior = true;
            return cell;
        }
        /// <summary>
        /// Enter/exit a cell via a portal
        /// </summary>
        /// <param name="name">Cell's name</param>
        /// <param name="path">Save/load folder path</param>
        /// <param name="cx">Cell's x value</param>
        /// <param name="cy">Cell's y value</param>
        /// <param name="ax">Avatar's target x value</param>
        /// <param name="ay">Avatar's target y value</param>
        /// <param name="intoInterior"></param>
        public virtual void enterExit(string name, string path, int cx, int cy, float ax, float ay, 
            bool intoInterior = false)
        {
            //Ensure savePath set to valid folder
            string savePath = "";
            //string cellName = "";
            if (path == "")
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }
            //save cells
            saveCellStates(savePath);
            avatar.setPos(ax + (cx * cellWidth * tileWidth), ay + (cy * cellHeight * tileHeight));
            //handle for into interior cell
            if (intoInterior)
            {
                alpha = loadInteriorCell(name, path);
                //set adjacent cells to null while in an interior cell
                beta = null;
                delta = null;
                gamma = null;
                interior = true;
            }
            else//handle for into exterior cell
            {
                alpha = loadCell(cx, cy, path);
                alpha.IsInteriorCell = false;
                interior = false;
                updateWorldSides();
                calculateCurrentQuadrent();
                updateCells();
            }
        }
        /// <summary>
        /// Save cell's states
        /// </summary>
        /// <param name="savePath"></param>
        public virtual void saveCellStates(string savePath, bool saveAuto = true)
        {
            string cellName = "";
            if (alpha.IsInteriorCell)
            {
                cellName = alpha.Name;
            }
            else
            {
                cellName = alpha.CellX + "_" + alpha.CellY;
            }
            if (isPlanet == true)
            {
                alpha.saveData(savePath + planet + "\\", cellName, saveAuto);
            }
            else
            {
                alpha.saveData(savePath, cellName, saveAuto);
            }
            alpha = null;
            if (beta != null)
            {
                if (beta.IsInteriorCell)
                {
                    cellName = beta.Name;
                }
                else
                {
                    cellName = beta.CellX + "_" + beta.CellY;
                }
                if (isPlanet == true)
                {
                    beta.saveData(savePath + planet + "\\", cellName, saveAuto);
                }
                else
                {
                    beta.saveData(savePath, cellName, saveAuto);
                }
            }
            beta = null;
            if (delta != null)
            {
                if (delta.IsInteriorCell)
                {
                    cellName = delta.Name;
                }
                else
                {
                    cellName = delta.CellX + "_" + delta.CellY;
                }
                if (isPlanet == true)
                {
                    delta.saveData(savePath + planet + "\\", cellName, saveAuto);
                }
                else
                {
                    delta.saveData(savePath, cellName, saveAuto);
                }
            }
            delta = null;
            if (gamma != null)
            {
                if (gamma.IsInteriorCell)
                {
                    cellName = gamma.Name;
                }
                else
                {
                    cellName = gamma.CellX + "_" + gamma.CellY;
                }
                if (isPlanet == true)
                {
                    gamma.saveData(savePath + planet + "\\", cellName, saveAuto);
                }
                else
                {
                    gamma.saveData(savePath, cellName, saveAuto);
                }
            }
            gamma = null;
        }
        /// <summary>
        /// Updates the world sides to defaults
        /// </summary>
        public void updateWorldSides()
        {
            worldLeftSide = 0;
            worldTopSide = 0;
            worldRightSide = worldWidth * (cellWidth * tileWidth);
            worldBottomSide = worldHeight * (CellHeight * tileHeight);
        }
        //test version for the moment
        public void loadWorld()
        {
            alpha = loadCell(0, 0);
            beta = loadCell(1, 0);
            delta = loadCell(0, 1);
            gamma = loadCell(1, 1);
        }
        //test version for the moment
        public virtual void generateWorld()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            OpenWorldCell temp = new OpenWorldCell(source, autoSource, CellWidth, cellHeight, tileWidth, tileHeight, winWidth, winHeight, 0, 0, 0, 0, "cell_0_0");
            temp.Tiles.fillLayer(1, 0, 0);
            alpha = temp;
            alpha.Tiles.setTile(1, 99, 99, 96, 96);
            alpha.AutoTiles.addTile(97, 98);
            alpha.AutoTiles.addTile(96, 98);
            alpha.AutoTiles.addTile(96, 97);
            alpha.AutoTiles.addTile(96, 96);
            alpha.AutoTiles.addTile(97, 97);
            alpha.AutoTiles.addTile(97, 96);
            alpha.AutoTiles.addTile(98, 96);
            alpha.AutoTiles.addTile(98, 97);
            alpha.AutoTiles.addTile(97, 95);
            addPortalToCell(alpha, 93 * 32, 93 * 32, 0, 0, 50 * 32, 12 * 32, "dungeon", true);
            temp.saveData(path, "", true);
            temp = new OpenWorldCell(source, autoSource, CellWidth, cellHeight, tileWidth, tileHeight, winWidth, winHeight, 0, 0, 1, 0, "cell_1_0");
            temp.Tiles.fillLayer(1, 0, 0);
            beta = temp;
            beta.Tiles.setTile(1, 0, 99, 96, 96);
            temp.saveData(path, "", true);
            temp = new OpenWorldCell(source, autoSource, CellWidth, cellHeight, tileWidth, tileHeight, winWidth, winHeight, 0, 0, 1, 1, "cell_1_1");
            temp.Tiles.fillLayer(1, 0, 0);
            delta = temp;
            delta.Tiles.setTile(1, 0, 0, 96, 96);
            temp.saveData(path, "", true);
            temp = new OpenWorldCell(source, autoSource, CellWidth, cellHeight, tileWidth, tileHeight, winWidth, winHeight, 0, 0, 0, 1, "cell_0_1");
            temp.Tiles.fillLayer(1, 0, 0);
            gamma = temp;
            gamma.Tiles.setTile(1, 99, 0, 96, 96);
            temp.saveData(path, "", true);
            temp = new OpenWorldCell(source, autoSource, CellWidth, cellHeight, tileWidth, tileHeight, winWidth, winHeight, 0, 0, 2, 0, "cell_2_0");
            temp.Tiles.fillLayer(1, 160, 32);
            temp.saveData(path, "", true);
            temp = new OpenWorldCell(source, autoSource, CellWidth, cellHeight, tileWidth, tileHeight, winWidth, winHeight, 0, 0, 2, 1, "cell_2_1");
            temp.Tiles.fillLayer(1, 32, 160);
            temp.saveData(path, "", true);
            temp = new OpenWorldCell(source, autoSource, CellWidth, cellHeight, tileWidth, tileHeight, winWidth, winHeight, 0, 0, 2, 2, "cell_2_2");
            temp.Tiles.fillLayer(1, 96, 160);
            temp.saveData(path, "", true);
            temp = new OpenWorldCell(source, autoSource, CellWidth, cellHeight, tileWidth, tileHeight, winWidth, winHeight, 0, 0, 2, 3, "cell_2_3");
            temp.Tiles.fillLayer(1, 160, 96);
            temp.saveData(path, "", true);
            temp = new OpenWorldCell(source, autoSource, CellWidth, cellHeight, tileWidth, tileHeight, winWidth, winHeight, 0, 0, 0, 2, "cell_0_2");
            temp.Tiles.fillLayer(1, 128, 128);
            temp.saveData(path, "", true);
            temp = new OpenWorldCell(source, autoSource, CellWidth, cellHeight, tileWidth, tileHeight, winWidth, winHeight, 0, 0, 0, 3, "cell_0_3");
            temp.Tiles.fillLayer(1, 32, 32);
            temp.saveData(path, "", true);
            temp = new OpenWorldCell(source, autoSource, CellWidth, cellHeight, tileWidth, tileHeight, winWidth, winHeight, 0, 0, 1, 2, "cell_1_2");
            temp.Tiles.fillLayer(1, 64, 64);
            temp.saveData(path, "", true);
            temp = new OpenWorldCell(source, autoSource, CellWidth, cellHeight, tileWidth, tileHeight, winWidth, winHeight, 0, 0, 1, 3, "cell_1_3");
            temp.Tiles.fillLayer(1, 128, 96);
            temp.saveData(path, "", true);
            temp = new OpenWorldCell(source, autoSource, CellWidth, cellHeight, tileWidth, tileHeight, winWidth, winHeight, 0, 0, 3, 1, "cell_3_1");
            temp.Tiles.fillLayer(1, 96, 128);
            temp.saveData(path, "", true);
            temp = new OpenWorldCell(source, autoSource, CellWidth, cellHeight, tileWidth, tileHeight, winWidth, winHeight, 0, 0, 3, 2, "cell_3_2");
            temp.Tiles.fillLayer(1, 32, 96);
            temp.saveData(path, "", true);
            temp = new OpenWorldCell(source, autoSource, CellWidth, cellHeight, tileWidth, tileHeight, winWidth, winHeight, 0, 0, 3, 3, "cell_3_3");
            temp.Tiles.fillLayer(1, 96, 64);
            temp.saveData(path, "", true);
            temp = new OpenWorldCell(source, autoSource, CellWidth, cellHeight, tileWidth, tileHeight, winWidth, winHeight, 0, 0, 3, 0, "cell_3_0");
            temp.Tiles.fillLayer(1, 64, 96);
            temp.saveData(path, "", true);
        }
        /// <summary>
        /// Generate a dungeon and save (doors and portals not yet incorporated)
        /// </summary>
        /// <param name="savePath">File path to save to</param>
        /// <param name="name">Dungeon name</param>
        /// <param name="spawnable">Spawnable flag value</param>
        /// <param name="saveAuto">Close StreamWriter when finished flag value</param>
        public virtual void generatedungeon(string savePath = "", string name = "dungeon", bool spawnable = true, bool saveAuto = true)
        {
            if(savePath == "")
            {
                savePath = AppDomain.CurrentDomain.BaseDirectory;
            }
            //generate a depth 3 dungeon for testing
            OpenWorldCell dungeon = new OpenWorldCell(source, autoSource, cellWidth, cellHeight, tileWidth, 
                tileHeight, winWidth, winHeight, 0, 0, 0, 0, name, spawnable);
            dungeon.Tiles.fillLayer(1, 0, 0);
            dungeon.AutoTiles.fill();
            //add dungeon at 50, 10 to ensure space but add portal later
            dungeon.addChamber(6, 8, CHAMBER.DOWN, 50, 10, true);
            //add test portal back
            addPortalToCell(dungeon, 49 * tileWidth, 10 * tileHeight, 0, 0, 93 * 32, 95 * 32, "", false);
            //save dungeon
            //savePath += name + ".owc";
            if (isPlanet == true)
            {
                dungeon.saveData(savePath + planet + "\\", name, saveAuto);
            }
            else
            {
                dungeon.saveData(savePath, name, saveAuto);
            }
        }
        /// <summary>
        /// Centers window on a position in world
        /// </summary>
        /// <param name="x">X position in world</param>
        /// <param name="y">Y position in world</param>
        public void centerWindow(int x, int y)
        {
            int tempx = x - (((winWidth - 3) * tileWidth) / 2);
            //modx = (tempx % (tileWidth - 1));
            winx = tempx;// - modx;
            if(winx < 0)
            {
                winx = 0;
            }
            if (interior)
            {
                if (winx > alpha.Width * tileWidth)
                {
                    winx = (alpha.Width * tileWidth) - (winWidth * tileWidth);
                }
            }
            else
            {
                if (winx > worldWidth * (cellWidth * tileWidth))
                {
                    winx = (worldWidth * (cellWidth * tileWidth)) - winWidth * tileWidth;
                }
            }
            int tempy = y - (((winHeight - 3) * tileHeight) / 2);

            //mody = (tempy % (tileHeight - 1));
            winy = tempy;// - mody;
            if (winy < 0)
            {
                winy = 0;
            }
            if (interior)
            {
                if (winx > alpha.Height * tileHeight)
                {
                    winx = (alpha.Height * tileHeight) - (winHeight * tileHeight);
                }
            }
            else
            {
                if (winy > worldHeight * (cellHeight * tileHeight))
                {
                    winy = (worldHeight * (cellHeight * tileHeight)) - winHeight * tileHeight;
                }
            }
            interWinX = winx;
            interWinY = winy;
        }
        /// <summary>
        /// Shifts the window
        /// </summary>
        /// <param name="x">X shift value</param>
        /// <param name="y">Y shift value</param>
        public void shiftWindow(int x, int y)
        {
            winx += x;
            winy += y;
        }
        /// <summary>
        /// centers the window on the player avatar
        /// </summary>
        public void fallowAvatar()
        {
            centerWindow((int)avatar.X, (int)avatar.Y);
        }
        /// <summary>
        /// Checks if a cell is in the bounds of the world
        /// </summary>
        /// <param name="cx">Tile x value</param>
        /// <param name="cy">Tile y value</param>
        /// <returns>Boolean</returns>
        public bool inWorld(int cx, int cy)
        {
            if(cx >= 0 && cx < worldWidth)
            {
                if (cy >= 0 && cy < worldHeight)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Updates world MapGraph
        /// </summary>
        public void updateWorldMapGraph()
        {
            shiftRight = 0;
            shiftDown = 0;
            mg.setAllTrue();
            switch (currentQuadrent)
            {
                case 1:
                    //calculate mgLeft based on alpha cell's leftSide
                    mgLeft = alpha.CellLeftSide - (cellWidth * tileWidth);
                    shiftRight = cellWidth;
                    if(mgLeft < 0)
                    {
                        mgLeft = 0;
                    }
                    //calculate mgTop based on alpha cell's topSide
                    mgTop = alpha.CellTopSide - (cellHeight * tileHeight);
                    shiftDown = cellHeight;
                    if (mgTop < 0)
                    {
                        mgTop = 0;
                    }
                    updateWorldMGQ1();
                    break;
                case 2:
                    //calculate mgLeft based on alpha cell's leftSide
                    mgLeft = alpha.CellLeftSide;
                    shiftRight = 0;
                    if (mgLeft < 0)
                    {
                        mgLeft = 0;
                    }
                    //calculate mgTop based on alpha cell's topSide
                    mgTop = alpha.CellTopSide - (cellHeight * tileHeight);
                    shiftDown = cellHeight;
                    if (mgTop < 0)
                    {
                        mgTop = 0;
                    }
                    updateWorldMGQ2();
                    break;
                case 3:
                    //calculate mgLeft based on alpha cell's leftSide
                    mgLeft = alpha.CellLeftSide;
                    shiftRight = 0;
                    if (mgLeft < 0)
                    {
                        mgLeft = 0;
                    }
                    //calculate mgTop based on alpha cell's topSide
                    mgTop = alpha.CellTopSide;
                    shiftDown = 0;
                    if (mgTop < 0)
                    {
                        mgTop = 0;
                    }
                    updateWorldMGQ3();
                    break;
                case 4:
                    //calculate mgLeft based on alpha cell's leftSide
                    mgLeft = alpha.CellLeftSide - (cellWidth * tileWidth);
                    shiftRight = cellWidth;
                    if (mgLeft < 0)
                    {
                        mgLeft = 0;
                    }
                    //calculate mgTop based on alpha cell's topSide
                    mgTop = alpha.CellTopSide;
                    shiftDown = 0;
                    if (mgTop < 0)
                    {
                        mgTop = 0;
                    }
                    updateWorldMGQ4();
                    break;
            }
            for(int c = 0; c < creatures.Count; c++)
            {
                //x position + mgLeft to get relative leftside of creatures[c]
                //y position + mgTop tp get relative topSide of creatures[c]
                mg.setCell((int)(creatures[c].Left / tileWidth) + shiftRight, (int)(creatures[c].Bottom / tileHeight) + shiftDown, false);
            }
            for (int n = 0; n < npcs.Count; n++)
            {
                //x position - mgLeft to get relative leftside of npcs[c]
                //y position - mgTop tp get relative topSide of npcs[c]
                mg.setCell((int)(npcs[n].Left / tileWidth) + shiftRight, (int)(npcs[n].Bottom / tileHeight) + shiftDown, false);
            }
            if(alpha != null)
            {
                alpha.updateMG(mg, shiftRight, shiftDown, interior);
            }
            if (beta != null)
            {
                beta.updateMG(mg, shiftRight, shiftDown, interior);
            }
            if (delta != null)
            {
                delta.updateMG(mg, shiftRight, shiftDown, interior);
            }
            if (gamma != null)
            {
                gamma.updateMG(mg, shiftRight, shiftDown, interior);
            }
            //discard setting avatar mg for now
            //x position - mgLeft to get relative leftside of avatar
            //y position - mgTop tp get relative topSide of avatar
            //mg.setCell(((int)avatar.X - mgLeft) / tileWidth, ((int)avatar.Bottom - mgTop) / tileHeight, true);
        }
        /// <summary>
        /// Applies the local cell's MapGraphs to world MapGraph 
        /// </summary>
        public void applyCellMGsToWorldMG()
        {
            int hazx = mg.Size.IX / 2;
            int hazy = mg.Size.IY / 2;
            //mg.setAllTrue();
            switch (currentQuadrent)
            {
                case 1:
                    if(alpha != null)
                    {
                        for(int x = hazx; x < mg.Size.IX; x++)
                        {
                            for (int y = hazy; y < mg.Size.IY; y++)
                            {
                                if(alpha.LocalMG.getCell(x - hazx, y - hazy) == true)
                                {
                                    mg.setCell(x, y, true);
                                }
                                else
                                {
                                    mg.setCell(x, y, false);
                                }
                            }
                        }
                    }
                    if(beta != null)
                    {
                        for (int x = 0; x < hazx; x++)
                        {
                            for (int y = hazy; y < mg.Size.IY; y++)
                            {
                                if (beta.LocalMG.getCell(x, y - hazy) == true)
                                {
                                    mg.setCell(x, y, true);
                                }
                                else
                                {
                                    mg.setCell(x, y, false);
                                }
                            }
                        }
                    }
                    if(delta != null)
                    {
                        for (int x = 0; x < hazx; x++)
                        {
                            for (int y = 0; y < hazy; y++)
                            {
                                if (delta.LocalMG.getCell(x, y) == true)
                                {
                                    mg.setCell(x, y, true);
                                }
                                else
                                {
                                    mg.setCell(x, y, false);
                                }
                            }
                        }
                    }
                    if (gamma != null)
                    {
                        for (int x = hazx; x < mg.Size.IX; x++)
                        {
                            for (int y = 0; y < hazy; y++)
                            {
                                if (gamma.LocalMG.getCell(x - hazx, y) == true)
                                {
                                    mg.setCell(x, y, true);
                                }
                                else
                                {
                                    mg.setCell(x, y, false);
                                }
                            }
                        }
                    }
                    break;
                case 2:
                    if (alpha != null)
                    {
                        for (int x = 0; x < hazx; x++)
                        {
                            for (int y = hazy; y < mg.Size.IY; y++)
                            {
                                if (alpha.LocalMG.getCell(x, y - hazy) == true)
                                {
                                    mg.setCell(x, y, true);
                                }
                                else
                                {
                                    mg.setCell(x, y, false);
                                }
                            }
                        }
                    }
                    if (beta != null)
                    {
                        for (int x = 0; x < hazx; x++)
                        {
                            for (int y = 0; y < hazy; y++)
                            {
                                if (beta.LocalMG.getCell(x, y) == true)
                                {
                                    mg.setCell(x, y, true);
                                }
                                else
                                {
                                    mg.setCell(x, y, false);
                                }
                            }
                        }
                    }
                    if (delta != null)
                    {
                        for (int x = hazx; x < mg.Size.IX; x++)
                        {
                            for (int y = 0; y < hazy; y++)
                            {
                                if (delta.LocalMG.getCell(x - hazx, y) == true)
                                {
                                    mg.setCell(x, y, true);
                                }
                                else
                                {
                                    mg.setCell(x, y, false);
                                }
                            }
                        }
                    }
                    if (gamma != null)
                    {
                        for (int x = hazx; x < mg.Size.IX; x++)
                        {
                            for (int y = hazy; y < mg.Size.IY; y++)
                            {
                                if (gamma.LocalMG.getCell(x - hazx, y - hazy) == true)
                                {
                                    mg.setCell(x, y, true);
                                }
                                else
                                {
                                    mg.setCell(x, y, false);
                                }
                            }
                        }
                    }
                    break;
                case 3:
                    if (alpha != null)
                    {
                        for (int x = 0; x < hazx; x++)
                        {
                            for (int y = 0; y < hazy; y++)
                            {
                                if (alpha.LocalMG.getCell(x, y) == true)
                                {
                                    mg.setCell(x, y, true);
                                }
                                else
                                {
                                    mg.setCell(x, y, false);
                                }
                            }
                        }
                    }
                    if (beta != null)
                    {
                        for (int x = hazx; x < mg.Size.IX; x++)
                        {
                            for (int y = 0; y < hazy; y++)
                            {
                                if (beta.LocalMG.getCell(x - hazx, y) == true)
                                {
                                    mg.setCell(x, y, true);
                                }
                                else
                                {
                                    mg.setCell(x, y, false);
                                }
                            }
                        }
                    }
                    if (delta != null)
                    {
                        for (int x = hazx; x < mg.Size.IX; x++)
                        {
                            for (int y = hazy; y < mg.Size.IY; y++)
                            {
                                if (delta.LocalMG.getCell(x - hazx, y - hazy) == true)
                                {
                                    mg.setCell(x, y, true);
                                }
                                else
                                {
                                    mg.setCell(x, y, false);
                                }
                            }
                        }
                    }
                    if (gamma != null)
                    {
                        for (int x = 0; x < hazx; x++)
                        {
                            for (int y = hazy; y < mg.Size.IY; y++)
                            {
                                if (gamma.LocalMG.getCell(x, y - hazy) == true)
                                {
                                    mg.setCell(x, y, true);
                                }
                                else
                                {
                                    mg.setCell(x, y, false);
                                }
                            }
                        }
                    }
                    break;
                case 4:
                    if (alpha != null)
                    {
                        for (int x = hazx; x < mg.Size.IX; x++)
                        {
                            for (int y = 0; y < hazy; y++)
                            {
                                if (alpha.LocalMG.getCell(x - hazx, y) == true)
                                {
                                    mg.setCell(x, y, true);
                                }
                                else
                                {
                                    mg.setCell(x, y, false);
                                }
                            }
                        }
                    }
                    if (beta != null)
                    {
                        for (int x = hazx; x < mg.Size.IX; x++)
                        {
                            for (int y = hazy; y < mg.Size.IY; y++)
                            {
                                if (beta.LocalMG.getCell(x - hazx, y - hazy) == true)
                                {
                                    mg.setCell(x, y, true);
                                }
                                else
                                {
                                    mg.setCell(x, y, false);
                                }
                            }
                        }
                    }
                    if (delta != null)
                    {
                        for (int x = 0; x < hazx; x++)
                        {
                            for (int y = hazy; y < mg.Size.IY; y++)
                            {
                                if (delta.LocalMG.getCell(x, y - hazy) == true)
                                {
                                    mg.setCell(x, y, true);
                                }
                                else
                                {
                                    mg.setCell(x, y, false);
                                }
                            }
                        }
                    }
                    if (gamma != null)
                    {
                        for (int x = 0; x < hazx; x++)
                        {
                            for (int y = 0; y < hazy; y++)
                            {
                                if (gamma.LocalMG.getCell(x, y) == true)
                                {
                                    mg.setCell(x, y, true);
                                }
                                else
                                {
                                    mg.setCell(x, y, false);
                                }
                            }
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Set MapGraph if avatar in cell quadrent 1
        /// </summary>
        public void updateWorldMGQ1()
        {
            //check delta
            if (delta != null)
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = 0; y < cellHeight; y++)
                    {
                        if (delta.isAutoTile(x, y))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = 0; y < cellHeight; y++)
                    {
                        mg.setCell(x, y, false);
                    }
                }
            }
            //check beta
            if (beta != null)
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = cellHeight; y < cellHeight + cellHeight; y++)
                    {
                        if (beta.isAutoTile(x, y - cellHeight))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = cellHeight; y < cellHeight + cellHeight; y++)
                    {
                        mg.setCell(x, y, false);
                    }
                }
            }
            //check gamma
            if (gamma != null)
            {
                for (int x = cellWidth; x < cellWidth + cellWidth; x++)
                {
                    for (int y = 0; y < cellHeight; y++)
                    {
                        if (gamma.isAutoTile(x - cellWidth, y))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            else
            {
                for (int x = cellWidth; x < cellWidth + cellWidth; x++)
                {
                    for (int y = 0; y < cellHeight; y++)
                    {
                        mg.setCell(x, y, false);
                    }
                }
            }
            //check alpha
            if (alpha != null)
            {
                for (int x = cellWidth; x < cellWidth + cellWidth; x++)
                {
                    for (int y = cellHeight; y < cellWidth + cellHeight; y++)
                    {
                        if (alpha.isAutoTile(x - cellWidth, y - cellHeight))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            else
            {
                for (int x = cellWidth; x < cellWidth + cellWidth; x++)
                {
                    for (int y = cellHeight; y < cellHeight + cellHeight; y++)
                    {
                        mg.setCell(x, y, false);
                    }
                }
            }
        }
        /// <summary>
        /// Set MapGraph if avatar in cell quadrent 2
        /// </summary>
        public void updateWorldMGQ2()
        {
            //check beta
            if (beta != null)
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = 0; y < cellHeight; y++)
                    {
                        if (beta.isAutoTile(x, y))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = 0; y < cellHeight; y++)
                    {
                        mg.setCell(x, y, false);
                    }
                }
            }
            //check delta
            if (delta != null)
            {
                for (int x = cellWidth; x < cellWidth + cellWidth; x++)
                {
                    for (int y = 0; y < cellHeight; y++)
                    {
                        if (delta.isAutoTile(x - cellWidth, y))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            else
            {
                for (int x = cellWidth; x < cellWidth + cellWidth; x++)
                {
                    for (int y = 0; y < cellHeight; y++)
                    {
                        mg.setCell(x, y, false);
                    }
                }
            }
            //check gamma
            if (gamma != null)
            {
                for (int x = cellWidth; x < cellWidth + cellWidth; x++)
                {
                    for (int y = cellHeight; y < cellHeight + cellHeight; y++)
                    {
                        if (gamma.isAutoTile(x - cellWidth, y - cellHeight))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            else
            {
                for (int x = cellWidth; x < cellWidth + cellWidth; x++)
                {
                    for (int y = cellHeight; y < cellHeight + cellHeight; y++)
                    {
                        mg.setCell(x, y, false);
                    }
                }
            }
            //check alpha
            if (alpha != null)
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = cellHeight; y < cellHeight + cellHeight; y++)
                    {
                        if (alpha.isAutoTile(x, y - cellHeight))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = cellHeight; y < cellHeight + cellHeight; y++)
                    {
                        mg.setCell(x, y, false);
                    }
                }
            }
        }
        /// <summary>
        /// Set MapGraph if avatar in cell quadrent 3
        /// </summary>
        public void updateWorldMGQ3()
        {
            //check alpha
            if (alpha != null)
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = 0; y < cellHeight; y++)
                    {
                        if (alpha.isAutoTile(x, y))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = 0; y < cellHeight; y++)
                    {
                        mg.setCell(x, y, false);
                    }
                }
            }
            //check beta
            if (beta != null)
            {
                for (int x = cellWidth; x < cellWidth + cellWidth; x++)
                {
                    for (int y = 0; y < cellHeight; y++)
                    {
                        if (beta.isAutoTile(x - cellWidth, y))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            else
            {
                for (int x = cellWidth; x < cellWidth + cellWidth; x++)
                {
                    for (int y = 0; y < cellHeight; y++)
                    {
                        mg.setCell(x, y, false);
                    }
                }
            }
            //check delta
            if (delta != null)
            {
                for (int x = cellWidth; x < cellWidth + cellWidth; x++)
                {
                    for (int y = cellHeight; y < cellHeight + cellHeight; y++)
                    {
                        if (delta.isAutoTile(x - cellWidth, y - cellHeight))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            else
            {
                for (int x = cellWidth; x < cellWidth + cellWidth; x++)
                {
                    for (int y = cellHeight; y < cellHeight + cellHeight; y++)
                    {
                        mg.setCell(x, y, false);
                    }
                }
            }
            //check gamma
            if (gamma != null)
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = cellHeight; y < cellHeight + cellHeight; y++)
                    {
                        if (gamma.isAutoTile(x, y - cellHeight))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = cellHeight; y < cellHeight + cellHeight; y++)
                    {
                        mg.setCell(x, y, false);
                    }
                }
            }
        }
        /// <summary>
        /// Set MapGraph if avatar in cell quadrent 4
        /// </summary>
        public void updateWorldMGQ4()
        {
            //check alpha
            if (alpha != null)
            {
                for (int x = cellWidth; x < cellWidth + cellWidth; x++)
                {
                    for (int y = 0; y < cellHeight; y++)
                    {
                        if (alpha.isAutoTile(x - cellWidth, y))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            else
            {
                for (int x = cellWidth; x < cellWidth + cellWidth; x++)
                {
                    for (int y = 0; y < cellHeight; y++)
                    {
                        mg.setCell(x, y, false);
                    }
                }
            }
            //check beta
            if (beta != null)
            {
                for (int x = cellWidth; x < cellWidth + cellWidth; x++)
                {
                    for (int y = cellHeight; y < cellHeight + cellHeight; y++)
                    {
                        if (beta.isAutoTile(x - cellWidth, y - cellHeight))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            else
            {
                for (int x = cellWidth; x < cellWidth + cellWidth; x++)
                {
                    for (int y = cellHeight; y < cellHeight + cellHeight; y++)
                    {
                        mg.setCell(x, y, false);
                    }
                }
            }
            //check delta
            if (delta != null)
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = cellHeight; y < cellHeight + cellHeight; y++)
                    {
                        if (delta.isAutoTile(x, y - cellHeight))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = cellHeight; y < cellHeight + cellHeight; y++)
                    {
                        mg.setCell(x, y, false);
                    }
                }
            }
            //check gamma
            if (gamma != null)
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = 0; y < cellHeight; y++)
                    {
                        if (gamma.isAutoTile(x, y))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = 0; y < cellWidth; y++)
                    {
                        mg.setCell(x, y, false);
                    }
                }
            }
        }
        /// <summary>
        /// Updates interMG for when player is in an interior cell
        /// </summary>
        public virtual void updateInterMG()
        {
            mg.setAllTrue();
            if (alpha != null)
            {
                for (int x = 0; x < cellWidth; x++)
                {
                    for (int y = 0; y < cellWidth; y++)
                    {
                        if (alpha.isAutoTile(x, y))
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                }
            }
            for (int c = 0; c < creatures.Count; c++)
            {
                mg.setCell((int)(creatures[c].X / tileWidth), (int)(creatures[c].Y / tileHeight), false);
            }
            for (int n = 0; n < npcs.Count; n++)
            {
                mg.setCell((int)(npcs[n].X / tileWidth), (int)(npcs[n].Y / tileHeight), false);
            }
            for(int b = 0; b < alpha.Buildings.Count; b++)
            {
                alpha.Buildings[b].updateMGLower(mg, mgLeft, mgTop, alpha.IsInteriorCell);
            }
            //mg.setCell((int)avatar.X, (int)avatar.Y, true);
        }
        /// <summary>
        /// Updates local MapGraphs
        /// </summary>
        public virtual void updateLocalMG()
        {
            if (alpha != null)
            {
                alpha.updateLocalMG();
            }
            if (beta != null)
            {
                beta.updateLocalMG();
            }
            if (delta != null)
            {
                delta.updateLocalMG();
            }
            if (gamma != null)
            {
                gamma.updateLocalMG();
            }
        }
        /// <summary>
        /// Checks if a point is on screen for interiors and exteriors
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Boolean</returns>
        public virtual bool onScreen(float x, float y)
        {
            if (interior)
            {
                //add two tiles worth of space to create a deadzone around visable space
                if (x >= interX - (2 * tileWidth) && x <= winx + (winWidth * tileWidth) + (2 * tileWidth))
                {
                    if (y >= interY - (2 * tileHeight) && y <= winy + (winHeight * tileHeight) + (2 * tileHeight))
                    {
                        return true;
                    }
                }
            }
            else
            {
                //add two tiles worth of space to create a deadzone around visable space
                if (x >= winx - (2 * tileWidth) && x <= winx + (winWidth * tileWidth) + (2 * tileWidth))
                {
                    if (y >= winy - (2 * tileHeight) && y <= winy + (winHeight * tileHeight) + (2 * tileHeight))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Returns a list of valid points in radius for x and y positions in alpha cell
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="radius">Radius around x, y position</param>
        /// <param name="curve">How shallow or sharp the curve is</param>
        /// <returns>List of Point2D objects</returns>
        public List<Point2D> getArea(int x, int y, int radius, int curve = 1)
        {
            List<Point2D> temp = new List<Point2D>();
            if(radius < 1)
            {
                radius = 1;
            }
            int tempW = curve + 1;
            int txs = x - curve;
            //int tempShift = 0;
            for(int ty = y - radius; ty < y + (radius * 2) + 1; ty++)
            {
                for(int tx = txs; tx < txs + tempW; tx++)
                {
                    if(alpha.Tiles.inDomain(tx, ty))
                    {
                        temp.Add(new Point2D(tx, ty));
                    }
                }
                if(ty < y - 1)
                {
                    txs -= curve;
                    tempW += (curve * 2);
                }
                else if(ty > y + 1)
                {
                    txs += curve;
                    tempW -= (curve * 2);
                }
            }
            return temp;
        }
        /// <summary>
        /// Add a portal to alpha cell
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="tx">Target x position</param>
        /// <param name="ty">Target y position</param>
        /// <param name="cellx">Target cell's x value</param>
        /// <param name="celly">Target cell's y value</param>
        /// <param name="name">Target cell name</param>
        public void addPortal(float x, float y, int cellx, int celly, int tx, int ty, string name, bool toInterior)
        {
            if (alpha != null)
            {
                alpha.Portals.Add(new XenoPortal(TextureBank.getTexture("portal"), x, y, 64, 64, 2, cellx, celly, tx, ty, name, toInterior));
            }
        }
        /// <summary>
        /// Adds a portal to a provided cell
        /// </summary>
        /// <param name="cell">OpenWorldCell reference</param>
        /// <param name="x">Portal x position</param>
        /// <param name="y">Portal y position</param>
        /// <param name="cellx">Portal target cell x value</param>
        /// <param name="celly">Portal target cell y value</param>
        /// <param name="tx">Portal target x value</param>
        /// <param name="ty">Portal target y value</param>
        /// <param name="name">Target cell name</param>
        /// <param name="toInterior">Portal to an interior cell falg value</param>
        public void addPortalToCell(OpenWorldCell cell, float x, float y, int cellx, int celly, int tx, int ty, string name, bool toInterior)
        {
            if (cell != null)
            {
                cell.Portals.Add(new XenoPortal(TextureBank.getTexture("portal"), x, y, 64, 64, 2, cellx, celly, tx, ty, name, toInterior));
            }
        }
        /// <summary>
        /// Checks for avatar interection with portals
        /// </summary>
        public void intersectPortals()
        {
            if(alpha != null)
            {
                alpha.intersectPortal(avatar, this);
            }
        }
        /// <summary>
        /// Transports avatar to new cell
        /// </summary>
        /// <param name="x">Avatar x position</param>
        /// <param name="y">Avatar y position</param>
        /// <param name="cellx">Target's cell x value</param>
        /// <param name="celly">Target's cell y value</param>
        /// <param name="name">Name of cell</param>
        /// <param name="interiorCell">Is interior cell flag value</param>
        public void portalToCell(float x, float y, int cellx, int celly, string name, bool interiorCell)
        {
            if(interiorCell)
            {
                enterExit(name, saveFolderPath, cellx, celly, x, y, true);
            }
            else
            {
                enterExit(name, saveFolderPath, cellx, celly, x, y, false);
            }
        }
        /// <summary>
        /// Returns the sector for a given position
        /// </summary>
        /// <param name="tx">Target X position</param>
        /// <param name="ty">Target Y position</param>
        /// <returns>Intager</returns>
        public int calcSector(float tx, float ty)
        {
            if(alpha != null)
            {
                if(alpha.inCell(tx, ty))
                {
                    return alpha.SG.calculateSector((int)tx - alpha.WorldX, (int)ty - alpha.WorldY);
                }
            }
            if (beta != null)
            {
                if (beta.inCell(tx, ty))
                {
                    return beta.SG.calculateSector((int)tx - beta.WorldX, (int)ty - beta.WorldY);
                }
            }
            if (delta != null)
            {
                if (delta.inCell(tx, ty))
                {
                    return delta.SG.calculateSector((int)tx - delta.WorldX, (int)ty - delta.WorldY);
                }
            }
            if (gamma != null)
            {
                if (gamma.inCell(tx, ty))
                {
                    return gamma.SG.calculateSector((int)tx - gamma.WorldX, (int)ty - gamma.WorldY);
                }
            }
            return -1;//no sector
        }
        /// <summary>
        /// CurrentQuadrent property
        /// </summary>
        public int CurrentQuadrent
        {
            get { return currentQuadrent; }
        }
        /// <summary>
        /// Returns the cell a point is within
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <returns>OpenWorldCell</returns>
        public OpenWorldCell getCurrentCell(float x, float y)
        {
            if(alpha != null)
            {
                if(x >= alpha.CellLeftSide && x <= alpha.CellRightSide)
                {
                    if (y >= alpha.CellTopSide && y <= alpha.CellBottomSide)
                    {
                        return alpha;
                    }
                }
            }
            if (beta != null)
            {
                if (x >= beta.CellLeftSide && x <= beta.CellRightSide)
                {
                    if (y >= beta.CellTopSide && y <= beta.CellBottomSide)
                    {
                        return beta;
                    }
                }
            }
            if (gamma != null)
            {
                if (x >= gamma.CellLeftSide && x <= gamma.CellRightSide)
                {
                    if (y >= gamma.CellTopSide && y <= gamma.CellBottomSide)
                    {
                        return gamma;
                    }
                }
            }
            if (delta != null)
            {
                if (x >= delta.CellLeftSide && x <= delta.CellRightSide)
                {
                    if (y >= delta.CellTopSide && y <= delta.CellBottomSide)
                    {
                        return delta;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Winx property
        /// </summary>
        public int Winx
        {
            get { return winx; }
        }
        /// <summary>
        /// Winy property
        /// </summary>
        public int Winy
        {
            get { return winy; }
        }
        /// <summary>
        /// CellWidth property
        /// </summary>
        public int CellWidth
        {
            get { return cellWidth; }
        }
        /// <summary>
        /// CellHeight property
        /// </summary>
        public int CellHeight
        {
            get { return cellHeight; }
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
        /// WorldWidth property
        /// </summary>
        public int WorldWidth
        {
            get { return worldWidth; }
            //set { worldWidth = value; }
        }
        /// <summary>
        /// WorldHeight property
        /// </summary>
        public int WorldHeight
        {
            get { return worldHeight; }
            //set { worldHeight = value; }
        }
        /// <summary>
        /// WorldWidthPixels property
        /// </summary>
        public int WorldWidthPixels
        {
            get { return worldWidth * cellWidth * tileWidth; }
        }
        /// <summary>
        /// WorldHeightPixels property
        /// </summary>
        public int WorldHeightPixels
        {
            get { return worldHeight * cellHeight * tileHeight; }
            //set { worldHeight = value; }
        }
        /// <summary>
        /// WinWidth property
        /// </summary>
        public int WinWidth
        {
            get { return winWidth; }
        }
        /// <summary>
        /// WinWidthInPixels property
        /// </summary>
        public int WinWidthInPixels
        {
            get { return winWidth * tileWidth; }
        }
        /// <summary>
        /// WinHeight property
        /// </summary>
        public int WinHeight
        {
            get { return winHeight; }
        }
        /// <summary>
        /// WinHeightInPixels property
        /// </summary>
        public int WinHeightInPixels
        {
            get { return winHeight * tileHeight; }
        }
        /// <summary>
        /// InterWinX property
        /// </summary>
        public int InterWinX
        {
            get { return interWinX; }
        }
        /// <summary>
        /// InterWinY property
        /// </summary>
        public int InterWinY
        {
            get { return interWinY; }
        }
        /// <summary>
        /// InterX property
        /// </summary>
        public int InterX
        {
            get { return interX; }
        }
        /// <summary>
        /// InterY property
        /// </summary>
        public int InterY
        {
            get { return interY; }
        }
        /// <summary>
        /// Interior property
        /// </summary>
        public bool Interior
        {
            get { return interior; }
            set { interior = value; }
        }
        /// <summary>
        /// Avatar property
        /// </summary>
        public XenoSprite Avatar
        {
            get { return avatar; }
            set { avatar = value; }
        }
        /// <summary>
        /// Rand proerty
        /// </summary>
        public Random Rand
        {
            get { return rand; }
        }
        /// <summary>
        /// Delay1 property
        /// </summary>
        public Counter Delay1
        {
            get { return delay1; }
        }
        /// <summary>
        /// Delay2 property
        /// </summary>
        public Counter Delay2
        {
            get { return delay2; }
        }
        /// <summary>
        /// Delay3 property
        /// </summary>
        public Counter Delay3
        {
            get { return delay3; }
        }
        /// <summary>
        /// Delay4 property
        /// </summary>
        public Counter Delay4
        {
            get { return delay4; }
        }
        /// <summary>
        /// Delay5 property
        /// </summary>
        public Counter Delay5
        {
            get { return delay5; }
        }
        /// <summary>
        /// Delay6 property
        /// </summary>
        public Counter Delay6
        {
            get { return delay6; }
        }
        /// <summary>
        /// SaveFolderPath property
        /// </summary>
        public string SaveFolderPath
        {
            get { return saveFolderPath; }
            set { saveFolderPath = value; }
        }
        /// <summary>
        /// InterSource property
        /// </summary>
        public Texture2D InterSource
        {
            get { return interSource; }
            set { interSource = value; }
        }
        /// <summary>
        /// InterAutoSource property
        /// </summary>
        public Texture2D InterAutoSource
        {
            get { return interAutoSource; }
            set { interAutoSource = value; }
        }
        /// <summary>
        /// SPF property
        /// </summary>
        public SimplePathFinder SPF
        {
            get { return spf; }
        }
        /// <summary>
        /// NPCs property
        /// </summary>
        public List<XenoSprite> NPCs
        {
            get { return npcs; }
        }
        /// <summary>
        /// Creatures property
        /// </summary>
        public List<XenoSprite> Creatures
        {
            get { return creatures; }
        }
        /// <summary>
        /// MG property
        /// </summary>
        public MapGraph MG
        {
            get { return mg;  }
        }
        /// <summary>
        /// MGLeft property
        /// </summary>
        public int MGLeft
        {
            get { return mgLeft; }
        }
        /// <summary>
        /// MGTop property
        /// </summary>
        public int MGTop
        {
            get { return mgTop; }
        }
        /// <summary>
        /// ShiftRight property
        /// </summary>
        public int ShiftRight
        {
            get { return shiftRight; }
        }
        /// <summary>
        /// ShiftDown property
        /// </summary>
        public int ShiftDown
        {
            get { return shiftDown; }
        }
        /// <summary>
        /// Alpha property
        /// </summary>
        public OpenWorldCell Alpha
        {
            get { return alpha; }
        }
        /// <summary>
        /// Beta property
        /// </summary>
        public OpenWorldCell Beta
        {
            get { return beta; }
        }
        /// <summary>
        /// Delta property
        /// </summary>
        public OpenWorldCell Delta
        {
            get { return delta; }
        }
        /// <summary>
        /// Gamma property
        /// </summary>
        public OpenWorldCell Gamma
        {
            get { return gamma; }
        }
        /// <summary>
        /// Planet property
        /// </summary>
        public string Planet
        {
            get { return planet; }
            set { planet = value; }
        }
        /// <summary>
        /// IsPlanet property
        /// </summary>
        public bool IsPlanet
        {
            get { return isPlanet; }
            set { isPlanet = value; }
        }
    }
}
