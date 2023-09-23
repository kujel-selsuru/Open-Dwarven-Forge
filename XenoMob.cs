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
    /// AttackTarget objective class
    /// </summary>
    public class AttackTarget : XenoObjective
    {
        //protected

        //public
        /// <summary>
        /// AttackTarget constructor
        /// </summary>
        /// <param name="target">Target to attack</param>
        /// <param name="body">Object reference to object using this XenoObjective</param>
        /// <param name="rand">Random reference</param>
        /// <param name="ticks">Delay value</param>
        public AttackTarget(XenoSprite target, XenoMob body, Random rand = null, int ticks = 10) : base(rand, ticks)
        {
            this.target = target;
            this.body = body;
        }
        /// <summary>
        /// Updates AttackTarget's internal state
        /// </summary>
        /// <param name="actions">List of XenoSprite objects</param>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="spf">SimplePathFinder reference</param>
        /// <param name="r">Minimum range to target in tiles</param>
        /// <param name="mgLeft">Left side of MapGraph</param>
        /// <param name="mgTop">Top side of MapGraph</param>
        /// <param name="cell">OpenWorldCell reference</param>
        /// <param name="scaler">Scaler value</param>
        /// <param name="rangeMode">Hypontinuse of tiles</param>
        public override void update(List<XenoSprite> actions, MapGraph mg, SimplePathFinder spf, int r, 
            int mgLeft, int mgTop, OpenWorldCell cell, int scaler = 32, int rangeMod = 45)
        {
            switch(phaze)
            {
                case PHAZE.one:
                    if(target == null)
                    {
                        phaze = PHAZE.four;
                    }
                    body.Route = spf.findPathV4(body.Center, target.Center, cell.CellLeftSide, cell.CellTopSide);
                    phaze = PHAZE.two;
                    break;
                case PHAZE.two:
                    if (target == null)
                    {
                        phaze = PHAZE.four;
                    }
                    body.fallowPath2(mg, 2, cell.CellLeftSide, cell.CellTopSide, scaler);
                    if (Point2D.AsqrtB(body.Center, target.Center) <= r * (rangeMod * rangeMod))
                    {
                        phaze = PHAZE.three;
                    }
                    break;
                case PHAZE.three:
                    if (target == null)
                    {
                        phaze = PHAZE.four;
                    }
                    if (wait.tick())
                    {
                        ((XenoMob)body).strike(actions);
                        if (Point2D.AsqrtB(body.Center, target.Center) > (r * 2) * (rangeMod * rangeMod))
                        {
                            phaze = PHAZE.four;
                        }
                    }
                    break;
                case PHAZE.four:
                    done = true;
                    break;
            }
        }
        /// <summary>
        /// Updates AttackTarget's internal state
        /// </summary>
        /// <param name="actions">List of XenoSprite objects</param>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="spf">SimplePathFinder reference</param>
        /// <param name="range">Minimum range to target in tiles</param>
        /// <param name="mgLeft">Left side of MapGraph</param>
        /// <param name="mgTop">Top side of MapGraph</param>
        /// <param name="cell">OpenWorldCell reference</param>
        /// <param name="quad">Mob quadrent value</param>
        /// <param name="scaler">Scaler value</param>
        /// <param name="rangeMode">Hypontinuse of tiles</param>
        public override void update(List<XenoSprite> actions, MapGraph mg, SimplePathFinder spf, int range,
            int mgLeft, int mgTop, OpenWorldCell cell, int quad, int scaler = 32, int rangeMod = 45)
        {
            int xmod = 0;
            int ymod = 0;
            switch (quad)
            {
                case 1://top left
                    xmod = 3200;
                    ymod = 3200;
                    break;
                case 2://top right
                    xmod = -3200;
                    ymod = 3200;
                    break;
                case 3://bottom right
                    xmod = -3200;
                    ymod = -3200;
                    break;
                case 4://bottom left
                    xmod = 3200;
                    ymod = -3200;
                    break;
            }
            switch (phaze)
            {
                case PHAZE.one:
                    if (target == null)
                    {
                        phaze = PHAZE.four;
                    }
                    body.Route = spf.findPathV4(body.Center, target.Center, cell.CellLeftSide - xmod, cell.CellTopSide - ymod);
                    phaze = PHAZE.two;
                    break;
                case PHAZE.two:
                    if (target == null)
                    {
                        phaze = PHAZE.four;
                    }
                    body.fallowPath2(mg, 2, cell.CellLeftSide - xmod, cell.CellTopSide - ymod);
                    if (Point2D.AsqrtB(body.Center, target.Center) <= range * (rangeMod * rangeMod))
                    {
                        phaze = PHAZE.three;
                    }
                    break;
                case PHAZE.three:
                    if (target == null)
                    {
                        phaze = PHAZE.four;
                    }
                    if (wait.tick())
                    {
                        ((XenoMob)body).strike(actions);
                        if (Point2D.AsqrtB(body.Center, target.Center) > (range * 2) * (rangeMod * rangeMod))
                        {
                            phaze = PHAZE.four;
                        }
                    }
                    break;
                case PHAZE.four:
                    done = true;
                    break;
            }
        }
        /// <summary>
        /// Updates AttackTarget's internal state
        /// </summary>
        /// <param name="actions">List of XenoSprite objects</param>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="spf">SimplePathFinder reference</param>
        /// <param name="range">Minimum range to target in tiles</param>
        /// <param name="mgLeft">Left side of MapGraph</param>
        /// <param name="mgTop">Top side of MapGraph</param>
        /// <param name="scaler">Scaler value</param>
        /// <param name="rangeMode">Hypontinuse of tiles</param>
        public override void update(List<XenoSprite> actions, MapGraph mg, SimplePathFinder spf, int range,
            int mgLeft, int mgTop, int scaler = 32, int rangeMod = 45)
        {
            switch (phaze)
            {
                case PHAZE.one:
                    if (target == null)
                    {
                        phaze = PHAZE.four;
                    }
                    body.Route = spf.findPath(body.Center, target.Center);
                    phaze = PHAZE.two;
                    break;
                case PHAZE.two:
                    if (target == null)
                    {
                        phaze = PHAZE.four;
                    }
                    body.fallowPath2(mg, 2);
                    if (Point2D.AsqrtB(body.Center, target.Center) <= range * (rangeMod * rangeMod))
                    {
                        phaze = PHAZE.three;
                    }
                    break;
                case PHAZE.three:
                    if (target == null)
                    {
                        phaze = PHAZE.four;
                    }
                    if (wait.tick())
                    {
                        ((XenoMob)body).strike(actions);
                        if (Point2D.AsqrtB(body.Center, target.Center) > (range * 2) * (rangeMod * rangeMod))
                        {
                            phaze = PHAZE.four;
                        }
                    }
                    break;
                case PHAZE.four:
                    done = true;
                    break;
            }
        }
    }

    /// <summary>
    /// MoveToTarget objective class
    /// </summary>
    public class MoveToTarget : XenoObjective
    {
        //protected

        //public
        /// <summary>
        /// MoveToTarget constructor
        /// </summary>
        /// <param name="body">Object reference to object using this XenoObjective</param>
        /// <param name="rand">Random reference</param>
        /// <param name="ticks">Delay value</param>
        public MoveToTarget(XenoMob body, Random rand = null, int ticks = 10) : base(rand, ticks)
        {
            this.body = body;
            if(rand != null)
            {
                this.rand = rand;
            }
        }
        /// <summary>
        /// Updates MoveToTarget's internal state
        /// </summary>
        /// <param name="actions">List of XenoSprite objects</param>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="spf">SimplePathFinder reference</param>
        /// <param name="range">Minimum range to target position in tiles</param>
        /// <param name="mgLeft">Left side of MapGraph</param>
        /// <param name="mgTop">Top side of MapGraph</param>
        /// <param name="cell">OpenWorldCell reference</param>
        /// <param name="quad">Mob quadrent value</param>
        /// <param name="scaler">Scaler value</param>
        /// <param name="rangeMode">Hypontinuse of tiles</param>
        public override void update(List<XenoSprite> actions, MapGraph mg, SimplePathFinder spf, int range,
            int mgLeft, int mgTop, OpenWorldCell cell, int quad, int scaler = 32, int rangeMod = 45)
        {
            double dir = 0;
            Point2D dest = new Point2D();
            int xmod = 0;
            int ymod = 0;
            switch (quad)
            {
                case 1://top left
                    xmod = 3200;
                    ymod = 3200;
                    break;
                case 2://top right
                    xmod = -3200;
                    ymod = 3200;
                    break;
                case 3://bottom right
                    xmod = -3200;
                    ymod = -3200;
                    break;
                case 4://bottom left
                    xmod = 3200;
                    ymod = -3200;
                    break;
            }
            switch (phaze)
            {
                case PHAZE.one:
                    dir = 22.5f * (rand.Next(0, 1600) / 100);
                    dest.X = body.Center.X + (float)Math.Cos(Helpers<int>.degreesToRadians(dir)) * (rangeMod * 5);
                    dest.Y = body.Center.Y + (float)Math.Sin(Helpers<int>.degreesToRadians(dir)) * (rangeMod * 5);
                    
                    body.Route = spf.findPathV4(body.Center, dest, cell.CellLeftSide - xmod, cell.CellTopSide - ymod);
                    phaze = PHAZE.two;
                    break;
                case PHAZE.two:
                    body.fallowPath2(mg, 2, cell.CellLeftSide - xmod, cell.CellTopSide -ymod, scaler);
                    if (body.Route == null)
                    {
                        phaze = PHAZE.three;
                    }
                    break;
                case PHAZE.three:
                    done = true;
                    break;
            }
        }
        /// <summary>
        /// Updates MoveToTarget's internal state
        /// </summary>
        /// <param name="actions">List of XenoSprite objects</param>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="spf">SimplePathFinder reference</param>
        /// <param name="range">Minimum range to target position in tiles</param>
        /// <param name="mgLeft">Left side of MapGraph</param>
        /// <param name="mgTop">Top side of MapGraph</param>
        /// <param name="cell">OpenWorldCell reference</param>
        /// <param name="scaler">Scaler value</param>
        /// <param name="rangeMode">Hypontinuse of tiles</param>
        public override void update(List<XenoSprite> actions, MapGraph mg, SimplePathFinder spf, int range,
            int mgLeft, int mgTop, OpenWorldCell cell, int scaler = 32, int rangeMod = 45)
        {
            double dir = 0;
            Point2D dest = new Point2D();
            switch (phaze)
            {
                case PHAZE.one:
                    dir = 22.5f * (rand.Next(0, 1600) / 100);
                    dest.X = body.Center.X + (float)Math.Cos(Helpers<int>.degreesToRadians(dir)) * (rangeMod * 5);
                    dest.Y = body.Center.Y + (float)Math.Sin(Helpers<int>.degreesToRadians(dir)) * (rangeMod * 5);

                    body.Route = spf.findPathV4(body.Center, dest, cell.CellLeftSide, cell.CellTopSide);
                    phaze = PHAZE.two;
                    break;
                case PHAZE.two:
                    body.fallowPath2(mg, 2, cell.CellLeftSide, cell.CellTopSide, scaler);
                    if (body.Route == null)
                    {
                        phaze = PHAZE.three;
                    }
                    break;
                case PHAZE.three:
                    done = true;
                    break;
            }
        }
        /// <summary>
        /// Updates MoveToTarget's internal state
        /// </summary>
        /// <param name="actions">List of XenoSprite objects</param>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="spf">SimplePathFinder reference</param>
        /// <param name="range">Minimum range to target position in tiles</param>
        /// <param name="mgLeft">Left side of MapGraph</param>
        /// <param name="mgTop">Top side of MapGraph</param>
        /// <param name="scaler">Scaler value</param>
        /// <param name="rangeMode">Hypontinuse of tiles</param>
        public override void update(List<XenoSprite> actions, MapGraph mg, SimplePathFinder spf, int range, 
            int mgLeft, int  mgTop, int scaler = 32, int rangeMod = 45)
        {
            double dir = 0;
            Point2D dest = new Point2D();
            switch (phaze)
            {
                case PHAZE.one:
                    dir = 22.5f * (rand.Next(0, 1600) / 100);
                    dest.X = body.Center.X + (float)Math.Cos(Helpers<int>.degreesToRadians(dir)) * (rangeMod * 5);
                    dest.Y = body.Center.Y + (float)Math.Sin(Helpers<int>.degreesToRadians(dir)) * (rangeMod * 5);
                    body.Route = spf.findPath(body.Center, dest);
                    phaze = PHAZE.two;
                    break;
                case PHAZE.two:
                    body.fallowPath(mg, 2, mgLeft, mgTop, scaler);
                    if(body.Route == null)
                    {
                        phaze = PHAZE.three;
                    }
                    break;
                case PHAZE.three:
                    done = true;
                    break;
            }
        }
    }

    /// <summary>
    /// XenoMob class
    /// </summary>
    public class XenoMob : XenoSprite
    {
        //protected
        protected double angle;
        protected XenoObjective tar;
        protected string action;
        protected Random rand;
        protected int range;

        //public
        /// <summary>
        /// XenoMob constructor
        /// </summary>
        /// <param name="name">Name of mob and source texture</param>
        /// <param name="action">Name of action and action's source texture</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="numFrames">Number of frames</param>
        /// <param name="range">Minimum strike range in tiles</param>
        public XenoMob(string name, string action, float x, float y, int width, int height, int numFrames, int range = 12) : base(name, x, y, width, height, numFrames)
        {
            angle = 270;
            tar = null;
            this.action = action;
            rand = new Random(new Guid().GetHashCode());
            speed = 2;
            this.range = range;
        }
        /// <summary>
        /// XenoMob from file constructor
        /// </summary>
        /// <param name="sr">StreamReader refenence</param>
        public XenoMob(StreamReader sr) : base(sr)
        {
            angle = Convert.ToDouble(sr.ReadLine());
            tar = null;
            action = sr.ReadLine();
            rand = new Random(new Guid().GetHashCode());
            speed = (float)Convert.ToDecimal(sr.ReadLine());
            range = Convert.ToInt32(sr.ReadLine());
        }
        /// <summary>
        /// SaveData override
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public override void saveData(StreamWriter sw)
        {
            base.saveData(sw);
            sw.Write(angle);
            sw.WriteLine(action);
            sw.WriteLine(speed);
            sw.WriteLine(range);
        }
        /// <summary>
        /// Scans for the player and sets objective
        /// </summary>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="avatar">XenoSprite reference</param>
        /// <param name="rand">Random reference</param>
        /// <param name="sightRange">Range of lines in tiles</param>
        /// <param name="scalerx">Scaler x value</param>
        /// <param name="scalery">Scaler y value</param>
        public virtual void scanForAvatar(MapGraph mg, XenoSprite avatar, Random rand, int sightRange = 12, int scalerx = 32, int scalery = 32)
        {
            VisionRays.castLines(Center.X, Center.Y, angle, mg, 7, sightRange, scalerx, scalery);
            if(VisionRays.testLines(avatar.HitBox))
            {
                tar = new AttackTarget(avatar, this, rand, 60); 
            }
            if(tar == null)//+90 degrees
            {
                VisionRays.castLines(Center.X, Center.Y, angle + 90, mg, 7, sightRange, scalerx, scalery);
                if (VisionRays.testLines(avatar.HitBox))
                {
                    tar = new AttackTarget(avatar, this, rand, 60);
                }
            }
            if (tar == null)//+180 degrees
            {
                VisionRays.castLines(Center.X, Center.Y, angle + 180, mg, 7, sightRange, scalerx, scalery);
                if (VisionRays.testLines(avatar.HitBox))
                {
                    tar = new AttackTarget(avatar, this, rand, 60);
                }
            }
            if (tar == null)//+270 degrees
            {
                VisionRays.castLines(Center.X, Center.Y, angle + 270, mg, 7, sightRange, scalerx, scalery);
                if (VisionRays.testLines(avatar.HitBox))
                {
                    tar = new AttackTarget(avatar, this, rand, 60);
                }
            }
            if (tar == null)
            {
                tar = new MoveToTarget(this, rand, 10);
            }
        }
        /// <summary>
        /// XenoMob's AI routine
        /// </summary>
        /// <param name="avatar">XenoSprite reference</param>
        /// <param name="actions">List of XenoSprite objects</param>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="spf">SimplePathFinder reference</param>
        /// <param name="sightRange">Minimum range to target object</param>
        /// <param name="rand">Random reference</param>
        /// <param name="scaler">Scaler value</param>
        public virtual void think(XenoSprite avatar, List<XenoSprite> actions, MapGraph mg, SimplePathFinder spf, int sightRange, 
            int mgLeft, int mgTop, Random rand, int scaler = 32)
        {
            if(tar != null)
            {
                tar.update(actions, mg, spf, range, mgLeft, mgTop, scaler);
                if(tar.Done)
                {
                    tar = null;
                }
            }
            else
            {
                scanForAvatar(mg, avatar, rand, sightRange, scaler, scaler);
            }
        }
        /// <summary>
        /// XenoMob's AI routine
        /// </summary>
        /// <param name="avatar">XenoSprite reference</param>
        /// <param name="actions">List of XenoSprite objects</param>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="spf">SimplePathFinder reference</param>
        /// <param name="rand">Random reference</param>
        /// <param name="cell">OpenWorldCell reference</param>
        /// <param name="sightRange">Minimum range to target object</param>
        /// <param name="scaler">Scaler value</param>
        /// <param name="rangeMod">Tile hypontenous value</param>
        public virtual void think(XenoSprite avatar, List<XenoSprite> actions, MapGraph mg, SimplePathFinder spf,
            int mgLeft, int mgTop, Random rand, OpenWorldCell cell, int sightRange, int scaler = 32, int rangeMod = 45)
        {
            if (tar != null)
            {
                tar.update(actions, mg, spf, range, mgLeft, mgTop, cell, scaler, rangeMod);
                if (tar.Done)
                {
                    tar = null;
                }
            }
            else
            {
                scanForAvatar(mg, avatar, rand, sightRange, scaler, scaler);
            }
        }
        /// <summary>
        /// XenoMob's AI routine
        /// </summary>
        /// <param name="avatar">XenoSprite reference</param>
        /// <param name="actions">List of XenoSprite objects</param>
        /// <param name="mg">MapGraph reference</param>
        /// <param name="spf">SimplePathFinder reference</param>
        /// <param name="rand">Random reference</param>
        /// <param name="cell">OpenWorldCell reference</param>
        /// <param name="sightRange">Minimum range to target object</param>
        /// <param name="quad">Mob quadrent value</param>
        /// <param name="scaler">Scaler value</param>
        /// <param name="rangeMod">tile hypontenous value</param>
        public virtual void think(XenoSprite avatar, List<XenoSprite> actions, MapGraph mg, SimplePathFinder spf,
            int mgLeft, int mgTop, Random rand, OpenWorldCell cell, int sightRange, int quad, int scaler = 32, int rangeMod = 45)
        {
            if (tar != null)
            {
                tar.update(actions, mg, spf, range, mgLeft, mgTop, cell, quad, scaler, rangeMod);
                if (tar.Done)
                {
                    tar = null;
                }
            }
            else
            {
                scanForAvatar(mg, avatar, rand, sightRange, scaler, scaler);
            }
        }
        /// <summary>
        /// Sets the angle based on current facing direction
        /// </summary>
        public void setAngle()
        {
            switch(direct)
            {
                case DIRECT.UP:
                    angle = 270;
                    break;
                case DIRECT.UPRIGHT:
                    angle = 315;
                    break;
                case DIRECT.RIGHT:
                    angle = 0;
                    break;
                case DIRECT.DOWNRIGHT:
                    angle = 45;
                    break;
                case DIRECT.DOWN:
                    angle = 90;
                    break;
                case DIRECT.DOWNLEFT:
                    angle = 135;
                    break;
                case DIRECT.LEFT:
                    angle = 180;
                    break;
                case DIRECT.UPLEFT:
                    angle = 225;
                    break;
            }
        }
        /// <summary>
        /// Faces the target and creates an action
        /// </summary>
        /// <param name="actions"></param>
        /// <param name="scaler"></param>
        /// <param name="aw"></param>
        /// <param name="ah"></param>
        /// <param name="af"></param>
        public virtual void strike(List<XenoSprite> actions, int scaler = 32, int aw = 16, int ah = 16, int af = 4)
        {
            facePoint(tar.Target.Center.X, tar.Target.Center.Y, scaler);
            actions.Add(new XenoSprite(action, Center.X, Center.Y, aw, ah, af));
        }
    }
}
