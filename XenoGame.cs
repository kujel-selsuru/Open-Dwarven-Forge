//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// XenoGame class
    /// </summary>
    public class XenoGame
    {
        //protected
        protected IntPtr renderer;
        protected IntPtr surface;
        protected IntPtr window;
        protected int windowWidth;
        protected int windowHeight;
        protected SDL.SDL_Event eve;
        protected bool quit;
        protected LTimer capTimer;
        protected LTimer frameTimer;
        protected float fps;
        protected uint cap;
        protected int countedFrames;
        protected int TICKSPERFRAME;
        protected OpenWorld world;
        protected Dictionary<string, string> songs;
        protected bool cursorVisible;
        protected SDL.SDL_Color colour;
        protected MessageBoard msgs;

        //public
        /// <summary>
        /// XenoGame constructor
        /// </summary>
        /// <param name="width">Window width in pixels</param>
        /// <param name="height">Window height in pixels</param>
        /// <param name="gameName">Name of game displayed on title bar</param>
        /// <param name="showCursor">ShowCursor flag value</param>
        public XenoGame(int x, int y, int width, int height, string gameName, int showCursor = 0)
        {
            windowWidth = width;
            windowHeight = height;
            SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);
            window = SDL.SDL_CreateWindow(gameName, x, y, width, height, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
            renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
            SDL.SDL_ShowCursor(showCursor);
            if(showCursor == 1)
            {
                cursorVisible = true;
            }
            else
            {
                cursorVisible = false;
            }
            quit = false;
            capTimer = new LTimer();
            frameTimer = new LTimer();
            fps = 0;
            cap = 0;
            countedFrames = 0;
            TICKSPERFRAME = 1000 / 60;
            frameTimer.start();
            SDL.SDL_Color colour;
            colour.r = 255;
            colour.g = 255;
            colour.b = 255;
            colour.a = 1;
            msgs = new MessageBoard(0, 0, 15, 210);
        }
        /// <summary>
        /// Game update funcion
        /// </summary>
        public virtual void update()
        {
            //SDL.SDL_PollEvent(out eve);
            while (SDL.SDL_PollEvent(out eve) != 0)
            {
                if (eve.type == SDL.SDL_EventType.SDL_QUIT)
                {
                    quit = true;
                }
                if (eve.type == SDL.SDL_EventType.SDL_KEYDOWN || 
                    eve.type == SDL.SDL_EventType.SDL_KEYUP)
                {
                    KeyboardHandler.update(eve);
                }
                if(eve.type == SDL.SDL_EventType.SDL_MOUSEMOTION ||
                   eve.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN ||
                   eve.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP)
                {
                    MouseHandler.update(eve);
                }
            }
            if (world != null)
            {
                world.updateAvatarPos();
            }
            msgs.update();
            //game update code here
        }
        /// <summary>
        /// Run at start of updates, for capping frame rate
        /// </summary>
        public void atStartUpdate()
        {
            capTimer.start();
        }
        /// <summary>
        /// Run at end of renders, for capping frame rate
        /// </summary>
        public void atEndRender()
        {
            countedFrames++;
        }
        /// <summary>
        /// Game level rendering function
        /// </summary> 
        public virtual void draw()
        {
            
            //clears screen
            SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
            SDL.SDL_RenderClear(renderer);
            //renderer update code here

            SDL.SDL_RenderPresent(renderer);
            
        }
        /// <summary>
        /// Caps the framerate (caps at 60 frames per second)
        /// </summary>
        public void capFrames()
        {
            if (frameTimer.getTicks() != 0)
            {
                uint temp = (uint)((double)frameTimer.getTicks() / 1000.0);
                if(temp == 0)
                {
                    temp = 1;
                }
                fps = countedFrames / temp;
            }
            cap = capTimer.getTicks();
            if (fps > 2000000)
            {
                fps = 0;
            }
            //Console.WriteLine(fps);
            if (cap < TICKSPERFRAME)
            {
                SDL.SDL_Delay((uint)(TICKSPERFRAME - cap));
            }
        }
        /// <summary>
        /// Loads a specifed graphics file
        /// </summary>
        /// <param name="name">File name/key</param>
        /// <param name="filePath">File path</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public void loadTexture(string name, string filePath, int width, int height)
        {
            SDL.SDL_Color magenta;
            magenta.r = 255;
            magenta.g = 0;
            magenta.b = 255;
            magenta.a = 1;
            TextureBank.addTexture(name, TextureLoader.load(filePath, renderer, magenta, width, height));
        }
        /// <summary>
        /// Replaces a specifed graphics file
        /// </summary>
        /// <param name="name">File name/key</param>
        /// <param name="filePath">File path</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public void replaceTexture(string name, string filePath, int width, int height)
        {
            SDL.SDL_Color magenta;
            magenta.r = 255;
            magenta.g = 0;
            magenta.b = 255;
            magenta.a = 1;
            TextureBank.replaceTexture(name, TextureLoader.load(filePath, renderer, magenta, width, height));
        }
        /// <summary>
        /// Initializes a standard sized world (empty, no cells generated)
        /// </summary>
        /// <param name="source">Tile source Texture2D reference</param>
        /// <param name="autoSource">AutoTile source Texture2D reference</param>
        /// <param name="ww">World width in cells</param>
        /// <param name="wh">World height in cells</param>
        public virtual void initWorld(Texture2D source, Texture2D autoSource, int ww, int wh, int inputDelay = 1)
        {
            world = new OpenWorld(source, autoSource, 100, 100, 0, 0, 22, 17, 32, 32, ww, wh, inputDelay);
        }
        /// <summary>
        /// Loads a MP3 file for music into internal dictionary
        /// </summary>
        /// <param name="name">MP3 name/key</param>
        /// <param name="filePath">File path</param>
        public void loadMP3(string name, string filePath)
        {
            songs.Add(name, filePath);
        }
        /// <summary>
        /// Loads a wave file for SFXs into the SFXBank
        /// </summary>
        /// <param name="name">Wave name/key</param>
        /// <param name="filePath">File path</param>
        public void loadWave(string name, string filePath)
        {
            SFXBank.addSFX(name, filePath);
        }
        /// <summary>
        /// A simple drawing function for testing
        /// </summary>
        /// <param name="name">File name/key</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        public void drawTexture(string name, int x, int y, int w, int h)
        {
            SDL.SDL_Rect src;
            src.x = 0;
            src.y = 0;
            src.w = w;
            src.h = h;
            SDL.SDL_Rect dest;
            dest.x = x;
            dest.y = y;
            dest.w = w;
            dest.h = h;
            SDL.SDL_SetRenderDrawColor(renderer, 255, 255, 255, 1);
            SDL.SDL_RenderClear(renderer);
            SimpleDraw.draw(renderer, TextureBank.getTexture(name), src, dest);
            SDL.SDL_RenderPresent(renderer);
        }
        /// <summary>
        /// Clears the renderer and sets screen to white
        /// <param name="r">Red value (0-255)</param>
        /// <param name="g">Green value (0-255)</param>
        /// <param name="b">Blue value (0-255)</param>
        /// </summary>
        public void clearScreen(int r = 0, int g = 0, int b = 0)
        {
            SDL.SDL_SetRenderDrawColor(renderer, (byte)r, (byte)g, (byte)b, 1);
            SDL.SDL_RenderClear(renderer);
        }
        /// <summary>
        /// Presents whatever is in the renderer buffer
        /// </summary>
        public void renderPresent()
        {
            SDL.SDL_RenderPresent(renderer);
        }
        /// <summary>
        /// Draws the message board
        /// </summary>
        /// <param name="renderer">Renderer refernence</param>
        public void renderMsgs(IntPtr renderer)
        {
            SDL.SDL_Color color;
            color.r = 255;
            color.g = 255;
            color.b = 255;
            color.a = 0;
            msgs.draw(renderer, color);
        }
        /// <summary>
        /// Flushes events from event queue
        /// </summary>
        public void flushEvents()
        {
            SDL.SDL_FlushEvents(0, SDL.SDL_EventType.SDL_WINDOWEVENT);
        }
        /// <summary>
        /// Toggles cursor visibility
        /// </summary>
        public void toggleCursor()
        {
            if (cursorVisible)
            {
                SDL.SDL_ShowCursor(0);
            }
            else
            {
                SDL.SDL_ShowCursor(1);
            }
        }
        /// <summary>
        /// World property
        /// </summary>
        public OpenWorld World
        {
            get { return world; }
        }
        /// <summary>
        /// Renderer property
        /// </summary>
        public IntPtr Renderer
        {
            get { return renderer; }
        }
        /// <summary>
        /// Eve property
        /// </summary>
        public SDL.SDL_Event Eve
        {
            get { return eve; }
        }
        /// <summary>
        /// Quit property
        /// </summary>
        public bool Quit
        {
            get { return quit; }
            set { quit = value; }
        }
        /// <summary>
        /// Colour property
        /// </summary>
        public SDL.SDL_Color Coulour
        {
            get { return colour; }
            set { colour = value; }
        }
        /// <summary>
        /// Msgs property
        /// </summary>
        public MessageBoard Msgs
        {
            get { return msgs; }
        }
    }
    /// <summary>
    /// Inherited OpenWorldCell class, operates simple to interiors cells
    /// </summary>
    public class XenoScene : OpenWorldCell
    {
        //protected

        //public
        /// <summary>
        /// XenoScene constructor
        /// </summary>
        /// <param name="tileSource">TileSource name/key</param>
        /// <param name="autoTileSource">AutoTileSource name/key</param>
        /// <param name="tileW">Tile width</param>
        /// <param name="tileH">Tile height</param>
        /// <param name="winW">Window width</param>
        /// <param name="winH">window Height</param>
        /// <param name="winx">Window X position</param>
        /// <param name="winy">Window Y position</param>
        /// <param name="width">Width in tiles</param>
        /// <param name="height">Height in tiles</param>
        /// <param name="cellX">CellX value (used for compatability)</param>
        /// <param name="cellY">CellY value (used for compatability)</param>
        /// <param name="name">Name of XenoScene</param>
        public XenoScene(string tileSource, string autoTileSource, int tileW, int tileH, int winW, int winH, int winx, int winy, int width, int height, int cellX, int cellY, string name) : 
            base(TextureBank.getTexture(tileSource), TextureBank.getTexture(autoTileSource), width, height, tileW, tileH, winW, winH, winx, winy, cellX, cellY, name, false)
        {

        }
    }
    /// <summary>
    /// XenoGameScene class handles scene/single cell based, ie only uses 
    /// one cell at a time 
    /// </summary>
    public class XenoGameScene : XenoGame
    {
        //protected
        protected XenoScene scene;
        protected string sceneName;
        //public
        /// <summary>
        /// XenoGameScene constructor
        /// </summary>
        /// <param name="tileSource">TileSource name/key</param>
        /// <param name="autoTileSource">AutoTileSource name/key</param>
        /// <param name="x">Width in tiles</param>
        /// <param name="y">Width in tiles</param>
        /// <param name="winx">Window X position</param>
        /// <param name="winy">Window Y position</param>
        /// <param name="width">Width in tiles</param>
        /// <param name="height">Height in tiles</param>
        /// <param name="showCursor">ShowCursor flag</param>
        /// <param name="tileW">Tile width</param>
        /// <param name="tileH">Tile height</param>
        /// <param name="winW">Window width</param>
        /// <param name="winH">window Height</param>
        /// <param name="gameName">Name of game</param>
        /// <param name="sceneName">Name of XenoScene</param>
        public XenoGameScene(string tileSource, string autoTileSource, int x, int y, int winx, int winy, int width, int height, int showCursor, int tileW, int tileH, int winWidth, int winHeight, string gameName, string sceneName) : 
            base(x, y, width, height, gameName, showCursor)
        {
            scene = new XenoScene(tileSource, autoTileSource, width, height, tileW, tileH, winWidth, winHeight, 0, 0, 0, 0, sceneName);
            this.sceneName = sceneName;
        }
    }
}
