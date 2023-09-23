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
    /// A stack of rectangles similar to a snow man configuration
    /// </summary>
    public class SnowManBox
    {
        //protected
        protected List<Rectangle> boxes;
        protected Point2D center;
        protected float dist;
        protected float dist2;
        protected double angle;

        //public
        /// <summary>
        /// SnowManBox constructor
        /// </summary>
        /// <param name="x">Center x position</param>
        /// <param name="y">Center y position</param>
        /// <param name="width">Width of boxes</param>
        /// <param name="height">Height of boxes</param>
        /// <param name="angle">Angle of rotation around center (in degrees)</param>
        public SnowManBox(float x, float y, float width, float height, double angle = 0)
        {
            boxes = new List<Rectangle>();
            center = new Point2D(x, y);
            dist = height;
            this.angle = angle;
            for(int b = 0; b < 3; b++)
            {
                boxes.Add(new Rectangle(x - (width / 2), (y + (b * height)) - (height * 1.5f), width, height)); 
            }
        }
        /// <summary>
        /// SnowManBox copy constructor
        /// </summary>
        /// <param name="obj">SnowManBox reference</param>
        public SnowManBox(SnowManBox obj)
        {
            boxes = new List<Rectangle>();
            center = new Point2D(obj.Center.X, obj.Center.Y);
            if (boxes.Count > 0)
            {
                dist = obj.Boxes[0].Height;
                for (int b = 0; b < 3; b++)
                {
                    boxes.Add(new Rectangle(obj.Boxes[b].X, obj.Boxes[b].Y, obj.Boxes[b].Width, obj.Boxes[b].Height));
                }
            }
            angle = obj.Angle;
        }

        public SnowManBox(StreamReader sr)
        {
            sr.ReadLine();//discard testing data
            boxes = new List<Rectangle>();
            center = new Point2D((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()));
            dist = (float)Convert.ToDecimal(sr.ReadLine());
            dist2 = (float)Convert.ToDecimal(sr.ReadLine());
            angle = (double)Convert.ToDecimal(sr.ReadLine());
            float x = (float)Convert.ToDecimal(sr.ReadLine());
            float y = (float)Convert.ToDecimal(sr.ReadLine());
            float width = (float)Convert.ToDecimal(sr.ReadLine());
            float height = (float)Convert.ToDecimal(sr.ReadLine());
            boxes = new List<Rectangle>();
            for (int i = 0; i < 3; i++)
            {
                boxes.Add(new Rectangle(x - (width / 2), (y + (i * height)) - (height * 1.5f), width, height));
            }
        }

        public void saveData(StreamWriter sw)
        {
            sw.WriteLine("======SnowManBox Data======");
            sw.WriteLine(center.X);
            sw.WriteLine(center.Y);
            sw.WriteLine(dist);
            sw.WriteLine(dist2);
            sw.WriteLine(angle);
            sw.WriteLine(center.X);//used when loading
            sw.WriteLine(center.Y);
            sw.WriteLine(boxes[0].Width);
            sw.WriteLine(boxes[0].Height);
        }
        /// <summary>
        /// Move SnowManBox by x and y values
        /// </summary>
        /// <param name="x">X move value</param>
        /// <param name="y">Y move value</param>
        public virtual void move(float x, float y)
        {
            for(int b = 0; b < boxes.Count; b++)
            {
                boxes[b].X += x;
                boxes[b].Y += y;
            }
        }
        /// <summary>
        /// Moves SnowManBox by speed in direction angle
        /// </summary>
        /// <param name="angle">Direction of movement (in degrees)</param>
        /// <param name="speed">Speed of movement</param>
        public virtual void move(double angle, float speed)
        {
            float shiftx = (float)Math.Cos(Helpers<int>.degreesToRadians(angle)) * speed;
            float shifty = (float)Math.Sin(Helpers<int>.degreesToRadians(angle)) * speed;
            for (int b = 0; b < boxes.Count; b++)
            {
                boxes[b].X += shiftx;
                boxes[b].Y += shifty;
            }
        }
        /// <summary>
        /// Set SnowManBox at a position
        /// </summary>
        /// <param name="x">Position center X value</param>
        /// <param name="y">Position center Y value</param>
        public virtual void set(float x, float y)
        {
            center.X = x;
            center.Y = y;
            for (int b = 0; b < boxes.Count; b++)
            {
                boxes[b].X = center.X - (boxes[b].Width / 2);
                boxes[b].Y = (y + (b * boxes[b].Height)) - (boxes[b].Height * 1.5f);
            }
        }
        /// <summary>
        /// Center property
        /// </summary>
        public Point2D Center
        {
            get { return center; }
        }
        /// <summary>
        /// Boxes property
        /// </summary>
        public List<Rectangle> Boxes
        {
            get { return boxes; }
        }
        /// <summary>
        /// Dist property
        /// </summary>
        public float Dist
        {
            get { return dist; }
        }

        /// <summary>
        /// Dist2 property
        /// </summary>
        public float Dist2
        {
            get { return dist2; }
        }
        /// <summary>
        /// Angle property
        /// </summary>
        public virtual double Angle
        {
            get { return angle; }
            set
                {
                    boxes[0].X = (float)Math.Cos(Helpers<int>.degreesToRadians(angle + 270)) * (boxes[0].Width * 1.5f) + center.X;
                    boxes[0].Y = (float)Math.Sin(Helpers<int>.degreesToRadians(angle + 270)) * (boxes[0].Height * 1.5f) + center.Y;
                    boxes[2].X = (float)Math.Cos(Helpers<int>.degreesToRadians(angle + 90)) * (boxes[2].Width * 1.5f) + center.X;
                    boxes[2].Y = (float)Math.Sin(Helpers<int>.degreesToRadians(angle + 90)) * (boxes[2].Height * 1.5f) + center.Y;
                }
        }
    }
    /// <summary>
    /// Has a small box on top, medium box in middle and large box on bottom
    /// Top box is half the size of middle box and bottom box is double the
    /// size of middle box
    /// </summary>
    public class SnowManBox2 : SnowManBox
    {
        //protected
        protected float dist1;
        //public
        /// <summary>
        /// SnowManBox2 constructor
        /// </summary>
        /// <param name="x">Center x position</param>
        /// <param name="y">Center y position</param>
        /// <param name="width">Width of center box</param>
        /// <param name="height">Height of center box</param>
        /// <param name="angle">Angle of SnowManBox2</param>
        public SnowManBox2(float x, float y, float width, float height, double angle) : 
            base(x, y, width, height, angle)
        {
            boxes[0].X -= boxes[0].Width / 4;
            boxes[0].Y += boxes[0].Height / 4;
            boxes[0].Width = boxes[0].Width / 2;
            boxes[0].Height = boxes[0].Height / 2;

            boxes[2].X -= boxes[0].Width / 2;
            boxes[2].Y += boxes[0].Height / 2;
            boxes[2].Width = boxes[0].Width * 2;
            boxes[2].Height = boxes[0].Height * 2;

            dist1 = boxes[0].Height + (boxes[1].Height / 2);
            dist2 = boxes[1].Height / 2;
        }
        /// <summary>
        /// SnowManBox2 copy constructor
        /// </summary>
        /// <param name="obj">SnowManBox2 reference</param>
        public SnowManBox2(SnowManBox2 obj) : base(obj)
        {
            dist = obj.boxes[1].Height;
            dist1 = obj.Dist1;
            dist2 = obj.Dist2;
        }
        /// <summary>
        /// Set SnowManBox2 center position
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public override void set(float x, float y)
        {
            center.X = x;
            center.Y = y;
            boxes[0].X = center.X - (boxes[0].Width / 2);
            boxes[0].Y = center.Y - ((boxes[1].Height / 2) + boxes[0].Height);
            boxes[1].X = center.X - (boxes[1].Width / 2);
            boxes[1].Y = center.Y - (boxes[1].Height / 2);
            boxes[2].X = center.X - (boxes[2].Width / 2);
            boxes[2].Y = center.Y + (boxes[1].Height / 2);
        }
        /// <summary>
        /// Angle property override
        /// </summary>
        public override double Angle
        {
            get { return angle; }
            set
            {
                boxes[0].X = (float)Math.Cos(Helpers<int>.degreesToRadians(angle + 270) * dist1) + center.X;
                boxes[0].Y = (float)Math.Sin(Helpers<int>.degreesToRadians(angle + 270) * dist1) + center.Y;
                boxes[2].X = (float)Math.Cos(Helpers<int>.degreesToRadians(angle + 90) * dist2) + center.X;
                boxes[2].Y = (float)Math.Sin(Helpers<int>.degreesToRadians(angle + 90) * dist2) + center.Y;
            }
        }
        /// <summary>
        /// Dist1 property
        /// </summary>
        public float Dist1
        {
            get { return dist1; }
        }
    }
}
