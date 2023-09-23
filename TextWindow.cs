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
    /// <summary>
    /// Handles and renders a text window in games
    /// </summary>
    public class TextWindow
    {
        //protected
        protected string[] lines;
        protected int lineIndex;
        protected string[] processingLines;
        protected string processingStr;
        protected int fragIndex;
        protected int numLines;
        protected string text;
        protected Rectangle background;
        protected Rectangle edging;

        //public
        /// <summary>
        /// TextWindow constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width of full window</param>
        /// <param name="h">Height of full window</param>
        /// <param name="gap">Gap between outer Window and text space</param>
        /// <param name="numLines">Number of total lines displayable</param>
        public TextWindow(float x, float y, float w, float h, float gap = 8, int numLines = 16)
        {
            setWinSize(x, y, w, h, gap, numLines);
            defaultInternals();
        }
        /// <summary>
        /// Sets's the TextWindow's text to match window
        /// </summary>
        /// <param name="nText">Text to display</param>
        /// <param name="scaler">Text scaling value</param>
        public void setText(string nText, float scaler)
        {
            char[] space = { ' ' };
            text = nText;
            processingStr = "";
            lineIndex = 0;
            processingLines = StringParser.parse(text, space);
            //After cutting into word fragments check each line to see
            //if it's within the bounds of the TextWindow
            while (fragIndex < processingLines.Length)
            {
                if (edging.X + SimpleFont.stringRenderWidth(processingStr +
                processingLines[fragIndex], scaler) < edging.X + edging.Width)
                {
                    //in the window, just add to line and move to next fragment
                    processingStr += processingLines[fragIndex];
                    fragIndex++;
                }
                else
                {
                    //save to current line, index by 1 and start the next line
                    lines[lineIndex] = processingStr;
                    lineIndex++;
                    processingStr = "";
                    processingStr += processingLines[fragIndex];
                    fragIndex++;
                }
            } 
        }
        /// <summary>
        /// Sets the window's render size and position
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width of full window</param>
        /// <param name="h">Height of full window</param>
        /// <param name="gap">Gap between outer Window and text space</param>
        /// <param name="numLines">Number of total lines displayable</param>
        public void setWinSize(float x, float y, float w, float h, float gap = 8, int numLines = 16)
        {
            background = new Rectangle(x, y, w, h);
            edging = new Rectangle(x + gap, y + gap, w - (2 * gap), h - (2 * gap));
            this.numLines = numLines;
            lines = new string[numLines];
        }
        /// <summary>
        /// Defaults internals to safe states
        /// </summary>
        public void defaultInternals()
        {
            processingStr = "";
            processingLines = null;
            fragIndex = 0;
            text = "";
        }
        /// <summary>
        /// Draws the set text colour text in a box of a colour user sets
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="backColour">Background colour</param>
        /// <param name="scaler">Scaler value</param>
        public void draw(IntPtr renderer, XENOCOLOURS backColour, float scaler = 1)
        {
            DrawRects.drawRect(renderer, background, ColourBank.getColour(backColour));
            for(int i = 0; i <= lineIndex; i++)
            {
                SimpleFont.DrawString(renderer, lines[i], edging.X, edging.Y, scaler);
            }
        }
        /// <summary>
        /// Draws coloured text using SimpleFont
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="backColour">Background colour</param>
        /// <param name="colour">Text colour</param>
        /// <param name="scaler">Scaler value</param>
        public void draw(IntPtr renderer, XENOCOLOURS backColour, string colour = "white", float scaler = 1)
        {
            DrawRects.drawRect(renderer, background, ColourBank.getColour(backColour));
            for (int i = 0; i <= lineIndex; i++)
            {
                SimpleFont.drawColourString(renderer, lines[i], edging.X, edging.Y, colour, scaler);
            }
        }
    }
}
