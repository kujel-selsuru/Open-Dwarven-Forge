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
    public enum RTSABILITYTYPE { ab_cmd = 0, ab_action, ab_targetted, ab_upgrade, ab_self, ab_train, ab_blank };
    public class RTSAbility
    {
        //protected
        protected Texture2D source;
        protected Rectangle srcRect;
        protected Rectangle destRect;
        protected string action;
        protected RTSABILITYTYPE abt;
        protected RTSACTIONTYPE act;
        protected RTSObject owner;
        protected string name;
        protected Rectangle box;
        protected CoolDown delay;
        protected CoolDown coolDown;
        protected int timeSpan;
        protected int timeSpent;
        protected bool locked;
        protected RTSTurret turret;
        protected int turretIndex;
        protected int aniIndex;
        protected Texture2D aniTexture;
        protected int aniFrames;
        protected string aniName;
        protected bool hasAnimation;
        protected bool targetting;
        protected int cost1;
        protected string cost1Name;
        protected int cost2;
        protected string cost2Name;
        protected int cost3;
        protected string cost3Name;
        //public
        /// <summary>
        /// RTS Ability instance
        /// </summary>
        /// <param name="x">draw x position</param>
        /// <param name="y">draw y position</param>
        /// <param name="width">draw width</param>
        /// <param name="height">draw height</param>
        /// <param name="name">ability name</param>
        /// <param name="source">source texture</param>
        /// <param name="action">name of action generated</param>
        /// <param name="owner">owning object reference</param>
        /// <param name="turret">object object reference</param>
        /// <param name="turretIndex">index of turret object reference</param>
        /// <param name="coolDown">number of frames between activations of ability</param>
        /// <param name="hasAnimation">toggles whether of not the ability has a corisponding animation</param>
        public RTSAbility(int x, int y, int width, int height, string name,
            Texture2D source, string action, RTSObject owner, RTSTurret turret,
            int turretIndex, int coolDown, RTSABILITYTYPE abt, RTSACTIONTYPE act, bool hasAnimation = false)
        {
            destRect.X = x;
            destRect.Y = y;
            destRect.Width = width;
            destRect.Height = height;
            srcRect.X = 0;
            srcRect.Y = 0;
            srcRect.Width = width;
            srcRect.Height = height;
            this.source = source;
            this.action = action;
            this.owner = owner;
            this.name = name;
            box = new Rectangle(x, y, width, height);
            delay = new CoolDown(5);
            this.coolDown = new CoolDown(coolDown);
            timeSpan = coolDown;
            timeSpent = 0;
            this.abt = abt;
            this.act = act;
            this.turret = turret;
            aniIndex = -1;
            aniTexture = default(Texture2D);
            aniFrames = 0;
            aniName = "";
            this.hasAnimation = hasAnimation;
            cost1 = 0;
            cost1Name = "";
            cost2 = 0;
            cost2Name = "";
            cost3 = 0;
            cost3Name = "";
        }
        /// <summary>
        /// RTSAbility copy constructor
        /// </summary>
        /// <param name="obj">RTSAbility instance to copy</param>
        public RTSAbility(RTSAbility obj)
        {
            Width = obj.Width;
            Height = obj.Height;
            source = obj.Source;
            action = obj.Action;
            owner = obj.Owner;
            name = obj.Name;
            box = new Rectangle(obj.Box.X, obj.Box.Y, obj.Box.Width, obj.Box.Height);
            abt = obj.ABT;
            act = obj.ACT;
            delay = new CoolDown(obj.Delay);
            coolDown = new CoolDown(obj.Cool);
            timeSpan = obj.Cool;
            timeSpent = 0;
            turret = obj.Turret;
            turretIndex = obj.turretIndex;
            aniIndex = obj.AniIndex;
            aniTexture = obj.AniTexture;
            aniFrames = obj.AniFrames;
            aniName = obj.AniName;
            hasAnimation = obj.HasAnimation;
            cost1 = obj.Cost1;
            cost1Name = obj.Cost1Name;
            cost2Name = obj.Cost2Name;
            cost3Name = obj.Cost3Name;
        }
        /// <summary>
        /// RTSAbility constructor from file
        /// </summary>
        /// <param name="sr">Stream reader reference</param>
        /// <param name="owner">Object to which this instance belongs</param>
        public RTSAbility(System.IO.StreamReader sr, RTSObject owner)
        {
            sr.ReadLine();
            name = sr.ReadLine();
            destRect.X = Convert.ToInt32(sr.ReadLine());
            destRect.Y = Convert.ToInt32(sr.ReadLine());
            destRect.Width = Convert.ToInt32(sr.ReadLine());
            destRect.Height = Convert.ToInt32(sr.ReadLine());
            action = sr.ReadLine();
            abt = (RTSABILITYTYPE)(Convert.ToInt32(sr.ReadLine()));
            act = (RTSACTIONTYPE)(Convert.ToInt32(sr.ReadLine()));
            delay = new CoolDown(Convert.ToInt32(sr.ReadLine()));
            coolDown = new CoolDown(Convert.ToInt32(sr.ReadLine()));
            timeSpan = coolDown.MaxTicks;
            timeSpent = 0;
            locked = Convert.ToBoolean(sr.ReadLine());
            turretIndex = Convert.ToInt32(sr.ReadLine());
            turret = owner.Turrets[turretIndex];
            aniIndex = Convert.ToInt32(sr.ReadLine());
            aniFrames = Convert.ToInt32(sr.ReadLine());
            aniName = sr.ReadLine();
            switch (aniIndex)
            {
                case 1:
                    owner.AniName1 = aniName;
                    owner.AniFrames1 = aniFrames;
                    break;
                case 2:
                    owner.AniName2 = aniName;
                    owner.AniFrames2 = aniFrames;
                    break;
                case 3:
                    owner.AniName3 = aniName;
                    owner.AniFrames3 = aniFrames;
                    break;
            }
            hasAnimation = Convert.ToBoolean(sr.ReadLine());
            this.owner = owner;
            cost1 = Convert.ToInt32(sr.ReadLine());
            cost1Name = sr.ReadLine();
            cost2 = Convert.ToInt32(sr.ReadLine());
            cost2Name = sr.ReadLine();
            cost3 = Convert.ToInt32(sr.ReadLine());
            cost3Name = sr.ReadLine();
        }
        /// <summary>
        /// renders the ability object
        /// </summary>
        /// <param name="renderer">renderer reference</param>
        public void draw(IntPtr renderer)
        {
            SimpleDraw.draw(renderer, source, srcRect, destRect);
        }
        /// <summary>
        /// renders the ability object
        /// </summary>
        /// <param name="renderer">renderer reference</param>
        /// <param name="x">X postion to render at</param>
        /// <param name="y">Y postion to render at</param>
        /// <param name="w">Width to render at</param>
        /// <param name="h">Height to render at</param>
        public void draw(IntPtr renderer, int x, int y, int w = 32, int h = 32)
        {
            destRect.X = x;
            destRect.Y = y;
            destRect.Width = w;
            destRect.Height = h;
            SimpleDraw.draw(renderer, source, srcRect, destRect);
        }
        /// <summary>
        /// tests if an ability has been clicked by the user
        /// </summary>
        /// <returns>Boolean</returns>
        public bool clicked()
        {
            if (!delay.Active)
            {
                if (MouseHandler.getMouseX() >= destRect.X && MouseHandler.getMouseX() <= destRect.X + destRect.Width)
                {
                    if (MouseHandler.getMouseX() >= destRect.Y && MouseHandler.getMouseX() <= destRect.Y + destRect.Height)
                    {
                        if (MouseHandler.getLeft())
                        {
                            if (!coolDown.Active)
                            {
                                delay.activate();
                                coolDown.activate();
                                return true;
                            }
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// saves object instance data
        /// </summary>
        /// <param name="sw">stream writer reference</param>
        public virtual void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======RTSAbility Data======");
            sw.WriteLine(name);
            sw.WriteLine(destRect.X);
            sw.WriteLine(destRect.Y);
            sw.WriteLine(destRect.Width);
            sw.WriteLine(destRect.Height);
            sw.WriteLine(action);
            sw.WriteLine((int)abt);
            sw.WriteLine((int)act);
            sw.WriteLine(delay.MaxTicks);
            sw.WriteLine(coolDown.MaxTicks);
            sw.WriteLine(locked);
            sw.WriteLine(turretIndex);
            sw.WriteLine(aniIndex);
            sw.WriteLine(aniFrames);
            sw.WriteLine(aniName);
            sw.WriteLine(hasAnimation);
            sw.WriteLine(cost1);
            sw.WriteLine(cost1Name);
            sw.WriteLine(cost2);
            sw.WriteLine(cost2Name);
            sw.WriteLine(cost3);
            sw.WriteLine(cost2Name);
        }
        /// <summary>
        /// load RTS ability object instance
        /// </summary>
        /// <param name="sr">stream reader reference</param>
        /// <param name="owner">instance to which this instance belongs</param>
        public virtual void loadData(System.IO.StreamReader sr, RTSObject owner)
        {
            sr.ReadLine();
            name = sr.ReadLine();
            source = TextureBank.getTexture(name);
            destRect.X = Convert.ToInt32(sr.ReadLine());
            destRect.Y = Convert.ToInt32(sr.ReadLine());
            destRect.Width = Convert.ToInt32(sr.ReadLine());
            destRect.Height = Convert.ToInt32(sr.ReadLine());
            action = sr.ReadLine();
            abt = (RTSABILITYTYPE)(Convert.ToInt32(sr.ReadLine()));
            act = (RTSACTIONTYPE)(Convert.ToInt32(sr.ReadLine()));
            delay = new CoolDown(Convert.ToInt32(sr.ReadLine()));
            coolDown = new CoolDown(Convert.ToInt32(sr.ReadLine()));
            locked = Convert.ToBoolean(sr.ReadLine());
            turretIndex = Convert.ToInt32(sr.ReadLine());
            turret = owner.Turrets[turretIndex];
            aniIndex = Convert.ToInt32(sr.ReadLine());
            aniFrames = Convert.ToInt32(sr.ReadLine());
            aniName = sr.ReadLine();
            switch (aniIndex)
            {
                case 1:
                    owner.AniName1 = aniName;
                    owner.AniFrames1 = aniFrames;
                    break;
                case 2:
                    owner.AniName2 = aniName;
                    owner.AniFrames2 = aniFrames;
                    break;
                case 3:
                    owner.AniName3 = aniName;
                    owner.AniFrames3 = aniFrames;
                    break;
            }
            hasAnimation = Convert.ToBoolean(sr.ReadLine());
            this.owner = owner;
            cost1 = Convert.ToInt32(sr.ReadLine());
            cost1Name = sr.ReadLine();
            cost2 = Convert.ToInt32(sr.ReadLine());
            cost2Name = sr.ReadLine();
            cost3 = Convert.ToInt32(sr.ReadLine());
            cost3Name = sr.ReadLine();
        }
        /// <summary>
        /// Adds action to owners actions list
        /// </summary>
        public virtual void addAction()
        {
            owner.Owner.addAction(action, turret.barrelTip.X, turret.barrelTip.Y, (float)turret.Rotation);
        }
        /// <summary>
        /// Upgrade access to an ability if cost requirements are met
        /// </summary>
        public virtual void addUpgrade()
        {
            if (owner.Owner.Resource1Amount >= cost1)
            {
                if (owner.Owner.Resource2Amount >= cost2)
                {
                    if (owner.Owner.Resource3Amount >= cost3)
                    {
                        owner.Owner.unlockAbility(action);
                        owner.Owner.lockAbility(name);
                    }
                }
            }
        }
        /// <summary>
        /// Train a unit if cost requirement met and unit cap permits 
        /// </summary>
        public virtual void train()
        {
            if (owner.Owner.Resource1Amount >= cost1)
            {
                if (owner.Owner.Resource2Amount >= cost2)
                {
                    if (owner.Owner.Resource3Amount >= cost3)
                    {
                        if (owner.Owner.Units.Count < owner.Owner.UnitCap)
                        {
                            owner.addBuildToken(cost1, cost2, cost3, action);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Calls the ability instance
        /// </summary>
        public virtual void callAbility()
        {
            if (!coolDown.Active)
            {
                switch (abt)
                {
                    case RTSABILITYTYPE.ab_action:
                        addAction();
                        break;
                    case RTSABILITYTYPE.ab_targetted:
                        //can have cost, implement in child class
                        break;
                    case RTSABILITYTYPE.ab_upgrade:
                        addUpgrade();
                        break;
                    case RTSABILITYTYPE.ab_self:
                        //implement in child class
                        break;
                    case RTSABILITYTYPE.ab_cmd:
                        //implement in child class
                        break;
                    case RTSABILITYTYPE.ab_train:
                        train();
                        break;
                    case RTSABILITYTYPE.ab_blank:
                        //left blank to fill voids in ability list
                        break;
                }
                coolDown.activate();
            }
        }
        /// <summary>
        /// Calls the special functions of an ability, needs to be overridden 
        /// </summary>
        /// <param name="owner">Commander instance to which calling instance belongs</param>
        /// <param name="world">World scene reference</param>
        /// <param name="gb">Graphics bank reference</param>
        public virtual void callSpecial(RTSCommander owner, WorldScene world)
        {
            //override in inherited class
        }
        /// <summary>
        /// Updates ability's internal state
        /// </summary>
        public virtual void update()
        {
            /*
            if (coolDown.Active)
            {
                if (abt == ABILITYTYPE.ab_upgrade)
                {
                    timeSpent++;
                    if (timeSpent >= timeSpan)
                    {
                        locked = true;
                        owner.Owner.unlockAbility(action);
                        owner.Owner.lockAbility(name);
                    }
                }
                else if (abt == ABILITYTYPE.ab_train)
                {
                    timeSpent++;
                    if (timeSpent >= timeSpan)
                    {
                        owner.Owner.addUnit(action, owner.RallyPoint.X, owner.RallyPoint.Y, 0);
                        timeSpent = 0;
                    }
                }
            }
                */
            if (box.Width != destRect.Width)
            {
                box.Width = destRect.Width;
            }
            if (box.Height != destRect.Height)
            {
                box.Height = destRect.Height;
            }
            delay.update();
            coolDown.update();
        }
        /// <summary>
        /// Fire ability if owner has a target
        /// </summary>
        public virtual void fire()
        {
            if (owner.Objective != null)
            {
                if (!coolDown.Active)
                {
                    coolDown.activate();
                    if (owner.turretsLocked(owner.Objective.Target))
                    {
                        for (int i = 0; i < owner.Turrets.Count; i++)
                        {
                            owner.Owner.addAction(action, owner.Turrets[i].barrelTip.X,
                                owner.Turrets[i].barrelTip.Y, (float)owner.Turrets[i].Rotation);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Ability's cool down property
        /// </summary>
        public int CoolOff
        {
            get { return coolDown.MaxTicks; }
            set { coolDown = new CoolDown(value); }
        }
        /// <summary>
        /// Render x position property
        /// </summary>
        public float X
        {
            get { return destRect.X; }
            set { destRect.X = value; }
        }
        /// <summary>
        /// Render y position property
        /// </summary>
        public float Y
        {
            get { return destRect.Y; }
            set { destRect.Y = value; }
        }
        /// <summary>
        /// Ability name property
        /// </summary>
        public string Name
        {
            get { return name; }
        }
        /// <summary>
        /// Box property
        /// </summary>
        public Rectangle Box
        {
            get { return box; }
        }
        /// <summary>
        /// Owner object property
        /// </summary>
        public RTSObject Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        /// <summary>
        /// Action property
        /// </summary>
        public string Action
        {
            get { return action; }
            set { action = value; }
        }
        /// <summary>
        /// Ability type property
        /// </summary>
        public RTSABILITYTYPE ABT
        {
            get { return abt; }
            set { abt = value; }
        }
        /// <summary>
        /// Action type property
        /// </summary>
        public RTSACTIONTYPE ACT
        {
            get { return act; }
            set { act = value; }
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
        /// Delay property
        /// </summary>
        public int Delay
        {
            get { return delay.MaxTicks; }
            set { delay = new CoolDown(value); }
        }
        /// <summary>
        /// Cool down property
        /// </summary>
        public int Cool
        {
            get { return coolDown.MaxTicks; }
            set { coolDown = new CoolDown(value); }
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
                //box.Width = value;
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
                //box.Height = value;
            }
        }
        /// <summary>
        /// Turret property
        /// </summary>
        public RTSTurret Turret
        {
            get { return turret; }
            set { turret = value; }
        }
        /// <summary>
        /// Turret index property
        /// </summary>
        public int TurretIndex
        {
            get { return turretIndex; }
            set { turretIndex = value; }
        }
        /// <summary>
        /// toggle if ability is locked or unlocked
        /// </summary>
        public void toggleLock()
        {
            if (locked)
            {
                locked = false;
            }
            else
            {
                locked = true;
            }
        }
        /// <summary>
        /// Locked proerty
        /// </summary>
        public bool Locked
        {
            get { return locked; }
        }
        /// <summary>
        /// Animation index property
        /// </summary>
        public int AniIndex
        {
            get { return aniIndex; }
            set { aniIndex = value; }
        }
        /// <summary>
        /// Animation texture property
        /// </summary>
        public Texture2D AniTexture
        {
            get { return aniTexture; }
            set { aniTexture = value; }
        }
        /// <summary>
        /// Animation frames property
        /// </summary>
        public int AniFrames
        {
            get { return aniFrames; }
            set { aniFrames = value; }
        }
        /// <summary>
        /// Animation name property
        /// </summary>
        public string AniName
        {
            get { return aniName; }
            set { aniName = value; }
        }
        /// <summary>
        /// has animation property
        /// </summary>
        public bool HasAnimation
        {
            get { return hasAnimation; }
            set { hasAnimation = value; }
        }
        /// <summary>
        /// Targettting property
        /// </summary>
        public bool Targetting
        {
            get { return targetting; }
            set { targetting = value; }
        }
        /// <summary>
        /// Cost 1 proerty
        /// </summary>
        public int Cost1
        {
            get { return cost1; }
            set { cost1 = value; }
        }
        /// <summary>
        /// Cost 1 name property
        /// </summary>
        public string Cost1Name
        {
            get { return cost1Name; }
            set { cost1Name = value; }
        }
        /// <summary>
        /// Cost 2 proerty
        /// </summary>
        public int Cost2
        {
            get { return cost1; }
            set { cost2 = value; }
        }
        /// <summary>
        /// Cost 2 name property
        /// </summary>
        public string Cost2Name
        {
            get { return cost2Name; }
            set { cost2Name = value; }
        }
        /// <summary>
        /// Cost 3 proerty
        /// </summary>
        public int Cost3
        {
            get { return cost3; }
            set { cost3 = value; }
        }
        /// <summary>
        /// Cost 3 name property
        /// </summary>
        public string Cost3Name
        {
            get { return cost3Name; }
            set { cost3Name = value; }
        }
        /// <summary>
        /// Tests if ability's cool down is active
        /// </summary>
        /// <returns></returns>
        public bool coolingDown()
        {
            if (coolDown.Active)
            {
                return true;
            }
            return false;
        }
    }
}
