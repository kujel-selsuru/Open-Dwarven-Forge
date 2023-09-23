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
    public class Polygon2
    {
        protected Point2D[] vertices;
        public static Point2D mtv;

        public Polygon2(Point2D[] vs)
        {
            vertices = new Point2D[vs.Length];
            for (int i = 0; i < vs.Length; i++)
            {
                vertices[i] = new Point2D(vs[i].X, vs[i].Y);
            }
        }
        public Polygon2()
        {

        }
        public Point2D[] calculateEdges()
        {
            Point2D[] edges = new Point2D[vertices.Length];
            Point2D temp = Point2D.subtract(vertices[0], vertices[1]);
            edges[0] = perpendicularize(temp);
            for (int i = 1; i < vertices.Length; i++)
            {
                if (i == vertices.Length - 1)
                {
                    temp = Point2D.subtract(vertices[i], vertices[0]);
                    edges[i] = perpendicularize(temp);
                }
                else
                {
                    temp = Point2D.subtract(vertices[i], vertices[i + 1]);
                    edges[i] = perpendicularize(temp);
                }
            }
            return edges;
        }
        public static Projection project(Point2D axis, Polygon2 p)
        {
            float min;
            min = Point2D.dotProduct(axis, p.Vertices[0]);
            float max = min;
            for (int i = 1; i < p.Vertices.Length; i++)
            {
                float temp;
                temp = Point2D.dotProduct(axis, p.Vertices[i]);
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
        public static bool overlaps(Projection p1, Projection p2)
        {
            if (p1.min == p2.min | p1.max == p2.max)
            {
                return true;
            }
            else if (p1.min >= p2.min & p1.min <= p2.max)
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
        //returns a negative number if overlaps else a positive
        public static float intervalDistance(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }
            else
            {
                return minA - maxB;
            }
        }
        public static bool collide(Polygon2 p1, Polygon2 p2)
        {
            Point2D[] edges1 = p1.calculateEdges();
            Point2D[] edges2 = p2.calculateEdges();
            Point2D smallest = default(Point2D);
            float o = 999999999999;
            for (int i = 0; i < edges1.Length; i++)
            {
                Projection proj1 = Polygon2.project(edges1[i], p1);
                Projection proj2 = Polygon2.project(edges1[i], p2);
                //less then 0 is a collision
                float temp = intervalDistance(proj1.min, proj1.max, proj2.min, proj2.max);
                if (temp > 0)
                {
                    return false;
                }
                else
                {
                    if (temp < o)
                    {
                        smallest = edges1[i];
                        o = temp;
                    }
                }
                /*
                if (Polygon.overlaps(proj1, proj2) == false)
                {
                    return false;
                }
                 */
            }
            for (int i = 0; i < edges2.Length; i++)
            {
                Projection proj1 = Polygon2.project(edges2[i], p1);
                Projection proj2 = Polygon2.project(edges2[i], p2);
                float temp = intervalDistance(proj1.min, proj1.max, proj2.min, proj2.max);
                if (temp > 0)
                {
                    return false;
                }
                else
                {
                    if (temp < o)
                    {
                        smallest = edges2[i];
                        o = temp;
                    }
                }
                /*
                if (Polygon.overlaps(proj1, proj2) == false)
                {
                    return false;
                }
                 */
            }
            mtv = smallest;//smallest * o;
            return true;
        }
        public void move(float x, float y)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].X += x;
                vertices[i].Y += y;
            }
        }
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
        public static Point2D perpendicularize(Point2D v)
        {
            float x = v.X;
            float y = v.Y;
            y *= -1;
            return new Point2D(y, x);
        }
        /// <summary>
        /// draws lines of a polygon
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="winx"></param>
        /// <param name="winy"></param>
        /// <param name="c"></param>
        public void draw(IntPtr renderer, int winx, int winy, SDL.SDL_Color c)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                if (i + 1 == vertices.Length)
                {
                    DrawLine.draw(renderer, new Point2D(vertices[i].X - winx, vertices[i].Y - winy), new Point2D(vertices[0].X - winx, vertices[0].Y - winy), c);
                    //GLIB.BasicLine.drawLineSegment(sb, GLIB.TextureBank.getTexture(pixel), new Vector2(vertices[i].X - winx, vertices[i].Y - winy), new Vector2(vertices[0].X - winx, vertices[0].Y - winy), c, 2);
                }
                else
                {
                    DrawLine.draw(renderer, new Point2D(vertices[i].X - winx, vertices[i].Y - winy), new Point2D(vertices[i + 1].X - winx, vertices[i + 1].Y - winy), c);
                    //GLIB.BasicLine.drawLineSegment(sb, GLIB.TextureBank.getTexture(pixel), new Vector2(vertices[i].X - winx, vertices[i].Y - winy), new Vector2(vertices[i + 1].X - winx, vertices[i + 1].Y - winy), c, 2);
                }
            }
        }
        public Point2D[] Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }
        public static Point2D MTV
        {
            get { return mtv; }
        }
    }
}
