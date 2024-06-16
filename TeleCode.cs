//====================================================
//Written by Kujel Selsuru
//Last Updated 07/05/24
//====================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenoLib
{
    /// <summary>
    /// Stores teleport coordinates for transporting between open world cells
    /// </summary>
    public class TeleCode
    {
        //protected
        protected int cx;
        protected int cy;
        protected int px;
        protected int py;

        //public
        /// <summary>
        /// TeleCode constructor
        /// </summary>
        /// <param name="cx">Cell X value</param>
        /// <param name="cy">Cell Y value</param>
        /// <param name="px">Position X value</param>
        /// <param name="py">Position Y value</param>
        public TeleCode(int cx = 0, int cy = 0, int px = 0, int py = 0)
        {
            this.cx = cx;
            this.cy = cy;
            this.px = px;
            this.py = py;
        }
        /// <summary>
        /// TeleCode from file constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public TeleCode(System.IO.StreamReader sr)
        {
            sr.ReadLine();
            cx = Convert.ToInt32(sr.ReadLine());
            cy = Convert.ToInt32(sr.ReadLine());
            px = Convert.ToInt32(sr.ReadLine());
            py = Convert.ToInt32(sr.ReadLine());
        }
        /// <summary>
        /// TeleCode copy constructor
        /// </summary>
        /// <param name="obj">TeleCode reference</param>
        public TeleCode(TeleCode obj)
        {
            cx = obj.cx;
            cy = obj.cy;
            px = obj.px;
            py = obj.py;
        }
        /// <summary>
        /// Saves TeleCode data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======TeleCode Data======");
        }
        /// <summary>
        /// Cx property
        /// </summary>
        public int Cx
        {
            get { return cx; }
            set
            {
                cx = value;
                if(cx < 0)
                {
                    cx = 0;
                }
            }
        }
        /// <summary>
        /// Cy property
        /// </summary>
        public int Cy
        {
            get { return cy; }
            set
            {
                cy = value;
                if(cy < 0)
                {
                    cy = 0;
                }
            }
        }
        /// <summary>
        /// Px property
        /// </summary>
        public int Px
        {
            get { return px; }
            set
            {
                px = value;
                if(px < 0)
                {
                    px = 0;
                }
            }
        }
        /// <summary>
        /// Py property
        /// </summary>
        public int Py
        {
            get { return py; }
            set
            {
                py = value;
                if(py < 0)
                {
                    py = 0;
                }
            }
        }
    }
}
