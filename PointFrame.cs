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
    /// A four corner point frame for world collision testing
    /// p1-----p2
    /// |       |
    /// |   C   |
    /// |       |
    /// p4-----p4
    /// </summary>
    public class PointFrame
    {
        //protected
        protected Point2D p1;
        protected Point2D p2;
        protected Point2D p3;
        protected Point2D p4;
        protected Point2D center;
        protected float p1d;
        protected float p2d;
        protected float p3d;
        protected float p4d;
        protected double p1a;
        protected double p2a;
        protected double p3a;
        protected double p4a;
        protected float w;
        protected float h;

        //public
        /// <summary>
        /// PointFrame constructor
        /// </summary>
        /// <param name="x">Center X position</param>
        /// <param name="y">Center Y position</param>
        /// <param name="w">Width of frame</param>
        /// <param name="h">Height of frame</param>
        public PointFrame(float x, float y, float w, float h)
        {
            center = new Point2D(x, y);
            p1 = new Point2D(x - (w / 2), y - (h / 2));
            p2 = new Point2D(x + (w / 2), y - (h / 2));
            p3 = new Point2D(x + (w / 2), y + (h / 2));
            p4 = new Point2D(x - (w / 2), y + (h / 2));
            p1d = Point2D.calculateDistance(center, p1);
            p2d = Point2D.calculateDistance(center, p2);
            p3d = Point2D.calculateDistance(center, p3);
            p4d = Point2D.calculateDistance(center, p4);
            p1a = Point2D.CalcAngle(center, p1);
            p2a = Point2D.CalcAngle(center, p2);
            p3a = Point2D.CalcAngle(center, p3);
            p4a = Point2D.CalcAngle(center, p4);
            this.w = w;
            this.h = h;
        }
        /// <summary>
        /// PointFrame copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public PointFrame(PointFrame obj)
        {
            center = new Point2D(obj.Center.X, obj.Center.Y);
            p1 = new Point2D(obj.Center.X - (obj.W / 2), obj.Center.Y - (obj.H / 2));
            p2 = new Point2D(obj.Center.X + (obj.W / 2), obj.Center.Y - (obj.H / 2));
            p3 = new Point2D(obj.Center.X + (obj.W / 2), obj.Center.Y + (obj.H / 2));
            p4 = new Point2D(obj.Center.X - (obj.W / 2), obj.Center.Y + (obj.H / 2));
            p1d = obj.P1D;
            p2d = obj.P2D;
            p3d = obj.P3D;
            p4d = obj.P4D;
            p1a = obj.P1A;
            p2a = obj.P2A;
            p3a = obj.P3A;
            p4a = obj.P4A;
            w = obj.W;
            h = obj.H;
        }
        /// <summary>
        /// From file PointFrame constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public PointFrame(StreamReader sr)
        {
            sr.ReadLine(); //discard testing data
            center = new Point2D((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()));
            p1 = new Point2D((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()));
            p2 = new Point2D((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()));
            p3 = new Point2D((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()));
            p4 = new Point2D((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()));
            p1d = (float)Convert.ToDecimal(sr.ReadLine());
            p2d = (float)Convert.ToDecimal(sr.ReadLine());
            p3d = (float)Convert.ToDecimal(sr.ReadLine());
            p4d = (float)Convert.ToDecimal(sr.ReadLine());
            p1a = (double)Convert.ToDecimal(sr.ReadLine());
            p2a = (double)Convert.ToDecimal(sr.ReadLine());
            p3a = (double)Convert.ToDecimal(sr.ReadLine());
            p4a = (double)Convert.ToDecimal(sr.ReadLine());
            w = (float)Convert.ToDecimal(sr.ReadLine());
            h = (float)Convert.ToDecimal(sr.ReadLine());
        }
        /// <summary>
        /// Saves PointFrame data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveData(StreamWriter sw)
        {
            sw.WriteLine("======PointFrame Data======");
            sw.WriteLine(center.X);
            sw.WriteLine(center.Y);
            sw.WriteLine(p1.X);
            sw.WriteLine(p1.Y);
            sw.WriteLine(p2.X);
            sw.WriteLine(p2.Y);
            sw.WriteLine(p3.X);
            sw.WriteLine(p3.Y);
            sw.WriteLine(p4.X);
            sw.WriteLine(p4.Y);
            sw.WriteLine(p1d);
            sw.WriteLine(p2d);
            sw.WriteLine(p3d);
            sw.WriteLine(p4d);
            sw.WriteLine(p1a);
            sw.WriteLine(p2a);
            sw.WriteLine(p3a);
            sw.WriteLine(p4a);
            sw.WriteLine(w);
            sw.WriteLine(h);
        }
        /// <summary>
        /// Move PointFrame by angle in a specified direction
        /// </summary>
        /// <param name="dir">Direction of movement</param>
        /// <param name="speed">Speed of movement</param>
        public void move(double dir, float speed)
        {
            Point2D vector = new Point2D(0, 0);
            vector.X = (float)Math.Cos(Helpers<int>.degreesToRadians(dir)) * speed;
            vector.Y = (float)Math.Sin(Helpers<int>.degreesToRadians(dir)) * speed;
            center.X += vector.X;
            center.Y += vector.Y;
            p1.X += vector.X;
            p1.Y += vector.Y;
            p2.X += vector.X;
            p2.Y += vector.Y;
            p3.X += vector.X;
            p3.Y += vector.Y;
            p4.X += vector.X;
            p4.Y += vector.Y;
        }
        /// <summary>
        /// Moves PointFrame in a specified direction
        /// </summary>
        /// <param name="x">X direction</param>
        /// <param name="y">Y direction</param>
        public void move(float x, float y)
        {
            center.X += x;
            center.Y += y;
            p1.X += x;
            p1.Y += y;
            p2.X += x;
            p2.Y += y;
            p3.X += x;
            p3.Y += y;
            p4.X += x;
            p4.Y += y;
        }
        /// <summary>
        /// Sets PointFrame position around center point
        /// </summary>
        /// <param name="x">Center X position</param>
        /// <param name="y">Center Y position</param>
        public void set(float x, float y)
        {
            center.X = x;
            center.Y = y;
            p1.X = (float)(Math.Cos(Helpers<int>.degreesToRadians(p1a)) * p1d) + center.X;
            p1.Y = (float)(Math.Cos(Helpers<int>.degreesToRadians(p1a)) * p1d) + center.Y;
            p2.X = (float)(Math.Cos(Helpers<int>.degreesToRadians(p2a)) * p2d) + center.X;
            p2.Y = (float)(Math.Cos(Helpers<int>.degreesToRadians(p2a)) * p2d) + center.Y;
            p3.X = (float)(Math.Cos(Helpers<int>.degreesToRadians(p3a)) * p3d) + center.X;
            p3.Y = (float)(Math.Cos(Helpers<int>.degreesToRadians(p3a)) * p3d) + center.Y;
            p4.X = (float)(Math.Cos(Helpers<int>.degreesToRadians(p4a)) * p4d) + center.X;
            p4.Y = (float)(Math.Cos(Helpers<int>.degreesToRadians(p4a)) * p4d) + center.Y;
        }
        /// <summary>
        /// P1 property
        /// </summary>
        public Point2D P1
        {
            get { return p1; }
        }
        /// <summary>
        /// P1D property
        /// </summary>
        public float P1D
        {
            get { return p1d; }
        }
        /// <summary>
        /// P2 property
        /// </summary>
        public Point2D P2
        {
            get { return p2; }
        }
        /// <summary>
        /// P2D property
        /// </summary>
        public float P2D
        {
            get { return p2d; }
        }
        /// <summary>
        /// P3 property
        /// </summary>
        public Point2D P3
        {
            get { return p3; }
        }
        /// <summary>
        /// P3D property
        /// </summary>
        public float P3D
        {
            get { return p3d; }
        }
        /// <summary>
        /// P4 property
        /// </summary>
        public Point2D P4
        {
            get { return p4; }
        }
        /// <summary>
        /// P4D property
        /// </summary>
        public float P4D
        {
            get { return p4d; }
        }
        /// <summary>
        /// Center property
        /// </summary>
        public Point2D Center
        {
            get { return center; }
        }
        /// <summary>
        /// P1A property
        /// </summary>
        public double P1A
        {
            get { return p1a; }
        }
        /// <summary>
        /// P2A property
        /// </summary>
        public double P2A
        {
            get { return p2a; }
        }
        /// <summary>
        /// P3A property
        /// </summary>
        public double P3A
        {
            get { return p3a; }
        }
        /// <summary>
        /// P4A property
        /// </summary>
        public double P4A
        {
            get { return p4a; }
        }
        /// <summary>
        /// W property
        /// </summary>
        public float W
        {
            get { return w; }
        }
        /// <summary>
        /// H property
        /// </summary>
        public float H
        {
            get { return h; }
        }
    }
}
