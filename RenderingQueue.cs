//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// A system for prioritzed rendering callable from any code in a project
    /// </summary>
    
    /// <summary>
    /// RenderTask class
    /// </summary>
    public class RenderTask
    {
        //protected
        protected SDL.SDL_Rect srcRect;
        protected SDL.SDL_Rect destRect;
        protected Texture2D source;
        
        //public
        public RenderTask(Texture2D source, SDL.SDL_Rect srcRect, SDL.SDL_Rect destRect)
        {
            this.source = source;
            this.srcRect = srcRect;
            this.destRect = destRect;
        }
        /// <summary>
        /// Source property
        /// </summary>
        public Texture2D Source
        {
            get { return source; }
        }
        /// <summary>
        /// SrcRect property
        /// </summary>
        public SDL.SDL_Rect SrcRect
        {
            get { return srcRect; }
        }
        /// <summary>
        /// DestRect property
        /// </summary>
        public SDL.SDL_Rect DestRect
        {
            get { return destRect; }
        }
    }
    /// <summary>
    /// RenderTaskEx class
    /// </summary>
    public class RenderTaskEx : RenderTask
    {
        //protected
        protected Point2D center;
        protected double angle;

        //public
        public RenderTaskEx(Texture2D source, SDL.SDL_Rect srcRect, SDL.SDL_Rect destRect, Point2D center, 
            double angle) : base(source, srcRect, destRect)
        {
            this.center = center;
            this.angle = angle;
        }
        /// <summary>
        /// Center property
        /// </summary>
        public Point2D Center
        {
            get { return center; }
        }
        /// <summary>
        /// Angle property
        /// </summary>
        public double Angle
        {
            get { return angle; }
        }
    }
    /// <summary>
    /// RenderingQueue
    /// </summary>
    public static class RenderingQueue
    {
        //private
        static PriorityQueue<RenderTask> tasks;
        static PriorityQueue<RenderTaskEx> tasksEx;

        //public
        static RenderingQueue()
        {
            tasks = new PriorityQueue<RenderTask>();
            tasksEx = new PriorityQueue<RenderTaskEx>();
        }
        /// <summary>
        /// Adds a rendering task to queue
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="srcRect">SDL_Rect</param>
        /// <param name="destRect">SDL_Rect</param>
        /// <param name="depth">depth in rendering, lower depth is rendered later</param>
        public static void addRenderTask(Texture2D source, SDL.SDL_Rect srcRect, SDL.SDL_Rect destRect, int depth)
        {
            tasks.enqueue(new RenderTask(source, srcRect, destRect), depth);
        }
        /// <summary>
        /// Adds a rendering task to queue
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="srcRect">SDL_Rect</param>
        /// <param name="destRect">SDL_Rect</param>
        /// <param name="depth">depth in rendering, lower depth is rendered later</param>
        /// <param name="center">Point2D</param>
        /// <param name="angle">angle of rendering</param>
        public static void addRenderTaskEx(Texture2D source, SDL.SDL_Rect srcRect, SDL.SDL_Rect destRect, int depth,
            Point2D center, double angle)
        {
            tasks.enqueue(new RenderTaskEx(source, srcRect, destRect, center, angle), depth);
        }
        /// <summary>
        /// Renders simple render jobs
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window X offset</param>
        /// <param name="winy">Window Y offset</param>
        public static void render(IntPtr renderer, int winx = 0, int winy = 0)
        {
            RenderTask task = null;
            SDL.SDL_Rect src;
            SDL.SDL_Rect dest;

            while (!tasks.isEmpty())
            {
                task = tasks.dequeue();
                src = task.SrcRect;
                dest = task.DestRect;
                SDL.SDL_RenderCopy(renderer, task.Source.texture, ref src, ref dest);
            }
        }
        /// <summary>
        /// Renders angled rendering jobs
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public static void RenderEx(IntPtr renderer)
        {
            RenderTaskEx taskEx = null;
            SDL.SDL_Rect src;
            SDL.SDL_Rect dest;
            SDL.SDL_Point center;
            while (!tasksEx.isEmpty())
            {
                taskEx = tasksEx.dequeue();
                src = taskEx.SrcRect;
                dest = taskEx.DestRect;
                center.x = taskEx.Center.IX;
                center.y = taskEx.Center.IY;
                SDL.SDL_RenderCopyEx(renderer, taskEx.Source.texture, ref src, ref dest, taskEx.Angle, ref center, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
            }
        }
    }
}
