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
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// XenoPortal class
    /// </summary>
    public class XenoPortal : XenoSprite
    {
        //protected
        protected int cx;
        protected int cy;
        protected int tx;
        protected int ty;
        protected string cellName;
        protected bool toInterior;

        //public
        /// <summary>
        /// XenoPortal constructor
        /// </summary>
        /// <param name="source">Texture2D</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y poisition</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="cx">Cell x to portal to</param>
        /// <param name="cy">Cell y to portal to</param>
        /// <param name="tx">Target x to portal to</param>
        /// <param name="ty">Target y to portal to</param>
        /// <param name="cellName">Cell name to portal to</param>
        /// <param name="cellName">Is target cell an interior cell flag value</param>
        public XenoPortal(Texture2D source, float x, float y, int width, int height, int numFrames, 
            int cx, int cy, int tx, int ty, string cellName, bool toInterior) : 
            base(source, x, y, width, height, numFrames)
        {
            this.cx = cx;
            this.cy = cy;
            this.tx = tx;
            this.ty = ty;
            this.cellName = cellName;
            this.toInterior = toInterior;
        }
        /// <summary>
        /// XenoPortal copy constructor
        /// </summary>
        /// <param name="obj">XenoPortal reference</param>
        public XenoPortal(XenoPortal obj) : base(obj)
        {
            this.cx = obj.cx;
            this.cy = obj.cy;
            this.tx = obj.tx;
            this.ty = obj.ty;
            this.cellName = obj.cellName;
            this.toInterior = obj.toInterior;
        }
        /// <summary>
        /// XenoPortal from file constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="sr">StreamReader reference</param>
        public XenoPortal(Texture2D source, StreamReader sr) : base(source, sr)
        {
            cx = Convert.ToInt32(sr.ReadLine());
            cy = Convert.ToInt32(sr.ReadLine());
            tx = Convert.ToInt32(sr.ReadLine());
            ty = Convert.ToInt32(sr.ReadLine());
            cellName = sr.ReadLine();
            toInterior = Convert.ToBoolean(sr.ReadLine());
        }
        /// <summary>
        /// Save data override
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine(cx);
            sw.WriteLine(cy);
            sw.WriteLine(tx);
            sw.WriteLine(ty);
            sw.WriteLine(cellName);
            sw.WriteLine(toInterior);
        }
        /// <summary>
        /// CX property
        /// </summary>
        public int CX
        {
            get { return cx; }
            set { cx = value; }
        }
        /// <summary>
        /// CY property
        /// </summary>
        public int CY
        {
            get { return cy; }
            set { cy = value; }
        }
        /// <summary>
        /// TX property
        /// </summary>
        public int TX
        {
            get { return tx; }
            set { tx = value; }
        }
        /// <summary>
        /// TY property
        /// </summary>
        public int TY
        {
            get { return ty; }
            set { ty = value; }
        }
        /// <summary>
        /// CellName property
        /// </summary>
        public string CellName
        {
            get { return cellName; }
            set { cellName = value; }
        }
        /// <summary>
        /// ToInterior property
        /// </summary>
        public bool ToInterior
        {
            get { return toInterior; }
        }
    }
}
