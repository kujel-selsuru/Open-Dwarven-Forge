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
public enum TASKTYPE
    {
        tt_eu = 0, tt_eb, tt_repair, tt_heal, tt_mine, tt_harvest, tt_deposit,
        tt_flee, tt_explore, tt_build, tt_train, tt_upgrade
    };
    public class RTSTask
    {
        //protected
        protected RTSObject target;
        protected RTSObject owner;
        protected TASKTYPE tt;
        protected float x;
        protected float y;
        protected string buildName;
        //public
        public RTSTask(RTSObject target, RTSObject owner, TASKTYPE tt, float x, float y, string buildName = "")
        {
            this.target = target;
            this.owner = owner;
            this.tt = tt;
            this.x = x;
            this.y = y;
            this.buildName = buildName;
        }
        /// <summary>
        /// Task type property
        /// </summary>
        public TASKTYPE TT
        {
            get { return tt; }
        }
        /// <summary>
        /// Target property
        /// </summary>
        public RTSObject Target
        {
            get { return target; }
        }
        /// <summary>
        /// X property
        /// </summary>
        public float X
        {
            get { return x; }
        }
        /// <summary>
        /// Y property
        /// </summary>
        public float Y
        {
            get { return y; }
        }
        /// <summary>
        /// Owner property
        /// </summary>
        public RTSObject Owner
        {
            get { return owner; }
        }
        /// <summary>
        /// Build name property
        /// </summary>
        public string BuildName
        {
            get { return buildName; }
            set { buildName = value; }
        }
    }
    public class RTSAI
    {
        //protected
        protected RTSCommander owner;
        protected PriorityQueue<RTSTask> tasks;
        protected PriorityQueue<RTSUnit> unitDoers;
        protected PriorityQueue<RTSBuilding> buildingDoers;
        protected Counter counter1;
        protected Counter counter2;

        //public
        /// <summary>
        /// basic implementation of RTS AI and layout for later implementations
        /// </summary>
        /// <param name="owner">The RTSCommander to which this AI belongs</param>
        public RTSAI(RTSCommander owner)
        {
            this.owner = owner;
            tasks = new PriorityQueue<RTSTask>();
            unitDoers = new PriorityQueue<RTSUnit>();
            buildingDoers = new PriorityQueue<RTSBuilding>();
            counter1 = new Counter(49);
            counter2 = new Counter(81);
        }
        /// <summary>
        /// Handles building bases and outputting units from bases
        /// Can be overriden to add more specialized behavior
        /// </summary>
        public virtual void baseCommander()
        {
            //cycle through buildings making a list of all buildings AI currently has than cycle 
            //through the workers checking each for a construction not on the list
            List<string> names = new List<string>();
            bool foundNames = false;
            for (int b = 0; b < owner.Buildings.Count; b++)
            {
                foundNames = false;
                for (int n = 0; n < names.Count; n++)
                {
                    if (owner.Buildings[b].Name == names[n])
                    {
                        foundNames = true;
                        break;
                    }
                }
                if (!foundNames)
                {
                    names.Add(owner.Buildings[b].Name);
                }
            }
            //find an available possible upgrad and call it
            string upgradeName = "";
            for (int u = 0; u < owner.Units.Count; u++)
            {
                for (int a = 0; a < owner.Units[u].Abilities.Count; a++)
                {
                    if (owner.Units[u].Abilities[a].ABT == RTSABILITYTYPE.ab_upgrade)
                    {
                        if (owner.Units[u].Abilities[a].Locked)
                        {
                            if (owner.Units[u].Abilities[a].Cost1Name == owner.Resource1)
                            {
                                if (owner.Resource1Amount >= owner.Units[u].Abilities[a].Cost1)
                                {
                                    if (owner.Units[u].Abilities[a].Cost2Name == owner.Resource2)
                                    {
                                        if (owner.Resource2Amount >= owner.Units[u].Abilities[a].Cost2)
                                        {
                                            if (owner.Units[u].Abilities[a].Cost3Name == owner.Resource3)
                                            {
                                                if (owner.Resource3Amount >= owner.Units[u].Abilities[a].Cost3)
                                                {
                                                    owner.Resource1Amount -= owner.Units[u].Abilities[a].Cost1;
                                                    owner.Resource2Amount -= owner.Units[u].Abilities[a].Cost2;
                                                    owner.Resource3Amount -= owner.Units[u].Abilities[a].Cost3;
                                                    upgradeName = owner.Units[u].Abilities[a].Action;
                                                    owner.unlockAbility(upgradeName);

                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }
        /// <summary>
        /// Handles unit response to seeing a target and not already attacking someone
        /// </summary>
        /// <param name="cmdrs">List of all commanders in the world</param>
        /// <param name="rangeScaler">Scaler value for unit sight ranges</param>
        public virtual void unitAssistCommander(List<RTSCommander> cmdrs, int rangeScaler)
        {
            //cycle through all comanders
            for (int c = 0; c < cmdrs.Count; c++)
            {
                //skip commander of this AI
                if (owner.Index != cmdrs[c].Index)
                {
                    //cycle through all units of owner of this AI
                    for (int u = 0; u < owner.Units.Count; u++)
                    {
                        //cycle through all units of each enemy commander and test if distance 
                        //less than sight range of the unit to test and assign attack objective 
                        //to that unit if it has no current objective
                        for (int eu = 0; eu < cmdrs[c].Units.Count; eu++)
                        {
                            if (Point2D.calculateDistance(owner.Units[u].Center,
                                cmdrs[c].Units[eu].Center) <= (owner.Units[u].Sight * rangeScaler))
                            {
                                //Should the unit that saw the target be doing nothing or moving/attacking it may be
                                //automatically asigned to attack targets; 
                                if (owner.Units[u].Objective != null)
                                {
                                    owner.Units[u].changeOBjective(new RTSAttackTarget((int)cmdrs[c].Units[eu].X, (int)cmdrs[c].Units[eu].Y, owner.Units[u], cmdrs[c].Units[eu]));
                                }
                                else
                                {
                                    owner.Units[u].Objective = new RTSAttackTarget((int)cmdrs[c].Units[eu].X, (int)cmdrs[c].Units[eu].Y, owner.Units[u], cmdrs[c].Units[eu]);
                                }
                            }
                        }
                        //cycle through all buildings of each enemy commander and test if distance 
                        //less than sight range of the unit to test and assign attack objective 
                        //to that unit if it has no current objective
                        for (int eb = 0; eb < cmdrs[c].Buildings.Count; eb++)
                        {
                            if (Point2D.calculateDistance(owner.Units[u].Center,
                                cmdrs[c].Units[eb].Center) <= (owner.Units[u].Sight * rangeScaler))
                            {
                                if (owner.Units[u].Objective != null)
                                {
                                    if (owner.Units[u].Objective != null)
                                    {
                                        owner.Units[u].changeOBjective(new RTSAttackTarget((int)cmdrs[c].Buildings[eb].X, (int)cmdrs[c].Buildings[eb].Y, owner.Units[u], cmdrs[c].Buildings[eb]));
                                    }
                                    else
                                    {
                                        owner.Units[u].Objective = new RTSAttackTarget((int)cmdrs[c].Buildings[eb].X, (int)cmdrs[c].Buildings[eb].Y, owner.Units[u], cmdrs[c].Buildings[eb]);
                                    }
                                }
                            }
                        }
                    }
                    //cycle through all buildings of owner of this AI
                    for (int b = 0; b < owner.Buildings.Count; b++)
                    {
                        //cycle through all units of each enemy commander and test if distance 
                        //less than sight range of the building to test and assign attack objective 
                        //to that building if it has no current objective
                        for (int eu = 0; eu < cmdrs[c].Units.Count; eu++)
                        {
                            if (Point2D.calculateDistance(owner.Buildings[b].Center,
                                cmdrs[c].Units[eu].Center) <= (owner.Buildings[b].Sight * rangeScaler))
                            {
                                //Should the building that saw the target be doing nothing or moving/attacking it may be
                                //automatically asigned to attack targets; 
                                if (owner.Buildings[b].Objective != null)
                                {
                                    owner.Buildings[b].changeOBjective(new BuildingAttack((int)cmdrs[c].Units[eu].X, (int)cmdrs[c].Units[eu].Y, owner.Buildings[b], cmdrs[c].Units[eu]));
                                }
                                else
                                {
                                    owner.Buildings[b].Objective = new BuildingAttack((int)cmdrs[c].Units[eu].X, (int)cmdrs[c].Units[eu].Y, owner.Buildings[b], cmdrs[c].Units[eu]);
                                }
                            }
                        }
                        //cycle through all buildings of each enemy commander and test if distance 
                        //less than sight range of the building to test and assign attack objective 
                        //to that unit if it has no current objective
                        for (int eb = 0; eb < cmdrs[c].Buildings.Count; eb++)
                        {
                            if (Point2D.calculateDistance(owner.Buildings[b].Center,
                                cmdrs[c].Units[eb].Center) <= (owner.Buildings[b].Sight * rangeScaler))
                            {
                                if (owner.Buildings[b].Objective != null)
                                {
                                    if (owner.Buildings[b].Objective != null)
                                    {
                                        owner.Buildings[b].changeOBjective(new RTSAttackTarget((int)cmdrs[c].Buildings[eb].X, (int)cmdrs[c].Buildings[eb].Y, owner.Buildings[b], cmdrs[c].Buildings[eb]));
                                    }
                                    else
                                    {
                                        owner.Buildings[b].Objective = new RTSAttackTarget((int)cmdrs[c].Buildings[eb].X, (int)cmdrs[c].Buildings[eb].Y, owner.Buildings[b], cmdrs[c].Buildings[eb]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //cycle through all units of owner of this AI identifiying harvsters and if their tank 
            //is empty attempt to loacate tehm a new resource they had been harvesting
            Point2D tmp = null;
            for (int u = 0; u < owner.Units.Count; u++)
            {
                if (owner.Units[u].UT == UNITTYPE.ut_airHarvester)
                {
                    if (owner.Units[u].Tank == 0)
                    {
                        tmp = locateNearestResource(owner.Units[u].ResourceName, owner, owner.Units[u]);
                        if (tmp != null)
                        {
                            owner.Units[u].Objective = new HarvestTarget((int)tmp.X, (int)tmp.Y, owner.Units[u], null);
                        }
                    }
                }
                else if (owner.Units[u].UT == UNITTYPE.ut_gndHarvester)
                {
                    if (owner.Units[u].Tank == 0)
                    {
                        tmp = locateNearestResource(owner.Units[u].ResourceName, owner, owner.Units[u]);
                        if (tmp != null)
                        {
                            owner.Units[u].Objective = new HarvestTarget((int)tmp.X, (int)tmp.Y, owner.Units[u], null);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Handles all tasks generated
        /// </summary>
        public virtual void unitCommander()
        {
            RTSTask temp = null;
            RTSUnit tempUnit = null;
            while (tasks.Count > 0)
            {
                temp = tasks.dequeue();
                switch (temp.TT)
                {
                    case TASKTYPE.tt_eu:
                        switch (((RTSUnit)temp.Target).UT)
                        {
                            case UNITTYPE.ut_air:
                            case UNITTYPE.ut_airHero:
                            case UNITTYPE.ut_airMedic:
                            case UNITTYPE.ut_airSpecial:
                            case UNITTYPE.ut_airWorker:
                                //cycle through units and find the closest unit capable of 
                                //striking target
                                for (int u = 0; u < owner.Units.Count; u++)
                                {
                                    if (owner.Units[u].canStrikeAir())
                                    {
                                        unitDoers.enqueue(owner.Units[u],
                                            -Point2D.calculateDistance(owner.Units[u].Center,
                                            temp.Target.Center));
                                    }
                                }
                                break;
                            case UNITTYPE.ut_ground:
                            case UNITTYPE.ut_gndHero:
                            case UNITTYPE.ut_gndMedic:
                            case UNITTYPE.ut_gndSpecial:
                            case UNITTYPE.ut_gndWorker:
                                //cycle through units and find the closest unit capable of 
                                //striking target
                                for (int u = 0; u < owner.Units.Count; u++)
                                {
                                    if (owner.Units[u].canStrikeGround())
                                    {
                                        unitDoers.enqueue(owner.Units[u],
                                            -Point2D.calculateDistance(owner.Units[u].Center,
                                            temp.Target.Center));
                                    }
                                }
                                break;
                        }
                        tempUnit = unitDoers.dequeue();
                        do
                        {
                            if (tempUnit.Objective == null)
                            {
                                tempUnit.Objective = new RTSAttackTarget((int)temp.X, (int)temp.Y, tempUnit, temp.Target);
                                unitDoers.clear();
                            }
                        } while (unitDoers.Count > 0);
                        break;
                    case TASKTYPE.tt_eb:
                        switch (((RTSUnit)temp.Target).UT)
                        {
                            case UNITTYPE.ut_ground:
                            case UNITTYPE.ut_gndHero:
                            case UNITTYPE.ut_gndMedic:
                            case UNITTYPE.ut_gndSpecial:
                            case UNITTYPE.ut_gndWorker:
                                //cycle through units and find the closest unit capable of 
                                //striking target
                                for (int u = 0; u < owner.Units.Count; u++)
                                {
                                    if (owner.Units[u].canStrikeGround())
                                    {
                                        unitDoers.enqueue(owner.Units[u],
                                            -Point2D.calculateDistance(owner.Units[u].Center,
                                            temp.Target.Center));
                                    }
                                }
                                break;
                        }
                        tempUnit = unitDoers.dequeue();
                        do
                        {
                            if (tempUnit.Objective == null)
                            {
                                tempUnit.Objective = new RTSAttackTarget((int)temp.X, (int)temp.Y, tempUnit, temp.Target);
                                unitDoers.clear();
                            }
                        } while (unitDoers.Count > 0);
                        break;
                    case TASKTYPE.tt_harvest:
                        //cycle through units and find the closest unit capable of 
                        //harvesting at target
                        for (int u = 0; u < owner.Units.Count; u++)
                        {
                            switch (owner.Units[u].UT)
                            {
                                case UNITTYPE.ut_gndHarvester:
                                case UNITTYPE.ut_airHarvester:
                                    unitDoers.enqueue(owner.Units[u],
                                            -Point2D.calculateDistance(owner.Units[u].Center,
                                            temp.Target.Center));
                                    break;
                            }
                        }
                        tempUnit = unitDoers.dequeue();
                        do
                        {
                            if (tempUnit.Objective == null)
                            {
                                tempUnit.Objective = new HarvestTarget((int)temp.X, (int)temp.Y, tempUnit, temp.Target);
                                unitDoers.clear();
                            }
                        } while (unitDoers.Count > 0);
                        break;
                    case TASKTYPE.tt_mine:
                        //cycle through units and find the closest unit capable of 
                        //harvesting at target
                        for (int u = 0; u < owner.Units.Count; u++)
                        {
                            switch (owner.Units[u].UT)
                            {
                                case UNITTYPE.ut_gndMiner:
                                case UNITTYPE.ut_airMiner:
                                    unitDoers.enqueue(owner.Units[u],
                                            -Point2D.calculateDistance(owner.Units[u].Center,
                                            temp.Target.Center));
                                    break;
                            }
                        }
                        tempUnit = unitDoers.dequeue();
                        do
                        {
                            if (tempUnit.Objective == null)
                            {
                                tempUnit.Objective = new HarvestTarget((int)temp.X, (int)temp.Y, tempUnit, temp.Target);
                                unitDoers.clear();
                            }
                        } while (unitDoers.Count > 0);
                        break;
                    case TASKTYPE.tt_deposit:
                        //cycle through units and find the closest unit capable of 
                        //refining at striking target
                        for (int u = 0; u < owner.Units.Count; u++)
                        {
                            switch (owner.Units[u].UT)
                            {
                                case UNITTYPE.ut_gndHarvester:
                                case UNITTYPE.ut_airHarvester:
                                    for (int b = 0; b < owner.Buildings.Count; b++)
                                    {
                                        unitDoers.enqueue(owner.Units[u],
                                                -Point2D.calculateDistance(owner.Units[u].Center,
                                                owner.Buildings[b].Center));
                                    }
                                    break;
                            }
                        }
                        tempUnit = unitDoers.dequeue();
                        do
                        {
                            if (tempUnit.Objective == null)
                            {
                                tempUnit.Objective = new RefineTarget((int)temp.X, (int)temp.Y, tempUnit, temp.Target);
                                unitDoers.clear();
                            }
                        } while (unitDoers.Count > 0);
                        break;
                    case TASKTYPE.tt_build:
                        //cycle through units and find the closest unit capable of 
                        //building target
                        for (int u = 0; u < owner.Units.Count; u++)
                        {
                            switch (owner.Units[u].UT)
                            {
                                case UNITTYPE.ut_airWorker:
                                case UNITTYPE.ut_gndWorker:
                                    unitDoers.enqueue(owner.Units[u],
                                            -Point2D.calculateDistance(owner.Units[u].Center,
                                            temp.Target.Center));
                                    break;
                            }
                        }
                        tempUnit = unitDoers.dequeue();
                        do
                        {
                            if (tempUnit.Objective == null)
                            {
                                tempUnit.Objective = new BuildTarget((int)temp.X, (int)temp.Y, tempUnit, temp.Target);
                                unitDoers.clear();
                            }
                        } while (unitDoers.Count > 0);
                        break;
                    case TASKTYPE.tt_repair:
                        //cycle through units and find the closest unit capable of 
                        //repairing target
                        for (int u = 0; u < owner.Units.Count; u++)
                        {
                            switch (owner.Units[u].UT)
                            {
                                case UNITTYPE.ut_airWorker:
                                case UNITTYPE.ut_gndWorker:
                                    unitDoers.enqueue(owner.Units[u],
                                            -Point2D.calculateDistance(owner.Units[u].Center,
                                            temp.Target.Center));
                                    break;
                            }
                        }
                        tempUnit = unitDoers.dequeue();
                        do
                        {
                            if (tempUnit.Objective == null)
                            {
                                tempUnit.Objective = new RepairTarget((int)temp.X, (int)temp.Y, tempUnit, temp.Target);
                                unitDoers.clear();
                            }
                        } while (unitDoers.Count > 0);
                        break;
                    case TASKTYPE.tt_heal:
                        //cycle through units and find the closest unit capable of 
                        //healing target
                        for (int u = 0; u < owner.Units.Count; u++)
                        {
                            switch (owner.Units[u].UT)
                            {
                                case UNITTYPE.ut_airMedic:
                                case UNITTYPE.ut_gndMedic:
                                    unitDoers.enqueue(owner.Units[u],
                                            -Point2D.calculateDistance(owner.Units[u].Center,
                                            temp.Target.Center));
                                    break;
                            }
                        }
                        tempUnit = unitDoers.dequeue();
                        do
                        {
                            if (tempUnit.Objective == null)
                            {
                                tempUnit.Objective = new HealTarget((int)temp.X, (int)temp.Y, tempUnit, temp.Target);
                                unitDoers.clear();
                            }
                        } while (unitDoers.Count > 0);
                        break;
                    case TASKTYPE.tt_flee:
                        //add later if needed
                        break;
                    case TASKTYPE.tt_explore:
                        //add later if needed
                        break;
                }
            }
        }
        /// <summary>
        /// Generate tasks for the AI to address
        /// </summary>
        /// <param name="cmdrs">List of all comanders in world</param>
        /// <param name="rangeScaler">Scalling value of sight ranges</param>
        public virtual void generateTasks(List<RTSCommander> cmdrs, int rangeScaler = 32)
        {
            if (counter2.tick())
            {
                //cycle through all comanders
                for (int c = 0; c < cmdrs.Count; c++)
                {
                    //skip commander of this AI
                    if (owner.Index != cmdrs[c].Index)
                    {
                        //cycle through all units of owner of this AI
                        for (int u = 0; u < owner.Units.Count; u++)
                        {
                            //cycle through all units of each enemy commander and test if distance 
                            //less than sight range of the unit to test
                            for (int eu = 0; eu < cmdrs[c].Units.Count; eu++)
                            {
                                if (Point2D.calculateDistance(owner.Units[u].Center,
                                    cmdrs[c].Units[eu].Center) <= (owner.Units[u].Sight * rangeScaler))
                                {
                                    tasks.enqueue(new RTSTask(cmdrs[c].Units[eu], owner.Units[u],
                                        TASKTYPE.tt_eu, cmdrs[c].Units[eu].Position.X,
                                        cmdrs[c].Units[eu].Position.Y), (int)TASKTYPE.tt_eu);
                                }
                            }
                            //cycle through all buildings of each enemy commander and test if distance 
                            //less than sight range of the unit to test
                            for (int eb = 0; eb < cmdrs[c].Buildings.Count; eb++)
                            {
                                if (Point2D.calculateDistance(owner.Units[u].Center,
                                    cmdrs[c].Units[eb].Center) <= (owner.Units[u].Sight * rangeScaler))
                                {
                                    tasks.enqueue(new RTSTask(cmdrs[c].Units[eb], owner.Units[u],
                                        TASKTYPE.tt_eb, cmdrs[c].Units[eb].Position.X,
                                        cmdrs[c].Units[eb].Position.Y), (int)TASKTYPE.tt_eb);
                                }
                            }
                        }
                        //cycle through all buildings of owner of this AI
                        for (int b = 0; b < owner.Buildings.Count; b++)
                        {
                            //cycle through all units of each enemy commander and test if distance 
                            //less than sight range of the building to test
                            for (int eu = 0; eu < cmdrs[c].Units.Count; eu++)
                            {
                                if (Point2D.calculateDistance(owner.Buildings[b].Center,
                                    cmdrs[c].Units[eu].Center) <= (owner.Buildings[b].Sight * rangeScaler))
                                {
                                    tasks.enqueue(new RTSTask(cmdrs[c].Units[eu], owner.Buildings[b],
                                        TASKTYPE.tt_eu, cmdrs[c].Units[eu].Position.X,
                                        cmdrs[c].Units[eu].Position.Y), (int)TASKTYPE.tt_eu);
                                }
                            }
                            //cycle through all buildings of each enemy commander and test if distance 
                            //less than sight range of the building to test
                            for (int eb = 0; eb < cmdrs[c].Buildings.Count; eb++)
                            {
                                if (Point2D.calculateDistance(owner.Buildings[b].Center,
                                    cmdrs[c].Units[eb].Center) <= (owner.Buildings[b].Sight * rangeScaler))
                                {
                                    tasks.enqueue(new RTSTask(cmdrs[c].Units[eb], owner.Buildings[b],
                                        TASKTYPE.tt_eb, cmdrs[c].Units[eb].Position.X,
                                        cmdrs[c].Units[eb].Position.Y), (int)TASKTYPE.tt_eb);
                                }
                            }
                        }
                    }
                }
            }
            if (counter1.tick())
            {
                //cycle through all units of owner of this AI identifying all harvesters, should a 
                //harvester have an empty tank add harvest objective and if the tank is full add deposit task
                for (int u = 0; u < owner.Units.Count; u++)
                {
                    if (owner.Units[u].UT == UNITTYPE.ut_gndHarvester
                        || owner.Units[u].UT == UNITTYPE.ut_airHarvester)
                    {
                        if (owner.Units[u].Tank == owner.Units[u].TankSize)
                        {
                            tasks.enqueue(new RTSTask(owner.Units[u], owner.Units[u],
                                TASKTYPE.tt_deposit, owner.Units[u].Position.X,
                                owner.Units[u].Position.Y), (int)TASKTYPE.tt_deposit);
                        }
                        else if (owner.Units[u].Tank == 0)
                        {
                            Point2D tmp = locateNearestResource(owner.Units[u].ResourceName,
                                owner, owner.Units[u]);
                            owner.Units[u].Objective = new HarvestTarget((int)tmp.X, (int)tmp.Y,
                                owner.Units[u], null);
                        }
                    }
                }
                //cycle through all buildings of owner of this AI and if buildings HP is less than max HP
                //add repair task, if building is a mine add a mine task and if building is still 
                //under construction add build task
                for (int b = 0; b < owner.Buildings.Count; b++)
                {
                    if (owner.Buildings[b].HP < owner.Buildings[b].MaxHP)
                    {
                        tasks.enqueue(new RTSTask(owner.Buildings[b], owner.Buildings[b],
                            TASKTYPE.tt_repair, owner.Buildings[b].X,
                            owner.Buildings[b].Y), (int)TASKTYPE.tt_repair);
                    }
                    if (owner.Buildings[b].BT == BUILDINGTYPE.bt_mine)
                    {
                        tasks.enqueue(new RTSTask(owner.Buildings[b], owner.Buildings[b],
                            TASKTYPE.tt_harvest, owner.Buildings[b].X,
                            owner.Buildings[b].Y), (int)TASKTYPE.tt_mine);
                    }
                    else if (((RTSBuilding)owner.Buildings[b]).CT == CONSTRUCTIONTYPE.ct_constructing)
                    {
                        tasks.enqueue(new RTSTask(owner.Buildings[b], owner.Buildings[b],
                            TASKTYPE.tt_build, owner.Buildings[b].X,
                            owner.Buildings[b].Y), (int)TASKTYPE.tt_build);
                    }
                }
            }
        }
        /// <summary>
        /// Locates the nearest resource of the same type a harvester was collecting and generates a 
        /// a task corrisponding to that resource
        /// </summary>
        /// <param name="name">Name of resource</param>
        /// <param name="owner">Owning commander of the harvester</param>
        /// <param name="harvester">The harvester being compared against for distance</param>
        public virtual Point2D locateNearestResource(string name, RTSCommander owner, RTSUnit harvester)
        {
            RTSResource temp = null;
            RTSResource temp2 = null;
            for (int col = 0; col < owner.Fog.Width; col++)
            {
                for (int row = 0; row < owner.Fog.Height; row++)
                {
                    if (!owner.Fog.grid(col, row))
                    {
                        temp = owner.World.Ores.atPosition(col, row);
                        if (temp != null)
                        {
                            if (temp.Name == name)
                            {
                                if (temp2 != null)
                                {
                                    if (Point2D.AsqrtB(temp.Position, harvester.Position) <
                                        Point2D.AsqrtB(temp2.Position, harvester.Position))
                                    {
                                        temp2 = temp;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //tasks.enqueue(new RTSTask(null, harvester, TASKTYPE.tt_harvest, 
            //                        temp2.X * owner.World.TileWidth, temp2.Y * owner.World.TileHeight), 
            //                        (int)TASKTYPE.tt_harvest);
            return temp2.Position;
        }
        /// <summary>
        /// Add a task to the internal list of tasks
        /// </summary>
        /// <param name="t">Task to add to internal queue</param>
        /// <param name="pri">Task priority</param>
        public void addTask(RTSTask t, int pri)
        {
            tasks.enqueue(t, pri);
        }
        /// <summary>
        /// Handles responding to emeny sightings and assisant units like workers and harvesters
        /// </summary>
        public virtual void AIAssistant()
        {
            //impliment in sub classes
        }
        /// <summary>
        /// Finds a random position on the parimeter of the base
        /// </summary>
        /// <returns>WorldForge.Point instance</returns>
        public Point2D getRandBasePrim()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            Point2D baseCenter = null;
            float bx = 0;
            float by = 0;
            for (int b = 0; b < owner.Buildings.Count; b++)
            {
                bx += owner.Buildings[b].X;
                by += owner.Buildings[b].Y;
                baseCenter = new Point2D(bx / owner.Buildings.Count, by / owner.Buildings.Count);
            }
            int num = rand.Next(0, 360);
            int num2 = rand.Next(0, 360);
            bx = (float)Math.Sin(num) * num2;
            by = (float)Math.Cos(num) * num2;
            return new Point2D(bx, by);
        }
        /// <summary>
        /// Counter1 property
        /// </summary>
        public Counter Counter1
        {
            get { return counter1; }
        }
        /// <summary>
        /// Counter2 property
        /// </summary>
        public Counter Counter2
        {
            get { return counter2; }
        }
        /// <summary>
        /// Tasks property
        /// </summary>
        public PriorityQueue<RTSTask> Tasks
        {
            get { return tasks; }
        }
    }
}
