//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenoLib
{
    /// <summary>
    /// Base class for in game window, can be moved, resized and closed/opened, detect if mouse cursor in window space
    /// </summary>
    public class InGameWindow
    {
        //protected
        protected string name;
        protected Rectangle box;
        protected Rectangle strip;
        protected SimpleButton4 closeButton;
        protected bool close;
        protected bool drag;
        protected Point2D prevPos;

        //public
        /// <summary>
        /// InGameWindow constructor, strip is height of 32 pixels but width varies, 
        /// min width and hight of window is 320 pixels
        /// </summary>
        /// <param name="name">Name to display in window strip</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Window width</param>
        /// <param name="h">Window height</param>
        /// <param name="closeTex1">Close button pressed Texture2D reference</param>
        /// <param name="closeTex2">Close button depressed Texture2D reference</param>
        public InGameWindow(string name, int x, int y, int w, int h, string closeTex1 = "close button pressed", string closeTex2 = "close button depressed")
        {
            this.name = name;
            strip = new Rectangle(x, y, w, 32);
            box = new Rectangle(x, y + (int)strip.Height, w, h);
            closeButton = new SimpleButton4(TextureBank.getTexture(closeTex2), TextureBank.getTexture(closeTex1), x + w - 32, y, "close");
            close = false;
            drag = false;
            prevPos = new Point2D(x, y);
        }
        /// <summary>
        /// Base update method, handles dragging InGameWindow within main game win
        /// </summary>
        public virtual void update()
        {
            dragWin();
            if (drag == true)
            {
                if (prevPos.IX == strip.IX &&
                    prevPos.IY == strip.IY)
                {
                    prevPos.IX = MouseHandler.getMouseX();
                    prevPos.IY = MouseHandler.getMouseY();
                }
                else
                {
                    if (prevPos.IX > MouseHandler.getMouseX())//shifted left
                    {
                        strip.IX -= prevPos.IX - MouseHandler.getMouseX();
                    }
                    else//shifted right
                    {
                        strip.IX += MouseHandler.getMouseX() - prevPos.IX;
                    }
                    if (prevPos.IY > MouseHandler.getMouseY())//shifted up
                    {
                        strip.IY -= prevPos.IY - MouseHandler.getMouseY();
                    }
                    else//shifted down
                    {
                        strip.IY += MouseHandler.getMouseY() - prevPos.IY;
                    }
                    box.IX = strip.IX;
                    box.IY = strip.IY + (int)strip.Height;
                    closeButton.X = strip.X + strip.Width - 32;
                    closeButton.Y = strip.Y;
                }
            }
            if(closeButton.clicked() == true)
            {
                close = true;
            }
        }
        /// <summary>
        /// Base window rendering 
        /// </summary>
        /// <param name="renderer">IntPtr reference</param>
        /// <param name="stripColor">Background color of strip</param>
        /// <param name="winColor">Background color of window</param>
        /// <param name="textColor">Name of strip text rendering</param>
        public virtual void draw(IntPtr renderer, XENOCOLOURS stripColor, XENOCOLOURS winColor, string textColor = "White")
        {
            DrawRects.drawRect(renderer, strip, ColourBank.getColour(stripColor), true);
            DrawRects.drawRect(renderer, box, ColourBank.getColour(stripColor), true);
            SimpleFont.drawColourString(renderer, name, strip.X + 2, strip.Y + 2, textColor, 0.75f);
            closeButton.draw(renderer);
        }
        /// <summary>
        /// Resize window dimensions but only strip's width not height changes
        /// </summary>
        /// <param name="w">New width value</param>
        /// <param name="h">New height value</param>
        public void resize(int w, int h)
        {
            if(w < 320)
            {
                w = 320;
            }
            if(h < 320)
            {
                h = 320;
            }
            strip.Width = w;
            closeButton.X = strip.X + strip.Width - 32;
            closeButton.Y = strip.Y;
            box.Width = w;
            box.Height = h;
        }
        /// <summary>
        /// Sets InGameWindow top left corner position
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public void setPos(int x, int y)
        {
            strip.X = x;
            strip.Y = y;
            box.X = strip.X;
            box.Y = strip.Y + strip.Height;
            closeButton.X = strip.X + strip.Width - 32;
            closeButton.Y = strip.Y;
        }
        /// <summary>
        /// Activate or deactivate the drag state depending on whether or not the left mouse button is 
        /// pressed and the mouse pointer is in the window strip
        /// </summary>
        public void dragWin()
        {
            if(drag == false)
            {
                if(MouseHandler.getLeft() == true)
                {
                    if(InStrip == true)
                    {
                        drag = true;
                    }
                }
            }
            else
            {
                if(MouseHandler.getLeft() == false)
                {
                    drag = false;
                }
            }
        }
        /// <summary>
        /// InWindow property
        /// </summary>
        public bool InWindow
        {
            get { return box.pointInRect((float)MouseHandler.getMouseX(),
                (float)MouseHandler.getMouseX()); }
        }
        /// <summary>
        /// InStrip property
        /// </summary>
        public bool InStrip
        {
            get { return strip.pointInRect((float)MouseHandler.getMouseX(),
              (float)MouseHandler.getMouseX());
            }
        }
        /// <summary>
        /// Close property
        /// </summary>
        public bool Close
        {
            get { return close; }
            set { close = value; }
        }
        /// <summary>
        /// Box propertry
        /// </summary>
        public Rectangle Box
        {
            get {return box;}
        }
        /// <summary>
        /// Strip propertry
        /// </summary>
        public Rectangle Strip
        {
            get { return strip; }
        }
    }
}
