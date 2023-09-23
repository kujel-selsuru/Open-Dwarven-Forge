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
    public enum OBJECTTYPE { ot_unit = 0, ot_building, ot_action, ot_effect };
    /// <summary>
    /// A class that stores and builds build unit commands in buildings/units
    /// </summary>
    public class BuildToken
    {
        //protected
        protected int cost1;
        protected int cost2;
        protected int cost3;
        protected string name;
        protected RTSObject owner;
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
        public BuildToken(int cost1, int cost2, int cost3, string name, RTSObject owner)
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
        public BuildToken(System.IO.StreamReader sr, RTSObject owner)
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
        public BuildToken(BuildToken obj)
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
                owner.Owner.addUnit(name, owner.RallyPoint.X, owner.RallyPoint.Y, rotation);
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
        public RTSObject Owner
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
    public class RTSObject : XenoSprite
    {
        //protected
        protected RTSCommander owner;
        protected bool dead;
        protected int maxHP;
        protected int sight;
        protected List<RTSAbility> abilities;
        protected List<RTSTurret> turrets;
        protected RTSObjective objective;
        protected RTSObjective tempObjective;
        protected OBJECTTYPE ot;
        protected Point2D rallyPoint;
        protected int cost1;
        protected string cost1Name;
        protected int cost2;
        protected string cost2Name;
        protected int cost3;
        protected string cost3Name;
        protected int aniFrames1;
        protected string aniName1;
        protected int aniFrames2;
        protected string aniName2;
        protected int aniFrames3;
        protected string aniName3;
        protected Queue<BuildToken> buildQueue;
        protected BuildToken buildToken;
        //public
        /// <summary>
        /// builds a RTS Object instance, sprite details parsed from sprite name
        /// </summary>
        /// <param name="spriteName">sprite sheet name in graphics bank</param>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        /// <param name="numFrames">number of frames</param>
        /// <param name="maxHP">max object hp</param>
        /// <param name="ot">Object type</param>
        /// <param name="owner">Owning RTS commander</param>
        public RTSObject(string spriteName, float x, float y, int w, int h, int numFrames, int maxHP, OBJECTTYPE ot, string name, RTSCommander owner = null)
            : base(spriteName, x, y, w, h, numFrames)
        {
            this.maxHP = maxHP;
            speed = 0;
            dead = false;
            sight = 1;
            objective = null;
            tempObjective = null;
            abilities = new List<RTSAbility>();
            turrets = new List<RTSTurret>();
            this.ot = ot;
            cost1 = 0;
            cost1Name = "";
            cost2 = 0;
            cost2Name = "";
            cost3 = 0;
            cost3Name = "";
            rallyPoint = new Point2D(x, y + w);
            buildQueue = new Queue<BuildToken>();
            buildToken = null;
            this.name = name;
            this.owner = owner;
            aniFrames1 = 0;
            aniName1 = "";
            aniFrames2 = 0;
            aniName2 = "";
            aniFrames3 = 0;
            aniName3 = "";
        }
        /// <summary>
        /// RTSObject constructor
        /// </summary>
        /// <param name="obj">RTSObject instance to copy</param>
        public RTSObject(RTSObject obj) : base((XenoSprite)obj)
        {
            owner = obj.Owner;
            name = obj.Name;
            speed = obj.Speed;
            hp = obj.HP;
            maxHP = obj.MaxHP;
            dead = obj.Dead;
            sight = obj.Sight;
            objective = null;
            tempObjective = null;
            abilities = new List<RTSAbility>();
            turrets = new List<RTSTurret>();
            for (int a = 0; a < obj.Abilities.Count; a++)
            {
                abilities.Add(new RTSAbility(obj.Abilities[a]));
            }
            for (int t = 0; t < obj.Turrets.Count; t++)
            {
                turrets.Add(new RTSTurret(obj.Turrets[t]));
            }
            ot = obj.OT;
            cost1 = obj.Cost1;
            cost1Name = obj.Cost1Name;
            cost2 = obj.Cost2;
            cost2Name = obj.Cost2Name;
            cost3 = obj.Cost3;
            cost3Name = obj.Cost3Name;
            aniFrames1 = obj.AniFrames1;
            aniName1 = obj.AniName1;
            rallyPoint = new Point2D(obj.RallyPoint.X, obj.RallyPoint.Y);
            buildQueue = new Queue<BuildToken>();
            for (int b = 0; b < obj.BuildQueue.Count; b++)
            {
                buildQueue.Enqueue(new BuildToken(obj.BuildQueue.ToArray()[b]));
            }
            buildToken = null;
        }
        /// <summary>
        /// RTS Object constructor from file
        /// </summary>
        /// <param name="sr">Stream reader refenece</param>
        public RTSObject(System.IO.StreamReader sr) : base(sr)
        {
            {
                sr.ReadLine();
                speed = (float)Convert.ToDouble(sr.ReadLine());
                hp = Convert.ToInt32(sr.ReadLine());
                maxHP = Convert.ToInt32(sr.ReadLine());
                sight = Convert.ToInt32(sr.ReadLine());
                objective = null;
                tempObjective = null;
                int num = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < num; i++)
                {
                    RTSTurret tempT = new RTSTurret(default(Texture2D), "", 0, 0, 0, 0, 0, 0, 0, 0);
                    tempT.loadData(sr);
                    turrets.Add(tempT);
                }
                num = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < num; i++)
                {
                    RTSAbility tempA = new RTSAbility(0, 0, 0, 0, "", default(Texture2D), "", null, null, -1, 0,
                            RTSABILITYTYPE.ab_action, RTSACTIONTYPE.at_g2g, false);
                    tempA.loadData(sr, this);
                    abilities.Add(tempA);
                }
                ot = (OBJECTTYPE)Convert.ToInt32(sr.ReadLine());
                cost1 = Convert.ToInt32(sr.ReadLine());
                cost1Name = sr.ReadLine();
                cost2 = Convert.ToInt32(sr.ReadLine());
                cost2Name = sr.ReadLine();
                cost3 = Convert.ToInt32(sr.ReadLine());
                cost3Name = sr.ReadLine();
                rallyPoint = new Point2D((int)Convert.ToInt32(sr.ReadLine()), (int)Convert.ToInt32(sr.ReadLine()));
                num = Convert.ToInt32(sr.ReadLine());
                buildQueue = new Queue<BuildToken>();
                for (int b = 0; b < num; b++)
                {
                    buildQueue.Enqueue(new BuildToken(sr, this));
                }
                if (sr.ReadLine() == "null")
                {
                    buildToken = null;
                }
                else
                {
                    buildToken = new BuildToken(sr, this);
                }
            }
            name = sr.ReadLine();
            owner = null;
        }
        /// <summary>
        /// Moves the sprite in a direction at a given speed
        /// </summary>
        /// <param name="direction">direction in radians</param>
        /// <param name="speed">speed in pixels</param>
        public override void move(double direction, float speed)
        {
            double mx = Math.Cos(direction) * (double)speed;
            double my = Math.Sin(direction) * (double)speed;
            hitBox.X += (float)mx;
            hitBox.Y += (float)my;
            destRect.x += (int)mx;
            destRect.y += (int)my;
            hitBox.move((float)mx, (float)my);
            for (int t = 0; t < turrets.Count; t++)
            {
                turrets[t].moveDirection(direction, speed);
            }
        }
        /// <summary>
        /// Moves the sprite in explicit x and y directions
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="my"></param>
        public override void move(float mx, float my)
        {
            hitBox.X += mx;
            hitBox.Y += my;
            destRect.x += (int)mx;
            destRect.y += (int)my;
            hitBox.move(mx, my);
            for (int t = 0; t < turrets.Count; t++)
            {
                turrets[t].moveExplicit(mx, my);
            }
        }
        /// <summary>
        /// draws the RTS object and its respective turrets
        /// </summary>
        /// <param name="renderer">renderer reference</param>
        /// <param name="winx">window x position</param>
        /// <param name="winy">window y position</param>
        /// <param name="rotation">rotation of rendered source</param>
        public override void draw(IntPtr renderer, double rotation = 0, int winx = 0, int winy = 0)
        {
            //base.draw(sb, color, winx, winy, rotation);
            base.draw(renderer, rotation, winx, winy);
            for (int t = 0; t < turrets.Count; t++)
            {
                turrets[t].draw(renderer);
            }
        }
        /// <summary>
        /// Owner property
        /// </summary>
        public RTSCommander Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        /// <summary>
        /// Dead property
        /// </summary>
        public bool Dead
        {
            get { return dead; }
            set { dead = value; }
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
        /// Sight property;
        /// </summary>
        public int Sight
        {
            get { return sight; }
            set { sight = value; }
        }
        /// <summary>
        /// abilities property
        /// </summary>
        public List<RTSAbility> Abilities
        {
            get { return abilities; }
        }
        /// <summary>
        /// Turrets property
        /// </summary>
        public List<RTSTurret> Turrets
        {
            get { return turrets; }
        }
        /// <summary>
        /// Adds an RTS ability instance to the RTS object instance
        /// </summary>
        /// <param name="ab">the ability instance to add to this RTS object instance</param>
        public void addAbility(RTSAbility ab)
        {
            ab.Owner = this;
            if (ab.TurretIndex > -1 && ab.TurretIndex < turrets.Count)
            {
                ab.Turret = turrets[ab.TurretIndex];
            }
            abilities.Add(ab);
        }
        /// <summary>
        /// adds a RTS turret instance to this RTS Object instance
        /// </summary>
        /// <param name="tur">the turret instance to add to this RTS object instance</param>
        public void addTurret(RTSTurret tur)
        {
            turrets.Add(tur);
        }
        /*
        /// <summary>
        /// Call an ability by name if the instance poseses it
        /// </summary>
        /// <param name="name">Name of ability to call</param>
        public void callAbility(string name)
        {
            for (int a = 0; a < abilities.Count; a++)
            {
                if (abilities[a].Name == name)
                {
                    abilities[a].callAbility();
                }
            }
        }
        */
        /// <summary>
        /// tests if unit posses an ability to strike air units
        /// </summary>
        /// <returns>returns true if can strike air units</returns>
        public bool canStrikeAir()
        {
            for (int a = 0; a < abilities.Count; a++)
            {
                switch (abilities[a].ACT)
                {
                    case RTSACTIONTYPE.at_a2a:
                    case RTSACTIONTYPE.at_g2a:
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// tests if unit posses an ability to strike ground units
        /// </summary>
        /// <returns>returns true if can strike ground units</returns>
        public bool canStrikeGround()
        {
            for (int a = 0; a < abilities.Count; a++)
            {
                switch (abilities[a].ACT)
                {
                    case RTSACTIONTYPE.at_a2g:
                    case RTSACTIONTYPE.at_g2g:
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Tests if RTS object can heal
        /// </summary>
        /// <returns>Returns true if can and false if cannot</returns>
        public bool canHeal()
        {
            for (int a = 0; a < abilities.Count; a++)
            {
                switch (abilities[a].ACT)
                {
                    case RTSACTIONTYPE.at_heal:
                    case RTSACTIONTYPE.at_buff:
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Tests if RTS object can repair
        /// </summary>
        /// <returns>Returns true if can and false if cannot</returns>
        public bool canRepair()
        {
            for (int a = 0; a < abilities.Count; a++)
            {
                switch (abilities[a].ACT)
                {
                    case RTSACTIONTYPE.at_repair:
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Tests if RTS object can build
        /// </summary>
        /// <returns>Returns true if can and false if cannot</returns>
        public bool canBuild()
        {
            for (int a = 0; a < abilities.Count; a++)
            {
                switch (abilities[a].ACT)
                {
                    case RTSACTIONTYPE.at_build:
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Tests if RTS object can special
        /// </summary>
        /// <returns>Returns true if can and false if cannot</returns>
        public bool canSpecial()
        {
            for (int a = 0; a < abilities.Count; a++)
            {
                switch (abilities[a].ACT)
                {
                    case RTSACTIONTYPE.at_special:
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Tests if RTS object can harvest
        /// </summary>
        /// <returns>Returns true if can and false if cannot</returns>
        public bool canHarvest()
        {
            for (int a = 0; a < abilities.Count; a++)
            {
                switch (abilities[a].ACT)
                {
                    case RTSACTIONTYPE.at_harvest:
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Tests if RTS object can mine
        /// </summary>
        /// <returns>Returns true if can and false if cannot</returns>
        public bool canMine()
        {
            for (int a = 0; a < abilities.Count; a++)
            {
                switch (abilities[a].ACT)
                {
                    case RTSACTIONTYPE.at_mine:
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Tests if RTS object can debuff
        /// </summary>
        /// <returns>Returns true if can and false if cannot</returns>
        public bool canDebuff()
        {
            for (int a = 0; a < abilities.Count; a++)
            {
                switch (abilities[a].ACT)
                {
                    case RTSACTIONTYPE.at_debuff:
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Objective property
        /// </summary>
        public RTSObjective Objective
        {
            get { return objective; }
            set { objective = value; }
        }
        /// <summary>
        /// virtual function for updating RTSObjects
        /// </summary>
        public virtual void update()
        {
            //Update build queue and current build token
            while (buildToken != null)
            {
                if (buildToken.Percentage < 100)
                {
                    buildToken.update();
                }
                else
                {
                    if (buildQueue.Count > 0)
                    {
                        if (buildQueue.Peek().Cost1 >= owner.Resource1Amount)
                        {
                            if (buildQueue.Peek().Cost2 >= owner.Resource2Amount)
                            {
                                if (buildQueue.Peek().Cost3 >= owner.Resource3Amount)
                                {
                                    owner.Resource1Amount -= buildQueue.Peek().Cost1;
                                    owner.Resource2Amount -= buildQueue.Peek().Cost2;
                                    owner.Resource3Amount -= buildQueue.Peek().Cost3;
                                    buildToken = buildQueue.Dequeue();
                                }
                                else
                                {
                                    buildToken = null;
                                }
                            }
                            else
                            {
                                buildToken = null;
                            }
                        }
                        else
                        {
                            buildToken = null;
                        }
                    }
                    else
                    {
                        buildToken = null;
                    }
                }
            }
            if (buildToken == null && buildQueue.Count > 0)
            {
                if (buildQueue.Peek().Cost1 >= owner.Resource1Amount)
                {
                    if (buildQueue.Peek().Cost2 >= owner.Resource2Amount)
                    {
                        if (buildQueue.Peek().Cost3 >= owner.Resource3Amount)
                        {
                            owner.Resource1Amount -= buildQueue.Peek().Cost1;
                            owner.Resource2Amount -= buildQueue.Peek().Cost2;
                            owner.Resource3Amount -= buildQueue.Peek().Cost3;
                            buildToken = buildQueue.Dequeue();
                        }
                    }
                }
            }
            objective.update();
            //implment more fnctionailty in child classes
        }
        /// <summary>
        /// Applies strike damage to an RTS object
        /// </summary>
        /// <param name="val">Value of damage done in strike</param>
        public virtual void strike(int val)
        {
            if (val > 0)
            {
                hp -= val;
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
                buildQueue.Enqueue(new BuildToken(cost1, cost2, cost3, name, this));
            }
        }
        /// <summary>
        /// Cancel a build token and recover spent resources
        /// </summary>
        public void cancelBuildToken()
        {
            if (buildToken != null)
            {
                owner.Resource1Amount += buildToken.Cost1;
                owner.Resource2Amount += buildToken.Cost2;
                owner.Resource3Amount += buildToken.Cost3;
                buildToken = null;
            }
        }
        /// <summary>
        /// Saves RTS object data
        /// </summary>
        /// <param name="sw">Stream writer reference</param>
        public override void saveData(System.IO.StreamWriter sw)
        {
            base.saveData(sw);
            sw.WriteLine("======RTSObject Data======");
            sw.WriteLine(speed);
            sw.WriteLine(hp);
            sw.WriteLine(maxHP);
            sw.WriteLine(sight);
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
            sw.WriteLine(ot);
            sw.WriteLine(cost1);
            sw.WriteLine(cost1Name);
            sw.WriteLine(cost2);
            sw.WriteLine(cost2Name);
            sw.WriteLine(cost3);
            sw.WriteLine(cost3Name);
            sw.WriteLine(rallyPoint.X);
            sw.WriteLine(rallyPoint.Y);
            sw.WriteLine(buildQueue.Count);
            for (int b = 0; b < buildQueue.Count; b++)
            {
                buildQueue.ToArray()[b].saveData(sw);
            }
            if (buildToken != null)
            {
                buildToken.saveData(sw);
            }
            else
            {
                sw.WriteLine("null");
            }
            sw.WriteLine(name);
        }
        /// <summary>
        /// Loads RTS object data from file
        /// </summary>
        /// <param name="sr">Stream reader reference</param>
        public override void loadData(System.IO.StreamReader sr)
        {
            base.loadData(sr);
            sr.ReadLine();
            speed = (float)Convert.ToDouble(sr.ReadLine());
            hp = Convert.ToInt32(sr.ReadLine());
            maxHP = Convert.ToInt32(sr.ReadLine());
            sight = Convert.ToInt32(sr.ReadLine());
            int num = Convert.ToInt32(sr.ReadLine());
            for (int i = 0; i < num; i++)
            {
                RTSTurret tempT = new RTSTurret(default(Texture2D), "", 0, 0, 0, 0, 0, 0, 0, 0);
                tempT.loadData(sr);
                turrets.Add(tempT);
            }
            num = Convert.ToInt32(sr.ReadLine());
            for (int i = 0; i < num; i++)
            {
                RTSAbility tempA = new RTSAbility(0, 0, 0, 0, "", default(Texture2D), "", null, null, -1, 0,
                        RTSABILITYTYPE.ab_action, RTSACTIONTYPE.at_g2g, false);
                tempA.loadData(sr, this);
                abilities.Add(tempA);
            }
            ot = (OBJECTTYPE)Convert.ToInt32(sr.ReadLine());
            cost1 = Convert.ToInt32(sr.ReadLine());
            cost1Name = sr.ReadLine();
            cost2 = Convert.ToInt32(sr.ReadLine());
            cost2Name = sr.ReadLine();
            cost3 = Convert.ToInt32(sr.ReadLine());
            cost3Name = sr.ReadLine();
            rallyPoint = new Point2D((int)Convert.ToInt32(sr.ReadLine()), (int)Convert.ToInt32(sr.ReadLine()));
            num = Convert.ToInt32(sr.ReadLine());
            buildQueue = new Queue<BuildToken>();
            for (int b = 0; b < num; b++)
            {
                buildQueue.Enqueue(new BuildToken(sr, this));
            }
            if (sr.ReadLine() == "null")
            {
                buildToken = null;
            }
            else
            {
                buildToken = new BuildToken(sr, this);
            }
            name = sr.ReadLine();
        }
        /// <summary>
        /// Calls an RTS object's special function
        /// </summary>
        /// <param name="world">Name of special ability to call</param>
        /// <param name="world">World scene reference</param>
        /// <param name="gb">Graphics bank</param>
        public virtual void callSpecial(string name, WorldScene world)
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                if (abilities[i].Name == name)
                {
                    if (abilities[i].ACT == RTSACTIONTYPE.at_special)
                    {
                        abilities[i].callSpecial(owner, world);
                    }
                }
            }
        }
        /// <summary>
        /// Call the first available attack ability in the direction of provided target
        /// </summary>
        /// <param name="target">Target object to attack</param>
        public virtual void callAttack(RTSObject target)
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                switch (target.OT)
                {
                    case OBJECTTYPE.ot_building:
                        if (abilities[i].ACT == RTSACTIONTYPE.at_g2g)
                        {
                            abilities[i].addAction();
                            return;
                        }
                        else if (abilities[i].ACT == RTSACTIONTYPE.at_a2g)
                        {
                            abilities[i].addAction();
                            return;
                        }
                        break;
                    case OBJECTTYPE.ot_unit:
                        switch (((RTSUnit)target).UT)
                        {
                            case UNITTYPE.ut_air:
                            case UNITTYPE.ut_airHarvester:
                            case UNITTYPE.ut_airHero:
                            case UNITTYPE.ut_airMedic:
                            case UNITTYPE.ut_airSpecial:
                            case UNITTYPE.ut_airWorker:
                                abilities[i].addAction();
                                return;
                            case UNITTYPE.ut_ground:
                            case UNITTYPE.ut_gndHarvester:
                            case UNITTYPE.ut_gndHero:
                            case UNITTYPE.ut_gndMedic:
                            case UNITTYPE.ut_gndSpecial:
                            case UNITTYPE.ut_gndWorker:
                                abilities[i].addAction();
                                return;
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Call the specified ability
        /// </summary>
        /// <param name="name">Name of ability to call, "any" used to call any ability</param>
        public virtual void callAbility(string name)
        {
            if (name == "any")
            {
                for (int i = 0; i < abilities.Count; i++)
                {
                    if (!abilities[i].coolingDown())
                    {
                        abilities[i].callAbility();
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < abilities.Count; i++)
                {
                    if (abilities[i].Name == name)
                    {
                        abilities[i].callAbility();
                        break;
                    }

                }
            }
        }
        /// <summary>
        /// Return the name of first ability of specified type
        /// </summary>
        /// <param name="abilityType">Type of ability by action to search for</param>
        /// <returns>Name of ability, returns "" if none found</returns>
        public string getAbilityOfType(RTSACTIONTYPE abilityType)
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                if (abilities[i].ACT == abilityType)
                {
                    return abilities[i].Name;
                }
            }
            return "";
        }
        /// <summary>
        /// Have all turrets track the provided target;
        /// </summary>
        /// <param name="target">RTSObject reference</param>
        public void trackTarget(RTSObject target)
        {
            for (int i = 0; i < turrets.Count; i++)
            {
                turrets[i].trackTarget(new Point2D(target.HitBox.X, target.HitBox.Y));
            }
        }
        /// <summary>
        /// Tests if all turrets are facing the target
        /// </summary>
        /// <param name="target">RTSObject reference</param>
        /// <returns>Returns true if all turrets facing target and false otherwise</returns>
        public bool turretsLocked(RTSObject target)
        {
            for (int i = 0; i < turrets.Count; i++)
            {
                if (Point2D.CalcAngle(turrets[i].Pivot, new Point2D(target.HitBox.X, target.HitBox.Y)) == turrets[i].Rotation)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Change the current objective if either move or shoot and store for later restoration
        /// </summary>
        /// <param name="newObjective">the new objective to be applied</param>
        public void changeOBjective(RTSObjective newObjective)
        {
            if (newObjective.OT == RTSOBJECTIVETYPE.ot_attack)
            {
                if (objective.OT == RTSOBJECTIVETYPE.ot_move || objective.OT == RTSOBJECTIVETYPE.ot_shoot)
                {
                    tempObjective = objective;
                    objective = newObjective;
                }
            }
        }
        /// <summary>
        /// restores objective if a temporary objective had been set
        /// </summary>
        public void restoreObjective()
        {
            if (tempObjective != null)
            {
                if (objective.State == -1)
                {
                    objective = tempObjective;
                    tempObjective = null;
                }
            }
        }
        /// <summary>
        /// Temp Objective property
        /// </summary>
        public RTSObjective TempObjective
        {
            get { return tempObjective; }
        }
        /// <summary>
        /// Object type property
        /// </summary>
        public OBJECTTYPE OT
        {
            get { return ot; }
            set { ot = value; }
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
        /// Rally point proerty
        /// </summary>
        public Point2D RallyPoint
        {
            get { return rallyPoint; }
            set { rallyPoint = value; }
        }
        /// <summary>
        /// Build queue property
        /// </summary>
        public Queue<BuildToken> BuildQueue
        {
            get { return buildQueue; }
        }
        /// <summary>
        /// AniFrames1 property
        /// </summary>
        public int AniFrames1
        {
            get { return aniFrames1; }
            set { aniFrames1 = value; }
        }
        /// <summary>
        /// AniName1 property
        /// </summary>
        public string AniName1
        {
            get { return aniName1; }
            set { aniName1 = value; }
        }
        /// <summary>
        /// AniFrames2 property
        /// </summary>
        public int AniFrames2
        {
            get { return aniFrames2; }
            set { aniFrames2 = value; }
        }
        /// <summary>
        /// AniName2 property
        /// </summary>
        public string AniName2
        {
            get { return AniName1; }
            set { aniName1 = value; }
        }
        /// <summary>
        /// AniFrames3 property
        /// </summary>
        public int AniFrames3
        {
            get { return aniFrames3; }
            set { aniFrames3 = value; }
        }
        /// <summary>
        /// AniName3 property
        /// </summary>
        public string AniName3
        {
            get { return aniName3; }
            set { aniName3 = value; }
        }
        /// <summary>
        /// Position property
        /// </summary>
        public Point2D Position
        {
            get { return new Point2D(hitBox.X, hitBox.Y); }
            set {
                    hitBox.X = value.X;
                    hitBox.Y = value.Y;
                }
        }
    }
}
