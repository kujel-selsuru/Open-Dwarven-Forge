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
    /// Basic UI component, good for buttons or panels
    /// </summary>
    public class SimpleUI
    {
        //protected
        protected Texture2D src1;
        protected string src1Name;
        protected Texture2D src2;
        protected string src2Name;
        protected string name;
        protected Rectangle srcRect;
        protected Rectangle destRect;
        protected CoolDown delay;

        //public
        /// <summary>
        /// SimpleUI constructor
        /// </summary>
        /// <param name="src1Name">Source 1 string value</param>
        /// <param name="src2Name">Source 2 string value</param>
        /// <param name="name">Name of object</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="delay">Delay time in frames</param>
        public SimpleUI(string src1Name, string src2Name, string name, int x, int y, int width, int height, int delay = 60)
        {
            this.src1 = TextureBank.getTexture(src1Name);
            this.src1Name = src1Name;
            this.src2 = TextureBank.getTexture(src2Name);
            this.src2Name = src2Name;
            srcRect = new Rectangle(0, 0, width, height);
            destRect = new Rectangle(x, y, width, height);
            this.delay = new CoolDown(delay);
        }
        /// <summary>
        /// SimpleUI copy constructor
        /// </summary>
        /// <param name="obj">SimpleUI reference</param>
        public SimpleUI(SimpleUI obj)
        {
            this.src1 = obj.src1;
            this.src1Name = obj.src1Name;
            this.src2 = obj.src2;
            this.src2Name = obj.src2Name;
            srcRect = new Rectangle(0, 0, obj.srcRect.Width, obj.srcRect.Height);
            srcRect = new Rectangle(obj.destRect.IX, obj.destRect.IY, obj.destRect.Width, obj.destRect.Height);
            delay = new CoolDown(obj.delay.MaxTicks);
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        public void update()
        {
            delay.update();
        }
        /// <summary>
        /// Draws a visually dynamic UI object
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            if(delay.Active == true)
            {
                SimpleDraw.draw(renderer, src1, srcRect, destRect);
            }
            else
            {
                SimpleDraw.draw(renderer, src2, srcRect, destRect);
            }
        }
        /// <summary>
        /// Draws a visually static UI object
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void drawStatic(IntPtr renderer)
        {
            SimpleDraw.draw(renderer, src1, srcRect, destRect);
        }
        /// <summary>
        /// Returns true if mouse pointer is in hit box else returns false
        /// </summary>
        /// <returns>Boolean</returns>
        public bool clicked()
        {
            if(delay.Active == false)
            {
                if (destRect.pointInRect(new Point2D(MouseHandler.getMouseX(), MouseHandler.getMouseY())) == true)
                {
                    delay.activate();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Sets object postion
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void setPos(int x, int y)
        {
            destRect.X = x;
            destRect.Y = y;
        }
        /// <summary>
        /// Active property
        /// </summary>
        public bool Active
        {
            get { return delay.Active; }
        }
        /// <summary>
        /// Name property
        /// </summary>
        public string Name
        {
            get { return name; }
        }
    }
}
