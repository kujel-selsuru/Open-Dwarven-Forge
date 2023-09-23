//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Xna.Framework;

namespace XenoLib
{
    /// <summary>
    /// Point2D class
    /// </summary>
    public class Point2D
    {
        //protected
        protected float x, y;
        //public
        /// <summary>
        /// Point2D constructor
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        public Point2D(float x = 0, float y = 0)
        {
            this.x = x;
            this.y = y; 
        }
        /// <summary>
        /// Point2D copy constructor
        /// </summary>
        /// <param name="obj">Point2D reference</param>
        public Point2D(Point2D obj)
        {
            this.x = obj.X;
            this.y = obj.Y;
        }
        /// <summary>
        /// X property
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        /// <summary>
        /// Y property
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        /// <summary>
        /// IX property
        /// </summary>
        public int IX
        {
            get { return (int)x; }
            set { x = value; }
        }
        /// <summary>
        /// IY property
        /// </summary>
        public int IY
        {
            get { return (int)y; }
            set { y = value; }
        }
        /// <summary>
        /// Returns the cross product of this Point2D and another Point2D
        /// </summary>
        /// <param name="p">Point2D reference</param>
        /// <returns>Float</returns>
        public float cross(Point2D p)
        {
            return (x * p.Y) + (y * p.X);
        }
        /// <summary>
        /// Returns length of Point2D
        /// </summary>
        /// <returns>Float</returns>
        public float length()
        {
            float tmp = (x * x) + (y * y);
            return (float)Math.Sqrt(tmp);
        }
        /// <summary>
        /// Returns length squared
        /// </summary>
        /// <returns>Float</returns>
        public float lengthSquared()
        {
            return (x * x) + (y * y);
        }
        /// <summary>
        /// Normal Point2D
        /// </summary>
        /// <returns>Point2D reference</returns>
        public Point2D normal()
        {
            float tmp = x;
            x = -y;
            y = x;
            return this;
        }
        /// <summary>
        /// Normalize Point2D
        /// </summary>
        /// <returns></returns>
        public Point2D normalize()
        {
            if(length() != 0)
            {
                float len = lengthSquared();
                x /= len;
                y /= len;
                return this;
            }
            x = y = 0;
            return this;
        }
        /// <summary>
        /// Calculate in pixels the distnece between to points
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>Int</returns>
        public static int calculateDistance(Point2D p1, Point2D p2)
        {
            return (int)Math.Sqrt(AsqrtB(p1, p2));
        }
        /// <summary>
        /// Calculate the A^2 + B^2 between two points
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>Float</returns>
        public static float AsqrtB(Point2D p1, Point2D p2)
        {
            float tempx = Math.Abs(p1.X - p2.X);
            float tempy = Math.Abs(p1.Y - p2.Y);
            return (int)(Math.Pow(tempx, 2) + Math.Pow(tempy, 2));//tempx^2 + tempy^2;
        }
        /// <summary>
        /// returns the angle between to points (in degrees)
        /// </summary>
        /// <param name="p1">point 1</param>
        /// <param name="p2">point 2</param>
        /// <returns>Degrees as double</returns>
        public static double CalcAngle(Point2D p1, Point2D p2)
        {
            double diffx; 
            double diffy; 

            diffx = p2.X - p1.X;
            diffy = p2.Y - p1.Y;
            
            double result = Math.Atan2(diffy, diffx) * (180 / Math.PI);
            if (p2.Y < p1.Y)
            {
                result = 360 + result;
            }
            return result;
            //Point2D temp = new Point2D(p2.y - p1.y, p2.x - p1.x);
            //double angle = Math.Atan2((double)temp.X, (double)temp.Y);// *(180 / Math.PI);
            //return angle;
        }
        /// <summary>
        /// Depricated (limited range of angles return)
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>double</returns>
        public static double findAngle(Point2D p1, Point2D p2)
        {
            if (p1.X == p2.X)//above or below
            {
                if (p1.Y > p2.Y)//above
                {
                    return 0;
                }
                else//below
                {
                    return 180;
                }
            }
            else if (p1.Y == p2.Y)//left or right
            {
                if (p1.X > p2.X)//left
                {
                    return 270;
                }
                else//right
                {
                    return 90;
                }
            }
            if (p1.X > p2.X)//left
            {
                if (p1.Y > p2.Y)//bottom
                {
                    return 225;
                }
                else//top
                {
                    return 315;
                }
            }
            else
            {
                if (p1.Y > p2.Y)//bottom
                {
                    return 135;
                }
                else//top
                {
                    return 45;
                }
            }
        }
        /// <summary>
        /// Calculates the Dot product of two points
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>Int</returns>
        public static float dotProduct(Point2D p1, Point2D p2)
        {
            return (p1.X * p2.X) + (p1.Y * p2.Y);
        }
        /// <summary>
        /// Add two Point2D objects and return a new Point2D object as result
        /// </summary>
        /// <param name="a">Point2D a</param>
        /// <param name="b">Point2D b</param>
        /// <returns>Point2D</returns>
        public static Point2D add(Point2D a, Point2D b)
        {
            Point2D c = new Point2D();
            c.X = a.X + b.X;
            c.Y = a.Y + b.Y;
            return c;
        }
        /// <summary>
        /// Subtract two Point2D objects and return a new Point2D object as result
        /// </summary>
        /// <param name="a">Point2D a</param>
        /// <param name="b">Point2D b</param>
        /// <returns>Point2D</returns>
        public static Point2D subtract(Point2D a, Point2D b)
        {
            Point2D c = new Point2D();
            c.X = a.X - b.X;
            c.Y = a.Y - b.Y;
            return c;
        }
        /// <summary>
        /// Multiply two Point2D objects and return a new Point2D object as result
        /// </summary>
        /// <param name="a">Point2D a</param>
        /// <param name="b">Point2D b</param>
        /// <returns>Point2D</returns>
        public static Point2D multiply(Point2D a, Point2D b)
        {
            Point2D c = new Point2D();
            c.X = a.X * b.X;
            c.Y = a.Y * b.Y;
            return c;
        }
        /// <summary>
        /// Divide two Point2D objects and return a new Point2D object as result
        /// </summary>
        /// <param name="a">Point2D a</param>
        /// <param name="b">Point2D b</param>
        /// <returns>Point2D</returns>
        public static Point2D divide(Point2D a, Point2D b)
        {
            Point2D c = new Point2D();
            c.X = a.X / b.X;
            c.Y = a.Y / b.Y;
            return c;
        }
        /// <summary>
        /// Calculate the quadrent in which a relative point is
        /// </summary>
        /// <param name="center">Center point</param>
        /// <param name="target">Target center</param>
        /// <returns>quad enum</returns>
        public static quad quadrent(Point2D center, Point2D target)
        {
            if(target.IX > center.IX)//right side
            {
                if(target.IY > center.IY)//bottom right
                {
                    return quad.II;
                }
                else//top right
                {
                    return quad.I;
                }
            }
            else
            {
                if (target.IY > center.IY)//bottom left
                {
                    return quad.III;
                }
                else//top left
                {
                    return quad.IV;
                }
            }
        }
    }
    /// <summary>
    /// Quadrent enumeration
    /// </summary>
    public enum quad { I, II, III, IV }
    /// <summary>
    /// Alternate Point class (use Point2D)
    /// </summary>
    public class Point2
    {
        //protected
        protected float x, y;
        //public
        /// <summary>
        /// Point2 constructor
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        public Point2(float x = 0, float y = 0)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// X property
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        /// <summary>
        /// Y property
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        /// <summary>
        /// Calculates the distence between two points in pixels
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>Float</returns>
        public static float calculateDistance(Point2 p1, Point2 p2)
        {
            return (float)Math.Sqrt(AsqrtB(p1, p2));
        }
        /// <summary>
        /// Calculates which quadrent point 2 is relative point 1
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>quad enum</returns>
        public static quad quadrent(Point2 p1, Point2 p2)
        {
            if (p1.X == p2.X)
            {
                if (p1.Y > p2.Y)
                {
                    return quad.I;
                }
                else
                {
                    return quad.IV;
                }
            }
            if (p1.Y == p2.Y)
            {
                if (p1.X > p2.X)
                {
                    return quad.IV;
                }
                else
                {
                    return quad.III;
                }
            }
            if (p1.Y > p2.Y)//upper half
            {
                if (p1.X > p2.X)//left
                {
                    return quad.II;
                }
                else
                {
                    return quad.I;
                }
            }
            else//lower half
            {
                if (p1.X > p2.X)//left
                {
                    return quad.III;
                }
                else
                {
                    return quad.IV;
                }
            }
        }
        /// <summary>
        /// Calulates A^2 + B^2 in pixels between two points
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>Float</returns>
        public static float AsqrtB(Point2 p1, Point2 p2)
        {
            float tempx = Math.Abs(p1.X - p2.X);
            float tempy = Math.Abs(p1.Y - p2.Y);
            return (float)(Math.Pow(tempx, 2) + Math.Pow(tempy, 2));
        }
        /// <summary>
        /// Returns the angle between to points
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>Degrees as double</returns>
        public static double CalcAngle(Point2 p1, Point2 p2)
        {
            Point2 temp = new Point2(p2.y - p1.y, p2.x - p1.x);
            double angle = Math.Atan2((double)temp.X, (double)temp.Y);

            return angle;// - Helpers.degreesToRadians(90);
        }
        /// <summary>
        /// Deprecated (calculates the angle between two points in pixels)
        /// Limited range of angles returned
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>Double</returns>
        public static double findAngle(Point2 p1, Point2 p2)
        {
            if (p1.X == p2.X)//above or below
            {
                if (p1.Y > p2.Y)//above
                {
                    return 0;
                }
                else//below
                {
                    return 180;
                }
            }
            else if (p1.Y == p2.Y)//left or right
            {
                if (p1.X > p2.X)//left
                {
                    return 270;
                }
                else//right
                {
                    return 90;
                }
            }
            if (p1.X > p2.X)//left
            {
                if (p1.Y > p2.Y)//bottom
                {
                    return 225;
                }
                else//top
                {
                    return 315;
                }
            }
            else
            {
                if (p1.Y > p2.Y)//bottom
                {
                    return 135;
                }
                else//top
                {
                    return 45;
                }
            }
        }
        /// <summary>
        /// Calculates the Dot product between two points
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>Float</returns>
        public static float dotProduct(Point2 p1, Point2 p2)
        {
            return (p1.X * p2.X) + (p1.Y * p2.Y);
        }
    }
}
