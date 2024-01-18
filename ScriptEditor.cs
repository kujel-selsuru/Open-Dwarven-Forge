//====================================================
//Written by Kujel Selsuru
//Last Updated 07/01/24
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenoLib
{
    /// <summary>
    /// Handles script editing 
    /// ScriptPath needs to be set externally using property
    /// </summary>
    public class ScriptEditor
    {
        //protected 
        protected string[] lines;
        protected string segment;
        protected int lineRange;
        protected int lineIndex;
        protected int strX;
        protected int strY;
        protected int subStrX;
        protected int subEndX;
        protected int leftIndex;
        protected Rectangle cursorRect;
        protected Rectangle highlightedLine;
        protected SimpleStringBuilder ssb;
        protected Rectangle textWin;
        protected Rectangle nameWin;
        protected Rectangle leftStripe;
        protected Rectangle rightStripe;
        protected SimpleButton4 upButton;
        protected SimpleButton4 downButton;
        protected SimpleButton4 leftButton;
        protected SimpleButton4 rightButton;
        protected SimpleButton4 saveScript;
        protected SimpleButton4 loadScript;
        protected SimpleButton4 newScript;
        protected bool highLighted;
        protected string scriptsPath;
        protected string scriptName;
        protected string mode;
        protected TextFileExplorer fe;
        protected int subStart;
        protected int subEnd;
        protected CoolDown inputDelay;
        protected CoolDown inputDelay2;
        //public
        /// <summary>
        /// ScriptEditor constructor
        /// </summary>
        /// <param name="x">X position of text win</param>
        /// <param name="y">Y position of text win</param>
        /// <param name="w">Width of text win</param>
        /// <param name="h">Height of text win</param>
        /// <param name="upButton32">Up button graphic name</param>
        /// <param name="downButton32">Down button graphic name</param>
        /// <param name="leftButton32">Left button graphic name</param>
        /// <param name="rightButton32">Right button graphic name</param>
        /// <param name="sveScr">Save button graphic name</param>
        /// <param name="lodScr">Load button graphic name</param>
        /// <param name="newScr">New button graphic name</param>
        /// <param name="lineRange">Range of visible lines</param>
        /// <param name="inputDelay">Input delay value</param>
        public ScriptEditor(int x, int y, int w, int h, string upButton32 = "up button 32", string downButton32 = "down button 32", 
            string leftButton32 = "left button 32", string rightButton32 = "right button 32", string sveScr = "save script",
            string lodScr = "load script", string newScr = "new script", int lineRange = 15, int inputDelay = 120)
        {
            lines = new string[1];
            lines[0] = "";
            segment = "";
            this.lineRange = lineRange;
            lineIndex = 0;//First line of current code section
            strX = 0;//X position of cursor
            strY = 0;//Y position of cursor
            subStrX = 0;//Start of sub string 
            subEndX = 0;//End of sub string
            leftIndex = 0;
            cursorRect = new Rectangle(0, 0, 16, 32);
            highlightedLine = new Rectangle(0, 0, 32, 32);
            ssb = new SimpleStringBuilder(0, 0, inputDelay);
            textWin = new Rectangle(x, y, w, h);
            nameWin = new Rectangle(x, y - 32, w, 32);
            leftStripe = new Rectangle(x - 32, y, 32, h);
            rightStripe = new Rectangle(x + w, y, 64, h);
            upButton = new SimpleButton4(TextureBank.getTexture(upButton32), TextureBank.getTexture(upButton32), 
                w + 32, y, "up", inputDelay);
            upButton.setSize(32, 32);
            downButton = new SimpleButton4(TextureBank.getTexture(downButton32), TextureBank.getTexture(downButton32),
                w + 32, y + h - 32, "down", inputDelay);
            downButton.setSize(32, 32);
            leftButton = new SimpleButton4(TextureBank.getTexture(leftButton32), TextureBank.getTexture(leftButton32),
                x, y + h, "left", inputDelay);
            leftButton.setSize(32, 32);
            rightButton = new SimpleButton4(TextureBank.getTexture(rightButton32), TextureBank.getTexture(rightButton32),
                x + w - 32, y + h, "right", inputDelay);
            rightButton.setSize(32, 32);
            saveScript = new SimpleButton4(TextureBank.getTexture(sveScr), TextureBank.getTexture(sveScr),
                x, y + h + 32, "Save Script", inputDelay);
            saveScript.setSize(32, 32);
            loadScript = new SimpleButton4(TextureBank.getTexture(lodScr), TextureBank.getTexture(lodScr),
                x + 32, y + h + 32, "Load Script", inputDelay);
            loadScript.setSize(32, 32);
            newScript = new SimpleButton4(TextureBank.getTexture(newScr), TextureBank.getTexture(newScr),
                x + 64, y + h + 32, "New Script", inputDelay);
            newScript.setSize(32, 32);
            highLighted = false;
            scriptsPath = "";
            scriptName = "Untitled Script";
            mode = "edit";
            fe = new TextFileExplorer(x, y, w, h, TextureBank.getTexture(downButton32), TextureBank.getTexture(downButton32),
                TextureBank.getTexture(upButton32), TextureBank.getTexture(upButton32), TextureBank.getTexture(leftButton32), 
                TextureBank.getTexture(leftButton32), TextureBank.getTexture(downButton32), 15);
            this.inputDelay = new CoolDown(inputDelay);
            this.inputDelay2 = new CoolDown(inputDelay * 2);
        }
        /// <summary>
        /// Update ScriptEditor internal state
        /// </summary>
        /// <param name="cursor">SimpleCursor reference</param>
        public virtual void update(SimpleCursor cursor)
        {
            string choice = "";
            int lineRng = lineIndex + lineRange;
            if(lineRng > lines.Length - 1)
            {
                lineRng = lines.Length;
            }
            switch(mode)
            {
                case "edit":
                    if(inputDelay2.Active == false)
                    {
                        handleTextInput();
                    }
                    handleButtons();
                    handleKBCmds();
                    break;
                case "save":
                    saveScriptFile();
                    mode = "edit";
                    break;
                case "load":
                    choice = fe.update(cursor);
                    if(choice != " " && choice != "")
                    {
                        scriptName = choice;
                        loadScriptFile();
                        mode = "edit";
                    }
                    break;
                case "new":
                    newScriptFile();
                    mode = "edit";
                    break;
                case "name":
                    ssb.Sequence = scriptName;
                    ssb.update();
                    scriptName = ssb.Sequence;
                    break;
            }
            if(mode == "edit" || mode == "name")
            {
                if(MouseHandler.getLeft() == true)
                {
                    if (inputDelay.Active == false)
                    {
                        if (nameWin.pointInRect(MouseHandler.getTip()) == true)
                        {
                            mode = "name";
                        }
                        if (textWin.pointInRect(MouseHandler.getTip()) == true)
                        {
                            mode = "edit";
                            for(int i = lineIndex; i < lineRng; i++)
                            {
                                int tmp = SimpleFont.charIndex(lines[i], textWin.IX, textWin.IY + (i * 32) - (lineIndex * 32), 
                                    MouseHandler.getTip(), 0.75f);
                                if(tmp != -1)
                                {
                                    strX = tmp;
                                    strY = i;
                                    break;
                                }
                            }
                        }
                        inputDelay.activate();
                    }
                }
            }
            inputDelay.update();
            inputDelay2.update();
            saveScript.update();
            saveScript.updateToolTip(MouseHandler.getMouseX(), MouseHandler.getMouseY());
            loadScript.update();
            loadScript.updateToolTip(MouseHandler.getMouseX(), MouseHandler.getMouseY());
            newScript.update();
            newScript.updateToolTip(MouseHandler.getMouseX(), MouseHandler.getMouseY());
            upButton.update();
            downButton.update();
            leftButton.update();
            rightButton.update();
        }
        /// <summary>
        /// Draws ScriptEditor
        /// </summary>
        /// <param name="renderer">IntPtr reference</param>
        /// <param name="cursor">SimpleCursor reference</param>
        /// <param name="scaler">Scaler value</param>
        public virtual void draw(IntPtr renderer, SimpleCursor cursor, float scaler = 0.75f)
        {
            subStart = 0;
            subEnd = 0;
            switch (mode)
            {
                case "edit":
                    SimpleFont.DrawString(renderer, scriptName, nameWin.X, nameWin.Y, 0.75f);
                    DrawRects.drawRect(renderer, nameWin, ColourBank.getColour(XENOCOLOURS.WHITE));
                    if(highLighted == true)
                    {
                        //add highlighting here
                        if(subStrX >= 0 && subEndX <= lines[strY].Length)
                        {
                            string sub = lines[strY].Substring(subStrX, subEndX - subStrX);
                            highlightedLine.Width = SimpleFont.stringRenderWidth(sub, scaler);
                            highlightedLine.X = textWin.X + SimpleFont.charRenderX(lines[strY], subStrX, 0.75f);
                            highlightedLine.Y = textWin.Y + (strY * 32);
                        }
                    }
                    else
                    {
                        cursorRect.X = textWin.X + SimpleFont.charRenderX(lines[strY], strX, scaler) - leftIndex;
                        cursorRect.Y = textWin.Y + ((strY * 32) - (lineIndex * 32));
                        DrawRects.drawRect(renderer, cursorRect, ColourBank.getColour(XENOCOLOURS.BLUE));
                    }
                    DrawRects.drawRect(renderer, rightStripe, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                    if(highLighted == true)
                    {
                        DrawRects.drawRect(renderer, highlightedLine, ColourBank.getColour(XENOCOLOURS.BLUE), true);
                    }
                    drawLines(renderer, scaler);
                    DrawRects.drawRect(renderer, leftStripe, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                    for(int i = lineIndex; i < lineRange; i++)
                    {
                        SimpleFont.DrawString(renderer, (i + 1).ToString(), textWin.X - 32, textWin.Y + (((i - lineIndex)) * 32) + 12, 0.5f);
                    }
                    DrawRects.drawRect(renderer, textWin, ColourBank.getColour(XENOCOLOURS.WHITE));
                    upButton.draw(renderer);
                    downButton.draw(renderer);
                    rightButton.draw(renderer);
                    leftButton.draw(renderer);
                    break;
                case "load":
                    fe.draw2(renderer, cursor);
                    break;
                case "name":
                    SimpleFont.DrawString(renderer, scriptName, nameWin.X, nameWin.Y, 0.75f);
                    DrawRects.drawRect(renderer, nameWin, ColourBank.getColour(XENOCOLOURS.BLUE));
                    drawLines(renderer, scaler);
                    DrawRects.drawRect(renderer, leftStripe, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                    for(int i = lineIndex; i < lineRange; i++)
                    {
                        SimpleFont.DrawString(renderer, (i + 1).ToString(), textWin.X - 32, textWin.Y + (((i - lineIndex)) * 32) + 6, 0.5f);
                    }
                    DrawRects.drawRect(renderer, textWin, ColourBank.getColour(XENOCOLOURS.WHITE));
                    upButton.draw(renderer);
                    downButton.draw(renderer);
                    DrawRects.drawRect(renderer, rightStripe, ColourBank.getColour(XENOCOLOURS.BLACK), true);
                    rightButton.draw(renderer);
                    leftButton.draw(renderer);
                    break;
            }
            saveScript.draw(renderer);
            loadScript.draw(renderer);
            newScript.draw(renderer);
            saveScript.drawToolTip(renderer, MouseHandler.getMouseX(), MouseHandler.getMouseY());
            loadScript.drawToolTip(renderer, MouseHandler.getMouseX(), MouseHandler.getMouseY());
            newScript.drawToolTip(renderer, MouseHandler.getMouseX(), MouseHandler.getMouseY());
        }
        /// <summary>
        /// Draws a highlight rectangle for a specified sub string (deprecated)
        /// </summary>
        /// <param name="renderer">IntPtr referecne</param>
        /// <param name="line">Line index</param>
        /// <param name="subStart">Start of sub string</param>
        /// <param name="subEnd">End of sub string</param>
        public void drawHighlight(IntPtr renderer, int line, int subStart, int subEnd, float scaler = 0.75f)
        {
            string subStr = lines[line].Substring(subStart, subEnd);
            cursorRect.Width = SimpleFont.stringRenderWidth(subStr, scaler);
            DrawRects.drawRect(renderer, cursorRect, ColourBank.getColour(XENOCOLOURS.LIHGT_CYAN), true);
            cursorRect.Width = 32;
        }
        /// <summary>
        /// Draws lines of code in text win
        /// </summary>
        /// <param name="renderer">IntPtr renderer</param>
        /// <param name="scaler">Scaler value</param>
        public void drawLines(IntPtr renderer, float scaler = 0.75f)
        {
            int rng = lineIndex + lineRange;
            if(lines.Length > 1)
            {
                if(lines.Length < rng)
                {
                    rng = lines.Length;
                }
                for(int i = lineIndex; i < rng; i++)
                {
                    if(lines[i].Length > 1)
                    {
                        subStart = SimpleFont.SubStrCharIndex(renderer, lines[i], textWin.IX + 1,
                            textWin.IY + ((i * 32) - (lineIndex * 32)), cursorRect.IX + 1, cursorRect.IY + 1, scaler);
                        subEnd = SimpleFont.SubStrCharIndex(renderer, lines[i], textWin.IX + 1,
                            textWin.IY + ((i * 32) - (lineIndex * 32)), textWin.IX - 1, cursorRect.IY + 1, scaler);
                    }
                    SimpleFont.DrawString(renderer, lines[i], textWin.IX - leftIndex, textWin.IY + ((i * 32) - (lineIndex * 32)), scaler);
                }
            }
            else
            {
                SimpleFont.DrawString(renderer, lines[0], textWin.IX - leftIndex, textWin.IY, scaler);
            }
        }
        /// <summary>
        /// Handles editor buttons
        /// </summary>
        public void handleButtons()
        {
            if(upButton.clicked2() == true)
            {
                if(lineIndex > 0)
                {
                    lineIndex--;
                }
            }
            if(downButton.clicked2() == true)
            {
                if(lineIndex + lineRange < lines.Length - 1)
                {
                    lineIndex++;
                }
            }
            if(rightButton.clicked2() == true)
            {
                if(leftIndex + textWin.Width < 512 * 32)
                {
                    leftIndex += 4;
                }
            }
            if(leftButton.clicked2() == true)
            {
                if(leftIndex > 0)
                {
                    leftIndex -= 4;
                }
            }
        }
        /// <summary>
        /// Handles keyboard commands but not string input
        /// </summary>
        public void handleKBCmds()
        {
            if((KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_LCTRL) == true || 
                KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_RCTRL)) == true &&
                KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_x) == true)
            {
                if(inputDelay2.Active == false)
                {
                    inputDelay2.activate();
                    cutStr();
                }
            }
            if((KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_LCTRL) == true ||
                KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_RCTRL)) == true &&
                KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_c) == true)
            {
                if(inputDelay2.Active == false)
                {
                    inputDelay2.activate();
                    copyStr();
                }
            }
            if((KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_LCTRL) == true ||
                KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_RCTRL)) == true &&
                KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_v) == true)
            {
                if(inputDelay2.Active == false)
                {
                    inputDelay2.activate();
                    pasteStr();
                }
            }
            if((KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_LSHIFT) == true ||
                KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_RSHIFT)) == true &&
                KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_LEFT) == true)
            {
                if(inputDelay.Active == false)
                {
                    inputDelay.activate();
                    if(highLighted == false)
                    {
                        subStrX = strX;
                        subEndX = strX;
                        if(subEndX > lines[strY].Length - 1)
                        {
                            subEndX = lines[strY].Length - 1;
                        }
                    }
                    if(subStrX > 0)
                    {
                        subStrX--;
                    }
                    highLighted = true;
                }
            }
            if((KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_LSHIFT) == true ||
                KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_RSHIFT)) == true &&
                KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_RIGHT) == true)
            {
                if(inputDelay.Active == false)
                {
                    inputDelay.activate();
                    if(highLighted == false)
                    {
                        subStrX = strX;
                        subEndX = strX;
                        if(subEndX > lines[strY].Length)
                        {
                            subEndX = lines[strY].Length;
                        }
                    }
                    if (subEndX <= lines[lineIndex].Length)
                    {
                        subEndX++;
                    }
                    highLighted = true;
                }
            }
            /*
            if((KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_LSHIFT) == true ||
                KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_RSHIFT)) == true &&
                KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_UP) == true)
            {

            }
            if((KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_LSHIFT) == true ||
                KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_RSHIFT)) == true &&
                KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_DOWN) == true)
            {

            }
            */
            if((KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_LEFT)) == true)
            {
                if(inputDelay.Active == false)
                {
                    inputDelay.activate();
                    if (strX > 0)
                    {
                        strX--;
                    }
                    subStrX = strX;
                    subEndX = strX;
                    highLighted = false;
                }
            }
            if((KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_RIGHT)) == true)
            {
                if(inputDelay.Active == false)
                {
                    inputDelay.activate();
                    if(strX < lines[lineIndex].Length)
                    {
                        strX++;
                    }
                    subStrX = strX;
                    subEndX = strX;
                    highLighted = false;
                }
            }
            if((KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_UP)) == true)
            {
                if(inputDelay.Active == false)
                {
                    inputDelay.activate();
                    if(strY > 0)
                    {
                        strY--;
                    }
                    if(lineIndex < strY)
                    {
                        lineIndex--;
                        if(lineIndex < 0)
                        {
                            lineIndex = 0;
                        }
                    }
                    if(strX > lines[strY].Length - 1)
                    {
                        strX = lines[strY].Length - 1;
                    }
                    subStrX = strX;
                    highLighted = false;
                }
            }
            if((KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_DOWN)) == true)
            {
                if(inputDelay.Active == false)
                {
                    inputDelay.activate();
                    if(strY < lines.Length - 1)
                    {
                        strY++;
                    }
                    if(strY > lineIndex + lineRange)
                    {
                        lineIndex++;
                        if(lineIndex > lines.Length - 1)
                        {
                            lineIndex = lines.Length - 1;
                        }
                    }
                    if(strX > lines[strY].Length - 1)
                    {
                        strX = lines[strY].Length - 1;
                    }
                    subStrX = strX;
                    highLighted = false;
                }
            }
            if((KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_BACKSPACE)) == true)
            {
                if(inputDelay.Active == false)
                {
                    if(lines[strY].Length > 1)
                    {
                        if (strX != lines[strY].Length)
                        {
                            string tmp1 = lines[strY].Substring(0, strX - 1);
                            string tmp2 = lines[strY].Substring(strX + 1);//, lines[strY].Length - 1);
                            string tmp3 = tmp1 + tmp2;
                            lines[strY] = tmp3;
                            strX--;
                        }
                        else
                        {
                            string tmp = lines[strY].Substring(0, lines[strY].Length - 1);
                            lines[strY] = tmp;
                            strX = lines[strY].Length;
                        }
                    }
                    else if(lines[strY].Length == 1)
                    {
                        lines[strY] = "";
                    }
                    else if(lines[strY].Length == 0)
                    {
                        shrinkLines();
                        if(strY > 0)
                        {
                            strY--;
                        }
                        highLighted = false;
                    }
                    inputDelay.activate();
                }
            }
        }
        /// <summary>
        /// Collects text input from user to add to lines of code
        /// </summary>
        public void handleTextInput()
        {
            if(inputDelay.Active == false)
            {
                if(KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_RCTRL) == false &&
                    KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_LCTRL) == false)
                {
                    if((KeyboardHandler.getKeyState(SDL2.SDL.SDL_Keycode.SDLK_RETURN)) == true)
                    {
                        inputDelay.activate();
                        expandLines();
                        strY++;
                        strX = 0;
                        highLighted = false;
                    }
                    else
                    {
                        ssb.update();
                        if(ssb.Sequence != "")
                        {
                            if(strX != lines[strY].Length)
                            {
                                string tmp1 = lines[strY].Substring(0, strX);
                                string tmp2 = lines[strY].Substring(strX);
                                string tmp3 = tmp1 + ssb.Sequence + tmp2;
                                lines[strY] = tmp3;
                                ssb.Sequence = "";
                                strX++;
                            }
                            else
                            {
                                lines[strY] += ssb.Sequence;
                                ssb.Sequence = "";
                                strX++;
                            }
                            inputDelay.activate();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Cuts a substring and saves to segment
        /// </summary>
        public void cutStr()
        {
            int length = subEndX - subStrX;
            segment = lines[strY].Substring(subStrX, length);
            int segEnd = (lines[strY].Length) - subEndX;
            strX = subStrX;
            if(subStrX > 0)
            {
                lines[strY] = lines[strY].Substring(0, subStrX) +
                        lines[strY].Substring(subEndX, segEnd);
            }
            else
            {
                lines[strY] = lines[strY].Substring(0, subEndX);
            }
            highLighted = false;
        }
        /// <summary>
        /// Copies a sub string to segment
        /// </summary>
        public void copyStr()
        {
            int length = subEndX - subStrX;
            segment = lines[strY].Substring(subStrX, length);
            highLighted = false;
        }
        /// <summary>
        /// Pastes a sub string from segment
        /// </summary>
        public void pasteStr()
        {
            int length = lines[strY].Length - strX;
            string pt1 = lines[strY].Substring(0, strX);
            string pt3 = lines[strY].Substring(strX, length);
            lines[strY] = pt1 + segment + pt3;
            highLighted = false;
        }
        /// <summary>
        /// Increases the total number of lines by one each call
        /// </summary>
        public void expandLines()
        {
            string[] newLines = new string[lines.Length + 1];
            for(int i = 0; i < lines.Length; i++)
            {
                newLines[i] = lines[i];
            }
            lines = newLines;
            lines[lines.Length - 1] = "";
        }
        /// <summary>
        /// Shrinks the total number of lines by one each call
        /// </summary>
        public void shrinkLines()
        {
            if(lines.Length > 1)
            {
                string[] newLines = new string[lines.Length - 1];
                for(int i = 0; i < lines.Length - 1; i++)
                {
                    newLines[i] = lines[i];
                }
                lines = newLines;
            }
        }
        /// <summary>
        /// Saves a script to file
        /// </summary>
        public void saveScriptFile()
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(scriptsPath + scriptName + ".txt");
            //sw.WriteLine("======" + scriptName + "======");
            //sw.WriteLine(lines.Length);
            for(int i = 0; i < lines.Length - 1; i++)
            {
                sw.WriteLine(lines[i]);
            }
            sw.Close();
        }
        /// <summary>
        /// Loads a script from file
        /// </summary>
        public void loadScriptFile()
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(scriptsPath + scriptName + ".txt");
            /*
            sr.ReadLine();
            int num = Convert.ToInt32(sr.ReadLine());
            lines = new string[num];
            for(int i = 0; i < num; i++)
            {
                lines[i] = sr.ReadLine();
            }
            */
            lines = new string[1];
            int i = 0;
            while(sr.EndOfStream == false)
            {
                lines[i] = sr.ReadLine();
                expandLines();
                i++;
            }
            sr.Close();
        }
        /// <summary>
        /// Creates a new script
        /// </summary>
        public void newScriptFile()
        {
            lines = new string[1];
            scriptName = "Untitled Script";
        }
        /// <summary>
        /// ScriptsPath property
        /// </summary>
        public string ScriptsPath
        {
            get { return scriptsPath; }
            set { scriptsPath = value; }
        }
    }
}
