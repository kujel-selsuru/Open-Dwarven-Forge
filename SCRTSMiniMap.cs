//====================================================
//Written by Kujel Selsuru
//Last Updated 04/05/24
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace XenoLib
{
    //DEPRECATED
    public class SCRTSMiniMap
    {
        protected Texture2D pixelbl;
        protected Texture2D pixelg;
        protected Texture2D pixelr;
        protected Texture2D pixelbr;
        protected Texture2D pixelbk;
        protected Texture2D background;
        protected Texture2D background2;
        protected Texture2D background3;
        protected Texture2D background4;
        protected Texture2D midground;
        protected Texture2D midground2;
        protected Texture2D midground3;
        protected Texture2D midground4;
        protected Texture2D foreground;
        protected Texture2D foreground2;
        protected Texture2D foreground3;
        protected Texture2D foreground4;
        protected Rectangle pixelBox;
        protected Rectangle pixelSrcBox;
        protected Rectangle box;
        protected Rectangle mapBox;
        protected Point2D scale;
        //protected int fogScale;
        protected RTSCell cell;
        protected IntPtr tempRenderer;
        protected Rectangle winBox;

        public SCRTSMiniMap(int w, int h, int x, int y, IntPtr renderer, RTSCell cell)
        {
            this.cell = cell;
            //scale = new Vector2((((float)engine.Region.WidthInPixels / w) / engine.Scale) * 2.2f, (((float)engine.Region.HeightInPixels / h) / engine.Scale)* 2.2f);
            scale = new Point2D(w / (float)(cell.Tiles.Width * cell.Tiles.TileWidth), h / (float)(cell.Tiles.Height * cell.Tiles.TileHeight));
            //fogScale = (int)((engine.Fog.PatchWidth * 2) / scale.X);
            pixelBox = new Rectangle(0, 0, 3, 3);
            pixelSrcBox = new Rectangle(0, 0, 32, 32);
            //box = new Rectangle(x, y, (int)(engine.Region.WidthInPixels / scale.X), (int)(engine.Region.HeightInPixels / scale.Y)); 
            box = new Rectangle(x, y, w, h);
            buildBackground(renderer, cell);
            mapBox = new Rectangle(x, y, w, h);
            winBox = new Rectangle(x, y, (cell.WinWidth * cell.Tiles.TileWidth) * scale.X, (cell.WinHeight * cell.Tiles.TileHeight) * scale.Y);
        }
        public void loadPixels()
        {
            pixelbr = TextureBank.getTexture("brown pixel");
            pixelbl = TextureBank.getTexture("blue pixel");
            pixelbk = TextureBank.getTexture("black pixel");
            pixelr = TextureBank.getTexture("red pixel");
            pixelg = TextureBank.getTexture("green pixel");
        }
        public void initRenderer(IntPtr window)
        {
            tempRenderer = SDL.SDL_CreateRenderer(window, 0, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
        }
        public void draw(IntPtr renderer, RTSWorld world, int winx = 0, int winy = 0)
        {
            pixelBox.Width = 3;
            pixelBox.Height = 3;
            buildBackground(renderer, (RTSCell)world.Alpha);
            buildBackground(renderer, (RTSCell)world.Beta);
            buildBackground(renderer, (RTSCell)world.Delta);
            buildBackground(renderer, (RTSCell)world.Gamma);
            if (world.Alpha != null)
            {
                SimpleDraw.draw(renderer, background, box);
            }
            if (world.Alpha != null)
            {
                SimpleDraw.draw(renderer, background2, box);
            }
            if (world.Alpha != null)
            {
                SimpleDraw.draw(renderer, background3, box);
            }
            if (world.Alpha != null)
            {
                SimpleDraw.draw(renderer, background4, box);
            }
            /*
            for (int x = 0; x < cell.ResourceGrid.Width; x++)
            {
                for (int y = 0; y < cell.ResourceGrid.Height; y++)
                {
                    pixelBox.X = (int)x + ((x * scale.X) + box.X);
                    pixelBox.Y = (int)y + ((y * scale.Y) + box.Y);
                    if (cell.ResourceGrid.Fields.Grid[x, y] != null)
                    {
                        DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                    }
                }
            }
            */
            for (int p = 0; p < world.Commanders.Count; p++)
            {
                pixelBox.Width = 3;
                pixelBox.Height = 3;
                for (int a = 0; a < world.Commanders[p].AirUnits.Count; a++)
                {
                    pixelBox.X = (int)((world.Commanders[p].AirUnits[a].X / cell.Tiles.TileWidth) * scale.X) + box.X;
                    pixelBox.Y = (int)((world.Commanders[p].AirUnits[a].Y / cell.Tiles.TileHeight) * scale.Y) + box.Y;
                    if (p == 0)
                    {
                        DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.BLUE), true);
                    }
                    else
                    {
                        DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.RED), true);
                    }
                }
                for(int g = 0; g < world.Commanders[p].GroundUnits.Count; g++)
                {
                    pixelBox.X = (int)((world.Commanders[p].GroundUnits[g].X / cell.Tiles.TileWidth) * scale.X) + box.X;
                    pixelBox.Y = (int)((world.Commanders[p].GroundUnits[g].Y / cell.Tiles.TileHeight) * scale.Y) + box.Y;
                    if (p == 0)
                    {
                        DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.BLUE), true);
                    }
                    else
                    {
                        DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.RED), true);
                    }
                }
                pixelBox.Width = 3;
                pixelBox.Height = 3;
                for(int b = 0; b < world.Commanders[p].Buildings.Count; b++)
                {
                    pixelBox.X = (int)((world.Commanders[p].Buildings[b].X / cell.Tiles.TileWidth) * scale.X) + box.X;
                    pixelBox.Y = (int)((world.Commanders[p].Buildings[b].Y / cell.Tiles.TileHeight) * scale.Y) + box.Y;
                    if (p == 0)
                    {
                        DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.BLUE), true);
                    }
                    else
                    {
                        DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.RED), true);
                    }
                }
            }
            pixelBox.Width = 3;
            pixelBox.Height = 3;
            DrawRects.drawRect(renderer, box, ColourBank.getColour(XENOCOLOURS.WHITE));
        }
        public void draw(IntPtr renderer, RTSCell cell, RTSWorld world, int winx = 0, int winy = 0)
        {
            pixelBox.Width = 3;
            pixelBox.Height = 3;
            //createBackground(renderer, engine);
            for (int x = 0; x < cell.ResourceGrid.Fields.Width; x++)
            {
                for (int y = 0; y < cell.ResourceGrid.Fields.Height; y++)
                {
                    pixelBox.X = (int)x + box.X;//((x * scale.X) + box.X);
                    pixelBox.Y = (int)y + box.Y;//((y * scale.Y) + box.Y);
                    if (cell.ResourceGrid.Fields.Grid[x, y] != null)
                    {
                        //SimpleDraw.draw(renderer, pixelg, pixelBox);
                        DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                    }
                }
            }
            for (int p = 0; p < world.Commanders.Count; p++)
            {
                pixelBox.Width = 3;
                pixelBox.Height = 3;
                for (int a = 0; a < world.Commanders[p].AirUnits.Count; a++)
                {
                    pixelBox.X = (int)((world.Commanders[p].AirUnits[a].X / cell.Tiles.TileWidth) * scale.X) + box.X;
                    pixelBox.Y = (int)((world.Commanders[p].AirUnits[a].Y / cell.Tiles.TileHeight) * scale.Y) + box.Y;
                    if (p == 0)
                    {
                        //SimpleDraw.draw(renderer, pixelg, pixelBox);
                        DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.BLUE), true);
                    }
                    else
                    {
                        //SimpleDraw.draw(renderer, pixelr, pixelBox);
                        DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.RED), true);
                    }
                }
                for (int g = 0; g < world.Commanders[p].GroundUnits.Count; g++)
                {
                    pixelBox.X = (int)((world.Commanders[p].GroundUnits[g].X / cell.Tiles.TileWidth) * scale.X) + box.X;
                    pixelBox.Y = (int)((world.Commanders[p].GroundUnits[g].Y / cell.Tiles.TileHeight) * scale.Y) + box.Y;
                    if (p == 0)
                    {
                        DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.BLUE), true);
                    }
                    else
                    {
                        DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.RED), true);
                    }
                }
                pixelBox.Width = 3;
                pixelBox.Height = 3;
                for (int b = 0; b < world.Commanders[p].Buildings.Count; b++)
                {
                    pixelBox.X = (int)((world.Commanders[p].Buildings[b].X / cell.Tiles.TileWidth) * scale.X) + box.X;
                    pixelBox.Y = (int)((world.Commanders[p].Buildings[b].Y / cell.Tiles.TileHeight) * scale.Y) + box.Y;
                    if (p == 0)
                    {
                        DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.BLUE), true);
                    }
                    else
                    {
                        DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.RED), true);
                    }
                }
            }
            pixelBox.Width = 3;
            pixelBox.Height = 3;
            DrawRects.drawRect(renderer, box, ColourBank.getColour(XENOCOLOURS.WHITE));
        }
        /*
        public void drawWindow(IntPtr renderer, TileInterface TI)
        {
            winBox.X = box.X + ((TI.Window.X * 32) * scale.X);
            winBox.Y = box.Y + ((TI.Window.Y * 32) * scale.Y);
            DrawRects.drawRect(renderer, winBox, ColourBank.getColour(XENOCOLOURS.YELLOW));
        }
        */
        public void drawWindow(IntPtr renderer, RTSWorld world)
        {
            //Rectangle winBox = new Rectangle(box.X + ((engine.Region.Windx * 32) * scale.X), box.Y + ((engine.Region.Windy * 32) * scale.Y),
            //    (engine.Region.WinWidth * 32) * scale.X, (engine.Region.WinHeight * 32) * scale.Y);
            winBox.X = box.X + (((world.Window.X / world.TileWidth) * world.TileWidth) * scale.X);
            winBox.Y = box.Y + (((world.Window.Y / world.TileHeight) * world.TileHeight) * scale.Y);
            DrawRects.drawRect(renderer, winBox, ColourBank.getColour(XENOCOLOURS.YELLOW));
        }
        /*
        public void draw2(IntPtr renderer, TileInterface TI, int winx = 0, int winy = 0)
        {
            pixelBox.Width = 3;
            pixelBox.Height = 3;
            buildBackground2(renderer, TI);
            drawForeground(renderer, TI);
            SimpleDraw.draw(renderer, background, box);
            //SimpleDraw.draw(renderer, foreground, box);
            DrawRects.drawRect(renderer, box, ColourBank.getColour(XENOCOLOURS.WHITE));
        }
        */
        /*
        public void draw2(IntPtr renderer, RTSCell cell, int winx = 0, int winy = 0)
        {
            pixelBox.Width = 3;
            pixelBox.Height = 3;
            buildBackground2(renderer, engine);
            drawFog(renderer, engine);
            SimpleDraw.draw(renderer, foreground, box);
            DrawRects.drawRect(renderer, box, ColourBank.getColour(XENOCOLOURS.WHITE));
        }
        */
        public void click(RTSCell cell, RTSWorld world)
        {
            if (box.pointInRect(new Point2D(MouseHandler.getMouseX(), MouseHandler.getMouseY())) == true)
            {
                if (MouseHandler.getLeft() == true)
                {
                    int x = MouseHandler.getMouseX();
                    int y = MouseHandler.getMouseY();
                    winBox.X = ((x - box.IX) - ((int)winBox.Width / 2));
                    winBox.Y = ((y - box.IY) - ((int)winBox.Height / 2));
                    world.setWindow((int)(((MouseHandler.getMouseX() - mapBox.IX) - (int)(winBox.Width / 2)) * cell.Tiles.TileWidth), (int)((MouseHandler.getMouseY() - mapBox.IY) - (int)(winBox.Height / 2)) * cell.Tiles.TileWidth);
                    /*
                    if (engine.Cur.DestBox.intersects(box))
                    {

                    }
                    */
                }
                if (MouseHandler.getRight() == true)
                {
                    world.Commanders[0].sendSelected((int)(MouseHandler.getMouseX() - mapBox.IX) * cell.Tiles.TileWidth, (int)(MouseHandler.getMouseY() - mapBox.IY) * cell.Tiles.TileWidth);
                }
            }
        }
        /*
        public void click(TileInterface TI)
        {
            if (MouseHandler.getLeft() == true)
            {
                //winBox.X = box.X + (box.IX / winBox.IX);
                //winBox.Y = box.Y + (box.IY / winBox.IY);
                int x = MouseHandler.getMouseX() - box.IX;
                int y = MouseHandler.getMouseY() - box.IY;
                float percentX = (x / box.Width) * ((engine.Region.TileWidth * engine.Region.Width) / engine.Region.TileWidth);
                float percentY = (y / box.Height) * ((engine.Region.TileHeight * engine.Region.Height) / engine.Region.TileHeight);
                TI.setWindow((int)percentX - (engine.Region.WinWidth / 2), (int)percentY - (engine.Region.WinHeight / 2));
                TI.setWinBox((int)percentX - (engine.Region.WinWidth / 2), (int)percentY - (engine.Region.WinHeight / 2));
            }
        }
        */
        public bool intersecting(Rectangle rect)
        {
            if (box.intersects(rect))
            {
                return true;
            }
            return false;
        }
        public void createBackground(IntPtr renderer, RTSCell cell)
        {
            Rectangle srcP = new Rectangle(0, 0, 1, 1);
            Rectangle destP = new Rectangle(0, 0, scale.X, scale.Y);//(box.Width / engine.Region.WidthInPixels) * 32, (box.Height / engine.Region.HeightInPixels) * 32);
            for (int x = 0; x < cell.Width; x++)
            {
                for (int y = 0; y < cell.Height; y++)
                {
                    srcP.X = cell.Tiles.tileAt(x, y, cell.Tiles.TileWidth, cell.Tiles.TileHeight).X * 32 + 16;
                    srcP.Y = cell.Tiles.tileAt(x, y, cell.Tiles.TileWidth, cell.Tiles.TileHeight).Y * 32 + 16;
                    destP.X = box.X + (x * destP.Width);
                    destP.Y = box.Y + (y * destP.Height);
                    if (cell.Tiles.noTile(x, y) == true)
                    {
                        DrawRects.drawRect(renderer, destP, ColourBank.getColour(XENOCOLOURS.BLACK));
                    }
                    else
                    {
                        SimpleDraw.draw(renderer, cell.Tiles.Source, srcP, destP);
                    }
                }
            }
            //SimpleDraw.savePNG(AppDomain.CurrentDomain.BaseDirectory, "map background", (int)box.X + 32, (int)box.Y + 32, (int)box.Width - 32, (int)box.Height - 32);
            //background = TextureLoader.load(AppDomain.CurrentDomain.BaseDirectory + "map background.png", renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), (int)box.Width, (int)box.Height);
        }
        /*
        public void createBackground(IntPtr renderer, RTSCell cell)
        {
            Rectangle srcP = new Rectangle(0, 0, 1, 1);
            Rectangle destP = new Rectangle(0, 0, 3, 3);// scale.X, scale.Y); //(box.Width / TI.WorldDimen.IX), (box.Height / TI.WorldDimen.IY));
            for (int x = 0; x < cell.Width; x++)
            {
                for (int y = 0; y < cell.Height; y++)
                {
                    Point2D pos = cell.Tiles.tileAt(x, y, cell.Tiles.TileWidth, cell.Tiles.TileHeight);
                    destP.X = box.X + (x * destP.Width);
                    destP.Y = box.Y + (y * destP.Height);
                    if (cell.Tiles.tileAt(x, y, cell.Tiles.TileWidth, cell.Tiles.TileHeight) == null)
                    {
                        DrawRects.drawRect(renderer, destP, ColourBank.getColour(XENOCOLOURS.BLACK));
                    }
                    else
                    {
                        srcP.X = pos.X * 32 + 16;
                        srcP.Y = pos.Y * 32 + 16;
                        SimpleDraw.draw(renderer, cell.Tiles.Source, srcP, destP);
                        //DrawRects.drawRect(renderer, destP, ColourBank.getColour(XENOCOLOURS.BROWN), true);
                    }
                }
            }
            //SimpleDraw.savePNG(AppDomain.CurrentDomain.BaseDirectory, "map background", (int)box.X + 32, (int)box.Y + 32, (int)box.Width - 32, (int)box.Height - 32);
            //background = TextureLoader.load(AppDomain.CurrentDomain.BaseDirectory + "map background.png", renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), (int)box.Width, (int)box.Height);
        }
        */
        /*
        public void saveBackground(IntPtr renderer, TileInterface TI)
        {
            unsafe
            {
                uint[] pixels = new UInt32[(int)(box.Width * box.Height)];
                fixed (uint* pArray = pixels)
                {
                    //clears screen
                    SDL.SDL_SetRenderTarget(tempRenderer, (IntPtr)0);
                    SDL.SDL_SetRenderDrawColor(tempRenderer, 0, 0, 0, 1);
                    SDL.SDL_RenderClear(tempRenderer);
                    //renderer update code here
                    IntPtr tex = SDL.SDL_CreateTexture(tempRenderer, SDL.SDL_PIXELFORMAT_ARGB8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STATIC, (int)box.Width, (int)box.Height);
                    SDL.SDL_Rect rect = new SDL.SDL_Rect();
                    rect.x = 0;
                    rect.y = 0;
                    rect.w = (int)box.Width;
                    rect.h = (int)box.Height;
                    SDL.SDL_Rect rect2 = new SDL.SDL_Rect();
                    rect2.x = (int)box.X;
                    rect2.y = (int)box.Y;
                    rect2.w = (int)box.Width;
                    rect2.h = (int)box.Height;
                    //convert pixels to IntPtr 
                    IntPtr ip = new IntPtr((void*)pArray);
                    //Rectangle srcP = new Rectangle(0, 0, 32, 32);
                    //Rectangle destP = new Rectangle(0, 0, 32, 32);
                    for (int x = 0; x < TI.WorldDimen.IX; x++)
                    {
                        for (int y = 0; y < TI.WorldDimen.IY; y++)
                        {
                            if (TI.tileAt(1, x, y, 32, 32) != null)
                            {
                                //destP.X = x * 32;
                                //destP.Y = y * 32;
                                //srcP.X = TI.tileAt(1, x, y, 32, 32).X;
                                //srcP.Y = TI.tileAt(1, x, y, 32, 32).Y;
                                pixels[y * (int)box.Width + x] = 150;
                                //SimpleDraw.draw(renderer, TI.tileSource(x, y), srcP, destP);
                            }
                        }
                    }
                    //SDL.SDL_memcpy(ip, tex, (IntPtr)((int)box.Width * (int)box.Height));
                    SDL.SDL_UpdateTexture(tex, ref rect, ip, (int)(box.Width * box.Height));
                    SDL.SDL_RenderCopy(tempRenderer, tex, ref rect, ref rect2);
                    SDL.SDL_RenderPresent(tempRenderer);
                    //SimpleDraw.savePNG(AppDomain.CurrentDomain.BaseDirectory, "map background", 0, 0, TI.WorldDimen.IX * 32, TI.WorldDimen.IY * 32);
                    //background = TextureLoader.load(AppDomain.CurrentDomain.BaseDirectory + "map background.png", renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), TI.WorldDimen.IX * 32, TI.WorldDimen.IY * 32);
                    SDL.SDL_SetRenderTarget(renderer, (IntPtr)0);
                }
            }
        }
        */
        /*
        public void buildBackground(IntPtr renderer, TileInterface TI)
        {
            Rectangle src = new Rectangle(0, 0, 3, 3);
            Rectangle dest = new Rectangle(0, 0, 4, 4);
            Point2D scale = new Point2D(box.Width / (TI.WorldDimen.X * TI.TW), box.Height / (TI.WorldDimen.Y * TI.TH));
            Point2D inverseScale = new Point2D((TI.WorldDimen.X * TI.TW) / box.Width, (TI.WorldDimen.Y * TI.TH) / box.Height);
            Point2D tileScale = new Point2D(TI.TW / (TI.WorldDimen.X * TI.TW), TI.TH / (TI.WorldDimen.Y * TI.TH));
            Texture2D tileSource = default(Texture2D);
            //DrawRects.drawRect(renderer, box, ColourBank.getColour(XENOCOLOURS.GREEN), true);
            for (int x = 0; x < TI.WorldDimen.IX; x++)
            {
                for (int y = 0; y < TI.WorldDimen.IY; y++)
                {
                    if (TI.tileAt(1, x, y, engine.Region.TileWidth, engine.Region.TileHeight) != null)
                    {
                        tileSource = TI.tileSource(x, y);
                        src.X = (TI.tileAt(1, x, y, engine.Region.TileWidth, engine.Region.TileHeight).X * engine.Region.TileWidth) + (engine.Region.TileWidth / 2) - 2;
                        src.Y = (TI.tileAt(1, x, y, engine.Region.TileWidth, engine.Region.TileHeight).Y * engine.Region.TileHeight) + (engine.Region.TileHeight / 2) - 2;
                        dest.X = box.X + (x * (engine.Region.TileWidth * scale.X));
                        dest.Y = box.Y + (y * (engine.Region.TileHeight * scale.Y));
                        SimpleDraw.draw(renderer, tileSource, src, dest);
                        //SimpleDraw.savePNG(AppDomain.CurrentDomain.BaseDirectory, "map background", box.IX + 32, box.IY + 32, (int)box.Width - 32, (int)box.Width - 64);
                        //background = TextureLoader.load(AppDomain.CurrentDomain.BaseDirectory + "map background.png", renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), (int)box.Width, (int)box.Height);
                    }
                }
            }
        }
        */
        /*
        public void buildBackground2(IntPtr renderer, TileInterface TI)
        {
            Rectangle src = new Rectangle(0, 0, 3, 3);
            Rectangle dest = new Rectangle(0, 0, 4, 4);
            Point2D scale = new Point2D(box.Width / (TI.WorldDimen.X * TI.TW), box.Height / (TI.WorldDimen.Y * TI.TH));
            Point2D inverseScale = new Point2D((TI.WorldDimen.X * TI.TW) / box.Width, (TI.WorldDimen.Y * TI.TH) / box.Height);
            Point2D tileScale = new Point2D(TI.TW / (TI.WorldDimen.X * TI.TW), TI.TH / (TI.WorldDimen.Y * TI.TH));
            SDL.SDL_Color colour;
            SDL.SDL_Color bg = ColourBank.getColour(XENOCOLOURS.BLACK);
            IntPtr tex = SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_ABGR8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, engine.Region.Width, engine.Region.Height);
            SDL.SDL_SetRenderTarget(renderer, tex);
            SDL.SDL_SetRenderDrawColor(renderer, bg.r, bg.g, bg.b, bg.a);
            SDL.SDL_RenderClear(renderer);
            for (int x = 0; x < TI.WorldDimen.IX; x++)
            {
                for (int y = 0; y < TI.WorldDimen.IY; y++)
                {
                    if (TI.tileAt(1, x, y, engine.Region.TileWidth, engine.Region.TileHeight) != null)
                    {
                        switch ((int)(TI.tileAt(1, x, y, engine.Region.TileWidth, engine.Region.TileHeight).Y))
                        {
                            case 0:
                                colour = ColourBank.getColour(XENOCOLOURS.TAN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 1:
                                colour = ColourBank.getColour(XENOCOLOURS.SADDLE_BROWN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 2:
                                colour = ColourBank.getColour(XENOCOLOURS.LAWN_GREEN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 3:
                                colour = ColourBank.getColour(XENOCOLOURS.GREEN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 4:
                                colour = ColourBank.getColour(XENOCOLOURS.FOREST_GREEN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 5:
                                colour = ColourBank.getColour(XENOCOLOURS.SLATE_GRAY);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 6:
                                colour = ColourBank.getColour(XENOCOLOURS.DIM_GRAY);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                        }
                        SDL.SDL_RenderDrawPoint(renderer, x, y);
                    }
                }
            }
            background = new Texture2D(tex, engine.Region.Width, engine.Region.Height);
            SDL.SDL_SetRenderTarget(renderer, default(IntPtr));
        }
        */
        public void buildBackground(IntPtr renderer, RTSCell cell)
        {
            if(cell == null)
            {
                return;
            }
            Rectangle src = new Rectangle(0, 0, 3, 3);
            Rectangle dest = new Rectangle(0, 0, 4, 4);
            Point2D scale = new Point2D(box.Width / cell.CellWidthInPixels, box.Height / cell.CellHeightInPixels);
            Point2D inverseScale = new Point2D(cell.CellWidthInPixels / box.Width, cell.CellHeightInPixels / box.Height);
            Point2D tileScale = new Point2D(cell.Tiles.TileWidth / cell.CellWidthInPixels, cell.Tiles.TileHeight / cell.CellHeightInPixels);
            SDL.SDL_Color colour;
            SDL.SDL_Color bg = ColourBank.getColour(XENOCOLOURS.WHITE);
            IntPtr tex = SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_ABGR8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, cell.Tiles.Width, cell.Tiles.Height);
            SDL.SDL_SetRenderTarget(renderer, tex);
            SDL.SDL_SetRenderDrawColor(renderer, bg.r, bg.g, bg.b, bg.a);
            SDL.SDL_RenderClear(renderer);
            for (int x = 0; x < cell.Width; x++)
            {
                for (int y = 0; y < cell.Height; y++)
                {
                    if (cell.Tiles.tileAt(x, y, cell.Tiles.TileWidth, cell.Tiles.TileHeight) != null)
                    {
                        switch((int)(cell.Tiles.tileAt(x, y, cell.Tiles.TileWidth, cell.Tiles.TileHeight).X))
                        {
                            case 0:
                                colour = ColourBank.getColour(XENOCOLOURS.TAN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 1:
                                colour = ColourBank.getColour(XENOCOLOURS.SADDLE_BROWN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 2:
                                colour = ColourBank.getColour(XENOCOLOURS.LAWN_GREEN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 3:
                                colour = ColourBank.getColour(XENOCOLOURS.GREEN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 4:
                                colour = ColourBank.getColour(XENOCOLOURS.FOREST_GREEN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 5:
                                colour = ColourBank.getColour(XENOCOLOURS.SLATE_GRAY);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 6:
                                colour = ColourBank.getColour(XENOCOLOURS.BLUE);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 7:
                                colour = ColourBank.getColour(XENOCOLOURS.BLUE);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                        }
                        SDL.SDL_RenderDrawPoint(renderer, x, y);
                    }
                }
            }
            //SimpleDraw.savePNG(AppDomain.CurrentDomain.BaseDirectory, "background", 0, 0, engine.Region.Width, engine.Region.Height);
            background = new Texture2D(tex, cell.CellWidthInPixels, cell.CellHeightInPixels);
            SDL.SDL_SetRenderTarget(renderer, default(IntPtr));
        }
        public void buildBackground2(IntPtr renderer, RTSCell cell)
        {
            if (cell == null)
            {
                return;
            }
            Rectangle src = new Rectangle(0, 0, 3, 3);
            Rectangle dest = new Rectangle(0, 0, 4, 4);
            Point2D scale = new Point2D(box.Width / cell.CellWidthInPixels, box.Height / cell.CellHeightInPixels);
            Point2D inverseScale = new Point2D(cell.CellWidthInPixels / box.Width, cell.CellHeightInPixels / box.Height);
            Point2D tileScale = new Point2D(cell.Tiles.TileWidth / cell.CellWidthInPixels, cell.Tiles.TileHeight / cell.CellHeightInPixels);
            SDL.SDL_Color colour;
            SDL.SDL_Color bg = ColourBank.getColour(XENOCOLOURS.WHITE);
            IntPtr tex = SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_ABGR8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, cell.Tiles.Width, cell.Tiles.Height);
            SDL.SDL_SetRenderTarget(renderer, tex);
            SDL.SDL_SetRenderDrawColor(renderer, bg.r, bg.g, bg.b, bg.a);
            SDL.SDL_RenderClear(renderer);
            for (int x = 0; x < cell.Width; x++)
            {
                for (int y = 0; y < cell.Height; y++)
                {
                    if (cell.Tiles.tileAt(x, y, cell.Tiles.TileWidth, cell.Tiles.TileHeight) != null)
                    {
                        switch ((int)(cell.Tiles.tileAt(x, y, cell.Tiles.TileWidth, cell.Tiles.TileHeight).X))
                        {
                            case 0:
                                colour = ColourBank.getColour(XENOCOLOURS.TAN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 1:
                                colour = ColourBank.getColour(XENOCOLOURS.SADDLE_BROWN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 2:
                                colour = ColourBank.getColour(XENOCOLOURS.LAWN_GREEN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 3:
                                colour = ColourBank.getColour(XENOCOLOURS.GREEN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 4:
                                colour = ColourBank.getColour(XENOCOLOURS.FOREST_GREEN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 5:
                                colour = ColourBank.getColour(XENOCOLOURS.SLATE_GRAY);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 6:
                                colour = ColourBank.getColour(XENOCOLOURS.BLUE);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 7:
                                colour = ColourBank.getColour(XENOCOLOURS.BLUE);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                        }
                        SDL.SDL_RenderDrawPoint(renderer, x, y);
                    }
                }
            }
            //SimpleDraw.savePNG(AppDomain.CurrentDomain.BaseDirectory, "background", 0, 0, engine.Region.Width, engine.Region.Height);
            background2 = new Texture2D(tex, cell.CellWidthInPixels, cell.CellHeightInPixels);
            SDL.SDL_SetRenderTarget(renderer, default(IntPtr));
        }
        public void buildBackground3(IntPtr renderer, RTSCell cell)
        {
            if (cell == null)
            {
                return;
            }
            Rectangle src = new Rectangle(0, 0, 3, 3);
            Rectangle dest = new Rectangle(0, 0, 4, 4);
            Point2D scale = new Point2D(box.Width / cell.CellWidthInPixels, box.Height / cell.CellHeightInPixels);
            Point2D inverseScale = new Point2D(cell.CellWidthInPixels / box.Width, cell.CellHeightInPixels / box.Height);
            Point2D tileScale = new Point2D(cell.Tiles.TileWidth / cell.CellWidthInPixels, cell.Tiles.TileHeight / cell.CellHeightInPixels);
            SDL.SDL_Color colour;
            SDL.SDL_Color bg = ColourBank.getColour(XENOCOLOURS.WHITE);
            IntPtr tex = SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_ABGR8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, cell.Tiles.Width, cell.Tiles.Height);
            SDL.SDL_SetRenderTarget(renderer, tex);
            SDL.SDL_SetRenderDrawColor(renderer, bg.r, bg.g, bg.b, bg.a);
            SDL.SDL_RenderClear(renderer);
            for (int x = 0; x < cell.Width; x++)
            {
                for (int y = 0; y < cell.Height; y++)
                {
                    if (cell.Tiles.tileAt(x, y, cell.Tiles.TileWidth, cell.Tiles.TileHeight) != null)
                    {
                        switch ((int)(cell.Tiles.tileAt(x, y, cell.Tiles.TileWidth, cell.Tiles.TileHeight).X))
                        {
                            case 0:
                                colour = ColourBank.getColour(XENOCOLOURS.TAN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 1:
                                colour = ColourBank.getColour(XENOCOLOURS.SADDLE_BROWN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 2:
                                colour = ColourBank.getColour(XENOCOLOURS.LAWN_GREEN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 3:
                                colour = ColourBank.getColour(XENOCOLOURS.GREEN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 4:
                                colour = ColourBank.getColour(XENOCOLOURS.FOREST_GREEN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 5:
                                colour = ColourBank.getColour(XENOCOLOURS.SLATE_GRAY);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 6:
                                colour = ColourBank.getColour(XENOCOLOURS.BLUE);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 7:
                                colour = ColourBank.getColour(XENOCOLOURS.BLUE);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                        }
                        SDL.SDL_RenderDrawPoint(renderer, x, y);
                    }
                }
            }
            //SimpleDraw.savePNG(AppDomain.CurrentDomain.BaseDirectory, "background", 0, 0, engine.Region.Width, engine.Region.Height);
            background3 = new Texture2D(tex, cell.CellWidthInPixels, cell.CellHeightInPixels);
            SDL.SDL_SetRenderTarget(renderer, default(IntPtr));
        }
        public void buildBackground4(IntPtr renderer, RTSCell cell)
        {
            if (cell == null)
            {
                return;
            }
            Rectangle src = new Rectangle(0, 0, 3, 3);
            Rectangle dest = new Rectangle(0, 0, 4, 4);
            Point2D scale = new Point2D(box.Width / cell.CellWidthInPixels, box.Height / cell.CellHeightInPixels);
            Point2D inverseScale = new Point2D(cell.CellWidthInPixels / box.Width, cell.CellHeightInPixels / box.Height);
            Point2D tileScale = new Point2D(cell.Tiles.TileWidth / cell.CellWidthInPixels, cell.Tiles.TileHeight / cell.CellHeightInPixels);
            SDL.SDL_Color colour;
            SDL.SDL_Color bg = ColourBank.getColour(XENOCOLOURS.WHITE);
            IntPtr tex = SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_ABGR8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, cell.Tiles.Width, cell.Tiles.Height);
            SDL.SDL_SetRenderTarget(renderer, tex);
            SDL.SDL_SetRenderDrawColor(renderer, bg.r, bg.g, bg.b, bg.a);
            SDL.SDL_RenderClear(renderer);
            for (int x = 0; x < cell.Width; x++)
            {
                for (int y = 0; y < cell.Height; y++)
                {
                    if (cell.Tiles.tileAt(x, y, cell.Tiles.TileWidth, cell.Tiles.TileHeight) != null)
                    {
                        switch ((int)(cell.Tiles.tileAt(x, y, cell.Tiles.TileWidth, cell.Tiles.TileHeight).X))
                        {
                            case 0:
                                colour = ColourBank.getColour(XENOCOLOURS.TAN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 1:
                                colour = ColourBank.getColour(XENOCOLOURS.SADDLE_BROWN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 2:
                                colour = ColourBank.getColour(XENOCOLOURS.LAWN_GREEN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 3:
                                colour = ColourBank.getColour(XENOCOLOURS.GREEN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 4:
                                colour = ColourBank.getColour(XENOCOLOURS.FOREST_GREEN);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 5:
                                colour = ColourBank.getColour(XENOCOLOURS.SLATE_GRAY);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 6:
                                colour = ColourBank.getColour(XENOCOLOURS.BLUE);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                            case 7:
                                colour = ColourBank.getColour(XENOCOLOURS.BLUE);
                                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                                break;
                        }
                        SDL.SDL_RenderDrawPoint(renderer, x, y);
                    }
                }
            }
            //SimpleDraw.savePNG(AppDomain.CurrentDomain.BaseDirectory, "background", 0, 0, engine.Region.Width, engine.Region.Height);
            background4 = new Texture2D(tex, cell.CellWidthInPixels, cell.CellHeightInPixels);
            SDL.SDL_SetRenderTarget(renderer, default(IntPtr));
        }
        /*
        public void drawForeground(IntPtr renderer, TileInterface TI)
        {
            //SDL.SDL_Color colour;
            //colour = ColourBank.getColour(XENOCOLOURS.GOLD);
            //SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
            Texture2D dot = TextureBank.getTexture("gold dot");
            for (int x = 0; x < engine.Fields.Width; x++)
            {
                for (int y = 0; y < engine.Fields.Height; y++)
                {
                    pixelBox.X = (int)(x + box.X);
                    pixelBox.Y = (int)(y + box.Y);
                    if (engine.Fields.Fields[x, y])
                    {
                        SimpleDraw.draw(renderer, dot, pixelSrcBox, pixelBox);
                        //DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.GOLD), true);
                        //SDL.SDL_RenderDrawPoint(renderer, pixelBox.IX, pixelBox.IY);
                    }
                }
            }
            for (int p = 0; p < engine.Players.Count; p++)
            {
                pixelBox.Width = 3;
                pixelBox.Height = 3;
                // SimpleDraw.draw(renderer, dot, pixelSrcBox, pixelBox);
                //SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                for (int a = 0; a < engine.Players[p].Air.Count; a++)
                {
                    pixelBox.X = (int)((engine.Players[p].Air[a].X) * scale.X) + box.X;
                    pixelBox.Y = (int)((engine.Players[p].Air[a].Y) * scale.Y) + box.Y;
                    
                    if (p == 0)
                    {
                        //SimpleDraw.draw(renderer, pixelg, pixelBox);
                        //DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.BLUE), true);
                        SDL.SDL_RenderDrawPoint(renderer, pixelBox.IX, pixelBox.IY);
                    }
                    else
                    {
                        //SimpleDraw.draw(renderer, pixelr, pixelBox);
                        //DrawRects.drawRect(renderer, pixelBox, ColourBank.getColour(XENOCOLOURS.RED), true);
                        SDL.SDL_RenderDrawPoint(renderer, pixelBox.IX, pixelBox.IY);
                    }
                    SimpleDraw.draw(renderer, dot, pixelSrcBox, pixelBox);
                }
                for (int b = 0; b < engine.Players[p].Builders.Count; b++)
                {
                    pixelBox.X = (int)((engine.Players[p].Builders[b].X) * scale.X) + box.X;
                    pixelBox.Y = (int)((engine.Players[p].Builders[b].Y) * scale.Y) + box.Y;
                    
                    SimpleDraw.draw(renderer, dot, pixelSrcBox, pixelBox);
                }
                for (int h = 0; h < engine.Players[p].Harvesters.Count; h++)
                {
                    pixelBox.X = (int)((engine.Players[p].Harvesters[h].X) * scale.X) + box.X;
                    pixelBox.Y = (int)((engine.Players[p].Harvesters[h].Y) * scale.Y) + box.Y;
                    
                    SimpleDraw.draw(renderer, dot, pixelSrcBox, pixelBox);
                }
                for (int g = 0; g < engine.Players[p].Ground.Count; g++)
                {
                    pixelBox.X = (int)((engine.Players[p].Ground[g].X) * scale.X) + box.X;
                    pixelBox.Y = (int)((engine.Players[p].Ground[g].Y) * scale.Y) + box.Y;
                    
                    SimpleDraw.draw(renderer, dot, pixelSrcBox, pixelBox);
                }
                pixelBox.Width = 3;
                pixelBox.Height = 3;
                for (int b = 0; b < engine.Players[p].Buildings.Count; b++)
                {
                    pixelBox.X = (int)((engine.Players[p].Buildings[b].X) * scale.X) + box.X;
                    pixelBox.Y = (int)((engine.Players[p].Buildings[b].Y) * scale.Y) + box.Y;
                    
                    SimpleDraw.draw(renderer, dot, pixelSrcBox, pixelBox);
                }
            }
        }
        */
        public void drawForeground(IntPtr renderer, RTSWorld world)
        {
            Texture2D dot = TextureBank.getTexture("gold dot");
            if (world.Alpha != null)
            {
                for (int x = 0; x < ((RTSCell)world.Alpha).ResourceGrid.Fields.Width; x++)
                {
                    for (int y = 0; y < ((RTSCell)world.Alpha).ResourceGrid.Fields.Height; y++)
                    {
                        pixelBox.X = (int)(x + box.X);
                        pixelBox.Y = (int)(y + box.Y);
                        if (((RTSCell)world.Alpha).ResourceGrid.Fields.Grid[x, y] != null)
                        {
                            SimpleDraw.draw(renderer, dot, pixelSrcBox, pixelBox);
                        }
                    }
                }
            }
            if (world.Beta != null)
            {
                for (int x = 0; x < ((RTSCell)world.Beta).ResourceGrid.Fields.Width; x++)
                {
                    for (int y = 0; y < ((RTSCell)world.Beta).ResourceGrid.Fields.Height; y++)
                    {
                        pixelBox.X = (int)(x + box.X + (box.Width / 2));
                        pixelBox.Y = (int)(y + box.Y);
                        if (((RTSCell)world.Beta).ResourceGrid.Fields.Grid[x, y] != null)
                        {
                            SimpleDraw.draw(renderer, dot, pixelSrcBox, pixelBox);
                        }
                    }
                }
            }
            if (world.Delta != null)
            {
                for (int x = 0; x < ((RTSCell)world.Delta).ResourceGrid.Fields.Width; x++)
                {
                    for (int y = 0; y < ((RTSCell)world.Delta).ResourceGrid.Fields.Height; y++)
                    {
                        pixelBox.X = (int)(x + box.X + (box.Width / 2));
                        pixelBox.Y = (int)(y + box.Y + (box.Height / 2));
                        if (((RTSCell)world.Delta).ResourceGrid.Fields.Grid[x, y] != null)
                        {
                            SimpleDraw.draw(renderer, dot, pixelSrcBox, pixelBox);
                        }
                    }
                }
            }
            if (world.Gamma != null)
            {
                for (int x = 0; x < ((RTSCell)world.Gamma).ResourceGrid.Fields.Width; x++)
                {
                    for (int y = 0; y < ((RTSCell)world.Gamma).ResourceGrid.Fields.Height; y++)
                    {
                        pixelBox.X = (int)(x + box.X + (box.Width / 2));
                        pixelBox.Y = (int)(y + box.Y + (box.Height / 2));
                        if (((RTSCell)world.Gamma).ResourceGrid.Fields.Grid[x, y] != null)
                        {
                            SimpleDraw.draw(renderer, dot, pixelSrcBox, pixelBox);
                        }
                    }
                }
            }
            for (int p = 0; p < world.Commanders.Count; p++)
            {
                pixelBox.Width = 3;
                pixelBox.Height = 3;
                if (p == 0)
                {
                    dot = TextureBank.getTexture("blue dot");
                }
                else
                {
                    dot = TextureBank.getTexture("red dot");
                }
                for (int a = 0; a < world.Commanders[p].AirUnits.Count; a++)
                {
                    pixelBox.X = (int)((world.Commanders[p].AirUnits[a].X) * scale.X) + box.X;
                    pixelBox.Y = (int)((world.Commanders[p].AirUnits[a].Y) * scale.Y) + box.Y;
                    SimpleDraw.draw(renderer, dot, pixelSrcBox, pixelBox);
                }
                for (int g = 0; g < world.Commanders[p].GroundUnits.Count; g++)
                {
                    pixelBox.X = (int)((world.Commanders[p].GroundUnits[g].X) * scale.X) + box.X;
                    pixelBox.Y = (int)((world.Commanders[p].GroundUnits[g].Y) * scale.Y) + box.Y;
                    SimpleDraw.draw(renderer, dot, pixelSrcBox, pixelBox);
                }
                pixelBox.Width = 3;
                pixelBox.Height = 3;
                for (int b = 0; b < world.Commanders[p].Buildings.Count; b++)
                {
                    pixelBox.X = (int)((world.Commanders[p].Buildings[b].X) * scale.X) + box.X;
                    pixelBox.Y = (int)((world.Commanders[p].Buildings[b].Y) * scale.Y) + box.Y;
                    SimpleDraw.draw(renderer, dot, pixelSrcBox, pixelBox);
                }
            }
        }
        /*
        public void drawFog(IntPtr renderer, RTSCell cell)
        {
            Rectangle canvas = new Rectangle(0, 0, engine.Region.Width, engine.Region.Height);
            SDL.SDL_Color bg = ColourBank.getColour(XENOCOLOURS.BLACK);
            IntPtr tex = SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_ABGR8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, engine.Region.Width, engine.Region.Height);
            SDL.SDL_SetRenderTarget(renderer, tex);
            SDL.SDL_Color colour = ColourBank.getColour(XENOCOLOURS.BLACK);
            SDL.SDL_SetRenderDrawColor(renderer, bg.r, bg.g, bg.b, bg.a);
            SDL.SDL_RenderClear(renderer);
            SimpleDraw.draw(renderer, background, canvas);
            drawMidground(renderer, engine);
            if (engine.FogOn == true)
            {
                for (int x = 0; x < engine.Fog.Fog.Width; x++)
                {
                    for (int y = 0; y < engine.Fog.Fog.Height; y++)
                    {
                        if (engine.Fog.Fog.dataAt(x, y) == true)
                        {
                            DrawRects.drawRect(renderer, new Rectangle(x, y, 3, 3), colour, true);
                        }
                    }
                }
            }
            foreground = new Texture2D(tex, engine.Fog.Fog.Width, engine.Fog.Fog.Height);
            SDL.SDL_SetRenderTarget(renderer, default(IntPtr));
        }
        */
        public void drawMidground(IntPtr renderer, RTSWorld world)
        {
            SDL.SDL_Color colour = ColourBank.getColour(XENOCOLOURS.BLACK);
            colour = ColourBank.getColour(XENOCOLOURS.GOLD);
            SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
            Point2D scaler2 = new Point2D(box.Width / ((RTSCell)world.Alpha).Width, box.Height / ((RTSCell)world.Alpha).Height);
            Point2D p2 = new Point2D();
            pixelBox.Width = 3;
            pixelBox.Height = 3;
            if (world.Alpha != null)
            {
                for (int x = 0; x < ((RTSCell)world.Alpha).ResourceGrid.Width; x++)
                {
                    for (int y = 0; y < ((RTSCell)world.Alpha).ResourceGrid.Height; y++)
                    {
                        if (((RTSCell)world.Alpha).ResourceGrid.Fields.Grid[x, y] != null)
                        {
                            pixelBox.X = (x * scaler2.X) + box.X;
                            pixelBox.Y = (y * scaler2.Y) + box.Y;
                            SDL.SDL_RenderDrawPoint(renderer, pixelBox.IX, pixelBox.IY);
                        }
                    }
                }
            }
            if (world.Beta != null)
            {
                for (int x = 0; x < ((RTSCell)world.Beta).ResourceGrid.Width; x++)
                {
                    for (int y = 0; y < ((RTSCell)world.Beta).ResourceGrid.Height; y++)
                    {
                        if (((RTSCell)world.Beta).ResourceGrid.Fields.Grid[x, y] != null)
                        {
                            pixelBox.X = (x * scaler2.X) + box.X + (box.Width / 2);
                            pixelBox.Y = (y * scaler2.Y) + box.Y;
                            SDL.SDL_RenderDrawPoint(renderer, pixelBox.IX, pixelBox.IY);
                        }
                    }
                }
            }
            if (world.Delta != null)
            {
                for (int x = 0; x < ((RTSCell)world.Delta).ResourceGrid.Width; x++)
                {
                    for (int y = 0; y < ((RTSCell)world.Delta).ResourceGrid.Height; y++)
                    {
                        if (((RTSCell)world.Delta).ResourceGrid.Fields.Grid[x, y] != null)
                        {
                            pixelBox.X = (x * scaler2.X) + box.X + (box.Width / 2);
                            pixelBox.Y = (y * scaler2.Y) + box.Y + (box.Height / 2);
                            SDL.SDL_RenderDrawPoint(renderer, pixelBox.IX, pixelBox.IY);
                        }
                    }
                }
            }
            if (world.Gamma != null)
            {
                for (int x = 0; x < ((RTSCell)world.Gamma).ResourceGrid.Width; x++)
                {
                    for (int y = 0; y < ((RTSCell)world.Gamma).ResourceGrid.Height; y++)
                    {
                        if (((RTSCell)world.Gamma).ResourceGrid.Fields.Grid[x, y] != null)
                        {
                            pixelBox.X = (x * scaler2.X) + box.X;
                            pixelBox.Y = (y * scaler2.Y) + box.Y + (box.Height / 2);
                            SDL.SDL_RenderDrawPoint(renderer, pixelBox.IX, pixelBox.IY);
                        }
                    }
                }
            }
            for (int p = 0; p < world.Commanders.Count; p++)
            {
                pixelBox.Width = 3;
                pixelBox.Height = 3;
                if (p == 0)
                {
                    colour = ColourBank.getColour(XENOCOLOURS.BLUE);
                }
                else
                {
                    colour = ColourBank.getColour(XENOCOLOURS.RED);
                }
                SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                for (int a = 0; a < world.Commanders[p].AirUnits.Count; a++)
                {
                    p2.X = (world.Commanders[p].AirUnits[a].X / world.TileWidth);
                    pixelBox.X = (int)(p2.X * scaler2.X) + box.X;// * scale.X);// + box.X;
                    p2.Y = (world.Commanders[p].AirUnits[a].Y / world.TileHeight);
                    pixelBox.Y = (int)(p2.Y * scaler2.Y) + box.Y;// * scale.Y);// + box.Y;
                    DrawRects.drawRect(renderer, pixelBox, colour, true);
                }
                for(int g = 0; g < world.Commanders[p].GroundUnits.Count; g++)
                {
                    p2.X = (world.Commanders[p].GroundUnits[g].X / world.TileWidth);
                    pixelBox.X = (int)(p2.X * scaler2.X) + box.X;// * scale.X);// + box.X;
                    p2.Y = (world.Commanders[p].GroundUnits[g].Y / world.TileHeight);
                    pixelBox.Y = (int)(p2.Y * scaler2.Y) + box.Y;// * scale.Y);// + box.Y;
                    DrawRects.drawRect(renderer, pixelBox, colour, true);
                }
                pixelBox.Width = 3;
                pixelBox.Height = 3;
                for (int b = 0; b < world.Commanders[p].Buildings.Count; b++)
                {
                    p2.X = (world.Commanders[p].Buildings[b].X / world.TileWidth);
                    pixelBox.X = (int)(p2.X * scaler2.X) + box.X; ;// * scale.X);// + box.X;
                    p2.Y = (world.Commanders[p].Buildings[b].Y / world.TileHeight);
                    pixelBox.Y = (int)(p2.Y * scaler2.Y) + box.Y; ;// * scale.Y);// + box.Y;
                    DrawRects.drawRect(renderer, pixelBox, colour, true);
                }
            }
        }
        public void setBoxPos(int x, int y)
        {
            box.X = x;
            box.Y = y;
        }
        /// <summary>
        /// Pixelbl property
        /// </summary>
        public Texture2D Pixelbl
        {
            set { pixelbl = value; }
        }
        /// <summary>
        /// Pixelg property
        /// </summary>
        public Texture2D Pixelg
        {
            set { pixelg = value; }
        }
        /// <summary>
        /// Pixelr property
        /// </summary>
        public Texture2D Pixelr
        {
            set { pixelr = value; }
        }
        /// <summary>
        /// Pixelbk property
        /// </summary>
        public Texture2D Pixelbk
        {
            set { pixelbk = value; }
        }
        /// <summary>
        /// MapBox property
        /// </summary>
        public Rectangle MapBox
        {
            get { return mapBox; }
        }
        /// <summary>
        /// Background property
        /// </summary>
        public Texture2D Background
        {
            get { return background; }
        }
    }
}
