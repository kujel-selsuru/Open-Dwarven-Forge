﻿//====================================================
//Written by Kujel Selsuru
//Last Updated 22/12/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SDL2;
using XenoLib;

namespace XenoLib
{
    public class RTSCell : OpenWorldCell2
    {
        //protected
        protected SCTerrainMap tm;
        protected SCRTSResourceField resourceGrid;
        protected SCFog fog;
        protected Point2D cellWin;
        protected Rectangle winRect;
        protected DataGrid<bool> occupied;
        protected List<XenoSprite> solids;
        protected List<XenoSprite> sprites;
        protected List<SCRTSCommander> localCommanders;
        protected List<SCScript> scripts;
        protected List<Rectangle> triggerRects;
        protected DataGrid<SCRTSDoodad> doodadLayer2;

        //public

        public RTSCell(Texture2D tileSrc, Texture2D autoTileSrc, int cellx, int celly, string name, int cellWinW = 21, 
            int cellWinH = 18, int tileWidth = 300, int tileHeight = 300) :
            base(tileSrc, autoTileSrc, tileWidth, tileHeight, 32, 32, cellWinW, cellWinH, 0, 0, cellx, celly, name)
        {
            tm = new SCTerrainMap(tileWidth, tileHeight);
            resourceGrid = new SCRTSResourceField(tileWidth, tileHeight);
            fog = new SCFog("fog", 32, 32, tileWidth, tileHeight, 0, 0, 23, 20);
            cellWin = new Point2D(0, 0);
            winRect = new Rectangle(0, 0, cellWinW * 32, cellWinH * 32);
            occupied = new DataGrid<bool>(tileWidth, tileHeight);
            solids = new List<XenoSprite>();
            sprites = new List<XenoSprite>();
            localCommanders = new List<SCRTSCommander>();
            scripts = new List<SCScript>();
            triggerRects = new List<Rectangle>();
            doodadLayer2 = new DataGrid<SCRTSDoodad>(tileWidth, tileHeight);
        }
        /// <summary>
        /// RTSCell copy constructor
        /// </summary>
        /// <param name="obj">RTSCell reference</param>
        public RTSCell(RTSCell obj)
        {
            tm = new SCTerrainMap(obj.Tiles.TileWidth, obj.Tiles.TileHeight);
            resourceGrid = new SCRTSResourceField(obj.Tiles.TileWidth, obj.Tiles.TileHeight);
            fog = new SCFog(obj.fog);
            cellWin = new Point2D(0, 0);
            winRect = new Rectangle(0, 0, obj.WinWidth, obj.WinHeight);
            occupied = new DataGrid<bool>(obj.Occupied.Width, obj.Occupied.Height);
            for(int x = 0; x < obj.Occupied.Width; x++)
            {
                for(int y = 0; y < obj.Occupied.Height; y++)
                {
                    occupied.Grid[x, y] = obj.Occupied.Grid[x, y];
                }
            }
            solids = new List<XenoSprite>();
            for (int i = 0; i < obj.Solids.Count; i++)
            {
                solids.Add(new XenoSprite(obj.Solids[i]));
            }
            sprites = new List<XenoSprite>();
            for (int i = 0; i < obj.Sprites.Count; i++)
            {
                sprites.Add(new XenoSprite(obj.Sprites[i]));
            }
            localCommanders = new List<SCRTSCommander>();
            for (int i = 0; i < obj.LocalCommanders.Count; i++)
            {
                localCommanders.Add(new SCRTSCommander(obj.LocalCommanders[i]));
            }
            scripts = new List<SCScript>();
            for(int i = 0; i < obj.Scripts.Count; i++)
            {
                scripts.Add(new SCScript(obj.Scripts[i]));
            }
            triggerRects = new List<Rectangle>();
            for(int i = 0; i < obj.triggerRects.Count; i++)
            {
                triggerRects.Add(new Rectangle(obj.triggerRects[i].X, obj.triggerRects[i].Y, 
                    obj.triggerRects[i].Width, obj.triggerRects[i].Height));
            }
            doodadLayer2 = new DataGrid<SCRTSDoodad>(obj.DoodadLayer2.Width, obj.DoodadLayer2.Height);
            for(int x = 0; x < obj.DoodadLayer2.Width; x++)
            {
                for(int y = 0; y < obj.DoodadLayer2.Height; y++)
                {
                    if(obj.DoodadLayer2.Grid[x, y] == null)
                    {
                        doodadLayer2.Grid[x, y] = null;
                    }
                    else
                    {
                        doodadLayer2.Grid[x, y] = new SCRTSDoodad(obj.DoodadLayer2.Grid[x, y]);
                    }
                }
            }
        }
        /// <summary>
        /// RTSCell from file constructor
        /// </summary>
        /// <param name="tileSrc">Tile source Texture2D reference</param>
        /// <param name="autoTileSrc">Auto tile source Texture2D reference</param>
        /// <param name="winWidth">Window width in tiles</param>
        /// <param name="winHeight">Window height in tiles</param>
        /// <param name="sr">StreamReader reference</param>
        public RTSCell(Texture2D tileSrc, Texture2D autoTileSrc, int winWidth, int winHeight, StreamReader sr) :
            base(tileSrc, autoTileSrc, winWidth, winHeight, sr)
        {
            string buffer = "";
            string buffer2 = "";
            string buffer3 = "";
            string buffer4 = "";
            int num = 0;
            buffer = sr.ReadLine();
            tm = new SCTerrainMap(sr);
            buffer = sr.ReadLine();
            resourceGrid = new SCRTSResourceField(sr);
            buffer = sr.ReadLine();
            fog = new SCFog(sr);
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            buffer2 = sr.ReadLine();
            cellWin = new Point2D(Convert.ToInt32(buffer), Convert.ToInt32(buffer2));
            buffer = sr.ReadLine();
            buffer2 = sr.ReadLine();
            buffer3 = sr.ReadLine();
            buffer4 = sr.ReadLine();
            winRect = new Rectangle(Convert.ToInt32(buffer), Convert.ToInt32(buffer2), Convert.ToInt32(buffer3), Convert.ToInt32(buffer4));
            buffer = sr.ReadLine();
            buffer2 = sr.ReadLine();
            occupied = new DataGrid<bool>(Convert.ToInt32(buffer), Convert.ToInt32(buffer2));
            /*
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            buffer2 = sr.ReadLine();
            occupied = new DataGrid<bool>(Convert.ToInt32(buffer), Convert.ToInt32(buffer2));
            for (int x = 0; x < occupied.Width; x++)
            {
                for (int y = 0; y < occupied.Height; y++)
                {
                    buffer = sr.ReadLine();
                    if (buffer == "TRUE")
                    {
                        occupied.Grid[x, y] = true;
                    }
                    else
                    {
                        occupied.Grid[x, y] = false;
                    }
                }
            }
            */
            solids = new List<XenoSprite>();
            doodadLayer = new DataGrid<XenoDoodad>(1, 1);
            /*
            buffer = sr.ReadLine();
            solids = new List<XenoSprite>();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            for (int s = 0; s < num; s++)
            {
                solids.Add(new SCRTSDoodad(sr));
            }
            //used for backwards compatability but not used for RTS games
            int xx = Convert.ToInt32(sr.ReadLine());
            int yy = Convert.ToInt32(sr.ReadLine());
            doodadLayer = new DataGrid<XenoDoodad>(xx, yy);
            for(int x = 0; x < xx; x++)
            {
                for(int y = 0; y < yy; y++)
                {
                    buffer = sr.ReadLine();
                    if(buffer == "obj")
                    {
                        doodadLayer.Grid[x, y] = new XenoDoodad(sr);
                    }
                    else
                    {
                        doodadLayer.Grid[x, y] = null;
                    }
                }
            }
            */
            buffer = sr.ReadLine();
            //Console.WriteLine(buffer);
            int xx = Convert.ToInt32(sr.ReadLine());
            int yy = Convert.ToInt32(sr.ReadLine());
            doodadLayer2 = new DataGrid<SCRTSDoodad>(xx, yy);
            /*
            for(int x = 0; x < xx; x++)
            {
                for (int y = 0; y < yy; y++)
                {
                    buffer = sr.ReadLine();
                    if(buffer == "OBJ")
                    {
                        Console.WriteLine(buffer);
                        SCRTSDoodad dood = new SCRTSDoodad(sr);
                        doodadLayer2.Grid[x, y] = dood;
                    }
                    else
                    {
                        //sr.ReadLine();
                        Console.WriteLine(buffer);
                        doodadLayer2.Grid[x, y] = null;
                    }
                }
            }
            */
            buffer = sr.ReadLine();
            do
            {
                if(buffer == "OBJ")
                {
                    SCRTSDoodad dood = new SCRTSDoodad(sr);
                    int dx = dood.IX / tiles.TileWidth;
                    int dy = ((dood.IY + tiles.TileHeight) / tiles.TileHeight);
                    if(doodadLayer2.inDomain(dx, dy) == true)
                    {
                        doodadLayer2.Grid[dx, dy] = dood;
                    }
                    
                }
                buffer = sr.ReadLine();
            } while(buffer != "======LocalCommanders Data======");
            sprites = new List<XenoSprite>();
            buffer = sr.ReadLine();
            num = Convert.ToInt32(buffer);
            localCommanders = new List<SCRTSCommander>();
            for(int c = 0; c < num; c++)
            {
                localCommanders.Add(new SCRTSCommander(sr));
            }
            sr.ReadLine();
            num = Convert.ToInt32(sr.ReadLine());
            scripts = new List<SCScript>();
            buffer = "";
            for(int s = 0; s < num; s++)
            {
                buffer = sr.ReadLine();
                scripts.Add(new SCScript(buffer));
            }
            sr.ReadLine();
            num = Convert.ToInt32(sr.ReadLine());
            triggerRects = new List<Rectangle>();
            for (int s = 0; s < num; s++)
            {
                triggerRects.Add(new Rectangle(Convert.ToInt32(sr.ReadLine()), Convert.ToInt32(sr.ReadLine()), 
                    Convert.ToInt32(sr.ReadLine()), Convert.ToInt32(sr.ReadLine())));
            }
            /*
            buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            buffer2 = sr.ReadLine();
            doodadLayer = new DataGrid<SCRTSDoodad>(Convert.ToInt32(buffer), Convert.ToInt32(buffer2));
            for (int x = 0; x < doodadLayer.Width; x++)
            {
                for (int y = 0; y < doodadLayer.Height; y++)
                {
                    buffer = sr.ReadLine();
                    if (buffer == "NULL")
                    {
                        doodadLayer.Grid[x, y] = null;
                    }
                    else
                    {
                        doodadLayer.Grid[x, y] = new SCRTSDoodad(sr);
                    }
                }
            }
            */
        }
        /// <summary>
        /// RTSCell saveData
        /// </summary>
        /// <param name="filePath">File path of save folder</param>
        /// <param name="cellName">Cell name</param>
        /// <param name="saveAuto">Auto save flag value</param>
        public override void saveData(string filePath, string cellName = "", bool saveAuto = false)
        {
            if(cellName == "")
            {
                filePath += "cell_" + cellx + "_" + celly + ".rtsc";
            }
            else
            {
                filePath += cellName + ".rtsc";
            }
            StreamWriter sw = new StreamWriter(filePath);
            base.saveData(sw, false);
            sw.WriteLine("======TerrainMap Data======");
            tm.saveData(sw);
            sw.WriteLine("======Resource Data======");
            resourceGrid.saveData(sw);
            sw.WriteLine("======Fog Data======");
            fog.saveData(sw);
            sw.WriteLine("======CellWin Data======");
            sw.WriteLine(cellWin.IX);
            sw.WriteLine(cellWin.IY);
            sw.WriteLine(winRect.IX);
            sw.WriteLine(winRect.IY);
            sw.WriteLine((int)winRect.Width);
            sw.WriteLine((int)winRect.Height);
            sw.WriteLine(occupied.Width);
            sw.WriteLine(occupied.Height);
            /*
            sw.WriteLine("======Occupied Data======");
            sw.WriteLine(occupied.Width);
            sw.WriteLine(occupied.Height);
            for(int x = 0; x < occupied.Width; x++)
            {
                for(int y = 0; y < occupied.Height; y++)
                {
                    if(occupied.dataAt(x, y) == true)
                    {
                        sw.WriteLine("TRUE");
                    }
                    else
                    {
                        sw.WriteLine("FALSE");
                    }
                }
            }
            */
            /*
            sw.WriteLine("======Solids Data======");
            sw.WriteLine(solids.Count);
            for(int s = 0; s < solids.Count; s++)
            {
                ((SCRTSDoodad)solids[s]).saveData(sw);
            }
            //used for backwards compatibility but not used in RTS games
            sw.WriteLine(doodadLayer.Width);
            sw.WriteLine(doodadLayer.Height);
            for(int x = 0; x < doodadLayer.Width; x++)
            {
                for(int y = 0; y < doodadLayer.Height; y++)
                {
                    if(doodadLayer.Grid[x, y] != null)
                    {
                        sw.WriteLine("obj");
                        doodadLayer.Grid[x, y].saveData(sw);
                    }
                    else
                    {
                        sw.WriteLine("NULL");
                    }
                }
            }
            */
            sw.WriteLine("======DoodadLayer2 Data======");
            sw.WriteLine(doodadLayer2.Width);
            sw.WriteLine(doodadLayer2.Height);
            for(int x = 0; x < doodadLayer2.Width; x++)
            {
                for(int y = 0; y < doodadLayer2.Height; y++)
                {
                    if(doodadLayer2.Grid[x, y] != null)
                    {
                        sw.WriteLine("OBJ");
                        doodadLayer2.Grid[x, y].saveData(sw);
                    }
                    else
                    {
                        sw.WriteLine("NULL");
                    }
                }
            }
            sw.WriteLine("======LocalCommanders Data======");
            sw.WriteLine(localCommanders.Count);
            for(int c = 0; c < localCommanders.Count; c++)
            {
                localCommanders[c].saveData(sw);
            }
            sw.WriteLine("======Scripts Data======");
            sw.WriteLine(scripts.Count);
            for(int s = 0; s < scripts.Count; s++)
            {
                sw.WriteLine(scripts[s].ScriptName);
            }
            sw.WriteLine("======Trigger Rectangle Data======");
            sw.WriteLine(triggerRects.Count);
            for (int t = 0; t < triggerRects.Count; t++)
            {
                sw.WriteLine(triggerRects[t].IX.ToString());
                sw.WriteLine(triggerRects[t].IY.ToString());
                sw.WriteLine(((int)triggerRects[t].Width).ToString());
                sw.WriteLine(((int)triggerRects[t].Height).ToString());
            }
            /*
            sw.WriteLine("======DooddadLayer Data======");
            sw.WriteLine(doodadLayer.Width);
            sw.WriteLine(doodadLayer.Height);
            for (int x = 0; x < doodadLayer.Width; x++)
            {
                for (int y = 0; y < doodadLayer.Height; y++)
                {
                    if (doodadLayer.dataAt(x, y) == null)
                    {
                        sw.WriteLine("NULL");
                    }
                    else
                    {
                        doodadLayer.dataAt(x, y).saveData(sw);
                    }
                }
            }
            */
            if (saveAuto)
            {
                sw.Close();
            }
        }
        /// <summary>
        /// Draws cell
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winW">Window width in tiles</param>
        /// <param name="winH">Window height in tiles</param>
        /// <param name="tileW">Tile width in pixels</param>
        /// <param name="tileH">Tile height in pixels</param>
        public void drawCell(IntPtr renderer, int shiftRight = 0, int shiftDown = 0, int winW = 21, int winH = 18, int tileW = 32, int tileH = 32)
        {
            for (int x = 0; x < (cellWin.IX / tileW) + winW; x++)
            {
                for (int y = 0; y < (cellWin.IY / tileH) + winH; y++)
                {
                    if (tiles.Layer1[x, y] != null)
                    {
                        tiles.Layer1[x, y].draw(renderer, cellWin.IX - shiftRight, cellWin.IY - shiftDown);
                    }
                    if (resourceGrid.Fields.Grid[x, y] != null)
                    {
                        resourceGrid.Fields.Grid[x, y].draw(renderer, cellWin.IX - shiftRight, cellWin.IY - shiftDown);
                    }
                }
            }
            sprites.Clear();
            for(int x = (CellWin.IX / tiles.TileWidth); x < (CellWin.IX / tiles.TileWidth) + (WinWidth + 2); x++)
            {
                for(int y = (CellWin.IY / tiles.TileHeight); y < (CellWin.IY / tiles.TileHeight) + WinHeight + 1; y++)
                {
                    if(doodadLayer2.inDomain(x, y) == true)
                    {
                        if(doodadLayer2.Grid[x, y] != null)
                        {
                            sprites.Add(doodadLayer2.Grid[x, y]);
                        }
                    }
                }
            }
            /*
            for (int i = 0; i < solids.Count; i++)
            {
                sprites.Add(solids[i]);
            }
            */
            for (int c = 0; c < localCommanders.Count; c++)
            {
                for (int u = 0; u < localCommanders[c].GroundUnits.Count; u++)
                {
                    if (onScreen(localCommanders[c].GroundUnits[u].IX, localCommanders[c].GroundUnits[u].IY) == true)
                    {
                        sprites.Add(localCommanders[c].GroundUnits[u]);
                    }
                }
                for (int b = 0; b < localCommanders[c].Buildings.Count; b++)
                {
                    if (onScreen(localCommanders[c].Buildings[b].IX, localCommanders[c].Buildings[b].IY) == true)
                    {
                        sprites.Add(localCommanders[c].Buildings[b]);
                    }
                }
            }
            for (int c = 0; c < localCommanders.Count; c++)
            {
                for (int a = 0; c < localCommanders[c].Actions.Count; a++)
                {
                    if (onScreen(localCommanders[c].Actions[a].IX, localCommanders[c].Actions[a].IY) == true)
                    {
                        sprites.Add(localCommanders[c].Actions[a]);
                    }
                }
                for (int p = 0; p < localCommanders[c].Particles.Count; p++)
                {
                    if (onScreen(localCommanders[c].Particles[p].IX, localCommanders[c].Particles[p].IY) == true)
                    {
                        sprites.Add(localCommanders[c].Particles[p]);
                    }
                }
                for (int au = 0; au < localCommanders[c].AirUnits.Count; au++)
                {
                    if (onScreen(localCommanders[c].AirUnits[au].IX, localCommanders[c].AirUnits[au].IY) == true)
                    {
                        sprites.Add(localCommanders[c].AirUnits[au]);
                    }
                }
                for (int ap = 0; ap < localCommanders[c].AirParticles.Count; ap++)
                {
                    if (onScreen(localCommanders[c].AirParticles[ap].IX, localCommanders[c].AirParticles[ap].IY) == true)
                    {
                        sprites.Add(localCommanders[c].AirParticles[ap]);
                    }
                }
            }
        }
        /// <summary>
        /// Adds doodads to DrawQueue (deprecated)
        /// <param name="renderer">Renderer reference</param>
        /// </summary>
        public void drawDoodads(IntPtr renderer)
        {
            //LayeredSpriteRenderer.sprites = solids;
            for(int x = CellWin.IX; x < CellWin.IX + WinWidth; x++)
            {
                for (int y = CellWin.IY; x < CellWin.IY + WinHeight; y++)
                {
                    LayeredSpriteRenderer.sprites.Add(doodadLayer2.Grid[x, y]);
                }
            }
            LayeredSpriteRenderer.renderSprites(renderer, winRect.IX, winRect.IY, (int)winRect.Width, (int)winRect.Height);
            
        }
        /// <summary>
        /// Scrolls cell window
        /// </summary>
        /// <param name="x">X shift in tiles</param>
        /// <param name="y">Y shift in tiles</param>
        public void scrollCellWin(int x, int y)
        {
            if(x < 0)
            {
                if(cellWin.IX - x >= -x)
                { 
                    cellWin.IX += x;
                    winRect.X += x;
                }
            }
            else
            {
                if(cellWin.IX + x <= (tiles.Width * tiles.TileWidth))// + x)
                {
                    cellWin.IX += x;
                    winRect.X += x;
                }
            }
            if(y < 0)
            {
                if((cellWin.IY - y) >= -y)
                {
                    cellWin.IY += y;
                    winRect.Y += y;
                }
            }
            else
            {
                if(cellWin.IY + y <= (tiles.Height * tiles.TileHeight))// + y)
                {
                    cellWin.IY += y;
                    winRect.Y += y;
                }
            }
        }
        /// <summary>
        /// Places a tile at specified position and if on layer 1 sets
        /// the terrain value as well
        /// </summary>
        /// <param name="x">X position in tiles</param>
        /// <param name="y">Y position in tiles</param>
        /// <param name="stamp">SCStamp reference</param>
        public void placeTile(int x, int y, SCStamp stamp)
        {
            tiles.setTile(x, y, stamp.X, stamp.Y);
            tm.setValue(x, y, stamp.TV);
        }
        /// <summary>
        /// Erases a tile in cell at specified position
        /// </summary>
        /// <param name="x">X position in tiles</param>
        /// <param name="y">Y position in tiles</param>
        public void eraseTile(int layer, int x, int y)
        {
            tiles.eraseTile(x, y);
        }
        /// <summary>
        /// Checks if a space is clear
        /// </summary>
        /// <param name="x">X position in tiles</param>
        /// <param name="y">Y position in tiles</param>
        /// <param name="w">Width in tiles</param>
        /// <param name="h">Height in tiles</param>
        /// <returns>Boolean</returns>
        public bool isSpaceClear(int x, int y, int w, int h)
        {
            for(int sx = x; sx < x + w; sx++)
            {
                for (int sy = y; sy < y + h; sy++)
                {
                    if (tiles.inDomain(sx, sy) == true)
                    {
                        if(tm.getValue(sx, sy) == -1)
                        {
                            return false;
                        }
                        if(occupied.Grid[sx, sy] == true)
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
        /// Clear the occupied grid in provided space
        /// </summary>
        /// <param name="x">X position in tiles</param>
        /// <param name="y">Y position in tiles</param>
        /// <param name="w">Width in tiles</param>
        /// <param name="h">Height in tiles</param>
        public void clearSpaces(int x, int y, int w, int h)
        {
            for(int sx = x; sx < x + w; sx++)
            {
                for(int sy = y; sy < y + h; sy++)
                {
                    if(tiles.inDomain(sx, sy) == true)
                    {
                        occupied.Grid[sx, sy] = false;
                    }
                }
            }
        }
        /// <summary>
        /// Returns true if point is in cell window else returns false
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Boolean</returns>
        public bool onScreen(int x, int y)
        {
            if(x >= winRect.X - (tiles.TileWidth * 2) && x <= winRect.X + (winRect.Width + (tiles.TileWidth * 2)))
            {
                if(y >= winRect.Y - (tiles.TileHeight * 2) && y <= winRect.Y + (winRect.Height + (tiles.TileHeight * 2)))
                {
                    return true;
                }
            }
            return false;
            /*
            if(x >= (cellWin.IX - (tiles.TileWidth * 2)) && x <= (cellWin.IX + (WinWidth * tiles.TileWidth) + (tiles.TileWidth * 2)))
            {
                if(y >= (cellWin.IY - (tiles.TileHeight * 2)) && y <= (cellWin.IY + (WinHeight * tiles.TileHeight) + (tiles.TileHeight * 2)))
                {
                    return true;
                }
            }
            return false;
            */
            /*
            if(x >= cellWin.IX + (cellx * Width) && x < (WinWidth + ((cellx + 1) * Width)) * tiles.TileWidth)
            {
                if(y >= cellWin.IY + (celly * Height) && y < (WinHeight + ((celly + 1) * Height)) * tiles.TileHeight)
                {
                    return true;
                }
            }
            return false;
            */
        }
        /// <summary>
        /// Loads script lines from script files but SCScript list must be populated
        /// with empty scripts first
        /// </summary>
        /// <param name="scriptsPath">Script's path</param>
        public void loadScripts(string scriptsPath)
        {
            string scriptName = "";
            string fullPath = "";
            StreamReader sr = null;
            for(int s = 0; s < scripts.Count; s++)
            {
                scriptName = scripts[s].ScriptName;
                fullPath = scriptsPath + "//" + scriptName + ".txt";
                sr = new StreamReader(fullPath);
                int num = Convert.ToInt32(sr.ReadLine());
                for(int l = 0; l < num; l++)
                {
                    scripts[s].Lines.Add(sr.ReadLine());
                }
                sr.Close();
            }
        }
        /// <summary>
        /// Sets the sectors of all soilds in cell
        /// </summary>
        public void setAllSolidSectors()
        {
            for(int s = 0; s < solids.Count - 1; s++)
            {
                solids[s].Sector = sg.calculateSector(solids[s].IX / tiles.TileWidth, solids[s].IY / tiles.TileHeight);
            }
        }
        /// <summary>
        /// Scales the values in the list of points by the tile width and height
        /// </summary>
        /// <param name="points">List of 2D point objects</param>
        public void scaleAllPoints(List<Point2D> points)
        {
            for(int i = 0; i < points.Count; i++)
            {
                points[i].X *= tiles.TileWidth;
                points[i].Y *= tiles.TileHeight;
            }
        }
        /// <summary>
        /// Clears solids in a radius around a point
        /// </summary>
        /// <param name="x">X value in tiles</param>
        /// <param name="y">Y value in tiles</param>
        /// <param name="radius">Radius in tiles</param>
        public void clearSolids(int x, int y, int radius = 3)
        {
            setAllSolidSectors();
            List<Point2D> points = getCircleArea(new Point2D(x, y), radius);
            scaleAllPoints(points);
            int sector = 0;
            for(int p = 0; p < points.Count; p++)
            {
                sector = sg.calculateSector(points[p].IX, points[p].IY);
                for(int s = 0; s < solids.Count; s++)
                {
                    if(solids[s].Sector == sector)
                    {
                        if(solids[s].HitBox.pointInRect(points[p]) == true)
                        {
                            solids.RemoveAt(s);
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Clears doodads in doodadLayer2 in a radius around a point
        /// </summary>
        /// <param name="x">X position in tiles</param>
        /// <param name="y">Y position in tiles</param>
        /// <param name="radius">Radius in tiles</param>
        public void clearRadiusInDoodadLayer2(int x, int y, int radius = 3)
        {
            List<Point2D> points = getCircleArea(new Point2D(x, y), radius);
            for(int i = 0; i < points.Count; i++)
            {
                if(doodadLayer2.inDomain(points[i].IX, points[i].IY) == true)
                {
                    doodadLayer2.Grid[points[i].IX, points[i].IY] = null;
                }
            }
        }
        /// <summary>
        /// Clears resources in resourceGrid in a radius around a point
        /// </summary>
        /// <param name="x">X position in tiles</param>
        /// <param name="y">Y position in tiles</param>
        /// <param name="radius">Radius in tiles</param>
        public void clearRadiusInResourceGrid(int x, int y, int radius = 3)
        {
            List<Point2D> points = getCircleArea(new Point2D(x, y), radius);
            for (int i = 0; i < points.Count; i++)
            {
                if (resourceGrid.Fields.inDomain(points[i].IX, points[i].IY) == true)
                {
                    resourceGrid.Fields.Grid[points[i].IX, points[i].IY] = null;
                }
            }
        }
        /// <summary>
        /// Returns true if no resources in area with one tile radius else returns false
        /// </summary>
        /// <param name="x">X position in tiles</param>
        /// <param name="y">Y position in tiles</param>
        /// <param name="w">Width in tiles</param>
        /// <param name="h">Height in tiles</param>
        /// <returns>Boolean</returns>
        public bool noResources(int x, int y, int w, int h)
        {
            int x2 = x - (w / 2) - 1;
            int y2 = y - (h / 2) - 1;
            int w2 = w + 2;
            int h2 = h + 2;
            for(int xx = x2; xx < x2 + w2; xx++)
            {
                for(int yy = y2; yy < y2 + h2; yy++)
                {
                    if (resourceGrid.Fields.inDomain(xx, yy) == true)
                    {
                        if (resourceGrid.Fields.Grid[xx, yy] != null)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Updates MapGraph for XenoBuildings and XenoDoodads (only sets lower half)
        /// </summary>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="shiftRight">Shift right value</param>
        /// <param name="shiftDown">Shift down value</param>
        /// <param name="interior">Is an interior cell</param>
        public override void updateMGLower(MapGraph mg, int shiftRight, int shiftDown, bool interior)
        {
            for(int b = 0; b < buildings.Count; b++)
            {
                buildings[b].updateMGLower(mg, shiftRight, shiftDown, interior);
            }
            for(int x = 0; x < doodadLayer2.Width; x++)
            {
                for(int y = 0; y < doodadLayer2.Height; y++)
                {
                    if(interior)
                    {
                        if(doodadLayer2.Grid[x, y] != null)
                        {
                            mg.setCell(x, y, false);
                        }
                    }
                    else
                    {
                        if(doodadLayer2.Grid[x, y] != null)
                        {
                            mg.setCell(x + (shiftRight / tiles.Width), y + (shiftDown / tiles.Height), false);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Updates the occupied layer
        /// </summary>
        public void updateOccupied()
        {
            //set all ocupied cells as false
            for(int x = 0; x < occupied.Width; x++)
            {
                for (int y = 0; y < occupied.Height; y++)
                {
                    occupied.Grid[x, y] = false;
                }
            }
            //check all doodads
            for(int x = 0; x < doodadLayer2.Width; x++)
            {
                for(int y = 0; y < doodadLayer2.Height; y++)
                {
                    if(doodadLayer2.Grid[x, y] != null)
                    {
                        if(doodadLayer2.inDomain(doodadLayer2.Grid[x, y].Left / tiles.TileWidth,
                            (doodadLayer2.Grid[x, y].Bottom / tiles.TileHeight)) == true)
                        {
                            occupied.Grid[doodadLayer2.Grid[x, y].Left / tiles.TileWidth,
                                (doodadLayer2.Grid[x, y].Bottom / tiles.TileHeight)] = true;
                        }
                    }
                }
            }
            for(int l = 0; l < localCommanders.Count; l++)
            {
                //check all ground units
                int gx = 0;
                int gy = 0;
                for(int g = 0; g < localCommanders[l].GroundUnits.Count; g++)
                {
                    gx = localCommanders[l].GroundUnits[g].Center.IX / tiles.TileWidth;
                    gy = localCommanders[l].GroundUnits[g].Center.IY / tiles.TileHeight;
                    if(occupied.inDomain(gx, gy) == true)
                    {
                        occupied.Grid[gx, gy] = true;
                    }
                }
                //check all buildings
                int bxs = 0;
                int bys = 0;
                int bw = 0;
                int bh = 0;
                for(int b = 0; b < localCommanders[l].Buildings.Count; b++)
                {
                    bxs = localCommanders[l].Buildings[b].Left / tiles.TileWidth;
                    bw = ((int)localCommanders[l].Buildings[b].W) / tiles.TileWidth;
                    bys = localCommanders[l].Buildings[b].Top / tiles.TileHeight;
                    bh = (((int)localCommanders[l].Buildings[b].H) / tiles.TileHeight) - 1;
                    if (bh >= 2)
                    {
                        bh -= 1;
                    }
                    for(int bx = bxs; bx < bxs + bw; bx++)
                    {
                        for(int by = bys; by < bys + bh; by++)
                        {
                            if(occupied.inDomain(bx, by) == true)
                            {
                                occupied.Grid[bx, by] = true;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Returns a list of all points in the radius with no doodads present
        /// </summary>
        /// <param name="x">X position in cell in pixels</param>
        /// <param name="y">Y position in cell in pixels</param>
        /// <param name="radius">radius in tiles</param>
        /// <returns>List of Point2D objects</returns>
        public List<Point2D> getClearTiles(float x, float y, int radius = 5)
        {
            List<Point2D> points = null;
            points = Helpers<SCRTSDoodad>.getCircleArea<SCRTSDoodad>(doodadLayer2, new Point2D(x / tiles.TileWidth, y / tiles.TileHeight), radius);
            for(int i = 0; i < points.Count; i++)
            {
                if(doodadLayer2.Grid[points[i].IX, points[i].IY] != null)
                {
                    points.RemoveAt(i);
                }
            }
            return points;
        }
        /*
        /// <summary>
        /// Clears doodads from doodadLayer
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>
        public void clearDoodads(int x, int y, int radius = 3)
        {
            List<Point2D> points = getCircleArea(new Point2D(x, y), radius);
            for(int i = 0; i < points.Count - 1; i++)
            {
                if(doodadLayer.Grid[points[i].IX, points[i].IY] != null)
                {
                    doodadLayer.Grid[points[i].IX, points[i].IY] = null;
                }
            }
        }
        */
        /// <summary>
        /// TerrainMap property
        /// </summary>
        public SCTerrainMap TM
        {
            get { return tm; }
            set { tm = value; }
        }
        /// <summary>
        /// SCResourceField property
        /// </summary>
        public SCRTSResourceField ResourceGrid
        {
            get { return resourceGrid; }
        }
        /// <summary>
        /// SCFog property
        /// </summary>
        public SCFog Fogg
        {
            get { return fog; }
        }
        /// <summary>
        /// CellWin property
        /// </summary>
        public Point2D CellWin
        {
            get { return cellWin; }
            set { cellWin = value; }
        }
        /// <summary>
        /// WinRect property
        /// </summary>
        public Rectangle WinRect
        {
            get { return winRect; }
        }
        /// <summary>
        /// Layer1 property
        /// </summary>
        public XenoTileSys2.XenoTile[,] Layer1
        {
            get { return tiles.Layer1; }
        }
        /// <summary>
        /// Occupied property
        /// </summary>
        public DataGrid<bool> Occupied
        {
            get { return occupied; }
        }
        /// <summary>
        /// Solids property (deprecated, use doodadLayer2)
        /// </summary>
        public List<XenoSprite> Solids
        {
            get { return solids; }
        }
        /// <summary>
        /// DoodadLayer2 property
        /// </summary>
        public DataGrid<SCRTSDoodad> DoodadLayer2
        {
            get { return doodadLayer2; }
        }
        /// <summary>
        /// Sprites prperty
        /// </summary>
        public List<XenoSprite> Sprites
        {
            get { return sprites; }
            set { sprites = value; }
        }
        /// <summary>
        /// LocalCommanders property
        /// </summary>
        public List<SCRTSCommander> LocalCommanders
        {
            get { return localCommanders; }
            set { localCommanders = value; }
        }
        /// <summary>
        /// Scripts property
        /// </summary>
        public List<SCScript> Scripts
        {
            get { return scripts; }
        }
    }

    public class RTSWorld : OpenWorld2
    {
        //protected
        protected Rectangle window;
        protected List<SCRTSCommander> commanders;
        protected GenericBank<SCRTSCommander> commanderDB;
        protected GenericBank<SCRTSResource> resourceDB;
        protected GenericBank<SCRTSDoodad> doodadDB;
        protected List<string> scriptNames;
        protected DataGrid<bool> occupiedGrid;
        protected List<XenoSprite> renderList;

        //public

        public RTSWorld(Texture2D tileSrc, Texture2D autoTileSrc, int tileWidth = 32, int tileHeight = 32, 
            int cellWidth = 300, int cellHeight = 300, int worldWidth = 300, int worldHeight = 300) : 
            base(tileSrc, autoTileSrc, cellWidth, cellHeight, 0, 0, 23, 20, tileWidth, tileHeight, 
                worldWidth, worldHeight)
        {
            window = new Rectangle(0, 0, winWidth, winHeight);
            commanders = new List<SCRTSCommander>();
            commanderDB = new GenericBank<SCRTSCommander>();
            resourceDB = new GenericBank<SCRTSResource>();
            doodadDB = new GenericBank<SCRTSDoodad>();
            scriptNames = new List<string>();
            occupiedGrid = new DataGrid<bool>(cellWidth* 2, cellHeight * 2);
            renderList = new List<XenoSprite>();
        }
        /// <summary>
        /// RTSWorld copy constructor
        /// </summary>
        /// <param name="obj">RTSWorld reference</param>
        public RTSWorld(RTSWorld obj) : base(obj)
        {
            window = new Rectangle(obj.window.X, obj.window.Y, obj.window.Width, obj.window.Height);
            commanders = new List<SCRTSCommander>();
            for(int i = 0; i < obj.Commanders.Count; i++)
            {
                commanders.Add( new SCRTSCommander(obj.Commanders[i]));
            }
            commanderDB = new GenericBank<SCRTSCommander>();
            for(int i = 0; i < obj.CommanderDB.Names.Count; i++)
            {
                commanderDB.addData(obj.CommanderDB.Names[i], new SCRTSCommander(obj.CommanderDB.getData(i)));
            }
            resourceDB = new GenericBank<SCRTSResource>();
            for(int i = 0; i < obj.ResourceDB.Names.Count; i++)
            {
                resourceDB.addData(obj.ResourceDB.Names[i], new SCRTSResource(obj.ResourceDB.getData(i)));
            }
            doodadDB = new GenericBank<SCRTSDoodad>();
            for (int i = 0; i < obj.DoodadDB.Names.Count; i++)
            {
                doodadDB.addData(obj.DoodadDB.Names[i], new SCRTSDoodad(obj.DoodadDB.getData(i)));
            }
            scriptNames = new List<string>();
            for(int i = 0; i < obj.ScriptNames.Count; i++)
            {
                scriptNames.Add(new string(obj.ScriptNames[i].ToCharArray()));
            }
            occupiedGrid = new DataGrid<bool>(cellWidth * 2, cellHeight * 2);
            for(int x = 0; x < obj.OccupiedGrid.Width; x++)
            {
                for(int y = 0; y < obj.OccupiedGrid.Height; y++)
                {
                    occupiedGrid.Grid[x, y] = obj.OccupiedGrid.Grid[x, y];
                }
            }
            renderList = new List<XenoSprite>();
        }
        /// <summary>
        /// Returns a Point2D object of resource at position
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <Point2D</returns>
        public Point2D resourceAt(int x, int y)
        {
            Point2D tmp = null;
            SCRTSResource res = null;
            if(alpha != null)
            {
                res = ((RTSCell)alpha).ResourceGrid.Fields.Grid[x - (alpha.CellLeftSide / alpha.Tiles.TileWidth), 
                    y - (alpha.CellTopSide / alpha.Tiles.TileHeight)];
                return new Point2D(res.X, res.Y);
            }
            if (beta != null)
            {
                res = ((RTSCell)beta).ResourceGrid.Fields.Grid[x - (beta.CellLeftSide / beta.Tiles.TileWidth),
                    y - (beta.CellTopSide / beta.Tiles.TileHeight)];
                return new Point2D(res.X, res.Y);
            }
            if (delta != null)
            {
                res = ((RTSCell)delta).ResourceGrid.Fields.Grid[x - (delta.CellLeftSide / delta.Tiles.TileWidth),
                    y - (delta.CellTopSide / delta.Tiles.TileHeight)];
                return new Point2D(res.X, res.Y);
            }
            if (gamma != null)
            {
                res = ((RTSCell)gamma).ResourceGrid.Fields.Grid[x - (gamma.CellLeftSide / gamma.Tiles.TileWidth),
                    y - (gamma.CellTopSide / gamma.Tiles.TileHeight)];
                return new Point2D(res.X, res.Y);
            }
            return tmp;
        }
        /// <summary>
        /// Returns a Point2D object for the position of the nearest resource
        /// </summary>
        /// <param name="p">Point of comparison</param>
        /// <returns>Point2D object</returns>
        public Point2D nearestResource(Point2D p)
        {
            SCRTSResource tmp = null;
            SCRTSResource tmp2 = null;
            Point2D pp = null;
            Point2D tp = null;
            Point2D tpp = null;
            if (alpha != null)
            {
                for(int x = 0; x < alpha.Width; x++)
                {
                    for(int y = 0; y < alpha.Height; y++)
                    {
                        tmp2 = ((RTSCell)alpha).ResourceGrid.Fields.Grid[x, y];
                        if(tmp2 != null)
                        {
                            if(tmp == null)
                            {
                                tmp = tmp2;
                            }
                            else
                            {
                                tp = new Point2D(tmp.X, tmp.Y);
                                tpp = new Point2D(tmp2.X, tmp2.Y);
                                if(Point2D.AsqrtB(tp, p) < Point2D.AsqrtB(tpp, p))
                                {
                                    tmp = tmp2;
                                }
                            }
                        }
                    }
                }
            }
            if (beta != null)
            {
                for (int x = 0; x < beta.Width; x++)
                {
                    for (int y = 0; y < beta.Height; y++)
                    {
                        tmp2 = ((RTSCell)beta).ResourceGrid.Fields.Grid[x, y];
                        if (tmp2 != null)
                        {
                            if (tmp == null)
                            {
                                tmp = tmp2;
                            }
                            else
                            {
                                tp = new Point2D(tmp.X, tmp.Y);
                                tpp = new Point2D(tmp2.X, tmp2.Y);
                                if (Point2D.AsqrtB(tp, p) < Point2D.AsqrtB(tpp, p))
                                {
                                    tmp = tmp2;
                                }
                            }
                        }
                    }
                }
            }
            if (delta != null)
            {
                for (int x = 0; x < delta.Width; x++)
                {
                    for (int y = 0; y < delta.Height; y++)
                    {
                        tmp2 = ((RTSCell)delta).ResourceGrid.Fields.Grid[x, y];
                        if (tmp2 != null)
                        {
                            if (tmp == null)
                            {
                                tmp = tmp2;
                            }
                            else
                            {
                                tp = new Point2D(tmp.X, tmp.Y);
                                tpp = new Point2D(tmp2.X, tmp2.Y);
                                if (Point2D.AsqrtB(tp, p) < Point2D.AsqrtB(tpp, p))
                                {
                                    tmp = tmp2;
                                }
                            }
                        }
                    }
                }
            }
            if (gamma != null)
            {
                for (int x = 0; x < gamma.Width; x++)
                {
                    for (int y = 0; y < gamma.Height; y++)
                    {
                        tmp2 = ((RTSCell)gamma).ResourceGrid.Fields.Grid[x, y];
                        if (tmp2 != null)
                        {
                            if (tmp == null)
                            {
                                tmp = tmp2;
                            }
                            else
                            {
                                tp = new Point2D(tmp.X, tmp.Y);
                                tpp = new Point2D(tmp2.X, tmp2.Y);
                                if (Point2D.AsqrtB(tp, p) < Point2D.AsqrtB(tpp, p))
                                {
                                    tmp = tmp2;
                                }
                            }
                        }
                    }
                }
            }
            if (tmp != null)
            {
                pp = new Point2D(tmp.X, tmp.Y);
            }
            return pp;
        }
        /// <summary>
        /// Calculates which quadrent in a cell a point is
        /// </summary>
        /// <param name="cell">RTSCell reference</param>
        /// <param name="x">X position in tiles</param>
        /// <param name="y">Y position in tiles</param>
        /// <returns>Integer</returns>
        public int calculateQuadrent(RTSCell cell, int x, int y)
        {
            if(x > cell.Width)
            {
                if(y > cell.Height)
                {
                    return 3;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                if (y > cell.Height)
                {
                    return 4;
                }
                else
                {
                    return 1;
                }
            }
        }
        /// <summary>
        /// Checks if a space is clear, returns true if clear else false
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width in tiles</param>
        /// <param name="h">Height in tiles</param>
        /// <returns>Boolean</returns>
        public bool checkSpace(float x, float y, int w, int h)
        {
            if(alpha != null)
            {
                if(alpha.inCell(x, y) == true)
                {
                    switch(calculateQuadrent((RTSCell)alpha, (int)x, (int)y))
                    {
                        case 1:
                            for(int i = (int)x; i < (int)x + w; i++)
                            {
                                for(int k = (int)y; k < (int)y + h; k++)
                                {
                                    if(((RTSCell)alpha).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + alpha.Width, k + alpha.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)beta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k + beta.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)delta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)gamma).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + gamma.Width, k) == true)
                                    {
                                        return false;
                                    }
                                }
                            }
                            break;
                        case 2:
                            for(int i = (int)x; i < (int)x + w; i++)
                            {
                                for(int k = (int)y; k < (int)y + h; k++)
                                {
                                    if(((RTSCell)alpha).TM.getValue(i, k) < 0 &&
                                    occupiedGrid.dataAt(i, k + alpha.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)beta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)delta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + delta.Width, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)gamma).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + gamma.Width, k + gamma.Height) == true)
                                    {
                                        return false;
                                    }
                                }
                            }
                            break;
                        case 3:
                            for(int i = (int)x; i < (int)x + w; i++)
                            {
                                for(int k = (int)y; k < (int)y + h; k++)
                                {
                                    if(((RTSCell)alpha).TM.getValue(i, k) < 0 &&
                                    occupiedGrid.dataAt(i, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)beta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + beta.Width, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)delta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + delta.Width, k + delta.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)gamma).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k + gamma.Height) == true)
                                    {
                                        return false;
                                    }
                                }
                            }
                            break;
                        case 4:
                            for(int i = (int)x; i < (int)x + w; i++)
                            {
                                for(int k = (int)y; k < (int)y + h; k++)
                                {
                                    if(((RTSCell)alpha).TM.getValue(i, k) < 0 &&
                                    occupiedGrid.dataAt(i + alpha.Width, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)beta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + beta.Width, k + beta.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)delta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k + delta.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)gamma).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k) == false)
                                    {
                                        return false;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            if(beta != null)
            {
                if(beta.inCell(x, y) == true)
                {
                    switch(calculateQuadrent((RTSCell)beta, (int)x, (int)y))
                    {
                        case 1:
                            for(int i = (int)x; i < (int)x + w; i++)
                            {
                                for(int k = (int)y; k < (int)y + h; k++)
                                {
                                    if(((RTSCell)alpha).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + alpha.Width, k + alpha.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)beta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k + beta.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)delta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)gamma).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + gamma.Width, k) == true)
                                    {
                                        return false;
                                    }
                                }
                            }
                            break;
                        case 2:
                            for(int i = (int)x; i < (int)x + w; i++)
                            {
                                for(int k = (int)y; k < (int)y + h; k++)
                                {
                                    if(((RTSCell)alpha).TM.getValue(i, k) < 0 &&
                                    occupiedGrid.dataAt(i, k + alpha.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)beta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)delta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + delta.Width, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)gamma).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + gamma.Width, k + gamma.Height) == true)
                                    {
                                        return false;
                                    }
                                }
                            }
                            break;
                        case 3:
                            for(int i = (int)x; i < (int)x + w; i++)
                            {
                                for(int k = (int)y; k < (int)y + h; k++)
                                {
                                    if(((RTSCell)alpha).TM.getValue(i, k) < 0 &&
                                    occupiedGrid.dataAt(i, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)beta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + beta.Width, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)delta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + delta.Width, k + delta.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)gamma).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k + gamma.Height) == true)
                                    {
                                        return false;
                                    }
                                }
                            }
                            break;
                        case 4:
                            for(int i = (int)x; i < (int)x + w; i++)
                            {
                                for(int k = (int)y; k < (int)y + h; k++)
                                {
                                    if(((RTSCell)alpha).TM.getValue(i, k) < 0 &&
                                    occupiedGrid.dataAt(i + alpha.Width, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)beta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + beta.Width, k + beta.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)delta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k + delta.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)gamma).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k) == false)
                                    {
                                        return false;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            if(delta != null)
            {
                if(delta.inCell(x, y) == true)
                {
                    switch(calculateQuadrent((RTSCell)delta, (int)x, (int)y))
                    {
                        case 1:
                            for(int i = (int)x; i < (int)x + w; i++)
                            {
                                for(int k = (int)y; k < (int)y + h; k++)
                                {
                                    if(((RTSCell)alpha).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + alpha.Width, k + alpha.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)beta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k + beta.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)delta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)gamma).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + gamma.Width, k) == true)
                                    {
                                        return false;
                                    }
                                }
                            }
                            break;
                        case 2:
                            for(int i = (int)x; i < (int)x + w; i++)
                            {
                                for(int k = (int)y; k < (int)y + h; k++)
                                {
                                    if(((RTSCell)alpha).TM.getValue(i, k) < 0 &&
                                    occupiedGrid.dataAt(i, k + alpha.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)beta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)delta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + delta.Width, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)gamma).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + gamma.Width, k + gamma.Height) == true)
                                    {
                                        return false;
                                    }
                                }
                            }
                            break;
                        case 3:
                            for(int i = (int)x; i < (int)x + w; i++)
                            {
                                for(int k = (int)y; k < (int)y + h; k++)
                                {
                                    if(((RTSCell)alpha).TM.getValue(i, k) < 0 &&
                                    occupiedGrid.dataAt(i, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)beta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + beta.Width, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)delta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + delta.Width, k + delta.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)gamma).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k + gamma.Height) == true)
                                    {
                                        return false;
                                    }
                                }
                            }
                            break;
                        case 4:
                            for(int i = (int)x; i < (int)x + w; i++)
                            {
                                for(int k = (int)y; k < (int)y + h; k++)
                                {
                                    if(((RTSCell)alpha).TM.getValue(i, k) < 0 &&
                                    occupiedGrid.dataAt(i + alpha.Width, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)beta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + beta.Width, k + beta.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)delta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k + delta.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)gamma).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k) == false)
                                    {
                                        return false;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            if(gamma != null)
            {
                if(gamma.inCell(x, y) == true)
                {
                    switch(calculateQuadrent((RTSCell)gamma, (int)x, (int)y))
                    {
                        case 1:
                            for(int i = (int)x; i < (int)x + w; i++)
                            {
                                for(int k = (int)y; k < (int)y + h; k++)
                                {
                                    if(((RTSCell)alpha).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + alpha.Width, k + alpha.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)beta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k + beta.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)delta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)gamma).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + gamma.Width, k) == true)
                                    {
                                        return false;
                                    }
                                }
                            }
                            break;
                        case 2:
                            for(int i = (int)x; i < (int)x + w; i++)
                            {
                                for(int k = (int)y; k < (int)y + h; k++)
                                {
                                    if(((RTSCell)alpha).TM.getValue(i, k) < 0 &&
                                    occupiedGrid.dataAt(i, k + alpha.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)beta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)delta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + delta.Width, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)gamma).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + gamma.Width, k + gamma.Height) == true)
                                    {
                                        return false;
                                    }
                                }
                            }
                            break;
                        case 3:
                            for(int i = (int)x; i < (int)x + w; i++)
                            {
                                for(int k = (int)y; k < (int)y + h; k++)
                                {
                                    if(((RTSCell)alpha).TM.getValue(i, k) < 0 &&
                                    occupiedGrid.dataAt(i, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)beta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + beta.Width, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)delta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + delta.Width, k + delta.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)gamma).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k + gamma.Height) == true)
                                    {
                                        return false;
                                    }
                                }
                            }
                            break;
                        case 4:
                            for(int i = (int)x; i < (int)x + w; i++)
                            {
                                for(int k = (int)y; k < (int)y + h; k++)
                                {
                                    if(((RTSCell)alpha).TM.getValue(i, k) < 0 &&
                                    occupiedGrid.dataAt(i + alpha.Width, k) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)beta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i + beta.Width, k + beta.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)delta).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k + delta.Height) == true)
                                    {
                                        return false;
                                    }
                                    if(((RTSCell)gamma).TM.getValue(i, k) < 0 &&
                                            occupiedGrid.dataAt(i, k) == false)
                                    {
                                        return false;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Calls draw on all commanders
        /// </summary>
        public void drawCommanders()
        {
            for(int c = 0; c < commanders.Count; c++)
            {
                commanders[c].draw(window.IX, window.IY, 20, 17);
            }
        }
        /// <summary>
        /// Draws all objects in DrawQueue
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void drawWorld(IntPtr renderer)
        {
            DrawQueue.drawObjects(renderer);
        }
        /// <summary>
        /// Draws layered sprites such as doodads, ground units and buildings
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="scaler">Scaler value in pixels</param>
        public void drawLayeredSprites(IntPtr renderer, int scaler = 32)
        {
            renderList.Clear();
            if(alpha != null)
            {
                for(int i = 0; i < ((RTSCell)alpha).Solids.Count; i++)
                {
                    renderList.Add(((RTSCell)alpha).Solids[i]);
                }
                
            }
            if(beta != null)
            {
                for(int i = 0; i < ((RTSCell)beta).Solids.Count; i++)
                {
                    renderList.Add(((RTSCell)beta).Solids[i]);
                }

            }
            if(delta != null)
            {
                for(int i = 0; i < ((RTSCell)delta).Solids.Count; i++)
                {
                    renderList.Add(((RTSCell)delta).Solids[i]);
                }

            }
            if(gamma != null)
            {
                for(int i = 0; i < ((RTSCell)gamma).Solids.Count; i++)
                {
                    renderList.Add(((RTSCell)gamma).Solids[i]);
                }
            }
            for(int c = 0; c < commanders.Count; c++)
            {
                for(int u = 0; u < commanders[c].GroundUnits.Count; u++)
                {
                    if(onScreen(commanders[c].GroundUnits[u].IX, commanders[c].GroundUnits[u].IY) == true)
                    {
                        renderList.Add(commanders[c].GroundUnits[u]);
                    }
                }
                for(int b = 0; b < commanders[c].Buildings.Count; b++)
                {
                    if(onScreen(commanders[c].Buildings[b].IX, commanders[c].Buildings[b].IY) == true)
                    {
                        renderList.Add(commanders[c].Buildings[b]);
                    }
                }
            }
            LayeredSpriteRenderer.sprites = renderList;
            LayeredSpriteRenderer.renderSprites(renderer, window.IX, window.IY, (int)window.Width, (int)window.Height, scaler);
        }
        /// <summary>
        /// Draws actions, particles, air units and air particles
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void drawNonlayeredSprites(IntPtr renderer)
        {
            for(int c = 0; c < commanders.Count; c++)
            {
                for(int a = 0; c < commanders[c].Actions.Count; a++)
                {
                    if(onScreen(commanders[c].Actions[a].IX, commanders[c].Actions[a].IY) == true)
                    {
                        commanders[c].Actions[a].draw(renderer, window.IX, window.IY);
                    }
                }
                for(int p = 0; p < commanders[c].Particles.Count; p++)
                {
                    if(onScreen(commanders[c].Particles[p].IX, commanders[c].Particles[p].IY) == true)
                    {
                        commanders[c].Particles[p].draw(renderer, window.IX, window.IY);
                    }
                }
                for(int au = 0; au < commanders[c].AirUnits.Count; au++)
                {
                    if(onScreen(commanders[c].AirUnits[au].IX, commanders[c].AirUnits[au].IY) == true)
                    {
                        commanders[c].AirUnits[au].draw(renderer, window.IX, window.IY);
                    }
                }
                for(int ap = 0; ap < commanders[c].AirParticles.Count; ap++)
                {
                    if(onScreen(commanders[c].AirParticles[ap].IX, commanders[c].AirParticles[ap].IY) == true)
                    {
                        commanders[c].AirParticles[ap].draw(renderer, window.IX, window.IY);
                    }
                }
            }
        }
        /// <summary>
        /// Determins if a point is on world screen
        /// </summary>
        /// <param name="x">X position in pixels</param>
        /// <param name="y">Y position in pixels</param>
        /// <param name="bounds">Screen outer bounds value in pixels</param>
        /// <returns></returns>
        public bool onScreen(int x, int y, int bounds = 64)
        {
            if(x >= (window.IX - bounds) && x <= window.IX + (int)window.Width + bounds)
            {
                if(y >= (window.IY - bounds) && y <= window.IY + (int)window.Height + bounds)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Sets a tile at a position provided the correct values
        /// </summary>
        /// <param name="x">X position in pixels</param>
        /// <param name="y">Y position in pixels</param>
        /// <param name="sx">New SX value in pixels</param>
        /// <param name="sy">New SY value in pixels</param>
        public void setTile(int x, int y, int sx, int sy)
        {
            if(alpha != null)
            {
                if (alpha.inCell(x, y) == true)
                {
                    alpha.Tiles.setTile(x / tileWidth, y / tileHeight, sx, sy);
                }
            }
            else if (beta != null)
            {
                if (beta.inCell(x, y) == true)
                {
                    beta.Tiles.setTile(x / tileWidth, y / tileHeight, sx, sy);
                }
            }
            else if (delta != null)
            {
                if (delta.inCell(x, y) == true)
                {
                    delta.Tiles.setTile(x / tileWidth, y / tileHeight, sx, sy);
                }
            }
            else if (gamma != null)
            {
                if (gamma.inCell(x, y) == true)
                {
                    gamma.Tiles.setTile(x / tileWidth, y / tileHeight, sx, sy);
                }
            }
        }
        /// <summary>
        /// Updates current commanders in cell
        /// </summary>
        public void updateLocalCommanders()
        {
            if(alpha != null)
            {
                ((RTSCell)alpha).LocalCommanders.Clear();
                ((RTSCell)alpha).LocalCommanders.Add(commanders[0]);
                ((RTSCell)alpha).LocalCommanders.Add(commanders[1]);
            }
            if(beta != null)
            {
                ((RTSCell)beta).LocalCommanders.Clear();
                ((RTSCell)beta).LocalCommanders.Add(commanders[0]);
                ((RTSCell)beta).LocalCommanders.Add(commanders[1]);
            }
            if(delta != null)
            {
                ((RTSCell)delta).LocalCommanders.Clear();
                ((RTSCell)delta).LocalCommanders.Add(commanders[0]);
                ((RTSCell)delta).LocalCommanders.Add(commanders[1]);
            }
            if(gamma != null)
            {
                ((RTSCell)gamma).LocalCommanders.Clear();
                ((RTSCell)gamma).LocalCommanders.Add(commanders[0]);
                ((RTSCell)gamma).LocalCommanders.Add(commanders[1]);
            }
        }
        /// <summary>
        /// Updates the occuiped layer in all active cells
        /// </summary>
        public void updateOccuipedLayerInCells()
        {
            if(alpha != null)
            {
                ((RTSCell)alpha).updateOccupied();
            }
            if(beta != null)
            {
                ((RTSCell)beta).updateOccupied();
            }
            if(delta != null)
            {
                ((RTSCell)delta).updateOccupied();
            }
            if(gamma != null)
            {
                ((RTSCell)gamma).updateOccupied();
            }
        }
        /// <summary>
        /// Copies a single cell into alpha from a RTSCell reference
        /// sets it to interior
        /// </summary>
        /// <param name="cell">RTSCell reference</param>
        public void setSingleCell(RTSCell cell)
        {
            alpha = new RTSCell(cell);
            alpha.IsInteriorCell = true;
            beta = null;
            delta = null;
            gamma = null;
        }
        /// <summary>
        /// Commanders property
        /// </summary>
        public List<SCRTSCommander> Commanders
        {
            get { return commanders; }
        }
        /// <summary>
        /// CommanderDB
        /// </summary>
        public GenericBank<SCRTSCommander> CommanderDB
        {
            get { return commanderDB; }
        }
        /// <summary>
        /// ResourceDB
        /// </summary>
        public GenericBank<SCRTSResource> ResourceDB
        {
            get { return resourceDB; }
        }
        /// <summary>
        /// DoodadDB
        /// </summary>
        public GenericBank<SCRTSDoodad> DoodadDB
        {
            get { return doodadDB; }
        }
        /// <summary>
        /// Window property
        /// </summary>
        public Rectangle Window
        {
            get {
                window.X = winx;
                window.Y = winy;
                return window;
                }
        }
        /// <summary>
        /// OccupiedGrid property
        /// </summary>
        public DataGrid<bool> OccupiedGrid
        {
            get { return occupiedGrid; }
        }
        /// <summary>
        /// ScriptNames property
        /// </summary>
        public List<string> ScriptNames
        {
            get { return scriptNames; }
        }
    }
}
