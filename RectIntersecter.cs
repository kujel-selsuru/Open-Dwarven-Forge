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
    /// Static class for handling intersection of two SDL_Rects
    /// </summary>
    public static class RectIntersecter
    {
        /// <summary>
        /// Tests if two SDL_Rects intersect
        /// </summary>
        /// <param name="A">SDL_Rect A</param>
        /// <param name="B">SDL_Rect B</param>
        /// <returns>Boolean</returns>
        public static bool intersects(SDL.SDL_Rect A, SDL.SDL_Rect B)
        {
            //the sides of the rectangles
            int leftA, leftB;
            int rightA, rightB;
            int topA, topB;
            int bottomA, bottomB;
            leftA = leftB = rightA = rightB = topA = topB = bottomA = bottomB = 0;

            //caluclate the sides of A
            leftA = A.x;
            rightA = A.x + A.w;
            topA = A.y;
            bottomA = A.y + A.h;

            //calculate the sides of B
            leftB = B.x;
            rightB = B.x + B.w;
            topB = B.y;
            bottomB = B.y + B.h;

            //if any of the sides of A are outside of B
            if (bottomA <= topB)
            {
                return false;
            }
            if (topA >= bottomB)
            {
                return false;
            }
            if (rightA <= leftB)
            {
                return false;
            }
            if (leftA >= rightB)
            {
                return false;
            }
            //if none of the sides of A are outside of B
            return true;
        }
    }
}
