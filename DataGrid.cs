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
    /// DataGrid container class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataGrid<T>
    {
        //protected
        protected int width;
        protected int height;
        protected T[,] grid;

        //public
        /// <summary>
        /// DataGrid constructor
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public DataGrid(int width, int height)
        {
            this.width = width;
            this.height = height;
            grid = new T[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = default(T);
                }
            }
        }
        /// <summary>
        /// A property that gives direct access to grid but no index checking
        /// </summary>
        public T[,] Grid
        {
            get { return grid; }
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
        /// returns either the data at x y in the grid or null
        /// </summary>
        /// <param name="x">grid x</param>
        /// <param name="y">grid y</param>
        /// <returns>Data of type T</returns>
        public T dataAt(int x, int y)
        {
            if (inDomain(x, y))
            {
                if (grid[x, y] != null)
                {
                    return grid[x, y];
                }
                else
                {
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }
        /// <summary>
        /// Tests if position in domian of grid
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <returns>Boolean</returns>
        public bool inDomain(int x, int y)
        {
            if (x >= 0 & x < width)
            {
                if (y >= 0 & y < height)
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
        /// Tests if a provided point is a valid point in DataGrid
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <returns>Boolean</returns>
        public bool isValidPoint(int x, int y)
        {
            if (inDomain(x, y))
            {
                if (grid[x, y] != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// Creates new internal board and sets all psoitions to default
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public void formatBoard(int width, int height)
        {
            this.width = width;
            this.height = height;
            grid = new T[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = default(T);
                }
            }
        }
    }
}
