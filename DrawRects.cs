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
    public static class DrawRects
    {
        //private 
        static SDL.SDL_Color oldColour;
        static SDL.SDL_Rect rect;
        //public
        /// <summary>
        /// Draws a rectangle
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="rectangle">Rectangle to draw</param>
        /// <param name="colour">Colour of rectangle</param>
        /// <param name="filled">Filled flag value</param>
        public static void drawRect(IntPtr renderer, Rectangle rectangle, SDL.SDL_Color colour, bool filled = false)
        {
            rect = rectangle.Rect;
            SDL.SDL_GetRenderDrawColor(renderer, out oldColour.r, out oldColour.g, out oldColour.b, out oldColour.a);
            SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
            if(!filled)
            {
                SDL.SDL_RenderDrawRect(renderer, ref rect);
            }
            else
            {
                SDL.SDL_RenderFillRect(renderer, ref rect);
            }
            SDL.SDL_SetRenderDrawColor(renderer, oldColour.r, oldColour.g, oldColour.b, oldColour.a);
        }
    }
}
