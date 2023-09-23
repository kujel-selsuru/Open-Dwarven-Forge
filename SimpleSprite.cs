//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

namespace XenoLib
{
    /// <summary>
    /// A four facings variable frames sprite with next position collsion and hitbox collision
    /// </summary>
    public enum simpleFacing { up = 0, right, down, left }
    /// <summary>
    /// SimpleSprite class
    /// </summary>
    public class SimpleSprite
    {
        //protected
        protected Texture2D source;
        protected SDL.SDL_Rect sourceBox;
        protected SDL.SDL_Rect destBox;
        protected SDL.SDL_Rect hitBox;
        protected SDL.SDL_Rect nextBox;
        protected Point2D pos;
        protected int width;
        protected int height;
        protected int numFrames;
        protected int frame;
        protected int rw;
        protected int rh;
        protected simpleFacing facing;
        protected Counter frameDelay;
        protected const int numFacings = 4;
        protected int speed;
        protected float speedz;
        protected List<Point2D> path;
        protected int pindex;
        protected int sector;//sector64
        public bool still;
        protected int bumps;
        protected int bounce;
        protected int radius;
        protected Objective objective;
        //public
        /// <summary>
        /// SimpleSprite constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="facing">Starting facing value</param>
        /// <param name="still">Still state</param>
        /// <param name="delay">Frame delay value</param>
        /// <param name="speed">Speed of object</param>
        /// <param name="rw">Region width</param>
        /// <param name="rh">Region height</param>
        /// <param name="sg">VariableSectorGraph reference</param>
        /// <param name="bounce">Bounce value</param>
        public SimpleSprite(Texture2D source, int x, int y, int numFrames, simpleFacing facing, bool still, int delay = 6, int speed = 2, int rw = 9600, int rh = 9600, VaribleSectorGraph sg = null, int bounce = 3)
        {
            this.source = source;
            width = calcWidth(source, numFrames);
            height = calcHeight(source);
            this.numFrames = numFrames;
            frame = 0;
            this.rw = rw;
            this.rh = rh;
            sourceBox.x = 0;
            sourceBox.y = (int)facing;
            sourceBox.w = width;
            sourceBox.h = height;
            hitBox.x = x;
            hitBox.y = y;
            hitBox.w = width;
            hitBox.h = height;
            destBox.x = x;
            destBox.y = y;
            destBox.w = width;
            destBox.h = height;
            nextBox.x = x;
            nextBox.y = y;
            nextBox.w = width;
            nextBox.h = height;
            pos = new Point2D(x, y);
            this.facing = facing;
            frameDelay = new Counter(delay);
            this.still = still;
            this.speed = speed;
            speedz = speed;
            path = new List<Point2D>();
            pindex = 0;
            bumps = 0;
            this.bounce = bounce;
            if (sg == null)
            {
                sector = -1;
            }
            else
            {
                sector = sg.findSector(x, y);
            }
            radius = hitBox.w / 2;
            objective = null;
        }
        /// <summary>
        /// SimpleSprite constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="facing">Starting facing</param>
        /// <param name="still">Still state</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="delay">Frame delay value</param>
        /// <param name="speed">Object speed</param>
        /// <param name="rw">Region width</param>
        /// <param name="rh">Region height</param>
        /// <param name="bounce">Bounce value</param>
        public SimpleSprite(Texture2D source, int x, int y, int numFrames, simpleFacing facing, bool still, int width, int height, int delay = 6, int speed = 2, int rw = 9600, int rh = 9600, int bounce = 3)
        {
            //width and height are hitbox and nextbox dimensions
            //this.width and this.height are destbox dimensions
            this.source = source;
            this.width = calcWidth(source, numFrames);
            this.height = calcHeight(source);
            this.numFrames = numFrames;
            frame = 0;
            sourceBox.x = 0;
            sourceBox.y = (int)facing;
            sourceBox.w = width;
            sourceBox.h = height;
            hitBox.x = x;
            hitBox.y = y;
            hitBox.w = width;
            hitBox.h = height;
            destBox.x = x;
            destBox.y = y;
            destBox.w = this.width;
            destBox.h = this.height;
            nextBox.x = x;
            nextBox.y = y;
            nextBox.w = width;
            nextBox.h = height;
            pos = new Point2D(x, y);
            this.facing = facing;
            frameDelay = new Counter(delay);
            this.still = still;
            this.speed = speed;
            speedz = speed;
            path = new List<Point2D>();
            pindex = 0;
            bumps = 0;
            this.bounce = bounce;
            radius = hitBox.w / 2;
            objective = null;
        }
        /// <summary>
        /// Calculates width of provided source based on provided number of frames
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="numFrames">Number of frames</param>
        /// <returns>Int</returns>
        protected int calcWidth(Texture2D source, int numFrames)
        {
            return source.width / numFrames;
        }
        /// <summary>
        /// Calculates the height of provided source based on number of facings
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <returns>Int</returns>
        protected int calcHeight(Texture2D source)
        {
            return source.height / numFacings;
        }
        /// <summary>
        /// Moves the next box to specified values
        /// </summary>
        /// <param name="shiftx">X value</param>
        /// <param name="shifty">Y value</param>
        public void setNextBox(int shiftx, int shifty)
        {
            nextBox.x = hitBox.x += shiftx;
            nextBox.y = hitBox.y += shifty;
        }
        /// <summary>
        /// Tests if next box intersects with target box
        /// </summary>
        /// <param name="target">Target box</param>
        /// <returns>Boolean</returns>
        public bool collideNextBox(SDL.SDL_Rect target)
        {
            return RectIntersecter.intersects(target, nextBox);
        }
        /// <summary>
        /// Tests if hit box intersects a provided target
        /// </summary>
        /// <param name="target">Target box</param>
        /// <returns>Boolean</returns>
        public bool collideHitBox(SDL.SDL_Rect target)
        {
            return RectIntersecter.intersects(target, hitBox);
        }
        /// <summary>
        /// Moves object a specified ammount
        /// </summary>
        /// <param name="shiftx">X value to shift</param>
        /// <param name="shifty">Y value to shift</param>
        public void move(int shiftx, int shifty)
        {
            hitBox.x += shiftx;
            hitBox.y += shifty;
        }
        /// <summary>
        /// Moves object a specified ammount
        /// </summary>
        /// <param name="shiftx">X value to shift</param>
        /// <param name="shifty">Y value to shift</param>
        public void move2(float shiftx, float shifty)
        {
            pos.X += shiftx;
            pos.Y += shifty;
            hitBox.x = (int)pos.X;
            hitBox.y = (int)pos.Y;
        }
        /// <summary>
        /// Sets object position to a given point
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public void teleport(int x, int y)
        {
            hitBox.x = x;
            hitBox.y = y;
        }
        /// <summary>
        /// Finds the side a target point is on of a point
        /// </summary>
        /// <param name="center">Center point</param>
        /// <param name="target">Target point</param>
        /// <param name="scale">Scaler value</param>
        /// <returns></returns>
        public static simpleFacing findSide(Point2D center, Point2D target, int scale = 32)
        {
            Point2D temp1 = new Point2D(center.X/scale, center.Y/scale);
            Point2D temp2 = new Point2D(target.X/scale, target.Y/scale);
            if (temp1.Y < temp2.Y)
            {
                if (temp1.X < temp2.X)
                {
                    return simpleFacing.right;
                }
                else if (temp1.X > temp2.Y)
                {
                    return simpleFacing.left;
                }
                else
                {
                    return simpleFacing.up;
                }
            }
            else
            {
                if (temp1.X < temp2.X)
                {
                    return simpleFacing.right;
                }
                else if (temp1.X > temp2.Y)
                {
                    return simpleFacing.left;
                }
                else
                {
                    return simpleFacing.down;
                }
            }
        }
        /// <summary>
        /// Draws sprite.
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Windows left side</param>
        /// <param name="winy">Windows top side</param>
        public void draw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            sourceBox.x = width * frame;
            sourceBox.y = height * (int)facing;
            destBox.x = hitBox.x - winx; //compensating for position relative to window
            destBox.y = hitBox.y - winy;
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
            SDL.SDL_RenderCopy(renderer, source.texture, ref sourceBox, ref destBox);
        }
        /// <summary>
        /// Draws sprite (depricated).
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="colour">SDL_Color referecne</param>
        /// <param name="winx">Windows left side</param>
        /// <param name="winy">Windows top side</param>
        public void draw(IntPtr renderer, SDL.SDL_Color colour, int winx = 0, int winy = 0)
        {
            sourceBox.x = width * frame;
            sourceBox.y = height * (int)facing;
            destBox.x = hitBox.x - winx; //compensating for position relative to window
            destBox.y = hitBox.y - winy;
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
            SDL.SDL_RenderCopy(renderer, source.texture, ref sourceBox, ref destBox);
        }
        /// <summary>
        /// Updates internal state, should be overridden in child class
        /// </summary>
        public virtual void update()
        {
            bumps = 0;
        }
        /// <summary>
        /// HitBox property
        /// </summary>
        public SDL.SDL_Rect HitBox
        {
            get { return hitBox; }
            set { hitBox = value; }
        }
        /// <summary>
        /// NextBox property
        /// </summary>
        public SDL.SDL_Rect NextBox
        {
            get { return nextBox; }
            set { nextBox = value; }
        }
        /// <summary>
        /// DestBox property
        /// </summary>
        public SDL.SDL_Rect DestBox
        {
            get { return destBox; }
            set { destBox = value; }
        }
        /// <summary>
        /// Sets dimensions of DestBox
        /// </summary>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        public void setDestBoxDimensions(int w, int h)
        {
            destBox.w = w;
            destBox.h = h;
        }
        /// <summary>
        /// Sets dimension of HitBox
        /// </summary>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        public void setHitBoxDimensions(int w, int h)
        {
            hitBox.w = w;
            hitBox.h = h;
        }
        /// <summary>
        /// Sets dimension of SourceBox
        /// </summary>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        public void setSourceBoxDimensions(int w, int h)
        {
            sourceBox.w = w;
            sourceBox.h = h;
        }
        /// <summary>
        /// Sets dimension of NextBox
        /// </summary>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        public void setNextBoxDimensions(int w, int h)
        {
            nextBox.w = w;
            nextBox.h = h;
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
        /// X property
        /// </summary>
        public int X
        {
            get { return hitBox.x; }
            set { hitBox.x = value; }
        }
        /// <summary>
        /// Y property
        /// </summary>
        public int Y
        {
            get { return hitBox.y; }
            set { hitBox.y = value; }
        }
        /// <summary>
        /// Returns current center of object
        /// </summary>
        public Point2D Center
        {
            get { return new Point2D(hitBox.x + (width/2), hitBox.y + (height/2)); }
        }
        /// <summary>
        /// Returns current center of object
        /// </summary>
        public Point2D Center2
        {
            get { return new Point2D(pos.X + (width / 2), pos.Y + (height / 2)); }
        }
        /// <summary>
        /// Attmepts to move object while testing for collisions
        /// </summary>
        /// <param name="shiftx">X shift value</param>
        /// <param name="shifty">Y shift value</param>
        /// <param name="boxes">List of hit boxes to test</param>
        /// <returns>Boolean</returns>
        public bool attemptMove(int shiftx, int shifty, List<SDL.SDL_Rect> boxes)
        {
            bool passable = true;
            setNextBox(shiftx, shifty);
            still = false;
            simpleFacing side = default(simpleFacing);
            for (int i = 0; i < boxes.Count; i++)
            {
                if (RectIntersecter.intersects(nextBox,boxes[i]))
                {
                    side = findSide(boxes[i]);
                    passable = false;
                    break;
                }
            }
            if (passable)
            {
                move(shiftx, shifty);
            }
            else
            {
                bumps++;
                
                switch(side)
                {
                    case simpleFacing.up:
                        move(-bounce * shiftx, -bounce * shifty);
                        break;
                    case simpleFacing.right:
                        move(-bounce * shiftx, -bounce * shifty);
                        break;
                    case simpleFacing.down:
                        move(-bounce * shiftx, -bounce * shifty);
                        break;
                    case simpleFacing.left:
                        move(-bounce * shiftx, -bounce * shifty);
                        break;
                }
                 
                //move(-2*shiftx, -2*shifty);
            }
            return passable;
        }
        /// <summary>
        /// Attempts to move object while testing a single collision target
        /// </summary>
        /// <param name="shiftx">X shift value</param>
        /// <param name="shifty">Y shift value</param>
        /// <param name="box">External hit box</param>
        /// <returns>Boolean</returns>
        public bool attemptMove(int shiftx, int shifty, SDL.SDL_Rect box)
        {
            bool passable = true;
            setNextBox(shiftx, shifty);
            still = false;
            simpleFacing side = default(simpleFacing);
            if (RectIntersecter.intersects(nextBox,box))
            {
                passable = false;
                side = findSide(box);
            }
            /*
            for (int i = 0; i < boxes.Count; i++)
            {
                if (nextBox.Intersects(boxes[i]))
                {
                    side = findSide(boxes[i]);
                    passable = false;
                    break;
                }
            }
             */ 
            if (passable)
            {
                move(shiftx, shifty);
            }
            else
            {
                bumps++;
                switch (side)
                {
                    case simpleFacing.up:
                        move(-bounce * shiftx, -bounce * shifty);
                        break;
                    case simpleFacing.right:
                        move(-bounce * shiftx, -bounce * shifty);
                        break;
                    case simpleFacing.down:
                        move(-bounce * shiftx, -bounce * shifty);
                        break;
                    case simpleFacing.left:
                        move(-bounce * shiftx, -bounce * shifty);
                        break;
                }
                //move(-2*shiftx, -2*shifty);
            }
            return passable;
        }
        /// <summary>
        /// Attempts to move in a provided direction
        /// </summary>
        /// <param name="facing">Direction to move in</param>
        /// <param name="grid">Object layer (DataGrid) reference</param>
        /// <param name="scale">Scaler value</param>
        /// <returns>Boolean</returns>
        public bool attemptMove(simpleFacing facing, DataGrid<SimpleSprite> grid, int scale = 32)
        {
            switch (facing)
            {
                case simpleFacing.up:
                    if (grid.inDomain((int)Center.X / scale, (int)(Center.Y / scale) - 1))//make sure object's target is in the domain
                    {
                        if (grid.Grid[(int)Center.X / scale, (int)(Center.Y / scale) - 1] == null)//check if the grid position is empty
                        {
                            move(0, -speed);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    break;
                case simpleFacing.right:
                    if (grid.inDomain((int)(Center.X / scale) + 1, (int)Center.Y / scale))//make sure object's target is in the domain
                    {
                        if (grid.Grid[(int)(Center.X / scale) + 1, (int)Center.Y / scale] == null)//check if the grid position is empty
                        {
                            move(speed, 0);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    break;
                case simpleFacing.down:
                    if (grid.inDomain((int)Center.X / scale, (int)(Center.Y / scale) + 1))//make sure object's target is in the domain
                    {
                        if (grid.Grid[(int)Center.X / scale, (int)(Center.Y / scale) + 1] == null)//check if the grid position is empty
                        {
                            move(0, speed);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    break;
                case simpleFacing.left:
                    if (grid.inDomain((int)(Center.X / scale) - 1, (int)Center.Y / scale))//make sure object's target is in the domain
                    {
                        if (grid.Grid[(int)(Center.X / scale) - 1, (int)Center.Y / scale] == null)//check if the grid position is empty
                        {
                            move(-speed, 0);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    break;
            }
            return false;
        }
        /// <summary>
        /// Attempts to move to a target point if path is clear
        /// </summary>
        /// <param name="x">X value of target</param>
        /// <param name="y">Y value of target</param>
        /// <param name="boxes">List of hit boxes</param>
        public void moveToPoint(int x, int y, List<SDL.SDL_Rect> boxes)
        {
            int shiftx = 0;
            if(Center.X < x)
            {
                shiftx = speed;
            }
            else if (Center.X > x)
            {
                shiftx = speed * -1;
            }
            int shifty = 0;
            if (Center.Y < y)
            {
                shifty = speed;
            }
            else if (Center.Y > y)
            {
                shifty = speed * -1;
            }
            setNextBox(shiftx, shifty);
            for (int i = 0; i < boxes.Count; i++)
            {
                if (RectIntersecter.intersects(nextBox,boxes[i]))
                {
                    shiftx *= -1;
                    shifty *= -1;
                    bumps++;
                    break;
                }
            }
            still = false;
            move(shiftx, shifty);
        }
        /// <summary>
        /// Sets current facing toward target point
        /// </summary>
        /// <param name="x">X value of target</param>
        /// <param name="y">Y value of target</param>
        public void facePoint(int x, int y)
        {
            int deltax = x - (int)Center.X;
            if (deltax < 0)
            {
                deltax *= -1;
            }
            int deltay = y - (int)Center.Y;
            if (deltay < 0)
            {
                deltay *= -1;
            }
            if (deltax > deltay)
            {
                if (Center.X > x)
                {
                    facing = simpleFacing.left;
                }
                else
                {
                    facing = simpleFacing.right;
                }
            }
            else
            {
                if(Center.Y > y)
                {
                    facing = simpleFacing.up;
                }
                else
                {
                    facing = simpleFacing.down;
                }
            }
        }
        /// <summary>
        /// Attempts to fallow path
        /// </summary>
        /// <param name="rects">List of external hit boxes</param>
        public void fallowPath(List<SDL.SDL_Rect> rects)
        {
            if (pindex < path.Count)
            {
                int shiftx = 0;
                int shifty = 0;
                if (hitBox.x != path[pindex].X)
                {
                    if (hitBox.x < path[pindex].X)
                    {
                        shiftx = speed;
                    }
                    else
                    {
                        shiftx = -speed;
                    }
                }
                if (hitBox.y != path[pindex].Y)
                {
                    if (hitBox.y < path[pindex].Y)
                    {
                        shifty = speed;
                    }
                    else
                    {
                        shifty = -speed;
                    }
                }
                facePoint((int)path[pindex].X, (int)path[pindex].Y);
                attemptMove(shiftx, shifty, rects);
                if (hitBox.x == path[pindex].X & hitBox.y == path[pindex].Y)
                {
                    pindex++;
                }
            }     
            else
            {
                path.Clear();
                still = true;
            }                 
        }
        /// <summary>
        /// Fallows an internally set path
        /// </summary>
        public void fallowPath()
        {
            if (pindex < path.Count)
            {
                int shiftx = 0;
                int shifty = 0;
                if (hitBox.x != path[pindex].X)
                {
                    if (hitBox.x < path[pindex].X)
                    {
                        shiftx = speed;
                    }
                    else
                    {
                        shiftx = -speed;
                    }
                }
                if (hitBox.y != path[pindex].Y)
                {
                    if (hitBox.y < path[pindex].Y)
                    {
                        shifty = speed;
                    }
                    else
                    {
                        shifty = -speed;
                    }
                }
                facePoint((int)path[pindex].X, (int)path[pindex].Y);
                move(shiftx, shifty);
                if (hitBox.x == path[pindex].X & hitBox.y == path[pindex].Y)
                {
                    pindex++;
                }
            }
            else
            {
                path.Clear();
                pindex = 0;
                still = true;
            }
        }
        /// <summary>
        /// Fallows an internally set path
        /// </summary>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="scale">Scaler value</param>
        /// <returns>Boolean</returns>
        public bool fallowPath(MapGraph mg, int scale = 32)
        {
            if (pindex < path.Count)
            {
                float shiftx = 0;
                float shifty = 0;
                if (hitBox.x != path[pindex].X)
                {
                    if (hitBox.y < path[pindex].X)
                    {
                        shiftx = speedz;
                    }
                    else
                    {
                        shiftx = -speedz;
                    }
                }
                if (hitBox.y != path[pindex].Y)
                {
                    if (hitBox.y < path[pindex].Y)
                    {
                        shifty = speed;
                    }
                    else
                    {
                        shifty = -speed;
                    }
                }
                facePoint((int)path[pindex].X, (int)path[pindex].Y);
                move2(shiftx, shifty);
                if (hitBox.x == path[pindex].X && hitBox.y == path[pindex].Y)
                {
                    if (path.Count > pindex + 1)
                    {
                        if (mg.getCell((int)path[pindex + 1].X / scale, (int)path[pindex + 1].Y / scale))
                        {
                            pindex++;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        path.Clear();
                        pindex = 0;
                        still = true;
                    }
                }
            }
            else
            {
                path.Clear();
                pindex = 0;
                still = true;
            }
            return true;
        }
        /// <summary>
        /// Returns the side a provided SDL_Rect is on of object
        /// </summary>
        /// <param name="rect">Target SDL_Rect</param>
        /// <returns>SimpleFacing</returns>
        public simpleFacing findSide(SDL.SDL_Rect rect)
        {
            int deltax = rect.x / 2 - (int)Center.X;
            if (deltax < 0)
            {
                deltax *= -1;
            }
            int deltay = rect.y / 2 - (int)Center.Y;
            if (deltay < 0)
            {
                deltay *= -1;
            }
            if (deltax > deltay)
            {
                if (Center.X > rect.x / 2)
                {
                    return simpleFacing.left;
                }
                else
                {
                    return simpleFacing.right;
                }
            }
            else
            {
                if (Center.Y > rect.y / 2)
                {
                    return simpleFacing.up;
                }
                else
                {
                    return simpleFacing.down;
                }
            }
        }
        /// <summary>
        /// Tests a radical collsion
        /// </summary>
        /// <param name="sprite">SimpleSprite reference</param>
        /// <returns>Boolean</returns>
        public bool radialCollision(SimpleSprite sprite)
        {
            int dist = Point2D.calculateDistance(Center, sprite.Center);
            if (radius + sprite.Radius > dist)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Facing property
        /// </summary>
        public simpleFacing Facing
        {
            get { return facing; }
            set { facing = value; }
        }
        /// <summary>
        /// Speed property
        /// </summary>
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        /// <summary>
        /// Speedz property
        /// </summary>
        public float Speedz
        {
            get { return speedz; }
            set { speedz = value; }
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
        /// Path property
        /// </summary>
        public List<Point2D> Path
        {
            get { return path; }
            set { path = value; }
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
        /// Bumps property
        /// </summary>
        public int Bumps
        {
            get { return bumps; }
            set { bumps = value; }
        }
        /// <summary>
        /// Region width property
        /// </summary>
        public int RW
        {
            get { return rw; }
            set { rw = value; }
        }
        /// <summary>
        /// Region height property
        /// </summary>
        public int RH
        {
            get { return rh; }
            set { rh = value; }
        }
        /// <summary>
        /// Radius property
        /// </summary>
        public int Radius
        {
            get { return radius; }
        }
        /// <summary>
        /// PathIndex property
        /// </summary>
        public int PathIndex
        {
            get { return pindex; }
            set { pindex = value; }
        }
        /// <summary>
        /// TargetObject property
        /// </summary>
        public Objective TargetObjective
        {
            get { return objective; }
            set { objective = value; }
        }
    }
    /// <summary>
    /// Inanimate object class
    /// </summary>
    public class Inanimate : SimpleSprite
    {
        //protected
        protected int hp;

        //public
        /// <summary>
        /// Inanimate constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="facing">Start facing value</param>
        /// <param name="still">Still state</param>
        public Inanimate(Texture2D source, int x, int y, int numFrames, simpleFacing facing, bool still) :
            base(source, x, y, numFrames, facing, still)
        {
            hp = 100;
        }
        /// <summary>
        /// Damage object
        /// </summary>
        /// <param name="dam"></param>
        public void damage(int dam)
        {
            hp -= dam;
            if (hp < 0)
            {
                hp = 0;
            }
        }
        /// <summary>
        /// HP property
        /// </summary>
        public int HP
        {
            get { return hp; }
        }
    }
}
