//====================================================
//Written by Kujel Selsuru
//Last Updated 23/02/24
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
    /// Types of Paper dolls
    /// </summary>
    public enum DOLLTYPES
    {
        HUMANOID = 0,
        RAPTOR,
        QUADRAPED,
        ROVER,
        PLANE
    };

    /// <summary>
    /// PaperBone class is used to link and position PaperBodyParts
    /// </summary>
    public class PaperBone
    {
        //protected
        protected Point2D start;
        protected Point2D end;

        //public
        /// <summary>
        /// PaperBone constructor
        /// </summary>
        /// <param name="sx">Start X value</param>
        /// <param name="sy">Start Y value</param>
        /// <param name="ex">End X value</param>
        /// <param name="ey">End Y value</param>
        public PaperBone(float sx, float sy, float ex, float ey)
        {
            start = new Point2D(sx, sy);
            end = new Point2D(ex, ey);
        }
        /// <summary>
        /// PaperBone copy constructor
        /// </summary>
        /// <param name="obj">PaperBone reference</param>
        public PaperBone(PaperBone obj)
        {
            start = new Point2D(obj.Start.X, obj.Start.Y);
            end = new Point2D(obj.End.X, obj.End.Y);
        }
        /// <summary>
        /// PaperBone from file constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public PaperBone(System.IO.StreamReader sr)
        {
            sr.ReadLine();
            start = new Point2D((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()));
            end = new Point2D((float)Convert.ToDecimal(sr.ReadLine()), (float)Convert.ToDecimal(sr.ReadLine()));
        }
        /// <summary>
        /// Saves PaperBone data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======PaperBone Data======");
            sw.WriteLine(start.X);
            sw.WriteLine(start.Y);
            sw.WriteLine(end.X);
            sw.WriteLine(end.Y);
        }
        /// <summary>
        /// Rotates the end point around the start point
        /// </summary>
        /// <param name="angle">Angle value in degrees</param>
        public void rotateBone(float angle)
        {
            int dist = Point2D.calculateDistance(start, end);
            end.X = (float)(Math.Cos(Helpers<int>.degreesToRadians(angle)) * dist) + start.IX;
            end.Y = (float)(Math.Sin(Helpers<int>.degreesToRadians(angle)) * dist) + start.IY;
        }
        /// <summary>
        /// Start property
        /// </summary>
        public Point2D Start
        {
            get { return start; }
            set { start = value; }
        }
        /// <summary>
        /// End property
        /// </summary>
        public Point2D End
        {
            get { return end; }
            set { end = value; }
        }
    }
    /// <summary>
    /// PaperBodyPart class is used to render body parts
    /// </summary>
    public class PaperBodyPart
    {
        //protected
        protected Rectangle srcRect;
        protected Rectangle destRect;
        protected Point2D pivotPoint;
        protected Texture2D source;
        protected string srcName;
        //public
        /// <summary>
        /// PaperBodyPart constructor
        /// </summary>
        /// <param name="srcName">Source name as string value</param>
        /// <param name="source">Texture2D reference</param>
        /// <param name="width">Source width in pixels</param>
        /// <param name="height">Source height in pixrels</param>
        /// <param name="pivotX">Pivot X value</param>
        /// <param name="pivotY">Pivot Y value</param>
        public PaperBodyPart(string srcName, Texture2D source, int width, int height, int pivotX, int pivotY)
        {
            this.srcName = srcName;
            this.source = source;
            srcRect = new Rectangle(0, 0, width, height);
            destRect = new Rectangle(0, 0, width, height);
            pivotPoint = new Point2D(pivotX, pivotY);
        }
        /// <summary>
        /// PaperBodyPart copy constructor
        /// </summary>
        /// <param name="obj">PaperBodyPart reference</param>
        public PaperBodyPart(PaperBodyPart obj)
        {
            srcName = obj.SrcName;
            source = obj.Source;
            srcRect = new Rectangle(0, 0, obj.SrcRect.Width, obj.SrcRect.Height);
            destRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            pivotPoint = new Point2D(obj.PivotPoint.X, obj.PivotPoint.Y);
        }
        /// <summary>
        /// PaperBodyPart from file constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public PaperBodyPart(System.IO.StreamReader sr)
        {
            sr.ReadLine();
            srcName = sr.ReadLine();
            source = TextureBank.getTexture(srcName);
            srcRect = new Rectangle(0, 0, Convert.ToInt32(sr.ReadLine()), Convert.ToInt32(sr.ReadLine()));
            destRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            pivotPoint = new Point2D(Convert.ToInt32(sr.ReadLine()), Convert.ToInt32(sr.ReadLine()));
        }
        /// <summary>
        /// Draws the body part at the specified angle
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="angle">Angle of part in degrees</param>
        public void draw(IntPtr renderer, float angle = 0)
        {
            SDL.SDL_Rect sRect = srcRect.Rect;
            SDL.SDL_Rect dRect = destRect.Rect;
            SDL.SDL_Point center;
            center.x = pivotPoint.IX; 
            center.y = pivotPoint.IY;
            SDL.SDL_RenderCopyEx(renderer, source.texture, ref sRect, ref dRect, Helpers<int>.degreesToRadians(angle), ref center, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }
        /// <summary>
        /// Saves PaperBodyPart data
        /// </summary>
        /// <param name="sw">StreamWriterData</param>
        public void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======PaperBodyPart Data======");
            sw.WriteLine(srcName);
            sw.WriteLine(srcRect.Width);
            sw.WriteLine(srcRect.Height);
            sw.WriteLine(pivotPoint.X);
            sw.WriteLine(pivotPoint.Y);
        }
        /// <summary>
        /// Scales the output render size, no input returns to 1:1 scale
        /// </summary>
        /// <param name="scale">Scaling value</param>
        public void scaleOutput(float scale = 1.0f)
        {
            destRect.Width = srcRect.Width * scale;
            destRect.Height = srcRect.Height * scale;
        }
        /// <summary>
        /// Sets the render position of the PaperBodyPart, position is cnetered at pivot point
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        public void setPos(float x, float y)
        {
            destRect.X = x - pivotPoint.X;
            destRect.Y = y - pivotPoint.Y;
        }
        /// <summary>
        /// Source property
        /// </summary>
        public Texture2D Source
        {
            get { return source; }
            set { source = value; }
        }
        /// <summary>
        /// SrcName property
        /// </summary>
        public string SrcName
        {
            get { return srcName; }
            set { srcName = value; }
        }
        /// <summary>
        /// PivotPoint property
        /// </summary>
        public Point2D PivotPoint
        {
            get { return pivotPoint; }
        }
        /// <summary>
        /// SrcRect property
        /// </summary>
        public Rectangle SrcRect
        {
            get { return srcRect; }
        }
        /// <summary>
        /// DestRect property
        /// </summary>
        public Rectangle DestRect
        {
            get { return destRect; }
        }
    }
    /// <summary>
    /// PaperCloths class is used to render cloths over the body
    /// </summary>
    public class PaperCloths : PaperBodyPart
    {
        //protected

        //public
        public PaperCloths(string srcName, Texture2D source, int width, int height, int pivotX, int pivotY) : 
            base(srcName, source, width, height, pivotX, pivotY)
        {

        }
        /// <summary>
        /// PaperCloths copy construtor
        /// </summary>
        /// <param name="obj">PaperCloths reference</param>
        public PaperCloths(PaperCloths obj) : base((PaperBodyPart)obj)
        {

        }
        /// <summary>
        /// PaperCloths from file constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public PaperCloths(System.IO.StreamReader sr) : base(sr)
        {

        }
    }
    /// <summary>
    /// PaperDoll class for handling storing and drawing paper doll objects
    /// </summary>
    public class PaperDoll
    {
        //protected
        protected Dictionary<string, PaperBone> skeleton;
        protected Dictionary<string, PaperBodyPart> body;
        protected Dictionary<string, PaperCloths> cloths;
        //public
        /// <summary>
        /// PaperDoll constructor
        /// </summary>
        public PaperDoll()
        {
            skeleton = new Dictionary<string, PaperBone>();
            body = new Dictionary<string, PaperBodyPart>();
            cloths = new Dictionary<string, PaperCloths>();
        }
        /// <summary>
        /// PaperDoll copy constructor
        /// </summary>
        /// <param name="obj">PaperDoll reference</param>
        public PaperDoll(PaperDoll obj)
        {
            skeleton = new Dictionary<string, PaperBone>();
            for(int i = 0; i < obj.Skeleton.Count - 1; i++)
            {
                skeleton.Add(obj.Skeleton.Keys.ElementAt(i), obj.Skeleton.Values.ElementAt(i));
            }
            body = new Dictionary<string, PaperBodyPart>();
            for (int i = 0; i < obj.Body.Count - 1; i++)
            {
                body.Add(obj.Body.Keys.ElementAt(i), obj.Body.Values.ElementAt(i));
            }
            cloths = new Dictionary<string, PaperCloths>();
            for (int i = 0; i < obj.Cloths.Count - 1; i++)
            {
                cloths.Add(obj.Cloths.Keys.ElementAt(i), obj.Cloths.Values.ElementAt(i));
            }
        }
        /// <summary>
        /// PaperDoll from file constuctor
        /// </summary>
        /// <param name="sr">StreamReader</param>
        public PaperDoll(System.IO.StreamReader sr)
        {
            sr.ReadLine();
            PaperBone bone = null;
            PaperBodyPart bodyPart = null;
            PaperCloths clothsPart = null;
            string partName = "";
            int num = Convert.ToInt32(sr.ReadLine());
            skeleton = new Dictionary<string, PaperBone>();
            for(int i = 0; i < num - 1; i++)
            {
                partName = sr.ReadLine();
                bone = new PaperBone(sr);
                skeleton.Add(partName, bone);
            }
            num = Convert.ToInt32(sr.ReadLine());
            body = new Dictionary<string, PaperBodyPart>();
            for (int i = 0; i < num - 1; i++)
            {
                partName = sr.ReadLine();
                bodyPart = new PaperBodyPart(sr);
                body.Add(partName, bodyPart);
            }
            num = Convert.ToInt32(sr.ReadLine());
            cloths = new Dictionary<string, PaperCloths>();
            for (int i = 0; i < num - 1; i++)
            {
                partName = sr.ReadLine();
                clothsPart = new PaperCloths(sr);
                cloths.Add(partName, clothsPart);
            }
        }
        /// <summary>
        /// Saves PaperDoll data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======PaperDoll Data======");
            sw.WriteLine(skeleton.Count);
            for(int i = 0; i < skeleton.Count - 1; i++)
            {
                sw.WriteLine(skeleton.Keys.ElementAt(i));
                skeleton.Values.ElementAt(i).saveData(sw);
            }
            sw.WriteLine(body.Count);
            for (int i = 0; i < body.Count - 1; i++)
            {
                sw.WriteLine(body.Keys.ElementAt(i));
                body.Values.ElementAt(i).saveData(sw);
            }
            sw.WriteLine(cloths.Count);
            for (int i = 0; i < cloths.Count - 1; i++)
            {
                sw.WriteLine(cloths.Keys.ElementAt(i));
                cloths.Values.ElementAt(i).saveData(sw);
            }
        }
        /// <summary>
        /// Draws all body parts in order of addition to PaperDoll
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void draw(IntPtr renderer)
        {
            for(int i = 0; i < body.Count - 1; i++)
            {
                body.Values.ElementAt(i).draw(renderer);
            }
            for (int i = 0; i < cloths.Count - 1; i++)
            {
                cloths.Values.ElementAt(i).draw(renderer);
            }
        }
        /// <summary>
        /// Draw PaperDoll skeleton
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        public void drawSkeleton(IntPtr renderer)
        {
            for(int i = 0; i < skeleton.Count - 1; i++)
            {
                DrawLine.draw(renderer, skeleton.Values.ElementAt(i).Start, skeleton.Values.ElementAt(i).End, ColourBank.getColour(XENOCOLOURS.PURPLE));
                DrawLine.drawCircle(renderer, skeleton.Values.ElementAt(i).Start, 3, ColourBank.getColour(XENOCOLOURS.BLUE));
                DrawLine.drawCircle(renderer, skeleton.Values.ElementAt(i).End, 3, ColourBank.getColour(XENOCOLOURS.RED));
            }
        }
        /// <summary>
        /// Draws a specified body part
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="name">Name of part to draw</param>
        /// <param name="angle">Angle of part's rotation</param>
        public void drawBodyPart(IntPtr renderer, string name, float angle = 0)
        {
            PaperBodyPart bodyPart = null;
            body.TryGetValue(name, out bodyPart);
            if(bodyPart != null)
            {
                bodyPart.draw(renderer, angle);
            }
        }
        /// <summary>
        /// Draws a specified cloths part
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="name">Name of part to draw</param>
        /// <param name="angle">Angle of part's rotation</param>
        public void drawClothsPart(IntPtr renderer, string name, float angle = 0)
        {
            PaperCloths clothsPart = null;
            cloths.TryGetValue(name, out clothsPart);
            if (clothsPart != null)
            {
                clothsPart.draw(renderer, angle);
            }
        }
        /// <summary>
        /// Skeleton property
        /// </summary>
        public Dictionary<string, PaperBone> Skeleton
        {
            get { return skeleton; }
        }
        /// <summary>
        /// Body property
        /// </summary>
        public Dictionary<string, PaperBodyPart> Body
        {
            get { return body; }
        }
        /// <summary>
        /// Cloths property
        /// </summary>
        public Dictionary<string, PaperCloths> Cloths
        {
            get { return cloths; }
        }
    }
}
