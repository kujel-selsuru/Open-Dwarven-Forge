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

namespace XenoLib
{
    /// <summary>
    /// DIRECT enumerator
    /// </summary>
    public enum DIRECT {UP = 0, UPRIGHT = 1, RIGHT = 2, DOWNRIGHT = 3, DOWN = 4, DOWNLEFT = 5, LEFT = 6, UPLEFT = 7}

    /// <summary>
    /// XenoSprite class
    /// </summary>
    public class XenoSprite
    {
        //protected
        protected Texture2D source;
        protected SDL2.SDL.SDL_Rect srcRect;
        protected SDL2.SDL.SDL_Rect destRect;
        protected int numFrames;
        protected int frame;
        protected bool still;
        protected DIRECT direct;
        protected Rectangle hitBox;
        protected List<Point2D> path;
        protected int hp;
        protected Objective target;
        protected int sector;
        protected Counter frameDelay;
        protected Counter moveDelay;
        protected int pathIndex;
        protected double selfAngle;
        protected string name;
        protected float speed;
        protected bool jump;
        protected bool falling;
        protected Counter leapCounter;
        protected float acceleration;
        protected Point2D center;

        //public
        /// <summary>
        /// XenoSprite constructor (internally sets source texture)
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="x">X positoin</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="hp">Hit points</param>
        /// <param name="delay">Frame delay value</param>
        /// <param name="Name">Source name</param>
        public XenoSprite(Texture2D source, float x, float y, int width, int height, int numFrames, int hp = 100, int delay = 5, string name = "")
        {
            this.source = source;
            srcRect.x = 0;
            srcRect.y = 0;
            srcRect.w = (int)width;
            srcRect.h = (int)height;
            destRect.x = (int)x;
            destRect.y = (int)y;
            destRect.w = (int)width;
            destRect.h = (int)height;
            this.numFrames = numFrames;
            frame = 0;
            still = true;
            direct = DIRECT.UP;
            hitBox = new Rectangle(x, y, (float)width, (float)height);
            path = null;
            this.hp = hp;
            target = null;
            sector = -1;
            frameDelay = new Counter(delay);
            moveDelay = new Counter(delay);
            pathIndex = 0;
            selfAngle = 0;
            this.name = name;
            speed = 0;
            jump = false;
            falling = false;
            leapCounter = new Counter(delay * 2);
            acceleration = 0;
            center = new Point2D(0, 0);
        }
        /// <summary>
        /// XenoSprite constructor
        /// </summary>
        /// <param name="Name">Source name</param>
        /// <param name="x">X positoin</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="hp">Hit points</param>
        /// <param name="delay">Frame delay value</param>
        public XenoSprite(string name, float x, float y, int width, int height, int numFrames, int hp = 100, int delay = 5)
        {
            this.source = TextureBank.getTexture(name);
            this.name = name;
            srcRect.x = 0;
            srcRect.y = 0;
            srcRect.w = (int)width;
            srcRect.h = (int)height;
            destRect.x = (int)x;
            destRect.y = (int)y;
            destRect.w = (int)width;
            destRect.h = (int)height;
            this.numFrames = numFrames;
            frame = 0;
            still = true;
            direct = DIRECT.UP;
            hitBox = new Rectangle(x, y, (float)width, (float)height);
            path = null;
            this.hp = hp;
            target = null;
            sector = -1;
            frameDelay = new Counter(delay);
            moveDelay = new Counter(delay);
            pathIndex = 0;
            selfAngle = 0;
            speed = 0;
            jump = false;
            falling = false;
            leapCounter = new Counter(delay * 2);
            acceleration = 0;
            center = new Point2D(0, 0);
        }
        /// <summary>
        /// XenoSprite SpriteProfile constructor
        /// </summary>
        /// <param name="sp">SpriteProfile reference</param>
        /// <param name="x">X positoin</param>
        /// <param name="y">Y position</param>
        /// <param name="hp">Hit points</param>
        /// <param name="delay">Frame delay value</param>
        public XenoSprite(SpriteProfile sp, float x, float y, int hp = 100, int delay = 5)
        {
            this.source = TextureBank.getTexture(sp.Name);
            this.name = sp.Name;
            srcRect.x = 0;
            srcRect.y = 0;
            srcRect.w = sp.Width;
            srcRect.h = sp.Height;
            destRect.x = (int)x;
            destRect.y = (int)y;
            destRect.w = sp.Width;
            destRect.h = sp.Height;
            this.numFrames = sp.NumFrames;
            frame = 0;
            still = true;
            direct = DIRECT.UP;
            hitBox = new Rectangle(x, y, sp.Width, sp.Height);
            path = null;
            this.hp = hp;
            target = null;
            sector = -1;
            frameDelay = new Counter(delay);
            moveDelay = new Counter(delay);
            pathIndex = 0;
            selfAngle = 0;
            speed = 0;
            jump = false;
            falling = false;
            leapCounter = new Counter(delay * 2);
            acceleration = 0;
            center = new Point2D(0, 0);
        }
        /// <summary>
        /// XenoSprite from file constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="sr">StreamReader reference</param>
        /// <param name="delay">Frame delay value</param>
        public XenoSprite(Texture2D source, StreamReader sr, int delay = 5)
        {
            this.source = source;
            string buffer = sr.ReadLine();
            srcRect.x = 0;
            srcRect.y = 0;
            srcRect.w = Convert.ToInt32(sr.ReadLine());
            srcRect.h = Convert.ToInt32(sr.ReadLine());
            destRect.x = Convert.ToInt32(sr.ReadLine());
            destRect.y = Convert.ToInt32(sr.ReadLine());
            destRect.w = srcRect.w;
            destRect.h = srcRect.h;
            numFrames = Convert.ToInt32(sr.ReadLine());
            frame = 0;
            still = Convert.ToBoolean(sr.ReadLine());
            direct = (DIRECT)Convert.ToInt32(sr.ReadLine());
            hitBox = new Rectangle(destRect.x, destRect.y, destRect.w, destRect.h);
            path = null;
            hp = Convert.ToInt32(sr.ReadLine());
            target = null;
            sector = -1;
            moveDelay = new Counter(delay);
            pathIndex = 0;
            selfAngle = Convert.ToDouble(sr.ReadLine());
            name = sr.ReadLine();
            if(name == null)
            {
                name = "";
            }
            if(name != "")
            {
                source = TextureBank.getTexture(name);
            }
            speed = (float)Convert.ToDouble(sr.ReadLine());
            jump = false;
            falling = false;
            leapCounter = new Counter(delay * 2);
            acceleration = 0;
            center = new Point2D(0, 0);
            frameDelay = new Counter(Convert.ToInt32(sr.ReadLine()));
        }
        /// <summary>
        /// XenoSprite from file constructor (internally sets source texture)
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        /// <param name="delay">Frame delay value</param>
        public XenoSprite(StreamReader sr, int delay = 5)
        {
            //this.source = source;
            string buffer = sr.ReadLine();
            srcRect.x = 0;
            srcRect.y = 0;
            buffer = sr.ReadLine();
            srcRect.w = Convert.ToInt32(buffer);
            buffer = sr.ReadLine();
            srcRect.h = Convert.ToInt32(buffer);
            buffer = sr.ReadLine();
            destRect.x = Convert.ToInt32(buffer);
            buffer = sr.ReadLine();
            destRect.y = Convert.ToInt32(buffer);
            destRect.w = srcRect.w;
            destRect.h = srcRect.h;
            buffer = sr.ReadLine();
            numFrames = Convert.ToInt32(buffer);
            frame = 0;
            still = Convert.ToBoolean(sr.ReadLine());
            direct = (DIRECT)Convert.ToInt32(sr.ReadLine());
            hitBox = new Rectangle(destRect.x, destRect.y, destRect.w, destRect.h);
            path = null;
            hp = Convert.ToInt32(sr.ReadLine());
            target = null;
            sector = -1;
            moveDelay = new Counter(delay);
            pathIndex = 0;
            selfAngle = Convert.ToDouble(sr.ReadLine());
            name = sr.ReadLine();
            if(name == null)
            {
                name = "";
            }
            if(name != "")
            {
                source = TextureBank.getTexture(name);
            }
            speed = (float)Convert.ToDouble(sr.ReadLine());
            jump = false;
            falling = false;
            leapCounter = new Counter(delay * 2);
            acceleration = 0;
            center = new Point2D(0, 0);
            frameDelay = new Counter(Convert.ToInt32(sr.ReadLine()));
        }
        /// <summary>
        /// XenoSprite copy constructor
        /// </summary>
        /// <param name="obj">XenoSprite reference</param>
        public XenoSprite(XenoSprite obj)
        {
            this.source = obj.Source;
            srcRect.x = 0;
            srcRect.y = 0;
            srcRect.w = (int)obj.W;
            srcRect.h = (int)obj.H;
            destRect.x = (int)obj.X;
            destRect.y = (int)obj.Y;
            destRect.w = (int)obj.W;
            destRect.h = (int)obj.H;
            this.numFrames = obj.NumFrames;
            frame = 0;
            still = true;
            direct = DIRECT.UP;
            hitBox = new Rectangle(obj.X, obj.Y, obj.W, obj.H);
            path = null;
            this.hp = obj.HP;
            target = null;
            sector = -1;
            frameDelay = new Counter(obj.FrameDelay.Max);
            pathIndex = 0;
            selfAngle = 0;
            this.name = obj.Name;
            speed = 0;
            jump = false;
            falling = false;
            leapCounter = new Counter(obj.FrameDelay.Max * 2);
            acceleration = 0;
            center = new Point2D(0 , 0);
        }
        /// <summary>
        /// Save data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public virtual void saveData(StreamWriter sw)
        {
            sw.WriteLine("======XenoSprite Data======");
            sw.WriteLine(Convert.ToInt32(hitBox.Width));
            sw.WriteLine(Convert.ToInt32(hitBox.Height));
            sw.WriteLine(hitBox.X);
            sw.WriteLine(hitBox.Y);
            sw.WriteLine(numFrames);
            sw.WriteLine(still);
            sw.WriteLine((int)direct);
            sw.WriteLine(hp);
            sw.WriteLine(selfAngle);
            sw.WriteLine(name);
            sw.WriteLine(speed);
            sw.WriteLine(frameDelay.Max);
        }
        /// <summary>
        /// Loads data without creating a new object
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public virtual void loadData(StreamReader sr)
        {
            string buffer = sr.ReadLine();
            srcRect.x = 0;
            srcRect.y = 0;
            buffer = sr.ReadLine();
            srcRect.w = Convert.ToInt32(buffer);
            buffer = sr.ReadLine();
            srcRect.h = Convert.ToInt32(buffer);
            destRect.x = Convert.ToInt32(sr.ReadLine());
            destRect.y = Convert.ToInt32(sr.ReadLine());
            destRect.w = srcRect.w;
            destRect.h = srcRect.h;
            numFrames = Convert.ToInt32(sr.ReadLine());
            frame = 0;
            still = Convert.ToBoolean(sr.ReadLine());
            direct = (DIRECT)Convert.ToInt32(sr.ReadLine());
            hitBox = new Rectangle(destRect.x, destRect.y, destRect.w, destRect.h);
            path = null;
            hp = Convert.ToInt32(sr.ReadLine());
            target = null;
            sector = -1;
            pathIndex = 0;
            selfAngle = Convert.ToDouble(sr.ReadLine());
            name = sr.ReadLine();
            if (name != "")
            {
                source = TextureBank.getTexture(name);
            }
            speed = (float)Convert.ToDouble(sr.ReadLine());
            jump = false;
            falling = false;
            leapCounter = new Counter(frameDelay.Max * 2);
            acceleration = 0;
            frameDelay = new Counter(Convert.ToInt32(sr.ReadLine()));
        }
        /// <summary>
        /// Draws XenoSprite
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        public virtual void draw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            destRect.x = (int)hitBox.X - winx;
            destRect.y = (int)hitBox.Y - winy;
            srcRect.x = frame * srcRect.w;
            srcRect.y = ((int)direct) * srcRect.h;
            SimpleDraw.draw(renderer, source, srcRect, destRect);
            if (!still)
            {
                if (frameDelay.tick())
                {
                    if (frame < numFrames - 1)
                    {
                        frame++;
                    }
                    else
                    {
                        frame = 0;
                    }
                }
            }
        }
        /// <summary>
        /// Draws a XenoSprite's path
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        /// <param name="scaler">Scaling value</param>
        public virtual void drawPath(IntPtr renderer, int winx = 0, int winy = 0, int scaler = 32)
        {
            if(path != null)
            {
                if(path.Count > 0)
                {
                    for(int p = pathIndex; p < path.Count; p++)
                    {
                        Point2D tp = Center;
                        tp.X -= (scaler / 2);
                        tp.Y -= (scaler / 2);
                        if (p <= 0)
                        {
                            DrawLine.draw(renderer, tp, path[0], ColourBank.getColour(XENOCOLOURS.BLUE),
                                winx - (scaler / 2), winy - (scaler / 2));
                        }
                        //less than the last element draw line to next
                        if(p < path.Count - 1)
                        {
                            DrawLine.draw(renderer, path[p], path[p + 1], ColourBank.getColour(XENOCOLOURS.BLUE), 
                                winx - (scaler / 2), winy - (scaler / 2));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Draws XenoSprite at a specified angle
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="angle">Angle of rotation (in degrees)</param>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        public virtual void draw(IntPtr renderer, double angle, int winx = 0, int winy = 0)
        {
            destRect.x = (int)hitBox.X - winx;
            destRect.y = (int)hitBox.Y - winy;
            srcRect.x = frame * srcRect.w;
            srcRect.y = ((int)direct) * srcRect.h;
            SimpleDraw.draw(renderer, source, srcRect, destRect, angle, new Point2D(destRect.w / 2, destRect.h /2), SDL.SDL_RendererFlip.SDL_FLIP_NONE);
            if (!still)
            {
                if (frameDelay.tick())
                {
                    if (frame < numFrames - 1)
                    {
                        frame++;
                    }
                    else
                    {
                        frame = 0;
                    }
                }
            }
        }
        /// <summary>
        /// Draws XenoSprite at a specified angle
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="angle">Angle of rotation (in degrees)</param>
        /// <param name="x">Y position</param>
        /// <param name="y">X position</param>
        public virtual void drawAtPos(IntPtr renderer, double angle, int x = 0, int y = 0)
        {
            destRect.x = x;
            destRect.y = y;
            srcRect.x = frame * srcRect.w;
            srcRect.y = ((int)direct) * srcRect.h;
            SimpleDraw.draw(renderer, source, srcRect, destRect, angle, new Point2D(destRect.w / 2, destRect.h / 2), SDL.SDL_RendererFlip.SDL_FLIP_NONE);
            if (!still)
            {
                if (frameDelay.tick())
                {
                    if (frame < numFrames - 1)
                    {
                        frame++;
                    }
                    else
                    {
                        frame = 0;
                    }
                }
            }
        }
        /// <summary>
        /// Draws XenoSprite at a specified position at angle
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="angle">Angle of rotation (in degrees)</param>
        /// <param name="x">Y position</param>
        /// <param name="y">X position</param>
        /// <param name="pivx">Pivot X position</param>
        /// <param name="pivy">Pivot Y position</param>
        public virtual void drawAtPos(IntPtr renderer, double angle, 
            int x = 0, int y = 0, int pivx = 0, int pivy = 0)
        {
            destRect.x = x;
            destRect.y = y;
            srcRect.x = frame * srcRect.w;
            srcRect.y = ((int)direct) * srcRect.h;
            SimpleDraw.draw(renderer, source, srcRect, destRect, angle, new Point2D(pivx, pivy), SDL.SDL_RendererFlip.SDL_FLIP_NONE);
            if (!still)
            {
                if (frameDelay.tick())
                {
                    if (frame < numFrames - 1)
                    {
                        frame++;
                    }
                    else
                    {
                        frame = 0;
                    }
                }
            }
        }
        /// <summary>
        /// Draws XenoSprite at specified position and specified
        /// dimensions
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="x">X position in pixels</param>
        /// <param name="y">Y position in pixels</param>
        /// <param name="w">Width in pixels</param>
        /// <param name="h">Height in pixels</param>
        public virtual void drawAtPos(IntPtr renderer, int x = 0, 
            int y = 0, int w = 0, int h = 0)
        {
            Rectangle tmpRect = new Rectangle(x, y, w, h);
            Rectangle tmpSrcRect = new Rectangle(0, 0, srcRect.w, srcRect.h);
            tmpSrcRect.X = frame * srcRect.w;
            tmpSrcRect.Y = ((int)direct) * srcRect.h;
            SimpleDraw.draw(renderer, source, tmpSrcRect, tmpRect);
            if (!still)
            {
                if (frameDelay.tick())
                {
                    if (frame < numFrames - 1)
                    {
                        frame++;
                    }
                    else
                    {
                        frame = 0;
                    }
                }
            }
        }
        /// <summary>
        /// Draws the XenoSprite's hitbox
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="colour">Color of hitbox</param>
        /// <param name="winx">Window X offset</param>
        /// <param name="winy">Window Y offset</param>
        public virtual void drawHitBox(IntPtr renderer, SDL.SDL_Color colour, int winx = 0, int winy = 0)
        {
            Rectangle tmp = new Rectangle(hitBox.X - winx, hitBox.Y - winy, hitBox.Width, hitBox.Height);
            DrawRects.drawRect(renderer, tmp, colour);
        }
        /// <summary>
        /// Advances the sprite frame wthout rendering
        /// </summary>
        public virtual void advanceFrame()
        {
            if (!still)
            {
                if (frameDelay.tick())
                {
                    if (frame < numFrames - 1)
                    {
                        frame++;
                    }
                    else
                    {
                        frame = 0;
                    }
                }
            }
        }
        /// <summary>
        /// Set XenoSprite position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void setPos(float x, float y)
        {
            hitBox.X = x;
            hitBox.Y = y;
        }
        /// <summary>
        /// Sets render and hitbox dimensions independent of
        /// source box dimentions
        /// </summary>
        /// <param name="w">Width in pixels</param>
        /// <param name="h">Height in pixels</param>
        public void setRenderSize(int w, int h)
        {
            destRect.w = w;
            destRect.h = h;
            hitBox.Width = w;
            hitBox.Height = h;
        }
        /// <summary>
        /// Move XenoSprite
        /// </summary>
        /// <param name="x">X shift value</param>
        /// <param name="y">Y shift value</param>
        public virtual void move(float x, float y)
        {
            hitBox.X += x;
            hitBox.Y += y;
        }
        /// <summary>
        /// Move XenoSprite in a direction by a specified speed
        /// </summary>
        /// <param name="angle">Direction of movement</param>
        /// <param name="speed">Speed of movement</param>
        public virtual void move(double angle, float speed)
        {
            hitBox.X += (float)(Math.Cos(Helpers<int>.degreesToRadians(angle)) * speed);
            hitBox.Y += (float)(Math.Sin(Helpers<int>.degreesToRadians(angle)) * speed);
        }
        /// <summary>
        /// Fallow XenoSprite's internally stored path
        /// </summary>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="speed">Speed of movement</param>
        /// <param name="shiftRight">Shift right value</param>
        /// <param name="shiftDown">Shift down value</param>
        /// <param name="scaler">Tile scaling value</param>
        public virtual void fallowPath(MapGraph mg, float speed, int shiftRight, int shiftDown, int scaler = 32)
        {
            int px, py;
            if (path != null)
            {
                if (pathIndex < path.Count && path.Count > 0)
                {
                    still = false;
                    px = (path[pathIndex].IX + shiftRight) / scaler;
                    py = (path[pathIndex].IY + shiftDown) / scaler;
                    //check for clear postion to move into
                    if (mg.getCell(px, py) == true)
                    {
                        //make sure spot is counted as occupied
                        //mg.setCell(px, py, true);
                        facePoint(path[pathIndex].X, path[pathIndex].Y, scaler);
                        if(hitBox.X > path[pathIndex].X)
                        {
                            //to avoid overshooting position move just enough to line up
                            if (hitBox.X - path[pathIndex].X > speed)
                            {
                                move(-speed, 0);
                            }
                            else
                            {
                                move(-(hitBox.X - path[pathIndex].X), 0);
                            }
                        }
                        else if(hitBox.X < path[pathIndex].X)
                        {
                            //to avoid overshooting position move just enough to line up
                            if (path[pathIndex].X - hitBox.X > speed)
                            {
                                move(speed, 0);
                            }
                            else
                            {
                                move((path[pathIndex].X - hitBox.X), 0);
                            }
                        }
                        if (hitBox.Y > path[pathIndex].Y)
                        {
                            //to avoid overshooting position move just enough to line up
                            if (hitBox.Y - path[pathIndex].Y > speed)
                            {
                                move(0, -speed);
                            }
                            else
                            {
                                move(0, -(hitBox.Y - path[pathIndex].Y));
                            }
                        }
                        else if(hitBox.Y < path[pathIndex].Y)
                        {
                            //to avoid overshooting position move just enough to line up
                            if (path[pathIndex].Y - hitBox.Y > speed)
                            {
                                move(0, speed);
                            }
                            else
                            {
                                move(0, (path[pathIndex].Y - hitBox.Y));
                            }
                        }
                        //check if reached point in path and increment pathIndex if true
                        if (hitBox.X == path[pathIndex].X && hitBox.Y == path[pathIndex].Y)
                        {
                            pathIndex++;
                            //if at end of path clear path list and reset pathIndex
                            if(pathIndex >= path.Count)
                            {
                                path = null;
                                pathIndex = 0;
                                still = true;
                            }
                        }
                    }
                    else
                    {
                        frame = 0;
                        still = true;
                    }
                }
            }
        }
        /// <summary>
        /// Fallow path using angular movement
        /// </summary>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="speed">Speed of movement</param>
        /// <param name="scaler">Scaling value (width of a tile)</param>
        public virtual void fallowPath(MapGraph mg, float speed, int scaler = 32)
        {
            if (path != null)
            {
                if (pathIndex <= path.Count - 1 && path.Count > 0)
                {
                    //check for clear postion to move into
                    if (mg.getCell(path[pathIndex].IX / scaler, path[pathIndex].IY / scaler) == true)
                    {
                        //make sure spot is counted as occupied
                        //mg.setCell(path[pathIndex].IX, path[pathIndex].IY, false);
                        //calculate angle to target point
                        selfAngle = Point2D.CalcAngle(hitBox.Center, path[pathIndex]);
                        //if distence to target point less than hitBox.Width increment pathIndex
                        if(Point2D.AsqrtB(hitBox.Center, path[pathIndex]) < hitBox.Width)
                        {
                            pathIndex++;
                            //if at end of path clear path list and reset pathIndex
                            if (pathIndex >= path.Count - 1)
                            {
                                path = null;
                                pathIndex = 0;
                            }
                        }
                        else //distence to point greater than hitBox.Width move in that direction
                        {
                            facePoint(path[pathIndex].X, path[pathIndex].Y, scaler);
                            move(selfAngle, speed);
                        }
                    }

                }
            }
        }
        /// <summary>
        /// Fallow path for platformer movement
        /// </summary>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="speed">Speed of movement</param>
        /// <param name="scaler">Scaling value (width of a tile)</param>
        public virtual void fallowPlatformPath(MapGraph mg, float speed, int scaler = 32)
        {
            int tmpSpd = 0;
            if (path != null)
            {
                if (pathIndex <= path.Count - 1 && path.Count > 0)
                {
                    if (moveDelay.tick() == true)
                    {
                        still = false;
                        //check for clear postion to move into (true value)
                        if (mg.getCell(path[pathIndex].IX / scaler, path[pathIndex].IY / scaler) == true)
                        {
                            //face either right or left
                            bool facingRight = true;
                            //move left if to right
                            if (Left < path[pathIndex].X)
                            {
                                tmpSpd = Math.Abs(Left - path[pathIndex].IX);
                                if(tmpSpd < speed)
                                {
                                    move(tmpSpd, 0);
                                }
                                else
                                {
                                    move(speed, 0);
                                }
                                facingRight = false;
                            }
                            //move right if to left
                            if (Left > path[pathIndex].X)
                            {
                                tmpSpd = Math.Abs(Left - path[pathIndex].IX);
                                if (tmpSpd < speed)
                                {
                                    move(-tmpSpd, 0);
                                }
                                else
                                {
                                    move(-speed, 0);
                                }
                                facingRight = true;
                            }
                            //move up if below
                            if (Bottom - scaler > path[pathIndex].Y)
                            {
                                tmpSpd = Math.Abs((Bottom - scaler) - path[pathIndex].IX);
                                if (tmpSpd < speed)
                                {
                                    move(0, -tmpSpd);
                                }
                                else
                                {
                                    move(0, -speed);
                                }
                            }
                            //move down if above
                            if (Bottom - scaler < path[pathIndex].Y)
                            {
                                tmpSpd = Math.Abs((Bottom - scaler) - path[pathIndex].IX);
                                if (tmpSpd < speed)
                                {
                                    move(0, tmpSpd);
                                }
                                else
                                {
                                    move(0, speed);
                                }
                            }
                            if (Left == path[pathIndex].IX)
                            {
                                if (Bottom - scaler == path[pathIndex].IY)
                                {
                                    pathIndex++;
                                }
                            }
                            //if at end of path clear path list and reset pathIndex
                            if (pathIndex >= path.Count - 1)
                            {
                                path = null;
                                pathIndex = 0;
                                still = true;
                            }
                            //set facing
                            if (facingRight == false)
                            {
                                direct = DIRECT.LEFT;
                            }
                            else
                            {
                                direct = DIRECT.RIGHT;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Fallow path using angular movement
        /// </summary>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="speed">Speed of movement</param>
        /// <param name="offsetx">offsetx value</param>
        /// <param name="offsety">offsety value</param>
        /// <param name="scaler">scaler value</param>
        public virtual void fallowPath2(MapGraph mg, float speed, int offsetx = 0, int offsety = 0, int scaler = 32)
        {
            if (path != null)
            {
                if (pathIndex <= path.Count - 1 && path.Count > 0)
                {
                    Point2D tmpPos = new Point2D(hitBox.X - offsetx, hitBox.Y - offsety);
                    int px = (path[pathIndex].IX) / scaler;
                    int py = (path[pathIndex].IY) / scaler;
                    //check for clear postion to move into
                    if (mg.getCell(px, py) == true)
                    {
                        //error if set to occupied//make sure spot is counted as occupied
                        //mg.setCell(px, py, false);
                        //calculate angle to target point
                        selfAngle = Point2D.CalcAngle(tmpPos, path[pathIndex]);
                        //if distence to target point less than hitBox.Width increment pathIndex
                        if (Point2D.AsqrtB(tmpPos, path[pathIndex]) < hitBox.Width)
                        {
                            pathIndex++;
                            //if at end of path clear path list and reset pathIndex
                            if (pathIndex >= path.Count - 1)
                            {
                                path = null;
                                pathIndex = 0;
                            }
                        }
                        else //distence to point greater than hitBox.Width move in that direction
                        {
                            facePoint(path[pathIndex].X, path[pathIndex].Y, scaler);
                            move(selfAngle, speed);
                        }
                    }

                }
            }
        }
        /// <summary>
        /// Fallow a hop path using angular movement and leaps when dx or dy is 
        /// greater than a tile width/height "scaler".
        /// </summary>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="speed">Speed of movement</param>
        /// <param name="scaler">W/H of a tile box</param>
        /// <param name="planetoid">XenoPlanetoid reference</param>
        /// <param name="leapStrength">Strength of leap against gravity</param>
        public virtual void fallowHopPath(MapGraph mg, float speed, float scaler, XenoPlanetoid planetoid,  double gravityAng, float gravity, float leapStrength = 8)
        {
            if(jump == true)
            {
                if(leapCounter.tick() == true)
                {
                    jump = false;
                    falling = true;
                }
                if (jump == true)
                {
                    leap(gravityAng, gravity);
                }
            }
            if(falling == true)
            {
                int txl = Center.IX + (int)(Math.Cos(Helpers<int>.degreesToRadians(gravityAng + 135)) * hitBox.Width);
                int tyl = Center.IY + (int)(Math.Sin(Helpers<int>.degreesToRadians(gravityAng + 135)) * hitBox.Width);
                int txr = Center.IX + (int)(Math.Cos(Helpers<int>.degreesToRadians(gravityAng + 45)) * hitBox.Width);
                int tyr = Center.IY + (int)(Math.Sin(Helpers<int>.degreesToRadians(gravityAng + 45)) * hitBox.Width);
                if(planetoid.Mass.Grid[txl, tyl] != null)
                {
                    falling = false;
                }
                if (planetoid.Mass.Grid[txr, tyr] != null)
                {
                    falling = false;
                }
                if(falling == true)
                {
                    fall(gravityAng, gravity);
                }
                
            }
            if (path != null)
            {
                if (pathIndex <= path.Count - 1 && path.Count > 0)
                {
                    //check for clear postion to move into
                    if (mg.getCell(path[pathIndex].IX, path[pathIndex].IY) == false)
                    {
                        //make sure spot is counted as occupied
                        mg.setCell(path[pathIndex].IX, path[pathIndex].IY, true);
                        //calculate angle to target point
                        selfAngle = Point2D.CalcAngle(hitBox.Center, path[pathIndex]);
                        //if distence to target point less than hitBox.Width increment pathIndex
                        if (Point2D.AsqrtB(hitBox.Center, path[pathIndex]) < hitBox.Width)
                        {
                            pathIndex++;
                            //if at end of path clear path list and reset pathIndex
                            if (pathIndex >= path.Count - 1)
                            {
                                path = null;
                                pathIndex = 0;
                            }
                        }
                        else //distence to point greater than hitBox.Width move in that direction
                        {
                            //difference in X or Y axis is greater than one tile so activate jump
                            if(path[pathIndex].X - hitBox.X > scaler || path[pathIndex].Y - hitBox.Y > scaler)
                            {
                                if(jump == false)
                                {
                                    jump = true;
                                    leapOn(leapStrength);
                                }
                            }
                            else
                            {
                                move(selfAngle, speed);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Fall after jumping
        /// </summary>
        /// <param name="gravityAng">Angle of gravity</param>
        /// <param name="gravity">Gravity strength</param>
        public virtual void fall(double gravityAng, float gravity)
        {
            acceleration += gravity;
            float dx = (float)Math.Cos(Helpers<int>.degreesToRadians(gravityAng)) * acceleration;
            float dy = (float)Math.Sin(Helpers<int>.degreesToRadians(gravityAng)) * acceleration;
            move(dx, dy);
        }
        /// <summary>
        /// Leap against gravity while jumping
        /// </summary>
        /// <param name="gravityAng">Angle of gravity</param>
        /// <param name="gravity">Gravity strength</param>
        public virtual void leap(double gravityAng, float gravity)
        {
            acceleration -= gravity;
            float dx = (float)Math.Cos(Helpers<int>.degreesToRadians(gravityAng + 180)) * acceleration;
            float dy = (float)Math.Sin(Helpers<int>.degreesToRadians(gravityAng + 180)) * acceleration;
            move(dx, dy);
        }
        /// <summary>
        /// Sets acceleration;
        /// </summary>
        /// <param name="power">Value to set acceleration to</param>
        public virtual void leapOn(float power)
        {
            acceleration = power;
        }
        /// <summary>
        /// Set XenoSprite's direct toward a point
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="scaler">Tile scaler value</param>
        public virtual void facePoint(float x, float y, int scaler)
        {
            int xenoX = (int)hitBox.Center.X / scaler;
            int xenoY = (int)hitBox.Center.Y / scaler;
            int tmpX = (int)x / scaler;
            int tmpY = (int)y / scaler;
            
            if(xenoX == tmpX)//point is either directly above or below XenoSprite
            {
                //point above XenoSprite
                if (xenoY > tmpY)
                {
                    direct = DIRECT.UP;
                }
                else if (xenoY < tmpY)//point above XenoSprite
                {
                    direct = DIRECT.DOWN;
                }
            }
            else if (xenoY == tmpY)//point is either directly left or right of XenoSprite
            {

                //point left of XenoSprite
                if (xenoX > tmpX)
                {
                    direct = DIRECT.LEFT;
                }
                else if (xenoX < tmpX)//point right of XenoSprite
                {
                    direct = DIRECT.RIGHT;
                }
            }
            else if (xenoX > tmpX)//point to left of XenoSprite
            {
                //point above XenoSprite
                if (xenoY > tmpY)
                {
                    direct = DIRECT.UPLEFT;
                }
                else if (xenoY < tmpY)//point above XenoSprite
                {
                    direct = DIRECT.DOWNLEFT;
                }
            }
            else if (xenoX < tmpX)//point to right of XenoSprite
            {
                //point above XenoSprite
                if (xenoY > tmpY)
                {
                    direct = DIRECT.UPRIGHT;
                }
                else if (xenoY < tmpY)//point above XenoSprite
                {
                    direct = DIRECT.DOWNRIGHT;
                }
            }
        }
        /// <summary>
        /// Damage XenoSprite, is ment to be overridden
        /// </summary>
        /// <param name="dam">damage value</param>
        public virtual void damage(int dam)
        {
            hp -= dam;
        }
        /// <summary>
        /// Sets destRect's position
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        public void setDestRectPos(int x, int y)
        {
            destRect.x = x;
            destRect.y = y;
        }
        /// <summary>
        /// Path property
        /// </summary>
        public List<Point2D> Route
        {
            get { return path; }
            set { path = value; }
        }
        /// <summary>
        /// SrcRect property
        /// </summary>
        public SDL.SDL_Rect SrcRect
        {
            get { return srcRect; }
            set { srcRect = value; }
        }
        /// <summary>
        /// DestRect property
        /// </summary>
        public SDL.SDL_Rect DestRect
        {
            get { return destRect; }
            set { destRect = value; }
        }
        /// <summary>
        /// RouteIndex property
        /// </summary>
        public int RouteIndex
        {
            get { return pathIndex; }
        }
        /// <summary>
        /// X property
        /// </summary>
        public float X
        {
            get { return hitBox.X; }
            set { hitBox.X = value; }
        }
        /// <summary>
        /// IX property
        /// </summary>
        public int IX
        {
            get { return (int)hitBox.X; }
            set { hitBox.X = value; }
        }
        /// <summary>
        /// Y property
        /// </summary>
        public float Y
        {
            get { return hitBox.Y; }
            set { hitBox.Y = value; }
        }
        /// <summary>
        /// IY property
        /// </summary>
        public int IY
        {
            get { return (int)hitBox.Y; }
            set { hitBox.Y = value; }
        }
        /// <summary>
        /// Width property
        /// </summary>
        public float W
        {
            get { return hitBox.Width; }
            set { hitBox.Width = value; }
        }
        /// <summary>
        /// Height property
        /// </summary>
        public float H
        {
            get { return hitBox.Height; }
            set { hitBox.Height = value; }
        }
        /// <summary>
        /// Source Width property
        /// </summary>
        public int SW
        {
            get { return srcRect.w; }
            set { srcRect.w = value; }
        }
        /// <summary>
        /// Source Height property
        /// </summary>
        public int SH
        {
            get { return srcRect.h; }
            set { srcRect.h = value; }
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
        /// Name property
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// NumFrames property
        /// </summary>
        public int NumFrames
        {
            get { return numFrames; }
            set { numFrames = value; }
        }
        /// <summary>
        /// HitBox property
        /// </summary>
        public Rectangle HitBox
        {
            get { return hitBox; }
        }
        /// <summary>
        /// HP property
        /// </summary>
        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }
        /// <summary>
        /// Still property
        /// </summary>
        public bool Still
        {
            get { return still; }
            set { still = value; }
        }
        /// <summary>
        /// FrameDelay property
        /// </summary>
        public Counter FrameDelay
        {
            get { return frameDelay; }
        }
        /// <summary>
        /// Target property
        /// </summary>
        public Objective Target
        {
            get { return target; }
            set { target = value; }
        }
        /// <summary>
        /// Sector property
        /// </summary>
        public int Sector
        {
            get { return sector; }
            set { sector = value; }
        }
        /// <summary>
        /// Direct property
        /// </summary>
        public DIRECT Direct
        {
            get { return direct; }
            set { direct = value; }
        }
        /// <summary>
        /// SelfAngle property
        /// </summary>
        public double SelfAngle
        {
            get { return selfAngle; }
            set { selfAngle = value; }
        }
        /// <summary>
        /// Left property
        /// </summary>
        public int Left
        {
            get { return (int)hitBox.X; }
        }
        /// <summary>
        /// Right property
        /// </summary>
        public int Right
        {
            get { return (int)(hitBox.X + hitBox.Width); }
        }
        /// <summary>
        /// Top property
        /// </summary>
        public int Top
        {
            get { return (int)hitBox.Y; }
        }
        /// <summary>
        /// Waist height property
        /// </summary>
        public int Waist
        {
            get { return (int)(hitBox.Y + (hitBox.Height / 2)); }
        }
        /// <summary>
        /// Bottom property
        /// </summary>
        public int Bottom
        {
            get { return (int)(hitBox.Y + hitBox.Height); }
        }
        /// <summary>
        /// Center property
        /// </summary>
        public Point2D Center
        {
            get
            {
                center.X = hitBox.X + (hitBox.Width / 2);
                center.Y = hitBox.Y + (hitBox.Height / 2);
                return center;
            }
        }
        /// <summary>
        /// Speed property
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        /// <summary>
        /// Jump property
        /// </summary>
        public bool Jump
        {
            get { return jump; }
            set { jump = value; }
        }
        /// <summary>
        /// LeapCounter property
        /// </summary>
        public Counter LeapCounter
        {
            get { return leapCounter; }
        }
        /// <summary>
        /// Acceleration property
        /// </summary>
        public float Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }
        /// <summary>
        /// Returns top left corner
        /// </summary>
        public Point2D TopLeft
        {
            get { return new Point2D(hitBox.X, hitBox.Y); }
        }
        /// <summary>
        /// Returns top right corner
        /// </summary>
        public Point2D TopRight
        {
            get { return new Point2D(hitBox.X + hitBox.Width, hitBox.Y); }
        }
        /// <summary>
        /// Returns bottom left corner
        /// </summary>
        public Point2D BottomLeft
        {
            get { return new Point2D(hitBox.X, hitBox.Y + hitBox.Height); }
        }
        /// <summary>
        /// Returns bottom right corner
        /// </summary>
        public Point2D BottomRight
        {
            get { return new Point2D(hitBox.X + hitBox.Width, hitBox.Y + hitBox.Height); }
        }
        /// <summary>
        /// Frame property
        /// </summary>
        public int Frame
        {
            get { return frame; }
            set
            {
                if(value >= 0 && value < numFrames)
                {
                    frame = value;
                }
            }
        }
    }
    /// <summary>
    /// Stores Sprite profile data for easier loading
    /// used to load sprite data from file name
    /// PROFILE SOURCE NAME FORMAT : SpriteName_widthValue_heightValue_numFramesValue
    /// </summary>
    public class SpriteProfile
    {
        //protected
        protected string name;
        protected int width;
        protected int height;
        protected int numFrames;

        //public
        /// <summary>
        /// SpriteProfile constructor (assumes 8 directions of movement)
        /// </summary>
        /// <param name="name">Name of sprite</param>
        /// <param name="width">Width of sprite in pixels</param>
        /// <param name="height">Height of sprite in pixels</param>
        /// <param name="numFrames">Number of frames of animation</param>
        public SpriteProfile(string name, int width, int height, int numFrames)
        {
            this.name = name;
            this.width = width;
            this.height = height;
            this.numFrames = numFrames;
        }
        /// <summary>
        /// Name property
        /// </summary>
        public string Name
        {
            get { return name; }
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
        /// NumFrames property
        /// </summary>
        public int NumFrames
        {
            get { return numFrames; }
        }
    }
    /// <summary>
    /// Stores SpriteProfiles to facilitate auto sprite loading
    /// </summary>
    public static class SPBank
    {
        //private
        static GenericBank<SpriteProfile> profiles;

        //public
        /// <summary>
        /// SPBank constructor
        /// </summary>
        static SPBank()
        {
            profiles = new GenericBank<SpriteProfile>();
        }
        /// <summary>
        /// Gets a stored profile from the SPBank
        /// </summary>
        /// <param name="key">Sprite Profile key value</param>
        /// <returns>SPriteProfile object</returns>
        public static SpriteProfile getProfile(string key)
        {
            SpriteProfile profile = null;
            profile = profiles.getData(key);
            return profile;
        }
        /// <summary>
        /// Loads a png file into TextureBank and crates a matching SpriteProfile
        /// that is added to the SPBank to aid loading sprites
        /// </summary>
        /// <param name="path">Full file path</param>
        /// <param name="renderer">Renderer reference</param>
        /// <returns>True if sucessful else returns false</returns>
        public static bool addProfile(string path, IntPtr renderer)
        {
            SDL.SDL_Color magenta = ColourBank.getColour(XENOCOLOURS.MAGENTA);
            string name = Helpers<int>.extractFileName(path);
            char[] underscore = { '_' };
            string[] pathData = StringParser.parse(name, underscore);
            //check that the format is correct
            if (pathData.Length < 4)
            {
                if(Helpers<int>.containsOnlyNumbers(pathData[1]) == false)
                {
                    return false;
                }
                if (Helpers<int>.containsOnlyNumbers(pathData[2]) == false)
                {
                    return false;
                }
                if (Helpers<int>.containsOnlyNumbers(pathData[3]) == false)
                {
                    return false;
                }
                string sourceName = pathData[0];
                int w = Convert.ToInt32(pathData[1]);
                int h = Convert.ToInt32(pathData[2]);
                int nf = Convert.ToInt32(pathData[3]);
                TextureBank.addTexture(sourceName, TextureLoader.load(path, renderer, magenta, w, h));
                SpriteProfile profile = new SpriteProfile(sourceName, w, h, nf);
                profiles.addData(sourceName, profile);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Checks for a key present in SPBank
        /// </summary>
        /// <param name="key">Key/name value</param>
        /// <returns>Boolean</returns>
        public static bool contains(string key)
        {
            return profiles.containsKey(key);
        }
        /// <summary>
        /// Names property
        /// </summary>
        public static List<string> Names
        {
            get { return profiles.Names; }
        }
    }


}
