//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SDL2;
//using Microsoft.Xna.Framework;

namespace XenoLib
{
    /// <summary>
    /// A class to store a graph of all passable loactions in a game map for pathfinding purposes
    /// false counts as occupied and true as clear
    /// </summary>
    public class MapGraph
    {
        //protected
        protected bool[,] graph;
        protected int width, height;

        //public
        /// <summary>
        /// MapGraph constructor
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public MapGraph(int width, int height)
        {
            this.width = width;
            this.height = height;

            graph = new bool[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    graph[i, k] = true;
                } 
            }
        }
        /// <summary>
        /// MapGraph from file constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public MapGraph(StreamReader sr)
        {
            sr.ReadLine();//discard testing data
            width = Convert.ToInt32(sr.ReadLine());
            height = Convert.ToInt32(sr.ReadLine());
            graph = new bool[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    graph[i, k] = Convert.ToBoolean(sr.ReadLine());
                }
            }
        }
        /// <summary>
        /// MapGraph copy constructor
        /// </summary>
        /// <param name="obj">MapGraph reference</param>
        public MapGraph(MapGraph obj)
        {
            width = obj.Width;
            height = obj.Height;
            graph = new bool[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    graph[i, k] = obj.getCell(i, k);
                }
            }
        }
        /// <summary>
        /// Save MapGraph data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveData(StreamWriter sw)
        {
            sw.WriteLine("======MapGraph Data======");
            sw.WriteLine(width);
            sw.WriteLine(height);
            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    sw.WriteLine(graph[i, k]);
                }
            }
        }
        /// <summary>
        /// Provide the x and y coordenate of a given cell and the desired value
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="value">Cell value</param>
        public void setCell(int x, int y, bool value)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                graph[x, y] = value;
            }
        }
        /// <summary>
        /// Provide the x and y coordenate of a given cell to get the value stored in it 
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <returns>Boolean</returns>
        public bool getCell(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                return graph[x, y];
            }
            return false;
        }
        /// <summary>
        /// Returns false if an invalid poistion is provided but can return true if a valid position is provided
        /// </summary>
        /// <param name="x">Grid x pos</param>
        /// <param name="y">Grid y pos</param>
        /// <returns>Boolean</returns>
        public bool isValidPoint(int x, int y)
        {
            if (x >= 0 & x < width)
            {
                if (y >= 0 & y < height)
                {
                    return graph[x, y];
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
        /// Returns false if an invalid poistion else returns true
        /// </summary>
        /// <param name="x">Grid x pos</param>
        /// <param name="y">Grid y pos</param>
        /// <returns>Boolean</returns>
        public bool inDomain(int x, int y)
        {
            if (x >= 0 & x < width)
            {
                if (y >= 0 & y < height)
                {
                    return true;
                } 
            }
            return false;
        }
        /// <summary>
        /// Size property, returns as Point2D object
        /// </summary>
        public Point2D Size
        {
            get { return new Point2D(width, height); }
        }
        /// <summary>
        /// Sets all positions to true
        /// </summary>
        public void setAllTrue()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    graph[x, y] = true;
                }
            }
        }
        /// <summary>
        /// Sets all positions to false
        /// </summary>
        public void setAllFalse()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    graph[x, y] = false;
                }
            }
        }
        /// <summary>
        /// Saves data to file
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void save(System.IO.StreamWriter sw)
        {
            sw.WriteLine(width);
            sw.WriteLine(height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    sw.WriteLine(graph[x, y].ToString());
                }
            }
        }
        /// <summary>
        /// Loads data from file
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public void load(System.IO.StreamReader sr)
        {
            width = Convert.ToInt32(sr.ReadLine());
            height = Convert.ToInt32(sr.ReadLine());
            graph = new bool[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    graph[x, y] = Convert.ToBoolean(sr.ReadLine());
                }
            }
        }
        /// <summary>
        /// Tests if position is in provided domain ranges
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <returns>Boolean</returns>
        public static bool inDomain(int x, int y, int w, int h)
        {
            if (x < 0 | x >= w)
            {
                return false;
            }
            if (y < 0 | y >= h)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Tests if a rectanglular area is clear
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="w">Width of area</param>
        /// <param name="h">Height of area</param>
        /// <returns>Boolean</returns>
        public bool areaClear(int x, int y, int w, int h)
        {
            for (int i = x; i < x + w; i++)
            {
                for (int k = y; k < y + h; k++)
                {
                    if (inDomain(i, k, width, height))
                    {
                        if (graph[i, k] == false)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Tests if a rectanglular area is all false
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="w">Width of area</param>
        /// <param name="h">Height of area</param>
        /// <returns>Boolean</returns>
        public bool areaFalse(int x, int y, int w, int h)
        {
            for (int i = x; i < x + w; i++)
            {
                for (int k = y; k < y + h; k++)
                {
                    if (inDomain(i, k, width, height))
                    {
                        if (graph[i, k] == true)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Tests if a rectanglular area is clear (depricated)
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="w">Width of area</param>
        /// <param name="h">Height of area</param>
        /// <returns>Boolean</returns>
        public bool areaClear2(int x, int y, int w, int h)
        {
            for (int i = x; i < x + w; i++)
            {
                for (int k = y; k < y + h; k++)
                {
                    if (inDomain(i, k, width, height))
                    {
                        if (graph[i, k] == false)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Sets a provided area to specified state
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="passible">Passable state</param>
        public void setArea(int x, int y, int w, int h, bool passible = false)
        {
            for (int sx = x; sx < x + w; sx++)
            {
                for (int sy = y; sy < y + h; sy++)
                {
                    if (inDomain(x, y, width, height))
                    {
                        setCell(sx, sy, passible);
                    }
                }
            }
        }
        /// <summary>
        /// Returns a list of open(true) points around a center (x - (w/2), y - (h/2))
        /// </summary>
        /// <param name="x">X center position</param>
        /// <param name="y">Y center position</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <returns>List of Point2D objects representing clear spaces</returns>
        public List<Point2D> getArea(int x, int y, int w, int h)
        {
            List<Point2D> temp = new List<Point2D>();
            for (int sx = x - (w / 2); sx < x + w; sx++)
            {
                for (int sy = y - (h / 2); sy < y + h; sy++)
                {
                    if (inDomain(x, y, width, height))
                    {
                        if(getCell(sx, sy) == true)
                        {
                            temp.Add(new Point2D(sx, sy));
                        }
                    }
                }
            }
            return temp;
        }
        /// <summary>
        /// Returns a list of valid points in a radius
        /// </summary>
        /// <param name="x">Center x position</param>
        /// <param name="y">Center y position</param>
        /// <param name="r">Radius in grid points</param>
        /// <param name="curve">Curve of circle</param>
        /// <returns>A list of Point2D objects</returns>
        public List<Point2D> getRadius(int x, int y, int r, int curve = 2)
        {
            int sx = x - 1;
            int sy = y - r;
            int col = (curve * 2);
            int row = (2 * r);
            List<Point2D> points = new List<Point2D>();
            for (int i = sy; i < sy + row; i++)
            {
                for (int k = sx; k < sx + col; k++)
                {
                    if(i < y - (curve / 2))
                    {
                        col += curve;
                        sx -= (curve / 2);
                    }
                    else if (i > y + (curve / 2))
                    {
                        col -= curve;
                        sx += (curve / 2);
                    }
                    if(isValidPoint(i, k) == true)
                    {
                        points.Add(new Point2D(i, k));
                    }
                }
            }
            return points;
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
    }
}
