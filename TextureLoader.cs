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
    /// Simple struct to hold texture data
    /// </summary>
    public struct Texture2D
    {
        //public 
        public IntPtr texture;
        public int width;
        public int height;

        public Texture2D(IntPtr texture, int width, int height)
        {
            this.texture = texture;
            this.width = width;
            this.height = height;
        }
    }
    /// <summary>
    /// Loads image files and and returns a SDL_Texture
    /// </summary>
    public static class TextureLoader
    {
        /// <summary>
        /// Loads a texture from a file path and makes the provided colour transparent
        /// </summary>
        /// <param name="name">Path of file</param>
        /// <param name="renderer">Renderer pointer</param>
        /// <param name="colour">Colour of transparent pixels</param>
        ///  <param name="width">Width of texture</param>
        ///   <param name="height">Height of texture</param>
        /// <returns>Texture2D</returns>
        public static Texture2D load(string name, IntPtr renderer, SDL.SDL_Color colour, int width, int height)
        {
            using (System.IO.Stream stream = System.IO.File.OpenRead(name))
            {
                IntPtr temp2 = default(IntPtr);
                IntPtr surface = SDL_image.IMG_Load(name);
                IntPtr format = SDL.SDL_AllocFormat(SDL.SDL_PIXELFORMAT_RGB888);
                SDL.SDL_SetColorKey(surface, 1, SDL.SDL_MapRGB(format, colour.r, colour.g, colour.b));
                temp2 = SDL.SDL_CreateTextureFromSurface(renderer, surface);
                Texture2D tex = new Texture2D(temp2, width, height);
                SDL.SDL_FreeSurface(surface);
                return tex;
            }
        }
        /// <summary>
        /// Loads png files with magenta backgrounds and names enging in _widthValue_heightValue
        /// </summary>
        /// <param name="name">Path of file</param>
        /// <param name="renderer">Renderer pointer</param>
        /// <returns>Texture2D object</returns>
        public static Texture2D autoLoad(string name, IntPtr renderer)
        {
            Point2D spriteDimension = Helpers<int>.extractSpriteSize(name);
            return load(name, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), spriteDimension.IX, spriteDimension.IY);
        }
        /// <summary>
        /// Sets specified colour of pixels to a set colour and returns a new Texture2D
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="srcTex">Source texture reference</param>
        /// <param name="colourSet">Colour to change to</param>
        /// <param name="colourSrc">Colour to be changed</param>
        /// <returns>Texture2D reference</returns>
        public static Texture2D setPixelColour(IntPtr renderer, Texture2D srcTex, SDL.SDL_Color colourSet, SDL.SDL_Color colourSrc)
        {
            SDL.SDL_Color tmpColour;
            SDL.SDL_GetRenderDrawColor(renderer, out tmpColour.r, out tmpColour.g, out tmpColour.b, out tmpColour.a);
            Texture2D texture = new Texture2D();
            SDL.SDL_SetRenderTarget(renderer, texture.texture);
            for (int x = 0; x < srcTex.width; x++)
            {
                for (int y = 0; y < srcTex.height; y++)
                {
                    if (getPixel(texture.texture, x, y) == convertColourToUint(colourSrc))
                    {
                        SDL.SDL_SetRenderDrawColor(renderer, colourSet.r, colourSet.g, colourSet.b, colourSet.a);
                        SDL.SDL_RenderDrawPoint(renderer, x, y);
                        SDL.SDL_SetRenderDrawColor(renderer, tmpColour.r, tmpColour.g, tmpColour.b, tmpColour.a);
                    }
                }
            }
            SDL.SDL_SetRenderTarget(renderer, default(IntPtr));
            return texture;
        }
        /// <summary>
        /// Returns a pixel value at a specific position in a surface
        /// Does not check dimensions of surface, check before calling 
        /// </summary>
        /// <param name="surf">IntPtr surface</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>uint pixel value</returns>
        public static uint getPixel(IntPtr surf, int x, int y)
        {
            SDL.SDL_Surface surface = new SDL.SDL_Surface();
            surface.userdata = surf;
            return ((uint)surface.pixels) + (uint)(surface.h * y + x);
        }
        /// <summary>
        /// Converts an SDL_Color to uint
        /// </summary>
        /// <param name="color">SDL_Color struct</param>
        /// <returns>uint value</returns>
        public static uint convertColourToUint(SDL.SDL_Color color)
        {
            return (uint)((color.a << 24) | (color.r << 16) |
                    (color.g << 8) | (color.b << 0));
        }
        /// <summary>
        /// Converts a uint to an SDL_Color
        /// </summary>
        /// <param name="color">uint value</param>
        /// <returns>SDL.SDL_Color struct</returns>
        public static SDL.SDL_Color convertUintToColour(uint color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);
            SDL.SDL_Color colour;
            colour.a = a;
            colour.r = r;
            colour.g = g;
            colour.b = b;
            return colour;
        }
    }
}
