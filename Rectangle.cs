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
    /// Rectangle class
    /// </summary>
    public class Rectangle
    {
        //protected
        float x;
        float y;
        float width;
        float height;

        //public
        /// <summary>
        /// Rectangle constructor
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public Rectangle(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
        /// <summary>
        /// Rectangle constructor
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public Rectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
        /// <summary>
        /// Rectangle copy constructor
        /// </summary>
        /// <param name="obj">Rectangle object reference</param>
        public Rectangle(Rectangle obj)
        {
            x = obj.X;
            y = obj.Y;
            width = obj.Width;
            height = obj.Height;
        }
        /// <summary>
        /// Set position of Rectangle
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        public void setPos(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// Set position of Rectangle
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        public void setPos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// Tests if rectangle B intersects with this Rectangle
        /// </summary>
        /// <param name="B">Rectangle to check against this Rectangle</param>
        /// <returns>Boolean</returns>
        public bool intersects(Rectangle B)
        {
            //the sides of the rectangles
            float leftA, leftB;
            float rightA, rightB;
            float topA, topB;
            float bottomA, bottomB;
            leftA = leftB = rightA = rightB = topA = topB = bottomA = bottomB = 0;

            //caluclate the sides of A
            leftA = x;
            rightA = x + width;
            topA = y;
            bottomA = y + height;

            //calculate the sides of B
            leftB = B.X;
            rightB = B.X + B.Width;
            topB = B.Y;
            bottomB = B.Y + B.Height;

            //if any of the sides of A are outside of B
            if (bottomA <= topB)
            {
                return false;
            }
            if (topA >= bottomB)
            {
                return false;
            }
            if (rightA <= leftB)
            {
                return false;
            }
            if (leftA >= rightB)
            {
                return false;
            }
            //if none of the sides of A are outside of B
            return true;
        }
        /// <summary>
        /// Moves Rectangle by specified values
        /// </summary>
        /// <param name="x">X shift value</param>
        /// <param name="y">Y shift value</param>
        public void move(float x, float y)
        {
            this.x += x;
            this.y += y;
        }
        /// <summary>
        /// Tests if a line intersects with Rectangle
        /// </summary>
        /// <param name="l">SimpleLine to test</param>
        /// <returns>Boolean</returns>
        public bool LineIntersects(SimpleLine l)
        {
            SimpleLine left = new SimpleLine(x, y, x, y + height);
            SimpleLine right = new SimpleLine(x + width, y, x + width, y + height);
            SimpleLine top = new SimpleLine(x, y, x + width, y);
            SimpleLine bottom = new SimpleLine(x, y + height, x + width, y + height);

            if(SimpleLine.isIntersect(left, l))
            {
                return true;
            }
            else if (SimpleLine.isIntersect(right, l))
            {
                return true;
            }
            else if (SimpleLine.isIntersect(top, l))
            {
                return true;
            }
            else if (SimpleLine.isIntersect(bottom, l))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if a point is within the Rectangle
        /// </summary>
        /// <param name="p">Point2D reference</param>
        /// <returns>Boolean</returns>
        public bool pointInRect(Point2D p)
        {
            if(p.X >= x && p.X <= x + width)
            {
                if (p.Y >= y && p.Y < y + height)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if a point is within the Rectangle
        /// </summary>
        /// <param name="px">X position</param>
        /// <param name="py">Y position</param>
        /// <returns>Boolean</returns>
        public bool pointInRect(float px, float py)
        {
            if (px >= x && px <= x + width)
            {
                if (py >= y && py < y + height)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Returns an SDL_Rect with the current state of this Rectangle
        /// </summary>
        public SDL.SDL_Rect Rect
        {
            get {
                    SDL.SDL_Rect rect = new SDL.SDL_Rect();
                    rect.x = (int)x;
                    rect.y = (int)y;
                    rect.w = (int)width;
                    rect.h = (int)height;
                    return rect;
                }
        }
        /// <summary>
        /// Returns a Point2D object representing current center of rectangle
        /// </summary>
        public Point2D Center
        {
            get { return new Point2D(x + width / 2, y + height / 2); }
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
        /// IX property
        /// </summary>
        public int IX
        {
            get { return (int)x; }
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
        /// IY property
        /// </summary>
        public int IY
        {
            get { return (int)y; }
            set { y = value; }
        }
        /// <summary>
        /// Width property
        /// </summary>
        public float Width
        {
            get { return width; }
            set { width = value; }
        }
        /// <summary>
        /// Height property
        /// </summary>
        public float Height
        {
            get { return height; }
            set { height = value; }
        }
    }
    /// <summary>
    /// SimpleLine class
    /// </summary>
    public class SimpleLine
    {
        //protected 
        protected Point2D a;
        protected Point2D b;

        //public
        /// <summary>
        /// SimpleLine constructor
        /// </summary>
        /// <param name="ax">Point A x value</param>
        /// <param name="ay">Point A y value</param>
        /// <param name="bx">Point B x value</param>
        /// <param name="by">Point B y value</param>
        public SimpleLine(float ax, float ay, float bx, float by)
        {
            a = new Point2D(ax, ay);
            b = new Point2D(bx, by);
        }
        /// <summary>
        /// SimpleLine copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public SimpleLine(SimpleLine obj)
        {
            a = new Point2D(obj.A.X, obj.A.Y);
            b = new Point2D(obj.B.X, obj.B.Y);
        }
        /// <summary>
        /// Tests if point is on line
        /// </summary>
        /// <param name="l1">Line 1</param>
        /// <param name="p">Point</param>
        /// <returns>Boolean</returns>
        public static bool onLine(SimpleLine l1, Point2D p)
        {   //check whether p is on the line or not
            if (p.X <= Math.Max(l1.A.X, l1.B.X) && p.X <= Math.Min(l1.A.X, l1.B.X) &&
               (p.Y <= Math.Max(l1.A.Y, l1.B.Y) && p.Y <= Math.Min(l1.A.Y, l1.B.Y)))
                return true;

            return false;
        }
        /// <summary>
        /// Gets direction
        /// </summary>
        /// <param name="a">Point a</param>
        /// <param name="b">Point b</param>
        /// <param name="c">Point c</param>
        /// <returns>int</returns>
        public static int direction(Point2D a, Point2D b, Point2D c)
        {
            int val = (int)((b.Y - a.Y) * (c.X - b.X) - (b.X - a.X) * (c.Y - b.Y));
            if (val == 0)
                return 0;     //colinear
            else if (val < 0)
                return 2;    //anti-clockwise direction
            return 1;    //clockwise direction
        }
        /// <summary>
        /// Tests if lines intersect
        /// </summary>
        /// <param name="l1">Line 1</param>
        /// <param name="l2">Line 2</param>
        /// <returns>Boolean</returns>
        public static bool isIntersect(SimpleLine l1, SimpleLine l2)
        {
            //four direction for two lines and points of other line
            int dir1 = direction(l1.A, l1.B, l2.A);
            int dir2 = direction(l1.A, l1.B, l2.B);
            int dir3 = direction(l2.A, l2.B, l1.A);
            int dir4 = direction(l2.A, l2.B, l1.B);

            if (dir1 != dir2 && dir3 != dir4)
                return true; //they are intersecting

            if (dir1 == 0 && onLine(l1, l2.A)) //when p2 of line2 are on the line1
                return true;

            if (dir2 == 0 && onLine(l1, l2.B)) //when p1 of line2 are on the line1
                return true;

            if (dir3 == 0 && onLine(l2, l1.A)) //when p2 of line1 are on the line2
                return true;

            if (dir4 == 0 && onLine(l2, l1.B)) //when p1 of line1 are on the line2
                return true;

            return false;
        }
        /// <summary>
        /// A property
        /// </summary>
        public Point2D A
        {
            get { return a; }
        }
        /// <summary>
        /// B property
        /// </summary>
        public Point2D B
        {
            get { return b; }
        }
    }
}
