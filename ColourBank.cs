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
    /// XENOCOLOURS enumeration
    /// </summary>
    public enum XENOCOLOURS
    {
        BLACK, WHITE, RED, GREEN, BLUE, NAVY, DARK_BLUE, MEDIUM_BLUE, DARK_GREEN, LIGHT_GREEN,
        TEAL, DARK_CYAN, SPRING_GREEN, AQUA, CORN_FLOWER_BLUE, LAWN_GREEN, MAROON, OLIVE,
        SKY_BLUE, PURPLE, GRAY, BLUE_VIOLET, DARK_RED, SADDLE_BROWN, BROWN, DARK_GRAY,
        DARK_GOLD, MAGENTA, ORCHID, CRIMSON, PALE_GOLD, DARK_ORANGE, ORANGE, FOREST_GREEN,
        SEA_GREEN, LIME_GREEN, TURQUOISE, ROYLE_BLUE, INDIGO, DARK_OLIVE, IVORY, ORANGE_RED,
        SNOW, PINK, LIHGT_PINK, MIDNIGHT_BLUE, LIHGT_CYAN, TAN, CHOCOLATE, SILVER, DARK_VIOLET,
        SLATE_GRAY, DIM_GRAY, GOLD, YELLOW
    }
    /// <summary>
    /// ColourBank class
    /// </summary>
    public static class ColourBank
    {
        //private

        //public
        /// <summary>
        /// ColourBank constructor
        /// </summary>
        static ColourBank()
        {

        }
        /// <summary>
        /// Returns a specified colour or black if none found
        /// </summary>
        /// <param name="value">Name of colour to return</param>
        /// <returns>SDL.SDL_Color</returns>
        public static SDL.SDL_Color getColour(XENOCOLOURS value)
        {
            SDL.SDL_Color colour;
            switch (value)
            {
                case XENOCOLOURS.BLACK:
                    colour.r = 0;
                    colour.g = 0;
                    colour.b = 0;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.WHITE:
                    colour.r = 255;
                    colour.g = 255;
                    colour.b = 255;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.RED:
                    colour.r = 255;
                    colour.g = 0;
                    colour.b = 0;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.GREEN:
                    colour.r = 0;
                    colour.g = 255;
                    colour.b = 0;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.BLUE:
                    colour.r = 0;
                    colour.g = 0;
                    colour.b = 255;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.NAVY:
                    colour.r = 0;
                    colour.g = 0;
                    colour.b = 128;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.DARK_BLUE:
                    colour.r = 0;
                    colour.g = 0;
                    colour.b = 139;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.MEDIUM_BLUE:
                    colour.r = 0;
                    colour.g = 0;
                    colour.b = 205;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.DARK_GREEN:
                    colour.r = 0;
                    colour.g = 100;
                    colour.b = 0;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.LIGHT_GREEN:
                    colour.r = 0;
                    colour.g = 0;
                    colour.b = 205;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.TEAL:
                    colour.r = 0;
                    colour.g = 128;
                    colour.b = 128;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.DARK_CYAN:
                    colour.r = 0;
                    colour.g = 139;
                    colour.b = 139;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.SPRING_GREEN:
                    colour.r = 0;
                    colour.g = 255;
                    colour.b = 127;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.AQUA:
                    colour.r = 0;
                    colour.g = 255;
                    colour.b = 255;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.CORN_FLOWER_BLUE:
                    colour.r = 100;
                    colour.g = 149;
                    colour.b = 237;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.LAWN_GREEN:
                    colour.r = 124;
                    colour.g = 252;
                    colour.b = 0;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.MAROON:
                    colour.r = 128;
                    colour.g = 0;
                    colour.b = 0;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.OLIVE:
                    colour.r = 128;
                    colour.g = 128;
                    colour.b = 0;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.PURPLE:
                    colour.r = 128;
                    colour.g = 0;
                    colour.b = 128;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.SKY_BLUE:
                    colour.r = 135;
                    colour.g = 206;
                    colour.b = 235;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.GRAY:
                    colour.r = 128;
                    colour.g = 128;
                    colour.b = 128;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.BLUE_VIOLET:
                    colour.r = 138;
                    colour.g = 43;
                    colour.b = 226;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.DARK_RED:
                    colour.r = 139;
                    colour.g = 0;
                    colour.b = 0;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.SADDLE_BROWN:
                    colour.r = 139;
                    colour.g = 69;
                    colour.b = 19;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.BROWN:
                    colour.r = 165;
                    colour.g = 42;
                    colour.b = 42;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.DARK_GRAY:
                    colour.r = 169;
                    colour.g = 169;
                    colour.b = 169;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.DARK_GOLD:
                    colour.r = 184;
                    colour.g = 134;
                    colour.b = 11;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.MAGENTA:
                    colour.r = 255;
                    colour.g = 0;
                    colour.b = 255;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.ORCHID:
                    colour.r = 218;
                    colour.g = 112;
                    colour.b = 214;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.CRIMSON:
                    colour.r = 220;
                    colour.g = 20;
                    colour.b = 60;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.PALE_GOLD:
                    colour.r = 238;
                    colour.g = 232;
                    colour.b = 170;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.DARK_ORANGE:
                    colour.r = 255;
                    colour.g = 140;
                    colour.b = 0;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.ORANGE:
                    colour.r = 255;
                    colour.g = 165;
                    colour.b = 0;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.GOLD:
                    colour.r = 255;
                    colour.g = 255;
                    colour.b = 0;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.FOREST_GREEN:
                    colour.r = 34;
                    colour.g = 139;
                    colour.b = 34;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.SEA_GREEN:
                    colour.r = 46;
                    colour.g = 139;
                    colour.b = 87;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.LIME_GREEN:
                    colour.r = 50;
                    colour.g = 205;
                    colour.b = 50;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.TURQUOISE:
                    colour.r = 64;
                    colour.g = 224;
                    colour.b = 208;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.ROYLE_BLUE:
                    colour.r = 65;
                    colour.g = 105;
                    colour.b = 255;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.INDIGO:
                    colour.r = 75;
                    colour.g = 0;
                    colour.b = 130;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.DARK_OLIVE:
                    colour.r = 85;
                    colour.g = 107;
                    colour.b = 47;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.IVORY:
                    colour.r = 255;
                    colour.g = 255;
                    colour.b = 240;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.ORANGE_RED:
                    colour.r = 255;
                    colour.g = 69;
                    colour.b = 0;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.SNOW:
                    colour.r = 255;
                    colour.g = 250;
                    colour.b = 250;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.PINK:
                    colour.r = 255;
                    colour.g = 192;
                    colour.b = 203;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.LIHGT_PINK:
                    colour.r = 255;
                    colour.g = 182;
                    colour.b = 193;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.MIDNIGHT_BLUE:
                    colour.r = 25;
                    colour.g = 25;
                    colour.b = 112;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.LIHGT_CYAN:
                    colour.r = 224;
                    colour.g = 255;
                    colour.b = 255;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.TAN:
                    colour.r = 210;
                    colour.g = 180;
                    colour.b = 140;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.CHOCOLATE:
                    colour.r = 210;
                    colour.g = 105;
                    colour.b = 30;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.SILVER:
                    colour.r = 192;
                    colour.g = 192;
                    colour.b = 192;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.DARK_VIOLET:
                    colour.r = 148;
                    colour.g = 0;
                    colour.b = 211;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.SLATE_GRAY:
                    colour.r = 128;
                    colour.g = 128;
                    colour.b = 144;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.DIM_GRAY:
                    colour.r = 105;
                    colour.g = 105;
                    colour.b = 105;
                    colour.a = 1;
                    return colour;
                case XENOCOLOURS.YELLOW:
                    colour.r = 255;
                    colour.g = 255;
                    colour.b = 0;
                    colour.a = 1;
                    return colour;
            }
            //no colour identified so return black
            colour.r = 0;
            colour.g = 0;
            colour.b = 0;
            colour.a = 1;
            return colour;
        }
    }
}
