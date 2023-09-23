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
    public class PlanetoidBlock
    {
        //protected
        protected Texture2D source;
        protected Rectangle srcRect;
        protected Rectangle destRect;
        protected Rectangle hitBox;
        protected int dirt;
        protected string ore;
        protected bool free;

        //public
        public PlanetoidBlock(Texture2D source, int x, int y, int w, int h, int sx, int sy, int dirt = 10, string ore = "dirt", bool free = false)
        {
            this.source = source;
            hitBox = new Rectangle(x, y, w, h);
            srcRect = new Rectangle(sx, sy, w, h);
            destRect = new Rectangle(x, y, w, h);
            this.dirt = dirt;
            this.ore = ore;
            this.free = free;
        }

        public PlanetoidBlock(PlanetoidBlock obj)
        {
            source = obj.Source;
            hitBox = new Rectangle(obj.X, obj.Y, obj.W, obj.H);
            srcRect = new Rectangle(obj.SX, obj.SY, obj.W, obj.H);
            destRect = new Rectangle(obj.X, obj.Y, obj.W, obj.H);
            dirt = obj.Dirt;
            ore = obj.Ore;
            free = obj.Free;
        }

        public PlanetoidBlock(Texture2D source, StreamReader sr)
        {
            this.source = source;
            sr.ReadLine();//discard testing data
            hitBox = new Rectangle((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()));
            srcRect = new Rectangle((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()), hitBox.Width, hitBox.Height);
            destRect = new Rectangle(hitBox.X, hitBox.Y, hitBox.Width, hitBox.Height);
            dirt = Convert.ToInt32(sr.ReadLine());
            ore = sr.ReadLine();
            free = Convert.ToBoolean(sr.ReadLine());
        }

        public void saveData(StreamWriter sw)
        {
            sw.WriteLine("======PlanetoidBlock Data======");
            sw.WriteLine(hitBox.X);
            sw.WriteLine(hitBox.Y);
            sw.WriteLine(hitBox.Width);
            sw.WriteLine(hitBox.Height);
            sw.WriteLine(srcRect.X);
            sw.WriteLine(srcRect.X);
            sw.WriteLine(dirt);
            sw.WriteLine(ore);
            sw.WriteLine(free);
        }

        public void draw(IntPtr renderer, int winx, int winy)
        {
            destRect.X = hitBox.X - winx;
            destRect.Y = hitBox.Y - winy;
            SimpleDraw.draw(renderer, source, srcRect, destRect);
        }
        /// <summary>
        /// Move PlanetoidBlock in specified direction
        /// </summary>
        /// <param name="x">X direction value</param>
        /// <param name="y">Y direction value</param>
        public void move(float x, float y)
        {
            hitBox.X += x;
            hitBox.Y += x;
        }
        /// <summary>
        /// Move PlanetoidBlock in direction (in degrees, zero == move right)
        /// </summary>
        /// <param name="dir">Direction of movement</param>
        /// <param name="speed">Speed of movement</param>
        public void move(double dir, float speed)
        {
            Point2D vector = new Point2D(0, 0);
            vector.X = (float)Math.Cos(Helpers<int>.degreesToRadians(dir)) * speed;
            vector.Y = (float)Math.Sin(Helpers<int>.degreesToRadians(dir)) * speed;
            hitBox.X += vector.X;
            hitBox.Y += vector.Y;
        }
        /// <summary>
        /// Sets PlanetoidBlock position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void set(float x, float y)
        {
            hitBox.X = x;
            hitBox.Y = y;
        }
        /// <summary>
        /// Attempts dig free a block, 20% minimum of dirt value required to 
        /// dig free the block
        /// </summary>
        /// <param name="power">Dig strength value</param>
        /// <returns>True if free else false</returns>
        public bool dig(int power)
        {
            if((dirt / power) >= 5)//20% of dig strength
            {
                return false;
            }
            dirt -= power;
            if(dirt <= 0)
            {
                free = true;
                return true;
            }
            return false;
        }
        /// <summary>
        /// X property
        /// </summary>
        public float X
        {
            get { return hitBox.X; }
            set { hitBox.X = value; }
        }
        /// <summary>
        /// Y property
        /// </summary>
        public float Y
        {
            get { return hitBox.Y; }
            set { hitBox.Y = value; }
        }
        /// <summary>
        /// W property
        /// </summary>
        public float W
        {
            get { return hitBox.Width; }
        }
        /// <summary>
        /// H property
        /// </summary>
        public float H
        {
            get { return hitBox.Height; }
        }
        /// <summary>
        /// Source property
        /// </summary>
        public Texture2D Source
        {
            get { return source; }
        }
        /// <summary>
        /// Ore property
        /// </summary>
        public string Ore
        {
            get { return ore; }
            set { ore = value; }
        }
        /// <summary>
        /// Dirt property
        /// </summary>
        public int Dirt
        {
            get { return dirt; }
            set { dirt = value; }
        }
        /// <summary>
        /// SX property
        /// </summary>
        public float SX
        {
            get { return srcRect.X; }
            set { srcRect.X = value; }
        }
        /// <summary>
        /// SY property
        /// </summary>
        public float SY
        {
            get { return srcRect.Y; }
            set { srcRect.Y = value; }
        }
        /// <summary>
        /// Free property
        /// </summary>
        public bool Free
        {
            get { return free; }
            set { free = value; }
        }
    }

    public class XenoPlanetoid
    {
        //protected
        protected Texture2D source;
        protected string sourceName;
        protected DataGrid<PlanetoidBlock> mass;
        protected Point2D center;
        protected float gravity;
        protected int radius;
        protected int blockW;
        protected int blockH;

        //public
        /// <summary>
        /// XenoPlanetoid constructor
        /// </summary>
        /// <param name="sourceName">Source Texture2D name</param>
        /// <param name="x">Center X value</param>
        /// <param name="y">Center Y value</param>
        /// <param name="gravity">Gravity value</param>
        /// <param name="radius">Radius value</param>
        /// <param name="blockW">Block width in pixels</param>
        /// <param name="blockH">Block Height in pixels</param>
        public XenoPlanetoid(string sourceName, float x, float y, float gravity, int radius, int blockW = 8, int blockH = 8)
        {
            source = TextureBank.getTexture(sourceName);
            this.sourceName = sourceName;
            center = new Point2D(x, y);
            this.gravity = gravity;
            this.radius = radius;
            mass = new DataGrid<PlanetoidBlock>(radius * 2, radius * 2);
            this.blockW = blockW;
            this.blockH = blockH;
        }
        /// <summary>
        /// From file XenoPlanetoid constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public XenoPlanetoid(StreamReader sr)
        {
            sr.ReadLine();//discard testing data
            sourceName = sr.ReadLine();
            source = TextureBank.getTexture(sourceName);
            mass = new DataGrid<PlanetoidBlock>(Convert.ToInt32(sr.ReadLine()), Convert.ToInt32(sr.ReadLine()));
            blockW = Convert.ToInt32(sr.ReadLine());
            blockH = Convert.ToInt32(sr.ReadLine());
            gravity = (float)Convert.ToDecimal(sr.ReadLine());
            radius = Convert.ToInt32(sr.ReadLine());
            center = new Point2D((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()));
            for (int bx = 0; bx < mass.Width; bx++)
            {
                for (int by = 0; by < mass.Height; by++)
                {
                    mass.Grid[bx, by] = new PlanetoidBlock(source, sr);
                }
            }
        }
        /// <summary>
        /// Save XenoPlanetoid data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public virtual void saveData(StreamWriter sw)
        {
            sw.WriteLine("======XenoPlanetoid Data======");
            sw.WriteLine(sourceName);
            sw.WriteLine(mass.Width);
            sw.WriteLine(mass.Height);
            sw.WriteLine(blockW);
            sw.WriteLine(blockH);
            sw.WriteLine(gravity);
            sw.WriteLine(radius);
            sw.WriteLine(center.X);
            sw.WriteLine(center.Y);
            for(int bx = 0; bx < mass.Width; bx++)
            {
                for (int by = 0; by < mass.Height; by++)
                {
                    mass.Grid[bx, by].saveData(sw);
                }
            }
        }
        /// <summary>
        /// Form a planetoid, assumes source tile pallet is 5 x 7 blocks
        /// </summary>
        /// <param name="ore">Preset ore value</param>
        public void form(string ore = "dirt")
        {
            List<Point2D> temp = Helpers<int>.getCircleArea<PlanetoidBlock>(mass, center, radius);
            Random rand = new Random((int)System.DateTime.Today.Ticks);
            int sx, sy;
            PlanetoidBlock block;
            for(int b = 0; b < temp.Count; b++)
            {
                sx = rand.Next(0, 500) / 100;
                sy = rand.Next(0, 700) / 100;
                block = new PlanetoidBlock(source, temp[b].IX, temp[b].IY, blockW, blockH, sx, sy, 10, ore, false);
            }
        }
        /// <summary>
        /// Mass property
        /// </summary>
        public DataGrid<PlanetoidBlock> Mass
        {
            get { return mass; }
        }
        /// <summary>
        /// Gravity property
        /// </summary>
        public float Gravity
        {
            get { return gravity; }
            set { gravity = value; }
        }
        /// <summary>
        /// Radius property
        /// </summary>
        public int Radius
        {
            get { return radius; }
        }
        /// <summary>
        /// SourceName property
        /// </summary>
        public string SourceName
        {
            get { return sourceName; }
        }
        /// <summary>
        /// Center property
        /// </summary>
        public Point2D Center
        {
            get { return center; }
        }
        /// <summary>
        /// BlockW property
        /// </summary>
        public int BlockW
        {
            get { return blockW; }
        }
        /// <summary>
        /// BlockH property
        /// </summary>
        public int BlockH
        {
            get { return blockH; }
        }
        /// <summary>
        /// Width property
        /// </summary>
        public int W
        {
            get { return mass.Width; }
        }
        /// <summary>
        /// Height property
        /// </summary>
        public int H
        {
            get { return mass.Height; }
        }
    }
}
