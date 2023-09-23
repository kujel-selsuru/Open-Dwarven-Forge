//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace XenoLib
{

    /// <summary>
    /// PlatformerNode
    /// </summary>
    public class PlatformerNode : PathNode
    {
        //protected
        protected bool jumping;
        protected int jumpMoves;

        //public
        /// <summary>
        /// PlatformerNode constructor
        /// </summary>
        /// <param name="jumping">Jumping flag</param>
        /// <param name="jumpMoves">Number of moves while jumping</param>
        /// <param name="moves">Moves in total</param>
        /// <param name="distance">Distince to target</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="parent">Platformer reference</param>
        public PlatformerNode(bool jumping, int jumpMoves, int moves, int distance, int x, int y, PlatformerNode parent = null) : 
            base(moves, distance, x, y, parent)
        {
            this.jumping = jumping;
            this.jumpMoves = jumpMoves;
        }
        /// <summary>
        /// Overrides finalCost func
        /// </summary>
        /// <returns>Final cost of a move</returns>
        public override int finalcost()
        {
            return base.finalcost() + (jumpMoves / 2);
        }
        /// <summary>
        /// Jumping property
        /// </summary>
        public bool Jumping
        {
            get { return jumping; }
            set { jumping = value; }
        }
        /// <summary>
        /// JumpMoves property
        /// </summary>
        public int JumpMoves
        {
            get { return jumpMoves; }
        }
    }

    public enum PLATFORMMOVES { landed = 0, fall = 1, jump = 2};
    /// <summary>
    /// PlatformNode
    /// </summary>
    public class PlatformNode : PathNode
    {
        //protected
        protected PLATFORMMOVES platform;
        protected int jumpMoves;

        //public
        /// <summary>
        /// PlatformNode constructor
        /// </summary>
        /// <param name="platform">Platform flag</param>
        /// <param name="jumpMoves">Number of moves while jumping</param>
        /// <param name="moves">Moves in total</param>
        /// <param name="distance">Distince to target</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="parent">Platformer reference</param>
        public PlatformNode(PLATFORMMOVES platform, int jumpMoves, int moves, int distance, int x, int y, PlatformNode parent = null) :
            base(moves, distance, x, y, parent)
        {
            this.jumpMoves = jumpMoves;
        }
        /// <summary>
        /// Overrides finalCost func
        /// </summary>
        /// <returns>Final cost of a move</returns>
        public override int finalcost()
        {
            return base.finalcost() + (jumpMoves / 2);
        }
        /// <summary>
        /// Plartform property
        /// </summary>
        public PLATFORMMOVES Platform
        {
            get { return platform; }
            set { platform = value; }
        }
        /// <summary>
        /// JumpMoves property
        /// </summary>
        public int JumpMoves
        {
            get { return jumpMoves; }
        }
    }
    /// <summary>
    /// Finds a path around a center of gravity on a grid of blocks
    /// </summary>
    public class PlatformerPathFinder : SimplePathFinder
    {
        //protected
        protected int playerHeight;
        protected int jumpHeight;
        protected double angle;
        protected Point2D center;
        protected new PriorityQueue<PlatformNode> open;
        protected new List<PlatformNode> closed;

        //public
        /// <summary>
        /// PlatformPathFinder constructor
        /// </summary>
        /// <param name="playerHeight">Player height in blocks</param>
        /// <param name="jumpHeight">Jump height in blocks</param>
        /// <param name="center">Center of gavity</param>
        /// <param name="scale">Scaler value</param>
        /// <param name="maxSearches">Max searches</param>
        /// <param name="mg">MapGRaph</param>
        /// <param name="minRange">Min number of tiles to target</param>
        public PlatformerPathFinder(int playerHeight, int jumpHeight, Point2D center, int scale, int maxSearches, MapGraph mg, int minRange) : 
            base(scale, maxSearches, mg, minRange)
        {
            this.playerHeight = playerHeight;
            this.jumpHeight = jumpHeight;
            this.center = center;
            angle = 0;
        }
        /// <summary>
        /// Calculates a path from start postion to end position
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns>List of Point2D objects</returns>
        public List<Point2D> findPlatformerPath(Point2D startPoint, Point2D endPoint)
        {
            open = new PriorityQueue<PlatformNode>();
            closed = new List<PlatformNode>();
            Point2D start = new Point2D(startPoint.IX / scale, startPoint.IY / scale);
            Point2D end = new Point2D(endPoint.IX / scale, endPoint.IY / scale);

            PlatformNode current = new PlatformNode(PLATFORMMOVES.landed, 0, 0, Point2D.calculateDistance(start, end), (int)start.IX, (int)start.IY, null);
            int searches = 0;
            setPlatformerNodes(current, end);
            bool running = true;

            while (running)
            {
                //base cases
                if (current == null)
                {
                    return new List<Point2D>();
                }
                if (current.p.IX == end.IX)
                {
                    if (current.p.IY == end.IY)
                    {
                        running = false;
                    }
                }
                if (searches >= maxSearches)
                {
                    running = false;
                }
                if (current.distance <= minRange)
                {
                    running = false;
                }

                //search closed for potentially shorter paths
                for (int i = 0; i < closed.Count; i++)
                {
                    if (current.p.IX == closed[i].p.IX)
                    {
                        if (current.p.IY == closed[i].p.IY)
                        {
                            if (current.finalcost() < closed[i].finalcost())
                            {
                                closed[i] = current;
                                break;
                            }
                        }
                    }
                }

                //while the open priority queue is not empty get the first node and set to current
                if (open.Count == 0)
                {
                    running = false;
                }
                else
                {
                    setPlatformerNodes(current, end);
                    current = open.dequeue();
                }
                searches++;
            }

            return scalePath(current);
        }
        /// <summary>
        /// Sets nodes with down being the angle between a node and the center of gravity
        /// right - 90 degrees
        /// left - 180 degrees
        /// </summary>
        /// <param name="current">PlatformerNode reference</param>
        /// <param name="end">End point of path</param>
        public void setPlatformerNodes(PlatformNode current, Point2D end)
        {
            angle = Point2D.CalcAngle(center, current.p);//this is down
            double focusAngle = angle;
            bool found = false;
            Point2D p = new Point2D(0, 0);
            Point2D pad = new Point2D(0, 0);
            Point2D roof = new Point2D(0, 0);
            PlatformNode node = null;
            if (current.Platform == PLATFORMMOVES.landed)
            {
                //move right
                focusAngle = angle - 90;
                p.X = current.p.IX + (int)(Math.Cos(Helpers<int>.degreesToRadians(focusAngle)) * 2);
                p.Y = current.p.IY + (int)(Math.Sin(Helpers<int>.degreesToRadians(focusAngle)) * 2);
                if (mg.isValidPoint(p.IX, p.IY) == true)
                {
                    //footing is valid
                    pad.X = current.p.IX + (int)(Math.Cos(Helpers<int>.degreesToRadians(focusAngle)) * 2);
                    pad.Y = current.p.IY + (int)(Math.Sin(Helpers<int>.degreesToRadians(focusAngle)) * 2);
                    if (mg.getCell(pad.IX, pad.IY) == true)
                    {
                        found = false;
                        for (int c = 0; c < closed.Count; c++)
                        {
                            if (closed[c].p.IX == p.IX && closed[c].p.IY == p.IY && closed[c].finalcost() > current.finalcost())
                            {
                                closed[c] = new PlatformNode(PLATFORMMOVES.landed, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            node = new PlatformNode(PLATFORMMOVES.landed, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                            open.enqueue(node, current.moves + 1);
                        }
                    }
                    
                }
                //move left
                focusAngle = angle + 90;
                p.X = current.p.IX + (int)(Math.Cos(Helpers<int>.degreesToRadians(focusAngle)) * 2);
                p.Y = current.p.IY + (int)(Math.Sin(Helpers<int>.degreesToRadians(focusAngle)) * 2);
                if (mg.isValidPoint(p.IX, p.IY) == true)
                {
                    //footing is valid
                    pad.X = current.p.IX + (int)(Math.Cos(Helpers<int>.degreesToRadians(focusAngle)) * 2);
                    pad.Y = current.p.IY + (int)(Math.Sin(Helpers<int>.degreesToRadians(focusAngle)) * 2);
                    if (mg.getCell(pad.IX, pad.IY) == true)
                    {
                        found = false;
                        for (int c = 0; c < closed.Count; c++)
                        {
                            if (closed[c].p.IX == p.IX && closed[c].p.IY == p.IY && closed[c].moves > current.moves + 1)
                            {
                                closed[c] = new PlatformNode(PLATFORMMOVES.landed, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            node = new PlatformNode(PLATFORMMOVES.landed, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                            open.enqueue(node, current.moves + 1);
                        }
                    }
                }
                //if can jump, add a jumpNode
                pad.X = current.p.IX + (int)(Math.Cos(Helpers<int>.degreesToRadians(angle)) * 2);
                pad.Y = current.p.IY + (int)(Math.Sin(Helpers<int>.degreesToRadians(angle)) * 2);
                if (mg.isValidPoint(pad.IX, pad.IY) == false)//foot on ground
                {
                    focusAngle = angle + 180; //this is up/jumping
                    p.X = (int)(Math.Cos(Helpers<int>.degreesToRadians(focusAngle)) * (playerHeight + 1)) + current.p.IX;
                    p.Y = (int)(Math.Sin(Helpers<int>.degreesToRadians(focusAngle)) * (playerHeight + 1)) + current.p.IY;
                    if (mg.getCell(p.IX, p.IY) == true)
                    {
                        found = false;
                        for (int c = 0; c < closed.Count; c++)
                        {
                            if (closed[c].p.IX == p.IX && closed[c].p.IY == p.IY && closed[c].moves > current.moves + 1)
                            {
                                closed[c] = new PlatformNode(PLATFORMMOVES.jump, 1, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            node = new PlatformNode(PLATFORMMOVES.jump, 0, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                            open.enqueue(node, current.moves + 1);
                        }
                    }
                }
            }
            else if (current.Platform == PLATFORMMOVES.jump)
            {
                if (current.JumpMoves >= jumpHeight)//can't jump so falling
                {
                    focusAngle = angle; //this is down/falling
                    p.X = (int)(Math.Cos(Helpers<int>.degreesToRadians(focusAngle)) * (playerHeight + 1)) + current.p.IX;
                    p.Y = (int)(Math.Sin(Helpers<int>.degreesToRadians(focusAngle)) * (playerHeight + 1)) + current.p.IY;
                    if (mg.getCell(p.IX, p.IY) == true)
                    {
                        found = false;
                        for (int c = 0; c < closed.Count; c++)
                        {
                            if (closed[c].p.IX == p.IX && closed[c].p.IY == p.IY && closed[c].moves > current.moves + 1)
                            {
                                closed[c] = new PlatformNode(PLATFORMMOVES.fall, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            node = new PlatformNode(PLATFORMMOVES.fall, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                            open.enqueue(node, current.moves + 1);
                        }
                    }
                }
                else
                {
                    //if can jump, add a jumpNode
                    roof.X = (int)Math.Cos(Helpers<int>.degreesToRadians(angle)) + current.p.IX;
                    roof.Y = (int)Math.Sin(Helpers<int>.degreesToRadians(angle)) + current.p.IY;
                    focusAngle = angle + 180; //this is up/jumping
                    p.X = (int)(Math.Cos(Helpers<int>.degreesToRadians(focusAngle)) * (playerHeight + 1)) + current.p.IX;
                    p.Y = (int)(Math.Sin(Helpers<int>.degreesToRadians(focusAngle)) * (playerHeight + 1)) + current.p.IY;
                    if (mg.isValidPoint(roof.IX, roof.IY) == true)
                    {
                        if (mg.isValidPoint(p.IX, p.IY) == true)
                        {
                            found = false;
                            for (int c = 0; c < closed.Count; c++)
                            {
                                if (closed[c].p.IX == p.IX && closed[c].p.IY == p.IY && closed[c].moves > current.moves + 1)
                                {
                                    closed[c] = new PlatformNode(PLATFORMMOVES.jump, current.JumpMoves + 1, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                                    found = true;
                                    break;
                                }
                            }
                            if (found == false)
                            {
                                node = new PlatformNode(PLATFORMMOVES.jump, current.JumpMoves + 1, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                                open.enqueue(node, current.moves + 1);
                            }
                        }
                    }
                }
            }
            else if(current.Platform == PLATFORMMOVES.fall)
            { 
                //fall right
                focusAngle = angle - 45;
                p.X = (int)Math.Cos(Helpers<int>.degreesToRadians(focusAngle)) + current.p.IX;
                p.Y = (int)Math.Sin(Helpers<int>.degreesToRadians(focusAngle)) + current.p.IY;
                if (mg.isValidPoint(p.IX, p.IY) == true)
                {
                    found = false;
                    for (int c = 0; c < closed.Count; c++)
                    {
                        if (closed[c].p.IX == p.IX && closed[c].p.IY == p.IY && closed[c].finalcost() > current.finalcost())
                        {
                            closed[c] = new PlatformNode(PLATFORMMOVES.fall, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                    {
                        node = new PlatformNode(PLATFORMMOVES.fall, 0, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                        open.enqueue(node, current.moves + 1);
                    }
                }
                //fall left
                focusAngle = angle + 45;
                p.X = (int)Math.Cos(Helpers<int>.degreesToRadians(focusAngle)) + current.p.IX;
                p.Y = (int)Math.Sin(Helpers<int>.degreesToRadians(focusAngle)) + current.p.IY;
                if (mg.isValidPoint(p.IX, p.IY) == true)
                {
                    found = false;
                    for (int c = 0; c < closed.Count; c++)
                    {
                        if (closed[c].p.IX == p.IX && closed[c].p.IY == p.IY && closed[c].moves > current.moves + 1)
                        {
                            closed[c] = new PlatformNode(PLATFORMMOVES.fall, 0, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                    {
                        node = new PlatformNode(PLATFORMMOVES.fall, 0, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                        open.enqueue(node, current.moves + 1);
                    }
                }
            }
        }
    }

    /// <summary>
    /// PlatformerNode
    /// </summary>
    public class PlatNode : PathNode
    {
        //protected
        protected bool jumping;
        protected bool falling;
        protected int jumpMoves;

        //public
        /// <summary>
        /// PlatNode constructor
        /// </summary>
        /// <param name="jumping">Jumping flag</param>
        /// <param name="falling">Jumping flag</param>
        /// <param name="jumpMoves">Number of moves while jumping</param>
        /// <param name="moves">Moves in total</param>
        /// <param name="distance">Distince to target</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="parent">Platformer reference</param>
        public PlatNode(bool jumping, bool falling, int jumpMoves, int moves, int distance, int x, int y, PlatNode parent = null) :
            base(moves, distance, x, y, parent)
        {
            this.jumping = jumping;
            this.falling = falling;
            this.jumpMoves = jumpMoves;
        }
        /// <summary>
        /// Overrides finalCost func
        /// </summary>
        /// <returns>Final cost of a move</returns>
        public override int finalcost()
        {
            return base.finalcost() + (jumpMoves / 2);
        }
        /// <summary>
        /// Jumping property
        /// </summary>
        public bool Jumping
        {
            get { return jumping; }
            set { jumping = value; }
        }
        /// <summary>
        /// Falling property
        /// </summary>
        public bool Falling
        {
            get { return falling; }
            set { falling = value; }
        }
        /// <summary>
        /// JumpMoves property
        /// </summary>
        public int JumpMoves
        {
            get { return jumpMoves; }
        }
    }

    /// <summary>
    /// Finds paths for a standard platformer environment
    /// </summary>
    public class SimplePlatformerPathFinder : SimplePathFinder
    {
        //protected
        protected int playerHeight;
        protected int jumpHeight;
        protected new PriorityQueue<PlatNode> open;
        protected new List<PlatNode> closed;

        //public
        /// <summary>
        /// SimplePlatformPathFinder constructor
        /// </summary>
        /// <param name="jumpHeight">Number of spaces object can jump</param>
        /// <param name="playerHeight">Object height in spaces</param>
        /// <param name="scale">Scaling value of tiles</param>
        /// <param name="maxSearches">Max searches for a path</param>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="minRange">Minmum range in spaces to target</param>
        public SimplePlatformerPathFinder(int jumpHeight, int playerHeight, int scale, int maxSearches, MapGraph mg, int minRange) : 
            base(scale, maxSearches, mg, minRange)
        {
            this.playerHeight = playerHeight;
            this.jumpHeight = jumpHeight;
        }
        /// <summary>
        /// Calculates a path from start postion to end position
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns>List of Point2D objects</returns>
        public List<Point2D> findPlatformerPath(Point2D startPoint, Point2D endPoint)
        {
            open = new PriorityQueue<PlatNode>();
            closed = new List<PlatNode>();
            Point2D start = new Point2D(startPoint.X / scale, startPoint.Y / scale);
            Point2D end = new Point2D(endPoint.X / scale, endPoint.Y / scale);
            open = new PriorityQueue<PlatNode>();
            closed = new List<PlatNode>();

            PlatNode current = new PlatNode(false, false, 0, 0, Point2D.calculateDistance(start, end), (int)start.X, (int)start.Y, null);
            int searches = 0;
            setPlatformerNodes(current, end);
            bool running = true;

            while (running)
            {
                //base cases
                if (current == null)
                {
                    running = false;
                }
                else
                {
                    if (current.p.IX == end.IX)
                    {
                        if (current.p.IY == end.IY)
                        {
                            running = false;
                        }
                    }
                    if (searches >= maxSearches)
                    {
                        running = false;
                    }
                    if (current.distance <= minRange)
                    {
                        running = false;
                    }

                    //search closed for potentially shorter paths
                    for (int i = 0; i < closed.Count; i++)
                    {
                        if (current.p.IX == closed[i].p.IX)
                        {
                            if (current.p.IY == closed[i].p.IY)
                            {
                                if (current.finalcost() < closed[i].finalcost())
                                {
                                    closed[i] = current;
                                    break;
                                }
                            }
                        }
                    }

                    //while the open priority queue is not empty get the first node and set to current
                    if (open.Count == 0)
                    {
                        running = false;
                    }
                    else
                    {
                        setPlatformerNodes(current, end);
                        PlatNode tmp = open.dequeue();
                        if(tmp == null)
                        {
                            running = false;
                        }
                        else
                        {
                            current = tmp;
                        }
                    }
                    searches++;
                }
            }

            return scalePath(current);
        }
        /// <summary>
        /// Sets nodes with down being the floor
        /// </summary>
        /// <param name="current">PlatformerNode reference</param>
        /// <param name="end">End point of path</param>
        public void setPlatformerNodes(PlatNode current, Point2D end)
        {
            
            bool found = false;
            Point2D p = new Point2D(0, 0);
            PlatNode node = null;
            Point2D foot = new Point2D(0, 0);
            Point2D roof = new Point2D(0, 0);
            if (current.Jumping == false)
            {
                //move right
                p.X = current.p.X + 1;
                p.Y = current.p.Y;
                if (mg.isValidPoint(p.IX, p.IY) == true)
                {
                    found = false;
                    for (int c = 0; c < closed.Count; c++)
                    {
                        if (closed[c].p.IX == p.IX && closed[c].p.IY == p.IY && closed[c].finalcost() > current.finalcost())
                        {
                            closed[c] = new PlatNode(false, false, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                    {
                        node = new PlatNode(false, false, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                        open.enqueue(node, current.moves + 1);
                    }
                }
                //move left
                p.X = current.p.X - 1;
                p.Y = current.p.Y;
                if (mg.isValidPoint(p.IX, p.IY) == true)
                {
                    found = false;
                    for (int c = 0; c < closed.Count; c++)
                    {
                        if (closed[c].p.IX == p.IX && closed[c].p.IY == p.IY && closed[c].moves > current.moves + 1)
                        {
                            closed[c] = new PlatNode(false, false, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                    {
                        node = new PlatNode(false, false, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                        open.enqueue(node, current.moves + 1);
                    }
                }
                //if can jump, add a jumpNode
                foot.X = current.p.X;
                foot.Y = current.p.Y + 1;
                if (mg.inDomain(foot.IX, foot.IY) == true)
                {
                    if (mg.getCell(foot.IX, foot.IY) == false)//foot on ground
                    {
                        //this is up/jumping
                        p.Y = current.p.IY - 1;
                        if (mg.isValidPoint(p.IX, p.IY) == true)
                        {
                            found = false;
                            for (int c = 0; c < closed.Count; c++)
                            {
                                if (closed[c].p.IX == p.IX && closed[c].p.IY == p.IY && closed[c].moves > current.moves + 1)
                                {
                                    closed[c] = new PlatNode(true, false, 0, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                                    found = true;
                                    break;
                                }
                            }
                            if (found == false)
                            {
                                node = new PlatNode(true, false, 0, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                                open.enqueue(node, current.moves + 1);
                            }
                        }
                    }
                }
            }
            else if(current.Jumping == true)//is currently jumping up
            {
                if (current.JumpMoves < jumpHeight * 2)
                {
                    //if can jump, add a jumpNode
                    roof.X = current.p.IX;
                    roof.Y = current.p.IY - playerHeight - 1;
                    if (mg.isValidPoint(roof.IX, roof.IY) == true)//head can enter
                    {
                        if (mg.isValidPoint(roof.IX, roof.IY) == true)
                        {
                            found = false;
                            for (int c = 0; c < closed.Count; c++)
                            {
                                if (closed[c].p.IX == roof.IX && closed[c].p.IY == roof.IY && closed[c].moves > current.moves + 1)
                                {
                                    closed[c] = new PlatNode(true, false, current.JumpMoves + 1, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                                    found = true;
                                    break;
                                }
                            }
                            if (found == false)
                            {
                                node = new PlatNode(true, false, current.JumpMoves + 1, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                                open.enqueue(node, current.moves + 1);
                            }
                        }
                    }
                    //move right
                    p.X = current.p.IX + 1;
                    p.Y = current.p.IY + 1;
                    if (mg.isValidPoint(p.IX, p.IY) == true)
                    {
                        found = false;
                        for (int c = 0; c < closed.Count; c++)
                        {
                            if (closed[c].p.IX == p.IX && closed[c].p.IY == p.IY && closed[c].finalcost() > current.finalcost())
                            {
                                closed[c] = new PlatNode(true, false, current.JumpMoves + 1, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            node = new PlatNode(true, false, current.JumpMoves + 1, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                            open.enqueue(node, current.moves + 1);
                        }
                    }
                    //move left
                    p.X = current.p.IX - 1;
                    p.Y = current.p.IY + 1;
                    if (mg.isValidPoint(p.IX, p.IY) == true)
                    {
                        found = false;
                        for (int c = 0; c < closed.Count; c++)
                        {
                            if (closed[c].p.IX == p.IX && closed[c].p.IY == p.IY && closed[c].moves > current.moves + 1)
                            {
                                closed[c] = new PlatNode(true, false, current.JumpMoves + 1, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            node = new PlatNode(true, false, current.JumpMoves + 1, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                            open.enqueue(node, current.moves + 1);
                        }
                    }
                }
            }
            else if (current.Falling == true)//can't jump so falling
            {
                p.X = current.p.IX;
                p.Y = current.p.IY + 1;
                found = false;
                for (int c = 0; c < closed.Count; c++)
                {
                    if (closed[c].p.IX == foot.IX && closed[c].p.IY == foot.IY && closed[c].moves > current.moves + 1)
                    {
                        closed[c] = new PlatNode(true, true, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    node = new PlatNode(true, true, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                    open.enqueue(node, current.moves + 1);
                }
                //move right
                p.X = current.p.X + 1;
                p.Y = current.p.Y;
                if (mg.isValidPoint(p.IX, p.IY) == true)
                {
                    found = false;
                    for (int c = 0; c < closed.Count; c++)
                    {
                        if (closed[c].p.IX == p.IX && closed[c].p.IY == p.IY && closed[c].finalcost() > current.finalcost())
                        {
                            closed[c] = new PlatNode(false, false, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                    {
                        node = new PlatNode(false, false, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                        open.enqueue(node, current.moves + 1);
                    }
                }
                //move left
                p.X = current.p.X - 1;
                p.Y = current.p.Y;
                if (mg.isValidPoint(p.IX, p.IY) == true)
                {
                    found = false;
                    for (int c = 0; c < closed.Count; c++)
                    {
                        if (closed[c].p.IX == p.IX && closed[c].p.IY == p.IY && closed[c].moves > current.moves + 1)
                        {
                            closed[c] = new PlatNode(false, false, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                    {
                        node = new PlatNode(false, false, current.JumpMoves, current.moves + 1, (int)Point2D.AsqrtB(end, p), p.IX, p.IY, current);
                        open.enqueue(node, current.moves + 1);
                    }
                }
            }
        }
        /// <summary>
        /// Scales path output to fit game world
        /// </summary>
        /// <param name="current">Current node to work from</param>
        /// <returns>List of Point2D objects</returns>
        public new List<Point2D> scalePath(PathNode current)
        {
            List<Point2D> temp = new List<Point2D>();
            while (current != null)
            {
                temp.Add(new Point2D(current.p.X * scale, current.p.Y * scale));
                current = current.parent;
            }
            List<Point2D> newPath = new List<Point2D>();
            for (int i = 0; i < temp.Count - 1; i++)
            {
                newPath.Add(temp[i]);
            }
            return newPath;

        }
    }
}
