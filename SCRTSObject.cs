//====================================================
//Written by Kujel Selsuru
//Last Updated 03/02/24
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SDL2;
using XenoLib;

namespace XenoLib
{
    public enum STATUSES
    {
        POISON = 0,
        HEAL,
        DAMAGE,
        BUFFARMOR,
        BUFFDAMAGE,
        BUFFSPEED,
        DEBUFFARMOR,
        DEBUFFDAMAGE,
        DEBUFFSPEED,
        REGEN,
        NONE
    }
    public enum ACTIONS
    {
        MELEE = 0,
        BEAM,
        LASER,
        MISSILE,
        BOMB,
        BUFF,
        BULLET,
        DEBUFF,
        HARVEST,
        BUILD,
        REPAIR,
        HEAL,
        NONE
    }
    public enum TARGETS
    {
        GROUND = 0,
        AIR,
        BUILDING,
        RESOURCE,
        TILE,
        NONE
    }
    public enum ABILITIES
    {
        SELF = 0,
        SHOOT,
        ANTIAIR,
        BOMBARD,
        TARGET,
        PASSIVE,
        UPGRADE,
        TRAIN,
        NONE
    }
    public enum UNITTYPES
    {
        AIRCOMBAT = 0,
        GROUNDCOMBAT,
        AIRSUPPORT,
        GROUNDSUPPORT,
        AIRTRANSPORT,
        GROUNDTRANSPORT,
        AIRHARVESTER,
        GROUNDHARVESTER,
        AIRWORKER,
        GROUNDWORKER,
        AIRCARRIER,
        GROUNDCARRIER,
        AIRHERO,
        GROUNDHERO,
        NONE
    }
    public enum BUILDINGTYPES
    {
        FACTORY = 0,
        TURRET,
        LAB,
        COMMANDCENTER,
        REFINORY,
        BUNKER,
        MINE,
        SUPPORT,
        NONE,
        FOUNDATION
    }
    public enum STAGES
    {
        ZERO = 0,
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        TEN,
        ELLEVEN,
        TWELVE
    }
    public enum TURRETTYPES
    {
        ORBITS = 0,
        FLIPS,
        FIXED,
        NONE
    };

    /// <summary>
    /// A class that stores and builds build unit commands in buildings/units
    /// </summary>
    public class SCRTSBuildToken
    {
        //protected
        protected int cost1;
        protected int cost2;
        protected int cost3;
        protected string name;
        protected SCRTSUnit owner;
        protected int constructed;
        protected int constructionCost;

        //public
        /// <summary>
        /// BuildToken constructor
        /// </summary>
        /// <param name="cost1">Cost value 1</param>
        /// <param name="cost2">Cost value 2</param>
        /// <param name="cost3">Cost value 3</param>
        /// <param name="name">Name of object to build</param>
        /// <param name="owner">Owner of BuildToken</param>
        public SCRTSBuildToken(int cost1, int cost2, int cost3, string name, SCRTSUnit owner)
        {
            this.cost1 = cost1;
            this.cost2 = cost2;
            this.cost3 = cost3;
            this.name = name;
            this.owner = owner;
            constructed = 0;
            constructionCost = (cost1 * 10) + (cost2 * 15) + (cost3 * 20);
        }
        /// <summary>
        /// BuildToekn from file constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        /// <param name="owner">Object instance that owns this instance</param>
        public SCRTSBuildToken(System.IO.StreamReader sr, SCRTSUnit owner)
        {
            sr.ReadLine();
            this.cost1 = Convert.ToInt32(sr.ReadLine());
            this.cost2 = Convert.ToInt32(sr.ReadLine());
            this.cost3 = Convert.ToInt32(sr.ReadLine());
            this.name = sr.ReadLine();
            this.owner = owner;
            constructed = Convert.ToInt32(sr.ReadLine());
            constructionCost = (cost1 * 10) + (cost2 * 15) + (cost3 * 20);
        }
        /// <summary>
        /// BuildToken copy constructor
        /// </summary>
        /// <param name="obj">BuildToekn instance reference</param>
        public SCRTSBuildToken(SCRTSBuildToken obj)
        {
            this.cost1 = obj.Cost1;
            this.cost2 = obj.Cost1;
            this.cost3 = obj.Cost1;
            this.name = obj.Name;
            this.owner = obj.Owner;
            constructed = obj.Constructed;
            constructionCost = (cost1 * 10) + (cost2 * 15) + (cost3 * 20);
        }
        /// <summary>
        /// Saves token data to file stream
        /// </summary>
        /// <param name="sw">Streamwriter reference</param>
        public virtual void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======BuildToken Data======");
            sw.WriteLine(cost1);
            sw.WriteLine(cost2);
            sw.WriteLine(cost3);
            sw.WriteLine(name);
            sw.WriteLine(constructed);
        }
        /// <summary>
        /// Updates internal state
        /// </summary>
        /// <param name="rotation">Rotation of unit created</param>
        public void update(int rotation = 0)
        {
            constructed++;
            if (constructed >= constructionCost)
            {
                owner.Commander.addUnit(name, owner.RallyPoint.IX, owner.RallyPoint.IY);
            }
        }
        /// <summary>
        /// Percentage property
        /// </summary>
        public float Percentage
        {
            get { return (float)constructed / (float)constructionCost; }
        }
        /// <summary>
        /// Cost 1 property
        /// </summary>
        public int Cost1
        {
            get { return cost1; }
            set { cost1 = value; }
        }
        /// <summary>
        /// Cost 2 property
        /// </summary>
        public int Cost2
        {
            get { return cost2; }
            set { cost2 = value; }
        }
        /// <summary>
        /// Cost 3 property
        /// </summary>
        public int Cost3
        {
            get { return cost3; }
            set { cost3 = value; }
        }
        /// <summary>
        /// Name property
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// Owner property
        /// </summary>
        public SCRTSUnit Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        /// <summary>
        /// Constructed property
        /// </summary>
        public int Constructed
        {
            get { return constructed; }
            set { constructed = value; }
        }
        /// <summary>
        /// ConstructionCost property
        /// </summary>
        public int ConstructionCost
        {
            get { return constructionCost; }
            //set { constructed = value; }
        }
    }
    public class SCRTSObject : XenoSprite
    {
        //protected
        protected float scalerValue;
        protected string sourceName;
        protected float drawScaler;

        //public
        /// <summary>
        /// SCRTSObject constructor
        /// </summary>
        /// <param name="name">Object name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="sourceName">Source name value</param>
        /// <param name="hp">Hitpoint value</param>
        public SCRTSObject(string name, float x, float y, int width,
            int height, int numFrames, string sourceName, int hp = 100, float drawScaler = 1.0f) :
            base(name, x, y, width, height, numFrames, hp)
        {
            scalerValue = 1;
            this.sourceName = sourceName;
            source = TextureBank.getTexture(sourceName);
            this.drawScaler = drawScaler;
        }
        /// <summary>
        /// RTSObject from file constructor 
        /// </summary>
        /// <param name="sr">StreamReader referecne</param>
        public SCRTSObject(StreamReader sr) : base(sr)
        {
            sr.ReadLine();
            scalerValue = (float)Convert.ToDecimal(sr.ReadLine());
            if(scalerValue <= 0)
            {
                scalerValue = 1;
            }
            destRect.w = (int)(srcRect.w * scalerValue);
            destRect.h = (int)(srcRect.h * scalerValue);
            string buffer;
            buffer = sr.ReadLine();
            sourceName = buffer;
            if(sourceName == null)
            {
                sourceName = name;
            }
            source = TextureBank.getTexture(sourceName);
            drawScaler = (float)Convert.ToDecimal(sr.ReadLine());
        }
        /// <summary>
        /// RTSObject copy constructor
        /// </summary>
        /// <param name="obj">SCRTSObject reference</param>
        public SCRTSObject(SCRTSObject obj) : base(obj)
        {
            scalerValue = obj.ScalerValue;
            destRect.w = (int)(srcRect.w * scalerValue);
            destRect.h = (int)(srcRect.h * scalerValue);
            sourceName = obj.SourceName;
            drawScaler = obj.DrawScaler;
        }
        /// <summary>
        /// Save data override
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======SCRTSObject Data======");
            sw.WriteLine(scalerValue);
            sw.WriteLine(sourceName);
            sw.WriteLine(drawScaler);
        }
        /// <summary>
        /// Updates SCRTSObject internal state
        /// </summary>
        public virtual void update()
        {

        }
        /// <summary>
        /// Draws object to store scale
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window X offset</param>
        /// <param name="winy">Window Y offset</param>
        public virtual void scaledDraw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            drawScaled(renderer, winx, winy, drawScaler);
        }
        /// <summary>
        /// Draws object to store scale at angle of rotation
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="angle">Angle of sprite rotation</param>
        /// <param name="winx">Window X offset</param>
        /// <param name="winy">Window Y offset</param>
        public virtual void scaledDraw(IntPtr renderer, float angle, int winx = 0, int winy = 0)
        {
            drawScaled(renderer, angle, winx, winy, drawScaler);
        }
        /// <summary>
        /// Scales object by a value
        /// </summary>
        /// <param name="val">Scaling value</param>
        public virtual void scale(float val)
        {
            scalerValue = val;
            destRect.w = (int)(srcRect.w * scalerValue);
            destRect.h = (int)(srcRect.h * scalerValue);
        }
        /// <summary>
        /// Returns hitbox center relative to deminsions
        /// </summary>
        public Point2D BoxCenter
        {
            get { return new Point2D(hitBox.Width / 2, hitBox.Height / 2); }
        }
        /// <summary>
        /// ScalerValue property
        /// </summary>
        public float ScalerValue
        {
            get { return scalerValue; }
        }
        /// <summary>
        /// Returns mid bottom position of RTSObject
        /// </summary>
        public Point2D MidBottom
        {
            get
            {
                return new Point2D(Center.X, Bottom);
            }
        }
        /// <summary>
        /// Returns mid top position of RTSObject
        /// </summary>
        public Point2D MidTop
        {
            get
            {
                return new Point2D(Center.X, Bottom);
            }
        }
        /// <summary>
        /// Returns mid left position of RTSObject
        /// </summary>
        public Point2D MidLeft
        {
            get
            {
                return new Point2D(Left, Center.Y);
            }
        }
        /// <summary>
        /// Returns mid right position of RTSObject
        /// </summary>
        public Point2D MidRight
        {
            get
            {
                return new Point2D(Right, Center.Y);
            }
        }
        /// <summary>
        /// SourceName property
        /// </summary>
        public string SourceName
        {
            get { return sourceName; }
            set { sourceName = value; }
        }
        /// <summary>
        /// DrawScaler property
        /// </summary>
        public float DrawScaler
        {
            get { return drawScaler; }
            set { drawScaler = value; }
        }
    }
    public class SCRTSTurret : SCRTSObject
    {
        //protected
        protected Point2D pivot;
        protected Point2D relativePos;
        protected int length;
        protected int fromCenter;
        protected float theta;
        protected string turretName;

        //public 
        /// <summary>
        /// SCRTSTurret constructor
        /// </summary>
        /// <param name="name">Graphic source name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="sourceName">Source name value</param>
        /// <param name="hp">HP value</param>
        /// <param name="px">Pivot X value</param>
        /// <param name="py">Pivot Y value</param>
        /// <param name="length">Barrel length in pixels</param>
        /// <param name="fromCenter">Distence from owner center</param>
        public SCRTSTurret(string name, float x, float y, int width,
            int height, int numFrames, string sourceName, int hp = 100, int px = 0,
            int py = 0, int length = 10, int fromCenter = 1, float relativeX = 1, 
            float relativeY = 1) :
            base(name, x, y, width, height, numFrames, sourceName, hp)
        {
            pivot = new Point2D(px, py);
            this.length = length;
            this.fromCenter = fromCenter;
            theta = 0f;
            if (name == "")
            {
                turretName = "default turret";
            }
            else
            {
                turretName = name;
            }
            relativePos = new Point2D(relativeX, relativeY);
        }
        /// <summary>
        /// RTSTurret from file constructor 
        /// </summary>
        /// <param name="sr">StreamReader referecne</param>
        public SCRTSTurret(StreamReader sr) : base(sr)
        {
            sr.ReadLine();
            pivot = new Point2D(Convert.ToInt32(sr.ReadLine()),
                Convert.ToInt32(sr.ReadLine()));
            length = Convert.ToInt32(sr.ReadLine());
            fromCenter = Convert.ToInt32(sr.ReadLine());
            theta = (float)Convert.ToDecimal(sr.ReadLine());
            turretName = sr.ReadLine();
            relativePos = new Point2D(Convert.ToInt32(sr.ReadLine()), Convert.ToInt32(sr.ReadLine()));
        }
        /// <summary>
        /// RTSTurret copy constructor
        /// </summary>
        /// <param name="obj">SCRTSTurret reference</param>
        public SCRTSTurret(SCRTSTurret obj) : base(obj)
        {
            pivot = new Point2D(obj.Pivot.X, obj.Pivot.Y);
            length = obj.Length;
            fromCenter = obj.FromCenter;
            theta = obj.Theta;
            turretName = obj.TurretName;
            relativePos = new Point2D(obj.RelativePos.IX, obj.RelativePos.IY);
        }
        /// <summary>
        /// Save data override
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======SCRTSTurret Data======");
            sw.WriteLine(pivot.IX);
            sw.WriteLine(pivot.IY);
            sw.WriteLine(length);
            sw.WriteLine(fromCenter);
            sw.WriteLine(theta);
            sw.WriteLine(turretName);
            sw.WriteLine(relativePos.IX);
            sw.WriteLine(relativePos.IY);
        }
        /// <summary>
        /// Override draw
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window X offset</param>
        /// <param name="winy">Window Y offset</param>
        public override void draw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            destRect.x = (int)hitBox.X - (pivot.IX + winx);
            destRect.y = (int)hitBox.Y - (pivot.IY + winy);
            SimpleDraw.draw(renderer, source, srcRect, destRect,
                selfAngle, pivot);
        }
        /// <summary>
        /// Updates SCRTSTurret internal state
        /// </summary>
        public override void update()
        {

        }
        /// <summary>
        /// Rotates turret toward a target
        /// </summary>
        /// <param name="target">Point2D reference</param>
        /// <param name="speed">Speed of rotation</param>
        public void rotateTowardTarget(Point2D target, int speed)
        {
            double angle = Point2D.CalcAngle(pivot, target);
            if (selfAngle > angle)
            {
                if (selfAngle - angle >= speed)
                {
                    selfAngle += speed;
                }
                else
                {
                    selfAngle -= (selfAngle - angle);
                }
            }
            else if (selfAngle < angle)
            {
                if (angle - selfAngle >= speed)
                {
                    selfAngle -= speed;
                }
                else
                {
                    selfAngle += (angle - selfAngle);
                }
            }
        }
        /// <summary>
        /// Rotates turret by a provied speed, clockwise or 
        /// counter clockwise
        /// </summary>
        /// <param name="speed">Speed of rotation</param>
        /// <param name="clockwise">Clockwise flag</param>
        public void rotate(int speed, bool clockwise = true)
        {
            if (clockwise == true)
            {
                selfAngle += speed;
            }
            else
            {
                selfAngle -= speed;
            }
        }
        /// <summary>
        /// Returns true if angle of turret rotation is less than or equal
        /// to 3 degrees of selfAngle else returns false
        /// </summary>
        /// <param name="target">Point2D reference</param>
        /// <returns>Boolean</returns>
        public bool turretLock(Point2D target)
        {
            Point2D cent = new Point2D(hitBox.X + pivot.X, hitBox.Y + pivot.Y);
            double ang = Point2D.CalcAngle(target, cent);
            if(Math.Abs(ang - selfAngle) <= 3)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Overrides setPos function to update relativePos as well as position
        /// </summary>
        /// <param name="x">X position in pixels</param>
        /// <param name="y">Y position in pixels</param>
        public override void setPos(float x, float y)
        {
            base.setPos(x, y);
            relativePos = new Point2D(x, y);
        }
        /// <summary>
        /// Returns turret tip
        /// </summary>
        public Point2D TurretTip
        {
            get
            {
                Point2D tip = new Point2D();
                tip.X = hitBox.X + pivot.X + (float)(Math.Cos(selfAngle) * length);
                tip.Y = hitBox.Y + pivot.Y + (float)(Math.Sin(selfAngle) * length);
                return tip;
            }
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
        /// Length property
        /// </summary>
        public int Length
        {
            get { return length; }
            set { length = value; }
        }
        /// <summary>
        /// FromCenter property
        /// </summary>
        public int FromCenter
        {
            get { return fromCenter; }
            set { fromCenter = value; }
        }
        /// <summary>
        /// Theta property
        /// </summary>
        public float Theta
        {
            get { return theta; }
            set { theta = value; }
        }
        /// <summary>
        /// TurretName property
        /// </summary>
        public string TurretName
        {
            get { return turretName; }
            set { turretName = value; }
        }
        /// <summary>
        /// RelativePos property
        /// </summary>
        public Point2D RelativePos
        {
            get { return relativePos; }
            set { relativePos = value; }
        }
    }
    public class SCRTSUnit : SCRTSObject
    {
        //protected
        protected List<SCRTSTurret> turrets;
        protected List<SCRTSAbility> abilities;
        protected StatusBar2 healthBar;
        protected int shields;
        protected StatusBar2 shieldsBar;
        protected SCRTSCommander commander;
        protected int iff;
        protected List<SCRTSUnit> cargo;
        protected int tank;
        protected int tankMax;
        protected string tankLoad;
        protected UNITTYPES ut;
        protected string unitName;
        protected int cost1;
        protected int cost2;
        protected int cost3;
        protected Objective storedTarget;
        protected int buildValue;
        protected int maxBuildValue;
        protected StatusBar2 buildBar;
        protected string buildName;
        protected string builderName;
        protected int maxHP;
        protected int maxShields;
        protected SCRTSStatus status;
        protected TURRETTYPES tt;
        protected Queue<SCRTSBuildToken> buildQueue;
        protected SCRTSBuildToken buildToken;
        protected Point2D rallyPoint;

        //public 
        /// <summary>
        /// SCRTSUnit constructor
        /// </summary>
        /// <param name="name">Unit name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="commander">SCRTCommander reference</param>
        /// <param name="sourceName">Source name value</param>
        /// <param name="hp">Hitpoint value</param>
        public SCRTSUnit(string name, float x, float y, int width,
            int height, int numFrames, SCRTSCommander commander, string sourceName, int hp = 100) :
            base(name, x, y, width, height, numFrames, sourceName, hp)
        {
            turrets = new List<SCRTSTurret>();
            abilities = new List<SCRTSAbility>();
            healthBar = new StatusBar2(hp, hp, width, (int)x, (int)y,
                ColourBank.getColour(XENOCOLOURS.GREEN), false);
            shields = hp;
            shieldsBar = new StatusBar2(hp, hp, width, (int)x, (int)y + 2,
                ColourBank.getColour(XENOCOLOURS.GREEN), false);
            this.commander = commander;
            iff = commander.IFF;
            cargo = new List<SCRTSUnit>();
            ut = UNITTYPES.GROUNDCOMBAT;
            if (name == "")
            {
                unitName = "default unit";
            }
            else
            {
                unitName = name;
            }
            cost1 = 0;
            cost2 = 0;
            cost3 = 0;
            tank = 0;
            tankMax = 1000;
            tankLoad = "";
            buildValue = 0;
            maxBuildValue = 0;
            buildName = "";
            builderName = "";
            buildBar = new StatusBar2(100, 0, width, (int)x, (int)y,
                ColourBank.getColour(XENOCOLOURS.GREEN), false);
            maxHP = hp;
            status = null;
            Path = new List<Point2D>();
            tt = TURRETTYPES.ORBITS;
            buildQueue = new Queue<SCRTSBuildToken>();
            buildToken = null;
            rallyPoint = new Point2D(x, y + height + 1);
        }
        /// <summary>
        /// SCRTSUnit from file constructor 
        /// </summary>
        /// <param name="sr">StreamReader referecne</param>
        /// <param name="commander">SCRTSCommander referecne</param>
        public SCRTSUnit(StreamReader sr, SCRTSCommander commander) : base(sr)
        {
            sr.ReadLine();
            turrets = new List<SCRTSTurret>();
            int num = Convert.ToInt32(sr.ReadLine());
            for (int i = 0; i < num; i++)
            {
                turrets.Add(new SCRTSTurret(sr));
            }
            abilities = new List<SCRTSAbility>();
            num = Convert.ToInt32(sr.ReadLine());
            for (int i = 0; i < num; i++)
            {
                SCRTSAbility temp = new SCRTSAbility(sr);
                temp.Turret = turrets[temp.TurretIndex];
                abilities.Add(temp);
            }
            healthBar = new StatusBar2(hp, hp, (int)hitBox.Width, (int)hitBox.X,
                (int)hitBox.Y, ColourBank.getColour(XENOCOLOURS.GREEN), false);
            shieldsBar = new StatusBar2(hp, hp, (int)hitBox.Width, (int)hitBox.X,
                (int)hitBox.Y + 2, ColourBank.getColour(XENOCOLOURS.GREEN), false);
            shields = Convert.ToInt32(sr.ReadLine());
            this.commander = commander;
            iff = Convert.ToInt32(sr.ReadLine());
            cargo = new List<SCRTSUnit>();
            num = Convert.ToInt32(sr.ReadLine());
            for (int i = 0; i < num; i++)
            {
                cargo.Add(new SCRTSUnit(sr, commander));
            }
            ut = (UNITTYPES)Convert.ToInt32(sr.ReadLine());
            unitName = sr.ReadLine();
            cost1 = Convert.ToInt32(sr.ReadLine());
            cost2 = Convert.ToInt32(sr.ReadLine());
            cost3 = Convert.ToInt32(sr.ReadLine());
            path = new List<Point2D>();
            num = Convert.ToInt32(sr.ReadLine());
            for (int i = 0; i < num; i++)
            {
                path.Add(new Point2D(Convert.ToInt32(sr.ReadLine()),
                    Convert.ToInt32(sr.ReadLine())));
            }
            tank = Convert.ToInt32(sr.ReadLine());
            tankMax = Convert.ToInt32(sr.ReadLine());
            tankLoad = sr.ReadLine();
            buildValue = Convert.ToInt32(sr.ReadLine());
            maxBuildValue = Convert.ToInt32(sr.ReadLine());
            buildName = sr.ReadLine();
            builderName = sr.ReadLine();
            buildBar = new StatusBar2(100, 0, (int)hitBox.Width, (int)hitBox.X, (int)hitBox.Y,
                ColourBank.getColour(XENOCOLOURS.GREEN), false);
            maxHP = Convert.ToInt32(sr.ReadLine());
            maxShields = Convert.ToInt32(sr.ReadLine());
            status = new SCRTSStatus(sr);
            tt = (TURRETTYPES)Convert.ToInt32(sr.ReadLine());
            buildQueue = new Queue<SCRTSBuildToken>();
            num = Convert.ToInt32(sr.ReadLine());
            for(int i = 0; i < num; i++)
            {
                buildQueue.Enqueue(new SCRTSBuildToken(sr, this));
            }
            if (sr.ReadLine() != null)
            {
                buildToken = new SCRTSBuildToken(sr, this);
            }
            else
            {
                buildToken = null;
            }
            rallyPoint = new Point2D(Convert.ToUInt32(sr.ReadLine()), Convert.ToUInt32(sr.ReadLine()));
        }
        /// <summary>
        /// SCRTSUnit copy constructor
        /// </summary>
        /// <param name="obj">RTSUnit reference</param>
        public SCRTSUnit(SCRTSUnit obj) : base(obj)
        {
            turrets = new List<SCRTSTurret>();
            for (int i = 0; i < obj.Turrets.Count; i++)
            {
                turrets.Add(new SCRTSTurret(obj.Turrets[i]));
            }
            abilities = new List<SCRTSAbility>();
            for (int i = 0; i < obj.Abilities.Count; i++)
            {
                abilities.Add(new SCRTSAbility(obj.Abilities[i]));
            }
            healthBar = new StatusBar2(hp, hp, (int)hitBox.Width, (int)hitBox.X,
                (int)hitBox.Y, ColourBank.getColour(XENOCOLOURS.GREEN), false);
            shieldsBar = new StatusBar2(hp, hp, (int)hitBox.Width, (int)hitBox.X,
                (int)hitBox.Y, ColourBank.getColour(XENOCOLOURS.GREEN), false);
            shields = obj.Shields;
            iff = obj.IFF;
            cargo = new List<SCRTSUnit>();
            for (int i = 0; i < obj.Cargo.Count; i++)
            {
                cargo.Add(obj.Cargo[i]);
            }
            ut = obj.UT;
            unitName = obj.UnitName;
            cost1 = obj.Cost1;
            cost2 = obj.Cost2;
            cost3 = obj.Cost3;
            path = new List<Point2D>();
            for (int i = 0; i < obj.Path.Count; i++)
            {
                path.Add(new Point2D(obj.Path[i].IX, obj.Path[i].IY));
            }
            tank = obj.Tank;
            tankMax = obj.TankMax;
            tankLoad = obj.TankLoad;
            buildValue = obj.BuildValue;
            maxBuildValue = obj.MaxBuildValue;
            buildName = obj.BuildName;
            builderName = obj.BuilderName;
            buildBar = new StatusBar2(100, 0, (int)hitBox.Width, (int)hitBox.X, (int)hitBox.Y,
                 ColourBank.getColour(XENOCOLOURS.GREEN), false);
            maxHP = obj.MaxHP;
            maxShields = obj.MaxShields;
            status = obj.Status;
            tt = obj.TT;
            buildQueue = new Queue<SCRTSBuildToken>();
            for(int i = 0; i < obj.BuildQueue.Count; i++)
            {
                buildQueue.Enqueue(new SCRTSBuildToken(obj.BuildQueue.ElementAt(i)));
            }
            rallyPoint = new Point2D(obj.RallyPoint.X, obj.RallyPoint.Y);
        }
        /// <summary>
        /// Save data override
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======SCRTSUnit Data======");
            sw.WriteLine(turrets.Count);
            for (int i = 0; i < turrets.Count; i++)
            {
                turrets[i].saveData(sw);
            }
            sw.WriteLine(abilities.Count);
            for (int i = 0; i < abilities.Count; i++)
            {
                abilities[i].saveData(sw);
            }
            sw.WriteLine(shields);
            sw.WriteLine(iff);
            sw.WriteLine(cargo.Count);
            for (int i = 0; i < cargo.Count; i++)
            {
                cargo[i].saveData(sw);
            }
            sw.WriteLine((int)ut);
            sw.WriteLine(unitName);
            sw.WriteLine(cost1);
            sw.WriteLine(cost2);
            sw.WriteLine(cost3);
            sw.WriteLine(path.Count);
            for (int i = 0; i < path.Count; i++)
            {
                sw.WriteLine(path[i].IX);
                sw.WriteLine(path[i].IY);
            }
            sw.WriteLine(tank);
            sw.WriteLine(tankMax);
            sw.WriteLine(tankLoad);
            sw.WriteLine(buildValue);
            sw.WriteLine(maxBuildValue);
            sw.WriteLine(buildName);
            sw.WriteLine(builderName);
            sw.WriteLine(maxHP);
            sw.WriteLine(maxShields);
            status.saveData(sw);
            sw.WriteLine(Convert.ToInt32(tt));
            sw.WriteLine(buildQueue.Count);
            for(int i = 0; i < buildQueue.Count; i++)
            {
                buildQueue.ElementAt(i).saveData(sw);
            }
            sw.WriteLine(rallyPoint.IX);
            sw.WriteLine(rallyPoint.IY);
        }
        /// <summary>
        /// Updates SCRTSUnit internal state
        /// </summary>
        public override void update()
        {
            healthBar.X = (int)hitBox.X;
            healthBar.Y = (int)hitBox.Y;
            if (status != null)
            {
                applyStatusEffect();
                if (status.LifeSpan < status.LifeSpent)
                {
                    status.LifeSpan++;
                }
                else
                {
                    status = null;
                }
            }
        }
        /// <summary>
        /// Overrides SCRTSObject scale function to account for turrets
        /// </summary>
        /// <param name="val">Scaling value</param>
        public override void scale(float val)
        {
            float dist = 0;
            double ang = 0;
            for(int t = 0; t < turrets.Count; t++)
            {
                dist = Point2D.calculateDistance(BoxCenter, turrets[t].Pivot);
                ang = Point2D.CalcAngle(BoxCenter, turrets[t].Pivot);
                dist *= val;
                turrets[t].X = (float)(((Math.Cos(ang) * dist) + BoxCenter.X) * val) - turrets[t].Pivot.X;
                turrets[t].Y = (float)(((Math.Sin(ang) * dist) + BoxCenter.Y) * val) - turrets[t].Pivot.Y;
            }
            base.scale(val);
        }
        /// <summary>
        /// Draws unit
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window X value</param>
        /// <param name="winy">Window Y value</param>
        public override void draw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            base.draw(renderer, winx, winy);
            for(int t = 0; t < turrets.Count; t++)
            {
                turrets[t].draw(renderer, winx, winy);
            }
        }
        /// <summary>
        /// Draws health bar
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window X offset</param>
        /// <param name="winy">Window Y offset</param>
        public void drawStatus(IntPtr renderer, int winx, int winy)
        {
            healthBar.draw(renderer, 3, winx, winy);
            if(BuildValue > 0)
            {
                buildBar.draw(renderer, 3, winx, winy - 3);
            }
        }
        /// <summary>
        /// Rotates turrets around center of RTSUnit
        /// </summary>
        /// <param name="rotation">Angle of rotation</param>
        public void rotateTurretsAroundCenter(double rotation)
        {
            float tx = 0;
            float ty = 0;
            for (int i = 0; i < turrets.Count; i++)
            {
                tx = ((float)Math.Cos(rotation) * (float)turrets[i].FromCenter) + turrets[i].Center.X;
                ty = ((float)Math.Sin(rotation) * (float)turrets[i].FromCenter) + turrets[i].Center.Y;
                turrets[i].setPos(tx, ty);
            }
        }
        /// <summary>
        /// Rotates turrets toward a target
        /// </summary>
        /// <param name="target">SCRTSUnit or RTSBuilding reference</param>
        public void targetTurrets(RTSUnit target)
        {
            for (int i = 0; i < turrets.Count; i++)
            {
                turrets[i].rotateTowardTarget(target.Center, 10);
            }
        }
        /// <summary>
        /// Checks if unit can attack
        /// </summary>
        /// <returns>Boolean</returns>
        public bool canAttack()
        {
            SCRTSAction act = null;
            for(int a = 0; a < abilities.Count; a++)
            {
                act = commander.ActionDB.getData(abilities[a].ActionName);
                switch(act.Act)
                {
                    case ACTIONS.BEAM:
                    case ACTIONS.BOMB:
                    case ACTIONS.DEBUFF:
                    case ACTIONS.MISSILE:
                    case ACTIONS.MELEE:
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if unit can buff
        /// </summary>
        /// <returns>Boolean</returns>
        public bool canBuff()
        {
            SCRTSAction act = null;
            for (int a = 0; a < abilities.Count; a++)
            {
                act = commander.ActionDB.getData(abilities[a].ActionName);
                switch (act.Act)
                {
                    case ACTIONS.BUFF:
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if unit can heal
        /// </summary>
        /// <returns>Boolean</returns>
        public bool canHeal()
        {
            SCRTSAction act = null;
            for (int a = 0; a < abilities.Count; a++)
            {
                act = commander.ActionDB.getData(abilities[a].ActionName);
                switch (act.Act)
                {
                    case ACTIONS.HEAL:
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if unit can repair
        /// </summary>
        /// <returns>Boolean</returns>
        public bool canRepair()
        {
            SCRTSAction act = null;
            for (int a = 0; a < abilities.Count; a++)
            {
                act = commander.ActionDB.getData(abilities[a].ActionName);
                switch (act.Act)
                {
                    case ACTIONS.REPAIR:
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if unit can build
        /// </summary>
        /// <returns>Boolean</returns>
        public bool canBuild()
        {
            SCRTSAction act = null;
            for(int a = 0; a < abilities.Count; a++)
            {
                act = commander.ActionDB.getData(abilities[a].ActionName);
                switch(act.Act)
                {
                    case ACTIONS.BUILD:
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if unit can upgrade
        /// </summary>
        /// <returns>Boolean</returns>
        public bool canUpgrade()
        {
            for(int a = 0; a < abilities.Count; a++)
            {
                if(abilities[a].AT == ABILITIES.UPGRADE)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if unit can train
        /// </summary>
        /// <returns>Boolean</returns>
        public bool canTrain()
        {
            for (int a = 0; a < abilities.Count; a++)
            {
                if (abilities[a].AT == ABILITIES.TRAIN)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if a unit can build a specified object
        /// </summary>
        /// <param name="buildName">Name of object to build</param>
        /// <returns>Boolean</returns>
        public bool canBuild(string buildName)
        {
            SCRTSAction act = null;
            for(int a = 0; a < abilities.Count; a++)
            {
                act = commander.ActionDB.getData(abilities[a].ActionName);
                switch(act.Act)
                {
                    case ACTIONS.BUILD:
                        if(act.ActionName == buildName)
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if unit can harvest
        /// </summary>
        /// <returns>Boolean</returns>
        public bool canHarvest()
        {
            SCRTSAction act = null;
            for (int a = 0; a < abilities.Count; a++)
            {
                act = commander.ActionDB.getData(abilities[a].ActionName);
                switch (act.Act)
                {
                    case ACTIONS.HARVEST:
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Returns the name of abilities with matching action type
        /// or returns "" if no matching ability found
        /// </summary>
        /// <param name="act">ACTIONS value</param>
        /// <returns>String</returns>
        public string getAbilityByActionType(ACTIONS act)
        {
            for(int i = 0; i < abilities.Count; i++)
            {
                if(commander.ActionDB.getData(abilities[i].ActionName).Act == act)
                {
                    return abilities[i].AbilityName;
                }
            }
            return "";
        }
        /// <summary>
        /// Call an ability by name
        /// </summary>
        /// <param name="name">Name of ability to call</param>
        public void callAbility(string name)
        {
            for(int i = 0; i < abilities.Count; i++)
            {
                if(abilities[i].AbilityName == name)
                {
                    if(abilities[i].Recharging == false)
                    {
                        if (status != null)
                        {
                            switch(status.SS)
                            {
                                case STATUSES.BUFFDAMAGE:
                                    abilities[i].performAbility(5);
                                    break;
                                case STATUSES.DEBUFFDAMAGE:
                                    abilities[i].performAbility(-5);
                                    break;
                            }
                        }
                        else
                        {
                            abilities[i].performAbility(0);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Returns max range of unit in pixels
        /// </summary>
        /// <returns>Integer</returns>
        public int getMaxRange()
        {
            SCRTSAction act = null;
            int mr = 0;
            for (int a = 0; a < abilities.Count; a++)
            {
                act = commander.ActionDB.getData(abilities[a].ActionName);
                switch (act.Act)
                {
                    case ACTIONS.BEAM:
                    case ACTIONS.BOMB:
                    case ACTIONS.DEBUFF:
                    case ACTIONS.MISSILE:
                    case ACTIONS.MELEE:
                        int num = (int)act.Speed * act.LifeSpan;
                        if(num > mr)
                        {
                            mr = num;
                        }
                        break;
                }
            }
            return mr;
        }
        /// <summary>
        /// Attempts to call an attack ability
        /// </summary>
        public void attemptAttackTarget()
        {
            //bool fired = false;
            for(int a = 0; a < abilities.Count; a++)
            {
                SCRTSAction act = new SCRTSAction(commander.ActionDB.getData(abilities[a].ActionName));
                switch(act.Act)
                {
                    case ACTIONS.BEAM:
                    case ACTIONS.LASER:
                    case ACTIONS.BOMB:
                    case ACTIONS.MISSILE:
                    case ACTIONS.MELEE:
                        if(abilities[a].Turret.turretLock(((SCAttackTarget)target).Target.Center) == false)
                        {
                            abilities[a].Turret.rotateTowardTarget(((SCAttackTarget)target).Target.Center, 3);
                        }
                        abilities[a].performAbility();
                        /*
                        if(fired == false)
                        {
                            if(abilities[a].Recharging == false)
                            {
                                fired = true;
                                abilities[a].activate();
                                act.StartPos = calculateBarrelTip(abilities[a].Turret);
                                Commander.Actions.Add(act);
                                break;
                            }
                        }
                        */
                        break;
                }
                /*
                if(fired == true)
                {
                    break;
                }
                */
            }
        }
        /// <summary>
        /// Calculates the barrel tip for a given turret
        /// </summary>
        /// <param name="turret">SCRTSTurret reference</param>
        /// <returns>Point2D object</returns>
        public Point2D calculateBarrelTip(SCRTSTurret turret)
        {
            Point2D barrelTip = new Point2D();
            barrelTip.X = hitBox.X + turret.Pivot.X + (float)Math.Cos(turret.SelfAngle) * turret.Length;
            barrelTip.X = hitBox.Y + turret.Pivot.Y + (float)Math.Sin(turret.SelfAngle) * turret.Length;
            return barrelTip;
        }
        /// <summary>
        /// Fallows path
        /// </summary>
        /// <param name="world">World reference</param>
        public void fallow(RTSWorld world)
        {
            if(path != null)
            {
                if(path[pathIndex].X > X)
                {
                    if(path[pathIndex].Y > Y)
                    {
                        if (tt != TURRETTYPES.ORBITS)
                        {
                            if (direct != DIRECT.DOWNRIGHT)
                            {
                                orbitTurrets(DIRECT.DOWNRIGHT);
                            }
                        }
                        direct = DIRECT.DOWNRIGHT;
                    }
                    else if(path[pathIndex].Y < Y)
                    {
                        if (tt != TURRETTYPES.ORBITS)
                        {
                            if (direct != DIRECT.UPRIGHT)
                            {
                                orbitTurrets(DIRECT.UPRIGHT);
                            }
                        }
                        direct = DIRECT.UPRIGHT;
                    }
                    else
                    {
                        if (tt != TURRETTYPES.ORBITS)
                        {
                            if (direct != DIRECT.RIGHT)
                            {
                                orbitTurrets(DIRECT.RIGHT);
                            }
                        }
                        direct = DIRECT.RIGHT;
                    }
                }
                else if(path[pathIndex].X < X)
                {
                    if (path[pathIndex].Y > Y)
                    {
                        if (tt != TURRETTYPES.ORBITS)
                        {
                            if (direct != DIRECT.DOWNLEFT)
                            {
                                orbitTurrets(DIRECT.DOWNLEFT);
                            }
                        }
                        direct = DIRECT.DOWNLEFT;
                    }
                    else if (path[pathIndex].Y < Y)
                    {
                        if (tt != TURRETTYPES.ORBITS)
                        {
                            if (direct != DIRECT.UPLEFT)
                            {
                                orbitTurrets(DIRECT.UPLEFT);
                            }
                        }
                        direct = DIRECT.UPLEFT;
                    }
                    else
                    {
                        if (tt != TURRETTYPES.ORBITS)
                        {
                            if (direct != DIRECT.LEFT)
                            {
                                orbitTurrets(DIRECT.LEFT);
                            }
                        }
                        direct = DIRECT.LEFT;
                    }
                }
                else
                {
                    if (path[pathIndex].Y > Y)
                    {
                        if (tt != TURRETTYPES.ORBITS)
                        {
                            if (direct != DIRECT.DOWN)
                            {
                                orbitTurrets(DIRECT.DOWN);
                            }
                        }
                        direct = DIRECT.DOWN;
                    }
                    else
                    {
                        if (tt != TURRETTYPES.ORBITS)
                        {
                            if (direct != DIRECT.UP)
                            {
                                orbitTurrets(DIRECT.UP);
                            }
                        }
                        direct = DIRECT.UP;
                    }
                }
                if(path[pathIndex].X > X)
                {
                    if(path[pathIndex].X - X < speed == true)
                    {
                        move(path[pathIndex].X - X, 0);
                    }
                    else
                    {
                        move(speed, 0);
                    }
                }
                else if(path[pathIndex].X < X)
                {
                    if(Math.Abs(X - path[pathIndex].X) < speed == true)
                    {
                        move(X - path[pathIndex].X, 0);
                    }
                    else
                    {
                        move(-speed, 0);
                    }
                }
                if(path[pathIndex].Y > Y)
                {
                    if(path[pathIndex].Y - Y < speed == true)
                    {
                        move(path[pathIndex].Y - Y, 0);
                    }
                    else
                    {
                        move(0, speed);
                    }
                }
                else if(path[pathIndex].Y < Y)
                {
                    if (Math.Abs(Y - path[pathIndex].Y) < speed == true)
                    {
                        move(Y - path[pathIndex].Y, 0);
                    }
                    else
                    {
                        move(0, -speed);
                    }
                }
                if(path[pathIndex].X == X &&
                    path[pathIndex].Y == Y)
                {
                    if(pathIndex < path.Count - 1)
                    {
                        if(world.OccupiedGrid.dataAt((path[pathIndex + 1].IX - world.Window.IX) / world.TileWidth,
                            (path[pathIndex + 1].IY - world.Window.IY) / world.TileHeight) == false)
                        {
                            pathIndex++;
                        }
                    }
                    else if(pathIndex >= path.Count - 1)
                    {
                        path = null;
                    }
                }
            }
        }
        /// <summary>
        /// Moves unit plus it's turrets
        /// </summary>
        /// <param name="x">Value to move on X axis</param>
        /// <param name="y">Value to move on Y axis</param>
        public override void move(float x, float y)
        {
            base.move(x, y);
            for(int t = 0; t < turrets.Count; t++)
            {
                turrets[t].move(x, y);
            }
        }
        /// <summary>
        /// Orbits turrets around center of unit (flips around center line if TURRETTYPE is FLIPS)
        /// </summary>
        /// <param name="dir">DIRECT enumeration</param>
        public void orbitTurrets(DIRECT dir)
        {
            float ang = 0;
            switch(tt)
            {
                case TURRETTYPES.ORBITS:
                    for (int t = 0; t < turrets.Count; t++)
                    {
                        switch (dir)
                        {
                            case DIRECT.UP:
                                ang = 270 + turrets[t].Theta;
                                break;
                            case DIRECT.UPRIGHT:
                                ang = 0 + turrets[t].Theta;
                                break;
                            case DIRECT.RIGHT:
                                ang = 90 + turrets[t].Theta;
                                break;
                            case DIRECT.DOWNRIGHT:
                                ang = 180 + turrets[t].Theta;
                                break;
                        }
                        turrets[t].X = (float)Math.Cos(ang) * turrets[t].FromCenter + Center.X;
                        turrets[t].Y = (float)Math.Sin(ang) * turrets[t].FromCenter + Center.Y;
                    }
                    break;
                case TURRETTYPES.FLIPS:
                    int dist = 0;
                    for (int t = 0; t < turrets.Count; t++)
                    {
                        dist = (int)(centerLine() - turrets[t].X);
                        turrets[t].X = turrets[t].X + (2 * dist);
                    }
                    break;
            }
        }
        /// <summary>
        /// Returns the center line of object's hitbox
        /// </summary>
        /// <returns>Integer</returns>
        public int centerLine()
        {
            return (int)(hitBox.X  + (hitBox.Width / 2));
        }
        /// <summary>
        /// Sets the target of specified ability and calls named ability
        /// </summary>
        /// <param name="abilityName">Name of ability to call</param>
        /// <param name="target">Point2D reference</param>
        public void setTargetedAbility(string abilityName, Point2D target)
        {
            for(int i = 0; i < abilities.Count; i++)
            {
                if(abilities[i].AbilityName == abilityName)
                {
                    abilities[i].TargetPoint = target;
                    callAbility(abilityName);
                    abilities[i].Targetting = false;
                    break;
                }
            }
        }
        /// <summary>
        /// Returns a specified ability reference
        /// </summary>
        /// <param name="abilityName">Name of specified ability</param>
        /// <returns>RTSAbility reference</returns>
        public SCRTSAbility getAbility(string abilityName)
        {
            for(int i = 0; i < abilities.Count; i++)
            {
                if(abilities[i].AbilityName == abilityName)
                {
                    return abilities[i];
                }
            }
            return null;
        }
        /// <summary>
        /// Train unit and create when done
        /// </summary>
        public void trainUnit()
        {
            if(buildValue >= 1 && buildValue < maxBuildValue)
            {
                buildValue++;
            }
            else
            {
                SCRTSUnit tmp = commander.createUnit(buildName, Center.X, Center.Y);
                buildValue = 0;
                if(this is SCRTSBuilding)
                {
                    tmp.Y = this.Bottom;
                    Point2D p = ((SCRTSBuilding)this).RallyPoint;
                    tmp.Target = new SCMoveTarget(p, tmp);
                }
            }
        }
        /// <summary>
        /// Starts training a specified unit
        /// </summary>
        /// <param name="trainee">Name of unit to train</param>
        public void startTraining(string trainee)
        {
            SCRTSAbility abil = getAbility(trainee);
            if(abil != null)
            {
                if(abil.AT == ABILITIES.TRAIN)
                {
                    builderName = abil.AbilityName;
                    maxBuildValue = (abil.Cost1 + abil.Cost2 + abil.Cost3) * 3;
                    buildName = abil.ActionName;
                    buildValue = 1;
                }
            }
        }
        /// <summary>
        /// Research ability and unlock when done
        /// </summary>
        public void research()
        {
            if(buildValue >= 1 && buildValue < maxBuildValue)
            {
                buildValue++;
            }
            else
            {
                buildValue = 0;
                commander.AbilityDB.getData(buildName).Locked = false;
                for(int a = 0; a < abilities.Count; a++)
                {
                    if(commander.AbilityDB.getData(a) is SCRTSUpgrade)
                    {
                        ((SCRTSUpgrade)abilities[a]).Researched = true;
                        commander.unlockAbility(abilities[a].ActionName);
                    }
                }
            }
        }
        /// <summary>
        /// Start researching specified ability
        /// </summary>
        /// <param name="study">Name of ability to research</param>
        public void startResearching(string study)
        {
            SCRTSAbility abil = getAbility(study);
            if(abil != null)
            {
                if(abil is SCRTSUpgrade)
                {
                    ((SCRTSUpgrade)abil).Researching = true;
                    builderName = abil.AbilityName;
                    maxBuildValue = (abil.Cost1 + abil.Cost2 + abil.Cost3) * 3;
                    buildName = abil.ActionName;
                    buildValue = 1;
                }
            }
        }
        /// <summary>
        /// Locks a specified ability
        /// </summary>
        /// <param name="abilityName">Specified ability name</param>
        public void lockAbility(string abilityName)
        {
            for(int a = 0; a < abilities.Count; a++)
            {
                if(abilities[a].AbilityName == abilityName)
                {
                    abilities[a].Locked = true;
                }
            }
        }
        /// <summary>
        /// Unlocks a specified ability
        /// </summary>
        /// <param name="abilityName">Specified ability name</param>
        public void unlockAbility(string abilityName)
        {
            for (int a = 0; a < abilities.Count; a++)
            {
                if (abilities[a].AbilityName == abilityName)
                {
                    abilities[a].Locked = false;
                }
            }
        }
        /// <summary>
        /// Research a specified ability
        /// </summary>
        /// <param name="abilityName">Specified ability name</param>
        public void researchAbility(string abilityName)
        {
            for(int a = 0; a < abilities.Count; a++)
            {
                if(abilities[a].AbilityName == abilityName)
                {
                    ((SCRTSUpgrade)abilities[a]).Researched = true;
                    ((SCRTSUpgrade)abilities[a]).Researching = false;
                }
            }
        }
        /// <summary>
        /// Unresearch a specified ability
        /// </summary>
        /// <param name="abilityName">Specified ability name</param>
        public void unresearchAbility(string abilityName)
        {
            for(int a = 0; a < abilities.Count; a++)
            {
                if(abilities[a].AbilityName == abilityName)
                {
                    ((SCRTSUpgrade)abilities[a]).Researched = false;
                    ((SCRTSUpgrade)abilities[a]).Researching = false;
                }
            }
        }
        /// <summary>
        /// Calculates sight range of unit based on action of
        /// abilities (speed * life span)
        /// </summary>
        public double GetSightRange
        {
            get
            {
                double rng = 0;
                double rngTmp = 0;
                SCRTSAction act = null;
                SCRTSAction actTmp = null;
                for(int a = 0; a < abilities.Count; a++)
                {
                    if(abilities[a].AT == ABILITIES.SHOOT ||
                        abilities[a].AT == ABILITIES.BOMBARD)
                    {
                        if(rng > 0)
                        {
                            actTmp = commander.ActionDB.getData(abilities[a].ActionName);
                            if(actTmp != null)
                            {
                                rngTmp = actTmp.Speed * actTmp.LifeSpan;
                            }
                            if(rngTmp > rng)
                            {
                                act = actTmp;
                                rng = rngTmp;
                            }
                        }
                        else
                        {
                            act = commander.ActionDB.getData(abilities[a].ActionName);
                            if(act != null)
                            {
                                rng = act.Speed * act.LifeSpan;
                            }
                        }
                    }
                }
                return rng;
            }
        }
        /// <summary>
        /// Applies hp and shield effects
        /// </summary>
        public void applyStatusEffect()
        {
            switch(status.SS)
            {
                case STATUSES.DAMAGE:
                    if(shields > 0)
                    {
                        shields -= 1;
                    }
                    else
                    {
                        if (hp > 0)
                        {
                            hp -= 1;
                        }
                    }
                    break;
                case STATUSES.REGEN:
                    if (shields < maxShields)
                    {
                        shields += 1;
                    }
                    break;
                case STATUSES.POISON:
                    if (hp > 0)
                    {
                        hp -= 1;
                    }
                    break;
                case STATUSES.HEAL:
                    if (hp < maxHP)
                    {
                        hp += 1;
                    }
                    break;
            }
        }
        /// <summary>
        /// Strike unit with damage while applying armor buff and debuffs
        /// </summary>
        /// <param name="dam">Damage value</param>
        public void strike(int dam)
        {
            if(status != null)
            {
                switch(status.SS)
                {
                    case STATUSES.BUFFARMOR:
                        dam = dam - (dam / 2);
                        if(dam < 1)
                        {
                            dam = 1;
                        }
                        hp -= dam;
                        break;
                    case STATUSES.DEBUFFARMOR:
                        dam = dam - (dam * 2);
                        hp -= dam;
                        break;
                }
            }
        }
        /// <summary>
        /// Sets the position of a specified turret to x and y values
        /// </summary>
        /// <param name="index">Turret index</param>
        /// <param name="tx">Target X postion</param>
        /// <param name="ty">Target Y postion</param>
        public void setTurretPos(int index, int tx, int ty)
        {
            if(index >= 0 && index < turrets.Count)
            {
                turrets[index].setPos(tx, ty);
            }
        }
        /// <summary>
        /// Returns a SCRenderTask object
        /// </summary>
        /// <returns>SCRenderTask object</returns>
        public SCRenderTask getRenderTask()
        {
            return new SCRenderTask(source, srcRect.x, srcRect.y, 
                srcRect.w, srcRect.h, destRect.x, destRect.y, 
                destRect.w, destRect.h, Bottom);

        }
        /// <summary>
        /// Adds a turret at the specified position relative unit's position
        /// </summary>
        /// <param name="turret">SCRTSTurret reference</param>
        /// <param name="x">X position on unit</param>
        /// <param name="y">Y position on unit</param>
        public void addTurret(SCRTSTurret turret, float x, float y)
        {
            turret.X = X + x;
            turret.Y = Y + y;
            turrets.Add(turret);
        }
        /// <summary>
        /// Overrides setPos, adds turret position setting in addition to base object
        /// </summary>
        /// <param name="x">X position in pixels</param>
        /// <param name="y">Y position in pixels</param>
        public override void setPos(float x, float y)
        {
            base.setPos(x, y);
            for(int t = 0; t < turrets.Count; t++)
            {
                turrets[t].IX = hitBox.IX + turrets[t].RelativePos.IX;
                turrets[t].IY = hitBox.IY + turrets[t].RelativePos.IY;
            }
        }
        /// <summary>
        /// Adds a build token for a specified cost and name, can only have up to ten tokens queued
        /// </summary>
        /// <param name="cost1">Cost 1</param>
        /// <param name="cost2">Cost 2</param>
        /// <param name="cost3">Cost 3</param>
        /// <param name="name">Name of unit/object to create</param>
        public void addBuildToken(int cost1, int cost2, int cost3, string name)
        {
            if (buildQueue.Count < 10)
            {
                buildQueue.Enqueue(new SCRTSBuildToken(cost1, cost2, cost3, name, this));
            }
        }
        /// <summary>
        /// Cancel a build token and recover spent resources
        /// </summary>
        public void cancelBuildToken()
        {
            if (buildToken != null)
            {
                commander.ResourceCount1 += buildToken.Cost1;
                commander.ResourceCount2 += buildToken.Cost2;
                commander.ResourceCount3 += buildToken.Cost3;
                buildToken = null;
            }
        }
        /// <summary>
        /// Turrets property
        /// </summary>
        public List<SCRTSTurret> Turrets
        {
            get { return turrets; }
        }
        /// <summary>
        /// Abiities property
        /// </summary>
        public List<SCRTSAbility> Abilities
        {
            get { return abilities; }
        }
        /// <summary>
        /// Shields property
        /// </summary>
        public int Shields
        {
            get { return shields; }
            set { shields = value; }
        }
        /// <summary>
        /// Commander property
        /// </summary>
        public SCRTSCommander Commander
        {
            get { return commander; }
            set { commander = value; }
        }
        /// <summary>
        /// IFF property
        /// </summary>
        public int IFF
        {
            get { return iff; }
            set { iff = value; }
        }
        /// <summary>
        /// Cargo property
        /// </summary>
        public List<SCRTSUnit> Cargo
        {
            get { return cargo; }
        }
        /// <summary>
        /// UNITTYPES property
        /// </summary>
        public UNITTYPES UT
        {
            get { return ut; }
            set { ut = value; }
        }
        /// <summary>
        /// UnitName property
        /// </summary>
        public string UnitName
        {
            get { return unitName; }
            set { unitName = value; }
        }
        /// <summary>
        /// Cost1 property
        /// </summary>
        public int Cost1
        {
            get { return cost1; }
            set { cost1 = value; }
        }
        /// <summary>
        /// Cost2 property
        /// </summary>
        public int Cost2
        {
            get { return cost2; }
            set { cost2 = value; }
        }
        /// <summary>
        /// Cost3 property
        /// </summary>
        public int Cost3
        {
            get { return cost3; }
            set { cost3 = value; }
        }
        /// <summary>
        /// Path property
        /// </summary>
        public List<Point2D> Path
        {
            get { return path; }
            set { path = value; }
        }
        /// <summary>
        /// Tank property
        /// </summary>
        public int Tank
        {
            get { return tank; }
            set { tank = value; }
        }
        /// <summary>
        /// TankLoad property
        /// </summary>
        public string TankLoad
        {
            get { return tankLoad; }
            set { tankLoad = value; }
        }
        /// <summary>
        /// TankMax property
        /// </summary>
        public int TankMax
        {
            get { return tankMax; }
            set { tankMax = value; }
        }
        /// <summary>
        /// StoredTarget property
        /// </summary>
        public Objective StoredTarget
        {
            get { return storedTarget; }
            set { StoredTarget = value; }
        }
        /// <summary>
        /// BuildName property
        /// </summary>
        public string BuildName
        {
            get { return buildName; }
            set { buildName = value; }
        }
        /// <summary>
        /// BuilderName property
        /// </summary>
        public string BuilderName
        {
            get { return builderName; }
            set { builderName = value; }
        }
        /// <summary>
        /// BuildValue property
        /// </summary>
        public int BuildValue
        {
            get { return buildValue; }
            set { buildValue = value; }
        }
        /// <summary>
        /// MaxBuildValue property
        /// </summary>
        public int MaxBuildValue
        {
            get { return maxBuildValue; }
            set { maxBuildValue = value; }
        }
        /// <summary>
        /// MaxHP property
        /// </summary>
        public int MaxHP
        {
            get { return maxHP; }
            set { maxHP = value; }
        }
        /// <summary>
        /// MaxShields property
        /// </summary>
        public int MaxShields
        {
            get { return maxShields; }
            set { maxShields = value; }
        }
        /// <summary>
        /// Status property
        /// </summary>
        public SCRTSStatus Status
        {
            get { return status; }
            set { status = value; }
        }
        /// <summary>
        /// TT property
        /// </summary>
        public TURRETTYPES TT
        {
            get { return tt; }
            set { tt = value; }
        }
        /// <summary>
        /// BuildQueue propertry
        /// </summary>
        public Queue<SCRTSBuildToken> BuildQueue
        {
            get { return buildQueue; }
        }
        /// <summary>
        /// BuildToken property
        /// </summary>
        public SCRTSBuildToken BldToken
        {
            get { return buildToken; }
            set { buildToken = value; }
        }
        /// <summary>
        /// RallyPoint property
        /// </summary>
        public Point2D RallyPoint
        {
            get { return rallyPoint; }
            set { rallyPoint = value; }
        }
    }
    public class SCRTSBuilding : SCRTSUnit
    {
        //protected
        protected BUILDINGTYPES bt;
        protected string buildingName;
        //protected Point2D rallyPoint;
        protected bool selfAssembling;

        //public
        /// <summary>
        /// SCRTSBuilding constructor
        /// </summary>
        /// <param name="name">Building name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="commander">Commander reference</param>
        /// <param name="hp">Hitpoints value</param>
        public SCRTSBuilding(string name, float x, float y, int width,
            int height, int numFrames, SCRTSCommander commander, string sourceName, int hp = 100) :
            base(name, x, y, width, height, numFrames, commander, sourceName, hp)
        {
            if (name == "")
            {
                buildingName = "default building";
            }
            else
            {
                buildingName = name;
            }
            //rallyPoint = new Point2D(MidBottom.X, MidBottom.Y + 32);
            selfAssembling = false;
        }
        /// <summary>
        /// RTSBuilding from file constructor 
        /// </summary>
        /// <param name="sr">StreamReader referecne</param>
        /// <param name="commander">SCRTSCommander referecne</param>
        public SCRTSBuilding(StreamReader sr, SCRTSCommander commander) : base(sr, commander)
        {
            sr.ReadLine();
            bt = (BUILDINGTYPES)Convert.ToInt32(sr.ReadLine());
            buildingName = sr.ReadLine();
            //rallyPoint = new Point2D(Convert.ToInt32(sr.ReadLine()),
            //    Convert.ToInt32(sr.ReadLine()));
        }
        /// <summary>
        /// SCRTSBuilding copy constructor
        /// </summary>
        /// <param name="obj">RTSBuilding reference</param>
        public SCRTSBuilding(SCRTSBuilding obj) : base(obj)
        {
            bt = obj.BT;
            buildingName = obj.BuildingName;
            //rallyPoint = new Point2D();
        }
        /// <summary>
        /// Save data override
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======SCRTSBuilding Data======");
            sw.WriteLine((int)bt);
            sw.WriteLine(buildingName);
            //sw.WriteLine(rallyPoint.IX);
            //sw.WriteLine(rallyPoint.IY);
        }
        /// <summary>
        /// Updates RTSBuilding internal state
        /// </summary>
        public override void update()
        {
            base.update();
            if(buildName != "" && buildName != " ")
            {
                if(buildValue < maxBuildValue)
                {
                    buildValue++;
                }
                else
                {
                    completeBuild();
                }
            }
        }
        /// <summary>
        /// Creates a new unit object below bottom middle 
        /// and gives it a path to rally point
        /// </summary>
        public void completeBuild()
        {
            buildName = "";
            buildValue = 0;
        }
        /// <summary>
         /// Draws building
         /// </summary>
         /// <param name="renderer">Renderer reference</param>
         /// <param name="winx">Window X value</param>
         /// <param name="winy">Window Y value</param>
        public override void draw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            base.draw(renderer, winx, winy);
            for (int t = 0; t < turrets.Count; t++)
            {
                turrets[t].draw(renderer, winx + center.IX, winy + center.IY);
            }
        }
        /// <summary>
        /// Draws buildBar only
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Winx value</param>
        /// <param name="winy">Winy value</param>
        public void drawBuildBar(IntPtr renderer, int winx = 0, int winy = 0)
        {
            buildBar.draw(renderer, 3, winx, winy);
        }
        /// <summary>
        /// Overrides RTSObject scale function to account for turrets
        /// </summary>
        /// <param name="val">Scaling value</param>
        public override void scale(float val)
        {
            float dist = 0;
            double ang = 0;
            for (int t = 0; t < turrets.Count; t++)
            {
                dist = Point2D.calculateDistance(BoxCenter, turrets[t].Pivot);
                ang = Point2D.CalcAngle(BoxCenter, turrets[t].Pivot);
                dist *= val;
                turrets[t].X = (float)(((Math.Cos(ang) * dist) + BoxCenter.X) * val) - turrets[t].Pivot.X;
                turrets[t].Y = (float)(((Math.Sin(ang) * dist) + BoxCenter.Y) * val) - turrets[t].Pivot.Y;
            }
            base.scale(val);
        }
        /// <summary>
        /// BT property
        /// </summary>
        public BUILDINGTYPES BT
        {
            get { return bt; }
            set { bt = value; }
        }
        /// <summary>
        /// BuildingName property
        /// </summary>
        public string BuildingName
        {
            get { return buildingName; }
            set { buildingName = value; }
        }
        /*
        /// <summary>
        /// RallyPoint property
        /// </summary>
        public Point2D RallyPoint
        {
            get { return rallyPoint; }
            set { rallyPoint = value; }
        }
        */
        /// <summary>
        /// SelfAssembling property
        /// </summary>
        public bool SelfAssembling
        {
            get { return selfAssembling; }
            set { selfAssembling = value; }
        }
    }
    public class SCRTSFoundation : SCRTSBuilding
    {
        //protected
        protected string foundationName;

        //public 
        /// <summary>
        /// SCRTSFoundation constructor
        /// </summary>
        /// <param name="name">Foundation name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="sourceName">Source name value</param>
        /// <param name="hp">Hitpoint value</param>
        public SCRTSFoundation(string name, float x, float y, int width,
            int height, int numFrames, SCRTSCommander commander, string sourceName, int hp = 100) :
            base(name, x, y, width, height, numFrames, commander, sourceName, hp)
        {
            if (name == "")
            {
                foundationName = "default foundation";
            }
            else
            {
                foundationName = name;
            }
            bt = BUILDINGTYPES.FOUNDATION;
        }
        /// <summary>
        /// SCRTSFoundation from file constructor 
        /// </summary>
        /// <param name="sr">StreamReader referecne</param>
        /// <param name="commander">RTSCommander referecne</param>
        public SCRTSFoundation(StreamReader sr, SCRTSCommander commander) : base(sr, commander)
        {
            sr.ReadLine();
            foundationName = sr.ReadLine();
        }
        /// <summary>
        /// SCRTSFoundation copy constructor
        /// </summary>
        /// <param name="obj">SCRTSFoundation reference</param>
        public SCRTSFoundation(SCRTSFoundation obj) : base(obj)
        {
            foundationName = obj.FoundationName;
        }
        /// <summary>
        /// RTSFoundation copy constructor from RTSBuilding object
        /// </summary>
        /// <param name="obj">RTSBuilding reference</param>
        public SCRTSFoundation(SCRTSBuilding obj) : base(obj)
        {
            foundationName = obj.BuildingName;
            bt = BUILDINGTYPES.FOUNDATION;
        }
        /// <summary>
        /// Save data override
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======SCRTSFoundation Data======");
            sw.WriteLine(foundationName);
        }
        /// <summary>
        /// Updates SCRTSBuilding internal state
        /// </summary>
        public override void update()
        {
            base.update();
        }
        /// <summary>
        /// FoundationName property
        /// </summary>
        public string FoundationName
        {
            get { return foundationName; }
            set { foundationName = value; }
        }
    }
    public class SCRTSResource : SCRTSObject
    {
        //protected
        protected string resourceName;
        protected int maxQuantity;
        protected int quantity;
        protected int growthRate;
        protected int grown;
        protected bool grows;
        protected bool solid;


        //public 
        /// <summary>
        /// SCRTSResource constructor
        /// </summary>
        /// <param name="name">Resource name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="sourceName">Source name value</param>
        /// <param name="hp">Hitpoint value</param>
        public SCRTSResource(string name, float x, float y, int width,
            int height, int numFrames, string sourceName, int hp = 100) :
            base(name, x, y, width, height, numFrames, sourceName, hp)
        {
            if (name == "")
            {
                resourceName = "default resource";
            }
            else
            {
                resourceName = name;
            }
            maxQuantity = 10000;
            quantity = (maxQuantity / numFrames);
            growthRate = 5000;
            grown = 0;
            grows = false;
            solid = false;
            this.sourceName = sourceName;
        }
        /// <summary>
        /// SCRTSResource from file constructor 
        /// </summary>
        /// <param name="sr">StreamReader referecne</param>
        public SCRTSResource(StreamReader sr) : base(sr)
        {
            sr.ReadLine();
            resourceName = sr.ReadLine();
            maxQuantity = Convert.ToInt32(sr.ReadLine());
            quantity = Convert.ToInt32(sr.ReadLine());
            growthRate = Convert.ToInt32(sr.ReadLine());
            grown = Convert.ToInt32(sr.ReadLine());
            string buffer = sr.ReadLine();
            if (buffer == "TRUE")
            {
                grows = true;
            }
            else
            {
                grows = false;
            }
            buffer = sr.ReadLine();
            if (buffer == "TRUE")
            {
                solid = true;
            }
            else
            {
                solid = false;
            }
            sourceName = sr.ReadLine();
            source = TextureBank.getTexture(sourceName);
        }
        /// <summary>
        /// SCRTSResource copy constructor
        /// </summary>
        /// <param name="obj">RTSResource reference</param>
        public SCRTSResource(SCRTSResource obj) : base(obj)
        {
            resourceName = obj.ResourceName;
            maxQuantity = obj.MaxQuantity;
            quantity = obj.Quantity;
            growthRate = obj.GrowthRate;
            grown = obj.Grown;
            grows = obj.Grows;
            solid = obj.Solid;
        }
        /// <summary>
        /// Save data override
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======SCRTSResource Data======");
            sw.WriteLine(resourceName);
            sw.WriteLine(maxQuantity);
            sw.WriteLine(quantity);
            sw.WriteLine(growthRate);
            sw.WriteLine(grown);
            if (grows == true)
            {
                sw.WriteLine("TRUE");
            }
            else
            {
                sw.WriteLine("FALSE");
            }
            if(solid == true)
            {
                sw.WriteLine("TRUE");
            }
            else
            {
                sw.WriteLine("FALSE");
            }
            sw.WriteLine(sourceName);
        }
        /// <summary>
        /// Updates RTSResource internal state
        /// </summary>
        public override void update()
        {
            grown++;
        }
        /// <summary>
        /// Advances resource frame by 1, add maxQuanity / numFrames
        /// to quanity and returns true else returns false
        /// </summary>
        /// <returns>Boolean</returns>
        public bool spread()
        {
            if (grown >= growthRate)
            {
                if (frame < numFrames)
                {
                    quantity += (maxQuantity / numFrames);
                    frame++;
                }
                grown = 0;
                return true;
            }
            return false;
        }
        /// <summary>
        /// ResourceName property
        /// </summary>
        public string ResourceName
        {
            get { return resourceName; }
            set { resourceName = value; }
        }
        /// <summary>
        /// MaxQuanity property
        /// </summary>
        public int MaxQuantity
        {
            get { return maxQuantity; }
            set { maxQuantity = value; }
        }
        /// <summary>
        /// Quanity property
        /// </summary>
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        /// <summary>
        /// GrowthRate property
        /// </summary>
        public int GrowthRate
        {
            get { return growthRate; }
            set { growthRate = value; }
        }
        /// <summary>
        /// Grown property
        /// </summary>
        public int Grown
        {
            get { return grown; }
            set { grown = value; }
        }
        /// <summary>
        /// Grows property
        /// </summary>
        public bool Grows
        {
            get { return grows; }
            set { grows = value; }
        }
        /// <summary>
        /// Solid property
        /// </summary>
        public bool Solid
        {
            get { return solid; }
            set { solid = value; }
        }
    }
    public class SCRTSResourceField
    {
        //protected
        protected DataGrid<SCRTSResource> fields;
        protected int width;
        protected int height;
        protected GenericBank<SCRTSResource> resourceDB;
        protected Random rand;

        //public
        /// <summary>
        /// SCRTSResourceField constructor
        /// </summary>
        /// <param name="w">Width in hexes</param>
        /// <param name="h">Height in hexes</param>
        public SCRTSResourceField(int w, int h)
        {
            width = w;
            height = h;
            fields = new DataGrid<SCRTSResource>(w, h);
            resourceDB = null;
            rand = new Random((int)System.DateTime.Today.Ticks);
        }
        /// <summary>
        /// SCRTSResourceField from file constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public SCRTSResourceField(StreamReader sr)
        {
            sr.ReadLine();
            width = Convert.ToInt32(sr.ReadLine());
            height = Convert.ToInt32(sr.ReadLine());
            fields = new DataGrid<SCRTSResource>(width, height);
            string buffer = "";
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    buffer = sr.ReadLine();
                    if (buffer == "TRUE")
                    {
                        fields.Grid[x, y] = new SCRTSResource(sr);
                    }
                }
            }
            resourceDB = null;
            rand = new Random((int)System.DateTime.Today.Ticks);
        }
        /// <summary>
        /// SCRTSResourceField copy constructor
        /// </summary>
        /// <param name="obj">SCRTSResourceField reference</param>
        public SCRTSResourceField(SCRTSResourceField obj)
        {
            width = obj.Width;
            height = obj.Height;
            fields = new DataGrid<SCRTSResource>(width, height);
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    fields.Grid[x, y] = obj.Fields.Grid[x, y];
                }
            }
            resourceDB = obj.ResourceDB;
            rand = new Random((int)System.DateTime.Today.Ticks);
        }
        /// <summary>
        /// Saves RTSResourceField data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveData(StreamWriter sw)
        {
            sw.WriteLine("======SCRTSResourceField Data======");
            sw.WriteLine(width);
            sw.WriteLine(height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (fields.Grid[x, y] != null)
                    {
                        sw.WriteLine("TRUE");
                        fields.Grid[x, y].saveData(sw);
                    }
                    else
                    {
                        sw.WriteLine("FALSE");
                    }
                }
            }
        }
        /// <summary>
        /// Updates RTSResourceField internal state
        /// </summary>
        /// <param name="cell">HexCell reference</param>
        public void update(RTSCell cell)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if(fields.Grid[x, y].Grows == true)
                    {
                        fields.Grid[x, y].update();
                        if(fields.Grid[x, y].spread() == true)
                        {
                            switch(rand.Next(0, 600) / 100)
                            {
                                case 0:
                                    if(cell.TM.inDomain(x, y - 1) == true)
                                    {
                                        if(cell.TM.getValue(x, y - 1) > -1)
                                        {
                                            if(fields.Grid[x, y - 1] == null)
                                            {
                                                fields.Grid[x, y - 1] = new SCRTSResource(fields.Grid[x, y].ResourceName, 
                                                    (x * 32) + cell.CellX * 9600, ((y - 1) * 32) + cell.CellY * 9600, 32, 32, 1, 
                                                    fields.Grid[x, y].SourceName);
                                            }
                                        }
                                    }
                                    break;
                                case 1:
                                    if (cell.TM.inDomain(x + 1, y - 1) == true)
                                    {
                                        if (cell.TM.getValue(x + 1, y - 1) > -1)
                                        {
                                            if (fields.Grid[x + 1, y - 1] == null)
                                            {
                                                fields.Grid[x, y - 1] = new SCRTSResource(fields.Grid[x, y].ResourceName,
                                                    ((x + 1) * 32) + cell.CellX * 9600, ((y - 1) * 32) + cell.CellY * 9600, 32, 32, 1,
                                                    fields.Grid[x, y].SourceName);
                                            }
                                        }
                                    }
                                    break;
                                case 2:
                                    if (cell.TM.inDomain(x - 1, y - 1) == true)
                                    {
                                        if (cell.TM.getValue(x - 1, y - 1) > -1)
                                        {
                                            if (fields.Grid[x - 1, y - 1] == null)
                                            {
                                                fields.Grid[x - 1, y - 1] = new SCRTSResource(fields.Grid[x, y].ResourceName,
                                                    ((x - 1) * 32) + cell.CellX * 9600, ((y - 1) * 32) + cell.CellY * 9600, 32, 32, 1,
                                                    fields.Grid[x, y].SourceName);
                                            }
                                        }
                                    }
                                    break;
                                case 3:
                                    if (cell.TM.inDomain(x + 1, y) == true)
                                    {
                                        if (cell.TM.getValue(x + 1, y) > -1)
                                        {
                                            if (fields.Grid[x + 1, y] == null)
                                            {
                                                fields.Grid[x + 1, y] = new SCRTSResource(fields.Grid[x, y].ResourceName,
                                                    ((x + 1) * 32) + cell.CellX * 9600, ((y) * 32) + cell.CellY * 9600, 32, 32, 1,
                                                    fields.Grid[x, y].SourceName);
                                            }
                                        }
                                    }
                                    break;
                                case 4:
                                    if (cell.TM.inDomain(x - 1, y) == true)
                                    {
                                        if (cell.TM.getValue(x - 1, y) > -1)
                                        {
                                            if (fields.Grid[x - 1, y] == null)
                                            {
                                                fields.Grid[x - 1, y] = new SCRTSResource(fields.Grid[x, y].ResourceName,
                                                    ((x - 1) * 32) + cell.CellX * 9600, ((y) * 32) + cell.CellY * 9600, 32, 32, 1,
                                                    fields.Grid[x, y].SourceName);
                                            }
                                        }
                                    }
                                    break;
                                case 5:
                                    if (cell.TM.inDomain(x, y + 1) == true)
                                    {
                                        if (cell.TM.getValue(x, y + 1) > -1)
                                        {
                                            if (fields.Grid[x, y + 1] == null)
                                            {
                                                fields.Grid[x, y + 1] = new SCRTSResource(fields.Grid[x, y].ResourceName,
                                                    ((x ) * 32) + cell.CellX * 9600, ((y + 1) * 32) + cell.CellY * 9600, 32, 32, 1,
                                                    fields.Grid[x, y].SourceName);
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Fields property
        /// </summary>
        public DataGrid<SCRTSResource> Fields
        {
            get { return fields; }
        }
        /// <summary>
        /// Width property
        /// </summary>
        public int Width
        {
            get { return width; }
        }
        /// <summary>
        /// Height property
        /// </summary>
        public int Height
        {
            get { return height; }
        }
        /// <summary>
        /// ResourceDB property
        /// </summary>
        public GenericBank<SCRTSResource> ResourceDB
        {
            get { return resourceDB; }
            set { resourceDB = value; }
        }
    }
    public class SCRTSAction : SCRTSObject
    {
        //protected
        protected ACTIONS act;
        protected string status;
        protected int lifeSpan;
        protected int lifeSpent;
        protected int owner;
        protected string secondaryAction;
        protected Point2D startPos;
        protected XENOCOLOURS colour;
        protected int power;
        protected string buff;
        protected string actionName;
        //protected bool isAir;

        //public
        /// <summary>
        /// RTSAction constructor
        /// </summary>
        /// <param name="name">Graphic source name</param>
        /// <param name="x">X position in pixels</param>
        /// <param name="y">Y position in pixels</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="sourceName">Name of source</param>
        /// <param name="act">ACTIONS value</param>
        /// <param name="status">Status value</param>
        /// <param name="lifeSpan">LifeSpan value</param>
        /// <param name="owner">Faction index value</param>
        /// <param name="secondaryAction">Secondary action name</param>
        /// <param name="colour">XENOCOLOUR value</param>
        /// <param name="power">Power value</param>
        /// <param name="buff">RTSStatus name</param>
        public SCRTSAction(string name, float x, float y, int width,
            int height, int numFrames, string sourceName, 
            ACTIONS act = ACTIONS.MISSILE, STATUSES status = STATUSES.DAMAGE, 
            int lifeSpan = 100, int owner = -1, string secondaryAction = "",
            XENOCOLOURS colour = XENOCOLOURS.WHITE, int power = 10, 
            string buff = "") ://, bool isAir = false) :
            base(name, x, y, width, height, numFrames, sourceName, 10)
        {
            this.act = act;
            this.status = "";
            this.lifeSpan = lifeSpan;
            lifeSpent = 0;
            this.owner = owner;
            this.secondaryAction = secondaryAction;
            startPos = new Point2D(x, y);
            this.colour = colour;
            this.power = power;
            this.buff = buff;
            //this.isAir = isAir;
            if (name == "")
            {
                actionName = "default action";
            }
            else
            {
                actionName = name;
            }
        }
        /// <summary>
        /// RTSAction from file constructor 
        /// </summary>
        /// <param name="sr">StreamReader referecne</param>
        public SCRTSAction(StreamReader sr) : base(sr)
        {
            sr.ReadLine();
            act = (ACTIONS)Convert.ToInt32(sr.ReadLine());
            status = sr.ReadLine();
            lifeSpan = Convert.ToInt32(sr.ReadLine());
            lifeSpent = Convert.ToInt32(sr.ReadLine());
            owner = Convert.ToInt32(sr.ReadLine());
            secondaryAction = sr.ReadLine();
            startPos = new Point2D((float)Convert.ToDecimal(sr.ReadLine()),
                (float)Convert.ToDecimal(sr.ReadLine()));
            colour = (XENOCOLOURS)Convert.ToInt32(sr.ReadLine());
            power = Convert.ToInt32(sr.ReadLine());
            buff = sr.ReadLine();
            /*
            if(buff == "TRUE")
            {
                isAir = true;
            }
            else
            {
                isAir = false;
            }
            */
            buff = sr.ReadLine();
            actionName = "default action";
        }
        /// <summary>
        /// RTSAction copy constructor
        /// </summary>
        /// <param name="obj">RTSAction reference</param>
        public SCRTSAction(SCRTSAction obj) : base(obj)
        {
            act = obj.Act;
            status = obj.Status;
            lifeSpan = obj.LifeSpan;
            lifeSpent = obj.LifeSpent;
            owner = obj.Owner;
            secondaryAction = obj.SecondaryAction;
            startPos = new Point2D(obj.StartPos.X, obj.StartPos.Y);
            colour = obj.Colour;
            power = obj.Power;
            buff = obj.Buff;
            //isAir = obj.IsAir;
            actionName = obj.ActionName;
        }
        /// <summary>
        /// Save data override
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======SCRTSAction Data======");
            sw.WriteLine(((int)act).ToString());
            sw.WriteLine(status);
            sw.WriteLine(lifeSpan);
            sw.WriteLine(lifeSpent);
            sw.WriteLine(owner);
            sw.WriteLine(secondaryAction);
            sw.WriteLine(startPos.X);
            sw.WriteLine(startPos.Y);
            sw.WriteLine((int)colour);
            sw.WriteLine(power);
            sw.WriteLine(buff);
            /*
            if(isAir == true)
            {
                sw.WriteLine("TRUE");
            }
            else
            {
                sw.WriteLine("False");
            }
            */
            sw.WriteLine(actionName);
        }
        /// <summary>
        /// Updates RTSAction internal state
        /// </summary>
        public override void update()
        {
            move(selfAngle, speed);
        }
        /// <summary>
        /// Override draw
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window X offset</param>
        /// <param name="winy">Window Y offset</param>
        public override void draw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            if (act == ACTIONS.BEAM)
            {
                Point2D a = new Point2D(startPos.X - winx, startPos.Y - winy);
                Point2D b = new Point2D(hitBox.X - winx, hitBox.Y - winy);
                DrawLine.draw(renderer, a, b, ColourBank.getColour(colour), winx, winy);
            }
            else
            {
                base.draw(renderer, winx, winy);
            }
        }
        /// <summary>
        /// Act property
        /// </summary>
        public ACTIONS Act
        {
            get { return act; }
            set { act = value; }
        }
        /// <summary>
        /// Status property
        /// </summary>
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        /// <summary>
        /// LifeSpan property
        /// </summary>
        public int LifeSpan
        {
            get { return lifeSpan; }
            set { lifeSpan = value; }
        }
        /// <summary>
        /// LifeSpent property
        /// </summary>
        public int LifeSpent
        {
            get { return lifeSpent; }
            set { lifeSpent = value; }
        }
        /// <summary>
        /// Owner property
        /// </summary>
        public int Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        /// <summary>
        /// SecondaryAction property
        /// </summary>
        public string SecondaryAction
        {
            get { return secondaryAction; }
            set { secondaryAction = value; }
        }
        /// <summary>
        /// StartPos property
        /// </summary>
        public Point2D StartPos
        {
            get { return startPos; }
            set { startPos = value; }
        }
        /// <summary>
        /// Colour property
        /// </summary>
        public XENOCOLOURS Colour
        {
            get { return colour; }
            set { colour = value; }
        }
        /// <summary>
        /// Power property
        /// </summary>
        public int Power
        {
            get { return power; }
            set { power = value; }
        }
        /// <summary>
        /// Buff property
        /// </summary>
        public string Buff
        {
            get { return buff; }
            set { buff = value; }
        }
        /// <summary>
        /// Done property
        /// </summary>
        public bool Done
        {
            get
            {
                if(lifeSpent < lifeSpan)
                {
                    return false;
                }
                return false;
            }
        }
        /// <summary>
        /// ActionName property
        /// </summary>
        public string ActionName
        {
            get { return actionName; }
            set { actionName = value; }
        }
        /*
        /// <summary>
        /// IsAir property
        /// </summary>
        public bool IsAir
        {
            get { return isAir; }
            set { isAir = value; }
        }
        */
    }
    public class SCRTSStatus : SCRTSObject
    {
        //protected
        protected STATUSES ss;
        protected int lifeSpan;
        protected int lifeSpent;
        protected string statusName;

        //public 
        /// <summary>
        /// RTSStatus contructor
        /// </summary>
        /// <param name="name">Graphic source name</param>
        /// <param name="x">X position in pixels</param>
        /// <param name="y">Y position in pixels</param>
        /// <param name="width">Source and defualt hitbox width in pixels</param>
        /// <param name="height">Source and defualt hitbox height in pixels</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="sourceName">Name of source</param>
        /// <param name="hp">Hitpoints value</param>
        public SCRTSStatus(string name, float x, float y, int width,
            int height, int numFrames, string sourceName, int hp = 100) :
            base(name, x, y, width, height, numFrames, sourceName, hp)
        {
            ss = STATUSES.NONE;
            lifeSpan = 600;
            lifeSpent = 0;
            if (name == "")
            {
                statusName = "default status";
            }
            else
            {
                statusName = name;
            }
        }
        /// <summary>
        /// RTSStatus from file constructor 
        /// </summary>
        /// <param name="sr">StreamReader referecne</param>
        public SCRTSStatus(StreamReader sr) : base(sr)
        {
            sr.ReadLine();
            ss = (STATUSES)Convert.ToInt32(sr.ReadLine());
            lifeSpan = Convert.ToInt32(sr.ReadLine());
            lifeSpent = Convert.ToInt32(sr.ReadLine());
            statusName = sr.ReadLine();
        }
        /// <summary>
        /// RTSStatus copy constructor
        /// </summary>
        /// <param name="obj">SCRTSStatus reference</param>
        public SCRTSStatus(SCRTSStatus obj) : base(obj)
        {
            ss = obj.SS;
            lifeSpan = obj.LifeSpan;
            LifeSpent = obj.LifeSpent;
            statusName = obj.StatusName;
        }
        /// <summary>
        /// Save data override
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======SCRTSStatus======");
            sw.WriteLine((int)ss);
            sw.WriteLine(lifeSpan);
            sw.WriteLine(lifeSpent);
            sw.WriteLine(statusName);
        }
        /// <summary>
        /// Updates RTSStatus internal state
        /// </summary>
        public override void update()
        {
            lifeSpent++;
        }
        /// <summary>
        /// Sets destRect dimensions provided a Rectangle
        /// </summary>
        /// <param name="ownerBox">Rectangle reference</param>
        public void matchHitBox(Rectangle ownerBox)
        {
            destRect.w = (int)ownerBox.Width;
            destRect.h = (int)ownerBox.Height;
        }
        /// <summary>
        /// Done property
        /// </summary>
        public bool Done
        {
            get
            {
                if(lifeSpent >= lifeSpan)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// SS property
        /// </summary>
        public STATUSES SS
        {
            get { return ss; }
            set { ss = value; }
        }
        /// <summary>
        /// LifeSpan property
        /// </summary>
        public int LifeSpan
        {
            get { return lifeSpan; }
            set { lifeSpan = value; }
        }
        /// <summary>
        /// LifeSpent property
        /// </summary>
        public int LifeSpent
        {
            get { return lifeSpent; }
            set { lifeSpent = value; }
        }
        /// <summary>
        /// StatusName property
        /// </summary>
        public string StatusName
        {
            get { return statusName; }
            set { statusName = value; }
        }
    }
    public class SCRTSAbility : SCRTSObject
    {
        //protected
        protected string abilityName;
        protected CoolDown recharge;
        protected ABILITIES at;
        protected bool targetting;
        protected bool targetted;
        protected string actionName;
        protected SCRTSTurret turret;
        protected int turretIndex;
        protected SCRTSUnit owner;
        protected Point2D clickPoint;
        protected Point2D targetPoint;
        protected bool locked;
        protected bool autoCast;
        protected int cost1;
        protected int cost2;
        protected int cost3;

        //public
        /// <summary>
        /// RTSAbility constructor
        /// </summary>
        /// <param name="name">Graphic source name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Hegiht in pixels</param>
        /// <param name="numFrames">Number of frame</param>
        /// <param name="sourceName">Name of source</param>
        /// <param name="hp">HP value</param>
        public SCRTSAbility(string name, float x, float y, int width,
            int height, int numFrames, string sourceName, int hp = 100) :
            base(name, x, y, width, height, numFrames, sourceName, hp)
        {
            if (name == "")
            {
                abilityName = "default ability";
            }
            else
            {
                abilityName = name;
            }
            recharge = new CoolDown(60);
            at = ABILITIES.NONE;
            targetting = false;
            targetted = false;
            actionName = "";
            turret = null;
            turretIndex = -1;
            owner = null;
            still = true;
            targetPoint = null;
            clickPoint = new Point2D();
            locked = false;
            autoCast = true;
            cost1 = 0;
            cost2 = 0;
            cost3 = 0;
        }
        /// <summary>
        /// RTSAbility from file constructor 
        /// </summary>
        /// <param name="sr">StreamReader referecne</param>
        public SCRTSAbility(StreamReader sr) : base(sr)
        {
            sr.ReadLine();
            abilityName = sr.ReadLine();
            recharge = new CoolDown(Convert.ToInt32(sr.ReadLine()));
            at = (ABILITIES)Convert.ToInt32(sr.ReadLine());
            string buffer = sr.ReadLine();
            if(buffer == "TRUE")
            {
                targetting = true;
            }
            else
            {
                targetting = false;
            }
            buffer = sr.ReadLine();
            if (buffer == "TRUE")
            {
                targetted = true;
            }
            else
            {
                targetted = false;
            }
            actionName = sr.ReadLine();
            turretIndex = Convert.ToInt32(sr.ReadLine());
            turret = null;
            owner = null;
            still = true;
            targetPoint = null;
            clickPoint = new Point2D();
            locked = Convert.ToBoolean(sr.ReadLine());
            buffer = sr.ReadLine();
            if (buffer == "TRUE")
            {
                autoCast = true;
            }
            else
            {
                autoCast = false;
            }
            cost1 = Convert.ToInt32(sr.ReadLine());
            cost2 = Convert.ToInt32(sr.ReadLine());
            cost3 = Convert.ToInt32(sr.ReadLine());
        }
        /// <summary>
        /// RTSAbility copy constructor
        /// </summary>
        /// <param name="obj">SCRTSAbility reference</param>
        public SCRTSAbility(SCRTSAbility obj) : base(obj)
        {
            abilityName = obj.AbilityName;
            recharge = new CoolDown(obj.Recharge);
            at = obj.AT;
            targetting = obj.Targetting;
            targetted = obj.Targetted;
            actionName = obj.ActionName;
            turret = obj.Turret;
            turretIndex = obj.TurretIndex;
            turret = obj.Turret;
            owner = obj.Owner;
            still = true;
            targetPoint = null;
            clickPoint = new Point2D();
            locked = obj.Locked;
            autoCast = obj.AutoCast;
            cost1 = obj.Cost1;
            cost2 = obj.Cost2;
            cost3 = obj.Cost3;
        }
        /// <summary>
        /// Save data override
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======SCRTSAbility Data======");
            sw.WriteLine(abilityName);
            sw.WriteLine(recharge.MaxTicks);
            sw.WriteLine((int)at);
            if(targetting == true)
            {
                sw.WriteLine("TRUE");
            }
            else
            {
                sw.WriteLine("FALSE");
            }
            if (targetted == true)
            {
                sw.WriteLine("TRUE");
            }
            else
            {
                sw.WriteLine("FALSE");
            }
            sw.WriteLine(actionName);
            sw.WriteLine(turretIndex);
            sw.WriteLine(locked);
            if (autoCast == true)
            {
                sw.WriteLine("TRUE");
            }
            else
            {
                sw.WriteLine("FALSE");
            }
            sw.WriteLine(cost1);
            sw.WriteLine(cost2);
            sw.WriteLine(cost3);
        }
        /// <summary>
        /// Updates RTSAbility internal state
        /// </summary>
        public void update(SCRTSCommander faction)
        {
            recharge.update();
        }
        /// <summary>
        /// Ability draw override
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window X offset</param>
        /// <param name="winy">Window Y offset</param>
        public override void draw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            base.draw(renderer, winx, winy);
            if(recharge.Active == true)
            {
                int tx = SimpleFont.stringRenderWidth(recharge.Percentage.ToString(), 0.75f);
                SimpleFont.drawColourString(renderer, recharge.Percentage.ToString(), 
                    center.IX - (tx / 2), center.IY - 8, "yellow", 0.75f);
            }
        }
        /// <summary>
        /// Returns true if ability icon is clicked else returns false
        /// </summary>
        /// <returns>Boolean</returns>
        public bool clicked()
        {
            if (locked == false)
            {
                if (MouseHandler.getLeft() == true)
                {
                    clickPoint.X = MouseHandler.getMouseX();
                    clickPoint.Y = MouseHandler.getMouseY();
                    if (hitBox.pointInRect(clickPoint))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Attempt's to activate ability
        /// </summary>
        public void activate()
        {
            if (recharge.Active == false)
            {
                recharge.activate();
            }
        }
        /// <summary>
        /// Attempts to perform ability
        /// <param name="amp">Amplification value</param>
        /// </summary>
        public void performAbility(int amp = 1)
        {
            SCRTSAction tmp = null;
            if(recharge.Active == false)
            {
                if(at == ABILITIES.UPGRADE)
                {
                    owner.startTraining(actionName);
                }
                else if(at == ABILITIES.TRAIN)
                {
                    owner.startResearching(actionName);
                }
                else
                {
                    tmp = new SCRTSAction(owner.Commander.ActionDB.getData(actionName));
                    tmp.SelfAngle = turret.SelfAngle;
                    tmp.setPos(turret.TurretTip.X, turret.TurretTip.Y);
                    tmp.Power += amp;
                    owner.Commander.Actions.Add(tmp);
                }
                recharge.activate();
            }
        }
        /// <summary>
        /// Checks if linked turret is locked, always returns true
        /// when no turret linked
        /// </summary>
        /// <param name="target">Point2D reference</param>
        /// <returns>Boolean</returns>
        public bool linkedTurretLocked(Point2D target)
        {
            if(turret == null)
            {
                return true;
            }
            return turret.turretLock(target);
        }
        /// <summary>
        /// Returns recharging status
        /// </summary>
        public bool Recharging
        {
            get { return recharge.Active; }
        }
        /// <summary>
        /// AbilityName property
        /// </summary>
        public string AbilityName
        {
            get { return abilityName; }
            set { abilityName = value; }
        }
        /// <summary>
        /// Recharge property
        /// </summary>
        public int Recharge
        {
            get { return recharge.MaxTicks; }
            set { recharge = new CoolDown(value); }
        }
        /// <summary>
        /// AT property
        /// </summary>
        public ABILITIES AT
        {
            get { return at; }
            set { at = value; }
        }
        /// <summary>
        /// Targetting property
        /// </summary>
        public bool Targetting
        {
            get { return targetting; }
            set { targetting = value; }
        }
        /// <summary>
        /// Targetted property
        /// </summary>
        public bool Targetted
        {
            get { return targetted; }
            set { targetted = value; }
        }
        /// <summary>
        /// ActionName property
        /// </summary>
        public string ActionName
        {
            get { return actionName; }
            set { actionName = value; }
        }
        /// <summary>
        /// Turret property
        /// </summary>
        public SCRTSTurret Turret
        {
            get { return turret; }
            set { turret = value; }
        }
        /// <summary>
        /// TurretIndex property
        /// </summary>
        public int TurretIndex
        {
            get { return turretIndex; }
            set { turretIndex = value; }
        }
        /// <summary>
        /// Owner property
        /// </summary>
        public SCRTSUnit Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        /// <summary>
        /// TargetPoint property
        /// </summary>
        public Point2D TargetPoint
        {
            get { return targetPoint; }
            set { targetPoint = value; }
        }
        /// <summary>
        /// Locked property
        /// </summary>
        public bool Locked
        {
            get { return locked; }
            set { locked = value; }
        }
        /// <summary>
        /// AutoCast property
        /// </summary>
        public bool AutoCast
        {
            get { return autoCast; }
            set { autoCast = value; }
        }
        /// <summary>
        /// Cost1 property
        /// </summary>
        public int Cost1
        {
            get { return cost1; }
            set { cost1 = value; }
        }
        /// <summary>
        /// Cost2 property
        /// </summary>
        public int Cost2
        {
            get { return cost2; }
            set { cost2 = value; }
        }
        /// <summary>
        /// Cost3 property
        /// </summary>
        public int Cost3
        {
            get { return cost3; }
            set { cost3 = value; }
        }
    }
    public class SCRTSUpgrade : SCRTSAbility
    {
        //protected
        protected bool researched;
        protected bool researching;
        protected int buildBlocks;
        protected int builtBlocks;

        //public
        /// <summary>
        /// RTSUpgrade constructor
        /// </summary>
        /// <param name="name">Graphic source name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="sourceName">Name of source</param>
        /// <param name="hp">HP value</param>
        public SCRTSUpgrade(string name, float x, float y, int width,
            int height, int numFrames, string sourceName, int hp = 100) :
            base(name, x, y, width, height, numFrames, sourceName, hp)
        {
            researched = false;
            researching = false;
            buildBlocks = 18000;
            builtBlocks = 0;
        }
        /// <summary>
        /// RTSUpgrade from file constructor 
        /// </summary>
        /// <param name="sr">StreamReader referecne</param>
        public SCRTSUpgrade(StreamReader sr) : base(sr)
        {
            sr.ReadLine();
            researched = Convert.ToBoolean(sr.ReadLine());
            researching = Convert.ToBoolean(sr.ReadLine());
            BuildBlocks = Convert.ToInt32(sr.ReadLine());
            BuiltBlocks = Convert.ToInt32(sr.ReadLine());
        }
        /// <summary>
        /// RTSUpgrade copy constructor
        /// </summary>
        /// <param name="obj">RTSUpgrade reference</param>
        public SCRTSUpgrade(SCRTSUpgrade obj) : base(obj)
        {
            researched = obj.Researched;
            researching = obj.Researching;
            buildBlocks = obj.BuildBlocks;
            builtBlocks = obj.BuiltBlocks;
        }
        /// <summary>
        /// Save data override
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======SCRTSUpgrade Data======");
            sw.WriteLine(researched);
            sw.WriteLine(researching);
            sw.WriteLine(buildBlocks);
            sw.WriteLine(builtBlocks);
        }
        /// <summary>
        /// Updates RTSUpgrade internal state
        /// </summary>
        public override void update()
        {
            if(researching == true)
            {
                if(builtBlocks < buildBlocks)
                {
                    builtBlocks++;
                }
                else if(builtBlocks >= buildBlocks)
                {
                    researched = true;
                    Researching = false;
                }
            }
        }
        /// <summary>
        /// Researched property
        /// </summary>
        public bool Researched
        {
            get { return researched; }
            set { researched = value; }
        }
        /// <summary>
        /// Researching property
        /// </summary>
        public bool Researching
        {
            get { return researching; }
            set { researching = value; }
        }
        /// <summary>
        /// BuildBlocks property
        /// </summary>
        public int BuildBlocks
        {
            get { return buildBlocks; }
            set { buildBlocks = value; }
        }
        /// <summary>
        /// BuiltBlocks property
        /// </summary>
        public int BuiltBlocks
        {
            get { return builtBlocks; }
            set { builtBlocks = value; }
        }
    }
    public class SCRTSParticle : SCRTSObject
    {
        //protected
        protected int lifeSpan;
        protected int lifeSpent;
        protected Counter counter;
        protected string particleName;
        protected bool isAir;

        //public
        /// <summary>
        /// SCRTSParticle constructor
        /// </summary>
        /// <param name="name">Particle name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="sourceName">Source name value</param>
        /// <param name="hp">Hitpoint value</param>
        public SCRTSParticle(string name, float x, float y, int width,
            int height, int numFrames, string sourceName, int hp = 100, int lifeSpan = 100,
            bool isAir = false) :
            base(name, x, y, width, height, numFrames, sourceName, hp)
        {
            this.lifeSpan = lifeSpan;
            lifeSpent = 0;
            counter = new Counter(lifeSpan / numFrames);
            if (name == "")
            {
                particleName = "default particle";
            }
            else
            {
                particleName = name;
            }
            this.isAir = isAir;
        }
        /// <summary>
        /// SCRTSParticle from file constructor 
        /// </summary>
        /// <param name="sr">StreamReader referecne</param>
        public SCRTSParticle(StreamReader sr) : base(sr)
        {
            sr.ReadLine();
            lifeSpan = Convert.ToInt32(sr.ReadLine());
            lifeSpent = Convert.ToInt32(sr.ReadLine());
            counter = new Counter(Convert.ToInt32(sr.ReadLine()));
            particleName = sr.ReadLine();
            if(sr.ReadLine() == "TRUE")
            {
                isAir = true;
            }
            else
            {
                isAir = false;
            }
        }
        /// <summary>
        /// SCRTSParticle copy constructor
        /// </summary>
        /// <param name="obj">SCRTSParticle reference</param>
        public SCRTSParticle(SCRTSParticle obj) : base(obj)
        {
            lifeSpan = obj.LifeSpan;
            lifeSpent = obj.LifeSpent;
            counter = new Counter(obj.Ticks.Max);
            particleName = obj.ParticleName;
            isAir = obj.IsAir;
        }
        /// <summary>
        /// Save data override
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======SCRTSParticle Data======");
            sw.WriteLine(lifeSpan);
            sw.WriteLine(LifeSpent);
            sw.WriteLine(counter.Max);
            sw.WriteLine(particleName);
            if(isAir == true)
            {
                sw.WriteLine("TRUE");
            }
            else
            {
                sw.WriteLine("FALSE");
            }
        }
        /// <summary>
        /// Updates SCRTSParticle internal state
        /// </summary>
        public override void update()
        {
            lifeSpent++;
        }
        /// <summary>
        /// Draw override
        /// </summary>
        /// <param name="renderer">Renderer reference</param>
        /// <param name="winx">Window X offset</param>
        /// <param name="winy">Window Y offset</param>
        public override void draw(IntPtr renderer, int winx = 0, int winy = 0)
        {
            srcRect.x = frame * srcRect.w;
            destRect.x = hitBox.IX - winx;
            destRect.y = hitBox.IY - winy;
            if (counter.tick() == true)
            {
                if (frame < numFrames)
                {
                    frame++;
                }
            }
            SimpleDraw.draw(renderer, source, srcRect, destRect);
        }
        /// <summary>
        /// Done property
        /// </summary>
        public bool Done
        {
            get
            {
                if (lifeSpent >= lifeSpan)
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// LifeSpent property
        /// </summary>
        public int LifeSpent
        {
            get { return lifeSpent; }
        }
        /// <summary>
        /// LifeSpent property
        /// </summary>
        public int LifeSpan
        {
            get { return lifeSpan; }
            set
            {
                lifeSpan = value;
                counter = new Counter(value / numFrames);
            }
        }
        /// <summary>
        /// Counter property
        /// </summary>
        public Counter Ticks
        {
            get { return counter; }
        }
        /// <summary>
        /// ParticleName property
        /// </summary>
        public string ParticleName
        {
            get { return particleName; }
            set { particleName = value; }
        }
        /// <summary>
        /// IsAir property
        /// </summary>
        public bool IsAir
        {
            get { return isAir; }
            set { isAir = value; }
        }
    }
    public class SCRTSDoodad : SCRTSObject
    {
        //protected
        protected string doodadName;

        //public
        /// <summary>
        /// SCRTSDoodad constructor
        /// </summary>
        /// <param name="name">Doodad name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="sourceName">Source name value</param>
        /// <param name="hp">Hitpoint value</param>
        public SCRTSDoodad(string name, float x, float y, string sourceName, int width = 32,
            int height = 32, int numFrames = 1, int hp = 100, int lifeSpan = 100) :
            base(name, x, y, width, height, numFrames, sourceName, hp)
        {
            if (name == "")
            {
                doodadName = "default doodad";
            }
            else
            {
                doodadName = name;
            }
        }
        /// <summary>
        /// SCRTSDoodad from file constructor 
        /// </summary>
        /// <param name="sr">StreamReader referecne</param>
        public SCRTSDoodad(StreamReader sr) : base(sr)
        {
            sr.ReadLine();
            string buffer;
            buffer = sr.ReadLine();
            if(buffer == null)
            {
                buffer = name;
            }
            doodadName = buffer;
            buffer = sr.ReadLine();
            if(buffer == null)
            {
                buffer = name;
            }
            sourceName = buffer;
            source = TextureBank.getTexture(sourceName);
        }
        /// <summary>
        /// SCRTSDoodad copy constructor
        /// </summary>
        /// <param name="obj">SCRTSDoodad reference</param>
        public SCRTSDoodad(SCRTSDoodad obj) : base(obj)
        {
            doodadName = obj.DoodadName;
        }
        /// <summary>
        /// Save data override
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======SCRTSDoodad Data======");
            sw.WriteLine(doodadName);
            sw.WriteLine(sourceName);
        }
        /// <summary>
        /// DoodadName property
        /// </summary>
        public string DoodadName
        {
            get { return doodadName; }
            set { doodadName = value; }
        }
    }
    //SCRTSBuff deprecated (use SCRTSStatus)
    public class SCRTSBuff : SCRTSObject
    {
        //protected
        protected int lifeSpan;
        protected int lifeSpent;
        protected Counter counter;

        //public 
        /// <summary>
        /// SCRTSBuff constructor
        /// </summary>
        /// <param name="name">Buff name</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="sourceName">Source name value</param>
        /// <param name="hp">Hitpoint value</param>
        public SCRTSBuff(string name, float x, float y, int width,
            int height, int numFrames, string sourceName, int hp = 100, int lifeSpan = 100) :
            base(name, x, y, width, height, numFrames, sourceName, hp)
        {
            this.lifeSpan = lifeSpan;
            lifeSpent = 0;
            counter = new Counter(10);
        }
        /// <summary>
        /// SCRTSParticle from file constructor 
        /// </summary>
        /// <param name="sr">StreamReader referecne</param>
        public SCRTSBuff(StreamReader sr) : base(sr)
        {
            sr.ReadLine();
            lifeSpan = Convert.ToInt32(sr.ReadLine());
            lifeSpent = Convert.ToInt32(sr.ReadLine());
            counter = new Counter(Convert.ToInt32(sr.ReadLine()));
        }
        /// <summary>
        /// SCRTSParticle copy constructor
        /// </summary>
        /// <param name="obj">SCRTSParticle reference</param>
        public SCRTSBuff(SCRTSBuff obj) : base(obj)
        {
            lifeSpan = obj.LifeSpan;
            lifeSpent = obj.LifeSpent;
            counter = new Counter(obj.Ticks.Max);
        }
        /// <summary>
        /// Save data override
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======SCRTSBuff Data======");
            sw.WriteLine(lifeSpan);
            sw.WriteLine(LifeSpent);
            sw.WriteLine(counter.Max);
        }
        /// <summary>
        /// Updates SCRTSBuff internal state
        /// </summary>
        public override void update()
        {
            lifeSpent++;
        }
        /// <summary>
        /// Done property
        /// </summary>
        public bool Done
        {
            get
            {
                if (lifeSpent >= lifeSpan)
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// LifeSpent property
        /// </summary>
        public int LifeSpent
        {
            get { return lifeSpent; }
        }
        /// <summary>
        /// LifeSpent property
        /// </summary>
        public int LifeSpan
        {
            get { return lifeSpan; }
        }
        /// <summary>
        /// Counter property
        /// </summary>
        public Counter Ticks
        {
            get { return counter; }
        }
    }

    //Objectives classes

    public class SCAttackTarget : Objective
    {
        //protected
        protected SCRTSUnit target;
        protected SCRTSUnit host;
        protected STAGES stages;
        protected int maxRange;
        //public
        /// <summary>
        /// AttackTarget constructor
        /// </summary>
        /// <param name="target">SCRTSUnit reference</param>
        /// <param name="host">SCRTSUnit reference</param>
        public SCAttackTarget(SCRTSUnit target, SCRTSUnit host) : base()
        {
            this.target = target;
            this.host = host;
            done = false;
            stages = STAGES.ZERO;
            maxRange = host.getMaxRange();
        }
        /// <summary>
        /// Update SCAttackTarget internal state
        /// </summary>
        /// <param name="world">World reference</param>
        public void update(RTSWorld world)
        {
            double dist = 0;
            if(target != null)
            {
                if(target.HP <= 0)
                {
                    target = null;
                }
            }
            switch(stages)
            {
                case STAGES.ZERO:
                    if(host is SCRTSBuilding == false)
                    {
                        host.Path = world.SPF.findPath(host.Center, target.Center);
                        stages = STAGES.ONE;
                    }
                    else
                    {
                        stages = STAGES.ONE;
                    }
                    break;
                case STAGES.ONE:
                    if(target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    if (host is SCRTSBuilding == false)
                    {
                        host.fallow(world);
                    }
                    dist = Point2D.AsqrtB(host.Center, target.Center);
                    if (dist <= (maxRange * maxRange))
                    {
                        stages = STAGES.TWO;
                    }
                    break;
                case STAGES.TWO:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    dist = Point2D.AsqrtB(host.Center, target.Center);
                    if (dist > (maxRange * maxRange))
                    {
                        stages = STAGES.ZERO;
                        break;
                    }
                    host.attemptAttackTarget();
                    break;
                case STAGES.TWELVE:
                    done = true;
                    break;
            }
        }
        /// <summary>
        /// Target property
        /// </summary>
        public SCRTSUnit Target
        {
            get { return target; }
        }
    }
    public class SCMoveTarget : Objective
    {
        //protected
        protected Point2D target;
        protected SCRTSUnit host;
        protected STAGES stages;
        protected int maxRange;
        //public
        /// <summary>
        /// SCMoveTarget Constructor
        /// </summary>
        /// <param name="target">Point2D reference</param>
        /// <param name="host">SCRTSUnit reference</param>
        public SCMoveTarget(Point2D target, SCRTSUnit host) : base()
        {
            this.target = target;
            this.host = host;
            done = false;
            stages = STAGES.ZERO;
            maxRange = host.getMaxRange();
        }
        /// <summary>
        /// Updates SCMoveTarget internal state
        /// </summary>
        /// <param name="world"></param>
        public void update(RTSWorld world)
        {
            switch (stages)
            {
                case STAGES.ZERO:
                    if (host is SCRTSBuilding == false)
                    {
                        host.Path = world.SPF.findPath(host.Center, target);
                        stages = STAGES.ONE;
                    }
                    else
                    {
                        stages = STAGES.TWELVE;
                    }
                    break;
                case STAGES.ONE:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    else if(host is SCRTSBuilding == true)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    else
                    {
                        host.fallow(world);
                        if (host.RouteIndex >= host.Path.Count - 1)
                        {
                            stages = STAGES.TWELVE;
                            break;
                        }
                    }
                    break;
                case STAGES.TWELVE:
                    done = true;
                    break;
            }
        }
    }
    public class SCPatrolTarget : Objective
    {
        //protected
        protected Point2D target;
        protected Point2D target2;
        protected SCRTSUnit host;
        protected STAGES stages;
        protected int maxRange;
        //public
        /// <summary>
        /// PatrolTarget Constructor
        /// </summary>
        /// <param name="target">Point2D reference</param>
        /// <param name="host">SCRTSUnit reference</param>
        public SCPatrolTarget(Point2D target, SCRTSUnit host) : base()
        {
            this.target = target;
            target2 = host.Center;
            this.host = host;
            done = false;
            stages = STAGES.ZERO;
            maxRange = host.getMaxRange();
        }
        /// <summary>
        /// Updates SCPatrolTarget internal state
        /// </summary>
        /// <param name="world">RTSWorld reference</param>
        public void update(RTSWorld world)
        {
            switch (stages)
            {
                case STAGES.ZERO:
                    if (host is SCRTSBuilding == false)
                    {
                        host.Path = world.SPF.findPath(host.Center, target);
                        stages = STAGES.ONE;
                    }
                    else
                    {
                        stages = STAGES.TWELVE;
                    }
                    break;
                case STAGES.ONE:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    else if (host is SCRTSBuilding == true)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    else
                    {
                        host.fallow(world);
                        if (host.RouteIndex >= host.Path.Count - 1)
                        {
                            Point2D tmp = target;
                            target = target2;
                            target2 = tmp;
                            stages = STAGES.ZERO;
                            break;
                        }
                    }
                    break;
                case STAGES.TWELVE:
                    done = true;
                    break;
            }
        }
    }
    public class SCBuffTarget : Objective
    {
        //protected
        protected SCRTSUnit target;
        protected SCRTSUnit host;
        protected STAGES stages;
        protected int maxRange;
        //public
        /// <summary>
        /// SCBuffTarget Constructor
        /// </summary>
        /// <param name="target">SCRTSUnit reference</param>
        /// <param name="host">SCRTSUnit reference</param>
        public SCBuffTarget(SCRTSUnit target, SCRTSUnit host) : base()
        {
            this.target = target;
            this.host = host;
            done = false;
            stages = STAGES.ZERO;
            maxRange = host.getMaxRange();
        }
        /// <summary>
        /// Updates SCBuffTarget internal state
        /// </summary>
        /// <param name="dworl">World reference</param>
        public void update(RTSWorld world)
        {
            double dist = 0;
            if (target != null)
            {
                if (target.HP <= 0)
                {
                    target = null;
                }
            }
            switch (stages)
            {
                case STAGES.ZERO:
                    if (host is SCRTSBuilding == false)
                    {
                        host.Path = world.SPF.findPath(host.Center, target.Center);
                        stages = STAGES.ONE;
                    }
                    else
                    {
                        stages = STAGES.ONE;
                    }
                    break;
                case STAGES.ONE:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    if (host is SCRTSBuilding == false)
                    {
                        host.fallow(world);
                    }
                    dist = Point2D.AsqrtB(host.Center, target.Center);
                    if (dist <= (maxRange * maxRange))
                    {
                        stages = STAGES.TWO;
                    }
                    break;
                case STAGES.TWO:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    dist = Point2D.AsqrtB(host.Center, target.Center);
                    if (dist > (maxRange * maxRange))
                    {
                        stages = STAGES.ZERO;
                        break;
                    }
                    string abil = host.getAbilityByActionType(ACTIONS.BUFF);
                    host.callAbility(abil);
                    break;
                case STAGES.TWELVE:
                    done = true;
                    break;
            }
        }
    }
    public class SCDebuffTarget : Objective
    {
        //protected
        protected SCRTSUnit target;
        protected SCRTSUnit host;
        protected STAGES stages;
        protected int maxRange;
        //public
        /// <summary>
        /// SCDebuffTarget Constructor
        /// </summary>
        /// <param name="target">SCRTSUnit reference</param>
        /// <param name="host">SCRTSUnit reference</param>
        public SCDebuffTarget(SCRTSUnit target, SCRTSUnit host) : base()
        {
            this.target = target;
            this.host = host;
            done = false;
            stages = STAGES.ZERO;
            maxRange = host.getMaxRange();
        }
        /// <summary>
        /// Updates SCDebuffTarget internal state
        /// </summary>
        /// <param name="world">RTSWorld reference</param>
        public void update(RTSWorld world)
        {
            double dist = 0;
            if (target != null)
            {
                if (target.HP <= 0)
                {
                    target = null;
                }
            }
            switch (stages)
            {
                case STAGES.ZERO:
                    if (host is SCRTSBuilding == false)
                    {
                        host.Path = world.SPF.findPath(host.Center, target.Center);
                        stages = STAGES.ONE;
                    }
                    else
                    {
                        stages = STAGES.ONE;
                    }
                    break;
                case STAGES.ONE:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    if (host is SCRTSBuilding == false)
                    {
                        host.fallow(world);
                    }
                    dist = Point2D.AsqrtB(host.Center, target.Center);
                    if (dist <= (maxRange * maxRange))
                    {
                        stages = STAGES.TWO;
                    }
                    break;
                case STAGES.TWO:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    dist = Point2D.AsqrtB(host.Center, target.Center);
                    if (dist > (maxRange * maxRange))
                    {
                        stages = STAGES.ZERO;
                        break;
                    }
                    string abil = host.getAbilityByActionType(ACTIONS.DEBUFF);
                    host.callAbility(abil);
                    break;
                case STAGES.TWELVE:
                    done = true;
                    break;
            }
        }
    }
    public class SCRepairTarget : Objective
    {
        //protected
        protected SCRTSUnit target;
        protected SCRTSUnit host;
        protected STAGES stages;
        protected int maxRange;
        //public
        /// <summary>
        /// SCRepairTarget Constructor
        /// </summary>
        /// <param name="target">SCRTSUnit reference</param>
        /// <param name="host">SCRTSUnit reference</param>
        public SCRepairTarget(SCRTSUnit target, SCRTSUnit host) : base()
        {
            this.target = target;
            this.host = host;
            done = false;
            stages = STAGES.ZERO;
            maxRange = host.getMaxRange();
        }
        /// <summary>
        /// Updates SCRepairTarget internal state
        /// </summary>
        /// <param name="world">RTSWorld reference</param>
        public void update(RTSWorld world)
        {
            double dist = 0;
            if (target != null)
            {
                if (target.HP <= 0)
                {
                    target = null;
                }
            }
            switch (stages)
            {
                case STAGES.ZERO:
                    if (host is SCRTSBuilding == false)
                    {
                        host.Path = world.SPF.findPath(host.Center, target.Center);
                        stages = STAGES.ONE;
                    }
                    else
                    {
                        stages = STAGES.ONE;
                    }
                    break;
                case STAGES.ONE:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    if (host is SCRTSBuilding == false)
                    {
                        host.fallow(world);
                    }
                    dist = Point2D.AsqrtB(host.Center, target.Center);
                    if (dist <= (maxRange * maxRange))
                    {
                        stages = STAGES.TWO;
                    }
                    break;
                case STAGES.TWO:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    dist = Point2D.AsqrtB(host.Center, target.Center);
                    if (dist > (maxRange * maxRange))
                    {
                        stages = STAGES.ZERO;
                        break;
                    }
                    string abil = host.getAbilityByActionType(ACTIONS.REPAIR);
                    host.callAbility(abil);
                    break;
                case STAGES.TWELVE:
                    done = true;
                    break;
            }
        }
    }
    public class SCBuildTarget : Objective
    {
        //protected
        protected SCRTSBuilding target;
        protected SCRTSUnit host;
        protected STAGES stages;
        protected int maxRange;
        //public
        /// <summary>
        /// SCBuildTarget Constructor
        /// </summary>
        /// <param name="target">SCRTSBuilding reference</param>
        /// <param name="host">SCRTSUnit reference</param>
        public SCBuildTarget(SCRTSBuilding target, SCRTSUnit host) : base()
        {
            this.target = target;
            this.host = host;
            done = false;
            stages = STAGES.ZERO;
            maxRange = host.getMaxRange();
        }
        /// <summary>
        /// Updates BuildTarget internal state
        /// </summary>
        /// <param name="world">RTSWorld reference</param>
        public void update(RTSWorld world)
        {
            double dist = 0;
            if (target != null)
            {
                if (target.HP <= 0)
                {
                    target = null;
                }
            }
            switch (stages)
            {
                case STAGES.ZERO:
                    if (host is SCRTSBuilding == false)
                    {
                        host.Path = world.SPF.findPath(host.Center, target.Center);
                        stages = STAGES.ONE;
                    }
                    else
                    {
                        stages = STAGES.ONE;
                    }
                    break;
                case STAGES.ONE:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    if (host is SCRTSBuilding == false)
                    {
                        host.fallow(world);
                    }
                    dist = Point2D.AsqrtB(host.Center, target.Center);
                    if (dist <= (maxRange * maxRange))
                    {
                        stages = STAGES.TWO;
                    }
                    break;
                case STAGES.TWO:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    dist = Point2D.AsqrtB(host.Center, target.Center);
                    if (dist > (maxRange * maxRange))
                    {
                        stages = STAGES.ZERO;
                        break;
                    }
                    string abil = host.getAbilityByActionType(ACTIONS.BUILD);
                    host.callAbility(abil);
                    break;
                case STAGES.TWELVE:
                    done = true;
                    break;
            }
        }
    }
    public class SCHealTarget : Objective
    {
        //protected
        protected SCRTSUnit target;
        protected SCRTSUnit host;
        protected STAGES stages;
        protected int maxRange;
        //public
        /// <summary>
        /// SCHealTarget Constructor
        /// </summary>
        /// <param name="target">SCRTSUnit reference</param>
        /// <param name="host">SCRTSUnit reference</param>
        public SCHealTarget(SCRTSUnit target, SCRTSUnit host) : base()
        {
            this.target = target;
            this.host = host;
            done = false;
            stages = STAGES.ZERO;
            maxRange = host.getMaxRange();
        }
        /// <summary>
        /// Updates HealTarget internal state
        /// </summary>
        /// <param name="world">RTSWorld reference</param>
        public void update(RTSWorld world)
        {
            double dist = 0;
            if (target != null)
            {
                if (target.HP <= 0)
                {
                    target = null;
                }
            }
            switch (stages)
            {
                case STAGES.ZERO:
                    if (host is SCRTSBuilding == false)
                    {
                        host.Path = world.SPF.findPath(host.Center, target.Center);
                        stages = STAGES.ONE;
                    }
                    else
                    {
                        stages = STAGES.ONE;
                    }
                    break;
                case STAGES.ONE:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    if (host is SCRTSBuilding == false)
                    {
                        host.fallow(world);
                    }
                    dist = Point2D.AsqrtB(host.Center, target.Center);
                    if (dist <= (maxRange * maxRange))
                    {
                        stages = STAGES.TWO;
                    }
                    break;
                case STAGES.TWO:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    dist = Point2D.AsqrtB(host.Center, target.Center);
                    if (dist > (maxRange * maxRange))
                    {
                        stages = STAGES.ZERO;
                        break;
                    }
                    string abil = host.getAbilityByActionType(ACTIONS.HEAL);
                    host.callAbility(abil);
                    break;
                case STAGES.TWELVE:
                    done = true;
                    break;
            }
        }
    }
    public class SCHarvestTarget : Objective
    {
        //protected
        protected Point2D target;
        protected SCRTSUnit host;
        protected STAGES stages;
        protected int maxRange;
        //public
        /// <summary>
        /// SCHarvestTarget Constructor
        /// </summary>
        /// <param name="target">Point2D reference</param>
        /// <param name="host">SCRTSUnit reference</param>
        public SCHarvestTarget(Point2D target, SCRTSUnit host) : base()
        {
            this.target = target;
            this.host = host;
            done = false;
            stages = STAGES.ZERO;
            maxRange = host.getMaxRange();
        }
        /// <summary>
        /// Updates HarvestTarget internal state
        /// </summary>
        /// <param name="world">RTSWorld reference</param>
        public void update(RTSWorld world)
        {
            double dist = 0;
            switch (stages)
            {
                case STAGES.ZERO:
                    if (host is SCRTSBuilding == false)
                    {
                        host.Path = world.SPF.findPath(host.Center, target);
                        stages = STAGES.ONE;
                    }
                    else
                    {
                        stages = STAGES.ONE;
                    }
                    break;
                case STAGES.ONE:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    if (host is SCRTSBuilding == false)
                    {
                        host.fallow(world);
                    }
                    dist = Point2D.AsqrtB(host.Center, target);
                    if (dist <= (maxRange * maxRange))
                    {
                        stages = STAGES.TWO;
                    }
                    break;
                case STAGES.TWO:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    if(world.resourceAt(target.IX / 
                        world.TileWidth, target.IY / 
                        world.TileHeight) == null)
                    {
                        target = world.nearestResource(target);
                        if(target == null)
                        {
                            stages = STAGES.TWELVE;
                            break;
                        }
                    }
                    dist = Point2D.AsqrtB(host.Center, target);
                    if (dist > (maxRange * maxRange))
                    {
                        stages = STAGES.ZERO;
                        break;
                    }
                    string abil = host.getAbilityByActionType(ACTIONS.HARVEST);
                    host.callAbility(abil);
                    if(host.Tank >= host.TankMax)
                    {
                        host.Target = new SCRefineTarget(host.Commander.nearestRefinory(host.Center), host);
                        stages = STAGES.TWELVE;
                    }
                    break;
                case STAGES.TWELVE:
                    done = true;
                    break;
            }
        }
    }
    public class SCRefineTarget : Objective
    {
        //protected
        protected SCRTSBuilding target;
        protected SCRTSUnit host;
        protected STAGES stages;
        protected int maxRange;
        //public
        /// <summary>
        /// SCRefineTarget Constructor
        /// </summary>
        /// <param name="target">RTSBuilding reference</param>
        /// <param name="host">RTSUnit reference</param>
        public SCRefineTarget(SCRTSBuilding target, SCRTSUnit host) : base()
        {
            this.target = target;
            this.host = host;
            done = false;
            stages = STAGES.ZERO;
            maxRange = host.getMaxRange();
        }
        /// <summary>
        /// Updates SCRefineTarget internal state
        /// </summary>
        /// <param name="world">RTSWorld reference</param>
        public void update(RTSWorld world)
        {
            double dist = 0;
            switch (stages)
            {
                case STAGES.ZERO:
                    if (host is SCRTSBuilding == false)
                    {
                        host.Path = world.SPF.findPath(host.Center, target.MidBottom);
                        stages = STAGES.ONE;
                    }
                    else
                    {
                        stages = STAGES.ONE;
                    }
                    break;
                case STAGES.ONE:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    if (host is SCRTSBuilding == false)
                    {
                        host.fallow(world);
                    }
                    dist = Point2D.AsqrtB(host.Center, target.MidBottom);
                    if (dist <= (maxRange * maxRange))
                    {
                        stages = STAGES.TWO;
                    }
                    break;
                case STAGES.TWO:
                    if (target == null)
                    {
                        stages = STAGES.TWELVE;
                        break;
                    }
                    dist = Point2D.AsqrtB(host.Center, target.MidBottom);
                    if (dist > (maxRange * maxRange))
                    {
                        stages = STAGES.ZERO;
                        break;
                    }
                    host.Commander.unloadResource(host);
                    host.Target = new SCHarvestTarget(world.nearestResource(host.Center), host);
                    stages = STAGES.TWELVE;
                    break;
                case STAGES.TWELVE:
                    done = true;
                    break;
            }
        }
    }

}
