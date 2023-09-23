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
    public class RTSTurret
    {
        //protected
        protected Texture2D source;
        protected string sourceName;
        protected Rectangle srcRect;
        protected Rectangle destRect;
        protected double rotation;
        protected double rotationRate;
        protected int range;
        protected Point2D pivot;
        protected Point2D position;
        //public
        /// <summary>
        /// RTS Turret constructor
        /// </summary>
        /// <param name="source">source texture</param>
        /// <param name="w">turret width</param>
        /// <param name="h">turret height</param>
        /// <param name="x">turret x position</param>
        /// <param name="y">turret y position</param>
        /// <param name="px">turret x pivot</param>
        /// <param name="py">turret y pivot</param>
        /// <param name="range">turret range from pivot</param>
        /// <param name="rotationRate">turret rotation rate</param>
        public RTSTurret(Texture2D source, string sourceName, int w, int h, float x, float y,
            float px, float py, int range, double rotationRate)
        {
            this.source = source;
            this.sourceName = sourceName;
            srcRect.X = 0;
            srcRect.Y = 0;
            srcRect.Width = w;
            srcRect.Height = h;
            destRect.X = (int)x;
            destRect.Y = (int)y;
            destRect.Width = w;
            destRect.Height = h;
            rotation = 0;
            this.rotationRate = rotationRate;
            this.range = range;
            pivot = new Point2D(x + px, y + py);
            position = new Point2D(x, y);
        }
        /// <summary>
        /// RTS turret copy constructor
        /// </summary>
        /// <param name="obj">RTS turret instance to copy</param>
        public RTSTurret(RTSTurret obj)
        {
            this.source = obj.Source;
            sourceName = obj.SourceName;
            srcRect.X = 0;
            srcRect.Y = 0;
            srcRect.Width = obj.Width;
            srcRect.Height = obj.Height;
            destRect.X = (int)obj.X;
            destRect.Y = (int)obj.Y;
            destRect.Width = obj.Width;
            destRect.Height = obj.Height;
            rotation = obj.Rotation;
            this.rotationRate = obj.RotationRate;
            this.range = obj.Range;
            pivot = new Point2D(obj.Pivot.X, obj.Pivot.Y);
            position = new Point2D(obj.position.X, obj.position.Y);
        }
        /// <summary>
        /// RTSTurret constructor from file
        /// </summary>
        /// <param name="sr">Stream reader reference</param>
        public RTSTurret(System.IO.StreamReader sr)
        {
            sr.ReadLine();
            sourceName = sr.ReadLine();
            source = TextureBank.getTexture(sourceName);
            destRect.X = Convert.ToInt32(sr.ReadLine());
            destRect.Y = Convert.ToInt32(sr.ReadLine());
            destRect.Width = Convert.ToInt32(sr.ReadLine());
            destRect.Height = Convert.ToInt32(sr.ReadLine());
            srcRect.Width = destRect.Width;
            srcRect.Height = destRect.Height;
            rotation = Convert.ToDouble(sr.ReadLine());
            rotationRate = Convert.ToDouble(sr.ReadLine());
            range = Convert.ToInt32(sr.ReadLine());
            pivot = new Point2D(Convert.ToInt32(sr.ReadLine()), Convert.ToInt32(sr.ReadLine()));
            position = new Point2D(Convert.ToInt32(sr.ReadLine()), Convert.ToInt32(sr.ReadLine()));
        }
        /// <summary>
        /// draws the turret
        /// </summary>
        /// <param name="renderer">renderer reference</param>
        public void draw(IntPtr renderer)
        {
            //sb.Draw(source, destRect, srcRect, Color.White, (float)rotation, new Vector2(pivot.X, pivot.Y), SpriteEffects.None, depth + 1);
            SimpleDraw.draw(renderer, source, srcRect, destRect, rotation, pivot, SDL2.SDL.SDL_RendererFlip.SDL_FLIP_NONE);
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
        /// X property
        /// </summary>
        public float X
        {
            get { return position.X; }
            set
            {
                position.X = value;
                destRect.X = (int)value;
            }
        }
        /// <summary>
        /// Y property
        /// </summary>
        public float Y
        {
            get { return position.Y; }
            set
            {
                position.Y = value;
                destRect.Y = (int)value;
            }
        }
        /// <summary>
        /// Width property
        /// </summary>
        public float Width
        {
            get { return destRect.Width; }
            set
            {
                destRect.Width = value;
                srcRect.Width = value;
            }
        }
        /// <summary>
        /// Height property
        /// </summary>
        public float Height
        {
            get { return destRect.Height; }
            set
            {
                destRect.Height = value;
                srcRect.Height = value;
            }
        }
        /// <summary>
        /// Rotation property
        /// </summary>
        public double Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        /// <summary>
        /// Rotation rate property
        /// </summary>
        public double RotationRate
        {
            get { return rotationRate; }
            set { rotationRate = value; }
        }
        /// <summary>
        /// Range property
        /// </summary>
        public int Range
        {
            get { return range; }
            set { range = value; }
        }
        /// <summary>
        /// Pivot property
        /// </summary>
        public Point2D Pivot
        {
            get { return pivot; }
            set { pivot = value; }
        }
        /// <summary>
        /// Barrel tip property
        /// </summary>
        public Point2D barrelTip
        {
            get { return new Point2D((float)Math.Cos((double)rotation) * range, (float)Math.Sin((double)rotation) * range); }
        }
        /// <summary>
        /// Source name property
        /// </summary>
        public string SourceName
        {
            get { return sourceName; }
            set { sourceName = value; }
        }
        /// <summary>
        /// Moves the turret in a direction at a given speed
        /// </summary>
        /// <param name="direction">direction in radians</param>
        /// <param name="speed">speed in pixels</param>
        public void moveDirection(double direction, float speed)
        {
            double mx = Math.Cos(direction) * (double)speed;
            double my = Math.Sin(direction) * (double)speed;
            position.X += (float)mx;
            position.Y += (float)my;
            destRect.X += (int)mx;
            destRect.Y += (int)my;
        }
        /// <summary>
        /// Moves the turret in explicit x and y directions
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="my"></param>
        public void moveExplicit(float mx, float my)
        {
            position.X += mx;
            position.Y += my;
            destRect.X += (int)mx;
            destRect.Y += (int)my;
        }
        /// <summary>
        /// rotates the turret by one rotation rate per call
        /// </summary>
        /// <param name="target">position to track relative to the turret</param>
        public void trackTarget(Point2D target)
        {
            //calculate the pivot point of the turret
            Point2D tempPoint = new Point2D(position.X + pivot.X, position.Y + pivot.Y);
            //calculate the angle between the two points
            double angle = Point2D.CalcAngle(tempPoint, target);
            //determine which direction to rotate
            if (angle - (rotation + rotationRate) < angle - (rotation - rotationRate))
            {
                //if rotatin rate is greater than difference to target angle just rotate the difference 
                if (angle - (rotation + rotationRate) < 0)
                {
                    rotation += Math.Abs(rotation + rotationRate);
                }
                else
                {
                    rotation += rotationRate;
                }
            }
            else
            {
                //if rotatin rate is greater than difference to target angle just rotate the difference
                if (angle - (rotation - rotationRate) < 0)
                {
                    rotation -= Math.Abs(rotation - rotationRate);
                }
                else
                {
                    rotation -= rotationRate;
                }
            }
        }
        /// <summary>
        /// Saves object instance data
        /// </summary>
        /// <param name="sw"></param>
        public void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======RTSTurret Data======");
            sw.WriteLine(sourceName);
            sw.WriteLine(destRect.X);
            sw.WriteLine(destRect.Y);
            sw.WriteLine(destRect.Width);
            sw.WriteLine(destRect.Height);
            sw.WriteLine(rotation);
            sw.WriteLine(rotationRate);
            sw.WriteLine(range);
            sw.WriteLine(pivot.X);
            sw.WriteLine(pivot.Y);
            sw.WriteLine(position.X);
            sw.WriteLine(position.Y);
        }
        /// <summary>
        /// Loads object instance data
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="gb"></param>
        public void loadData(System.IO.StreamReader sr)
        {
            sr.ReadLine();
            sourceName = sr.ReadLine();
            source = TextureBank.getTexture(sourceName);
            destRect.X = Convert.ToInt32(sr.ReadLine());
            destRect.Y = Convert.ToInt32(sr.ReadLine());
            destRect.Width = Convert.ToInt32(sr.ReadLine());
            destRect.Height = Convert.ToInt32(sr.ReadLine());
            srcRect.Width = destRect.Width;
            srcRect.Height = destRect.Height;
            rotation = Convert.ToDouble(sr.ReadLine());
            rotationRate = Convert.ToDouble(sr.ReadLine());
            range = Convert.ToInt32(sr.ReadLine());
            pivot = new Point2D(Convert.ToInt32(sr.ReadLine()), Convert.ToInt32(sr.ReadLine()));
            position = new Point2D(Convert.ToInt32(sr.ReadLine()), Convert.ToInt32(sr.ReadLine()));
        }
    }
}
