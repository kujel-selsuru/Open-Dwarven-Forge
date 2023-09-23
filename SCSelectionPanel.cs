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
using XenoLib;

namespace XenoLib
{
    public class SelectionPanel
    {
        //protected
        protected Rectangle box;
        protected List<SCRTSUnit> selected;
        protected List<Rectangle> boxes;
        protected int maxUnits;

        //public
        /// <summary>
        /// SelectionPanel constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public SelectionPanel(int x, int y)
        {
            box = new Rectangle(x, y, 416, 96);
            maxUnits = 0;
            selected = new List<SCRTSUnit>();
            boxes = new List<Rectangle>();
            for(int i = 0; i < 312; i++)
            {
                boxes[i] = new Rectangle(0, 0, 32, 32);
            }
        }
        /// <summary>
        /// Calculates the number of units drawn per row
        /// </summary>
        /// <param name="numUnits">Number of units total</param>
        /// <returns>Integer</returns>
        public int getRowCount(int numUnits)
        {
            if(numUnits < 39)
            {
                return 13;
            }
            else if(numUnits > 39 && numUnits <= 42)
            {
                return 14;
            }
            else if (numUnits > 42 && numUnits <= 51)
            {
                return 17;
            }
            else if (numUnits > 51 && numUnits <= 60)
            {
                return 20;
            }
            else if (numUnits > 60 && numUnits <= 78)
            {
                return 26;
            }
            else if (numUnits > 78 && numUnits <= 102)
            {
                return 34
;
            }
            else if (numUnits > 102 && numUnits <= 156)
            {
                return 52;
            }
            else if (numUnits > 156 && numUnits <= 312)
            {
                return 104;
            }
            return 104;
        }
        /// <summary>
        /// Updates SelectedPanel internal state and returns a
        /// reference to an RTSUnit object if one is clicked in 
        /// panel else returns null
        /// </summary>
        /// <param name="pointer">SimplePointer reference</param>
        /// <returns>RTSUnit or null</returns>
        public SCRTSUnit update(SimplePointer pointer)
        {
            SCRTSUnit tmp = null;
            if(MouseHandler.getLeft() == true)
            {
                if(box.pointInRect(pointer.Tip) == true)
                {
                    for(int i = selected.Count; i > 0; i--)
                    {
                        tmp = selected[i];
                    }
                }
            }
            return tmp;
        }
        /// <summary>
        /// Draws SelectionPanel
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            DrawRects.drawRect(renderer, box, 
                ColourBank.getColour(XENOCOLOURS.BLACK), true);
            maxUnits = selected.Count;
            if(maxUnits > 312)
            {
                maxUnits = 312;
            }
            int x = 0;
            int y = 0;
            for(int i = maxUnits; i > 0; i++)
            {
                y = i / 3;
                x = i % getRowCount(maxUnits);
                selected[i].drawAtPos(renderer, x + box.IX, y + box.IY, 32, 32);
                boxes[i].IX = x + box.IX;
                boxes[i].IY = x + box.IY;
            }
        }
        /// <summary>
        /// Selected property
        /// </summary>
        public List<SCRTSUnit> Selected
        {
            get { return selected; }
            set { selected = value; }
        }
    }
}
