//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SDL2;
using XenoLib;

namespace RTSLib
{
    public class SCMiniMap
    {
        //protected
        protected Rectangle box;
        protected Texture2D mapBack;
        protected Point2D pos;
        protected Rectangle win;
        protected float scalex;
        protected float scaley;
        protected RTSWorld world;

        //public
        /// <summary>
        /// SCMiniMap constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="ww">World width in hexes</param>
        /// <param name="wh">World height in hexes</param>
        /// <param name="tileSource">Tile source</param>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="world">RTSWorld reference</param>
        public SCMiniMap(int x, int y, int ww, int wh, Texture2D tileSource, 
            IntPtr renderer, RTSWorld world)
        {
            box = new Rectangle(x, y, 192, 128);
            mapBack = generateMapBack(renderer, tileSource, world);
            pos = new Point2D(0, 0);
            this.world = world;
            win = new Rectangle(0, 0, (ww * 32) * scalex, 
                (wh * 32) * scaley);
        }
        /// <summary>
        /// Generates a mapBackground layer
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="tileSource">Tile source reference</param>
        /// <param name="world">RTSWorld reference</param>
        /// <returns>Texture2D reference</returns>
        public Texture2D generateMapBack(IntPtr renderer, Texture2D tileSource,
            RTSWorld world)
        {
            Rectangle pixel = new Rectangle(0, 0, 32 / scalex, 32 / scaley);
            Rectangle rect = null;
            switch (world.calculateCurrentQuadrent())
            {
                case 1:
                    if (world.Alpha != null)
                    {
                        for (int x = 0; x < world.Alpha.Width; x++)
                        {
                            for (int y = 0; y < world.Alpha.Height; y++)
                            {
                                rect = world.Alpha.Tiles.getTileSrcRect(1, x, y);
                                rect.X = world.Alpha.Width;
                                rect.Y = world.Alpha.Height;
                                SimpleDraw.draw(renderer, tileSource, rect, pixel);
                            }
                        }
                    }
                    if (world.Beta != null)
                    {
                        for (int x = 0; x < world.Beta.Width; x++)
                        {
                            for (int y = 0; y < world.Beta.Height; y++)
                            {
                                rect = world.Beta.Tiles.getTileSrcRect(1, x, y);
                                rect.X = 0;
                                rect.Y = world.Beta.Height;
                                SimpleDraw.draw(renderer, tileSource, rect, pixel);
                            }
                        }
                    }
                    if (world.Delta != null)
                    {
                        for (int x = 0; x < world.Delta.Width; x++)
                        {
                            for (int y = 0; y < world.Delta.Height; y++)
                            {
                                rect = world.Delta.Tiles.getTileSrcRect(1, x, y);
                                rect.X = 0;
                                rect.Y = 0;
                                SimpleDraw.draw(renderer, tileSource, rect, pixel);
                            }
                        }
                    }
                    if (world.Gamma != null)
                    {
                        for (int x = 0; x < world.Gamma.Width; x++)
                        {
                            for (int y = 0; y < world.Gamma.Height; y++)
                            {
                                rect = world.Gamma.Tiles.getTileSrcRect(1, x, y);
                                rect.X = world.Gamma.Width;
                                rect.Y = 0;
                                SimpleDraw.draw(renderer, tileSource, rect, pixel);
                            }
                        }
                    }
                    break;
                case 2:
                    if (world.Alpha != null)
                    {
                        for (int x = 0; x < world.Alpha.Width; x++)
                        {
                            for (int y = 0; y < world.Alpha.Height; y++)
                            {
                                rect = world.Alpha.Tiles.getTileSrcRect(1, x, y);
                                rect.X = 0;
                                rect.Y = world.Alpha.Height;
                                SimpleDraw.draw(renderer, tileSource, rect, pixel);
                            }
                        }
                    }
                    if (world.Beta != null)
                    {
                        for (int x = 0; x < world.Beta.Width; x++)
                        {
                            for (int y = 0; y < world.Beta.Height; y++)
                            {
                                rect = world.Beta.Tiles.getTileSrcRect(1, x, y);
                                rect.X = 0;
                                rect.Y = 0;
                                SimpleDraw.draw(renderer, tileSource, rect, pixel);
                            }
                        }
                    }
                    if (world.Delta != null)
                    {
                        for (int x = 0; x < world.Delta.Width; x++)
                        {
                            for (int y = 0; y < world.Delta.Height; y++)
                            {
                                rect = world.Delta.Tiles.getTileSrcRect(1, x, y);
                                rect.X = world.Delta.Width;
                                rect.Y = 0;
                                SimpleDraw.draw(renderer, tileSource, rect, pixel);
                            }
                        }
                    }
                    if (world.Gamma != null)
                    {
                        for (int x = 0; x < world.Gamma.Width; x++)
                        {
                            for (int y = 0; y < world.Gamma.Height; y++)
                            {
                                rect = world.Gamma.Tiles.getTileSrcRect(1, x, y);
                                rect.X = world.Gamma.Width;
                                rect.Y = world.Gamma.Height;
                                SimpleDraw.draw(renderer, tileSource, rect, pixel);
                            }
                        }
                    }
                    break;
                case 3:
                    if (world.Alpha != null)
                    {
                        for (int x = 0; x < world.Alpha.Width; x++)
                        {
                            for (int y = 0; y < world.Alpha.Height; y++)
                            {
                                rect = world.Alpha.Tiles.getTileSrcRect(1, x, y);
                                rect.X = 0;
                                rect.Y = 0;
                                SimpleDraw.draw(renderer, tileSource, rect, pixel);
                            }
                        }
                    }
                    if (world.Beta != null)
                    {
                        for (int x = 0; x < world.Beta.Width; x++)
                        {
                            for (int y = 0; y < world.Beta.Height; y++)
                            {
                                rect = world.Beta.Tiles.getTileSrcRect(1, x, y);
                                rect.X = world.Beta.Width;
                                rect.Y = 0;
                                SimpleDraw.draw(renderer, tileSource, rect, pixel);
                            }
                        }
                    }
                    if (world.Delta != null)
                    {
                        for (int x = 0; x < world.Delta.Width; x++)
                        {
                            for (int y = 0; y < world.Delta.Height; y++)
                            {
                                rect = world.Delta.Tiles.getTileSrcRect(1, x, y);
                                rect.X = world.Delta.Width;
                                rect.Y = world.Delta.Height;
                                SimpleDraw.draw(renderer, tileSource, rect, pixel);
                            }
                        }
                    }
                    if (world.Gamma != null)
                    {
                        for (int x = 0; x < world.Gamma.Width; x++)
                        {
                            for (int y = 0; y < world.Gamma.Height; y++)
                            {
                                rect = world.Gamma.Tiles.getTileSrcRect(1, x, y);
                                rect.X = 0;
                                rect.Y = world.Gamma.Height;
                                SimpleDraw.draw(renderer, tileSource, rect, pixel);
                            }
                        }
                    }
                    break;
                case 4:
                    if (world.Alpha != null)
                    {
                        for (int x = 0; x < world.Alpha.Width; x++)
                        {
                            for (int y = 0; y < world.Alpha.Height; y++)
                            {
                                rect = world.Alpha.Tiles.getTileSrcRect(1, x, y);
                                rect.X = world.Alpha.Width;
                                rect.Y = 0;
                                SimpleDraw.draw(renderer, tileSource, rect, pixel);
                            }
                        }
                    }
                    if (world.Beta != null)
                    {
                        for (int x = 0; x < world.Beta.Width; x++)
                        {
                            for (int y = 0; y < world.Beta.Height; y++)
                            {
                                rect = world.Beta.Tiles.getTileSrcRect(1, x, y);
                                rect.X = world.Beta.Width;
                                rect.Y = world.Beta.Height;
                                SimpleDraw.draw(renderer, tileSource, rect, pixel);
                            }
                        }
                    }
                    if (world.Delta != null)
                    {
                        for (int x = 0; x < world.Delta.Width; x++)
                        {
                            for (int y = 0; y < world.Delta.Height; y++)
                            {
                                rect = world.Delta.Tiles.getTileSrcRect(1, x, y);
                                rect.X = 0;
                                rect.Y = world.Delta.Height;
                                SimpleDraw.draw(renderer, tileSource, rect, pixel);
                            }
                        }
                    }
                    if (world.Gamma != null)
                    {
                        for (int x = 0; x < world.Gamma.Width; x++)
                        {
                            for (int y = 0; y < world.Gamma.Height; y++)
                            {
                                rect = world.Gamma.Tiles.getTileSrcRect(1, x, y);
                                rect.X = 0;
                                rect.Y = 0;
                                SimpleDraw.draw(renderer, tileSource, rect, pixel);
                            }
                        }
                    }
                    break;
            }
            IntPtr surface = SDL.SDL_CreateRGBSurface(0, world.Alpha.Width, world.Alpha.Height, 0, 0, 0, 0, 1);
            IntPtr tmp = SDL.SDL_CreateTextureFromSurface(renderer, surface);
            Texture2D tex = new Texture2D(tmp, world.Alpha.Width, world.Alpha.Height);
            return tex;
        }
        /// <summary>
        /// Updates MiniMap internal state
        /// </summary>
        /// <param name="pointer">SimplePointer reference</param>
        public void update(SimplePointer pointer)
        {
            if(MouseHandler.getLeft() == true)
            {
                if(box.pointInRect(pointer.Tip) == true)
                {
                    win.X = pointer.X + box.X + (win.Width / 2);
                    pos.X = win.X * scalex;
                    win.Y = pointer.Y + box.Y + (win.Height / 2);
                    pos.Y = win.Y * scaley;
                }
            }
        }
        /// <summary>
        /// Draws MiniMap
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="world">RTSWorld reference</param>
        public void draw(IntPtr renderer, RTSWorld world)
        {
            Rectangle obj = new Rectangle(0, 0, 1, 1);
            SimpleDraw.draw(renderer, mapBack, box);
            switch (world.calculateCurrentQuadrent())
            {
                case 1:
                    if (world.Alpha != null)
                    {
                        for (int x = 0; x < ((RTSCell)world.Alpha).ResourceGrid.Width; x++)
                        {
                            for (int y = 0; y < ((RTSCell)world.Alpha).ResourceGrid.Height; y++)
                            {
                                obj.X = box.X + (((RTSCell)world.Alpha).ResourceGrid.Fields.Grid[x, y].X + world.Alpha.Width) / scalex;
                                obj.Y = box.Y + (((RTSCell)world.Alpha).ResourceGrid.Fields.Grid[x, y].Y + world.Alpha.Height) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                            }
                        }
                    }
                    if (world.Beta != null)
                    {
                        for (int x = 0; x < ((RTSCell)world.Beta).ResourceGrid.Width; x++)
                        {
                            for (int y = 0; y < ((RTSCell)world.Beta).ResourceGrid.Height; y++)
                            {
                                obj.X = box.X + (((RTSCell)world.Beta).ResourceGrid.Fields.Grid[x, y].X) / scalex;
                                obj.Y = box.Y + (((RTSCell)world.Beta).ResourceGrid.Fields.Grid[x, y].Y + world.Beta.Height) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                            }
                        }
                    }
                    if (world.Delta != null)
                    {
                        for (int x = 0; x < ((RTSCell)world.Delta).ResourceGrid.Width; x++)
                        {
                            for (int y = 0; y < ((RTSCell)world.Delta).ResourceGrid.Height; y++)
                            {
                                obj.X = box.X + (((RTSCell)world.Delta).ResourceGrid.Fields.Grid[x, y]).X / scalex;
                                obj.Y = box.Y + (((RTSCell)world.Delta).ResourceGrid.Fields.Grid[x, y].Y) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                            }
                        }
                    }
                    if (world.Gamma != null)
                    {
                        for (int x = 0; x < ((RTSCell)world.Gamma).ResourceGrid.Width; x++)
                        {
                            for (int y = 0; y < ((RTSCell)world.Gamma).ResourceGrid.Height; y++)
                            {
                                obj.X = box.X + (((RTSCell)world.Gamma).ResourceGrid.Fields.Grid[x, y].X + world.Gamma.Width) / scalex;
                                obj.Y = box.Y + (((RTSCell)world.Gamma).ResourceGrid.Fields.Grid[x, y].Y) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                            }
                        }
                    }
                    break;
                case 2:
                    if (world.Alpha != null)
                    {
                        for (int x = 0; x < ((RTSCell)world.Alpha).ResourceGrid.Width; x++)
                        {
                            for (int y = 0; y < ((RTSCell)world.Alpha).ResourceGrid.Height; y++)
                            {
                                obj.X = box.X + (((RTSCell)world.Alpha).ResourceGrid.Fields.Grid[x, y].X) / scalex;
                                obj.Y = box.Y + (((RTSCell)world.Alpha).ResourceGrid.Fields.Grid[x, y].Y + world.Alpha.Height) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                            }
                        }
                    }
                    if (world.Beta != null)
                    {
                        for (int x = 0; x < ((RTSCell)world.Beta).ResourceGrid.Width; x++)
                        {
                            for (int y = 0; y < ((RTSCell)world.Beta).ResourceGrid.Height; y++)
                            {
                                obj.X = box.X + (((RTSCell)world.Beta).ResourceGrid.Fields.Grid[x, y].X) / scalex;
                                obj.Y = box.Y + (((RTSCell)world.Beta).ResourceGrid.Fields.Grid[x, y].Y) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                            }
                        }
                    }
                    if (world.Delta != null)
                    {
                        for (int x = 0; x < ((RTSCell)world.Delta).ResourceGrid.Width; x++)
                        {
                            for (int y = 0; y < ((RTSCell)world.Delta).ResourceGrid.Height; y++)
                            {
                                obj.X = box.X + (((RTSCell)world.Delta).ResourceGrid.Fields.Grid[x, y].X + world.Delta.Width) / scalex;
                                obj.Y = box.Y + (((RTSCell)world.Delta).ResourceGrid.Fields.Grid[x, y].Y) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                            }
                        }
                    }
                    if (world.Gamma != null)
                    {
                        for (int x = 0; x < ((RTSCell)world.Gamma).ResourceGrid.Width; x++)
                        {
                            for (int y = 0; y < ((RTSCell)world.Gamma).ResourceGrid.Height; y++)
                            {
                                obj.X = box.X + (((RTSCell)world.Gamma).ResourceGrid.Fields.Grid[x, y].X + world.Gamma.Width) / scalex;
                                obj.Y = box.Y + (((RTSCell)world.Gamma).ResourceGrid.Fields.Grid[x, y].Y + world.Gamma.Height) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                            }
                        }
                    }
                    break;
                case 3:
                    if (world.Alpha != null)
                    {
                        for (int x = 0; x < ((RTSCell)world.Alpha).ResourceGrid.Width; x++)
                        {
                            for (int y = 0; y < ((RTSCell)world.Alpha).ResourceGrid.Height; y++)
                            {
                                obj.X = box.X + (((RTSCell)world.Alpha).ResourceGrid.Fields.Grid[x, y].X) / scalex;
                                obj.Y = box.Y + (((RTSCell)world.Alpha).ResourceGrid.Fields.Grid[x, y].Y) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                            }
                        }
                    }
                    if (world.Beta != null)
                    {
                        for (int x = 0; x < ((RTSCell)world.Beta).ResourceGrid.Width; x++)
                        {
                            for (int y = 0; y < ((RTSCell)world.Beta).ResourceGrid.Height; y++)
                            {
                                obj.X = box.X + (((RTSCell)world.Beta).ResourceGrid.Fields.Grid[x, y].X + world.Beta.Width) / scalex;
                                obj.Y = box.Y + (((RTSCell)world.Beta).ResourceGrid.Fields.Grid[x, y].Y) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                            }
                        }
                    }
                    if (world.Delta != null)
                    {
                        for (int x = 0; x < ((RTSCell)world.Delta).ResourceGrid.Width; x++)
                        {
                            for (int y = 0; y < ((RTSCell)world.Delta).ResourceGrid.Height; y++)
                            {
                                obj.X = box.X + (((RTSCell)world.Delta).ResourceGrid.Fields.Grid[x, y].X + world.Delta.Width) / scalex;
                                obj.Y = box.Y + (((RTSCell)world.Delta).ResourceGrid.Fields.Grid[x, y].Y + world.Delta.Height) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                            }
                        }
                    }
                    if (world.Gamma != null)
                    {
                        for (int x = 0; x < ((RTSCell)world.Gamma).ResourceGrid.Width; x++)
                        {
                            for (int y = 0; y < ((RTSCell)world.Gamma).ResourceGrid.Height; y++)
                            {
                                obj.X = box.X + (((RTSCell)world.Gamma).ResourceGrid.Fields.Grid[x, y].X + world.Gamma.Width) / scalex;
                                obj.Y = box.Y + (((RTSCell)world.Gamma).ResourceGrid.Fields.Grid[x, y].Y) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                            }
                        }
                    }
                    break;
                case 4:
                    if (world.Alpha != null)
                    {
                        for (int x = 0; x < ((RTSCell)world.Alpha).ResourceGrid.Width; x++)
                        {
                            for (int y = 0; y < ((RTSCell)world.Alpha).ResourceGrid.Height; y++)
                            {
                                obj.X = box.X + (((RTSCell)world.Alpha).ResourceGrid.Fields.Grid[x, y].X + world.Alpha.Width) / scalex;
                                obj.Y = box.Y + (((RTSCell)world.Alpha).ResourceGrid.Fields.Grid[x, y].Y) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                            }
                        }
                    }
                    if (world.Beta != null)
                    {
                        for (int x = 0; x < ((RTSCell)world.Beta).ResourceGrid.Width; x++)
                        {
                            for (int y = 0; y < ((RTSCell)world.Beta).ResourceGrid.Height; y++)
                            {
                                obj.X = box.X + (((RTSCell)world.Beta).ResourceGrid.Fields.Grid[x, y].X + world.Beta.Width) / scalex;
                                obj.Y = box.Y + (((RTSCell)world.Beta).ResourceGrid.Fields.Grid[x, y].Y + world.Beta.Height) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                            }
                        }
                    }
                    if (world.Delta != null)
                    {
                        for (int x = 0; x < ((RTSCell)world.Delta).ResourceGrid.Width; x++)
                        {
                            for (int y = 0; y < ((RTSCell)world.Delta).ResourceGrid.Height; y++)
                            {
                                obj.X = box.X + (((RTSCell)world.Delta).ResourceGrid.Fields.Grid[x, y].X) / scalex;
                                obj.Y = box.Y + (((RTSCell)world.Delta).ResourceGrid.Fields.Grid[x, y].Y + world.Delta.Height) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                            }
                        }
                    }
                    if (world.Gamma != null)
                    {
                        for (int x = 0; x < ((RTSCell)world.Gamma).ResourceGrid.Width; x++)
                        {
                            for (int y = 0; y < ((RTSCell)world.Gamma).ResourceGrid.Height; y++)
                            {
                                obj.X = box.X + (((RTSCell)world.Gamma).ResourceGrid.Fields.Grid[x, y].X) / scalex;
                                obj.Y = box.Y + (((RTSCell)world.Gamma).ResourceGrid.Fields.Grid[x, y].Y + world.Gamma.Height) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                            }
                        }
                    }
                    break;
            }
            for (int i = 0; i < world.Commanders.Count; i++)
            {
                obj.Width = 1;
                obj.Height = 1;
                for (int k = 0; k < world.Commanders[i].Units.Count; k++)
                {
                    obj.X = box.X + (world.Commanders[i].Units[k].X - world.getCurrentCell(world.Avatar.X, world.Avatar.Y).CellX) / scalex;
                    obj.Y = box.Y + (world.Commanders[i].Units[k].Y - world.getCurrentCell(world.Avatar.X, world.Avatar.Y).CellY) / scaley;
                    DrawRects.drawRect(renderer, obj, ColourBank.getColour((XENOCOLOURS)i), true);
                }
                obj.Width = 2;
                obj.Height = 2;
                for (int k = 0; k < world.Commanders[i].Buildings.Count; k++)
                {
                    obj.X = box.X + (world.Commanders[i].Buildings[k].X - world.getCurrentCell(world.Avatar.X, world.Avatar.Y).CellX) / scalex;
                    obj.Y = box.Y + (world.Commanders[i].Buildings[k].Y - world.getCurrentCell(world.Avatar.X, world.Avatar.Y).CellY) / scaley;
                    DrawRects.drawRect(renderer, obj, ColourBank.getColour((XENOCOLOURS)i), true);
                }
            }
            switch(world.calculateCurrentQuadrent())
            {
                case 1:
                    for(int x = 0; x < ((RTSCell)world.Alpha).Fogg.FogGrid.Width; x++)
                    {
                        for(int y = 0; y < ((RTSCell)world.Alpha).Fogg.FogGrid.Height; y++)
                        {
                            if(((RTSCell)world.Alpha).Fogg.FogGrid.Grid[x, y] == true)
                            {
                                obj.X = x + world.Alpha.Width / scalex;
                                obj.Y = y + world.Alpha.Height / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                            }
                        }
                    }
                    for (int x = 0; x < ((RTSCell)world.Beta).Fogg.FogGrid.Width; x++)
                    {
                        for (int y = 0; y < ((RTSCell)world.Beta).Fogg.FogGrid.Height; y++)
                        {
                            if (((RTSCell)world.Beta).Fogg.FogGrid.Grid[x, y] == true)
                            {
                                obj.X = x / scalex;
                                obj.Y = (y + world.Beta.Height) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                            }
                        }
                    }
                    for (int x = 0; x < ((RTSCell)world.Delta).Fogg.FogGrid.Width; x++)
                    {
                        for (int y = 0; y < ((RTSCell)world.Delta).Fogg.FogGrid.Height; y++)
                        {
                            if (((RTSCell)world.Delta).Fogg.FogGrid.Grid[x, y] == true)
                            {
                                obj.X = x / scalex;
                                obj.Y = y / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                            }
                        }
                    }
                    for (int x = 0; x < ((RTSCell)world.Gamma).Fogg.FogGrid.Width; x++)
                    {
                        for (int y = 0; y < ((RTSCell)world.Gamma).Fogg.FogGrid.Height; y++)
                        {
                            if (((RTSCell)world.Gamma).Fogg.FogGrid.Grid[x, y] == true)
                            {
                                obj.X = (x + world.Gamma.Width) / scalex;
                                obj.Y = y / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                            }
                        }
                    }
                    break;
                case 2:
                    for (int x = 0; x < ((RTSCell)world.Alpha).Fogg.FogGrid.Width; x++)
                    {
                        for (int y = 0; y < ((RTSCell)world.Alpha).Fogg.FogGrid.Height; y++)
                        {
                            if (((RTSCell)world.Alpha).Fogg.FogGrid.Grid[x, y] == true)
                            {
                                obj.X = x / scalex;
                                obj.Y = (y + world.Alpha.Height) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                            }
                        }
                    }
                    for (int x = 0; x < ((RTSCell)world.Beta).Fogg.FogGrid.Width; x++)
                    {
                        for (int y = 0; y < ((RTSCell)world.Beta).Fogg.FogGrid.Height; y++)
                        {
                            if (((RTSCell)world.Beta).Fogg.FogGrid.Grid[x, y] == true)
                            {
                                obj.X = x / scalex;
                                obj.Y = y / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                            }
                        }
                    }
                    for (int x = 0; x < ((RTSCell)world.Delta).Fogg.FogGrid.Width; x++)
                    {
                        for (int y = 0; y < ((RTSCell)world.Delta).Fogg.FogGrid.Height; y++)
                        {
                            if (((RTSCell)world.Delta).Fogg.FogGrid.Grid[x, y] == true)
                            {
                                obj.X = (x + world.Delta.Width) / scalex;
                                obj.Y = y / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                            }
                        }
                    }
                    for (int x = 0; x < ((RTSCell)world.Gamma).Fogg.FogGrid.Width; x++)
                    {
                        for (int y = 0; y < ((RTSCell)world.Gamma).Fogg.FogGrid.Height; y++)
                        {
                            if (((RTSCell)world.Gamma).Fogg.FogGrid.Grid[x, y] == true)
                            {
                                obj.X = (x + world.Gamma.Width) / scalex;
                                obj.Y = (y + world.Gamma.Height) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                            }
                        }
                    }
                    break;
                case 3:
                    for (int x = 0; x < ((RTSCell)world.Alpha).Fogg.FogGrid.Width; x++)
                    {
                        for (int y = 0; y < ((RTSCell)world.Alpha).Fogg.FogGrid.Height; y++)
                        {
                            if (((RTSCell)world.Alpha).Fogg.FogGrid.Grid[x, y] == true)
                            {
                                obj.X = x / scalex;
                                obj.Y = y / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                            }
                        }
                    }
                    for (int x = 0; x < ((RTSCell)world.Beta).Fogg.FogGrid.Width; x++)
                    {
                        for (int y = 0; y < ((RTSCell)world.Beta).Fogg.FogGrid.Height; y++)
                        {
                            if (((RTSCell)world.Beta).Fogg.FogGrid.Grid[x, y] == true)
                            {
                                obj.X = (x + world.Beta.Width) / scalex;
                                obj.Y = (y) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                            }
                        }
                    }
                    for (int x = 0; x < ((RTSCell)world.Delta).Fogg.FogGrid.Width; x++)
                    {
                        for (int y = 0; y < ((RTSCell)world.Delta).Fogg.FogGrid.Height; y++)
                        {
                            if (((RTSCell)world.Delta).Fogg.FogGrid.Grid[x, y] == true)
                            {
                                obj.X = (x + world.Delta.Width) / scalex;
                                obj.Y = (y + world.Delta.Height) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                            }
                        }
                    }
                    for (int x = 0; x < ((RTSCell)world.Gamma).Fogg.FogGrid.Width; x++)
                    {
                        for (int y = 0; y < ((RTSCell)world.Gamma).Fogg.FogGrid.Height; y++)
                        {
                            if (((RTSCell)world.Gamma).Fogg.FogGrid.Grid[x, y] == true)
                            {
                                obj.X = x / scalex;
                                obj.Y = (y + world.Gamma.Height) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                            }
                        }
                    }
                    break;
                case 4:
                    for (int x = 0; x < ((RTSCell)world.Alpha).Fogg.FogGrid.Width; x++)
                    {
                        for (int y = 0; y < ((RTSCell)world.Alpha).Fogg.FogGrid.Height; y++)
                        {
                            if (((RTSCell)world.Alpha).Fogg.FogGrid.Grid[x, y] == true)
                            {
                                obj.X = (x + world.Alpha.Width) / scalex;
                                obj.Y = y / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                            }
                        }
                    }
                    for (int x = 0; x < ((RTSCell)world.Beta).Fogg.FogGrid.Width; x++)
                    {
                        for (int y = 0; y < ((RTSCell)world.Beta).Fogg.FogGrid.Height; y++)
                        {
                            if (((RTSCell)world.Beta).Fogg.FogGrid.Grid[x, y] == true)
                            {
                                obj.X = (x + world.Beta.Width) / scalex;
                                obj.Y = (y + world.Beta.Height) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                            }
                        }
                    }
                    for (int x = 0; x < ((RTSCell)world.Delta).Fogg.FogGrid.Width; x++)
                    {
                        for (int y = 0; y < ((RTSCell)world.Delta).Fogg.FogGrid.Height; y++)
                        {
                            if (((RTSCell)world.Delta).Fogg.FogGrid.Grid[x, y] == true)
                            {
                                obj.X = x / scalex;
                                obj.Y = (y + world.Delta.Height) / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                            }
                        }
                    }
                    for (int x = 0; x < ((RTSCell)world.Gamma).Fogg.FogGrid.Width; x++)
                    {
                        for (int y = 0; y < ((RTSCell)world.Gamma).Fogg.FogGrid.Height; y++)
                        {
                            if (((RTSCell)world.Gamma).Fogg.FogGrid.Grid[x, y] == true)
                            {
                                obj.X = x / scalex;
                                obj.Y = y / scaley;
                                DrawRects.drawRect(renderer, obj, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                            }
                        }
                    }
                    break;
            }
            DrawRects.drawRect(renderer, win, ColourBank.getColour(XENOCOLOURS.WHITE), false);
        }
        /// <summary>
        /// Pos property
        /// </summary>
        public Point2D Pos
        {
            get { return pos; }
            set { pos = value; }
        }
        /// <summary>
        /// Box property
        /// </summary>
        public Rectangle Box
        {
            get { return box; }
        }
    }
}
