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
    /// Handles extending editor with a flexible menu for adding field manipulation of objects
    /// </summary>
    public class FieldEditor
    {
        //protected
        protected SimpleStringBuilder ssb;
        protected List<string> fieldNames;
        protected List<string> fieldValues;
        protected int curr;
        protected Rectangle box;
        protected int size;
        protected Texture2D background;
        protected Rectangle rect;

        //public
        /// <summary>
        /// Field editor constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width of menu</param>
        /// <param name="background">Background texture2d reference</param>
        /// <param name="size">Number of fields in menu</param>
        public FieldEditor(int x, int y, int w, Texture2D background, int size = 10)
        {
            fieldNames = new List<string>();
            fieldValues = new List<string>();
            this.size = size;
            curr = -1;
            box = new Rectangle(x, y, w, size * 32);
            this.background = background;
            rect.X = x;
            rect.Y = y;
            rect.Width = w;
            rect.Height = size * 32;
            ssb = new SimpleStringBuilder(5);
        }
        /// <summary>
        /// Add a new field and subsequent defualt value to menu
        /// </summary>
        /// <param name="name">Field name</param>
        /// <param name="value">Field defualt value</param>
        public void add(string name, string value = "")
        {
            bool found = false;
            for (int i = 0; i < fieldNames.Count; i++)
            {
                if (fieldNames[i] == name)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                fieldNames.Add(name);
                fieldValues.Add(value);
            }
            size = fieldNames.Count;
        }
        /// <summary>
        /// Get a field value at index i
        /// </summary>
        /// <param name="i">Index of field to access</param>
        /// <returns>Value as a string or ""</returns>
        public string getValue(int i)
        {
            if (i >= 0 && i < fieldValues.Count)
            {
                return fieldValues[i];
            }
            return "";
        }
        /// <summary>
        /// Get a field of name
        /// </summary>
        /// <param name="name">Name of field to access</param>
        /// <returns>Value as a string or ""</returns>
        public string getValue(string name)
        {
            for (int i = 0; i < fieldNames.Count; i++)
            {
                if (fieldNames[i] == name)
                {
                    return fieldValues[i];
                }
            }
            return "";
        }
        /// <summary>
        /// Tests if the left mouse button is depressed and sets the current field to selected index
        /// </summary>
        public void click()
        {
            if (MouseHandler.getLeft() == true)
            {
                setCurr(MouseHandler.getMouseX(), MouseHandler.getMouseY());
            }
        }
        /// <summary>
        /// Sets the current index of field to update
        /// </summary>
        /// <param name="msx">Mouse x position</param>
        /// <param name="msy">Mouse y position</param>
        public void setCurr(int msx, int msy)
        {
            if (box.pointInRect(new Point2D(msx, msy)))
            {
                curr = (msy - (int)box.Y) / 32;
            }
        }
        /// <summary>
        /// Updates the field value of the currrent index
        /// </summary>
        public void updateSSB()
        {
            if (fieldValues.Count > 0)
            {
                if (curr > -1)
                {
                    ssb.Sequence = fieldValues[curr];
                    ssb.update();
                    fieldValues[curr] = ssb.Sequence;
                }
            }
        }
        /// <summary>
        /// updates internal states
        /// </summary>
        public void update()
        {
            click();
            updateSSB();
        }
        /// <summary>
        /// Draws the field menus
        /// </summary>
        /// <param name="renderer">renderer refence</param>
        /// <param name="scaler">text scaler value</param>
        public void draw(IntPtr renderer, float scaler)
        {
            DrawRects.drawRect(renderer, rect, ColourBank.getColour(XENOCOLOURS.GRAY), true);
            for (int i = 0; i < fieldNames.Count; i++)
            {
                SimpleFont.DrawString(renderer, fieldNames[i], box.X, box.Y + (i * 32), scaler);
                SimpleFont.DrawString(renderer, fieldValues[i], box.X + (box.Width / 2), box.Y + (i * 32), scaler);
            }
        }
        /// <summary>
        /// Clears the current set of fields and values
        /// </summary>
        public void clear()
        {
            fieldNames.Clear();
            fieldValues.Clear();
        }
        /// <summary>
        /// Size property
        /// </summary>
        public int Size
        {
            get { return size; }
            //set { size = value; }
        }
        /// <summary>
        /// Box property
        /// </summary>
        public Rectangle Box
        {
            get { return box; }
        }
    }
}
