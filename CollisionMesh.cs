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
    /// A mesh of points for testing collisions with other meshes
    /// </summary>
    public class CollisionMesh
    {
        //protected
        protected List<Point2D> mesh;
        protected Point2D center;
        protected int width;
        protected int height;
        protected int scale;
        protected float hypotenus;

        //public
        /// <summary>
        /// CollisionMesh constructor
        /// </summary>
        /// <param name="x">X center position</param>
        /// <param name="y">Y center position</param>
        /// <param name="width">Width in number of points</param>
        /// <param name="height">Height in number of points</param>
        /// <param name="scale">Space between points</param>
        public CollisionMesh(float x, float y, int width, int height, int scale)
        {
            mesh = new List<Point2D>();
            center = new Point2D(x, y);
            for(int mx = 0; mx < width; mx++)
            {
                for (int my = 0; my < height; my++)
                {
                    mesh.Add(new Point2D(x + (scale * mx) - (width / 2), y + (scale * my) - (height / 2)));
                }
            }
            this.width = width;
            this.height = height;
            this.scale = scale;
            hypotenus = (float)Math.Sqrt((scale * scale) + (scale * scale));
        }
        /// <summary>
        /// CollisionMesh copy constructor
        /// </summary>
        /// <param name="obj">CollisionMesh reference</param>
        public CollisionMesh(CollisionMesh obj)
        {
            mesh = new List<Point2D>();
            center = new Point2D(obj.Center.X, obj.Center.Y);
            for(int m = 0; m < obj.Mesh.Count; m++)
            {
                mesh.Add(new Point2D(obj.Mesh[m].X, obj.Mesh[m].Y));
            }
            width = obj.Width;
            height = obj.Height;
            scale = obj.Scale;
            hypotenus = obj.Hypotenus;
        }
        /// <summary>
        /// Check if a CollisionMesh is in range of any points within this CollsionMesh
        /// </summary>
        /// <param name="collider">CollsionMesh reference</param>
        /// <returns>Boolean</returns>
        public bool collide(CollisionMesh collider)
        {
            for(int m = 0; m < mesh.Count; m++)
            {
                for(int cm = 0; cm < collider.Mesh.Count; cm++)
                {
                    if(Point2D.AsqrtB(mesh[m], collider.Mesh[cm]) < hypotenus)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Rotates mesh around center
        /// </summary>
        /// <param name="angle">Amount of rotation (positive is clockwise)</param>
        public void rotateMesh(double angle)
        {
            double ang = 0;
            for (int m = 0; m < mesh.Count; m++)
            {
                ang = Helpers<double>.RadiansToDegrees(Point2D.CalcAngle(mesh[m], center));
                float dist = Point2D.calculateDistance(mesh[m], center);
                mesh[m].X = (float)(Math.Cos(Helpers<double>.degreesToRadians(angle + ang) * dist) + center.X);
                mesh[m].Y = (float)(Math.Sin(Helpers<double>.degreesToRadians(angle + ang) * dist) + center.Y);
            }
        }
        /// <summary>
        /// Move mesh by delta x and delta y
        /// </summary>
        /// <param name="dx">Delta x value</param>
        /// <param name="dy">Delta y value</param>
        public void moveMesh(float dx, float dy)
        {
            for(int m = 0; m < mesh.Count; m++)
            {
                mesh[m].X += dx;
                mesh[m].Y += dy;
            }
        }
        /// <summary>
        /// Move mesh in a direction by a speed
        /// </summary>
        /// <param name="angle">Angle of movement (0 to right, 90 down, 180 left, 270 up)</param>
        /// <param name="speed">Speed of change</param>
        public void moveMesh(double angle, float speed)
        {
            for (int m = 0; m < mesh.Count; m++)
            {
                mesh[m].X += (float)Math.Cos(Helpers<double>.degreesToRadians(angle) * speed);
                mesh[m].Y += (float)Math.Sin(Helpers<double>.degreesToRadians(angle) * speed);
            }
        }
        /// <summary>
        /// Set mesh at a specified position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void setMesh(float x, float y)
        {
            mesh = new List<Point2D>();
            center = new Point2D(x, y);
            for (int mx = 0; mx < width; mx++)
            {
                for (int my = 0; my < height; my++)
                {
                    mesh.Add(new Point2D(x + (scale * mx) - (width / 2), y + (scale * my) - (height / 2)));
                }
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
        /// Mesh property
        /// </summary>
        public List<Point2D> Mesh
        {
            get { return mesh; }
        }
        /// <summary>
        /// Width property
        /// </summary>
        public int Width
        {
            get { return width; }
        }
        /// <summary>
        /// Hegiht property
        /// </summary>
        public int Height
        {
            get { return height; }
        }
        /// <summary>
        /// Scale property
        /// </summary>
        public int Scale
        {
            get { return scale; }
        }
        /// <summary>
        /// Hypotenus proerty
        /// </summary>
        public float Hypotenus
        {
            get { return hypotenus; }
        }
    }
}
