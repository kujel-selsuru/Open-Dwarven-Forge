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
    /// Collection of Rectangles with methods for collision handling and moving, works best
    /// with odd number of boxes
    /// </summary>
    public class MultiHitBox
    {
        //protected
        protected List<Rectangle> boxes;
        protected int bw;
        protected int bh;
        protected Point2D bc;
        protected int spacing;

        //public
        /// <summary>
        /// MutliHitBox constructor
        /// </summary>
        /// <param name="x">X center position</param>
        /// <param name="y">Y center position</param>
        /// <param name="bw">Box width in pixels</param>
        /// <param name="bh">Box height in pixels</param>
        /// <param name="bn">Numer of boxes</param>
        public MultiHitBox(int x, int y, int bw, int bh, int bn = 3)
        {
            bc = new Point2D(x, y);
            this.bw = bw;
            this.bh = bh;
            spacing = (int)(((bw * bw) + (bh * bh)) * 0.9f);
            boxes = new List<Rectangle>();
            boxes.Add(new Rectangle(x - (bw / 2), y - (bh / 2), bw, bh));
            for(int b = 1; b < bn; b++)
            {
                if (b % 2 == 0)
                {
                    boxes.Add(new Rectangle(x + (bn * spacing), y - (bh / 2), bw, bh));
                }
                else
                {
                    boxes.Add(new Rectangle(x - (bn * spacing), y - (bh / 2), bw, bh));
                }
            }
        }
        /// <summary>
        /// MutliHitBox constructor
        /// </summary>
        /// <param name="x">X center position</param>
        /// <param name="y">Y center position</param>
        /// <param name="bw">Box width in pixels</param>
        /// <param name="bh">Box height in pixels</param>
        /// <param name="bn">Numer of boxes</param>
        /// <param name="angle">Angle of boxes</param>
        public MultiHitBox(int x, int y, int bw, int bh, int bn, float angle)
        {
            bc = new Point2D(x, y);
            this.bw = bw;
            this.bh = bh;
            spacing = (int)(((bw * bw) + (bh * bh)) * 0.9f);
            boxes = new List<Rectangle>();
            Rectangle box = null;
            boxes.Add(new Rectangle(x - (bw / 2), y - (bh / 2), bw, bh));
            for (int b = 1; b < bn; b++)
            {
                if (b % 2 == 0)
                {
                    box = new Rectangle(x + (bn * spacing), y - (bh / 2), bw, bh);
                    box.X = Helpers<int>.getXValue(box.X, angle, spacing);
                    box.Y = Helpers<int>.getYValue(box.Y, angle, spacing);
                    boxes.Add(box);
                }
                else
                {
                    box = new Rectangle(x - (bn * spacing), y - (bh / 2), bw, bh);
                    box.X = Helpers<int>.getXValue(box.X, angle + 180, spacing);
                    box.Y = Helpers<int>.getYValue(box.Y, angle + 180, spacing);
                    boxes.Add(box);
                }
            }
        }
        /// <summary>
        /// MultiHitBox copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public MultiHitBox(MultiHitBox obj)
        {
            bc = new Point2D(obj.BC.X, obj.BC.Y);
            bw = obj.BW;
            bh = obj.BH;
            spacing = obj.spacing;
            boxes = new List<Rectangle>();
            for(int b = 0; b < obj.boxes.Count - 1; b++)
            {
                boxes.Add(new Rectangle(obj.boxes[b].X, obj.boxes[b].Y, obj.boxes[b].Width, obj.boxes[b].Height));
            }
        }
        /// <summary>
        /// Checks if a point is in MultiHitBox
        /// </summary>
        /// <param name="p">Point2D refernece</param>
        /// <returns>Boolean</returns>
        public bool pointInRect(Point2D p)
        {
            for(int b = 0; b < boxes.Count - 1; b++)
            {
                if(boxes[b].pointInRect(p) == true)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if a rectangle intersects MultiHitBox
        /// </summary>
        /// <param name="rect">Rectangle reference</param>
        /// <returns>Boolean</returns>
        public bool boxInRect(Rectangle rect)
        {
            for (int b = 0; b < boxes.Count - 1; b++)
            {
                if (boxes[b].intersects(rect) == true)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if another MultiHitBox intersects with this instance
        /// </summary>
        /// <param name="mhb">MultiHitBox reference</param>
        /// <returns>Boolean</returns>
        public bool intersects(MultiHitBox mhb)
        {
            for(int b = 0; b < boxes.Count - 1; b++)
            {
                for(int sb = 0; sb < mhb.Boxes.Count - 1; sb++)
                {
                    if(boxes[b].intersects(mhb.Boxes[sb]) == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Move MultiHitBox
        /// </summary>
        /// <param name="x">Movement in X axis</param>
        /// <param name="y">Movement in Y axis</param>
        public void move(int x, int y)
        {
            for(int b = 0; b < boxes.Count - 1; b++)
            {
                boxes[b].X += x;
                boxes[b].Y += y;
            }
        }
        /// <summary>
        /// Move MultiHitBox
        /// </summary>
        /// <param name="angle">Direction of movement</param>
        /// <param name="speed">Speed of movement</param>
        public void move(float angle, float speed)
        {
            for (int b = 0; b < boxes.Count - 1; b++)
            {
                boxes[b].X = Helpers<int>.movementX(angle, speed);
                boxes[b].Y = Helpers<int>.movementY(angle, speed);
            }
        }
        /// <summary>
        /// Sets the MultiHitBox position set to center of boxes
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public void setPos(float x, float y)
        {
            for (int b = 0; b < boxes.Count - 1; b++)
            {
                if (b % 2 == 0)
                {
                    boxes[b].X = x + (b * spacing);
                    boxes[b].Y = y - (bh / 2);
                }
                else
                {
                    boxes[b].X = x - (b * spacing);
                    boxes[b].Y = y - (bh / 2);
                }
            }
        }
        /// <summary>
        /// Boxes property
        /// </summary>
        public List<Rectangle> Boxes
        {
            get { return boxes; }
        }
        /// <summary>
        /// Box width property
        /// </summary>
        public int BW
        {
            get { return bw; }
            set { bw = value; }
        }
        /// <summary>
        /// Box height property
        /// </summary>
        public int BH
        {
            get { return bh; }
            set { bh = value; }
        }
        /// <summary>
        /// Box center property
        /// </summary>
        public Point2D BC
        {
            get { return bc; }
        }
        /// <summary>
        /// Spacing property
        /// </summary>
        public int Spacing
        {
            get { return spacing; }
            set { spacing = value; }
        }
        /// <summary>
        /// Box number property
        /// </summary>
        public int BN
        {
            get { return boxes.Count; }
        }
    }
}
