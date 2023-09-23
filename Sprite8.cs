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
    /// facing8 enumeration
    /// </summary>
    public enum facing8 { up = 0, upRight, right, downRight, down, downLeft, left, upLeft}
    /// <summary>
    /// Sprite8 class
    /// </summary>
    public class Sprite8 : SimpleSprite
    {
        //protected
        protected facing8 face;

        //public
        /// <summary>
        /// Sprite8 constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="face">Starting facing</param>
        /// <param name="still">Still state</param>
        public Sprite8(Texture2D source, int x, int y, int numFrames, facing8 face, bool still)
            : base(source, x, y, numFrames, simpleFacing.up, still)
        {
            this.face = face;
            sourceBox.h = source.height / 8;
            height = sourceBox.h;
            destBox.h = sourceBox.h;
            hitBox.h = sourceBox.h;
        }
        /// <summary>
        /// Draws Sprite8
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        public new void draw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            sourceBox.x = width * frame;
            sourceBox.y = height * (int)face;
            destBox.x = hitBox.x - winx; //compensating for position relative to window
            destBox.y = hitBox.y - winy;
            if (!still)
            {
                if (frameDelay.tick())
                {
                    if (frame < numFrames - 1)
                    {
                        frame++;
                    }
                    else
                    {
                        frame = 0;
                    }
                }
            }
            SDL.SDL_RenderCopy(renderer, source.texture, ref sourceBox, ref destBox);
        }
        /// <summary>
        /// Draws sprite8 (depricated)
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="colour">SDL_Colour reference</param>
        /// <param name="winx">Window x offset</param>
        /// <param name="winy">Window y offset</param>
        public new void draw(IntPtr renderer, SDL.SDL_Color colour, int winx = 0, int winy = 0)
        {
            sourceBox.x = width * frame;
            sourceBox.y = height * (int)face;
            destBox.x = hitBox.x - winx; //compensating for position relative to window
            destBox.y = hitBox.y - winy;
            if (!still)
            {
                if (frameDelay.tick())
                {
                    if (frame < numFrames - 1)
                    {
                        frame++;
                    }
                    else
                    {
                        frame = 0;
                    }
                }
            }
            SDL.SDL_RenderCopy(renderer, source.texture, ref sourceBox, ref destBox);
        }
        /// <summary>
        /// Sets facing toward a provided point
        /// </summary>
        /// <param name="target">Target point to face</param>
        public void facePoint(Point2D target)
        {
            double angle = Point2D.CalcAngle(this.Center, target);
            face = (facing8)((360 - angle) / 45);
        }
        /// <summary>
        /// Sets facing toward a provided point (depricated)
        /// </summary>
        /// <param name="target">Target point to face</param>
        public void facePoint2(Point2D target)
        {
            if (Center2.X == target.X)
            {
                if (Center2.Y > target.Y)
                {
                    face = facing8.up;
                }
                else
                {
                    face = facing8.down;
                }
            }
            if (Center2.Y == target.Y)
            {
                if (Center2.X > target.X)
                {
                    face = facing8.right;
                }
                else
                {
                    face = facing8.left;
                }
            }
            if (Center2.X > target.X)
            {
                if (Center2.Y > target.Y)
                {
                    face = facing8.downRight;
                }
                else
                {
                    face = facing8.upRight;
                }
            }
            else
            {
                if (Center2.Y > target.Y)
                {
                    face = facing8.downLeft;
                }
                else
                {
                    face = facing8.upLeft;
                }
            }
            //double angle = GLIB.Point2.CalcAngle(this.Center2, target);
            //face = (facing8)((360 - angle) / 45);
            
        }
        /// <summary>
        /// returns the angle in degress for 8 directions
        /// </summary>
        /// <param name="p1">center point</param>
        /// <param name="p2">outer point</param>
        /// <returns>degrees as a double</returns>
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
            else if(p1.Y == p2.Y)//left or right
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
        /// Fallows an internally set path
        /// </summary>
        public new void fallowPath()
        {
            if (pindex < path.Count)
            {
                float shiftx = 0;
                float shifty = 0;
                if (hitBox.x != path[pindex].X)
                {
                    if (hitBox.x < path[pindex].X)
                    {
                        shiftx = speedz;
                    }
                    else
                    {
                        shiftx = -speedz;
                    }
                }
                if (hitBox.y != path[pindex].Y)
                {
                    if (hitBox.y < path[pindex].Y)
                    {
                        shifty = speedz;
                    }
                    else
                    {
                        shifty = -speedz;
                    }
                }
                move2(shiftx, shifty);
                if (hitBox.x == path[pindex].X & hitBox.y == path[pindex].Y)
                {
                    faceNextPoint();
                    pindex++;
                }
            }
            else
            {
                path.Clear();
                pindex = 0;
                still = true;
            }
        }
        /// <summary>
        /// Sets facing toward next point in internal path list
        /// </summary>
        public void faceNextPoint()
        {
            double angle = 0;
            if (pindex > 0)
            {
                angle = Sprite8.findAngle(Center, path[pindex - 1]);
            }
            else
            {
                angle = Sprite8.findAngle(Center, path[pindex]);
            }
            face = (facing8)((360 - angle) / 45);
        }
        /// <summary>
        /// Face property
        /// </summary>
        public facing8 Face
        {
            get { return face; }
            set {face = value;}
        }
    }
}
