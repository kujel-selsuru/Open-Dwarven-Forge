//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

namespace XenoLib
{
    /// <summary>
    /// ProgressBar class
    /// </summary>
    public static class ProgressBar
    {
        //public
        /// <summary>
        /// Draws ProgressBar
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        /// <param name="width">Bar length</param>
        /// <param name="construction">Task completed value</param>
        /// <param name="time">Task total value</param>
        /// <param name="colour">SDL_Color</param>
        public static void draw(IntPtr renderer, int x, int y, int winx, int winy, int width, int construction, int time, SDL.SDL_Color colour)
        {
            float percent = percentage(construction, time);
            DrawLine.draw(renderer, new Point2D(x, y + 4), new Point2D(x + (percent * width), y + 4), colour, winx, winy);
            DrawLine.draw(renderer, new Point2D(x, y + 5), new Point2D(x + (percent * width), y + 5), colour, winx, winy);
            DrawLine.draw(renderer, new Point2D(x, y + 6), new Point2D(x + (percent * width), y + 6), colour, winx, winy);
            //BasicLine.drawLineSegment(sb, pixel, new Vector2(x-winx, y+4 - winy), new Vector2(x + (percent * width) - winx, y+4 - winy), Color.Turquoise, 3);
        }
        /// <summary>
        /// Calculates percentage of task completed
        /// </summary>
        /// <param name="construction">Task completed value</param>
        /// <param name="time">Task total value</param>
        /// <returns></returns>
        static float percentage(float construction, float time)
        {
            if (time == 0)
            {
                return 0;
            }
            return (construction / time);
        }
    }
}
