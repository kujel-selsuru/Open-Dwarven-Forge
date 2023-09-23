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
    /// SimpleDraw class
    /// </summary>
    public static class SimpleDraw
    {
        //private
        static SDL.SDL_Rect srcRect;
        static SDL.SDL_Rect destRect;
        static SDL.SDL_Point center;

        //public
        /// <summary>
        /// Draws texture with no transformations
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="texture">Texture2D reference</param>
        /// <param name="srcRectangle">Rectangle reference</param>
        /// <param name="destRectangle">Rectangle reference</param>
        public static void draw(IntPtr renderer, Texture2D texture, Rectangle srcRectangle, Rectangle destRectangle)
        {
            srcRect = srcRectangle.Rect;
            destRect = destRectangle.Rect;
            SDL.SDL_RenderCopy(renderer, texture.texture, ref srcRect, ref destRect);
        }
        /// <summary>
        /// Draws texture with no transformations
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="texture">Texture2D reference</param>
        /// <param name="destRectangle">Rectangle reference</param>
        public static void draw(IntPtr renderer, Texture2D texture, Rectangle destRectangle)
        {
            srcRect.x = 0;
            srcRect.y = 0;
            srcRect.w = (int)destRectangle.Width;
            srcRect.h = (int)destRectangle.Height;
            destRect = destRectangle.Rect;
            SDL.SDL_RenderCopy(renderer, texture.texture, ref srcRect, ref destRect);
        }
        /// <summary>
        /// Draws texture with no transformations
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="texture">Texture2D reference</param>
        /// <param name="srcRect">SDL_Rect reference</param>
        /// <param name="destRect">SDL_Rect reference</param>
        public static void draw(IntPtr renderer, Texture2D texture, SDL.SDL_Rect destRect)
        {
            srcRect.x = 0;
            srcRect.x = 0;
            srcRect.w = destRect.w;
            srcRect.h = destRect.h;
            SDL.SDL_RenderCopy(renderer, texture.texture, ref srcRect, ref destRect);
        }
        /// <summary>
        /// Draws texture with no transformations
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="texture">Texture2D reference</param>
        /// <param name="srcRect">SDL_Rect reference</param>
        /// <param name="destRect">SDL_Rect reference</param>
        public static void draw(IntPtr renderer, Texture2D texture, SDL.SDL_Rect srcRect, SDL.SDL_Rect destRect)
        {
            SDL.SDL_RenderCopy(renderer, texture.texture, ref srcRect, ref destRect);
        }
        /// <summary>
        /// Draws texture with simple rotation transformations
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="texture">Texture2D reference</param>
        /// <param name="srcRectangle">Rectangle reference</param>
        /// <param name="destRectangle">Rectangle reference</param>
        /// <param name="angle">Angle of rotation (in degrees)</param>
        /// <param name="centerP">Point of rotation around</param>
        /// <param name="flip">SDL_RendererFlip flag value</param>
        public static void draw(IntPtr renderer, Texture2D texture, Rectangle srcRectangle, Rectangle destRectangle, double angle, Point2D centerP, SDL.SDL_RendererFlip flip = SDL.SDL_RendererFlip.SDL_FLIP_NONE)
        {
            srcRect = srcRectangle.Rect;
            destRect = destRectangle.Rect;
            center.x = (int)centerP.X;
            center.y = (int)centerP.Y;
            SDL.SDL_RenderCopyEx(renderer, texture.texture, ref srcRect, ref destRect, angle, ref center, flip);
        }
        /// <summary>
        /// Draws texture with simple rotation transformations
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="texture">Texture2D reference</param>
        /// <param name="srcRect">SDL_Rect reference</param>
        /// <param name="destRect">SDL_Rect reference</param>
        /// <param name="angle">Angle of rotation (in degrees)</param>
        /// <param name="centerP">Point of rotation around</param>
        /// <param name="flip">SDL_RendererFlip flag value</param>
        public static void draw(IntPtr renderer, Texture2D texture, SDL.SDL_Rect srcRect, SDL.SDL_Rect destRect, double angle, Point2D centerP, SDL.SDL_RendererFlip flip = SDL.SDL_RendererFlip.SDL_FLIP_NONE)
        {
            center.x = (int)centerP.X;
            center.y = (int)centerP.Y;
            SDL.SDL_RenderCopyEx(renderer, texture.texture, ref srcRect, ref destRect, angle, ref center, flip);
        }
        /// <summary>
        /// Saves window as a png file
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="fileName">File name (do not add extention)</param>
        /// <param name="x">Window x position</param>
        /// <param name="y">Window y position</param>
        /// <param name="w">Window width</param>
        /// <param name="h">Window height</param>
        public static void savePNG(string filePath, string fileName, int x, int y, int w, int h)
        {
            System.Drawing.Rectangle bounds = new System.Drawing.Rectangle(x, y, w, h);
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(bounds.Width, bounds.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            g.CopyFromScreen(new System.Drawing.Point(bounds.X, bounds.Y), System.Drawing.Point.Empty, bounds.Size, System.Drawing.CopyPixelOperation.SourceCopy);
            bitmap.Save(filePath + fileName + ".png", System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
