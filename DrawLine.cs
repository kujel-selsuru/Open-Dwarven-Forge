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
    /// DrawLine class
    /// </summary>
    public static class DrawLine
    {
        private static List<Point2D> circle;
        /// <summary>
        /// Draws a line
        /// </summary>
        /// <param name="renderer">Render IntPtr</param>
        /// <param name="a">Point a</param>
        /// <param name="b">Point b</param>
        /// <param name="colour">Colour of line</param>
        /// <param name="winx">Window X offset</param>
        /// <param name="winy">Window Y offset</param>
        public static void draw(IntPtr renderer, Point2D a, Point2D b, SDL.SDL_Color colour, int winx = 0, int winy = 0)
        {
            SDL.SDL_Color temp;
            SDL.SDL_GetRenderDrawColor(renderer, out temp.r, out temp.g, out temp.b, out temp.a);
            SDL.SDL_SetRenderDrawColor(renderer, colour.r, colour.g, colour.b, colour.a);
            SDL.SDL_RenderDrawLine(renderer, (int)a.X - winx, (int)a.Y - winy, (int)b.X - winx, (int)b.Y - winy);
            SDL.SDL_SetRenderDrawColor(renderer, temp.r, temp.g, temp.b, temp.a);
        }

        public static void drawCircle(IntPtr renderer, Point2D center, float radius, SDL.SDL_Color colour, int winx = 0, int winy =0)
        {
            circle = new List<Point2D>();
            for (int i = 0; i < 18; i++)
            {
                circle.Add(new Point2D(((float)Math.Cos(Helpers<double>.degreesToRadians(i * 20)) * radius) + center.X, ((float)Math.Sin(Helpers<double>.degreesToRadians(i * 20)) * radius) + center.Y));
            }
            for(int i = 0; i < circle.Count - 1; i++)
            {
                draw(renderer, circle[i], circle[i + 1], colour, winx, winy);
            }
            draw(renderer, circle[0], circle[circle.Count - 1], colour, winx, winy);
        }
    }
}
