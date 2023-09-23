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
    public class DrawingNode
    {
        public XenoSprite sprite;
        public int priority;
        public DrawingNode next;
        /// <summary>
        /// DrawingNode constructor
        /// </summary>
        /// <param name="sprite">XenoSprite reference</param>
        /// <param name="priority">Rendering priority value</param>
        /// <param name="next">Next DrawingNode</param>
        /// <param name="prev">Prev DrawingNode</param>
        public DrawingNode(XenoSprite sprite, int priority, DrawingNode next = null)
        {
            this.sprite = sprite;
            this.priority = priority;
            this.next = next;
        }
    }

    public static class LayeredDrawing
    {
        //private
        private static DrawingNode start;
        private static int index;

        //public
        /// <summary>
        /// Initializes LayeredDrawing
        /// </summary>
        public static void init()
        {
            start = null;
            index = 0;
        }
        /// <summary>
        /// Add a sprite to rendering list
        /// </summary>
        /// <param name="sprite">XenoSprite reference</param>
        /// <param name="priority">Rendering priority value</param>
        public static void addSprite(XenoSprite sprite, int priority)
        {
            DrawingNode node = new DrawingNode(sprite, priority);
            DrawingNode current = start;
            if(start != null)
            {
                if(current != null)
                {
                    while(current != null && current.priority <= priority)
                    {
                        current = current.next;
                    }
                    if(current != null)
                    {
                        node.next = current.next;
                        current.next = node;
                        index++;
                    }
                }
            }
            else
            {
                start = node;
            }
        }
        /// <summary>
        /// Draws sprites in rendering list
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window X offset</param>
        /// <param name="winy">Window Y offset</param>
        public static void drawSprites(IntPtr renderer, int winx, int winy)
        {
            while(start != null)
            {
                if(start.sprite != null)
                {
                    start.sprite.draw(renderer, winx, winy);
                    start = start.next;
                    index--;
                }
                else
                {
                    index--;
                    break;
                }
            }
        }
        /// <summary>
        /// Clears drawing jobs
        /// </summary>
        public static void clearJobs()
        {
            if(start != null)
            {
                while(start.next != null)
                {
                    start = start.next;
                    index--;
                }
            }
        }
        /// <summary>
        /// JobCount property
        /// </summary>
        public static int JobCount
        {
            get { return index; }
        }
    }
}
