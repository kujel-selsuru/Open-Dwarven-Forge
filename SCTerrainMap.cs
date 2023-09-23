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
using XenoLib;

namespace XenoLib
{
    public class SCTerrainMap
    {
        //protected
        protected DataGrid<int> grid;

        //public
        /// <summary>
        /// SCTerrainMap constructor
        /// </summary>
        /// <param name="w">Width in tile/hex spaces</param>
        /// <param name="h">Height in tile/hex spaces</param>
        /// <param name="xScale">Horizontal scaler value</param>
        /// <param name="yScale">Vertical scaler value</param>
        public SCTerrainMap(int w, int h)
        {
            grid = new DataGrid<int>(w, h);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    grid.Grid[x, y] = 0;
                }
            }
        }
        /// <summary>
        /// SCTerrinMap from file constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public SCTerrainMap(StreamReader sr)
        {
            sr.ReadLine();
            string buff1 = sr.ReadLine();
            string buff2 = sr.ReadLine();
            grid = new DataGrid<int>(Convert.ToInt32(buff1),
                Convert.ToInt32(buff2));
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    grid.Grid[x, y] = Convert.ToInt32(sr.ReadLine());
                }
            }
        }
        /// <summary>
        /// Saves SCTerrainMap data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveData(StreamWriter sw)
        {
            sw.WriteLine("======SCTerrainMap Data======");
            sw.WriteLine(grid.Width);
            sw.WriteLine(grid.Height);
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    sw.WriteLine(grid.Grid[x, y]);
                }
            }
        }
        /// <summary>
        /// Returns grid value at specified position, normal values 
        /// are (-1) - 10 and 11 means outside of grid
        /// </summary>
        /// <param name="x">X position in grid spaces</param>
        /// <param name="y">Y position in grid spaces</param>
        /// <returns>Integer</returns>
        public int getValue(float x, float y)
        {
            if (grid.inDomain((int)x, (int)y) == true)
            {
                return grid.Grid[(int)x, (int)y];
            }
            return 11;
        }
        /// <summary>
        /// Sets grid position to specified value in range ((-1) - 10)
        /// </summary>
        /// <param name="x">X position in grid spaces</param>
        /// <param name="y">Y position in grid spaces</param>
        /// <param name="value">Terrain value</param>
        public void setValue(float x, float y, int value)
        {
            int v = value;
            if(v < -1)
            {
                v = -1;
            }
            if(v > 10)
            {
                v = 10;
            }
            if (grid.inDomain((int)x, (int)y) == true)
            {
                grid.Grid[(int)x, (int)y] = v;
            }
        }
        /// <summary>
        /// Returns true if in domain else returns false
        /// </summary>
        /// <param name="x">X position in hexes/tiles</param>
        /// <param name="y">Y position in hexes/tiles</param>
        /// <returns>Boolean</returns>
        public bool inDomain(int x, int y)
        {
            return grid.inDomain(x, y);
        }
    }
}
