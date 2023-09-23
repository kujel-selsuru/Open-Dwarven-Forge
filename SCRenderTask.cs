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
using XenoLib;

namespace XenoLib
{
    public class SCRenderTask
    {
        //protected
        protected Texture2D src;
        protected int srcX;
        protected int srcY;
        protected int srcW;
        protected int srcH;
        protected int destX;
        protected int destY;
        protected int destW;
        protected int destH;
        protected int renderPriority;

        //public
        /// <summary>
        /// SCRenderTask constructor
        /// </summary>
        /// <param name="src">Texture2D reference</param>
        /// <param name="srcX">Source X value</param>
        /// <param name="srcY">Source Y value</param>
        /// <param name="srcW">Source W value</param>
        /// <param name="srcH">Source H value</param>
        /// <param name="destX">Destination X value</param>
        /// <param name="destY">Destination Y value</param>
        /// <param name="destW">Destination W value</param>
        /// <param name="destH">Destination H value</param>
        /// <param name="renderPriority">Render priority value</param>
        public SCRenderTask(Texture2D src, int srcX, int srcY, 
            int srcW, int srcH, int destX, int destY, int destW, 
            int destH, int renderPriority)
        {
            this.src = src;
            this.srcX = srcX;
            this.srcY = srcY;
            this.srcW = srcW;
            this.srcH = srcH;
            this.destX = destX;
            this.destY = destY;
            this.destW = destW;
            this.destH = destH;
            this.renderPriority = renderPriority;
        }
        /// <summary>
        /// Src property
        /// </summary>
        public Texture2D Src
        {
            get { return src; }
        }
        /// <summary>
        /// SrcX property
        /// </summary>
        public int SrcX
        {
            get { return srcX; }
        }
        /// <summary>
        /// SrcY property
        /// </summary>
        public int SrcY
        {
            get { return srcY; }
        }
        /// <summary>
        /// SrcW property
        /// </summary>
        public int SrcW
        {
            get { return srcW; }
        }
        /// <summary>
        /// SrcH property
        /// </summary>
        public int SrcH
        {
            get { return srcH; }
        }
        /// <summary>
        /// DestX property
        /// </summary>
        public int DestX
        {
            get { return destX; }
        }
        /// <summary>
        /// DestY property
        /// </summary>
        public int DestY
        {
            get { return destY; }
        }
        /// <summary>
        /// DestW property
        /// </summary>
        public int DestW
        {
            get { return destW; }
        }
        /// <summary>
        /// DestH property
        /// </summary>
        public int DestH
        {
            get { return destH; }
        }
        /// <summary>
        /// RenderPriority property
        /// </summary>
        public int RenderPriority
        {
            get { return renderPriority; }
        }
    }
}
