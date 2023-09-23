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
    /// Tile class
    /// </summary>
    public class Tile
    {
        //private
        Point2D pos;
        Point2D sourcePos;
        Point2D dimen;

        //public
        /// <summary>
        /// Tile constructor
        /// </summary>
        /// <param name="posx">X position</param>
        /// <param name="posy">Y position</param>
        /// <param name="spx">Source x value</param>
        /// <param name="spy">Source y value</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        public Tile(int posx, int posy, int spx, int spy, int w, int h)
        {
            pos = new Point2D(posx, posy);
            sourcePos = new Point2D(spx, spy);
            dimen = new Point2D(w, h);
        }
        /// <summary>
        /// Draws Tile
        /// </summary>
        /// <param name="renderer">Renderer referecne</param>
        /// <param name="sourceImg">Texture2D reference</param>
        public void draw(IntPtr renderer, Texture2D sourceImg)
        {
            SimpleDraw.draw(renderer, sourceImg, new Rectangle(pos.X, pos.Y, dimen.X, dimen.Y), new Rectangle(sourcePos.X, sourcePos.Y, dimen.X, dimen.Y));
            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //spriteBatch.Draw(sourceImg, new Rectangle(pos.X, pos.Y, dimen.X, dimen.Y), new Rectangle(sourcePos.X, sourcePos.Y, dimen.X, dimen.Y), Color.White);
            //spriteBatch.End();
        }
    }
    /// <summary>
    /// An invisible button used for selecting segments of an image
    /// </summary>
    public class Spacer
    {
        //private
        Point2D pos;
        Rectangle box;
        
        //public
        /// <summary>
        /// Spacer constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="w">Width</param>
        /// <param name="h">Heihgt</param>
        public Spacer(int x, int y, int w, int h)
        {
            pos = new Point2D(x, y);
            box = new Rectangle(x, y, w, h);
        }
        /// <summary>
        /// Takes a rectangle for the cursor and returns a VectorInt if there is a collsion
        /// else return null to denote not clicked
        /// </summary>
        /// <param name="cursor"></param>
        /// <returns></returns>
        public Point2D clicked(Rectangle cursor)
        {
            if (box.intersects(cursor))
            {
                return pos;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// X property
        /// </summary>
        public float X
        {
            get { return pos.X; }
            set { pos.X = value; }
        }
        /// <summary>
        /// X property
        /// </summary>
        public float Y
        {
            get { return pos.Y; }
            set { pos.Y = value; }
        }
    }
    /// <summary>
    /// TileMap class
    /// </summary>
    public class TileMap
    {
        //protected
        protected Texture2D mapSource;
        protected Tile[,] tiles;
        protected Point2D dimen;
        //protected Spacer[,] spacers;

        //public
        /// <summary>
        /// TileMap constructor
        /// </summary>
        /// <param name="source">Texture2D reference</param>
        /// <param name="w">Width in tiles</param>
        /// <param name="h">Height in tiles</param>
        /// <param name="filename">Filename/path</param>
        public TileMap(Texture2D source, int w = 32, int h = 32, string filename = "")
        {
            mapSource = source;
            tiles = new Tile[w, h];
            dimen = new Point2D(w, h);
            //if filename == "" create empty map

            //else load a map file

        }
        /// <summary>
        /// Set size of map
        /// </summary>
        /// <param name="size">Point2D</param>
        public void set_size(Point2D size)
        {
            Tile[,] temp = new Tile[(int)size.X, (int)size.Y];
            //copy old tiles to new map
            //if size x or y less then dimen x or y clear 
            if (size.X < dimen.X || size.Y < dimen.Y)
            {
                //leave tiles empty
            }
            else
            {
                for (int x = 0; x < dimen.X - 1; x++)
                {
                    for (int y = 0; y < dimen.Y - 1; y++)
                    {
                        temp[x, y] = tiles[x, y];
                    }
                }
            }
            tiles = temp;
        }
    }
}
