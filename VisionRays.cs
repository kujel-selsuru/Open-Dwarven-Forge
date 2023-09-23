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
    /// Class for casting lines to simulate vision
    /// </summary>
    public static class VisionRays
    {
        /// private
        static List<SimpleLine> lines;

        /// <summary>
        /// VisionRays constructor
        /// </summary>
        static VisionRays()
        {
            lines = new List<SimpleLine>();
        }
        /// <summary>
        /// Casts out lines in an arc centered around an angle
        /// </summary>
        /// <param name="x">X position of casting point</param>
        /// <param name="y">Y position of casting point</param>
        /// <param name="angle">Angle of center of line cone</param>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="numLines">Number of lines to cast in cone</param>
        /// <param name="range">Max length of lines to cast</param>
        /// <param name="scalex">X scaling value</param>
        /// <param name="scaley">Y scaling value</param>
        /// <param name="arc">Range of cone angle</param>
        /// <param name="rangeMode">Hypotinuse of tiles value</param>
        public static void castLines(float x, float y, double angle, MapGraph mg, int numLines, int range, int scalex, int scaley, double arc = 90, int rangeMod = 45)
        {
            float tx = 0;
            float ty = 0;
            double ang = 0;
            ang = angle - (arc / 2);
            if(ang < 0)
            {
                ang = 360 + ang;
            }
            lines.Clear();

            for(int i = 0; i < numLines; i++)
            {
                for(int k = 0; k < range; k++)
                {
                    tx = (float)(Math.Cos(Helpers<int>.degreesToRadians(ang)) * (k * rangeMod)) + x;
                    ty = (float)(Math.Sin(Helpers<int>.degreesToRadians(ang)) * (k * rangeMod)) + y;
                    if(!mg.getCell((int)tx / scalex, (int)ty / scaley))
                    {
                        break;
                    }
                }
                lines.Add(new SimpleLine(x, y, tx, ty));
                ang += arc / numLines;
                if(ang > 360)
                {
                    ang -= 360;
                }
            }
        }
        /// <summary>
        /// Tests if a provided Rectangle interesects with any lines in internal cone of lines
        /// </summary>
        /// <param name="box">Rectangle reference</param>
        /// <returns>Boolean</returns>
        public static bool testLines(Rectangle box)
        {
            for(int i = 0; i < lines.Count; i++)
            {
                if(box.LineIntersects(lines[i]))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Clears internal list of lines
        /// </summary>
        public static void clearLines()
        {
            lines.Clear();
        }
    }
}
