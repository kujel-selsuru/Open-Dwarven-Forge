//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// Sector32 enumeration
    /// </summary>
    public enum sector32 { a1 = 0, a2, a3, a4, a5, a6, a7, a8, 
        b1, b2, b3, b4, b5, b6, b7, b8, 
        c1, c2, c3, c4, c5, c6, c7, c8,
        d1, d2, d3, d4, d5, d6, d7, d8, 
        e1, e2, e3, e4, e5, e6, e7, e8, 
        f1, f2, f3, f4, f5, f6, f7, f8, 
        g1, g2, g3, g4, g5, g6, g7, g8,
        h1, h2, h3, h4, h5, h6, h7, h8 }
    /// <summary>
    /// a class to calculate the sector in which a point is in within a given area.
    /// the scale values are calculated by deviding the width and height by 8.
    /// </summary>
    public class SectorGraph32
    {
        //protected
        protected sector32[,] graph;
        protected int scalex;
        protected int scaley;

        //public
        /// <summary>
        /// SectorGraph32 constructor
        /// </summary>
        /// <param name="scalex">Scaler x value</param>
        /// <param name="scaley">Scaler y value</param>
        public SectorGraph32(int scalex, int scaley)
        {
            this.scalex = scalex;
            this.scaley = scaley;
            graph = new sector32[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    switch (i)
                    {
                        case 0:
                            switch (k)
                            {
                                case 0:
                                    graph[i, k] = sector32.a1;
                                    break;
                                case 1:
                                    graph[i, k] = sector32.a2;
                                    break;
                                case 2:
                                    graph[i, k] = sector32.a3;
                                    break;
                                case 3:
                                    graph[i, k] = sector32.a4;
                                    break;
                                case 4:
                                    graph[i, k] = sector32.a5;
                                    break;
                                case 5:
                                    graph[i, k] = sector32.a6;
                                    break;
                                case 6:
                                    graph[i, k] = sector32.a7;
                                    break;
                                case 7:
                                    graph[i, k] = sector32.a8;
                                    break;
                            }
                            break;
                        case 1:
                            switch (k)
                            {
                                case 0:
                                    graph[i, k] = sector32.b1;
                                    break;
                                case 1:
                                    graph[i, k] = sector32.b2;
                                    break;
                                case 2:
                                    graph[i, k] = sector32.b3;
                                    break;
                                case 3:
                                    graph[i, k] = sector32.b4;
                                    break;
                                case 4:
                                    graph[i, k] = sector32.b5;
                                    break;
                                case 5:
                                    graph[i, k] = sector32.b6;
                                    break;
                                case 6:
                                    graph[i, k] = sector32.b7;
                                    break;
                                case 7:
                                    graph[i, k] = sector32.b8;
                                    break;
                            }
                            break;
                        case 2:
                            switch (k)
                            {
                                case 0:
                                    graph[i, k] = sector32.c1;
                                    break;
                                case 1:
                                    graph[i, k] = sector32.c2;
                                    break;
                                case 2:
                                    graph[i, k] = sector32.c3;
                                    break;
                                case 3:
                                    graph[i, k] = sector32.c4;
                                    break;
                                case 4:
                                    graph[i, k] = sector32.c5;
                                    break;
                                case 5:
                                    graph[i, k] = sector32.c6;
                                    break;
                                case 6:
                                    graph[i, k] = sector32.c7;
                                    break;
                                case 7:
                                    graph[i, k] = sector32.c8;
                                    break;
                            }
                            break;
                        case 3:
                            switch (k)
                            {
                                case 0:
                                    graph[i, k] = sector32.d1;
                                    break;
                                case 1:
                                    graph[i, k] = sector32.d2;
                                    break;
                                case 2:
                                    graph[i, k] = sector32.d3;
                                    break;
                                case 3:
                                    graph[i, k] = sector32.d4;
                                    break;
                                case 4:
                                    graph[i, k] = sector32.d5;
                                    break;
                                case 5:
                                    graph[i, k] = sector32.d6;
                                    break;
                                case 6:
                                    graph[i, k] = sector32.d7;
                                    break;
                                case 7:
                                    graph[i, k] = sector32.d8;
                                    break;
                            }
                            break;
                        case 4:
                            switch (k)
                            {
                                case 0:
                                    graph[i, k] = sector32.e1;
                                    break;
                                case 1:
                                    graph[i, k] = sector32.e2;
                                    break;
                                case 2:
                                    graph[i, k] = sector32.e3;
                                    break;
                                case 3:
                                    graph[i, k] = sector32.e4;
                                    break;
                                case 4:
                                    graph[i, k] = sector32.e5;
                                    break;
                                case 5:
                                    graph[i, k] = sector32.e6;
                                    break;
                                case 6:
                                    graph[i, k] = sector32.e7;
                                    break;
                                case 7:
                                    graph[i, k] = sector32.e8;
                                    break;
                            }
                            break;
                        case 5:
                            switch (k)
                            {
                                case 0:
                                    graph[i, k] = sector32.f1;
                                    break;
                                case 1:
                                    graph[i, k] = sector32.f2;
                                    break;
                                case 2:
                                    graph[i, k] = sector32.f3;
                                    break;
                                case 3:
                                    graph[i, k] = sector32.f4;
                                    break;
                                case 4:
                                    graph[i, k] = sector32.f5;
                                    break;
                                case 5:
                                    graph[i, k] = sector32.f6;
                                    break;
                                case 6:
                                    graph[i, k] = sector32.f7;
                                    break;
                                case 7:
                                    graph[i, k] = sector32.f8;
                                    break;
                            }
                            break;
                        case 6:
                            switch (k)
                            {
                                case 0:
                                    graph[i, k] = sector32.g1;
                                    break;
                                case 1:
                                    graph[i, k] = sector32.g2;
                                    break;
                                case 2:
                                    graph[i, k] = sector32.g3;
                                    break;
                                case 3:
                                    graph[i, k] = sector32.g4;
                                    break;
                                case 4:
                                    graph[i, k] = sector32.g5;
                                    break;
                                case 5:
                                    graph[i, k] = sector32.g6;
                                    break;
                                case 6:
                                    graph[i, k] = sector32.g7;
                                    break;
                                case 7:
                                    graph[i, k] = sector32.g8;
                                    break;
                            }
                            break;
                        case 7:
                            switch (k)
                            {
                                case 0:
                                    graph[i, k] = sector32.h1;
                                    break;
                                case 1:
                                    graph[i, k] = sector32.h2;
                                    break;
                                case 2:
                                    graph[i, k] = sector32.h3;
                                    break;
                                case 3:
                                    graph[i, k] = sector32.h4;
                                    break;
                                case 4:
                                    graph[i, k] = sector32.h5;
                                    break;
                                case 5:
                                    graph[i, k] = sector32.h6;
                                    break;
                                case 6:
                                    graph[i, k] = sector32.h7;
                                    break;
                                case 7:
                                    graph[i, k] = sector32.h8;
                                    break;
                            }
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Returns a secter32 enumeration provided an x and y position
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Sector32 enumeration</returns>
        public sector32 calculateSector(int x, int y)
        {
            int tempx = x / scalex;
            int tempy = y / scaley;
            return graph[tempx, tempy];
        }
    }
    /// <summary>
    /// Sector64Graph class
    /// </summary>
    public class SectorGraph64
    {
        //protected
        protected int[,] graph;
        protected int scalex;
        protected int scaley;

        //public
        /// <summary>
        /// a class to calculate the sector in which a point is in within a given area.
        /// </summary>
        /// <param name="width">region width</param>
        /// <param name="height">region height</param>
        public SectorGraph64(int width, int height)
        {
            scalex = width / 8;
            scaley = height / 8;
            graph = new int[8,8];
            int temp = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    graph[x, y] = temp;
                    temp++;
                }
            }
        }
        /// <summary>
        /// a value of -1 is considered a none
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <returns>Int</returns>
        public int calculateSector(int x, int y)
        {
            int tempx = x / scalex;
            int tempy = y / scaley;
            return graph[tempx, tempy];
        }
    }
}
