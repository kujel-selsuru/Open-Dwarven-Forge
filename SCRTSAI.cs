//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XenoLib;

namespace XenoLib
{
    //Task flags
    public enum TASKS
    {
        T_ATTACK = 0,
        T_DEFEND,
        T_REPAIR,
        T_HEAL,
        T_BUILD,
        T_SCOUT,
        T_HARVEST,
        T_REFINE,
        T_ABILITY,
        T_TRAIN,
        T_UPGRADE,
        T_IDLE,
        T_CONSTRUCT
    }

    public class SCRTSTask
    {
        //protected
        protected TASKS task;
        protected Point2D pos;
        protected SCRTSUnit issuer;
        protected SCRTSUnit target;
        protected string action;

        //public
        /// <summary>
        /// RTSTask constructor
        /// </summary>
        /// <param name="task">TASKS flaf</param>
        /// <param name="x">X position of task</param>
        /// <param name="y">Y position of task</param>
        /// <param name="issuer">Issuer of task</param>
        /// <param name="target">Target of task</param>
        /// <param name="action">Action of task</param>
        public SCRTSTask(TASKS task, float x, float y,
            SCRTSUnit issuer, SCRTSUnit target, string action = "")
        {
            this.task = task;
            pos = new Point2D(x, y);
            this.issuer = issuer;
            this.target = target;
            this.action = action;
        }
        /// <summary>
        /// Task property
        /// </summary>
        public TASKS Task
        {
            get { return task; }
        }
        /// <summary>
        /// Pos property
        /// </summary>
        public Point2D Pos
        {
            get { return pos; }
        }
        /// <summary>
        /// Issuer property
        /// </summary>
        public SCRTSUnit Issuer
        {
            get { return issuer; }
        }
        /// <summary>
        /// Target property
        /// </summary>
        public SCRTSUnit Target
        {
            get { return target; }
        }
        /// <summary>
        /// Action property
        /// </summary>
        public string Act
        {
            get { return action; }
        }
    }

    public class SCRTSDoer
    {
        //protected
        protected TASKS task;
        protected Point2D pos;
        protected SCRTSUnit doer;
        protected string action;

        //public
        /// <summary>
        /// SCRTSDoer constructor
        /// </summary>
        /// <param name="task">TASKS flag</param>
        /// <param name="x">X position of doer</param>
        /// <param name="y">Y position of doer</param>
        /// <param name="doer">Doer of task</param>
        /// <param name="action">Action of task</param>
        public SCRTSDoer(TASKS task, float x, float y,
            SCRTSUnit doer, string action = "")
        {
            this.task = task;
            pos = new Point2D(x, y);
            this.doer = doer;
            this.action = action;
        }
        /// <summary>
        /// Task propert
        /// </summary>
        public TASKS Task
        {
            get { return task; }
        }
        /// <summary>
        /// Pos property
        /// </summary>
        public Point2D Pos
        {
            get { return pos; }
        }
        /// <summary>
        /// Doer property
        /// </summary>
        public SCRTSUnit Doer
        {
            get { return doer; }
        }
        /// <summary>
        /// Action property
        /// </summary>
        public string Act
        {
            get { return action; }
        }
    }

    public class SCRTSAI
    {
        //protected
        protected PriorityQueue<SCRTSTask> tasks;
        protected PriorityQueue<SCRTSDoer> doers;
        protected SCRTSCommander cmdr;
        protected List<string> unitBuildQueueNames;
        protected int unitQueueIndex;
        protected List<string> buildingBuildQueueNames;
        protected List<bool> buildingBuildQueueBools;
        protected int buildingQueueIndex;
        protected Random rand;
        protected RTSWorld world;

        //public
        /// <summary>
        /// SCRTSAI Constructor
        /// </summary>
        /// <param name="cmdr">RTSCommander reference</param>
        /// <param name="crucable">Crucable reference</param>
        public SCRTSAI(SCRTSCommander cmdr, RTSWorld world)
        {
            tasks = new PriorityQueue<SCRTSTask>();
            doers = new PriorityQueue<SCRTSDoer>();
            this.cmdr = cmdr;
            unitBuildQueueNames = new List<string>();
            unitQueueIndex = 0;
            buildingBuildQueueNames = new List<string>();
            buildingBuildQueueBools = new List<bool>();
            buildingQueueIndex = 0;
            rand = new Random((int)System.DateTime.Today.Ticks);
            this.world = world;
        }
        /// <summary>
        /// Updates AI
        /// </summary>
        /// <param name="cmdrs">List of SCRTSCommander objects</param>
        public void updateAI(List<SCRTSCommander> cmdrs)
        {
            generateUnitTasks(cmdrs);
            generateUpgradeOrders();
            generateUnitBuildOrders();
            generateBuildingBuildOrders();
            processTasks();
        }
        /// <summary>
        /// Generates tasks for units
        /// </summary>
        /// <param name="cmdrs">List of RTSCommander objects</param>
        public void generateUnitTasks(List<SCRTSCommander> cmdrs)
        {
            List<SCRTSUnit> tmp;
            for(int u = 0; u < cmdr.Units.Count; u++)
            {
                tmp = cmdr.scanAroundObject(((SCRTSUnit)cmdr.Units[u]), cmdrs);
                for(int t = 0; t < tmp.Count; t++)
                {
                    tasks.enqueue(new SCRTSTask(TASKS.T_ATTACK,
                        cmdr.Units[u].Center.X, cmdr.Units[u].Center.Y,
                        ((SCRTSUnit)cmdr.Units[u]), tmp[t]), 3);
                }
            }
            for(int u = 0; u < cmdr.Units.Count; u++)
            {
                if(((SCRTSUnit)cmdr.Units[u]).UT == UNITTYPES.AIRHARVESTER)
                {
                    if (cmdr.Units[u].Target == null)
                    {
                        tasks.enqueue(new SCRTSTask(TASKS.T_HARVEST,
                            cmdr.Units[u].Center.X, cmdr.Units[u].Center.Y,
                            ((SCRTSUnit)cmdr.Units[u]), null), 2);
                    }
                }
                if(((SCRTSUnit)cmdr.Units[u]).UT == UNITTYPES.GROUNDHARVESTER)
                {
                    if (cmdr.Units[u].Target == null)
                    {
                        tasks.enqueue(new SCRTSTask(TASKS.T_HARVEST,
                        cmdr.Units[u].Center.X, cmdr.Units[u].Center.Y,
                        ((SCRTSUnit)cmdr.Units[u]), null), 2);
                    }
                }
                if (cmdr.Units[u].HP < cmdr.Units[u].HP)
                {
                    tasks.enqueue(new SCRTSTask(TASKS.T_HEAL,
                        cmdr.Units[u].Center.X, cmdr.Units[u].Center.Y,
                        null, ((SCRTSUnit)cmdr.Units[u])), 2);
                }
            }
            for(int b = 0; b < cmdr.Buildings.Count; b++)
            {
                tmp = cmdr.scanAroundObject(((SCRTSBuilding)cmdr.Buildings[b]), cmdrs);
                for(int t = 0; t < tmp.Count; t++)
                {
                    tasks.enqueue(new SCRTSTask(TASKS.T_ATTACK,
                        cmdr.Buildings[b].Center.X, cmdr.Buildings[b].Center.Y,
                        ((SCRTSBuilding)cmdr.Buildings[b]), tmp[t]), 4);
                }
            }
            for(int b = 0; b < cmdr.Buildings.Count; b++)
            {
                if(((SCRTSBuilding)cmdr.Buildings[b]).BT == BUILDINGTYPES.FOUNDATION)
                {
                    tasks.enqueue(new SCRTSTask(TASKS.T_BUILD,
                        cmdr.Buildings[b].Center.X, cmdr.Buildings[b].Center.Y,
                        ((SCRTSBuilding)cmdr.Buildings[b]), null), 3);
                }
                if (cmdr.Buildings[b].HP < cmdr.Buildings[b].HP)
                {
                    tasks.enqueue(new SCRTSTask(TASKS.T_REPAIR,
                        cmdr.Buildings[b].Center.X, cmdr.Buildings[b].Center.Y,
                        null, ((SCRTSBuilding)cmdr.Buildings[b])), 2);
                }
            }
        }
        /// <summary>
        /// Generates upgrade tasks
        /// </summary>
        public void generateUpgradeOrders()
        {
            List<string> tmp = cmdr.getUnlockedUpgrades();
            for(int i = 0; i < tmp.Count; i++)
            {
                tasks.enqueue(new SCRTSTask(TASKS.T_ABILITY, 0, 0, null,
                                null, tmp[i]), 1);
            }
        }
        /// <summary>
        /// Generates unit build orders
        /// </summary>
        public void generateUnitBuildOrders()
        {
            if(unitBuildQueueNames.Count > 0)
            {
                for(int i = 0; i < unitBuildQueueNames.Count; i++)
                {
                    tasks.enqueue(new SCRTSTask(TASKS.T_TRAIN, 0, 0, null,
                            cmdr.UnitDB.getData(unitBuildQueueNames[i])), 1);
                }
            }
            else
            {
                int num = rand.Next(3, (cmdr.Buildings.Count / 2) * 3);
                SCRTSUnit tmp = null;
                for(int i = 0; i < num; i++)
                {
                    tmp = cmdr.UnitDB.getData(rand.Next(0, cmdr.UnitDB.Index - 1));
                    while(cmdr.notWorker(tmp.UnitName) == false)
                    {
                        tmp = cmdr.UnitDB.getData(rand.Next(0, cmdr.UnitDB.Index - 1));
                        tasks.enqueue(new SCRTSTask(TASKS.T_TRAIN, 0, 0, null,
                                    tmp), 1);
                    }
                }
                if(cmdr.countWorkers() < 3)
                {
                    for(int i = 0; i < cmdr.UnitDB.Index; i++)
                    {
                        if(cmdr.UnitDB.getData(i).UT == UNITTYPES.AIRWORKER)
                        {
                            tasks.enqueue(new SCRTSTask(TASKS.T_TRAIN, 0, 0, null,
                                    cmdr.UnitDB.getData(i)), 1);
                        }
                        else if(cmdr.UnitDB.getData(i).UT == UNITTYPES.GROUNDWORKER)
                        {
                            tasks.enqueue(new SCRTSTask(TASKS.T_TRAIN, 0, 0, null,
                                    cmdr.UnitDB.getData(i)), 1);
                        }
                    }
                }
                if(cmdr.countRefinories() > cmdr.countHarvesters())
                {
                    for(int i = 0; i < cmdr.UnitDB.Index; i++)
                    {
                        if(cmdr.UnitDB.getData(i).UT == UNITTYPES.AIRHARVESTER)
                        {
                            tasks.enqueue(new SCRTSTask(TASKS.T_TRAIN, 0, 0, null,
                                    cmdr.UnitDB.getData(i)), 1);
                        }
                        else if(cmdr.UnitDB.getData(i).UT == UNITTYPES.GROUNDHARVESTER)
                        {
                            tasks.enqueue(new SCRTSTask(TASKS.T_TRAIN, 0, 0, null,
                                    cmdr.UnitDB.getData(i)), 1);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Generates building build orders
        /// </summary>
        public void generateBuildingBuildOrders()
        {
            if(buildingBuildQueueNames.Count > 0)
            {
                for(int i = 0; i < buildingBuildQueueNames.Count; i++)
                {
                    if(buildingBuildQueueBools[i] == false)
                    {
                        tasks.enqueue(new SCRTSTask(TASKS.T_CONSTRUCT, 0, 0, null, 
                            cmdr.BuildingDB.getData(buildingBuildQueueNames[i])), 1);
                        break;
                        
                    }
                }
            }
            else
            {
                for(int i = 0; i < cmdr.BuildingDB.Index; i++)
                {
                    if(checkHasBuilding(cmdr.BuildingDB.getData(i).BuildingName) == false)
                    {
                        tasks.enqueue(new SCRTSTask(TASKS.T_CONSTRUCT, 0, 0, null,
                                cmdr.BuildingDB.getData(i)), 1);
                    }
                }
            }
        }
        /// <summary>
        /// Checks buildingBuildQueue and identifies 
        /// buildings not built in queue
        /// </summary>
        public void checkBuildingBuildQueue()
        {
            for(int i = 0; i < buildingBuildQueueNames.Count; i++)
            {
                for(int k = 0; k < cmdr.Buildings.Count; k++)
                {
                    if(((SCRTSBuilding)cmdr.Buildings[i]).BuildingName == 
                        buildingBuildQueueNames[i])
                    {
                        buildingBuildQueueBools[i] = true;
                    }
                    else
                    {
                        buildingBuildQueueBools[i] = false;
                    }
                }
            }
        }
        /// <summary>
        /// Checks if commander has specified building
        /// </summary>
        /// <param name="buildName">Name of building to check</param>
        /// <returns>Boolean</returns>
        public bool checkHasBuilding(string buildName)
        {
            if(cmdr.BuildingDB.getData(buildName).BT == 
                BUILDINGTYPES.TURRET)
            {
                if(cmdr.countTurrets() < (cmdr.Buildings.Count / 4))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            for(int i = 0; i < cmdr.Buildings.Count; i++)
            {
                if(((SCRTSBuilding)cmdr.Buildings[i]).BuildingName == buildName)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Identifies potential doers of a given task
        /// </summary>
        /// <param name="task">SCRTSTask reference</param>
        public void identifyDoers(SCRTSTask task)
        {
            doers.clear();
            SCRTSDoer doer = null;
            switch (task.Task)
            {
                case TASKS.T_ATTACK:
                    for(int i = 0; i < cmdr.Units.Count; i++)
                    {
                        if(cmdr.Units[i].Target == null)
                        {
                            if(((SCRTSUnit)cmdr.Units[i]).canAttack() == true)
                            {
                                doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSUnit)cmdr.Units[i]));
                                doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Units[i].Center));
                            }
                        }
                        else
                        {
                            if(cmdr.Units[i].Target is SCPatrolTarget)
                            {
                                if(((SCRTSUnit)cmdr.Units[i]).canAttack() == true)
                                {
                                    doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSUnit)cmdr.Units[i]));
                                    doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Units[i].Center));
                                }
                            }
                        }
                    }
                    for (int i = 0; i < cmdr.Buildings.Count; i++)
                    {
                        if(cmdr.Buildings[i].Target == null)
                        {
                            if(((SCRTSUnit)cmdr.Buildings[i]).canAttack() == true)
                            {
                                doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSBuilding)cmdr.Buildings[i]));
                                doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Buildings[i].Center));
                            }
                        }
                    }
                    break;
                case TASKS.T_BUILD:
                    for (int i = 0; i < cmdr.Units.Count; i++)
                    {
                        if (cmdr.Units[i].Target == null)
                        {
                            if (((SCRTSUnit)cmdr.Units[i]).canBuild() == true)
                            {
                                doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSUnit)cmdr.Units[i]));
                                doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Units[i].Center));
                            }
                        }
                    }
                    for (int i = 0; i < cmdr.Buildings.Count; i++)
                    {
                        if (cmdr.Buildings[i].Target == null)
                        {
                            if (((SCRTSUnit)cmdr.Buildings[i]).canBuild() == true)
                            {
                                doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSBuilding)cmdr.Buildings[i]));
                                doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Buildings[i].Center));
                            }
                        }
                    }
                    break;
                case TASKS.T_DEFEND:
                    for (int i = 0; i < cmdr.Units.Count; i++)
                    {
                        if (cmdr.Units[i].Target == null)
                        {
                            if (((SCRTSUnit)cmdr.Units[i]).canAttack() == true)
                            {
                                doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSUnit)cmdr.Units[i]));
                                doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Units[i].Center));
                            }
                        }
                        else
                        {
                            if (cmdr.Units[i].Target is SCPatrolTarget)
                            {
                                if (((SCRTSUnit)cmdr.Units[i]).canAttack() == true)
                                {
                                    doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSUnit)cmdr.Units[i]));
                                    doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Units[i].Center));
                                }
                            }
                        }
                    }
                    for (int i = 0; i < cmdr.Buildings.Count; i++)
                    {
                        if (cmdr.Buildings[i].Target == null)
                        {
                            if (((SCRTSUnit)cmdr.Buildings[i]).canAttack() == true)
                            {
                                doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSBuilding)cmdr.Buildings[i]));
                                doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Buildings[i].Center));
                            }
                        }
                    }
                    break;
                case TASKS.T_HARVEST:
                    for (int i = 0; i < cmdr.Units.Count; i++)
                    {
                        if (cmdr.Units[i].Target == null)
                        {
                            if (((SCRTSUnit)cmdr.Units[i]).canHarvest() == true)
                            {
                                doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSUnit)cmdr.Units[i]));
                                doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Units[i].Center));
                            }
                        }
                    }
                    break;
                case TASKS.T_HEAL:
                    for (int i = 0; i < cmdr.Units.Count; i++)
                    {
                        if (cmdr.Units[i].Target == null)
                        {
                            if (((SCRTSUnit)cmdr.Units[i]).canHeal() == true)
                            {
                                doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSUnit)cmdr.Units[i]));
                                doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Units[i].Center));
                            }
                        }
                        else
                        {
                            if (cmdr.Units[i].Target is SCPatrolTarget)
                            {
                                if (((SCRTSUnit)cmdr.Units[i]).canHeal() == true)
                                {
                                    doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSUnit)cmdr.Units[i]));
                                    doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Units[i].Center));
                                }
                            }
                        }
                    }
                    for (int i = 0; i < cmdr.Buildings.Count; i++)
                    {
                        if (cmdr.Buildings[i].Target == null)
                        {
                            if (((SCRTSUnit)cmdr.Buildings[i]).canHeal() == true)
                            {
                                doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSBuilding)cmdr.Buildings[i]));
                                doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Buildings[i].Center));
                            }
                        }
                    }
                    break;
                case TASKS.T_REFINE:
                    for (int i = 0; i < cmdr.Buildings.Count; i++)
                    {
                        if (cmdr.Buildings[i].Target == null)
                        {
                            if (((SCRTSBuilding)cmdr.Buildings[i]).BT == BUILDINGTYPES.REFINORY)
                            {
                                doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSBuilding)cmdr.Buildings[i]));
                                doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Buildings[i].Center));
                            }
                        }
                    }
                    break;
                case TASKS.T_REPAIR:
                    for (int i = 0; i < cmdr.Units.Count; i++)
                    {
                        if (cmdr.Units[i].Target == null)
                        {
                            if (((SCRTSUnit)cmdr.Units[i]).canRepair() == true)
                            {
                                doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSUnit)cmdr.Units[i]));
                                doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Units[i].Center));
                            }
                        }
                        else
                        {
                            if (cmdr.Units[i].Target is SCPatrolTarget)
                            {
                                if (((SCRTSUnit)cmdr.Units[i]).canRepair() == true)
                                {
                                    doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSUnit)cmdr.Units[i]));
                                    doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Units[i].Center));
                                }
                            }
                        }
                    }
                    for (int i = 0; i < cmdr.Buildings.Count; i++)
                    {
                        if (cmdr.Buildings[i].Target == null)
                        {
                            if (((SCRTSBuilding)cmdr.Buildings[i]).canRepair() == true)
                            {
                                doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSUnit)cmdr.Buildings[i]));
                                doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Buildings[i].Center));
                            }
                        }
                    }
                    break;
                case TASKS.T_SCOUT:
                    for (int i = 0; i < cmdr.Units.Count; i++)
                    {
                        if (cmdr.Units[i].Target == null)
                        {
                            if (((SCRTSUnit)cmdr.Units[i]).canAttack() == true)
                            {
                                doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSUnit)cmdr.Units[i]));
                                doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Units[i].Center));
                            }
                        }
                    }
                    break;
                case TASKS.T_ABILITY:
                    for (int i = 0; i < cmdr.Units.Count; i++)
                    {
                        if (cmdr.Units[i].Target == null)
                        {
                            if (((SCRTSUnit)cmdr.Units[i]).canUpgrade() == true)
                            {
                                doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSUnit)cmdr.Units[i]));
                                doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Units[i].Center));
                            }
                        }
                    }
                    for (int i = 0; i < cmdr.Buildings.Count; i++)
                    {
                        if (cmdr.Buildings[i].Target == null)
                        {
                            if (((SCRTSBuilding)cmdr.Buildings[i]).canUpgrade() == true)
                            {
                                doer = new SCRTSDoer(task.Task, task.Pos.X, task.Pos.Y, ((SCRTSBuilding)cmdr.Buildings[i]));
                                doers.enqueue(doer, (int)Point2D.AsqrtB(task.Pos, cmdr.Buildings[i].Center));
                            }
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Process all tasks
        /// </summary>
        public void processTasks()
        {
            SCRTSTask curr = null;
            SCRTSDoer doer = null;
            if(tasks.isEmpty() == false)
            {
                curr = tasks.dequeue();
                identifyDoers(curr);
                doer = doers.dequeue();
                switch(curr.Task)
                {
                    case TASKS.T_ATTACK:
                        if(doer.Doer.Target != null)
                        {
                            if(doer.Doer.Target is SCPatrolTarget)
                            {
                                doer.Doer.StoredTarget = doer.Doer.Target;
                                doer.Doer.Target = new SCAttackTarget(curr.Target, doer.Doer);
                            }
                        }
                        else
                        {
                            doer.Doer.Target = new SCAttackTarget(curr.Target, doer.Doer);
                        }
                        break;
                    case TASKS.T_BUILD:
                        if (doer.Doer.Target != null)
                        {
                            doer.Doer.Target = new SCBuildTarget((SCRTSBuilding)curr.Target, doer.Doer);
                        }
                        break;
                    case TASKS.T_DEFEND:
                        if (doer.Doer.Target != null)
                        {
                            if (doer.Doer.Target is SCPatrolTarget)
                            {
                                doer.Doer.StoredTarget = doer.Doer.Target;
                                doer.Doer.Target = new SCAttackTarget(curr.Target, doer.Doer);
                            }
                        }
                        else
                        {
                            doer.Doer.Target = new SCAttackTarget(curr.Target, doer.Doer);
                        }
                        break;
                    case TASKS.T_HARVEST:
                        if (doer.Doer.Target != null)
                        {
                            doer.Doer.Target = new SCHarvestTarget(curr.Pos, doer.Doer);
                        }
                        break;
                    case TASKS.T_HEAL:
                        if (doer.Doer.Target != null)
                        {
                            if (doer.Doer.Target is SCPatrolTarget)
                            {
                                doer.Doer.StoredTarget = doer.Doer.Target;
                                doer.Doer.Target = new SCHealTarget(curr.Target, doer.Doer);
                            }
                        }
                        else
                        {
                            doer.Doer.Target = new SCHealTarget(curr.Target, doer.Doer);
                        }
                        break;
                    case TASKS.T_REFINE:
                        if (doer.Doer.Target != null)
                        {
                            doer.Doer.Target = new SCRefineTarget((SCRTSBuilding)curr.Target, doer.Doer);
                        }
                        break;
                    case TASKS.T_REPAIR:
                        if (doer.Doer.Target != null)
                        {
                            if (doer.Doer.Target is SCPatrolTarget)
                            {
                                doer.Doer.StoredTarget = doer.Doer.Target;
                                doer.Doer.Target = new SCRepairTarget(curr.Target, doer.Doer);
                            }
                        }
                        else
                        {
                            doer.Doer.Target = new SCRepairTarget(curr.Target, doer.Doer);
                        }
                        break;
                    case TASKS.T_SCOUT:
                        if (doer.Doer.Target != null)
                        {
                            doer.Doer.Target = new SCMoveTarget(curr.Pos, doer.Doer);
                        }
                        break;
                    case TASKS.T_TRAIN:
                        if (doer.Doer.canTrain() == true)
                        {
                            doer.Doer.startTraining(curr.Act);
                        }
                        break;
                    case TASKS.T_UPGRADE:
                        if (doer.Doer.canTrain() == true)
                        {
                            doer.Doer.startResearching(curr.Act);
                        }
                        break;
                    case TASKS.T_CONSTRUCT:
                        processBuildOrder(curr);
                        break;
                }
            }
        }
        /// <summary>
        /// Process a build order
        /// </summary>
        /// <param name="task">RTSTask reference</param>
        public void processBuildOrder(SCRTSTask task)
        {
            if(cmdr.Buildings.Count > 0)
            {
                SCRTSBuilding bld = cmdr.BuildingDB.getData(task.Act);
                SCRTSBuilding newBld = null;
                int p = rand.Next(0, cmdr.Buildings.Count - 1);
                Point2D pos = new Point2D(cmdr.Buildings[p].X, cmdr.Buildings[p].Y);
                double ang = rand.Next(0, 360);
                float px = (((float)Math.Cos(ang)) * bld.W) + pos.X;
                float py = (((float)Math.Sin(ang)) * bld.W) + pos.Y;
                if(world.checkSpace(px, py, (int)bld.W / 32, (int)bld.H / 32) == true)
                {
                    newBld = new SCRTSFoundation(bld);
                    cmdr.Buildings.Add(newBld);
                }
            }
        }
        /// <summary>
        /// UnitBuildQueueNames property
        /// </summary>
        public List<string> UnitBuildQueueNames
        {
            get { return unitBuildQueueNames; }
        }
        /// <summary>
        /// BuildingBuildQueueNames property
        /// </summary>
        public List<string> BuildingBuildQueueNames
        {
            get { return buildingBuildQueueNames; }
        }
        /// <summary>
        /// BuildingBuildQueueBools property
        /// </summary>
        public List<bool> BuildingBuildQueueBools
        {
            get { return buildingBuildQueueBools; }
        }
    }
}
