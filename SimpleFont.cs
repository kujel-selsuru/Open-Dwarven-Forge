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
using SDL2;

namespace XenoLib
{
    //**************************************************************************
    //Note: only renders in colour of source texture, change source texture for
    //alternate colours
    //**************************************************************************
    /// <summary>
    /// Simple font drawing class, font chars are 32 x 32 pixel white chars and ordered as fallows
    /// a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z,
    /// A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
    /// 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, !, @, #, $, %, ^, &, *, (, ), -, +. _, ?, :, ;,
    /// ", |, \, /, WHITESPACE, {, }, [, ], ., GREATER THAN LEFT, GREATER THAN RIGHT, Comma, =, ~, '
    /// </summary>
    public static class SimpleFont
    {
        //private
        static Texture2D source;
        static string sourceName;
        static Rectangle srcRect;
        static Rectangle destRect;
        static Point2D srcPos;
        static int charSize;
        static int charPos;
        static SDL.SDL_Color oldColour;

        //public
        /// <summary>
        /// SimpleFont constructor
        /// </summary>
        static SimpleFont()
        {
            source = TextureBank.getTexture("white");
            sourceName = "white";
            srcRect = new Rectangle(0, 0, 0, 0);
            destRect = new Rectangle(0, 0, 0, 0);
            charSize = 0;
            charPos = 0;
        }
        /// <summary>
        /// Loads a specified font source from TextureBank
        /// </summary>
        /// <param name="name">Font name</param>
        public static void loadFont(string name)
        {
            source = TextureBank.getTexture(name);
            sourceName = name;
        }
        /// <summary>
        /// Draws a string at a position provided a font source as a Texture2D (depricated)
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="str">String to draw</param>
        /// <param name="x">X position to draw string at</param>
        /// <param name="y">Y position to draw string at</param>
        /// <param name="colour">Colour to render (depricated)</param>
        /// <param name="scaler">Scales size of string to render with 1 being 32x32 pixel chars</param>
        public static void DrawString(IntPtr renderer, string str, int x, int y, SDL.SDL_Color colour = default(SDL.SDL_Color), float scaler = 1.0f)
        {
            charSize = 0;
            charPos = x;
            for (int i = 0; i < str.Length; i++)
            {
                srcPos = GetCharPos(str[i]);
                srcRect.X = srcPos.X;
                srcRect.Y = srcPos.Y;
                charPos += (int)(charSize * scaler);
                charSize = GetCharWidth(str[i]);
                destRect.Height = 32 * scaler;
                DrawChar(renderer, new Point2D(charPos, y), colour, scaler);
            }
        }
        /// <summary>
        /// Draws a string at a position provided a font source as a Texture2D (depricated)
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="str">String to draw</param>
        /// <param name="x">X position to draw string at</param>
        /// <param name="y">Y position to draw string at</param>
        /// <param name="colour">Colour to render (depricated)</param>
        /// <param name="scaler">Scales size of string to render with 1 being 32x32 pixel chars</param>
        public static void DrawString(IntPtr renderer, string str, float x, float y, SDL.SDL_Color colour = default(SDL.SDL_Color), float scaler = 1.0f)
        {
            charSize = 0;
            charPos = (int)x;
            for (int i = 0; i < str.Length; i++)
            {
                srcPos = GetCharPos(str[i]);
                srcRect.X = srcPos.X;
                srcRect.Y = srcPos.Y;
                charPos += (int)(charSize * scaler);
                charSize = GetCharWidth(str[i]);
                destRect.Height = 32 * scaler;
                DrawChar(renderer, new Point2D(charPos, y), colour, scaler);
            }
        }
        /// <summary>
        /// Draws a string at a position provided a font source as a Texture2D
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="str">String to draw</param>
        /// <param name="x">X position to draw string at</param>
        /// <param name="y">Y position to draw string at</param>
        /// <param name="scaler">Scales size of string to render with 1 being 32x32 pixel chars</param>
        public static void DrawString(IntPtr renderer, string str, int x, int y, float scaler = 1.0f)
        {
            charSize = 0;
            charPos = x;
            for (int i = 0; i < str.Length; i++)
            {
                srcPos = GetCharPos(str[i]);
                srcRect.X = srcPos.X;
                srcRect.Y = srcPos.Y;
                charPos += (int)(charSize * scaler);
                charSize = GetCharWidth(str[i]);
                destRect.Height = 32 * scaler;
                DrawChar(renderer, new Point2D(charPos, y), scaler);
            }
        }
        /// <summary>
        /// Draws a string at a position provided a font source as a Texture2D
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="str">String to draw</param>
        /// <param name="x">X position to draw string at</param>
        /// <param name="y">Y position to draw string at</param>
        /// <param name="scaler">Scales size of string to render with 1 being 32x32 pixel chars</param>
        public static void DrawString(IntPtr renderer, string str, float x, float y, float scaler = 1.0f)
        {
            charSize = 0;
            charPos = (int)x;
            for (int i = 0; i < str.Length; i++)
            {
                srcPos = GetCharPos(str[i]);
                srcRect.X = srcPos.X;
                srcRect.Y = srcPos.Y;
                charPos += (int)(charSize * scaler);
                charSize = GetCharWidth(str[i]);
                destRect.Height = 32 * scaler;
                DrawChar(renderer, new Point2D(charPos, y), scaler);
            }
        }
        /// <summary>
        /// Returns the source position of a provided char, used internally
        /// </summary>
        /// <param name="c">Char to find</param>
        /// <returns>Returns position as a Point2D</returns>
        public static Point2D GetCharPos(char c)
        {
            Point2D pos = new Point2D(0, 0);
            switch(c)
            { 
                case 'a':
                    pos.X = 6;
                    pos.Y = 0;
                    break;
                case 'b':
                    pos.X = 39;
                    pos.Y = 0;
                    break;
                case 'c':
                    pos.X = 71;
                    pos.Y = 0;
                    break;
                case 'd':
                    pos.X = 100;
                    pos.Y = 0;
                    break;
                case 'e':
                    pos.X = 133;
                    pos.Y = 0;
                    break;
                case 'f':
                    pos.X = 168;
                    pos.Y = 0;
                    break;
                case 'g':
                    pos.X = 198;
                    pos.Y = 0;
                    break;
                case 'h':
                    pos.X = 230;
                    pos.Y = 0;
                    break;
                case 'i':
                    pos.X = 268;
                    pos.Y = 0;
                    break;
                case 'j':
                    pos.X = 299;
                    pos.Y = 0;
                    break;
                case 'k':
                    pos.X = 326;
                    pos.Y = 0;
                    break;
                case 'l':
                    pos.X = 364;
                    pos.Y = 0;
                    break;
                case 'm':
                    pos.X = 385;
                    pos.Y = 0;
                    break;
                case 'n':
                    pos.X = 422;
                    pos.Y = 0;
                    break;
                case 'o':
                    pos.X = 453;
                    pos.Y = 0;
                    break;
                case 'p':
                    pos.X = 487;
                    pos.Y = -4;
                    break;
                case 'q':
                    pos.X = 517;
                    pos.Y = -4;
                    break;
                case 'r':
                    pos.X = 554;
                    pos.Y = 0;
                    break;
                case 's':
                    pos.X = 584;
                    pos.Y = 0;
                    break;
                case 't':
                    pos.X = 615;
                    pos.Y = 0;
                    break;
                case 'u':
                    pos.X = 646;
                    pos.Y = 0;
                    break;
                case 'v':
                    pos.X = 677;
                    pos.Y = 0;
                    break;
                case 'w':
                    pos.X = 705;
                    pos.Y = 0;
                    break;
                case 'x':
                    pos.X = 741;
                    pos.Y = 0;
                    break;
                case 'y':
                    pos.X = 775;
                    pos.Y = 0;
                    break;
                case 'z':
                    pos.X = 9;
                    pos.Y = 32;
                    break;
                case 'A':
                    pos.X = 35;
                    pos.Y = 32;
                    break;
                case 'B':
                    pos.X = 70;
                    pos.Y = 32;
                    break;
                case 'C':
                    pos.X = 100;
                    pos.Y = 32;
                    break;
                case 'D':
                    pos.X = 132;
                    pos.Y = 32;
                    break;
                case 'E':
                    pos.X = 167;
                    pos.Y = 32;
                    break;
                case 'F':
                    pos.X = 199;
                    pos.Y = 32;
                    break;
                case 'G':
                    pos.X = 227;
                    pos.Y = 32;
                    break;
                case 'H':
                    pos.X = 260;
                    pos.Y = 32;
                    break;
                case 'I':
                    pos.X = 299;
                    pos.Y = 32;
                    break;
                case 'J':
                    pos.X = 329;
                    pos.Y = 32;
                    break;
                case 'K':
                    pos.X = 357;
                    pos.Y = 32;
                    break;
                case 'L':
                    pos.X = 391;
                    pos.Y = 32;
                    break;
                case 'M':
                    pos.X = 416;
                    pos.Y = 32;
                    break;
                case 'N':
                    pos.X = 452;
                    pos.Y = 32;
                    break;
                case 'O':
                    pos.X = 483;
                    pos.Y = 32;
                    break;
                case 'P':
                    pos.X = 518;
                    pos.Y = 32;
                    break;
                case 'Q':
                    pos.X = 545;
                    pos.Y = 32;
                    break;
                case 'R':
                    pos.X = 581;
                    pos.Y = 32;
                    break;
                case 'S':
                    pos.X = 615;
                    pos.Y = 32;
                    break;
                case 'T':
                    pos.X = 645;
                    pos.Y = 32;
                    break;
                case 'U':
                    pos.X = 676;
                    pos.Y = 32;
                    break;
                case 'V':
                    pos.X = 707;
                    pos.Y = 32;
                    break;
                case 'W':
                    pos.X = 736;
                    pos.Y = 32;
                    break;
                case 'X':
                    pos.X = 772;
                    pos.Y = 32;
                    break;
                case 'Y':
                    pos.X = 4;
                    pos.Y = 64;
                    break;
                case 'Z':
                    pos.X = 38;
                    pos.Y = 64;
                    break;
                case '0':
                    pos.X = 69;
                    pos.Y = 64;
                    break;
                case '1':
                    pos.X = 102;
                    pos.Y = 64;
                    break;
                case '2':
                    pos.X = 134;
                    pos.Y = 64;
                    break;
                case '3':
                    pos.X = 166;
                    pos.Y = 64;
                    break;
                case '4':
                    pos.X = 197;
                    pos.Y = 64;
                    break;
                case '5':
                    pos.X = 230;
                    pos.Y = 64;
                    break;
                case '6':
                    pos.X = 261;
                    pos.Y = 64;
                    break;
                case '7':
                    pos.X = 294;
                    pos.Y = 64;
                    break;
                case '8':
                    pos.X = 325;
                    pos.Y = 64;
                    break;
                case '9':
                    pos.X = 358;
                    pos.Y = 64;
                    break;
                case '!':
                    pos.X = 395;
                    pos.Y = 64;
                    break;
                case '@':
                    pos.X = 416;
                    pos.Y = 64;
                    break;
                case '#':
                    pos.X = 454;
                    pos.Y = 64;
                    break;
                case '$':
                    pos.X = 486;
                    pos.Y = 64;
                    break;
                case '%':
                    pos.X = 514;
                    pos.Y = 64;
                    break;
                case '^':
                    pos.X = 549;
                    pos.Y = 64;
                    break;
                case '&':
                    pos.X = 578;
                    pos.Y = 64;
                    break;
                case '*':
                    pos.X = 615;
                    pos.Y = 64;
                    break;
                case '(':
                    pos.X = 648;
                    pos.Y = 64;
                    break;
                case ')':
                    pos.X = 684;
                    pos.Y = 64;
                    break;
                case '-':
                    pos.X = 711;
                    pos.Y = 64;
                    break;
                case '+':
                    pos.X = 741;
                    pos.Y = 64;
                    break;
                case '_':
                    pos.X = 769;
                    pos.Y = 96;
                    break;
                case '?':
                    pos.X = 9;
                    pos.Y = 96;
                    break;
                case ':':
                    pos.X = 43;
                    pos.Y =96;
                    break;
                case ';':
                    pos.X = 75;
                    pos.Y = 96;
                    break;
                case '"':
                    pos.X = 105;
                    pos.Y = 96;
                    break;
                case '|':
                    pos.X = 138;
                    pos.Y = 96;
                    break;
                case '\\':
                    pos.X = 166;
                    pos.Y = 96;
                    break;
                case '/':
                    pos.X = 198;
                    pos.Y = 96;
                    break;
                case ' ':
                    pos.X = 227;
                    pos.Y = 96;
                    break;
                case '{':
                    pos.X = 263;
                    pos.Y = 96;
                    break;
                case '}':
                    pos.X = 300;
                    pos.Y = 96;
                    break;
                case '[':
                    pos.X = 329;
                    pos.Y = 96;
                    break;
                case ']':
                    pos.X = 362;
                    pos.Y = 96;
                    break;
                case '.':
                    pos.X = 395;
                    pos.Y = 96;
                    break;
                case '<':
                    pos.X = 421;
                    pos.Y = 96;
                    break;
                case '>':
                    pos.X = 455;
                    pos.Y = 96;
                    break;
                case ',':
                    pos.X = 491;
                    pos.Y = 96;
                    break;
                case '=':
                    pos.X = 518;
                    pos.Y = 96;
                    break;
                case '~':
                    pos.X = 550;
                    pos.Y = 96;
                    break;
                case '\'':
                    pos.X = 588;
                    pos.Y = 96;
                    break;
            }
            return pos;
        }
        /// <summary>
        /// Returns the source width of a provided char, used internally
        /// </summary>
        /// <param name="c">Char to find</param>
        /// <returns>Returns width as an int</returns>
        public static int GetCharWidth(char c)
        {
            
            switch (c)
            {
                case 'a':
                    return 18;
                case 'b':
                    return 20;
                case 'c':
                    return 18;
                case 'd':
                    return 20;
                case 'e':
                    return 20;
                case 'f':
                    return 14;
                case 'g':
                    return 20;
                case 'h':
                    return 20;
                case 'i':
                    return 8;
                case 'j':
                    return 12;
                case 'k':
                    return 20;
                case 'l':
                    return 12;
                case 'm':
                    return 30;
                case 'n':
                    return 20;
                case 'o':
                    return 23;
                case 'p':
                    return 20;
                case 'q':
                    return 18;
                case 'r':
                    return 16;
                case 's':
                    return 18;
                case 't':
                    return 16;
                case 'u':
                    return 20;
                case 'v':
                    return 22;
                case 'w':
                    return 28;
                case 'x':
                    return 20;
                case 'y':
                    return 20;
                case 'z':
                    return 18;
                case 'A':
                    return 25;
                case 'B':
                    return 22;
                case 'C':
                    return 21;
                case 'D':
                    return 24;
                case 'E':
                    return 18;
                case 'F':
                    return 17;
                case 'G':
                    return 25;
                case 'H':
                    return 23;
                case 'I':
                    return 9;
                case 'J':
                    return 14;
                case 'K':
                    return 21;
                case 'L':
                    return 18;
                case 'M':
                    return 31;
                case 'N':
                    return 24;
                case 'O':
                    return 27;
                case 'P':
                    return 21;
                case 'Q':
                    return 28;
                case 'R':
                    return 21;
                case 'S':
                    return 19;
                case 'T':
                    return 22;
                case 'U':
                    return 24;
                case 'V':
                    return 26;
                case 'W':
                    return 32;
                case 'X':
                    return 24;
                case 'Y':
                    return 23;
                case 'Z':
                    return 20;
                case '0':
                    return 21;
                case '1':
                    return 20;
                case '2':
                    return 21;
                case '3':
                    return 21;
                case '4':
                    return 24;
                case '5':
                    return 21;
                case '6':
                    return 21;
                case '7':
                    return 19;
                case '8':
                    return 21;
                case '9':
                    return 21;
                case '!':
                    return 20;
                case '@':
                    return 31;
                case '#':
                    return 21;
                case '$':
                    return 21;
                case '%':
                    return 29;
                case '^':
                    return 22;
                case '&':
                    return 28;
                case '*':
                    return 17;
                case '(':
                    return 12;
                case ')':
                    return 12;
                case '-':
                    return 19;
                case '+':
                    return 21;
                case '_':
                    return 30;
                case '?':
                    return 18;
                case ':':
                    return 10;
                case ';':
                    return 12;
                case '"':
                    return 14;
                case '|':
                    return 10;
                case '\\':
                    return 20;
                case '/':
                    return 20;
                case ' ':
                    return 26;
                case '{':
                    return 15;
                case '}':
                    return 15;
                case '[':
                    return 13;
                case ']':
                    return 13;
                case '.':
                    return 10;
                case '<':
                    return 20;
                case '>':
                    return 20;
                case ',':
                    return 12;
                case '=':
                    return 20;
                case '~':
                    return 21;
                case '\'':
                    return 9;
            }
            return 0;
        }
        /// <summary>
        /// Draws a single char, used internally
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="pos">Position of char to render</param>
        /// <param name="colour">Colour to draw char</param>
        /// <param name="scaler">Scaling value</param>
        public static void DrawChar(IntPtr renderer, Point2D pos, SDL.SDL_Color colour, float scaler)
        {
            srcRect.Width = charSize;
            srcRect.Height = 32;
            destRect.X = pos.X;
            destRect.Y = pos.Y;
            destRect.Width = charSize * scaler;
            SDL.SDL_GetTextureColorMod(source.texture, out oldColour.r, out oldColour.g, out oldColour.b);
            SDL.SDL_SetTextureColorMod(renderer, colour.r, colour.g, colour.b);
            SimpleDraw.draw(renderer, source, srcRect, destRect);
            SDL.SDL_SetTextureColorMod(source.texture, oldColour.r, oldColour.g, oldColour.b);
            //SpriteBatch.Draw(fontSrc, pos, new Vector2(scaler, scaler), colour, new Vector2(0, 0), srcRect);
        }
        /// <summary>
        /// Draws a single char, used internally
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="pos">Position of char to render</param>
        /// <param name="scaler">Scaling value</param>
        public static void DrawChar(IntPtr renderer, Point2D pos, float scaler)
        {
            srcRect.Width = charSize;
            srcRect.Height = 32;
            destRect.X = pos.X;
            destRect.Y = pos.Y;
            destRect.Width = charSize * scaler;
            SimpleDraw.draw(renderer, source, srcRect, destRect);
        }
        /// <summary>
        /// Calculates the width of rendered text
        /// </summary>
        /// <param name="str">String reference</param>
        /// <param name="scaler">Scaler value</param>
        /// <returns>Integer</returns>
        public static int stringRenderWidth(string str, float scaler)
        {
            charSize = 0;
            charPos = 0;
            for (int i = 0; i < str.Length; i++)
            {
                //charPos += (int)(charSize * scaler);
                charSize = GetCharWidth(str[i]);
                charPos += (int)(charSize * scaler);
            }
            return charPos;
        }
        /// <summary>
        /// Loads coloured font sheets into the TextureBank, named by colour;
        /// black, white, green, yellow, pink, red, gray, orange, purple, brown
        /// </summary>
        /// <param name="path">Graphics folder path</param>
        /// <param name="renderer">Renderer reference</param>
        public static void loadFontColours(string path, IntPtr renderer)
        {
            string file = path;
            file += "my font white.png";
            TextureBank.addTexture("white", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font black.png";
            TextureBank.addTexture("black", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font green.png";
            TextureBank.addTexture("green", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font yellow.png";
            TextureBank.addTexture("yellow", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font pink.png";
            TextureBank.addTexture("pink", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font red.png";
            TextureBank.addTexture("red", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font gray.png";
            TextureBank.addTexture("gray", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font orange.png";
            TextureBank.addTexture("orange", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font purple.png";
            TextureBank.addTexture("purple", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font brown.png";
            TextureBank.addTexture("brown", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
        }
        /// <summary>
        /// Draws a coloured string at specified location
        /// *** LoadFontColours must be called before use and font graphics must
        /// use name of "my font 'colour'" ***
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="text">String to render</param>
        /// <param name="x">X position to render at</param>
        /// <param name="y">Y position to render at</param>
        /// <param name="colour">Colour of font to use</param>
        /// <param name="scaler">Scaling value</param>
        public static void drawColourString(IntPtr renderer, string text, float x, float y, string colour, float scaler = 1)
        {
            //set colour of text
            sourceName = colour;
            source = TextureBank.getTexture(colour);

            DrawString(renderer, text, x, y, scaler);

            //reset colour of text to white
            sourceName = "white";
            source = TextureBank.getTexture("white");
        }
    }

    //**************************************************************************
    //Note: only renders in colour of source texture, change source texture for
    //alternate colours
    //**************************************************************************
    /// <summary>
    /// Simple font drawing class, font chars are 32 x 32 pixel white chars and ordered as fallows
    /// a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z,
    /// A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
    /// 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, !, @, #, $, %, ^, &, *, (, ), -, +. _, ?, :, ;,
    /// ", |, \, /, WHITESPACE, {, }, [, ], ., GREATER THAN LEFT, GREATER THAN RIGHT, Comma, =, ~, '
    /// </summary>
    public static class CustomFont
    {
        //private
        static Texture2D source;
        static string sourceName;
        static Rectangle srcRect;
        static Rectangle destRect;
        static Point2D srcPos;
        static int charSize;
        static int charPos;
        static SDL.SDL_Color oldColour;
        static List<Rectangle> charRects;

        //public
        /// <summary>
        /// CustomFont constructor
        /// </summary>
        static CustomFont()
        {
            source = TextureBank.getTexture("white");
            sourceName = "white";
            srcRect = new Rectangle(0, 0, 0, 0);
            destRect = new Rectangle(0, 0, 0, 0);
            charSize = 0;
            charPos = 0;
            charRects = new List<Rectangle>();
        }
        /// <summary>
        /// Loads defualt character positions
        /// </summary>
        public static void loadDefualtSpecs()
        {
            Rectangle temp = new Rectangle(0, 0, 0, 0);
            //a
            temp.X = 6;
            temp.Y = 0;
            temp.Width = 18;
            temp.Height = 32;
            //b
            temp.X = 39;
            temp.Y = 0;
            temp.Width = 20;
            temp.Height = 32;
            //c
            temp.X = 71;
            temp.Y = 0;
            temp.Width = 18;
            temp.Height = 32;
            //d
            temp.X = 100;
            temp.Y = 0;
            temp.Width = 18;
            temp.Height = 32;
            //e
            temp.X = 133;
            temp.Y = 0;
            temp.Width = 14;
            temp.Height = 32;
            //f
            temp.X = 168;
            temp.Y = 0;
            temp.Width = 20;
            temp.Height = 32;
            //g
            temp.X = 198;
            temp.Y = 0;
            temp.Width = 20;
            temp.Height = 32;
            //h
            temp.X = 230;
            temp.Y = 0;
            temp.Width = 8;
            temp.Height = 32;
            //i
            temp.X = 268;
            temp.Y = 0;
            temp.Width = 12;
            temp.Height = 32;
            //j
            temp.X = 299;
            temp.Y = 0;
            temp.Width = 20;
            temp.Height = 32;
            //k
            temp.X = 326;
            temp.Y = 0;
            temp.Width = 12;
            temp.Height = 32;
            //l
            temp.X = 364;
            temp.Y = 0;
            temp.Width = 30;
            temp.Height = 32;
            //m
            temp.X = 385;
            temp.Y = 0;
            temp.Width = 20;
            temp.Height = 32;
            //n
            temp.X = 422;
            temp.Y = 0;
            temp.Width = 23;
            temp.Height = 32;
            //o
            temp.X = 453;
            temp.Y = 0;
            temp.Width = 20;
            temp.Height = 32;
            //p
            temp.X = 487;
            temp.Y = -4;
            temp.Width = 18;
            temp.Height = 32;
            //q
            temp.X = 517;
            temp.Y = -4;
            temp.Width = 16;
            temp.Height = 32;
            //r
            temp.X = 554;
            temp.Y = 0;
            temp.Width = 18;
            temp.Height = 32;
            //s
            temp.X = 584;
            temp.Y = 0;
            temp.Width = 16;
            temp.Height = 32;
            //t
            temp.X = 615;
            temp.Y = 0;
            temp.Width = 20;
            temp.Height = 32;
            //u
            temp.X = 646;
            temp.Y = 0;
            temp.Width = 22;
            temp.Height = 32;
            //v
            temp.X = 677;
            temp.Y = 0;
            temp.Width = 28;
            temp.Height = 32;
            //w
            temp.X = 705;
            temp.Y = 0;
            temp.Width = 20;
            temp.Height = 32;
            //x
            temp.X = 741;
            temp.Y = 0;
            temp.Width = 20;
            temp.Height = 32;
            //y
            temp.X = 775;
            temp.Y = 0;
            temp.Width = 18;
            temp.Height = 32;
            //z
            temp.X = 9;
            temp.Y = 32;
            temp.Width = 25;
            temp.Height = 32;
            //A
            temp.X = 35;
            temp.Y = 32;
            temp.Width = 22;
            temp.Height = 32;
            //B
            temp.X = 70;
            temp.Y = 32;
            temp.Width = 21;
            temp.Height = 32;
            //C
            temp.X = 100;
            temp.Y = 32;
            temp.Width = 24;
            temp.Height = 32;
            //D
            temp.X = 132;
            temp.Y = 32;
            temp.Width = 18;
            temp.Height = 32;
            //E
            temp.X = 167;
            temp.Y = 32;
            temp.Width = 17;
            temp.Height = 32;
            //F
            temp.X = 199;
            temp.Y = 32;
            temp.Width = 25;
            temp.Height = 32;
            //G
            temp.X = 227;
            temp.Y = 32;
            temp.Width = 23;
            temp.Height = 32;
            //H
            temp.X = 260;
            temp.Y = 32;
            temp.Width = 9;
            temp.Height = 32;
            //I
            temp.X = 299;
            temp.Y = 32;
            temp.Width = 14;
            temp.Height = 32;
            //J
            temp.X = 329;
            temp.Y = 32;
            temp.Width = 21;
            temp.Height = 32;
            //K
            temp.X = 357;
            temp.Y = 32;
            temp.Width = 18;
            temp.Height = 32;
            //L
            temp.X = 391;
            temp.Y = 32;
            temp.Width = 31;
            temp.Height = 32;
            //M
            temp.X = 416;
            temp.Y = 32;
            temp.Width = 24;
            temp.Height = 32;
            //N
            temp.X = 452;
            temp.Y = 32;
            temp.Width = 27;
            temp.Height = 32;
            //O
            temp.X = 483;
            temp.Y = 32;
            temp.Width = 21;
            temp.Height = 32;
            //P
            temp.X = 518;
            temp.Y = 32;
            temp.Width = 28;
            temp.Height = 32;
            //Q
            temp.X = 545;
            temp.Y = 32;
            temp.Width = 21;
            temp.Height = 32;
            //R
            temp.X = 581;
            temp.Y = 32;
            temp.Width = 19;
            temp.Height = 32;
            //S
            temp.X = 615;
            temp.Y = 32;
            temp.Width = 22;
            temp.Height = 32;
            //T
            temp.X = 645;
            temp.Y = 32;
            temp.Width = 24;
            temp.Height = 32;
            //U
            temp.X = 676;
            temp.Y = 32;
            temp.Width = 26;
            temp.Height = 32;
            //V
            temp.X = 707;
            temp.Y = 32;
            temp.Width = 32;
            temp.Height = 32;
            //W
            temp.X = 736;
            temp.Y = 32;
            temp.Width = 24;
            temp.Height = 32;
            //X
            temp.X = 772;
            temp.Y = 32;
            temp.Width = 23;
            temp.Height = 32;
            //Y
            temp.X = 4;
            temp.Y = 64;
            temp.Width = 20;
            temp.Height = 32;
            //Z
            temp.X = 38;
            temp.Y = 64;
            temp.Width = 21;
            temp.Height = 32;
            //0
            temp.X = 69;
            temp.Y = 64;
            temp.Width = 20;
            temp.Height = 32;
            //1
            temp.X = 102;
            temp.Y = 64;
            temp.Width = 21;
            temp.Height = 32;
            //2
            temp.X = 134;
            temp.Y = 64;
            temp.Width = 21;
            temp.Height = 32;
            //3
            temp.X = 166;
            temp.Y = 64;
            temp.Width = 24;
            temp.Height = 32;
            //4
            temp.X = 197;
            temp.Y = 64;
            temp.Width = 21;
            temp.Height = 32;
            //5
            temp.X = 230;
            temp.Y = 64;
            temp.Width = 19;
            temp.Height = 32;
            //6
            temp.X = 261;
            temp.Y = 64;
            temp.Width = 21;
            temp.Height = 32;
            //7
            temp.X = 294;
            temp.Y = 64;
            temp.Width = 21;
            temp.Height = 32;
            //8
            temp.X = 325;
            temp.Y = 64;
            temp.Width = 18;
            temp.Height = 32;
            //9
            temp.X = 358;
            temp.Y = 64;
            temp.Width = 18;
            temp.Height = 32;
            //!
            temp.X = 395;
            temp.Y = 64;
            temp.Width = 20;
            temp.Height = 32;
            //@
            temp.X = 416;
            temp.Y = 64;
            temp.Width = 31;
            temp.Height = 32;
            //#
            temp.X = 454;
            temp.Y = 64;
            temp.Width = 21;
            temp.Height = 32;
            //$
            temp.X = 486;
            temp.Y = 64;
            temp.Width = 21;
            temp.Height = 32;
            //%
            temp.X = 514;
            temp.Y = 64;
            temp.Width = 29;
            temp.Height = 32;
            //^
            temp.X = 549;
            temp.Y = 64;
            temp.Width = 22;
            temp.Height = 32;
            //&
            temp.X = 578;
            temp.Y = 64;
            temp.Width = 28;
            temp.Height = 32;
            //*
            temp.X = 615;
            temp.Y = 64;
            temp.Width = 17;
            temp.Height = 32;
            //(
            temp.X = 648;
            temp.Y = 64;
            temp.Width = 12;
            temp.Height = 32;
            //)':
            temp.X = 684;
            temp.Y = 64;
            temp.Width = 12;
            temp.Height = 32;
            //-
            temp.X = 711;
            temp.Y = 64;
            temp.Width = 19;
            temp.Height = 32;
            //+
            temp.X = 741;
            temp.Y = 64;
            temp.Width = 21;
            temp.Height = 32;
            //_
            temp.X = 769;
            temp.Y = 96;
            temp.Width = 30;
            temp.Height = 32;
            //?
            temp.X = 9;
            temp.Y = 96;
            temp.Width = 18;
            temp.Height = 32;
            //:
            temp.X = 43;
            temp.Y = 96;
            temp.Width = 10;
            temp.Height = 32;
            //;
            temp.X = 75;
            temp.Y = 96;
            temp.Width = 12;
            temp.Height = 32;
            //"
            temp.X = 105;
            temp.Y = 96;
            temp.Width = 14;
            temp.Height = 32;
            //|
            temp.X = 138;
            temp.Y = 96;
            temp.Width = 10;
            temp.Height = 32;
            //\\
            temp.X = 166;
            temp.Y = 96;
            temp.Width = 20;
            temp.Height = 32;
            ///
            temp.X = 198;
            temp.Y = 96;
            temp.Width = 20;
            temp.Height = 32;
            //' '
            temp.X = 227;
            temp.Y = 96;
            temp.Width = 26;
            temp.Height = 32;
            //{
            temp.X = 263;
            temp.Y = 96;
            temp.Width = 15;
            temp.Height = 32;
            //}
            temp.X = 300;
            temp.Y = 96;
            temp.Width = 15;
            temp.Height = 32;
            //[
            temp.X = 329;
            temp.Y = 96;
            temp.Width = 13;
            temp.Height = 32;
            //]
            temp.X = 362;
            temp.Y = 96;
            temp.Width = 13;
            temp.Height = 32;
            //.
            temp.X = 395;
            temp.Y = 96;
            temp.Width = 10;
            temp.Height = 32;
            //<
            temp.X = 421;
            temp.Y = 96;
            temp.Width = 20;
            temp.Height = 32;
            //>
            temp.X = 455;
            temp.Y = 96;
            temp.Width = 20;
            temp.Height = 32;
            //,
            temp.X = 491;
            temp.Y = 96;
            temp.Width = 12;
            temp.Height = 32;
            //=
            temp.X = 518;
            temp.Y = 96;
            temp.Width = 20;
            temp.Height = 32;
            //~
            temp.X = 550;
            temp.Y = 96;
            temp.Width = 21;
            temp.Height = 32;
            //'
            temp.X = 588;
            temp.Y = 96;
            temp.Width = 9;
            temp.Height = 32;
        }
        /// <summary>
        /// Loads a font specs from file
        /// </summary>
        /// <param name="fontName">Font file name</param>
        public static void loadFontSpecs(string fontName)
        {
            string fName = "";
            charRects = new List<Rectangle>();
            StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + fontName + ".font");
            sr.ReadLine();
            fName = sr.ReadLine();
            source = TextureBank.getTexture(fName);
            sourceName = fName;
            int num = Convert.ToInt32(sr.ReadLine());
            for(int i = 0; i < num; i++)
            {
                Rectangle temp = new Rectangle(Convert.ToInt32(sr.ReadLine()),
                    Convert.ToInt32(sr.ReadLine()),
                    Convert.ToInt32(sr.ReadLine()),
                    Convert.ToInt32(sr.ReadLine()));
                charRects.Add(temp);
            }
            sr.Close();
        }
        /// <summary>
        /// Saves font spec data
        /// </summary>
        /// <param name="fName">Font file name</param>
        public static void saveFontSpecs(string fName)
        {
            StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + fName + ".font");
            sw.WriteLine("======CustomFont Data======");
            sw.WriteLine(sourceName);
            sw.WriteLine(charRects.Count);
            for(int i = 0; i < charRects.Count - 1; i++)
            {
                sw.WriteLine(charRects[i].IX);
                sw.WriteLine(charRects[i].IY);
                sw.WriteLine((int)charRects[i].Width);
                sw.WriteLine((int)charRects[i].Height);
            }
        }
        /// <summary>
        /// Loads a specified font source from TextureBank
        /// </summary>
        /// <param name="name">Font name</param>
        public static void loadFont(string name)
        {
            source = TextureBank.getTexture(name);
            sourceName = name;
        }
        /// <summary>
        /// Assigns a source rectangle's values for a given character
        /// </summary>
        /// <param name="c">Character value</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width of char</param>
        /// <param name="h">Height of char</param>
        public static void setFontRect(char c, int x, int y, int w, int h)
        {
            switch (c)
            {
                case 'a':
                    charRects[0].X = x;
                    charRects[0].Y = y;
                    charRects[0].Width = w;
                    charRects[0].Height = h;
                    break;
                case 'b':
                    charRects[1].X = x;
                    charRects[1].Y = y;
                    charRects[1].Width = w;
                    charRects[1].Height = h;
                    break;
                case 'c':
                    charRects[2].X = x;
                    charRects[2].Y = y;
                    charRects[2].Width = w;
                    charRects[2].Height = h;
                    break;
                case 'd':
                    charRects[3].X = x;
                    charRects[3].Y = y;
                    charRects[3].Width = w;
                    charRects[3].Height = h;
                    break;
                case 'e':
                    charRects[4].X = x;
                    charRects[4].Y = y;
                    charRects[4].Width = w;
                    charRects[4].Height = h;
                    break;
                case 'f':
                    charRects[5].X = x;
                    charRects[5].Y = y;
                    charRects[5].Width = w;
                    charRects[5].Height = h;
                    break;
                case 'g':
                    charRects[6].X = x;
                    charRects[6].Y = y;
                    charRects[6].Width = w;
                    charRects[6].Height = h;
                    break;
                case 'h':
                    charRects[7].X = x;
                    charRects[7].Y = y;
                    charRects[7].Width = w;
                    charRects[7].Height = h;
                    break;
                case 'i':
                    charRects[8].X = x;
                    charRects[8].Y = y;
                    charRects[8].Width = w;
                    charRects[8].Height = h;
                    break;
                case 'j':
                    charRects[9].X = x;
                    charRects[9].Y = y;
                    charRects[9].Width = w;
                    charRects[9].Height = h;
                    break;
                case 'k':
                    charRects[10].X = x;
                    charRects[10].Y = y;
                    charRects[10].Width = w;
                    charRects[10].Height = h;
                    break;
                case 'l':
                    charRects[11].X = x;
                    charRects[11].Y = y;
                    charRects[11].Width = w;
                    charRects[11].Height = h;
                    break;
                case 'm':
                    charRects[12].X = x;
                    charRects[12].Y = y;
                    charRects[12].Width = w;
                    charRects[12].Height = h;
                    break;
                case 'n':
                    charRects[13].X = x;
                    charRects[13].Y = y;
                    charRects[13].Width = w;
                    charRects[13].Height = h;
                    break;
                case 'o':
                    charRects[14].X = x;
                    charRects[14].Y = y;
                    charRects[14].Width = w;
                    charRects[14].Height = h;
                    break;
                case 'p':
                    charRects[15].X = x;
                    charRects[15].Y = y;
                    charRects[15].Width = w;
                    charRects[15].Height = h;
                    break;
                case 'q':
                    charRects[16].X = x;
                    charRects[16].Y = y;
                    charRects[16].Width = w;
                    charRects[16].Height = h;
                    break;
                case 'r':
                    charRects[17].X = x;
                    charRects[17].Y = y;
                    charRects[17].Width = w;
                    charRects[17].Height = h;
                    break;
                case 's':
                    charRects[18].X = x;
                    charRects[18].Y = y;
                    charRects[18].Width = w;
                    charRects[18].Height = h;
                    break;
                case 't':
                    charRects[19].X = x;
                    charRects[19].Y = y;
                    charRects[19].Width = w;
                    charRects[19].Height = h;
                    break;
                case 'u':
                    charRects[20].X = x;
                    charRects[20].Y = y;
                    charRects[20].Width = w;
                    charRects[20].Height = h;
                    break;
                case 'v':
                    charRects[21].X = x;
                    charRects[21].Y = y;
                    charRects[21].Width = w;
                    charRects[21].Height = h;
                    break;
                case 'w':
                    charRects[22].X = x;
                    charRects[22].Y = y;
                    charRects[22].Width = w;
                    charRects[22].Height = h;
                    break;
                case 'x':
                    charRects[23].X = x;
                    charRects[23].Y = y;
                    charRects[23].Width = w;
                    charRects[23].Height = h;
                    break;
                case 'y':
                    charRects[24].X = x;
                    charRects[24].Y = y;
                    charRects[24].Width = w;
                    charRects[24].Height = h;
                    break;
                case 'z':
                    charRects[25].X = x;
                    charRects[25].Y = y;
                    charRects[25].Width = w;
                    charRects[25].Height = h;
                    break;
                case 'A':
                    charRects[26].X = x;
                    charRects[26].Y = y;
                    charRects[26].Width = w;
                    charRects[26].Height = h;
                    break;
                case 'B':
                    charRects[27].X = x;
                    charRects[27].Y = y;
                    charRects[27].Width = w;
                    charRects[27].Height = h;
                    break;
                case 'C':
                    charRects[28].X = x;
                    charRects[28].Y = y;
                    charRects[28].Width = w;
                    charRects[28].Height = h;
                    break;
                case 'D':
                    charRects[29].X = x;
                    charRects[29].Y = y;
                    charRects[29].Width = w;
                    charRects[29].Height = h;
                    break;
                case 'E':
                    charRects[30].X = x;
                    charRects[30].Y = y;
                    charRects[30].Width = w;
                    charRects[30].Height = h;
                    break;
                case 'F':
                    charRects[31].X = x;
                    charRects[31].Y = y;
                    charRects[31].Width = w;
                    charRects[31].Height = h;
                    break;
                case 'G':
                    charRects[32].X = x;
                    charRects[32].Y = y;
                    charRects[32].Width = w;
                    charRects[32].Height = h;
                    break;
                case 'H':
                    charRects[33].X = x;
                    charRects[33].Y = y;
                    charRects[33].Width = w;
                    charRects[33].Height = h;
                    break;
                case 'I':
                    charRects[34].X = x;
                    charRects[34].Y = y;
                    charRects[34].Width = w;
                    charRects[34].Height = h;
                    break;
                case 'J':
                    charRects[35].X = x;
                    charRects[35].Y = y;
                    charRects[35].Width = w;
                    charRects[35].Height = h;
                    break;
                case 'K':
                    charRects[36].X = x;
                    charRects[36].Y = y;
                    charRects[36].Width = w;
                    charRects[36].Height = h;
                    break;
                case 'L':
                    charRects[37].X = x;
                    charRects[37].Y = y;
                    charRects[37].Width = w;
                    charRects[37].Height = h;
                    break;
                case 'M':
                    charRects[38].X = x;
                    charRects[38].Y = y;
                    charRects[38].Width = w;
                    charRects[38].Height = h;
                    break;
                case 'N':
                    charRects[39].X = x;
                    charRects[39].Y = y;
                    charRects[39].Width = w;
                    charRects[39].Height = h;
                    break;
                case 'O':
                    charRects[40].X = x;
                    charRects[40].Y = y;
                    charRects[40].Width = w;
                    charRects[40].Height = h;
                    break;
                case 'P':
                    charRects[41].X = x;
                    charRects[41].Y = y;
                    charRects[41].Width = w;
                    charRects[41].Height = h;
                    break;
                case 'Q':
                    charRects[42].X = x;
                    charRects[42].Y = y;
                    charRects[42].Width = w;
                    charRects[42].Height = h;
                    break;
                case 'R':
                    charRects[43].X = x;
                    charRects[43].Y = y;
                    charRects[43].Width = w;
                    charRects[43].Height = h;
                    break;
                case 'S':
                    charRects[44].X = x;
                    charRects[44].Y = y;
                    charRects[44].Width = w;
                    charRects[44].Height = h;
                    break;
                case 'T':
                    charRects[45].X = x;
                    charRects[45].Y = y;
                    charRects[45].Width = w;
                    charRects[45].Height = h;
                    break;
                case 'U':
                    charRects[46].X = x;
                    charRects[46].Y = y;
                    charRects[46].Width = w;
                    charRects[46].Height = h;
                    break;
                case 'V':
                    charRects[47].X = x;
                    charRects[47].Y = y;
                    charRects[47].Width = w;
                    charRects[47].Height = h;
                    break;
                case 'W':
                    charRects[48].X = x;
                    charRects[48].Y = y;
                    charRects[48].Width = w;
                    charRects[48].Height = h;
                    break;
                case 'X':
                    charRects[49].X = x;
                    charRects[49].Y = y;
                    charRects[49].Width = w;
                    charRects[49].Height = h;
                    break;
                case 'Y':
                    charRects[50].X = x;
                    charRects[50].Y = y;
                    charRects[50].Width = w;
                    charRects[50].Height = h;
                    break;
                case 'Z':
                    charRects[51].X = x;
                    charRects[51].Y = y;
                    charRects[51].Width = w;
                    charRects[51].Height = h;
                    break;
                case '0':
                    charRects[52].X = x;
                    charRects[52].Y = y;
                    charRects[52].Width = w;
                    charRects[52].Height = h;
                    break;
                case '1':
                    charRects[53].X = x;
                    charRects[53].Y = y;
                    charRects[53].Width = w;
                    charRects[53].Height = h;
                    break;
                case '2':
                    charRects[54].X = x;
                    charRects[54].Y = y;
                    charRects[54].Width = w;
                    charRects[54].Height = h;
                    break;
                case '3':
                    charRects[55].X = x;
                    charRects[55].Y = y;
                    charRects[55].Width = w;
                    charRects[55].Height = h;
                    break;
                case '4':
                    charRects[56].X = x;
                    charRects[56].Y = y;
                    charRects[56].Width = w;
                    charRects[56].Height = h;
                    break;
                case '5':
                    charRects[57].X = x;
                    charRects[57].Y = y;
                    charRects[57].Width = w;
                    charRects[57].Height = h;
                    break;
                case '6':
                    charRects[58].X = x;
                    charRects[58].Y = y;
                    charRects[58].Width = w;
                    charRects[58].Height = h;
                    break;
                case '7':
                    charRects[59].X = x;
                    charRects[59].Y = y;
                    charRects[59].Width = w;
                    charRects[59].Height = h;
                    break;
                case '8':
                    charRects[60].X = x;
                    charRects[60].Y = y;
                    charRects[60].Width = w;
                    charRects[60].Height = h;
                    break;
                case '9':
                    charRects[61].X = x;
                    charRects[61].Y = y;
                    charRects[61].Width = w;
                    charRects[61].Height = h;
                    break;
                case '!':
                    charRects[62].X = x;
                    charRects[62].Y = y;
                    charRects[62].Width = w;
                    charRects[62].Height = h;
                    break;
                case '@':
                    charRects[63].X = x;
                    charRects[63].Y = y;
                    charRects[63].Width = w;
                    charRects[63].Height = h;
                    break;
                case '#':
                    charRects[64].X = x;
                    charRects[64].Y = y;
                    charRects[64].Width = w;
                    charRects[64].Height = h;
                    break;
                case '$':
                    charRects[65].X = x;
                    charRects[65].Y = y;
                    charRects[65].Width = w;
                    charRects[65].Height = h;
                    break;
                case '%':
                    charRects[66].X = x;
                    charRects[66].Y = y;
                    charRects[66].Width = w;
                    charRects[66].Height = h;
                    break;
                case '^':
                    charRects[67].X = x;
                    charRects[67].Y = y;
                    charRects[67].Width = w;
                    charRects[67].Height = h;
                    break;
                case '&':
                    charRects[68].X = x;
                    charRects[68].Y = y;
                    charRects[68].Width = w;
                    charRects[68].Height = h;
                    break;
                case '*':
                    charRects[69].X = x;
                    charRects[69].Y = y;
                    charRects[69].Width = w;
                    charRects[69].Height = h;
                    break;
                case '(':
                    charRects[70].X = x;
                    charRects[70].Y = y;
                    charRects[70].Width = w;
                    charRects[70].Height = h;
                    break;
                case ')':
                    charRects[71].X = x;
                    charRects[71].Y = y;
                    charRects[71].Width = w;
                    charRects[71].Height = h;
                    break;
                case '-':
                    charRects[72].X = x;
                    charRects[72].Y = y;
                    charRects[72].Width = w;
                    charRects[72].Height = h;
                    break;
                case '+':
                    charRects[73].X = x;
                    charRects[73].Y = y;
                    charRects[73].Width = w;
                    charRects[73].Height = h;
                    break;
                case '_':
                    charRects[74].X = x;
                    charRects[74].Y = y;
                    charRects[74].Width = w;
                    charRects[74].Height = h;
                    break;
                case '?':
                    charRects[75].X = x;
                    charRects[75].Y = y;
                    charRects[75].Width = w;
                    charRects[75].Height = h;
                    break;
                case ':':
                    charRects[76].X = x;
                    charRects[76].Y = y;
                    charRects[76].Width = w;
                    charRects[76].Height = h;
                    break;
                case ';':
                    charRects[77].X = x;
                    charRects[77].Y = y;
                    charRects[77].Width = w;
                    charRects[77].Height = h;
                    break;
                case '"':
                    charRects[78].X = x;
                    charRects[78].Y = y;
                    charRects[78].Width = w;
                    charRects[78].Height = h;
                    break;
                case '|':
                    charRects[79].X = x;
                    charRects[79].Y = y;
                    charRects[79].Width = w;
                    charRects[79].Height = h;
                    break;
                case '\\':
                    charRects[80].X = x;
                    charRects[80].Y = y;
                    charRects[80].Width = w;
                    charRects[80].Height = h;
                    break;
                case '/':
                    charRects[81].X = x;
                    charRects[81].Y = y;
                    charRects[81].Width = w;
                    charRects[81].Height = h;
                    break;
                case ' ':
                    charRects[82].X = x;
                    charRects[82].Y = y;
                    charRects[82].Width = w;
                    charRects[82].Height = h;
                    break;
                case '{':
                    charRects[83].X = x;
                    charRects[83].Y = y;
                    charRects[83].Width = w;
                    charRects[83].Height = h;
                    break;
                case '}':
                    charRects[84].X = x;
                    charRects[84].Y = y;
                    charRects[84].Width = w;
                    charRects[84].Height = h;
                    break;
                case '[':
                    charRects[85].X = x;
                    charRects[85].Y = y;
                    charRects[85].Width = w;
                    charRects[85].Height = h;
                    break;
                case ']':
                    charRects[86].X = x;
                    charRects[86].Y = y;
                    charRects[86].Width = w;
                    charRects[86].Height = h;
                    break;
                case '.':
                    charRects[87].X = x;
                    charRects[87].Y = y;
                    charRects[87].Width = w;
                    charRects[87].Height = h;
                    break;
                case '<':
                    charRects[88].X = x;
                    charRects[88].Y = y;
                    charRects[88].Width = w;
                    charRects[88].Height = h;
                    break;
                case '>':
                    charRects[89].X = x;
                    charRects[89].Y = y;
                    charRects[89].Width = w;
                    charRects[89].Height = h;
                    break;
                case ',':
                    charRects[90].X = x;
                    charRects[90].Y = y;
                    charRects[90].Width = w;
                    charRects[90].Height = h;
                    break;
                case '=':
                    charRects[91].X = x;
                    charRects[91].Y = y;
                    charRects[91].Width = w;
                    charRects[91].Height = h;
                    break;
                case '~':
                    charRects[92].X = x;
                    charRects[92].Y = y;
                    charRects[92].Width = w;
                    charRects[92].Height = h;
                    break;
                case '\'':
                    charRects[93].X = x;
                    charRects[93].Y = y;
                    charRects[93].Width = w;
                    charRects[93].Height = h;
                    break;
            }
        }
        /// <summary>
        /// Draws a string at a position provided a font source as a Texture2D (depricated)
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="str">String to draw</param>
        /// <param name="x">X position to draw string at</param>
        /// <param name="y">Y position to draw string at</param>
        /// <param name="colour">Colour to render (depricated)</param>
        /// <param name="scaler">Scales size of string to render with 1 being 32x32 pixel chars</param>
        public static void DrawString(IntPtr renderer, string str, int x, int y, SDL.SDL_Color colour = default(SDL.SDL_Color), float scaler = 1.0f)
        {
            charSize = 0;
            charPos = x;
            for (int i = 0; i < str.Length; i++)
            {
                srcPos = GetCharPos(str[i]);
                srcRect.X = srcPos.X;
                srcRect.Y = srcPos.Y;
                charPos += (int)(charSize * scaler);
                charSize = GetCharWidth(str[i]);
                destRect.Height = 32 * scaler;
                DrawChar(renderer, new Point2D(charPos, y), colour, scaler);
            }
        }
        /// <summary>
        /// Draws a string at a position provided a font source as a Texture2D (depricated)
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="str">String to draw</param>
        /// <param name="x">X position to draw string at</param>
        /// <param name="y">Y position to draw string at</param>
        /// <param name="colour">Colour to render (depricated)</param>
        /// <param name="scaler">Scales size of string to render with 1 being 32x32 pixel chars</param>
        public static void DrawString(IntPtr renderer, string str, float x, float y, SDL.SDL_Color colour = default(SDL.SDL_Color), float scaler = 1.0f)
        {
            charSize = 0;
            charPos = (int)x;
            for (int i = 0; i < str.Length; i++)
            {
                srcPos = GetCharPos(str[i]);
                srcRect.X = srcPos.X;
                srcRect.Y = srcPos.Y;
                charPos += (int)(charSize * scaler);
                charSize = GetCharWidth(str[i]);
                destRect.Height = 32 * scaler;
                DrawChar(renderer, new Point2D(charPos, y), colour, scaler);
            }
        }
        /// <summary>
        /// Draws a string at a position provided a font source as a Texture2D
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="str">String to draw</param>
        /// <param name="x">X position to draw string at</param>
        /// <param name="y">Y position to draw string at</param>
        /// <param name="scaler">Scales size of string to render with 1 being 32x32 pixel chars</param>
        public static void DrawString(IntPtr renderer, string str, int x, int y, float scaler = 1.0f)
        {
            charSize = 0;
            charPos = x;
            for (int i = 0; i < str.Length; i++)
            {
                srcPos = GetCharPos(str[i]);
                srcRect.X = srcPos.X;
                srcRect.Y = srcPos.Y;
                charPos += (int)(charSize * scaler);
                charSize = GetCharWidth(str[i]);
                destRect.Height = 32 * scaler;
                DrawChar(renderer, new Point2D(charPos, y), scaler);
            }
        }
        /// <summary>
        /// Draws a string at a position provided a font source as a Texture2D
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="str">String to draw</param>
        /// <param name="x">X position to draw string at</param>
        /// <param name="y">Y position to draw string at</param>
        /// <param name="scaler">Scales size of string to render with 1 being 32x32 pixel chars</param>
        public static void DrawString(IntPtr renderer, string str, float x, float y, float scaler = 1.0f)
        {
            charSize = 0;
            charPos = (int)x;
            for (int i = 0; i < str.Length; i++)
            {
                srcPos = GetCharPos(str[i]);
                srcRect.X = srcPos.X;
                srcRect.Y = srcPos.Y;
                charPos += (int)(charSize * scaler);
                charSize = GetCharWidth(str[i]);
                destRect.Height = 32 * scaler;
                DrawChar(renderer, new Point2D(charPos, y), scaler);
            }
        }
        /// <summary>
        /// Returns the source position of a provided char, used internally
        /// </summary>
        /// <param name="c">Char to find</param>
        /// <returns>Returns position as a Point2D</returns>
        public static Point2D GetCharPos(char c)
        {
            Point2D pos = new Point2D(0, 0);
            switch (c)
            {
                case 'a':
                    pos.X = charRects[0].X;
                    pos.Y = charRects[0].Y;
                    break;
                case 'b':
                    pos.X = charRects[2].X;
                    pos.Y = charRects[2].Y;
                    break;
                case 'c':
                    pos.X = charRects[3].X;
                    pos.Y = charRects[3].Y;
                    break;
                case 'd':
                    pos.X = charRects[4].X;
                    pos.Y = charRects[4].Y;
                    break;
                case 'e':
                    pos.X = charRects[5].X;
                    pos.Y = charRects[5].Y;
                    break;
                case 'f':
                    pos.X = charRects[6].X;
                    pos.Y = charRects[6].Y;
                    break;
                case 'g':
                    pos.X = charRects[7].X;
                    pos.Y = charRects[7].Y;
                    break;
                case 'h':
                    pos.X = charRects[8].X;
                    pos.Y = charRects[8].Y;
                    break;
                case 'i':
                    pos.X = charRects[9].X;
                    pos.Y = charRects[9].Y;
                    break;
                case 'j':
                    pos.X = charRects[10].X;
                    pos.Y = charRects[10].Y;
                    break;
                case 'k':
                    pos.X = charRects[11].X;
                    pos.Y = charRects[11].Y;
                    break;
                case 'l':
                    pos.X = charRects[12].X;
                    pos.Y = charRects[12].Y;
                    break;
                case 'm':
                    pos.X = charRects[13].X;
                    pos.Y = charRects[13].Y;
                    break;
                case 'n':
                    pos.X = charRects[14].X;
                    pos.Y = charRects[14].Y;
                    break;
                case 'o':
                    pos.X = charRects[15].X;
                    pos.Y = charRects[15].Y;
                    break;
                case 'p':
                    pos.X = charRects[16].X;
                    pos.Y = charRects[16].Y;
                    break;
                case 'q':
                    pos.X = charRects[17].X;
                    pos.Y = charRects[17].Y;
                    break;
                case 'r':
                    pos.X = charRects[18].X;
                    pos.Y = charRects[18].Y;
                    break;
                case 's':
                    pos.X = charRects[19].X;
                    pos.Y = charRects[19].Y;
                    break;
                case 't':
                    pos.X = charRects[20].X;
                    pos.Y = charRects[20].Y;
                    break;
                case 'u':
                    pos.X = charRects[21].X;
                    pos.Y = charRects[21].Y;
                    break;
                case 'v':
                    pos.X = charRects[22].X;
                    pos.Y = charRects[22].Y;
                    break;
                case 'w':
                    pos.X = charRects[23].X;
                    pos.Y = charRects[23].Y;
                    break;
                case 'x':
                    pos.X = charRects[24].X;
                    pos.Y = charRects[24].Y;
                    break;
                case 'y':
                    pos.X = charRects[25].X;
                    pos.Y = charRects[25].Y;
                    break;
                case 'z':
                    pos.X = charRects[0].X;
                    pos.Y = charRects[0].Y;
                    break;
                case 'A':
                    pos.X = charRects[26].X;
                    pos.Y = charRects[26].Y;
                    break;
                case 'B':
                    pos.X = charRects[27].X;
                    pos.Y = charRects[27].Y;
                    break;
                case 'C':
                    pos.X = charRects[28].X;
                    pos.Y = charRects[28].Y;
                    break;
                case 'D':
                    pos.X = charRects[29].X;
                    pos.Y = charRects[29].Y;
                    break;
                case 'E':
                    pos.X = charRects[30].X;
                    pos.Y = charRects[30].Y;
                    break;
                case 'F':
                    pos.X = charRects[31].X;
                    pos.Y = charRects[31].Y;
                    break;
                case 'G':
                    pos.X = charRects[32].X;
                    pos.Y = charRects[32].Y;
                    break;
                case 'H':
                    pos.X = charRects[33].X;
                    pos.Y = charRects[33].Y;
                    break;
                case 'I':
                    pos.X = charRects[34].X;
                    pos.Y = charRects[34].Y;
                    break;
                case 'J':
                    pos.X = charRects[35].X;
                    pos.Y = charRects[35].Y;
                    break;
                case 'K':
                    pos.X = charRects[36].X;
                    pos.Y = charRects[36].Y;
                    break;
                case 'L':
                    pos.X = charRects[37].X;
                    pos.Y = charRects[37].Y;
                    break;
                case 'M':
                    pos.X = charRects[38].X;
                    pos.Y = charRects[38].Y;
                    break;
                case 'N':
                    pos.X = charRects[39].X;
                    pos.Y = charRects[39].Y;
                    break;
                case 'O':
                    pos.X = charRects[40].X;
                    pos.Y = charRects[40].Y;
                    break;
                case 'P':
                    pos.X = charRects[41].X;
                    pos.Y = charRects[41].Y;
                    break;
                case 'Q':
                    pos.X = charRects[42].X;
                    pos.Y = charRects[42].Y;
                    break;
                case 'R':
                    pos.X = charRects[43].X;
                    pos.Y = charRects[43].Y;
                    break;
                case 'S':
                    pos.X = charRects[44].X;
                    pos.Y = charRects[44].Y;
                    break;
                case 'T':
                    pos.X = charRects[45].X;
                    pos.Y = charRects[45].Y;
                    break;
                case 'U':
                    pos.X = charRects[46].X;
                    pos.Y = charRects[46].Y;
                    break;
                case 'V':
                    pos.X = charRects[47].X;
                    pos.Y = charRects[47].Y;
                    break;
                case 'W':
                    pos.X = charRects[48].X;
                    pos.Y = charRects[48].Y;
                    break;
                case 'X':
                    pos.X = charRects[49].X;
                    pos.Y = charRects[49].Y;
                    break;
                case 'Y':
                    pos.X = charRects[50].X;
                    pos.Y = charRects[50].Y;
                    break;
                case 'Z':
                    pos.X = charRects[51].X;
                    pos.Y = charRects[51].Y;
                    break;
                case '0':
                    pos.X = charRects[52].X;
                    pos.Y = charRects[52].Y;
                    break;
                case '1':
                    pos.X = charRects[53].X;
                    pos.Y = charRects[53].Y;
                    break;
                case '2':
                    pos.X = charRects[54].X;
                    pos.Y = charRects[54].Y;
                    break;
                case '3':
                    pos.X = charRects[55].X;
                    pos.Y = charRects[55].Y;
                    break;
                case '4':
                    pos.X = charRects[56].X;
                    pos.Y = charRects[56].Y;
                    break;
                case '5':
                    pos.X = charRects[57].X;
                    pos.Y = charRects[57].Y;
                    break;
                case '6':
                    pos.X = charRects[58].X;
                    pos.Y = charRects[58].Y;
                    break;
                case '7':
                    pos.X = charRects[59].X;
                    pos.Y = charRects[59].Y;
                    break;
                case '8':
                    pos.X = charRects[60].X;
                    pos.Y = charRects[60].Y;
                    break;
                case '9':
                    pos.X = charRects[61].X;
                    pos.Y = charRects[61].Y;
                    break;
                case '!':
                    pos.X = charRects[62].X;
                    pos.Y = charRects[62].Y;
                    break;
                case '@':
                    pos.X = charRects[63].X;
                    pos.Y = charRects[63].Y;
                    break;
                case '#':
                    pos.X = charRects[64].X;
                    pos.Y = charRects[64].Y;
                    break;
                case '$':
                    pos.X = charRects[65].X;
                    pos.Y = charRects[65].Y;
                    break;
                case '%':
                    pos.X = charRects[66].X;
                    pos.Y = charRects[66].Y;
                    break;
                case '^':
                    pos.X = charRects[67].X;
                    pos.Y = charRects[67].Y;
                    break;
                case '&':
                    pos.X = charRects[68].X;
                    pos.Y = charRects[68].Y;
                    break;
                case '*':
                    pos.X = charRects[69].X;
                    pos.Y = charRects[69].Y;
                    break;
                case '(':
                    pos.X = charRects[70].X;
                    pos.Y = charRects[70].Y;
                    break;
                case ')':
                    pos.X = charRects[71].X;
                    pos.Y = charRects[71].Y;
                    break;
                case '-':
                    pos.X = charRects[72].X;
                    pos.Y = charRects[72].Y;
                    break;
                case '+':
                    pos.X = charRects[73].X;
                    pos.Y = charRects[73].Y;
                    break;
                case '_':
                    pos.X = charRects[74].X;
                    pos.Y = charRects[74].Y;
                    break;
                case '?':
                    pos.X = charRects[75].X;
                    pos.Y = charRects[75].Y;
                    break;
                case ':':
                    pos.X = charRects[76].X;
                    pos.Y = charRects[76].Y;
                    break;
                case ';':
                    pos.X = charRects[77].X;
                    pos.Y = charRects[77].Y;
                    break;
                case '"':
                    pos.X = charRects[78].X;
                    pos.Y = charRects[78].Y;
                    break;
                case '|':
                    pos.X = charRects[79].X;
                    pos.Y = charRects[79].Y;
                    break;
                case '\\':
                    pos.X = charRects[80].X;
                    pos.Y = charRects[80].Y;
                    break;
                case '/':
                    pos.X = charRects[81].X;
                    pos.Y = charRects[81].Y;
                    break;
                case ' ':
                    pos.X = charRects[82].X;
                    pos.Y = charRects[82].Y;
                    break;
                case '{':
                    pos.X = charRects[83].X;
                    pos.Y = charRects[83].Y;
                    break;
                case '}':
                    pos.X = charRects[84].X;
                    pos.Y = charRects[84].Y;
                    break;
                case '[':
                    pos.X = charRects[85].X;
                    pos.Y = charRects[85].Y;
                    break;
                case ']':
                    pos.X = charRects[86].X;
                    pos.Y = charRects[86].Y;
                    break;
                case '.':
                    pos.X = charRects[87].X;
                    pos.Y = charRects[87].Y;
                    break;
                case '<':
                    pos.X = charRects[88].X;
                    pos.Y = charRects[88].Y;
                    break;
                case '>':
                    pos.X = charRects[89].X;
                    pos.Y = charRects[89].Y;
                    break;
                case ',':
                    pos.X = charRects[90].X;
                    pos.Y = charRects[90].Y;
                    break;
                case '=':
                    pos.X = charRects[91].X;
                    pos.Y = charRects[91].Y;
                    break;
                case '~':
                    pos.X = charRects[92].X;
                    pos.Y = charRects[92].Y;
                    break;
                case '\'':
                    pos.X = charRects[93].X;
                    pos.Y = charRects[93].Y;
                    break;
            }
            return pos;
        }
        /// <summary>
        /// Returns the source width of a provided char, used internally
        /// </summary>
        /// <param name="c">Char to find</param>
        /// <returns>Returns width as an int</returns>
        public static int GetCharWidth(char c)
        {
            switch (c)
            {
                case 'a':
                    return (int)charRects[0].Width;
                case 'b':
                    return (int)charRects[1].Width;
                case 'c':
                    return (int)charRects[2].Width;
                case 'd':
                    return (int)charRects[3].Width;
                case 'e':
                    return (int)charRects[4].Width;
                case 'f':
                    return (int)charRects[5].Width;
                case 'g':
                    return (int)charRects[6].Width;
                case 'h':
                    return (int)charRects[7].Width;
                case 'i':
                    return (int)charRects[8].Width;
                case 'j':
                    return (int)charRects[9].Width;
                case 'k':
                    return (int)charRects[10].Width;
                case 'l':
                    return (int)charRects[11].Width;
                case 'm':
                    return (int)charRects[12].Width;
                case 'n':
                    return (int)charRects[13].Width;
                case 'o':
                    return (int)charRects[14].Width;
                case 'p':
                    return (int)charRects[15].Width;
                case 'q':
                    return (int)charRects[16].Width;
                case 'r':
                    return (int)charRects[17].Width;
                case 's':
                    return (int)charRects[18].Width;
                case 't':
                    return (int)charRects[19].Width;
                case 'u':
                    return (int)charRects[20].Width;
                case 'v':
                    return (int)charRects[21].Width;
                case 'w':
                    return (int)charRects[22].Width;
                case 'x':
                    return (int)charRects[23].Width;
                case 'y':
                    return (int)charRects[24].Width;
                case 'z':
                    return (int)charRects[25].Width;
                case 'A':
                    return (int)charRects[26].Width;
                case 'B':
                    return (int)charRects[27].Width;
                case 'C':
                    return (int)charRects[28].Width;
                case 'D':
                    return (int)charRects[29].Width;
                case 'E':
                    return (int)charRects[30].Width;
                case 'F':
                    return (int)charRects[31].Width;
                case 'G':
                    return (int)charRects[32].Width;
                case 'H':
                    return (int)charRects[33].Width;
                case 'I':
                    return (int)charRects[34].Width;
                case 'J':
                    return (int)charRects[35].Width;
                case 'K':
                    return (int)charRects[36].Width;
                case 'L':
                    return (int)charRects[37].Width;
                case 'M':
                    return (int)charRects[38].Width;
                case 'N':
                    return (int)charRects[39].Width;
                case 'O':
                    return (int)charRects[40].Width;
                case 'P':
                    return (int)charRects[41].Width;
                case 'Q':
                    return (int)charRects[42].Width;
                case 'R':
                    return (int)charRects[43].Width;
                case 'S':
                    return (int)charRects[44].Width;
                case 'T':
                    return (int)charRects[45].Width;
                case 'U':
                    return (int)charRects[46].Width;
                case 'V':
                    return (int)charRects[47].Width;
                case 'W':
                    return (int)charRects[48].Width;
                case 'X':
                    return (int)charRects[49].Width;
                case 'Y':
                    return (int)charRects[50].Width;
                case 'Z':
                    return (int)charRects[51].Width;
                case '0':
                    return (int)charRects[52].Width;
                case '1':
                    return (int)charRects[53].Width;
                case '2':
                    return (int)charRects[54].Width;
                case '3':
                    return (int)charRects[55].Width;
                case '4':
                    return (int)charRects[56].Width;
                case '5':
                    return (int)charRects[57].Width;
                case '6':
                    return (int)charRects[58].Width;
                case '7':
                    return (int)charRects[59].Width;
                case '8':
                    return (int)charRects[60].Width;
                case '9':
                    return (int)charRects[61].Width;
                case '!':
                    return (int)charRects[62].Width;
                case '@':
                    return (int)charRects[63].Width;
                case '#':
                    return (int)charRects[64].Width;
                case '$':
                    return (int)charRects[65].Width;
                case '%':
                    return (int)charRects[66].Width;
                case '^':
                    return (int)charRects[67].Width;
                case '&':
                    return (int)charRects[68].Width;
                case '*':
                    return (int)charRects[69].Width;
                case '(':
                    return (int)charRects[70].Width;
                case ')':
                    return (int)charRects[71].Width;
                case '-':
                    return (int)charRects[72].Width;
                case '+':
                    return (int)charRects[73].Width;
                case '_':
                    return (int)charRects[74].Width;
                case '?':
                    return (int)charRects[75].Width;
                case ':':
                    return (int)charRects[76].Width;
                case ';':
                    return (int)charRects[77].Width;
                case '"':
                    return (int)charRects[78].Width;
                case '|':
                    return (int)charRects[79].Width;
                case '\\':
                    return (int)charRects[80].Width;
                case '/':
                    return (int)charRects[81].Width;
                case ' ':
                    return (int)charRects[82].Width;
                case '{':
                    return (int)charRects[83].Width;
                case '}':
                    return (int)charRects[84].Width;
                case '[':
                    return (int)charRects[85].Width;
                case ']':
                    return (int)charRects[86].Width;
                case '.':
                    return (int)charRects[87].Width;
                case '<':
                    return (int)charRects[88].Width;
                case '>':
                    return (int)charRects[89].Width;
                case ',':
                    return (int)charRects[90].Width;
                case '=':
                    return (int)charRects[91].Width;
                case '~':
                    return (int)charRects[92].Width;
                case '\'':
                    return (int)charRects[93].Width;
            }
            return 0;
        }
        /// <summary>
        /// Draws a single char, used internally
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="pos">Position of char to render</param>
        /// <param name="colour">Colour to draw char</param>
        /// <param name="scaler">Scaling value</param>
        public static void DrawChar(IntPtr renderer, Point2D pos, SDL.SDL_Color colour, float scaler)
        {
            srcRect.Width = charSize;
            srcRect.Height = 32;
            destRect.X = pos.X;
            destRect.Y = pos.Y;
            destRect.Width = charSize * scaler;
            SDL.SDL_GetTextureColorMod(source.texture, out oldColour.r, out oldColour.g, out oldColour.b);
            SDL.SDL_SetTextureColorMod(renderer, colour.r, colour.g, colour.b);
            SimpleDraw.draw(renderer, source, srcRect, destRect);
            SDL.SDL_SetTextureColorMod(source.texture, oldColour.r, oldColour.g, oldColour.b);
            //SpriteBatch.Draw(fontSrc, pos, new Vector2(scaler, scaler), colour, new Vector2(0, 0), srcRect);
        }
        /// <summary>
        /// Draws a single char, used internally
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="pos">Position of char to render</param>
        /// <param name="scaler">Scaling value</param>
        public static void DrawChar(IntPtr renderer, Point2D pos, float scaler)
        {
            srcRect.Width = charSize;
            srcRect.Height = 32;
            destRect.X = pos.X;
            destRect.Y = pos.Y;
            destRect.Width = charSize * scaler;
            SimpleDraw.draw(renderer, source, srcRect, destRect);
        }
        /// <summary>
        /// Calculates the width of rendered text
        /// </summary>
        /// <param name="str">String reference</param>
        /// <param name="scaler">Scaler value</param>
        /// <returns>Integer</returns>
        public static int stringRenderWidth(string str, float scaler)
        {
            charSize = 0;
            charPos = 0;
            for (int i = 0; i < str.Length; i++)
            {
                //charPos += (int)(charSize * scaler);
                charSize = GetCharWidth(str[i]);
                charPos += (int)(charSize * scaler);
            }
            return charPos;
        }
        /// <summary>
        /// Loads coloured font sheets into the TextureBank, named by colour;
        /// black, white, green, yellow, pink, red, gray, orange, purple, brown
        /// </summary>
        /// <param name="path">Graphics folder path</param>
        /// <param name="renderer">Renderer reference</param>
        public static void loadFontColours(string path, IntPtr renderer)
        {
            string file = path;
            file += "my font white.png";
            TextureBank.addTexture("white", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font black.png";
            TextureBank.addTexture("black", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font green.png";
            TextureBank.addTexture("green", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font yellow.png";
            TextureBank.addTexture("yellow", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font pink.png";
            TextureBank.addTexture("pink", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font red.png";
            TextureBank.addTexture("red", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font gray.png";
            TextureBank.addTexture("gray", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font orange.png";
            TextureBank.addTexture("orange", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font purple.png";
            TextureBank.addTexture("purple", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font brown.png";
            TextureBank.addTexture("brown", TextureLoader.load(file, renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
        }
        /// <summary>
        /// Draws a coloured string at specified location
        /// *** LoadFontColours must be called before use and font graphics must
        /// use name of "my font 'colour'" ***
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="text">String to render</param>
        /// <param name="x">X position to render at</param>
        /// <param name="y">Y position to render at</param>
        /// <param name="colour">Colour of font to use</param>
        /// <param name="scaler">Scaling value</param>
        public static void drawColourString(IntPtr renderer, string text, float x, float y, string colour, float scaler = 1)
        {
            //set colour of text
            sourceName = colour;
            source = TextureBank.getTexture(colour);

            DrawString(renderer, text, x, y, scaler);

            //reset colour of text to white
            sourceName = "white";
            source = TextureBank.getTexture("white");
        }
        /// <summary>
        /// Returns a reference to a Rectangle for the char source rectange
        /// </summary>
        /// <param name="i">Index value</param>
        /// <returns>Rectangle reference</returns>
        public static Rectangle getRect(int i)
        {
            if(i < 0 || i > charRects.Count - 1)
            {
                return null;
            }
            return charRects[i];
        }
    }

    /// <summary>
    /// DynamicFont class, adds colour shading of white sources and varied source values
    /// </summary>
    public static class DynamicFont
    {
        //private
        static List<Rectangle> boxes;
        static List<string> chars;
        static Texture2D source;
        static string sourceName;
        static Rectangle srcRect;
        static Rectangle destRect;
        static Point2D srcPos;
        static int charSize;
        static int charPos;
        //static SDL.SDL_Color oldColour;

        //public
        static DynamicFont()
        {
            boxes = new List<Rectangle>();
            chars = new List<string>();
            source = TextureBank.getTexture("my font white");
            sourceName = "my font white";
            srcRect = new Rectangle(0, 0, 0, 0);
            destRect = new Rectangle(0, 0, 0, 0);
            charSize = 0;
            charPos = 0;
        }
        /// <summary>
        /// DynamicFont from file contructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public static void loadFontFile(System.IO.StreamReader sr)
        {
            boxes = new List<Rectangle>();
            chars = new List<string>();
            srcRect = new Rectangle(0, 0, 0, 0);
            destRect = new Rectangle(0, 0, 0, 0);
            charSize = 0;
            charPos = 0;

            sr.ReadLine();
            sourceName = sr.ReadLine();
            source = TextureBank.getTexture(sourceName);
            int tmp = Convert.ToInt32(sr.ReadLine());
            string str = "";
            for(int c = 0; c < tmp; c++)
            {
                str = sr.ReadLine();
                chars.Add(str);
            }
            tmp = Convert.ToInt32(sr.ReadLine());
            Rectangle rect = null;
            for (int b = 0; b < tmp; b++)
            {
                rect = new Rectangle(Convert.ToInt32(sr.ReadLine()), Convert.ToInt32(sr.ReadLine()),
                    Convert.ToInt32(sr.ReadLine()), Convert.ToInt32(sr.ReadLine()));
                boxes.Add(rect);
            }
        }
        /// <summary>
        /// Saves DynamicFont data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public static void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======Dynamic Font Data======");
            sw.WriteLine(sourceName);
            sw.WriteLine(chars.Count);
            for(int c = 0; c < chars.Count; c++)
            {
                sw.WriteLine(chars[c]);
            }
            for (int b = 0; b < boxes.Count; b++)
            {
                sw.WriteLine(boxes[b].X);
                sw.WriteLine(boxes[b].Y);
                sw.WriteLine(boxes[b].Width);
                sw.WriteLine(boxes[b].Height);
            }
        }
        /// <summary>
        /// Sets font values to defualts for defualt font source
        /// </summary>
        public static void setDefualts()
        {
            Point2D pos = null;
            Rectangle rect = null;
            int width = 0;
            string str = "";
            pos = GetCharDefualtPos('a');
            width = GetCharDefualtWidth('a');
            str = "a";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('b');
            width = GetCharDefualtWidth('b');
            str = "b";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('c');
            width = GetCharDefualtWidth('c');
            str = "c";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('d');
            width = GetCharDefualtWidth('d');
            str = "d";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('e');
            width = GetCharDefualtWidth('e');
            str = "e";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('f');
            width = GetCharDefualtWidth('f');
            str = "f";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('g');
            width = GetCharDefualtWidth('g');
            str = "g";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('h');
            width = GetCharDefualtWidth('h');
            str = "h";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('i');
            width = GetCharDefualtWidth('i');
            str = "i";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('j');
            width = GetCharDefualtWidth('j');
            str = "j";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('k');
            width = GetCharDefualtWidth('k');
            str = "k";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('l');
            width = GetCharDefualtWidth('l');
            str = "l";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('m');
            width = GetCharDefualtWidth('m');
            str = "m";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('n');
            width = GetCharDefualtWidth('n');
            str = "n";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('o');
            width = GetCharDefualtWidth('o');
            str = "o";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('p');
            width = GetCharDefualtWidth('p');
            str = "p";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('q');
            width = GetCharDefualtWidth('q');
            str = "q";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('r');
            width = GetCharDefualtWidth('r');
            str = "r";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('s');
            width = GetCharDefualtWidth('s');
            str = "s";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('t');
            width = GetCharDefualtWidth('t');
            str = "t";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('u');
            width = GetCharDefualtWidth('u');
            str = "u";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('v');
            width = GetCharDefualtWidth('v');
            str = "v";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('w');
            width = GetCharDefualtWidth('w');
            str = "w";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('x');
            width = GetCharDefualtWidth('x');
            str = "x";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('y');
            width = GetCharDefualtWidth('y');
            str = "y";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('z');
            width = GetCharDefualtWidth('z');
            str = "z";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('A');
            width = GetCharDefualtWidth('A');
            str = "A";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('B');
            width = GetCharDefualtWidth('B');
            str = "B";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('C');
            width = GetCharDefualtWidth('C');
            str = "C";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('D');
            width = GetCharDefualtWidth('D');
            str = "D";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('E');
            width = GetCharDefualtWidth('E');
            str = "E";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('F');
            width = GetCharDefualtWidth('F');
            str = "F";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('G');
            width = GetCharDefualtWidth('G');
            str = "G";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('H');
            width = GetCharDefualtWidth('H');
            str = "H";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('I');
            width = GetCharDefualtWidth('I');
            str = "I";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('J');
            width = GetCharDefualtWidth('J');
            str = "J";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('K');
            width = GetCharDefualtWidth('K');
            str = "K";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('L');
            width = GetCharDefualtWidth('L');
            str = "L";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('M');
            width = GetCharDefualtWidth('M');
            str = "M";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('N');
            width = GetCharDefualtWidth('N');
            str = "N";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('O');
            width = GetCharDefualtWidth('O');
            str = "O";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('P');
            width = GetCharDefualtWidth('P');
            str = "P";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('Q');
            width = GetCharDefualtWidth('Q');
            str = "Q";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('R');
            width = GetCharDefualtWidth('R');
            str = "R";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('S');
            width = GetCharDefualtWidth('S');
            str = "S";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('T');
            width = GetCharDefualtWidth('T');
            str = "T";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('U');
            width = GetCharDefualtWidth('U');
            str = "U";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('V');
            width = GetCharDefualtWidth('V');
            str = "V";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('W');
            width = GetCharDefualtWidth('W');
            str = "W";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('X');
            width = GetCharDefualtWidth('X');
            str = "X";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('Y');
            width = GetCharDefualtWidth('Y');
            str = "Y";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('Z');
            width = GetCharDefualtWidth('Z');
            str = "Z";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('0');
            width = GetCharDefualtWidth('0');
            str = "0";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('1');
            width = GetCharDefualtWidth('1');
            str = "1";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('2');
            width = GetCharDefualtWidth('2');
            str = "2";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('3');
            width = GetCharDefualtWidth('3');
            str = "3";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('4');
            width = GetCharDefualtWidth('4');
            str = "4";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('5');
            width = GetCharDefualtWidth('5');
            str = "5";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('6');
            width = GetCharDefualtWidth('6');
            str = "6";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('7');
            width = GetCharDefualtWidth('7');
            str = "7";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('8');
            width = GetCharDefualtWidth('8');
            str = "8";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('9');
            width = GetCharDefualtWidth('9');
            str = "9";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('!');
            width = GetCharDefualtWidth('!');
            str = "!";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('@');
            width = GetCharDefualtWidth('@');
            str = "@";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('#');
            width = GetCharDefualtWidth('#');
            str = "#";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('$');
            width = GetCharDefualtWidth('$');
            str = "$";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('%');
            width = GetCharDefualtWidth('%');
            str = "%";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('^');
            width = GetCharDefualtWidth('^');
            str = "^";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('&');
            width = GetCharDefualtWidth('&');
            str = "&";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('*');
            width = GetCharDefualtWidth('*');
            str = "*";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('(');
            width = GetCharDefualtWidth('(');
            str = "(";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos(')');
            width = GetCharDefualtWidth(')');
            str = ")";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('+');
            width = GetCharDefualtWidth('+');
            str = "+";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('-');
            width = GetCharDefualtWidth('-');
            str = "-";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('_');
            width = GetCharDefualtWidth('_');
            str = "_";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('?');
            width = GetCharDefualtWidth('?');
            str = "?";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos(':');
            width = GetCharDefualtWidth(':');
            str = ":";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos(';');
            width = GetCharDefualtWidth(';');
            str = ";";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('"');
            width = GetCharDefualtWidth('"');
            str = "\"";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('|');
            width = GetCharDefualtWidth('|');
            str = "|";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('\\');
            width = GetCharDefualtWidth('\\');
            str = "\\";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('/');
            width = GetCharDefualtWidth('/');
            str = "/";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos(' ');
            width = GetCharDefualtWidth(' ');
            str = " ";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('[');
            width = GetCharDefualtWidth('[');
            str = "[";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos(']');
            width = GetCharDefualtWidth(']');
            str = "]";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('{');
            width = GetCharDefualtWidth('{');
            str = "{";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('}');
            width = GetCharDefualtWidth('}');
            str = "}";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('>');
            width = GetCharDefualtWidth('>');
            str = ">";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('<');
            width = GetCharDefualtWidth('<');
            str = "<";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos(',');
            width = GetCharDefualtWidth(',');
            str = ",";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('=');
            width = GetCharDefualtWidth('=');
            str = "=";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('`');
            width = GetCharDefualtWidth('`');
            str = "`";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
            pos = GetCharDefualtPos('\'');
            width = GetCharDefualtWidth('\'');
            str = "'";
            rect = new Rectangle(pos.X, pos.Y, width, 32);
            boxes.Add(rect);
            chars.Add(str);
        }
        /// <summary>
        /// Find a matching char if any and set it's source values
        /// </summary>
        /// <param name="c">Char to set</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position </param>
        /// <param name="w">int width</param>
        /// <param name="h">int height</param>
        public static void setChar(char c, int x, int y, int w, int h)
        {
            for(int i = 0; i < chars.Count; i++)
            {
                if(chars[i] == c.ToString())
                {
                    boxes[i].X = x;
                    boxes[i].Y = y;
                    boxes[i].Width = w;
                    boxes[i].Height = h;
                    break;
                }
            }
        }
        /// <summary>
        /// Returns the source defualt position of a provided char, used internally
        /// </summary>
        /// <param name="c">Char to find</param>
        /// <returns>Returns position as a Point2D</returns>
        public static Point2D GetCharDefualtPos(char c)
        {
            Point2D pos = new Point2D(0, 0);
            switch (c)
            {
                case 'a':
                    pos.X = 6;
                    pos.Y = 0;
                    break;
                case 'b':
                    pos.X = 39;
                    pos.Y = 0;
                    break;
                case 'c':
                    pos.X = 71;
                    pos.Y = 0;
                    break;
                case 'd':
                    pos.X = 100;
                    pos.Y = 0;
                    break;
                case 'e':
                    pos.X = 133;
                    pos.Y = 0;
                    break;
                case 'f':
                    pos.X = 168;
                    pos.Y = 0;
                    break;
                case 'g':
                    pos.X = 198;
                    pos.Y = 0;
                    break;
                case 'h':
                    pos.X = 230;
                    pos.Y = 0;
                    break;
                case 'i':
                    pos.X = 268;
                    pos.Y = 0;
                    break;
                case 'j':
                    pos.X = 299;
                    pos.Y = 0;
                    break;
                case 'k':
                    pos.X = 326;
                    pos.Y = 0;
                    break;
                case 'l':
                    pos.X = 364;
                    pos.Y = 0;
                    break;
                case 'm':
                    pos.X = 385;
                    pos.Y = 0;
                    break;
                case 'n':
                    pos.X = 422;
                    pos.Y = 0;
                    break;
                case 'o':
                    pos.X = 453;
                    pos.Y = 0;
                    break;
                case 'p':
                    pos.X = 487;
                    pos.Y = -4;
                    break;
                case 'q':
                    pos.X = 517;
                    pos.Y = -4;
                    break;
                case 'r':
                    pos.X = 554;
                    pos.Y = 0;
                    break;
                case 's':
                    pos.X = 584;
                    pos.Y = 0;
                    break;
                case 't':
                    pos.X = 615;
                    pos.Y = 0;
                    break;
                case 'u':
                    pos.X = 646;
                    pos.Y = 0;
                    break;
                case 'v':
                    pos.X = 677;
                    pos.Y = 0;
                    break;
                case 'w':
                    pos.X = 705;
                    pos.Y = 0;
                    break;
                case 'x':
                    pos.X = 741;
                    pos.Y = 0;
                    break;
                case 'y':
                    pos.X = 775;
                    pos.Y = 0;
                    break;
                case 'z':
                    pos.X = 9;
                    pos.Y = 32;
                    break;
                case 'A':
                    pos.X = 35;
                    pos.Y = 32;
                    break;
                case 'B':
                    pos.X = 70;
                    pos.Y = 32;
                    break;
                case 'C':
                    pos.X = 100;
                    pos.Y = 32;
                    break;
                case 'D':
                    pos.X = 132;
                    pos.Y = 32;
                    break;
                case 'E':
                    pos.X = 167;
                    pos.Y = 32;
                    break;
                case 'F':
                    pos.X = 199;
                    pos.Y = 32;
                    break;
                case 'G':
                    pos.X = 227;
                    pos.Y = 32;
                    break;
                case 'H':
                    pos.X = 260;
                    pos.Y = 32;
                    break;
                case 'I':
                    pos.X = 299;
                    pos.Y = 32;
                    break;
                case 'J':
                    pos.X = 329;
                    pos.Y = 32;
                    break;
                case 'K':
                    pos.X = 357;
                    pos.Y = 32;
                    break;
                case 'L':
                    pos.X = 391;
                    pos.Y = 32;
                    break;
                case 'M':
                    pos.X = 416;
                    pos.Y = 32;
                    break;
                case 'N':
                    pos.X = 452;
                    pos.Y = 32;
                    break;
                case 'O':
                    pos.X = 483;
                    pos.Y = 32;
                    break;
                case 'P':
                    pos.X = 518;
                    pos.Y = 32;
                    break;
                case 'Q':
                    pos.X = 545;
                    pos.Y = 32;
                    break;
                case 'R':
                    pos.X = 581;
                    pos.Y = 32;
                    break;
                case 'S':
                    pos.X = 615;
                    pos.Y = 32;
                    break;
                case 'T':
                    pos.X = 645;
                    pos.Y = 32;
                    break;
                case 'U':
                    pos.X = 676;
                    pos.Y = 32;
                    break;
                case 'V':
                    pos.X = 707;
                    pos.Y = 32;
                    break;
                case 'W':
                    pos.X = 736;
                    pos.Y = 32;
                    break;
                case 'X':
                    pos.X = 772;
                    pos.Y = 32;
                    break;
                case 'Y':
                    pos.X = 4;
                    pos.Y = 64;
                    break;
                case 'Z':
                    pos.X = 38;
                    pos.Y = 64;
                    break;
                case '0':
                    pos.X = 67;
                    pos.Y = 64;
                    break;
                case '1':
                    pos.X = 102;
                    pos.Y = 64;
                    break;
                case '2':
                    pos.X = 134;
                    pos.Y = 64;
                    break;
                case '3':
                    pos.X = 166;
                    pos.Y = 64;
                    break;
                case '4':
                    pos.X = 197;
                    pos.Y = 64;
                    break;
                case '5':
                    pos.X = 230;
                    pos.Y = 64;
                    break;
                case '6':
                    pos.X = 261;
                    pos.Y = 64;
                    break;
                case '7':
                    pos.X = 294;
                    pos.Y = 64;
                    break;
                case '8':
                    pos.X = 325;
                    pos.Y = 64;
                    break;
                case '9':
                    pos.X = 358;
                    pos.Y = 64;
                    break;
                case '!':
                    pos.X = 395;
                    pos.Y = 64;
                    break;
                case '@':
                    pos.X = 416;
                    pos.Y = 64;
                    break;
                case '#':
                    pos.X = 454;
                    pos.Y = 64;
                    break;
                case '$':
                    pos.X = 486;
                    pos.Y = 64;
                    break;
                case '%':
                    pos.X = 514;
                    pos.Y = 64;
                    break;
                case '^':
                    pos.X = 549;
                    pos.Y = 64;
                    break;
                case '&':
                    pos.X = 578;
                    pos.Y = 64;
                    break;
                case '*':
                    pos.X = 615;
                    pos.Y = 64;
                    break;
                case '(':
                    pos.X = 648;
                    pos.Y = 64;
                    break;
                case ')':
                    pos.X = 684;
                    pos.Y = 64;
                    break;
                case '-':
                    pos.X = 711;
                    pos.Y = 64;
                    break;
                case '+':
                    pos.X = 741;
                    pos.Y = 64;
                    break;
                case '_':
                    pos.X = 769;
                    pos.Y = 96;
                    break;
                case '?':
                    pos.X = 9;
                    pos.Y = 96;
                    break;
                case ':':
                    pos.X = 43;
                    pos.Y = 96;
                    break;
                case ';':
                    pos.X = 75;
                    pos.Y = 96;
                    break;
                case '"':
                    pos.X = 105;
                    pos.Y = 96;
                    break;
                case '|':
                    pos.X = 138;
                    pos.Y = 96;
                    break;
                case '\\':
                    pos.X = 166;
                    pos.Y = 96;
                    break;
                case '/':
                    pos.X = 198;
                    pos.Y = 96;
                    break;
                case ' ':
                    pos.X = 227;
                    pos.Y = 96;
                    break;
                case '{':
                    pos.X = 263;
                    pos.Y = 96;
                    break;
                case '}':
                    pos.X = 300;
                    pos.Y = 96;
                    break;
                case '[':
                    pos.X = 329;
                    pos.Y = 96;
                    break;
                case ']':
                    pos.X = 362;
                    pos.Y = 96;
                    break;
                case '.':
                    pos.X = 395;
                    pos.Y = 96;
                    break;
                case '<':
                    pos.X = 421;
                    pos.Y = 96;
                    break;
                case '>':
                    pos.X = 455;
                    pos.Y = 96;
                    break;
                case ',':
                    pos.X = 491;
                    pos.Y = 96;
                    break;
                case '=':
                    pos.X = 518;
                    pos.Y = 96;
                    break;
                case '~':
                    pos.X = 550;
                    pos.Y = 96;
                    break;
                case '\'':
                    pos.X = 588;
                    pos.Y = 96;
                    break;
            }
            return pos;
        }
        /// <summary>
        /// Returns the source defualt width of a provided char, used internally
        /// </summary>
        /// <param name="c">Char to find</param>
        /// <returns>Returns width as an int</returns>
        public static int GetCharDefualtWidth(char c)
        {

            switch (c)
            {
                case 'a':
                    return 18;
                case 'b':
                    return 20;
                case 'c':
                    return 18;
                case 'd':
                    return 20;
                case 'e':
                    return 20;
                case 'f':
                    return 14;
                case 'g':
                    return 20;
                case 'h':
                    return 20;
                case 'i':
                    return 8;
                case 'j':
                    return 12;
                case 'k':
                    return 20;
                case 'l':
                    return 12;
                case 'm':
                    return 30;
                case 'n':
                    return 20;
                case 'o':
                    return 23;
                case 'p':
                    return 20;
                case 'q':
                    return 18;
                case 'r':
                    return 16;
                case 's':
                    return 18;
                case 't':
                    return 16;
                case 'u':
                    return 20;
                case 'v':
                    return 22;
                case 'w':
                    return 28;
                case 'x':
                    return 20;
                case 'y':
                    return 20;
                case 'z':
                    return 18;
                case 'A':
                    return 25;
                case 'B':
                    return 22;
                case 'C':
                    return 21;
                case 'D':
                    return 24;
                case 'E':
                    return 18;
                case 'F':
                    return 17;
                case 'G':
                    return 25;
                case 'H':
                    return 23;
                case 'I':
                    return 9;
                case 'J':
                    return 14;
                case 'K':
                    return 21;
                case 'L':
                    return 18;
                case 'M':
                    return 31;
                case 'N':
                    return 24;
                case 'O':
                    return 27;
                case 'P':
                    return 21;
                case 'Q':
                    return 28;
                case 'R':
                    return 21;
                case 'S':
                    return 19;
                case 'T':
                    return 22;
                case 'U':
                    return 24;
                case 'V':
                    return 26;
                case 'W':
                    return 32;
                case 'X':
                    return 24;
                case 'Y':
                    return 23;
                case 'Z':
                    return 20;
                case '0':
                    return 21;
                case '1':
                    return 20;
                case '2':
                    return 21;
                case '3':
                    return 21;
                case '4':
                    return 24;
                case '5':
                    return 21;
                case '6':
                    return 21;
                case '7':
                    return 19;
                case '8':
                    return 21;
                case '9':
                    return 21;
                case '!':
                    return 20;
                case '@':
                    return 31;
                case '#':
                    return 21;
                case '$':
                    return 21;
                case '%':
                    return 29;
                case '^':
                    return 22;
                case '&':
                    return 28;
                case '*':
                    return 17;
                case '(':
                    return 12;
                case ')':
                    return 12;
                case '-':
                    return 19;
                case '+':
                    return 21;
                case '_':
                    return 30;
                case '?':
                    return 18;
                case ':':
                    return 12;
                case ';':
                    return 12;
                case '"':
                    return 14;
                case '|':
                    return 10;
                case '\\':
                    return 20;
                case '/':
                    return 20;
                case ' ':
                    return 26;
                case '{':
                    return 15;
                case '}':
                    return 15;
                case '[':
                    return 13;
                case ']':
                    return 13;
                case '.':
                    return 10;
                case '<':
                    return 20;
                case '>':
                    return 20;
                case ',':
                    return 12;
                case '=':
                    return 20;
                case '~':
                    return 21;
                case '\'':
                    return 9;
            }
            return 0;
        }
        /// <summary>
        /// Loads a specified font source from TextureBank
        /// </summary>
        /// <param name="name">Font name</param>
        public static void loadFont(string name)
        {
            source = TextureBank.getTexture(name);
            sourceName = name;
        }
        /// <summary>
        /// Loads the source directly from file path
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="name">File path of font source</param>
        /// <param name="transparent">Transparent colour</param>
        /// <param name="width">Width of source texture</param>
        /// <param name="height">Height of source texture</param>
        public static void loadSourceDirectly(IntPtr renderer, string name, XENOCOLOURS transparent, int width, int height)
        {
            //save sourceName
            string[] strArray = StringParser.parse(name);
            string[] strArray2 = StringParser.parse(strArray[strArray.Length - 1]);
            sourceName = strArray2[1];
            //load source
            IntPtr temp2 = default(IntPtr);
            IntPtr surface = SDL_image.IMG_Load(name);
            IntPtr format = SDL.SDL_AllocFormat(SDL.SDL_PIXELFORMAT_RGB888);
            SDL.SDL_Color colour = ColourBank.getColour(transparent);
            SDL.SDL_SetColorKey(surface, 1, SDL.SDL_MapRGB(format, colour.r, colour.g, colour.b));
            temp2 = SDL.SDL_CreateTextureFromSurface(renderer, surface);
            Texture2D tex = new Texture2D(temp2, width, height);
            SDL.SDL_FreeSurface(surface);
            source = tex;
        }
        /// <summary>
        /// Mods source by colour provided
        /// </summary>
        /// <param name="colour">Colour flag</param>
        public static void setWhiteToColour(XENOCOLOURS colour)
        {
            SDL.SDL_Color c = ColourBank.getColour(colour);
            SDL.SDL_SetTextureColorMod(source.texture, c.r, c.g, c.b);
        }
        /// <summary>
        /// Draws a string at a position provided a font source as a Texture2D
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="str">String to draw</param>
        /// <param name="x">X position to draw string at</param>
        /// <param name="y">Y position to draw string at</param>
        /// <param name="scaler">Scales size of string to render with 1 being 32x32 pixel chars</param>
        public static void DrawString(IntPtr renderer, string str, int x, int y, float scaler = 1.0f)
        {
            charSize = 0;
            charPos = x;
            for (int i = 0; i < str.Length; i++)
            {
                srcPos = GetCharPos(str[i]);
                srcRect.X = srcPos.X;
                srcRect.Y = srcPos.Y;
                charPos += (int)(charSize * scaler);
                charSize = GetCharWidth(str[i]);
                destRect.Height = 32 * scaler;
                DrawChar(renderer, new Point2D(charPos, y), scaler);
            }
        }
        /// <summary>
        /// Draws a string at a position provided a font source as a Texture2D
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="str">String to draw</param>
        /// <param name="x">X position to draw string at</param>
        /// <param name="y">Y position to draw string at</param>
        /// <param name="scaler">Scales size of string to render with 1 being 32x32 pixel chars</param>
        public static void DrawString(IntPtr renderer, string str, float x, float y, float scaler = 1.0f)
        {
            charSize = 0;
            charPos = (int)x;
            for (int i = 0; i < str.Length; i++)
            {
                srcPos = GetCharPos(str[i]);
                srcRect.X = srcPos.X;
                srcRect.Y = srcPos.Y;
                charPos += (int)(charSize * scaler);
                charSize = GetCharWidth(str[i]);
                destRect.Height = 32 * scaler;
                DrawChar(renderer, new Point2D(charPos, y), scaler);
            }
        }
        /// <summary>
        /// Draws a string at a position provided a font source as a Texture2D
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="str">String to draw</param>
        /// <param name="x">X position to draw string at</param>
        /// <param name="y">Y position to draw string at</param>
        /// <param name="width">Max width of render space</param>
        /// <param name="scaler">Scales size of string to render with 1 being 32x32 pixel chars</param>
        public static void DrawString(IntPtr renderer, string str, float x, float y, float width, float scaler = 1.0f)
        {
            charSize = 0;
            charPos = (int)x;
            int srcIndex = 0;
            for (int i = 0; i < str.Length; i++)
            {
                srcPos = GetCharPos(str[i]);
                if (charPos + GetCharWidth(str[i]) <= x + width)
                {
                    srcIndex = getCharIndex(str[i]);
                    srcRect.X = srcPos.X;
                    srcRect.Y = srcPos.Y;
                    srcRect.Width = boxes[srcIndex].Width;
                    srcRect.Height = boxes[srcIndex].Height;
                    charPos += (int)(charSize * scaler);
                    charSize = GetCharWidth(str[i]);
                    destRect.Width = charSize * scaler;
                    destRect.Height = 32 * scaler;
                    DrawChar(renderer, new Point2D(charPos, y), scaler);
                }
                else
                {
                    srcIndex = getCharIndex(str[i]);
                    float tmp = (x + width) - (charPos + boxes[srcIndex].Width);
                    srcRect.X = srcPos.X;
                    srcRect.Y = srcPos.Y;
                    srcRect.Width = boxes[srcIndex].Width;
                    srcRect.Height = boxes[srcIndex].Height;
                    destRect.Width = tmp * scaler;
                    destRect.Height = boxes[srcIndex].Height * scaler;
                    DrawChar(renderer, new Point2D(charPos, y), scaler);
                    break;//no more characters can be drawn so break from loop
                }
            }
        }
        /// <summary>
        /// Returns the source position of a provided char, used internally
        /// </summary>
        /// <param name="c">Char to find</param>
        /// <returns>Returns position as a Point2D</returns>
        public static Point2D GetCharPos(char c)
        {
            Point2D pos = new Point2D();
            for(int i = 0; i < chars.Count; i++)
            {
                if(chars[i] == c.ToString())
                {
                    pos.X = boxes[i].X;
                    pos.Y = boxes[i].Y;
                    break;
                }
            }
            return pos;
        }
        /// <summary>
        /// Returns the source width of a provided char, used internally
        /// </summary>
        /// <param name="c">Char to find</param>
        /// <returns>Returns width as an int</returns>
        public static int GetCharWidth(char c)
        {
            for (int i = 0; i < chars.Count; i++)
            {
                if (chars[i] == c.ToString())
                {
                    return (int)boxes[i].Width;
                }
            }
            return 0;
        }
        /// <summary>
        /// Draws a single char, used internally
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="pos">Position of char to render</param>
        /// <param name="scaler">Scaling value</param>
        public static void DrawChar(IntPtr renderer, Point2D pos, float scaler)
        {
            destRect.X = pos.X;
            destRect.Y = pos.Y;;
            SimpleDraw.draw(renderer, source, srcRect, destRect);
        }
        /// <summary>
        /// Returns the source box index value provided a char
        /// </summary>
        /// <param name="c">Character to get index of</param>
        /// <returns>Integar</returns>
        public static int getCharIndex(char c)
        {
            switch (c)
            {
                case 'a':
                    return 0;
                case 'b':
                    return 1;
                case 'c':
                    return 2;
                case 'd':
                    return 3;
                case 'e':
                    return 4;
                case 'f':
                    return 5;
                case 'g':
                    return 6;
                case 'h':
                    return 7;
                case 'i':
                    return 8;
                case 'j':
                    return 9;
                case 'k':
                    return 10;
                case 'l':
                    return 11;
                case 'm':
                    return 12;
                case 'n':
                    return 13;
                case 'o':
                    return 14;
                case 'p':
                    return 15;
                case 'q':
                    return 16;
                case 'r':
                    return 17;
                case 's':
                    return 18;
                case 't':
                    return 19;
                case 'u':
                    return 20;
                case 'v':
                    return 21;
                case 'w':
                    return 22;
                case 'x':
                    return 23;
                case 'y':
                    return 24;
                case 'z':
                    return 25;
                case 'A':
                    return 26;
                case 'B':
                    return 27;
                case 'C':
                    return 28;
                case 'D':
                    return 29;
                case 'E':
                    return 30;
                case 'F':
                    return 31;
                case 'G':
                    return 32;
                case 'H':
                    return 33;
                case 'I':
                    return 34;
                case 'J':
                    return 35;
                case 'K':
                    return 36;
                case 'L':
                    return 37;
                case 'M':
                    return 38;
                case 'N':
                    return 39;
                case 'O':
                    return 40;
                case 'P':
                    return 41;
                case 'Q':
                    return 42;
                case 'R':
                    return 43;
                case 'S':
                    return 44;
                case 'T':
                    return 45;
                case 'U':
                    return 46;
                case 'V':
                    return 47;
                case 'W':
                    return 48;
                case 'X':
                    return 49;
                case 'Y':
                    return 50;
                case 'Z':
                    return 51;
                case '0':
                    return 52;
                case '1':
                    return 53;
                case '2':
                    return 54;
                case '3':
                    return 55;
                case '4':
                    return 56;
                case '5':
                    return 57;
                case '6':
                    return 58;
                case '7':
                    return 59;
                case '8':
                    return 60;
                case '9':
                    return 61;
                case '!':
                    return 62;
                case '@':
                    return 63;
                case '#':
                    return 64;
                case '$':
                    return 65;
                case '%':
                    return 66;
                case '^':
                    return 67;
                case '&':
                    return 68;
                case '*':
                    return 69;
                case '(':
                    return 70;
                case ')':
                    return 71;
                case '-':
                    return 72;
                case '+':
                    return 73;
                case '_':
                    return 74;
                case '?':
                    return 75;
                case ':':
                    return 76;
                case ';':
                    return 77;
                case '"':
                    return 78;
                case '|':
                    return 79;
                case '\\':
                    return 80;
                case '/':
                    return 81;
                case ' ':
                    return 82;
                case '{':
                    return 83;
                case '}':
                    return 84;
                case '[':
                    return 85;
                case ']':
                    return 86;
                case '.':
                    return 87;
                case '<':
                    return 88;
                case '>':
                    return 89;
                case ',':
                    return 90;
                case '=':
                    return 91;
                case '~':
                    return 92;
                case '\'':
                    return 93;
            }
            return 0;
        }
        /// <summary>
        /// Calculates the width of rendered text
        /// </summary>
        /// <param name="str">String reference</param>
        /// <param name="scaler">Scaler value</param>
        /// <returns>Integer</returns>
        public static int stringRenderWidth(string str, float scaler)
        {
            charSize = 0;
            charPos = 0;
            for (int i = 0; i < str.Length; i++)
            {
                //charPos += (int)(charSize * scaler);
                charSize = GetCharWidth(str[i]);
                charPos += (int)(charSize * scaler);
            }
            return charPos;
        }
        /// <summary>
        /// Loads coloured font sheets into the TextureBank, named by colour;
        /// black, white, green, yellow, pink, red, gray, orange, purple, brown
        /// </summary>
        /// <param name="path">Graphics folder path</param>
        /// <param name="renderer">Renderer reference</param>
        public static void loadFontColours(string path, IntPtr renderer)
        {
            string file = path;
            file += "my font white.png";
            TextureBank.addTexture("white", TextureLoader.load("white", renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font black.png";
            TextureBank.addTexture("black", TextureLoader.load("black", renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font green.png";
            TextureBank.addTexture("green", TextureLoader.load("green", renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font yellow.png";
            TextureBank.addTexture("yellow", TextureLoader.load("yellow", renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font pink.png";
            TextureBank.addTexture("pink", TextureLoader.load("pink", renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font red.png";
            TextureBank.addTexture("red", TextureLoader.load("red", renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font gray.png";
            TextureBank.addTexture("gray", TextureLoader.load("gray", renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font orange.png";
            TextureBank.addTexture("orange", TextureLoader.load("orange", renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font purple.png";
            TextureBank.addTexture("purple", TextureLoader.load("purple", renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
            file = path;
            file += "my font brown.png";
            TextureBank.addTexture("brown", TextureLoader.load("brown", renderer, ColourBank.getColour(XENOCOLOURS.MAGENTA), 800, 128));
        }
        /// <summary>
        /// Draws a coloured string at specified location
        /// *** LoadFontColours must be called before use and font graphics must
        /// use name of "my font 'colour'" ***
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="text">String to render</param>
        /// <param name="x">X position to render at</param>
        /// <param name="y">Y position to render at</param>
        /// <param name="colour">Colour of font to use</param>
        /// <param name="scaler">Scaling value</param>
        public static void drawColourString(IntPtr renderer, string text, float x, float y, string colour, float scaler = 1)
        {
            //set colour of text
            sourceName = colour;
            source = TextureBank.getTexture(colour);

            DrawString(renderer, text, x, y, scaler);

            //reset colour of text to white
            sourceName = "white";
            source = TextureBank.getTexture("white");
        }
        /// <summary>
        /// Draws a coloured string at specified location
        /// *** LoadFontColours must be called before use and font graphics must
        /// use name of "my font 'colour'" ***
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="text">String to render</param>
        /// <param name="x">X position to render at</param>
        /// <param name="y">Y position to render at</param>
        /// <param name="colour">Colour of font to use</param>
        /// <param name="width">Max width of render space</param>
        /// <param name="scaler">Scaling value</param>
        public static void drawColourString(IntPtr renderer, string text, float x, float y, string colour, int width, float scaler = 1)
        {
            //set colour of text
            sourceName = colour;
            source = TextureBank.getTexture(colour);

            DrawString(renderer, text, x, y, width, scaler);

            //reset colour of text to white
            sourceName = "white";
            source = TextureBank.getTexture("white");
        }
        /// <summary>
        /// Returns the position horizontally along a string 
        /// of characters of a specified character
        /// </summary>
        /// <param name="str">String to examine</param>
        /// <param name="index">Index of character to examine</param>
        /// <param name="scaler">String scaling value</param>
        /// <returns>Float value</returns>
        public static float getCharPosition(string str, int index, 
            float scaler = 1.0f)
        {
            float pos = 0;
            float charWidth = 0;
            if(index > str.Length - 1)
            {
                index = str.Length - 1;
            }
            if (str.Length > 1)
            {
                for (int i = 1; i < index; i++)
                {
                    charWidth = GetCharWidth(str[i]);
                    pos += (charWidth * scaler);
                }
            }
            return pos;
        }
        /// <summary>
        /// Returns the index of character at point provided (mp) else returns -1 if point not
        /// at any character in rendered string
        /// </summary>
        /// <param name="str">String that is being rendered</param>
        /// <param name="posx">X position of rendered string</param>
        /// <param name="posy">Y position of rendered string</param>
        /// <param name="mp">Point to check against rendered string</param>
        /// <param name="scaler">Scaler value for string to be rendered</param>
        /// <returns>Index as integer value</returns>
        public static int getCharIndex(string str, int posx, int posy, Point2D mp, float scaler = 1.0f)
        {
            int charSize = 0;
            int charPos = 0;
            for (int i = 0; i < str.Length; i++)
            {
                srcPos = GetCharPos(str[i]);
                srcRect.X = srcPos.X;
                srcRect.Y = srcPos.Y;
                charPos += (int)(charSize * scaler);
                charSize = GetCharWidth(str[i]);
                destRect.Height = 32 * scaler;
                if(destRect.pointInRect(mp) == true)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// Calculates the render width of a sub string provided a start and end index position and scaler value
        /// returns 0 if end is less than start or start is less than zero
        /// </summary>
        /// <param name="str">String to process</param>
        /// <param name="start">Index start</param>
        /// <param name="end">Index end</param>
        /// <param name="scaler">Scaler value</param>
        /// <returns>Length as integer value</returns>
        public static int getSubStringWidth(string str, int start, int end, float scaler = 1.0f)
        {
            if(start > end)
            {
                return 0;
            }
            if(start < 0)
            {
                return 0;
            }
            int charSize = 0;
            int charPos = 0;
            int len = 0;
            for (int i = 0; i < str.Length; i++)
            {
                srcPos = GetCharPos(str[i]);
                srcRect.X = srcPos.X;
                srcRect.Y = srcPos.Y;
                charPos += (int)(charSize * scaler);
                charSize = GetCharWidth(str[i]);
                destRect.Height = 32 * scaler;
                if (i >= start && i <= end)
                {
                    len += (int)destRect.Width;
                }
                if(i > end)
                {
                    return len;
                }
            }
            return 0;
        }
    }
    /// <summary>
    /// Stores and renders a text message at a position for a set number of ticks
    /// </summary>
    public class SimpleTextObject
    {
        //protected
        protected string message;
        protected string colour;
        protected Point2D center;
        protected int width;
        protected int ticks;
        protected int maxTicks;
        protected float scaler;

        //public
        /// <summary>
        /// SimpleTextObject constructor
        /// </summary>
        /// <param name="x">X position of center</param>
        /// <param name="y">Y position of center</param>
        /// <param name="colour">Colour of font</param>
        /// <param name="maxTicks">Max number of ticks</param>
        /// <param name="message">Message to display</param>
        /// <param name="scaler">Scaler value</param>
        public SimpleTextObject(float x, float y, string colour, int maxTicks, string message, float scaler = 1)
        {
            center = new Point2D(x, y);
            this.width = SimpleFont.stringRenderWidth(message, scaler);
            this.message = message;
            this.colour = colour;
            this.maxTicks = maxTicks;
            this.scaler = scaler;
            ticks = 0;
        }
        /// <summary>
        /// Draws the SimpleTextObject
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window X offset</param>
        /// <param name="winy">Window Y offset</param>
        /// <returns>True if finished else false</returns>
        public bool draw(IntPtr renderer, float winx = 0, float winy = 0)
        {
            SimpleFont.drawColourString(renderer, message, center.X - (width / 2) - winx, center.Y - winy, colour, scaler);
            ticks++;
            if(ticks >= maxTicks)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Ticks up count but does not render
        /// </summary>
        /// <returns></returns>
        public bool tickUp()
        {
            ticks++;
            if (ticks >= maxTicks)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Center property
        /// </summary>
        public Point2D Center
        {
            get { return center; }
        }
    }
    /// <summary>
    /// Provides a means to add text objects anywhere on screen or off screen 
    /// for a specified number of frames 
    /// </summary>
    public static class TextObjects
    {
        //private
        static List<SimpleTextObject> textObjects;
        static int numTicks;

        //public
        /// <summary>
        /// TextObjects constructor
        /// </summary>
        static TextObjects()
        {
            textObjects = new List<SimpleTextObject>();
            numTicks = 180;
        }
        /// <summary>
        /// Adds a text object to render
        /// </summary>
        /// <param name="text">Text to render</param>
        /// <param name="colour">Colour of font</param>
        /// <param name="x">Center X position</param>
        /// <param name="y">Center Y position</param>
        /// <param name="scaler">Scaler value</param>
        public static void addTextObject(string text, string colour, float x, float y, float scaler)
        {
            textObjects.Add(new SimpleTextObject(x, y, colour, numTicks, text, scaler));
        }
        /// <summary>
        /// Renders or updates all SimpleTextObjects stored, removing finished ones
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="wx">Window position in pixels</param>
        /// <param name="wy">Window position in pixels</param>
        /// <param name="ww">Window width in pixels </param>
        /// <param name="wh">Window height in pixels</param>
        public static void drawTextObjects(IntPtr renderer, int wx, int wy, int ww, int wh)
        {
            for(int i = 0; i < textObjects.Count; i++)
            {
                if(textObjects[i].Center.X >= wx && textObjects[i].Center.X <= wx + ww)
                {
                    if (textObjects[i].Center.Y >= wy && textObjects[i].Center.Y <= wy + wh)
                    {
                        if(textObjects[i].draw(renderer, wx, wy) == true)
                        {
                            textObjects.RemoveAt(i);
                        }
                    }
                    else
                    {
                        if(textObjects[i].tickUp() == true)
                        {
                            textObjects.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    if (textObjects[i].tickUp() == true)
                    {
                        textObjects.RemoveAt(i);
                    }
                }
            }
        }
    }
}
