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
    /// StickManBox collision framework
    /// </summary>
    public class StickManBox
    {
        //protected
        protected Rectangle head;
        protected float headDist;
        protected double headAngle;
        protected Rectangle leftHand;
        protected float leftHandDist;
        protected double leftHandAngle;
        protected Rectangle rightHand;
        protected float rightHandDist;
        protected double rightHandAngle;
        protected Rectangle leftFoot;
        protected float leftFootDist;
        protected double leftFootAngle;
        protected Rectangle rightFoot;
        protected float rightFootDist;
        protected double rightFootAngle;
        protected Point2D center;

        //public
        /// <summary>
        /// Basic StickManBox constructor
        /// </summary>
        /// <param name="x">Center X position</param>
        /// <param name="y">Center Y position</param>
        public StickManBox(float x, float y)
        {
            center = new Point2D(x, y);
            head = new Rectangle(x - 4, y - 24, 8, 8);
            headAngle = 270;
            headDist = 24;
            leftHand = new Rectangle(x - 16, y - 20, 4, 4);
            leftHandAngle = 180;
            leftHandDist = 12;
            rightHand = new Rectangle(x + 12, y - 20, 4, 4);
            rightHandAngle = 0;
            rightHandDist = 12;
            leftFoot = new Rectangle(x - 18, y + 18, 6, 6);
            leftFootAngle = Point2D.CalcAngle(center, new Point2D(x - 18, y + 18));
            leftFootDist = Point2D.calculateDistance(center, new Point2D(x - 18, y + 18));
            rightFoot = new Rectangle(x + 10, y + 18, 6, 6);
            rightFootAngle = Point2D.CalcAngle(center, new Point2D(x + 10, y + 18));
            rightFootDist = Point2D.calculateDistance(center, new Point2D(x + 10, y + 18));
        }
        /// <summary>
        /// StickManBox constructor
        /// </summary>
        /// <param name="x">Center X positon</param>
        /// <param name="y">Center Y position</param>
        /// <param name="headDist">Head distance</param>
        /// <param name="HandDist">Hand distance</param>
        /// <param name="legLength">Leg length</param>
        /// <param name="scale">Box width and height (head and others derived from head)</param>
        public StickManBox(float x, float y, float headDist, float HandDist, float legLength, float scale)
        {
            center = new Point2D(x, y);
            head = new Rectangle(x - (scale / 2), y - headDist, scale, scale);
            headAngle = 270;
            headDist = 24;
            leftHand = new Rectangle(x - HandDist, y - head.Height, scale / 2, scale / 2);
            leftHandAngle = 180;
            leftHandDist = 12;
            rightHand = new Rectangle(x + (HandDist - (scale / 2)), y - head.Height, scale / 2, scale / 2);
            rightHandAngle = 0;
            rightHandDist = 12;
            leftFoot = new Rectangle(x - HandDist, y + legLength, 6, 6);
            Point2D lft = new Point2D(x - HandDist, y + legLength);
            leftFootAngle = Point2D.CalcAngle(center, lft);
            leftFootDist = Point2D.calculateDistance(center, lft);
            Point2D rft = new Point2D(x + (HandDist - (scale * 0.75f)), y + legLength);
            rightFoot = new Rectangle(x + (HandDist - (scale * 0.75f)), y + legLength, 6, 6);
            rightFootAngle = Point2D.CalcAngle(center, rft);
            rightFootDist = Point2D.calculateDistance(center, rft);
        }
        /// <summary>
        /// StickManBox copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public StickManBox(StickManBox obj)
        {
            center = new Point2D(obj.Center.X, obj.Center.Y);
            head = new Rectangle(obj.Head.X, obj.Head.Y, obj.Head.Width, obj.Head.Height);
            headAngle = obj.HeadAngle;
            headDist = obj.headDist;
            leftHand = new Rectangle(obj.LeftHand.X, obj.LeftHand.Y, obj.LeftHand.Width, obj.LeftHand.Height);
            leftHandAngle = obj.LeftHandAngle;
            leftHandDist = obj.LeftHandDist;
            rightHand = new Rectangle(obj.RightHand.X, obj.RightHand.Y, obj.RightHand.Width, obj.RightHand.Height);
            rightHandAngle = obj.rightHandAngle;
            rightHandDist = obj.RightHandDist;
            leftFoot = new Rectangle(obj.LeftFoot.X, obj.LeftFoot.Y, obj.LeftFoot.Width, obj.LeftFoot.Height);
            leftFootAngle = obj.LeftFootAngle;
            leftFootDist = obj.LeftFootDist;
            rightFoot = new Rectangle(obj.RightFoot.X, obj.RightFoot.Y, obj.RightFoot.Width, obj.RightFoot.Height);
            rightFootAngle = obj.RightFootAngle;
            rightFootDist = obj.RightFootDist;
        }
        /// <summary>
        /// From file StickManBox constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public StickManBox(StreamReader sr)
        {
            sr.ReadLine();//discard testing data
            center = new Point2D((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()));
            head = new Rectangle((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()));
            headAngle = (double)Convert.ToDecimal(sr.ReadLine());
            headDist = (float)Convert.ToDecimal(sr.ReadLine());
            leftHand = new Rectangle((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()));
            leftHandAngle = (double)Convert.ToDecimal(sr.ReadLine());
            leftHandDist = (float)Convert.ToDecimal(sr.ReadLine());
            rightHand = new Rectangle((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()));
            rightHandAngle = (double)Convert.ToDecimal(sr.ReadLine());
            rightHandDist = (float)Convert.ToDecimal(sr.ReadLine());
            leftFoot = new Rectangle((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()));
            leftFootAngle = (double)Convert.ToDecimal(sr.ReadLine());
            leftFootDist = (float)Convert.ToDecimal(sr.ReadLine());
            rightFoot = new Rectangle((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()));
            rightFootAngle = (double)Convert.ToDecimal(sr.ReadLine());
            rightFootDist = (float)Convert.ToDecimal(sr.ReadLine());
        }
        /// <summary>
        /// Save StickManBox data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveData(StreamWriter sw)
        {
            sw.WriteLine("======StickManBox Data======");
            sw.WriteLine(center.X);
            sw.WriteLine(center.Y);
            sw.WriteLine(head.X);
            sw.WriteLine(head.Y);
            sw.WriteLine(head.Width);
            sw.WriteLine(head.Height);
            sw.WriteLine(headAngle);
            sw.WriteLine(headDist);
            sw.WriteLine(leftHand.X);
            sw.WriteLine(leftHand.Y);
            sw.WriteLine(leftHand.Width);
            sw.WriteLine(leftHand.Height);
            sw.WriteLine(leftHandAngle);
            sw.WriteLine(leftHandDist);
            sw.WriteLine(rightHand.X);
            sw.WriteLine(rightHand.Y);
            sw.WriteLine(rightHand.Width);
            sw.WriteLine(rightHand.Height);
            sw.WriteLine(rightHandAngle);
            sw.WriteLine(rightHandDist);
            sw.WriteLine(LeftFoot.X);
            sw.WriteLine(LeftFoot.Y);
            sw.WriteLine(LeftFoot.Width);
            sw.WriteLine(LeftFoot.Height);
            sw.WriteLine(LeftFootAngle);
            sw.WriteLine(LeftFootDist);
            sw.WriteLine(rightFoot.X);
            sw.WriteLine(rightFoot.Y);
            sw.WriteLine(rightFoot.Width);
            sw.WriteLine(rightFoot.Height);
            sw.WriteLine(rightFootAngle);
            sw.WriteLine(rightFootDist);
        }
        /// <summary>
        /// Set StickManBox center and all relative boxes
        /// </summary>
        /// <param name="x">Center X position</param>
        /// <param name="y">Center Y position</param>
        public void set(float x, float y)
        {
            center.X = x;
            center.Y = y;
            head.X = x - (head.Width / 2);
            head.Y = y - headDist;
            leftHand.X = x - LeftHandDist;
            leftHand.Y = y - head.Height;
            rightHand.X = x + rightHandDist;
            rightHand.Y = y - head.Height;
            leftFoot.X = x - LeftHandDist;
            leftFoot.Y = y + LeftFootDist;
            rightFoot.X = x + rightHandDist;
            rightFoot.Y = y + rightFootDist;
        }
        /// <summary>
        /// Move StickManBox in direction 
        /// </summary>
        /// <param name="angle">Angle of directon (in degrees)</param>
        /// <param name="speed">Speed of movement</param>
        public void move(double angle, float speed)
        {
            float dx = (float)(Math.Cos(Helpers<int>.degreesToRadians(angle)) * speed);
            float dy = (float)(Math.Sin(Helpers<int>.degreesToRadians(angle)) * speed);
            head.X += dx;
            head.Y += dy;
            leftHand.X += dx;
            leftHand.Y += dy;
            rightHand.X += dx;
            rightHand.Y += dy;
            leftFoot.X += dx;
            leftFoot.Y += dy;
            rightFoot.X += dx;
            rightFoot.Y += dy;
            center.X += dx;
            center.Y += dy;
        }
        /// <summary>
        /// Move StickManBox by delta x and y
        /// </summary>
        /// <param name="x">Delta X value</param>
        /// <param name="y">Delta Y value</param>
        public void move(float x, float y)
        {
            head.X += x;
            head.Y += y;
            leftHand.X += x;
            leftHand.Y += y;
            rightHand.X += x;
            rightHand.Y += y;
            leftFoot.X += x;
            leftFoot.Y += y;
            rightFoot.X += x;
            rightFoot.Y += y;
            center.X += x;
            center.Y += y;
        }
        /// <summary>
        /// Rotate all boxes around the center of StickManBox
        /// </summary>
        /// <param name="angle">Rotation amout (in degrees)</param>
        public void rotate(double angle)
        {
            head.X = (float)(Math.Cos(Helpers<int>.degreesToRadians(angle + headAngle) * headDist) + center.X);
            head.Y = (float)(Math.Sin(Helpers<int>.degreesToRadians(angle + headAngle) * headDist) + center.Y);
            leftHand.X = (float)(Math.Cos(Helpers<int>.degreesToRadians(angle + leftHandAngle) * leftHandDist) + center.X);
            leftHand.Y = (float)(Math.Sin(Helpers<int>.degreesToRadians(angle + leftHandAngle) * leftHandDist) + center.Y);
            rightHand.X = (float)(Math.Cos(Helpers<int>.degreesToRadians(angle + rightHandAngle) * rightHandDist) + center.X);
            rightHand.Y = (float)(Math.Sin(Helpers<int>.degreesToRadians(angle + rightHandAngle) * rightHandDist) + center.Y);
            leftFoot.X = (float)(Math.Cos(Helpers<int>.degreesToRadians(angle + leftFootAngle) * leftFootDist) + center.X);
            leftFoot.Y = (float)(Math.Sin(Helpers<int>.degreesToRadians(angle + leftFootAngle) * leftFootDist) + center.Y);
            rightFoot.X = (float)(Math.Cos(Helpers<int>.degreesToRadians(angle + rightFootAngle) * rightFootDist) + center.X);
            rightFoot.Y = (float)(Math.Sin(Helpers<int>.degreesToRadians(angle + rightFootAngle) * rightFootDist) + center.Y);
        }
        /// <summary>
        /// Check if a Rectangle collides with any internal boxes
        /// </summary>
        /// <param name="rect">Rectangle reference</param>
        /// <returns>Boolean</returns>
        public bool collide(Rectangle rect)
        {
            if(head.intersects(rect) == true)
            {
                return true;
            }
            if (leftHand.intersects(rect) == true)
            {
                return true;
            }
            if (rightHand.intersects(rect) == true)
            {
                return true;
            }
            if (leftFoot.intersects(rect) == true)
            {
                return true;
            }
            if (rightFoot.intersects(rect) == true)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if a StickManBox collides with any internal boxes
        /// </summary>
        /// <param name="stickMan">StickManBox reference</param>
        /// <returns>Boolean</returns>
        public bool collide(StickManBox stickMan)
        {
            //check head
            if (head.intersects(stickMan.Head) == true)
            {
                return true;
            }
            if (head.intersects(stickMan.LeftHand) == true)
            {
                return true;
            }
            if (head.intersects(stickMan.RightHand) == true)
            {
                return true;
            }
            if (head.intersects(stickMan.LeftFoot) == true)
            {
                return true;
            }
            if (head.intersects(stickMan.RightFoot) == true)
            {
                return true;
            }
            //check left hand
            if (leftHand.intersects(stickMan.Head) == true)
            {
                return true;
            }
            if (leftHand.intersects(stickMan.LeftHand) == true)
            {
                return true;
            }
            if (leftHand.intersects(stickMan.RightHand) == true)
            {
                return true;
            }
            if (leftHand.intersects(stickMan.LeftFoot) == true)
            {
                return true;
            }
            if (leftHand.intersects(stickMan.RightFoot) == true)
            {
                return true;
            }
            //check right hand
            if (rightHand.intersects(stickMan.Head) == true)
            {
                return true;
            }
            if (rightHand.intersects(stickMan.LeftHand) == true)
            {
                return true;
            }
            if (rightHand.intersects(stickMan.RightHand) == true)
            {
                return true;
            }
            if (rightHand.intersects(stickMan.LeftFoot) == true)
            {
                return true;
            }
            if (rightHand.intersects(stickMan.RightFoot) == true)
            {
                return true;
            }
            //check left foot
            if (leftFoot.intersects(stickMan.Head) == true)
            {
                return true;
            }
            if (leftFoot.intersects(stickMan.LeftHand) == true)
            {
                return true;
            }
            if (leftFoot.intersects(stickMan.RightHand) == true)
            {
                return true;
            }
            if (leftFoot.intersects(stickMan.LeftFoot) == true)
            {
                return true;
            }
            if (leftFoot.intersects(stickMan.RightFoot) == true)
            {
                return true;
            }
            //check right foot
            if (rightFoot.intersects(stickMan.Head) == true)
            {
                return true;
            }
            if (rightFoot.intersects(stickMan.LeftHand) == true)
            {
                return true;
            }
            if (rightFoot.intersects(stickMan.RightHand) == true)
            {
                return true;
            }
            if (rightFoot.intersects(stickMan.LeftFoot) == true)
            {
                return true;
            }
            if (rightFoot.intersects(stickMan.RightFoot) == true)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Head property
        /// </summary>
        public Rectangle Head
        {
            get { return head; }
        }
        /// <summary>
        /// HeadDist property
        /// </summary>
        public float HeadDist
        {
            get { return headDist; }
        }
        /// <summary>
        /// HeadAngle property
        /// </summary>
        public double HeadAngle
        {
            get { return headAngle; }
        }
        /// <summary>
        /// LeftHand property
        /// </summary>
        public Rectangle LeftHand
        {
            get { return leftHand; }
        }
        /// <summary>
        /// LeftHandDist property
        /// </summary>
        public float LeftHandDist
        {
            get { return leftHandDist; }
        }
        /// <summary>
        /// LeftHandAngle property
        /// </summary>
        public double LeftHandAngle
        {
            get { return leftHandAngle; }
        }
        /// <summary>
        /// RightHand property
        /// </summary>
        public Rectangle RightHand
        {
            get { return rightHand; }
        }
        /// <summary>
        /// RightHandDist property
        /// </summary>
        public float RightHandDist
        {
            get { return rightHandDist; }
        }
        /// <summary>
        /// RightHandAngle property
        /// </summary>
        public double RightHandAngle
        {
            get { return rightHandAngle; }
        }
        /// <summary>
        /// LeftFoot property
        /// </summary>
        public Rectangle LeftFoot
        {
            get { return leftFoot; }
        }
        /// <summary>
        /// LeftFootDist property
        /// </summary>
        public float LeftFootDist
        {
            get { return leftFootDist; }
        }
        /// <summary>
        /// LeftFootAngle property
        /// </summary>
        public double LeftFootAngle
        {
            get { return leftFootAngle; }
        }
        /// <summary>
        /// RightFoot property
        /// </summary>
        public Rectangle RightFoot
        {
            get { return rightFoot; }
        }
        /// <summary>
        /// RightFootDist property
        /// </summary>
        public float RightFootDist
        {
            get { return rightFootDist; }
        }
        /// <summary>
        /// RightFootAngle property
        /// </summary>
        public double RightFootAngle
        {
            get { return rightFootAngle; }
        }
        /// <summary>
        /// Center property
        /// </summary>
        public Point2D Center
        {
            get { return center; }
        }
    }
}
