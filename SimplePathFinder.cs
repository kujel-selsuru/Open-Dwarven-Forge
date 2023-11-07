//====================================================
//Written by Kujel Selsuru
//Last Updated 06/11/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// PathNode class
    /// </summary>
    public class PathNode
    {
        //public
        public int moves;
        public int distance;
        public Point2D p;
        public PathNode parent;

        /// <summary>
        /// PathNode constructor
        /// </summary>
        /// <param name="moves">Moves value</param>
        /// <param name="distance">Distence value</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="parent">Parent Node</param>
        public PathNode(int moves, int distance, int x, int y, PathNode parent = null)
        {
            this.moves = moves;
            this.distance = distance;
            p = new Point2D(x, y);
            this.parent = parent;
        }
        /// <summary>
        /// Calculates final value of node
        /// </summary>
        /// <returns></returns>
        public virtual int finalcost()
        {
            return moves + distance;
        }
    }
    /// <summary>
    /// SimplePathFinder class
    /// </summary>
    public class SimplePathFinder
    {
        //protected
        protected int scale;
        protected MapGraph mg;
        protected int maxSearches;
        protected PriorityQueue<PathNode> open;
        protected List<PathNode> closed;
        protected int minRange;
        /// <summary>
        /// builds a pathfinder object, requires a MapGraph representation of enviroment to function properly
        /// </summary>
        /// <param name="scale">length of box sides</param>
        /// <param name="maxSearches">how long to search before giving up</param>
        /// <param name="mg">enviroments MapGraph</param>
        /// <param name="minRange">how close in terms of spaces to stop looking for a path</param>
        public SimplePathFinder(int scale, int maxSearches, MapGraph mg, int minRange)
        {
            this.minRange = minRange;
            this.mg = mg;
            this.maxSearches = maxSearches;
            this.scale = scale;
        }
        /// <summary>
        /// Returns a List of Point2D objects 
        /// </summary>
        /// <param name="startPoint">Start point of search</param>
        /// <param name="endPoint">End point of search</param>
        /// <returns>List of Point2D objects</returns>
        public virtual List<Point2D> findPath(Point2D startPoint, Point2D endPoint)
        {
            Point2D start = new Point2D(startPoint.X/scale, startPoint.Y/scale);
            Point2D end = new Point2D(endPoint.X/scale, endPoint.Y/scale);
            open = new PriorityQueue<PathNode>();
            closed = new List<PathNode>();

            PathNode current = new PathNode(0, Point2D.calculateDistance(start, end), (int)start.X, (int)start.Y);
            int searches = 0;
            setNodes(current, end);
            bool running = true;
            bool found = false;

            while(running)
            {
                //base cases
                if(current == null)
                {
                    return new List<Point2D>();
                }
                if(current.p.X == end.X)
                {
                    if(current.p.Y == end.Y)
                    {
                        running = false;
                    }
                }
                if(searches >= maxSearches)
                {
                    running = false;
                }
                if(current.distance <= minRange)
                {
                    running = false;
                }
                /*
                //search closed for potentially shorter paths
                found = false;
                for (int i = 0; i < closed.Count; i++)
                {
                    if (current.p.X == closed[i].p.X)
                    {
                        if(current.p.Y == closed[i].p.Y)
                        {
                            if (current.moves < closed[i].moves)
                            {
                                closed[i] = current;
                                break;
                            }
                            else
                            {
                                found = true;
                            }
                        }
                    }
                }
                */
                if(found == true)
                {
                    if(open.Count > 0)
                    {
                        current = open.dequeue();
                    }
                }

                //while the open priority queue is not empty get the first node and set to current
                if (open.Count == 0)
                {
                    running = false;
                }
                else
                {
                    setNodes(current, end);
                    current = open.dequeue();
                }
                searches++;
            }

            return scalePath(current);
        }
        /// <summary>
        /// Returns a List of Point2D objects (version 2)
        /// </summary>
        /// <param name="startPoint">Start point of search</param>
        /// <param name="endPoint">End point of search</param>
        /// <returns>List of Point2D objects</returns>
        public virtual List<Point2D> findPathV2(Point2D startPoint, Point2D endPoint)
        {
            Point2D end = new Point2D(startPoint.X / scale, startPoint.Y / scale);
            Point2D start = new Point2D(endPoint.X / scale, endPoint.Y / scale);
            open = new PriorityQueue<PathNode>();
            closed = new List<PathNode>();

            PathNode current = new PathNode(0, Point2D.calculateDistance(start, end), (int)start.X, (int)start.Y);
            int searches = 0;
            setNodes(current, end);
            bool running = true;

            while (running)
            {
                //base cases
                if (current.p.X == end.X)
                {
                    if (current.p.Y == end.Y)
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
                /*
                //search closed for potentially shorter paths
                for (int i = 0; i < closed.Count; i++)
                {
                    if (current.p.X == closed[i].p.X)
                    {
                        if (current.p.Y == closed[i].p.Y)
                        {
                            if (current.finalcost() < closed[i].finalcost())
                            {
                                closed[i] = current;
                                break;
                            }
                        }
                    }
                }
                */
                //while the open priority queue is not empty get the first node and set to current
                if (open.Count == 0)
                {
                    running = false;
                }
                else
                {
                    setNodes(current, end);
                    current = open.dequeue();
                }
                searches++;
            }

            return inversePath(scalePath(current));
        }
        /// <summary>
        /// Returns a List of Point2D objects (version 3)
        /// </summary>
        /// <param name="startPoint">Start point of search</param>
        /// <param name="endPoint">End point of search</param>
        /// <returns>List of Point2D objects</returns>
        public virtual List<Point2D> findPathV3(Point2D startPoint, Point2D endPoint)
        {
            Point2D end = new Point2D(startPoint.X / scale, startPoint.Y / scale);
            Point2D start = new Point2D(endPoint.X / scale, endPoint.Y / scale);
            open = new PriorityQueue<PathNode>();
            closed = new List<PathNode>();

            PathNode current = new PathNode(0, Point2D.calculateDistance(start, end), (int)start.X, (int)start.Y);
            int searches = 0;
            setNodes(current, end);
            bool running = true;

            while (running)
            {
                //base cases
                if (current.p.X == end.X)
                {
                    if (current.p.Y == end.Y)
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
                /*
                //search closed for potentially shorter paths
                for (int i = 0; i < closed.Count; i++)
                {
                    if (current.p.X == closed[i].p.X)
                    {
                        if (current.p.Y == closed[i].p.Y)
                        {
                            if (current.finalcost() < closed[i].finalcost())
                            {
                                closed[i] = current;
                                break;
                            }
                        }
                    }
                }
                */
                //while the open priority queue is not empty get the first node and set to current
                if (open.Count == 0)
                {
                    running = false;
                }
                else
                {
                    setNodes(current, end);
                    current = open.dequeue();
                }
                searches++;
            }

            return scalePath2(current);
        }
        /// <summary>
        /// Returns a List of Point2D objects (version 4)
        /// </summary>
        /// <param name="startPoint">Start point of search</param>
        /// <param name="endPoint">End point of search</param>
        /// <param name="offsetx">Offsetx value</param>
        /// <param name="offsety">Offsety value</param>
        /// <returns>List of Point2D objects</returns>
        public virtual List<Point2D> findPathV4(Point2D startPoint, Point2D endPoint, int offsetx, int offsety)
        {
            Point2D end = new Point2D((startPoint.X - offsetx) / scale, (startPoint.Y - offsety) / scale);
            Point2D start = new Point2D((endPoint.X - offsetx) / scale, (endPoint.Y - offsety) / scale);
            open = new PriorityQueue<PathNode>();
            closed = new List<PathNode>();

            PathNode current = new PathNode(0, Point2D.calculateDistance(start, end), (int)start.X, (int)start.Y);
            int searches = 0;
            setNodes(current, end);
            bool running = true;

            while (running)
            {
                //base cases
                if (current.p.X == end.X)
                {
                    if (current.p.Y == end.Y)
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
                /*
                //search closed for potentially shorter paths
                for (int i = 0; i < closed.Count; i++)
                {
                    if (current.p.X == closed[i].p.X)
                    {
                        if (current.p.Y == closed[i].p.Y)
                        {
                            if (current.finalcost() < closed[i].finalcost())
                            {
                                closed[i] = current;
                                break;
                            }
                        }
                    }
                }
                */
                //while the open priority queue is not empty get the first node and set to current
                if (open.Count == 0)
                {
                    running = false;
                }
                else
                {
                    setNodes(current, end);
                    current = open.dequeue();
                }
                searches++;
            }

            return scalePath2(current);
        }
        /// <summary>
        /// Returns a List of Point2D objects with single obsticle avoidence 
        /// </summary>
        /// <param name="startPoint">Start point of search</param>
        /// <param name="endPoint">End point of search</param>
        /// /// <param name="obsticle">obsticle to avoid</param>
        /// <returns>List of Point2D objects</returns>
        public virtual List<Point2D> findPath(Point2D startPoint, Point2D endPoint, Point2D obsticle)
        {
            Point2D end = new Point2D(startPoint.X / scale, startPoint.Y / scale);
            Point2D start = new Point2D(endPoint.X / scale, endPoint.Y / scale);
            open = new PriorityQueue<PathNode>();
            closed = new List<PathNode>();
            obsticle.X = obsticle.X / scale;
            obsticle.Y = obsticle.Y / scale;

            PathNode current = new PathNode(0, Point2D.calculateDistance(start, end), (int)start.X, (int)start.Y);
            int searches = 0;
            setNodes(current, end, obsticle);
            bool running = true;

            while (running)
            {
                //base cases
                if (current.p.X == end.X)
                {
                    if (current.p.Y == end.Y)
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
                /*
                //search closed for potentially shorter paths
                for (int i = 0; i < closed.Count; i++)
                {
                    if (current.p.X == closed[i].p.X)
                    {
                        if (current.p.Y == closed[i].p.Y)
                        {
                            if (current.finalcost() < closed[i].finalcost())
                            {
                                closed[i] = current;
                                break;
                            }
                        }
                    }
                }
                */

                //while the open priority queue is not empty get the first node and set to current
                if (open.Count == 0)
                {
                    running = false;
                }
                else
                {
                    setNodes(current, end, obsticle);
                    current = open.dequeue();
                }
                searches++;
            }

            return scalePath(current);
        }
        /// <summary>
        /// Returns a List of Point2D objects 
        /// </summary>
        /// <param name="startPoint">Start point of search</param>
        /// <param name="endPoint">End point of search</param>
        /// <returns>List of Point2D objects</returns>
        public virtual List<Point2D> findPathV5(Point2D startPoint, Point2D endPoint)
        {
            Point2D start = new Point2D(startPoint.X / scale, startPoint.Y / scale);
            Point2D end = new Point2D(endPoint.X / scale, endPoint.Y / scale);
            open = new PriorityQueue<PathNode>();
            closed = new List<PathNode>();

            PathNode current = new PathNode(0, Point2D.calculateDistance(start, end), (int)start.X, (int)start.Y);
            int searches = 0;
            setNodes2(current, end);
            bool running = true;

            while (running)
            {
                //base cases
                if (current == null)
                {
                    return new List<Point2D>();
                }
                if (current.p.X == end.X)
                {
                    if (current.p.Y == end.Y)
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
                /*
                //search closed for potentially shorter paths
                for (int i = 0; i < closed.Count; i++)
                {
                    if (current.p.X == closed[i].p.X)
                    {
                        if (current.p.Y == closed[i].p.Y)
                        {
                            if (current.finalcost() < closed[i].finalcost())
                            {
                                closed[i] = current;
                                break;
                            }
                        }
                    }
                }
                */
                //while the open priority queue is not empty get the first node and set to current
                if (open.Count == 0)
                {
                    running = false;
                }
                else
                {
                    setNodes2(current, end);
                    current = open.dequeue();
                }
                searches++;
            }

            return scalePath(current);
        }
        /// <summary>
        /// Checks if a path between two points exists
        /// </summary>
        /// <param name="startPoint">Starting position</param>
        /// <param name="endPoint">Ending position</param>
        /// <returns>Boolean</returns>
        public bool pathExists(Point2D startPoint, Point2D endPoint)
        {
            Point2D start = new Point2D(startPoint.X / scale, startPoint.Y / scale);
            Point2D end = new Point2D(endPoint.X / scale, endPoint.Y / scale);
            open = new PriorityQueue<PathNode>();
            closed = new List<PathNode>();

            PathNode current = new PathNode(0, Point2D.calculateDistance(start, end), (int)start.X, (int)start.Y);
            int searches = 0;
            setNodes(current, end);
            bool running = true;

            while (running)
            {
                //base cases
                if (current.p.X == end.X)
                {
                    if (current.p.Y == end.Y)
                    {
                        return true;
                    }
                }
                if (searches >= maxSearches)
                {
                    running = false;
                }
                if (current.distance <= minRange)
                {
                    return true;
                }
                /*
                //search closed for potentially shorter paths
                for (int i = 0; i < closed.Count; i++)
                {
                    if (current.p.X == closed[i].p.X)
                    {
                        if (current.p.Y == closed[i].p.Y)
                        {
                            if (current.finalcost() < closed[i].finalcost())
                            {
                                closed[i] = current;
                                break;
                            }
                        }
                    }
                }
                */
                //while the open priority queue is not empty get the first node and set to current
                if (open.Count == 0)
                {
                    running = false;   
                }
                else
                {
                    setNodes(current, end);
                    current = open.dequeue();
                }
                searches++;
            }

            return false; ;
        }
        /// <summary>
        /// Adds points to the open list
        /// </summary>
        /// <param name="current">Current node to search from</param>
        /// <param name="end">End point to search toward</param>
        public virtual void setNodes(PathNode current, Point2D end)
        {
            PathNode temp;
            //up
            if (mg.isValidPoint((int)current.p.X, (int)current.p.Y - 1))
            {
                temp = addNode(current.p.IX, current.p.IY - 1, current.moves, current, end);//new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X, current.p.Y - 1), end), (int)current.p.X, (int)current.p.Y - 1, current);
                open.enqueue(temp, temp.finalcost());
            }
            //up right
            if (mg.isValidPoint((int)current.p.X + 1, (int)current.p.Y - 1))
            {
                temp = addNode(current.p.IX + 1, current.p.IY - 1, current.moves, current, end);//new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X + 1, current.p.Y - 1), end), (int)current.p.X + 1, (int)current.p.Y - 1, current);
                open.enqueue(temp, temp.finalcost());
            }
            //right
            if (mg.isValidPoint((int)current.p.X + 1, (int)current.p.Y))
            {
                temp = addNode(current.p.IX + 1, current.p.IY, current.moves, current, end);//new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X + 1, current.p.Y), end), (int)current.p.X + 1, (int)current.p.Y, current);
                open.enqueue(temp, temp.finalcost());
            }
            //right down
            if (mg.isValidPoint((int)current.p.X + 1, (int)current.p.Y + 1))
            {
                temp = addNode(current.p.IX + 1, current.p.IY + 1, current.moves, current, end);//new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X + 1, current.p.Y + 1), end), (int)current.p.X + 1, (int)current.p.Y + 1, current);
                open.enqueue(temp, temp.finalcost());
            }
            //down
            if (mg.isValidPoint((int)current.p.X, (int)current.p.Y + 1))
            {
                temp = addNode(current.p.IX, current.p.IY + 1, current.moves, current, end);//new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X, current.p.Y + 1), end), (int)current.p.X, (int)current.p.Y + 1, current);
                open.enqueue(temp, temp.finalcost());
            }
            //down left
            if (mg.isValidPoint((int)current.p.X - 1, (int)current.p.Y + 1))
            {
                temp = addNode(current.p.IX - 1, current.p.IY + 1, current.moves, current, end);//new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X - 1, current.p.Y + 1), end), (int)current.p.X - 1, (int)current.p.Y + 1, current);
                open.enqueue(temp, temp.finalcost());
            }
            //left 
            if (mg.isValidPoint((int)current.p.X - 1, (int)current.p.Y))
            {
                temp = addNode(current.p.IX - 1, current.p.IY, current.moves, current, end);//new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X - 1, current.p.Y), end), (int)current.p.X - 1, (int)current.p.Y, current);
                open.enqueue(temp, temp.finalcost());
            }
            //left up
            if (mg.isValidPoint((int)current.p.X - 1, (int)current.p.Y - 1))
            {
                temp = addNode(current.p.IX - 1, current.p.IY - 1, current.moves, current, end);//new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X - 1, current.p.Y - 1), end), (int)current.p.X - 1, (int)current.p.Y - 1, current);
                open.enqueue(temp, temp.finalcost());
            }
        }
        /// <summary>
        /// Adds points to the open list
        /// </summary>
        /// <param name="current">Current node to search from</param>
        /// <param name="end">End point to search toward</param>
        public virtual void setNodes2(PathNode current, Point2D end)
        {
            PathNode temp;
            //up
            if (scanClosed(current.p.IX, current.p.IY - 1) == false)
            {
                if (mg.isValidPoint(current.p.IX, current.p.IY - 1))
                {
                    temp = addNode(current.p.IX, current.p.IY - 1, current.moves, current, end);//new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X, current.p.Y - 1), end), (int)current.p.X, (int)current.p.Y - 1, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }
            //up right
            if (scanClosed(current.p.IX + 1, current.p.IY - 1) == false)
            {
                if (mg.isValidPoint(current.p.IX + 1, current.p.IY - 1))
                {
                    temp = addNode(current.p.IX + 1, current.p.IY - 1, current.moves, current, end);//new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X + 1, current.p.Y - 1), end), (int)current.p.X + 1, (int)current.p.Y - 1, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }
            //right
            if (scanClosed(current.p.IX + 1, current.p.IY) == false)
            {
                if (mg.isValidPoint(current.p.IX + 1, current.p.IY))
                {
                    temp = addNode(current.p.IX + 1, current.p.IY, current.moves, current, end);//new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X + 1, current.p.Y), end), (int)current.p.X + 1, (int)current.p.Y, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }
            //right down
            if (scanClosed(current.p.IX + 1, current.p.IY + 1) == false)
            {
                if (mg.isValidPoint(current.p.IX + 1, current.p.IY + 1))
                {
                    temp = addNode(current.p.IX + 1, current.p.IY + 1, current.moves, current, end);//new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X + 1, current.p.Y + 1), end), (int)current.p.X + 1, (int)current.p.Y + 1, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }
            //down
            if (scanClosed(current.p.IX, current.p.IY + 1) == false)
            {
                if (mg.isValidPoint(current.p.IX, current.p.IY + 1))
                {
                    temp = addNode(current.p.IX, current.p.IY + 1, current.moves, current, end);//new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X, current.p.Y + 1), end), (int)current.p.X, (int)current.p.Y + 1, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }
            //down left
            if (scanClosed(current.p.IX - 1, current.p.IY + 1) == false)
            {
                if (mg.isValidPoint(current.p.IX - 1, current.p.IY + 1))
                {
                    temp = addNode(current.p.IX - 1, current.p.IY + 1, current.moves, current, end);//new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X - 1, current.p.Y + 1), end), (int)current.p.X - 1, (int)current.p.Y + 1, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }
            //left 
            if (scanClosed(current.p.IX - 1, current.p.IY) == false)
            {
                if (mg.isValidPoint(current.p.IX - 1, current.p.IY))
                {
                    temp = addNode(current.p.IX - 1, current.p.IY, current.moves, current, end);//new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X - 1, current.p.Y), end), (int)current.p.X - 1, (int)current.p.Y, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }
            //left up
            if (scanClosed(current.p.IX - 1, current.p.IY - 1) == false)
            {
                if (mg.isValidPoint(current.p.IX - 1, current.p.IY - 1))
                {
                    temp = addNode(current.p.IX - 1, current.p.IY - 1, current.moves, current, end);//new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X - 1, current.p.Y - 1), end), (int)current.p.X - 1, (int)current.p.Y - 1, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }
        }
        /// <summary>
        /// Scans the closed list for a given position, return true if fould else false
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <returns>Boolean</returns>
        public bool scanClosed(int x, int y)
        {
            for(int i = 0; i < closed.Count; i++)
            {
                if(closed[i].p.IX == x)
                {
                    if (closed[i].p.IY == y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Adds points to the open list with single obsticle avoidence
        /// </summary>
        /// <param name="current">Current node to search from</param>
        /// <param name="end">End point to search toward</param>
        /// <param name="obsticle">Obsticle to avoid</param>
        public virtual void setNodes(PathNode current, Point2D end, Point2D obsticle)
        {
            PathNode temp;
            //up
            if (mg.isValidPoint((int)current.p.X, (int)current.p.Y - 1))
            {
                if (current.p.X != obsticle.X || current.p.Y != obsticle.Y)
                {
                    temp = new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X, current.p.Y - 1), end), (int)current.p.X, (int)current.p.Y - 1, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }
            //up right
            if (mg.isValidPoint((int)current.p.X + 1, (int)current.p.Y - 1))
            {
                if (current.p.X != obsticle.X || current.p.Y != obsticle.Y)
                {
                    temp = new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X + 1, current.p.Y - 1), end), (int)current.p.X + 1, (int)current.p.Y - 1, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }
            //right
            if (mg.isValidPoint((int)current.p.X + 1, (int)current.p.Y))
            {
                if (current.p.X != obsticle.X || current.p.Y != obsticle.Y)
                {
                    temp = new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X + 1, current.p.Y), end), (int)current.p.X + 1, (int)current.p.Y, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }
            //right down
            if (mg.isValidPoint((int)current.p.X + 1, (int)current.p.Y + 1))
            {
                if (current.p.X != obsticle.X || current.p.Y != obsticle.Y)
                {
                    temp = new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X + 1, current.p.Y + 1), end), (int)current.p.X + 1, (int)current.p.Y + 1, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }
            //down
            if (mg.isValidPoint((int)current.p.X, (int)current.p.Y + 1))
            {
                if (current.p.X != obsticle.X || current.p.Y != obsticle.Y)
                {
                    temp = new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X, current.p.Y + 1), end), (int)current.p.X, (int)current.p.Y + 1, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }
            //down left
            if (mg.isValidPoint((int)current.p.X - 1, (int)current.p.Y + 1))
            {
                if (current.p.X != obsticle.X || current.p.Y != obsticle.Y)
                {
                    temp = new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X - 1, current.p.Y + 1), end), (int)current.p.X - 1, (int)current.p.Y + 1, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }
            //left 
            if (mg.isValidPoint((int)current.p.X - 1, (int)current.p.Y))
            {
                if (current.p.X != obsticle.X || current.p.Y != obsticle.Y)
                {
                    temp = new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X - 1, current.p.Y), end), (int)current.p.X - 1, (int)current.p.Y, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }
            //left up
            if (mg.isValidPoint((int)current.p.X - 1, (int)current.p.Y - 1))
            {
                if (current.p.X != obsticle.X || current.p.Y != obsticle.Y)
                {
                    temp = new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(current.p.X - 1, current.p.Y - 1), end), (int)current.p.X - 1, (int)current.p.Y - 1, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }
        }
        /// <summary>
        /// Scales path output to fit game world
        /// </summary>
        /// <param name="current">Current node to work from</param>
        /// <returns>List of Point2D objects</returns>
        public List<Point2D> scalePath(PathNode current)
        {
            List<Point2D> temp = new List<Point2D>();
            while(current != null)
            {
                temp.Add(new Point2D(current.p.X * scale, current.p.Y * scale));
                current = current.parent;
            }
            List<Point2D> newPath = new List<Point2D>();
            for (int i = temp.Count - 1; i > -1; i--)
            {
                newPath.Add(temp[i]);
            }
            return newPath;
            
        }
        /// <summary>
        /// Scales path output to fit game world (version 2)
        /// </summary>
        /// <param name="current">Current node to work from</param>
        /// <returns>List of Point2D objects</returns>
        public List<Point2D> scalePath2(PathNode current)
        {
            List<Point2D> temp = new List<Point2D>();
            while (current != null)
            {
                temp.Add(new Point2D(current.p.X * scale, current.p.Y * scale));
                mg.setCell((int)current.p.X, (int)current.p.Y, false);
                current = current.parent;
            }
            return temp;
        }
        /// <summary>
        /// Scales path output to fit game world (version 3)
        /// </summary>
        /// <param name="current">Current node to work from</param>
        /// <param name="offsetx">Offsetx value</param>
        /// <param name="offsety">Offsety value</param>
        /// <returns>List of Point2D objects</returns>
        public List<Point2D> scalePath3(PathNode current, int offsetx = 0, int offsety = 0)
        {
            List<Point2D> temp = new List<Point2D>();
            while (current != null)
            {
                mg.setCell((int)current.p.X, (int)current.p.Y, false);
                temp.Add(new Point2D((current.p.X * scale) + offsetx, ((current.p.Y * scale) + offsety)));
                current = current.parent;
            }
            return temp;
        }
        /// <summary>
        /// Returns a reverse order version of provided path
        /// </summary>
        /// <param name="path">A List of Point2D objects representing a path </param>
        /// <returns>A list of Point2D objects</returns>
        public List<Point2D> inversePath(List<Point2D> path)
        {
            List<Point2D> newPath = new List<Point2D>();
            for(int i = path.Count - 1; i > -1; i--)
            {
                newPath.Add(path[i]);
            }
            return newPath;
        }
        /// <summary>
        /// Adds a node if no duplicates found in closed and replaces the original in closed if
        /// it has a higher move count
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="moves">Number of moves</param>
        /// <param name="current">Current PathNode reference</param>
        /// <param name="end">End point</param>
        /// <returns>PathNode</returns>
        public PathNode addNode(int x, int y, int moves, PathNode current, Point2D end)
        {
            PathNode node = null;
            bool found = false;
            for(int i = 0; i < closed.Count - 1; i++)
            {
                if(closed[i].p.IX == x)
                {
                    if(closed[i].p.IY == y)
                    {
                        if(closed[i].moves > moves)
                        {
                            node = new PathNode(moves + 1, Point2D.calculateDistance(new Point2D(current.p.X - 1, current.p.Y - 1), end), x, y, current);
                            closed[i] = node;
                        }
                        found = true;
                    }
                }
            }
            if (found == false && node == null)
            {
                node = new PathNode(moves + 1, Point2D.calculateDistance(new Point2D(current.p.X - 1, current.p.Y - 1), end), x, y, current);
            }
            return node;
        }
        /// <summary>
        /// MapGraph property
        /// </summary>
        public MapGraph MG
        {
            get { return mg; }
            set { mg = value; }
        }
    }
}
