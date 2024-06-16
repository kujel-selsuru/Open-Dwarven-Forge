//====================================================
//Written by Kujel Selsuru
//Last Updated 15/06/24
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace XenoLib
{
    public enum QUADRANT
    {
        ALPHA = 0,
        BETA,
        DELTA,
        GAMMA
    };

    public class SCRadar
    {
        //protected
        protected Texture2D background;
        protected Rectangle box;
        protected Rectangle window;
        protected Point2D scaler;
        protected Rectangle pixel;
        protected Rectangle fog;
        protected RTSCell cell;
        protected Rectangle buttonBack;
        protected Rectangle selectedQuad;
        protected SimpleButton4 alphaButton;
        protected SimpleButton4 betaButton;
        protected SimpleButton4 deltaButton;
        protected SimpleButton4 gammaButton;
        protected QUADRANT quad;

        //public
        /// <summary>
        /// SCRadar constructor
        /// </summary>
        /// <param name="x">X position on screen</param>
        /// <param name="y">Y position on screen</param>
        /// <param name="cellWidth">Width of cell in tiles</param>
        /// <param name="cellHeight">Height of cell in tiles</param>
        public SCRadar(int x, int y, int cellWidth, int cellHeight)
        {
            background = default(Texture2D);
            box = new Rectangle(x, y, 6 * 32, 6 * 32);
            scaler = new Point2D(box.Width / cellWidth, box.Height / cellHeight);
            window = new Rectangle(x, y, (24 * 32) * scaler.X, (21 * 32) * scaler.Y);
            pixel = new Rectangle(0, 0, 4, 4);
            fog = new Rectangle(0, 0, 4, 4);
            cell = null;
            buttonBack = new Rectangle(x, y - 32, 128, 32);
            selectedQuad = new Rectangle(x, y - 32, 32, 32);
            quad = QUADRANT.ALPHA;
            alphaButton = new SimpleButton4(TextureBank.getTexture("Invisible Button_32_32"), TextureBank.getTexture("Invisible Button_32_32"), x, y - 32, "A");
            betaButton = new SimpleButton4(TextureBank.getTexture("Invisible Button_32_32"), TextureBank.getTexture("Invisible Button_32_32"), x + 32, y - 32, "B");
            deltaButton = new SimpleButton4(TextureBank.getTexture("Invisible Button_32_32"), TextureBank.getTexture("Invisible Button_32_32"), x + 64, y - 32, "D");
            gammaButton = new SimpleButton4(TextureBank.getTexture("Invisible Button_32_32"), TextureBank.getTexture("Invisible Button_32_32"), x + 96, y - 32, "G");
        }
        /// <summary>
        /// Builds the currently selected quadrant background
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="world">RTSWorld reference</param>
        /// <param name="quad">QUADRANT value</param>
        public void buildQuadrant(IntPtr renderer, RTSWorld world, QUADRANT quad)
        {
            switch(quad)
            {
                case QUADRANT.ALPHA:
                    cell = (RTSCell)world.Alpha;
                    buildBackground(renderer, (RTSCell)world.Alpha);
                    break;
                case QUADRANT.BETA:
                    cell = (RTSCell)world.Beta;
                    buildBackground(renderer, (RTSCell)world.Beta);
                    break;
                case QUADRANT.DELTA:
                    cell = (RTSCell)world.Delta;
                    buildBackground(renderer, (RTSCell)world.Delta);
                    break;
                case QUADRANT.GAMMA:
                    cell = (RTSCell)world.Gamma;
                    buildBackground(renderer, (RTSCell)world.Gamma);
                    break;
            }
        }
        /// <summary>
        /// Builds the background for the radar image
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="world">RTSWorld reference</param>
        public void buildBackground(IntPtr renderer, RTSCell cell)
        {
            SDL.SDL_Color colour;
            SDL.SDL_Color bg = ColourBank.getColour(XENOCOLOURS.WHITE);
            IntPtr tex = SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_ABGR8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, cell.Tiles.Width, cell.Tiles.Height);
            SDL.SDL_SetRenderTarget(renderer, tex);
            SDL.SDL_SetRenderDrawColor(renderer, bg.r, bg.g, bg.b, bg.a);
            SDL.SDL_RenderClear(renderer);
            for(int x = 0; x < cell.Width; x++)
            {
                for(int y = 0; y < cell.Height; y++)
                {
                    if(cell.DoodadLayer2.Grid[x, y] != null)
                    {
                        colour = ColourBank.getColour(XENOCOLOURS.FOREST_GREEN);
                        SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                    }
                    else
                    {
                        colour = ColourBank.getColour(XENOCOLOURS.TAN);
                        SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
                    }
                        SDL.SDL_RenderDrawPoint(renderer, x, y);
                }
            }
            background = new Texture2D(tex, cell.Width, cell.Height);
            SDL.SDL_SetRenderTarget(renderer, default(IntPtr));
        }
        /// <summary>
        /// Tracks window position
        /// </summary>
        /// <param name="world">RTSWorld reference</param>
        /// <param name="singleCell">Flag value</param>
        public void trackWin(RTSWorld world, bool singleCell = false)
        {
            if(singleCell == true)
            {
                window.X = box.X + ((((RTSCell)world.Alpha).CellWin.X - cell.WorldX) * scaler.X);
                window.Y = box.Y + ((((RTSCell)world.Alpha).CellWin.Y - cell.WorldY) * scaler.Y);
            }
            else
            {
                window.X = box.X + ((world.Winx - cell.WorldX) * scaler.X);
                window.Y = box.Y + ((world.Winy - cell.WorldY) * scaler.Y);
            }
        }
        /// <summary>
        /// Tracks window position
        /// </summary>
        /// <param name="world">RTSWorld reference</param>
        /// <param name="wx">Change in X position</param>
        /// <param name="wy">Change in Y position</param>
        public void trackCellWin(RTSWorld world, int wx, int wy)
        {
            world.setWindow(world.Winx + wx, world.Winy + wy);
            window.X = box.X + ((world.Winx - cell.WorldX) * scaler.X);
            window.Y = box.Y + ((world.Winy - cell.WorldY) * scaler.Y);
        }
        /// <summary>
        /// Updates position of window
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="world">RTSWorld reference</param>
        /// <param name="singleCell">Flag value</param>
        public void update(IntPtr renderer, RTSWorld world, bool singleCell = false)
        {
            if(box.pointInRect(MouseHandler.getTip()) == true)
            {
                if(MouseHandler.getLeft() == true)
                {
                    window.X = box.X + ((MouseHandler.getTip().X - box.X) - (window.Width / 2));
                    if(window.X < box.X)
                    {
                        window.X = box.X;
                    }
                    window.Y = box.Y + ((MouseHandler.getTip().Y - box.Y) - (window.Height / 2));
                    if(window.Y < box.Y)
                    {
                        window.Y = box.Y;
                    }
                    if (singleCell == true)
                    {
                        cell.CellWin.X = (window.X - box.X) / scaler.X;
                        cell.WinRect.X = cell.CellWin.X;
                        cell.CellWin.Y = (window.Y - box.Y) / scaler.Y;
                        cell.WinRect.Y = cell.CellWin.Y;
                    }
                    else
                    {
                        world.setWindow((int)((cell.CellX + (window.X - box.X)) / scaler.Y), 
                            (int)((cell.CellY + (window.Y - box.Y)) / scaler.Y));
                    }
                }
            }
        }
        /// <summary>
        /// Updates SCRadar buttons
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="world">RTSWorld reference</param>
        public void updateButtons(IntPtr renderer, RTSWorld world)
        {
            if (alphaButton.clicked2() == true)
            {
                if (world.Alpha != null)
                {
                    selectedQuad.X = box.X;
                    buildQuadrant(renderer, world, QUADRANT.ALPHA);
                    quad = QUADRANT.ALPHA;
                }
            }
            else if (betaButton.clicked2() == true)
            {
                if (world.Beta != null)
                {
                    selectedQuad.X = box.X + 32;
                    buildQuadrant(renderer, world, QUADRANT.BETA);
                    quad = QUADRANT.BETA;
                }
            }
            else if (deltaButton.clicked2() == true)
            {
                if (world.Delta != null)
                {
                    selectedQuad.X = box.X + 64;
                    buildQuadrant(renderer, world, QUADRANT.DELTA);
                    quad = QUADRANT.DELTA;
                }
            }
            else if (gammaButton.clicked2() == true)
            {
                if (world.Gamma != null)
                {
                    selectedQuad.X = box.X + 96;
                    buildQuadrant(renderer, world, QUADRANT.GAMMA);
                    quad = QUADRANT.GAMMA;
                }
            }
            alphaButton.update();
            betaButton.update();
            deltaButton.update();
            gammaButton.update();
        }
        /// <summary>
        /// Draws radar window
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void drawWindow(IntPtr renderer)
        {
            DrawRects.drawRect(renderer, window, ColourBank.getColour(XENOCOLOURS.YELLOW), false);
        }
        /// <summary>
        /// Draws the radar image
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="world">RTSWorld reference</param>
        public void draw(IntPtr renderer, RTSWorld world)
        {
            SimpleDraw.draw(renderer, background, box);
            //Dwarves
            for(int i = 0; i < world.Commanders[0].Buildings.Count - 1; i++)
            {
                if(world.Commanders[0].Buildings[i].X >= cell.CellX &&
                    world.Commanders[0].Buildings[i].X <= cell.CellX + cell.CellWidthInPixels)
                {
                    if(world.Commanders[0].Buildings[i].Y >= cell.CellY &&
                    world.Commanders[0].Buildings[i].Y <= cell.CellY + cell.CellHeightInPixels)
                    {
                        pixel.X = box.X + (world.Commanders[0].Buildings[i].X - cell.CellX) * scaler.X;
                        pixel.Y = box.Y + (world.Commanders[0].Buildings[i].Y - cell.CellY) * scaler.Y;
                        DrawRects.drawRect(renderer, pixel, ColourBank.getColour(XENOCOLOURS.BLUE), true);
                    }
                }   
            }
            for(int i = 0; i < world.Commanders[0].GroundUnits.Count - 1; i++)
            {
                if(world.Commanders[0].GroundUnits[i].X >= cell.CellX &&
                    world.Commanders[0].GroundUnits[i].X <= cell.CellX + cell.CellWidthInPixels)
                {
                    if(world.Commanders[0].GroundUnits[i].Y >= cell.CellY &&
                    world.Commanders[0].GroundUnits[i].Y <= cell.CellY + cell.CellHeightInPixels)
                    {
                        pixel.X = box.X + (world.Commanders[0].GroundUnits[i].X - cell.CellX) * scaler.X;
                        pixel.Y = box.Y + (world.Commanders[0].GroundUnits[i].Y - cell.CellY) * scaler.Y;
                        DrawRects.drawRect(renderer, pixel, ColourBank.getColour(XENOCOLOURS.BLUE), true);
                    }
                }
            }
            for(int i = 0; i < world.Commanders[0].AirUnits.Count - 1; i++)
            {
                if(world.Commanders[0].AirUnits[i].X >= cell.CellX &&
                    world.Commanders[0].AirUnits[i].X <= cell.CellX + cell.CellWidthInPixels)
                {
                    if(world.Commanders[0].AirUnits[i].Y >= cell.CellY &&
                    world.Commanders[0].AirUnits[i].Y <= cell.CellY + cell.CellHeightInPixels)
                    {
                        pixel.X = box.X + (world.Commanders[0].AirUnits[i].X - cell.CellX) * scaler.X;
                        pixel.Y = box.Y + (world.Commanders[0].AirUnits[i].Y - cell.CellY) * scaler.Y;
                        DrawRects.drawRect(renderer, pixel, ColourBank.getColour(XENOCOLOURS.BLUE), true);
                    }
                }
            }
            //WEF
            for(int i = 0; i < world.Commanders[1].Buildings.Count - 1; i++)
            {
                if(world.Commanders[1].Buildings[i].X >= cell.CellX &&
                    world.Commanders[1].Buildings[i].X <= cell.CellX + cell.CellWidthInPixels)
                {
                    if(world.Commanders[1].Buildings[i].Y >= cell.CellY &&
                    world.Commanders[1].Buildings[i].Y <= cell.CellY + cell.CellHeightInPixels)
                    {
                        pixel.X = box.X + (world.Commanders[1].Buildings[i].X - cell.CellX) * scaler.X;
                        pixel.Y = box.Y + (world.Commanders[1].Buildings[i].Y - cell.CellY) * scaler.Y;
                        DrawRects.drawRect(renderer, pixel, ColourBank.getColour(XENOCOLOURS.RED), true);
                    }
                }
            }
            for(int i = 0; i < world.Commanders[1].GroundUnits.Count - 1; i++)
            {
                if(world.Commanders[1].GroundUnits[i].X >= cell.CellX &&
                    world.Commanders[1].GroundUnits[i].X <= cell.CellX + cell.CellWidthInPixels)
                {
                    if(world.Commanders[1].GroundUnits[i].Y >= cell.CellY &&
                    world.Commanders[1].GroundUnits[i].Y <= cell.CellY + cell.CellHeightInPixels)
                    {
                        pixel.X = box.X + (world.Commanders[1].GroundUnits[i].X - cell.CellX) * scaler.X;
                        pixel.Y = box.Y + (world.Commanders[1].GroundUnits[i].Y - cell.CellY) * scaler.Y;
                        DrawRects.drawRect(renderer, pixel, ColourBank.getColour(XENOCOLOURS.RED), true);
                    }
                }
            }
            for(int i = 0; i < world.Commanders[1].AirUnits.Count - 1; i++)
            {
                if(world.Commanders[1].AirUnits[i].X >= cell.CellX &&
                    world.Commanders[1].AirUnits[i].X <= cell.CellX + cell.CellWidthInPixels)
                {
                    if(world.Commanders[1].AirUnits[i].Y >= cell.CellY &&
                    world.Commanders[1].AirUnits[i].Y <= cell.CellY + cell.CellHeightInPixels)
                    {
                        pixel.X = box.X + (world.Commanders[1].AirUnits[i].X - cell.CellX) * scaler.X;
                        pixel.Y = box.Y + (world.Commanders[1].AirUnits[i].Y - cell.CellY) * scaler.Y;
                        DrawRects.drawRect(renderer, pixel, ColourBank.getColour(XENOCOLOURS.RED), true);
                    }
                }
            }
            for(int x = 0; x < cell.Fogg.FogGrid.Width - 1; x++)
            {
                for(int y = 0; y < cell.Fogg.FogGrid.Height - 1; y++)
                {
                    if(cell.Fogg.FogGrid.Grid[x, y] == false)
                    {
                        fog.X = box.X + x;
                        fog.Y = box.Y + y;
                        DrawRects.drawRect(renderer, fog, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                    }
                }
            }
            DrawRects.drawRect(renderer, box, ColourBank.getColour(XENOCOLOURS.GRAY), false);
            DrawRects.drawRect(renderer, buttonBack, ColourBank.getColour(XENOCOLOURS.BLUE), true);
            alphaButton.drawName(renderer, true);
            betaButton.drawName(renderer, true);
            deltaButton.drawName(renderer, true);
            gammaButton.drawName(renderer, true);
            DrawRects.drawRect(renderer, selectedQuad, ColourBank.getColour(XENOCOLOURS.MIDNIGHT_BLUE), false);
            drawWindow(renderer);
        }
        /// <summary>
        /// Sets the window position based on a provided point
        /// </summary>
        /// <param name="p">Point2D reference</param>
        public void setWinPos(Point2D p)
        {
            switch(quad)
            {
                case QUADRANT.ALPHA:
                    window.X = box.X + (p.X - cell.WorldX);
                    window.Y = box.Y + (p.Y - cell.WorldY);
                    break;
                case QUADRANT.BETA:
                    window.X = box.X + (p.X - cell.WorldX);
                    window.Y = box.Y + (p.Y - cell.WorldY);
                    break;
                case QUADRANT.DELTA:
                    window.X = box.X + (p.X - cell.WorldX);
                    window.Y = box.Y + (p.Y - cell.WorldY);
                    break;
                case QUADRANT.GAMMA:
                    window.X = box.X + (p.X - cell.WorldX);
                    window.Y = box.Y + (p.Y - cell.WorldY);
                    break;
            }
        }
        /// <summary>
        /// Window position relative to the alpha's position
        /// </summary>
        public Point2D WindowPos
        {
            get { return new Point2D(window.X * scaler.X, window.Y * scaler.Y); }
        }
        /// <summary>
        /// Box property
        /// </summary>
        public Rectangle Box
        {
            get { return box; }
        }
        /// <summary>
        /// ButtonBack property
        /// </summary>
        public Rectangle ButtonBack
        {
            get { return buttonBack; }
        }
    }
}
