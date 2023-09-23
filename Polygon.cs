//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

namespace XenoLib
{
    /// <summary>
    /// Projection class used by Polygon class
    /// </summary>
    public class Projection
    {
        //public
        public float max;
        public float min;
        /// <summary>
        /// Projection constructor
        /// </summary>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        public Projection(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
    /// <summary>
    /// Polygon class
    /// </summary>
    public class Polygon
    {
        //protected
        protected Point2D[] vertices;

        //public
        /// <summary>
        /// Polygon constructor
        /// </summary>
        /// <param name="vs">Array of Point2D objects</param>
        public Polygon(Point2D[] vs)
        {
            vertices = new Point2D[vs.Length];
            for (int i = 0; i < vs.Length; i++)
            {
                vertices[i] = new Point2D(vs[i].X, vs[i].Y);
            }
        }
        /// <summary>
        /// Calculates the edges of Polygon
        /// </summary>
        /// <returns>Arrary of Point2D objects</returns>
        public Point2D[] calculateEdges()
        {
            Point2D[] edges = new Point2D[vertices.Length];
            Point2D temp = new Point2D(vertices[0].X - vertices[1].X, vertices[0].Y - vertices[1].Y);
            edges[0] = perpendicularize(temp);
            for (int i = 1; i < vertices.Length; i++)
            {
                if (i == vertices.Length - 1)
                {
                    temp = new Point2D(vertices[i].X - vertices[0].X, vertices[i].Y - vertices[0].Y);
                    edges[i] = perpendicularize(temp);
                }
                else
                {
                    temp = new Point2D(vertices[i].X - vertices[0].X, vertices[i].Y - vertices[0].Y);
                    edges[i] = perpendicularize(temp);
                }
            }
            return edges;
        }
        /// <summary>
        /// Calculate a projection
        /// </summary>
        /// <param name="axis">Axis of projection</param>
        /// <param name="p">Point of projection</param>
        /// <returns>Projection</returns>
        protected static Projection project(Point2D axis, Polygon p)
        {
            float min = Point2D.dotProduct(axis, p.Vertices[0]); ;
            
            float max = min;
            for (int i = 1; i < p.Vertices.Length; i++)
            {
                float temp = Point2D.dotProduct(axis, p.Vertices[i]);
                
                if (temp < min)
                {
                    min = temp;
                }
                else if (temp > max)
                {
                    max = temp;
                }
            }
            return new Projection(min, max);
        }
        /// <summary>
        /// Calculates if two projections overlap
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        protected static bool overlaps(Projection p1, Projection p2)
        {
            if (p1.min == p2.min | p1.max == p2.max)
            {
                return true;
            }
            else if(p1.min >= p2.min & p1.min <= p2.max)
            {
                return true;
            }
            else if (p1.max >= p2.min & p1.max <= p2.max)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Calculate if two Polygons collide
        /// </summary>
        /// <param name="p1">Polygon 1</param>
        /// <param name="p2">Polygon 2</param>
        /// <returns>Boolean</returns>
        public static bool collide(Polygon p1, Polygon p2)
        {
            Point2D[] edges1 = p1.calculateEdges();
            Point2D[] edges2 = p2.calculateEdges();
            for (int i = 0; i < edges1.Length; i++)
            {
                Projection proj1 = Polygon.project(edges1[i], p1);
                Projection proj2 = Polygon.project(edges1[i], p2);
                if (Polygon.overlaps(proj1, proj2) == false)
                {
                    return false;
                }
            }
            for (int i = 0; i < edges2.Length; i++)
            {
                Projection proj1 = Polygon.project(edges2[i], p1);
                Projection proj2 = Polygon.project(edges2[i], p2);
                if (Polygon.overlaps(proj1, proj2) == false)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Move Polygon by an X and Y value 
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        public void move(float x, float y)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].X += x;
                vertices[i].Y += y;
            }
        }
        /// <summary>
        /// Calculate the Polygon centoid
        /// </summary>
        /// <returns>Point2D</returns>
        public Point2D Centoid()
        {
            float cx = 0;
            float cy = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                cx += vertices[i].X;
                cy += vertices[i].Y;
            }
            cx /= vertices.Length;
            cy /= vertices.Length;
            return new Point2D(cx, cy);
        }
        /// <summary>
        /// Rotate Polygon around centoid
        /// </summary>
        /// <param name="angle">Angle to set to</param>
        public void rotate(double angle)
        {
            Point2D center = Centoid();
            for (int i = 0; i < vertices.Length; i++)
            {
                int x = (int)(Math.Cos(angle) * (vertices[i].X - center.X) - Math.Sin(angle) * (vertices[i].Y - center.Y) + center.X);
                int y = (int)(Math.Sin(angle) * (vertices[i].X - center.X) + Math.Cos(angle) * (vertices[i].Y - center.Y) + center.Y);
                vertices[i].X = x;
                vertices[i].Y = y;
            }
        }
        /// <summary>
        /// Rotate Polygon around a point
        /// </summary>
        /// <param name="angle">Angle to set to</param>
        /// <param name="cent">Point to rotate around</param>
        public void rotate(double angle, Point2D cent)
        {
            Point2D center = cent;
            for (int i = 0; i < vertices.Length; i++)
            {
                int x = (int)(Math.Cos(angle) * (vertices[i].X - center.X) - Math.Sin(angle) * (vertices[i].Y - center.Y) + center.X);
                int y = (int)(Math.Sin(angle) * (vertices[i].X - center.X) + Math.Cos(angle) * (vertices[i].Y - center.Y) + center.Y);
                vertices[i].X = x;
                vertices[i].Y = y;
            }
        }
        /// <summary>
        /// Perpendicularize Polygon axis
        /// </summary>
        /// <param name="v">Axis to perpendicularize</param>
        /// <returns>Point2D</returns>
        protected Point2D perpendicularize(Point2D v)
        {
            float x = v.X;
            float y = v.Y;
            x *= -1;
            return new Point2D(y, x);
        }
        /// <summary>
        /// draws lines of a polygon, requires a texture bank with an acessible texture
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="winx"></param>
        /// <param name="winy"></param>
        /// <param name="pixel"></param>
        public void draw(IntPtr renderer, int winx, int winy, SDL.SDL_Color c)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                if (i + 1 == vertices.Length)
                {
                    DrawLine.draw(renderer, vertices[i], vertices[0], c, winx, winy);
                }
                else
                {
                    DrawLine.draw(renderer, vertices[i], vertices[i + 1], c, winx, winy);
                }
            }
        }
        /// <summary>
        /// Verticies property
        /// </summary>
        public Point2D[] Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }
    }
}
