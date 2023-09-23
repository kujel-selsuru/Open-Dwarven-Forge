//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XenoLib
{
    /// <summary>
    /// creates and handles varible sized and shaped sector graphs
    /// all sectors are some varient of rectangle, dimensions vary by data input
    /// numSectors is square rooted to get the graph's dimensions so assing this value carefully
    /// </summary>
    public class VaribleSectorGraph
    {
        //protected
        protected int[,] graph;
        protected int scale;
        protected int width;
        protected int height;
        //public
        /// <summary>
        /// VariableSectorGraph constructor
        /// </summary>
        /// <param name="numSectors">Number of sectors</param>
        /// <param name="regionWidth">Region width in pixels</param>
        /// <param name="regionHeight">Region height in pixels</param>
        public VaribleSectorGraph(int numSectors, int regionWidth, int regionHeight)
        {
            scale = (int)Math.Sqrt(numSectors);
            width = regionWidth / scale;
            height = regionHeight / scale;
            graph = new int[scale, scale];
            int counter = 0;
            for (int y = 0; y < scale; y++)
            {
                for (int x = 0; x < scale; x++)
                {
                    graph[x, y] = counter;
                    counter++;
                }
            }
        }
        /// <summary>
        /// Find just one sector when given a postion
        /// </summary>
        /// <param name="x">X postion in pixels</param>
        /// <param name="y">Y postion in pixels</param>
        /// <returns>Sector number as an int</returns>
        public int findSector(int x, int y)
        {
            if (inDomain(x / width, y / height))
            {
                return graph[x / width, y / height];
            }
            else
            {
                return -9;
            }
        }
        /// <summary>
        /// Collects and returns a list of sectors in a given box
        /// </summary>
        /// <param name="x">X position in pixels</param>
        /// <param name="y">Y position in pixels</param>
        /// <param name="radius">radius in tiles</param>
        /// <returns>List of sector numbers as ints</returns>
        public List<int> findSubSectors(int x, int y, int radius)
        {
            int tempx = (x / width) - radius;
            int tempy = (y / height) - radius;
            List<int> subSectors = new List<int>();
            for (int sx = tempx; sx < tempx + (2 * radius) + 1; sx++)
            {
                for (int sy = tempy; sy < tempy + (2 * radius) + 1; sy++)
                {
                    if(inDomain(sx, sy))
                    {
                        subSectors.Add(graph[sx, sy]);
                    }
                }
            }
            return subSectors;
        }
        /// <summary>
        /// Returns a list of sectors 
        /// </summary>
        /// <param name="x">X position in pixels</param>
        /// <param name="y">Y position in pixels</param>
        /// <param name="radius">Radius in tiles</param>
        /// <returns>List of sector numbers as ints</returns>
        public List<int> collectArea(int x, int y, int radius)
        {
            Point2D temp = new Point2D(x / width, y / height);
            if (radius < 3)
            {
                radius = 3;
            }
            List<int> area = new List<int>();
            int loops = radius * 2 + 1;
            int sweep = 3;
            Point2D current = new Point2D(temp.X - 1, temp.Y - radius);

            for (int i = 0; i < loops; i++)
            {
                for (int k = 0; k < sweep; k++)
                {
                    current.X += 1;
                    if (inDomain((int)current.X, (int)current.Y))
                    {
                        area.Add(graph[(int)current.X, (int)current.Y]);
                    }
                }
                current.Y += 1;
                if (i >= radius - 2 && i <= radius)
                {
                    sweep = (radius * 2) + 1;
                    current.X = temp.X - radius;
                }
                else if (i < radius - 2)
                {
                    sweep += 2;
                    current.X = temp.X - (sweep / 2);
                }
                else if (i > radius)
                {
                    sweep -= 2;
                    current.X = temp.X - (sweep / 2);
                }
            }
            return area;
        }
        /// <summary>
        /// Tests if a grid point is in the domain
        /// </summary>
        /// <param name="x">Grid x</param>
        /// <param name="y">Grid y</param>
        /// <returns>Boolean</returns>
        public bool inDomain(int x, int y)
        {
            if (x >= 0 & x < scale)
            {
                if (y >= 0 & y < scale)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
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
        /// Scale property
        /// </summary>
        public int Scale
        {
            get { return scale; }
        }
    }
}
