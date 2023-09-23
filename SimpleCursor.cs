//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// CursorState enumeration
    /// </summary>
    public enum CursorState { good = 0, bad, nutral }
    /// <summary>
    /// MouseButtonState enumeration
    /// </summary>
    public enum MBS { left = 0, middle, right, none }

    /// <summary>
    /// SimpleCursor class
    /// </summary>
    public class SimpleCursor
    {
        //protected
        protected Counter delay;
        protected CoolDown delay2;
        protected Texture2D source;
        protected Rectangle sourceBox;
        protected Rectangle destBox;
        protected Rectangle box;
        protected CursorState state;
        protected MBS mbs;
        protected int sector;

        //public
        /// <summary>
        /// SimpleCursor constructor
        /// </summary>
        /// <param name="source">Texture2D referecne</param>
        /// <param name="delay">Input delay value</param>
        public SimpleCursor(Texture2D source, int delay)
        {
            this.source = source;
            this.delay = new Counter(delay);
            delay2 = new CoolDown(1);
            sourceBox = new Rectangle(0, 0, source.width / 3, source.height);
            destBox = new Rectangle(0, 0, source.width / 3, source.height);
            box = new Rectangle(0, 0, 8, 8);
            state = CursorState.good;
        }
        /// <summary>
        /// Draws SimpleCursor
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        public void draw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            sourceBox.X = sourceBox.Width * (int)state;
            destBox.X = box.X - ((destBox.Width / 2) - 4) - winx;
            destBox.Y = box.Y - winy;
            SimpleDraw.draw(renderer, source, sourceBox, destBox);
            //sb.Draw(source, destBox, sourceBox, Color.White);
        }
        /// <summary>
        /// Updates internal state (no input delay)
        /// </summary>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        /// <returns>MBS enumeration</returns>
        public MBS update(int winx = 0, int winy = 0)
        {
            box.X = MouseHandler.getMouseX() + ((destBox.Width / 2) - 4) + winx;
            box.Y = MouseHandler.getMouseY() + winy;
            destBox.X = MouseHandler.getMouseX();
            destBox.Y = MouseHandler.getMouseY();
            delay2.update();
            
            if (MouseHandler.getLeft())
            {
                mbs = MBS.left;
                //return MBS.left;
            }
            else if (MouseHandler.getMiddle())
            {
                mbs = MBS.middle;
                //return MBS.middle;
            }
            else if (MouseHandler.getRight())
            {
                mbs = MBS.right;
                //return MBS.right;
            }
            mbs = MBS.none;
            return mbs;
        }
        /// <summary>
        /// Box property
        /// </summary>
        public Rectangle Box
        {
            get { return box; }
        }
        /// <summary>
        /// DestBox property
        /// </summary>
        public Rectangle DestBox
        {
            get { return destBox; }
        }
        /// <summary>
        /// Gets Mouse Button State (no input delay)
        /// </summary>
        /// <returns>MBS enumeration</returns>
        public MBS getMBS()
        {
            if (MouseHandler.getLeft())
            {
                mbs = MBS.left;
                //return MBS.left;
            }
            else if (MouseHandler.getMiddle())
            {
                mbs = MBS.middle;
                //return MBS.middle;
            }
            else if (MouseHandler.getRight())
            {
                mbs = MBS.right;
                //return MBS.right;
            }
            return mbs;
        }
        /// <summary>
        /// Gets Mouse Button State (has input delay)
        /// </summary>
        /// <returns>MBS enumeration</returns>
        public MBS getMBS2()
        {
            if (!delay2.Active)
            {
                if (MouseHandler.getLeft())
                {
                    mbs = MBS.left;
                    //return MBS.left;
                }
                else if (MouseHandler.getMiddle())
                {
                    mbs = MBS.middle;
                    //return MBS.middle;
                }
                else if (MouseHandler.getRight())
                {
                    mbs = MBS.right;
                    //return MBS.right;
                }
                delay2.activate();
            }
            return mbs;
        }
        /// <summary>
        /// X property
        /// </summary>
        public float X
        {
            get { return box.X; }
            set { box.X = value; }
        }
        /// <summary>
        /// Y property
        /// </summary>
        public float Y
        {
            get { return box.Y; }
            set { box.Y = value; }
        }
        /// <summary>
        /// Gets current center of SimpleCursor
        /// </summary>
        public Point2D Center
        {
            get { return new Point2D(box.X + box.Width / 2, box.Y + box.Height / 2); }
        }
        /// <summary>
        /// Gets top left corner of cursor box
        /// </summary>
        public Point2D Tip
        {
            get { return new Point2D(box.X, box.Y); }
        }
        /// <summary>
        /// Sector property
        /// </summary>
        public int Sector
        {
            get { return sector; }
            set { sector = value; }
        }
        /// <summary>
        /// CursorState property
        /// </summary>
        public CursorState State
        {
            get { return state; }
            set { state = value; }
        }
        /// <summary>
        /// Left button state
        /// </summary>
        public bool Left
        {
            get
            {
                if (mbs == MBS.left)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Middle button state
        /// </summary>
        public bool Middle
        {
            get
            {
                if (mbs == MBS.middle)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Right button state
        /// </summary>
        public bool Right
        {
            get
            {
                if (mbs == MBS.right)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Delay property
        /// </summary>
        public CoolDown Delay2
        {
            get { return delay2; }
        }
    }
}
