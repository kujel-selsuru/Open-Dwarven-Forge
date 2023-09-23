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
    /// A draw queue which render's XenoSprite objects in the priority 
    /// they are assigned
    /// </summary>
    public static class DrawQueue
    {
        static PriorityQueue<XenoSprite> objects;
        /// <summary>
        /// Initializes the DrawQueue
        /// </summary>
        public static void init()
        {
            objects = new PriorityQueue<XenoSprite>();
        }
        /// <summary>
        /// Clears all references in the DrawQueue
        /// </summary>
        public static void clearQueue()
        {
            objects.clear();
        }
        /// <summary>
        /// Add a XenoSprite object reference to the queue
        /// </summary>
        /// <param name="obj">XenoSprite reference</param>
        /// <param name="height">Height on screen of object to render</param>
        public static void addObject(XenoSprite obj, int height)
        {
            objects.enqueue(obj, height);
        }
        /// <summary>
        /// Draws all objects in the DrawQueue
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window x offset value</param>
        /// <param name="winy">Window y offset value</param>
        public static void drawObjects(IntPtr renderer, int winx = 0, int winy = 0)
        {
            while(objects.isEmpty() == false)
            {
                objects.dequeue().draw(renderer, winx, winy);
            }
        }
    }
}
