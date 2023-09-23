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
    public class HopPathFinder : SimplePathFinder
    {
        //protected
        protected int jumpRange;
        protected double gravityAngle;
        protected int cap;

        //public
        /// <summary>
        /// HopPathFinder constructor
        /// </summary>
        /// <param name="scale">Scaling value ie space width/height</param>
        /// <param name="maxSearches">Max number of searches</param>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="minRange">Cut off range for searching to target</param>
        /// <param name="jumpRange">Radius/jump range of path user</param>
        /// <param name="gravityAngle">Angle of gravity to object (90 == down)</param>
        /// <param name="cap">Min height above user that needs to be clear to move into a point</param>
        public HopPathFinder(int scale, int maxSearches, MapGraph mg, int minRange, int jumpRange, double gravityAngle, int cap) : 
            base(scale, maxSearches, mg, minRange)
        {
            this.jumpRange = jumpRange;
            this.gravityAngle = gravityAngle;
            this.cap = cap;
        }
        /// <summary>
        /// Finds a hopping path through a space 
        /// </summary>
        /// <param name="startPoint">Starting point of search</param>
        /// <param name="endPoint">Ending point of search</param>
        /// <returns>List of scaled points as a path</returns>
        public List<Point2D> findHopPath(Point2D startPoint, Point2D endPoint)
        {
            open.clear();
            closed.Clear();
            Point2D start = new Point2D(startPoint);
            Point2D end = new Point2D(endPoint);
            PathNode current = new PathNode(0, (int)Point2D.AsqrtB(start, end), start.IX, start.IY, null);
            int searches = 0;
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
        /// Sets nodes to any with a closed space below
        /// </summary>
        /// <param name="current">Current path node</param>
        /// <param name="end">End point of path</param>
        public override void setNodes(PathNode current, Point2D end)
        {
            PathNode temp;
            List<Point2D> area = mg.getRadius(current.p.IX, current.p.IY, jumpRange);
            for(int p = 0; p < area.Count; p++)
            {
                int spx = (int)Helpers<int>.getXValue(area[p].IX, gravityAngle, scale);
                int spy = (int)Helpers<int>.getYValue(area[p].IY, gravityAngle, scale);
                int scx = (int)Helpers<int>.getXValue(area[p].IX, gravityAngle + 180, cap * scale);
                int scy = (int)Helpers<int>.getYValue(area[p].IY, gravityAngle + 180, cap * scale);
                //Above has nothing blocking, below has an occupied space
                if (mg.isValidPoint(spx, spy) == true && mg.isValidPoint(scx, scy) == false)
                {
                    temp = new PathNode(current.moves + 1, Point2D.calculateDistance(new Point2D(area[p].X, area[p].Y), end), (int)area[p].X, (int)area[p].Y, current);
                    open.enqueue(temp, temp.finalcost());
                }
            }

        }
        /// <summary>
        /// JumpRange property
        /// </summary>
        public int JumpRange
        {
            get { return jumpRange; }
            set { jumpRange = value; }
        }
        /// <summary>
        /// GravityAngle property
        /// </summary>
        public double GravityAngle
        {
            get { return gravityAngle; }
            set { gravityAngle = value; }
        }
        /// <summary>
        /// Cap property
        /// </summary>
        public int Cap
        {
            get { return cap; }
            set { cap = value; }
        }
    }
}
