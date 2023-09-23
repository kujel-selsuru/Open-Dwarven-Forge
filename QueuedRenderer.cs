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
    /// Holds render job data
    /// </summary>
    public class RenderJob
    {
        //public
        public Texture2D source;
        public Rectangle srcRect;
        public Rectangle destRect;
        /// <summary>
        /// RenderJob constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="srcRect">Rectangle reference</param>
        /// <param name="destRect">Rectangle reference</param>
        public RenderJob(Texture2D source, Rectangle srcRect, Rectangle destRect)
        {
            this.source = source;
            this.srcRect = srcRect;
            this.destRect = destRect;
        }
        /// <summary>
        /// RenderJob copy constructor
        /// </summary>
        /// <param name="obj">RenderJob reference</param>
        public RenderJob(RenderJob obj)
        {
            source = obj.source;
            srcRect = obj.srcRect;
            destRect = obj.destRect;
        }
    }
    /// <summary>
    /// Queued rendering system
    /// </summary>
    public static class QueuedRenderer
    {
        //private
        static PriorityQueue<RenderJob> jobs;

        //public 
        /// <summary>
        /// QueuedRender constructor
        /// </summary>
        static QueuedRenderer()
        {
            jobs = new PriorityQueue<RenderJob>();
        }
        /// <summary>
        /// Adds a job to QueuedRenderer
        /// </summary>
        /// <param name="source">Texture2D referecne</param>
        /// <param name="srcRect">Rectangle reference</param>
        /// <param name="destRect">Rectangle reference</param>
        /// <param name="pri">Priority of rendering job (higher values yeild higher priority)</param>
        public static void addJob(Texture2D source, Rectangle srcRect, Rectangle destRect, int pri)
        {
            RenderJob job = new RenderJob(source, srcRect, destRect);
            jobs.enqueue(job, pri);
        }
        /// <summary>
        /// Clears all jobs currently in renderer
        /// </summary>
        public static void clearRenderer()
        {
            jobs.clear();
        }
        /// <summary>
        /// Draws all jobs currently in QueuedRenderer
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public static void drawJobs(IntPtr renderer)
        {
            RenderJob job = null;
            while (jobs.isEmpty() == false)
            {
                job = jobs.dequeue();
                if (job != null)
                {
                    SimpleDraw.draw(renderer, job.source, job.srcRect, job.destRect);
                }
            }
        }

        /// <summary>
        /// Holds render job data
        /// </summary>
        public class RenderJobMKI
        {
            //public
            public Texture2D source;
            public Rectangle srcRect;
            public Rectangle destRect;
            /// <summary>
            /// RenderJobMKI constructor
            /// </summary>
            /// <param name="source">Texture2D reference</param>
            /// <param name="srcRect">Rectangle reference</param>
            /// <param name="destRect">Rectangle reference</param>
            public RenderJobMKI(Texture2D source, Rectangle srcRect, Rectangle destRect)
            {
                this.source = source;
                this.srcRect = srcRect;
                this.destRect = destRect;
            }
            /// <summary>
            /// RenderJobMKI copy constructor
            /// </summary>
            /// <param name="obj">RenderJob reference</param>
            public RenderJobMKI(RenderJobMKI obj)
            {
                source = obj.source;
                srcRect = obj.srcRect;
                destRect = obj.destRect;
            }
        }

        /// <summary>
        /// Holds render job data
        /// </summary>
        public class RenderJobMKII
        {
            //public
            public Texture2D source;
            public Rectangle srcRect;
            public Rectangle destRect;
            public double rotation;
            public Point2D center;
            /// <summary>
            /// RenderJobMKII constructor
            /// </summary>
            /// <param name="source">Texture2D reference</param>
            /// <param name="srcRect">Rectangle reference</param>
            /// <param name="destRect">Rectangle reference</param>
            /// <param name="rotation">Angle of rotation</param>
            /// <param name="center">Center of rotation</param>
            public RenderJobMKII(Texture2D source, Rectangle srcRect, Rectangle destRect, double rotation, Point2D center)
            {
                this.source = source;
                this.srcRect = srcRect;
                this.destRect = destRect;
                this.rotation = rotation;
                this.center = center;
            }
            /// <summary>
            /// RenderJob copy constructor
            /// </summary>
            /// <param name="obj">RenderJob reference</param>
            public RenderJobMKII(RenderJobMKII obj)
            {
                source = obj.source;
                srcRect = obj.srcRect;
                destRect = obj.destRect;
                rotation = obj.rotation;
                center = obj.center;
            }
        }
        /// <summary>
        /// Queued rendering system
        /// </summary>
        public static class QueuedRendererMKII
        {
            //private
            static PriorityQueue<RenderJobMKII> jobsMKII;
            static PriorityQueue<RenderJobMKI> jobsMKI;

            //public 
            static QueuedRendererMKII()
            {
                jobsMKII = new PriorityQueue<RenderJobMKII>();
                jobsMKI = new PriorityQueue<RenderJobMKI>();
            }
            /// <summary>
            /// Adds a renderJob to the renderer (with rotation parameters)
            /// </summary>
            /// <param name="source">Texture2D reference</param>
            /// <param name="srcRect">Rectangle reference</param>
            /// <param name="destRect">Rectangle reference</param>
            /// <param name="rotation">Rotation angle</param>
            /// <param name="center">Center of rotation</param>
            /// <param name="pri">Priority of job</param>
            public static void addJobMKII(Texture2D source, Rectangle srcRect, Rectangle destRect, double rotation, Point2D center, int pri)
            {
                RenderJobMKII job = new RenderJobMKII(source, srcRect, destRect, rotation, center);
                jobsMKII.enqueue(job, pri);
            }
            /// <summary>
            /// Adds a renderJob to the renderer (with rotation parameters)
            /// </summary>
            /// <param name="source">Texture2D reference</param>
            /// <param name="srcRect">Rectangle reference</param>
            /// <param name="destRect">Rectangle reference</param>
            /// <param name="pri">Priority of job</param>
            public static void addJobMKI(Texture2D source, Rectangle srcRect, Rectangle destRect, int pri)
            {
                RenderJobMKI job = new RenderJobMKI(source, srcRect, destRect);
                jobsMKI.enqueue(job, pri);
            }
            /// <summary>
            /// Clears all jobs currently in renderers
            /// </summary>
            public static void clearRenderers()
            {
                jobsMKII.clear();
                jobsMKI.clear();
            }
            /// <summary>
            /// Draws all MKII jobs currently in QueuedRenderer
            /// </summary>
            /// <param name="renderer">Renderer reference</param>
            public static void drawJobsMKII(IntPtr renderer)
            {
                RenderJobMKII job = null;
                while (jobs.isEmpty() == false)
                {
                    job = jobsMKII.dequeue();
                    if (job != null)
                    {
                        SimpleDraw.draw(renderer, job.source, job.srcRect, job.destRect, job.rotation, job.center, SDL2.SDL.SDL_RendererFlip.SDL_FLIP_NONE);
                    }
                }
            }
            /// <summary>
            /// Draws all MKI jobs currently in QueuedRenderer
            /// </summary>
            /// <param name="renderer">Renderer reference</param>
            public static void drawJobsMKI(IntPtr renderer)
            {
                RenderJobMKI job = null;
                while (jobsMKI.isEmpty() == false)
                {
                    job = jobsMKI.dequeue();
                    if (job != null)
                    {
                        SimpleDraw.draw(renderer, job.source, job.srcRect, job.destRect);
                    }
                }
            }
        }
    }
}
