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
    /// SpriteFont class
    /// </summary>
    public static class SpriteFont
    {
        //private 
        static Texture2D texture;
        static IntPtr textSurface;
        static IntPtr font;
        static SDL.SDL_Rect srcRect;
        static SDL.SDL_Rect destRect;

        //public
        /// <summary>
        /// SpriteFont constructor
        /// </summary>
        static SpriteFont()
        {
            texture = default(Texture2D);
            font = default(IntPtr);
        }
        /// <summary>
        /// Load a font for rendering
        /// </summary>
        /// <param name="fileName">File path/name</param>
        /// <param name="ptSize">Font size</param>
        public static void loadFont(string fileName, int ptSize)
        {
            font = SDL_ttf.TTF_OpenFont(fileName, ptSize);
        }
        /// <summary>
        /// Draw text at a specific location with a provided colour 
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="text">Text to render</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="colour">Colour of rendered text</param>
        public static void draw(IntPtr renderer, string text, int x, int y, SDL.SDL_Color colour)
        {  
            textSurface = SDL_ttf.TTF_RenderText_Solid(font, text, colour);
            texture.texture = SDL.SDL_CreateTextureFromSurface(renderer, textSurface);
            srcRect.x = 0;
            srcRect.y = 0;
            srcRect.w = texture.width;
            srcRect.h = texture.width;
            destRect.x = x;
            destRect.y = y;
            destRect.w = texture.width;
            destRect.h = texture.width; 
            SDL.SDL_RenderCopy(renderer, texture.texture, ref srcRect, ref destRect);
            SDL.SDL_FreeSurface(textSurface);
        }
        /// <summary>
        /// Draw text at a specific location with a provided colour 
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="text">Text to render</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="colour">Colour of rendered text</param>
        public static void draw(IntPtr renderer, string text, float x, float y, SDL.SDL_Color colour)
        {
            textSurface = SDL_ttf.TTF_RenderText_Solid(font, text, colour);
            texture.texture = SDL.SDL_CreateTextureFromSurface(renderer, textSurface);
            srcRect.x = 0;
            srcRect.y = 0;
            srcRect.w = texture.width;
            srcRect.h = texture.width;
            destRect.x = (int)x;
            destRect.y = (int)y;
            destRect.w = texture.width;
            destRect.h = texture.width;
            SDL.SDL_RenderCopy(renderer, texture.texture, ref srcRect, ref destRect);
            SDL.SDL_FreeSurface(textSurface);
        }
    }
}
