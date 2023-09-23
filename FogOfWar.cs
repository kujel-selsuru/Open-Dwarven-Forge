//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenoLib
{
    public class FogOfWar
    {
        //protected
        protected int worldWidth;
        protected int worldHeight;
        protected int tileWidth;
        protected int tileHeight;
        protected bool[,] fog;
        protected Texture2D image;
        protected string imageName;
        protected Rectangle srcRect;
        protected Rectangle destRect;
        protected bool inDomain(int tx, int ty)
        {
            if (tx >= 0 && tx < worldWidth)
            {
                if (ty >= 0 && ty < worldHeight)
                {
                    return true;
                }
            }
            return false;
        }
        //public
        /// <summary>
        /// fog of war constructor
        /// </summary>
        /// <param name="ww">world width in tiles</param>
        /// <param name="wh">world height in tiles</param>
        /// <param name="tw">tile width</param>
        /// <param name="th">tile height</param>
        /// <param name="image">fog image</param>
        /// <param name="imageName">for image name</param>
        public FogOfWar(int ww, int wh, int tw, int th, Texture2D image, string imageName)
        {
            worldWidth = ww;
            worldHeight = wh;
            tileWidth = tw;
            tileHeight = th;
            this.image = image;
            this.imageName = imageName;
            fog = new bool[ww, wh];
            srcRect.X = 0;
            srcRect.Y = 0;
            srcRect.Width = tileWidth;
            srcRect.Height = tileHeight;
            destRect.X = 0;
            destRect.Y = 0;
            destRect.Width = tileWidth + 4;
            destRect.Height = tileHeight + 4;
        }
        /// <summary>
        /// Fog of war constructor from file
        /// </summary>
        /// <param name="sr">Stream reader reference</param>
        /// <param name="gb">Graphics bank</param>
        public FogOfWar(System.IO.StreamReader sr)
        {
            sr.ReadLine();
            worldWidth = Convert.ToInt32(sr.ReadLine());
            worldHeight = Convert.ToInt32(sr.ReadLine());
            tileWidth = Convert.ToInt32(sr.ReadLine());
            tileHeight = Convert.ToInt32(sr.ReadLine());
            imageName = sr.ReadLine();
            image = TextureBank.getTexture(imageName);
            fog = new bool[worldWidth, worldHeight];
            for (int col = 0; col < worldWidth; col++)
            {
                for (int row = 0; row < worldHeight; row++)
                {
                    if (sr.ReadLine() == "true")
                    {
                        fog[col, row] = true;
                    }
                    else
                    {
                        fog[col, row] = false;
                    }
                }
            }
            srcRect.X = 0;
            srcRect.Y = 0;
            srcRect.Width = tileWidth;
            srcRect.Height = tileHeight;
            destRect.X = 0;
            destRect.Y = 0;
            destRect.Width = tileWidth + 4;
            destRect.Height = tileHeight + 4;
        }
        /// <summary>
        /// clears fog around sight radius 
        /// </summary>
        /// <param name="tx">target x</param>
        /// <param name="ty">target y</param>
        /// <param name="sight">radius of sight</param>
        public void clearFog(int tx, int ty, int sight)
        {
            if (inDomain(tx, ty))
            {
                int starty = ty - sight;
                int range = starty + (sight * 2) + 1;
                int midRange = starty + sight;
                int traverse = 4;
                int startx = tx - 1;
                for (int vert = starty; vert < range; vert++)
                {
                    for (int hori = tx - (traverse / 2); hori < tx + (traverse / 2); hori++)
                    {
                        if (inDomain(hori, vert))
                        {
                            fog[hori, vert] = false;
                        }
                    }
                    if (vert <= ty - 1)
                    {
                        traverse += 2;
                    }
                    else if (vert >= ty + 1)
                    {
                        traverse += 2;
                    }
                }
            }
        }
        /// <summary>
        /// fills fog across the entire map
        /// </summary>
        public void fillFog()
        {
            for (int ww = 0; ww < worldWidth; ww++)
            {
                for (int wh = 0; wh < worldHeight; wh++)
                {
                    fog[ww, wh] = true;
                }
            }
        }
        /// <summary>
        /// Clears all fog
        /// </summary>
        public void clearAllFog()
        {
            for (int ww = 0; ww < worldWidth; ww++)
            {
                for (int wh = 0; wh < worldHeight; wh++)
                {
                    fog[ww, wh] = false;
                }
            }
        }
        /// <summary>
        /// renders the fog on screen
        /// </summary>
        /// <param name="renderer">renderer reference</param>
        /// <param name="winx">window x position</param>
        /// <param name="winy">window y position</param>
        /// <param name="winW">window width in tiles</param>
        /// <param name="winH">window height in tiles</param>
        public void draw(IntPtr renderer, int winx, int winy, int winW, int winH)
        {
            int wx = winx / tileWidth;
            int wy = winy / tileHeight;
            for (int ix = wx; ix < winW; ix++)
            {
                for (int iy = wy; iy < winH; iy++)
                {
                    if (fog[ix, iy])
                    {
                        destRect.X = (wx * tileWidth) + (ix * tileWidth) - 2;
                        destRect.Y = (wy * tileHeight) + (iy * tileHeight) - 2;
                        //sb.Draw(image, destRect, srcRect, Color.White);
                        SimpleDraw.draw(renderer, image, srcRect, destRect);
                    }
                }
            }
        }
        /// <summary>
        /// Saves fog of war data
        /// </summary>
        /// <param name="sw">Stream writer refernece</param>
        public void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======FogOfWar Data======");
            sw.WriteLine(worldWidth);
            sw.WriteLine(worldHeight);
            sw.WriteLine(tileWidth);
            sw.WriteLine(tileHeight);
            sw.WriteLine(imageName);
            for (int col = 0; col < worldWidth; col++)
            {
                for (int row = 0; row < worldHeight; row++)
                {
                    if (fog[col, row])
                    {
                        sw.WriteLine("true");
                    }
                    else
                    {
                        sw.WriteLine("false");
                    }
                }
            }
        }
        /// <summary>
        /// Patch width property
        /// </summary>
        public float PatchWidth
        {
            get { return destRect.Width; }
        }
        /// <summary>
        /// PatchHeight property
        /// </summary>
        public float PatchHeight
        {
            get { return destRect.Height; }
        }
        /// <summary>
        /// World width property
        /// </summary>
        public int Width
        {
            get { return worldWidth; }
        }
        /// <summary>
        /// World height property
        /// </summary>
        public int Height
        {
            get { return worldHeight; }
        }
        /// <summary>
        /// Returns fog state at x and y position
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Returns boolean state at x y position</returns>
        public bool grid(int x, int y)
        {
            if (inDomain(x, y))
            {
                return fog[x, y];
            }
            return false;
        }
    }
}
