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
    public enum RTSOBJECTIVETYPE { ot_move = 0, ot_attack, ot_shoot, ot_repair, ot_heal, ot_build, ot_harvest, ot_refine, ot_buff, ot_debuff, ot_special, ot_flee, ot_train };
    public class RTSObjective
    {
        //protected
        protected Point2D position;
        protected RTSObject owner;
        protected RTSObject target;
        protected RTSOBJECTIVETYPE ot;
        protected int state;
        protected string name;
        //public
        /// <summary>
        /// RTS objective constructor 
        /// </summary>
        /// <param name="x">objective target x position</param>
        /// <param name="y">objective target y position</param>
        /// <param name="owner">instance to which this instance belongs</param>
        /// <param name="target">target object of this instance</param>
        /// <param name="ot">objective type of instance</param>
        public RTSObjective(int x, int y, RTSObject owner, RTSObject target, RTSOBJECTIVETYPE ot, string name = "")
        {
            state = 0;
            position = new Point2D(x, y);
            this.owner = owner;
            this.target = target;
            this.ot = ot;
            this.name = name;
        }
        /// <summary>
        /// RTS Objective copy constructor
        /// </summary>
        /// <param name="obj">instance to be copied</param>
        public RTSObjective(RTSObjective obj)
        {
            this.state = obj.State;
            this.position = new Point2D(obj.Position.X, obj.Position.Y);
            owner = obj.Owner;
            target = obj.Target;
            ot = obj.OT;
            name = obj.name;
        }
        /// <summary>
        /// Position property
        /// </summary>
        public Point2D Position
        {
            get { return position; }
            set { position = value; }
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
        /// Target property
        /// </summary>
        public RTSObject Target
        {
            get { return target; }
            set { target = value; }
        }
        /// <summary>
        /// Objective type property
        /// </summary>
        public RTSOBJECTIVETYPE OT
        {
            get { return ot; }
            set { ot = value; }
        }
        /// <summary>
        /// State property
        /// </summary>
        public int State
        {
            get { return state; }
            set { state = value; }
        }
        /// <summary>
        /// Name property
        /// </summary>
        public string Name
        {
            get { return name; }
        }
        /// <summary>
        /// Tests if objective has completed all its tasks
        /// </summary>
        /// <returns>returns true if done else false</returns>
        public bool isDone()
        {
            if (state == -1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// updates the internal state of the objective and performs actions upon the owning object
        /// </summary>
        /// <param name="owner">instance which this objective belongs to an operates on</param>
        public virtual void update()
        {
            //design pattern for later use
            switch (state)
            {
                case 0:

                    break;
                case 1:

                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

                    break;
                case -1:

                    break;
            }
        }

    }

    //All proceeding classes are basic forms which can be inherited to make more specialized variants

    public class RTSMoveToTarget : RTSObjective
    {
        //protected

        //public
        /// <summary>
        /// Move to target objective
        /// </summary>
        /// <param name="x">Objective x position</param>
        /// <param name="y">Objective y position</param>
        /// <param name="owner">Objective owner</param>
        /// <param name="target">Target of objective, set to null for moving</param>
        public RTSMoveToTarget(int x, int y, RTSObject owner, RTSObject target)
            : base(x, y, owner, target, RTSOBJECTIVETYPE.ot_move)
        {
            state = 0;
        }
        /// <summary>
        /// Move to target copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public RTSMoveToTarget(RTSMoveToTarget obj) : base(obj)
        {

        }
        /// <summary>
        /// Move to target update method
        /// </summary>
        public override void update()
        {
            switch (state)
            {
                case 0:
                    //caluculate path
                    owner.Route = owner.Owner.World.findPath(owner.Position, position);
                    state++;
                    break;
                case 1:
                    owner.fallowPath(owner.Owner.World.MG, owner.Speed);
                    if (owner.RouteIndex == 0 || owner.RouteIndex == owner.Route.Count)
                    {
                        state++;
                    }
                    break;
                case 2:
                    state = -1;
                    break;
                case 3:

                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

                    break;
                case -1:

                    break;
            }
        }
    }
    public class RTSAttackTarget : RTSObjective
    {
        //protected

        //public
        /// <summary>
        /// Attack target
        /// </summary>
        /// <param name="x">Target x position</param>
        /// <param name="y">Target y position</param>
        /// <param name="owner">Owner of objective</param>
        /// <param name="target">Target object to attack</param>
        public RTSAttackTarget(int x, int y, RTSObject owner, RTSObject target)
            : base(x, y, owner, target, RTSOBJECTIVETYPE.ot_attack)
        {
            state = 0;
        }
        /// <summary>
        /// Attack target copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public RTSAttackTarget(RTSAttackTarget obj)
            : base(obj)
        {

        }
        /// <summary>
        /// Attack target update method 
        /// </summary>
        public override void update()
        {
            switch (state)
            {
                case 0:
                    //caluculate path
                    owner.Route = owner.Owner.World.findPath(owner.Position, position);
                    state++;
                    break;
                case 1:
                    owner.fallowPath(owner.Owner.World.MG, owner.Speed);
                    if (Point2D.calculateDistance(target.Position, owner.Position) <= owner.Sight)
                    {
                        state++;
                    }
                    else if (owner.RouteIndex == 0 || owner.RouteIndex == owner.Route.Count)
                    {
                        state++;
                    }
                    break;
                case 2:
                    if (target.HP > 0)
                    {
                        owner.callAttack(target);
                    }
                    else if (Point2D.calculateDistance(target.Position, owner.Position) > owner.Sight)
                    {
                        state = 0;
                    }
                    else
                    {
                        state++;
                    }
                    break;
                case 3:
                    state = -1;
                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

                    break;
                case -1:

                    break;
            }
        }
    }
    public class AirstrikeTarget : RTSObjective
    {
        //protected
        protected bool returning;
        //public
        /// <summary>
        /// Airstrike target
        /// </summary>
        /// <param name="x">Target x position</param>
        /// <param name="y">Target y position</param>
        /// <param name="owner">Owner of objective</param>
        /// <param name="target">Target object to attack</param>
        public AirstrikeTarget(int x, int y, RTSObject owner, RTSObject target)
            : base(x, y, owner, target, RTSOBJECTIVETYPE.ot_attack)
        {
            state = 0;
            returning = false;
        }
        /// <summary>
        /// Airstrike target copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public AirstrikeTarget(AirstrikeTarget obj)
            : base(obj)
        {
            returning = false;
        }
        /// <summary>
        /// Airstrike target update method 
        /// </summary>
        public override void update()
        {
            switch (state)
            {
                case 0:
                    //caluculate path
                    owner.Route = owner.Owner.World.findPath(owner.Position, position);
                    returning = false;
                    state++;
                    break;
                case 1:
                    owner.fallowPath(owner.Owner.World.MG, owner.Speed);
                    if (!returning)
                    {
                        if (Point2D.calculateDistance(target.Position, owner.Position) <= owner.Sight)
                        {
                            state++;
                        }
                        else if (owner.RouteIndex == 0 || owner.RouteIndex == owner.Route.Count)
                        {
                            state++;
                        }
                    }
                    else
                    {
                        if (Point2D.calculateDistance(((RTSUnit)owner).Host.Center, owner.Position) <= owner.Sight)
                        {
                            state++;
                        }
                        else if (owner.RouteIndex == 0 || owner.RouteIndex == owner.Route.Count)
                        {
                            state++;
                        }
                    }
                    break;
                case 2:
                    if (!returning)
                    {
                        if (target.HP > 0)
                        {
                            owner.callAttack(target);
                            state = 4;
                        }
                        else if (Point2D.calculateDistance(target.Position, owner.Position) > owner.Sight)
                        {
                            state = 0;
                            returning = false;
                        }
                        else
                        {
                            state++;
                        }
                    }
                    else
                    {
                        state = 5;
                    }
                    break;
                case 3:
                    state = -1;
                    break;
                case 4:
                    //special case for returnig to host unit
                    owner.Route = owner.Owner.World.findPath(owner.Position, ((RTSUnit)owner).Host.Center);
                    returning = true;
                    state = 1;
                    break;
                case 5:
                    //special case for fallowing host unit
                    if (Point2D.calculateDistance(((RTSUnit)owner).Host.Center, owner.Position) <= 32)
                    {
                        returning = true;
                        state = 1;
                    }
                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

                    break;
                case -1:

                    break;
            }
        }
    }
    public class ShootTarget : RTSObjective
    {
        //protected

        //public
        /// <summary>
        /// Shoot target
        /// </summary>
        /// <param name="x">Target x position</param>
        /// <param name="y">Target y position</param>
        /// <param name="owner">Owner of objective</param>
        /// <param name="target">Target object to attack</param>
        public ShootTarget(int x, int y, RTSObject owner, RTSObject target)
            : base(x, y, owner, target, RTSOBJECTIVETYPE.ot_attack)
        {
            state = 0;
        }
        /// <summary>
        /// Shoot target copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public ShootTarget(ShootTarget obj)
            : base(obj)
        {

        }
        /// <summary>
        /// Shoot target update method 
        /// </summary>
        public override void update()
        {
            switch (state)
            {
                case 0:
                    //caluculate path
                    owner.Route = owner.Owner.World.findPath(owner.Position, position);
                    state++;
                    break;
                case 1:
                    owner.fallowPath(owner.Owner.World.MG, owner.Speed);
                    if (Point2D.calculateDistance(target.Position, Owner.Position) <= owner.Sight)
                    {
                        state++;
                    }
                    else if (owner.RouteIndex == 0 || owner.RouteIndex == owner.Route.Count)
                    {
                        state++;
                    }
                    break;
                case 2:
                    if (target.HP > 0)
                    {
                        owner.callAbility("any");
                    }
                    else if (Point2D.calculateDistance(target.Position, owner.Position) > owner.Sight)
                    {
                        state = 0;
                    }
                    else
                    {
                        state++;
                    }
                    break;
                case 3:
                    state = -1;
                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

                    break;
                case -1:

                    break;
            }
        }
    }
    public class RepairTarget : RTSObjective
    {
        //protected

        //public
        /// <summary>
        /// Repair target
        /// </summary>
        /// <param name="x">Target x position</param>
        /// <param name="y">Target y position</param>
        /// <param name="owner">Owner of objective</param>
        /// <param name="target">Target object to attack</param>
        public RepairTarget(int x, int y, RTSObject owner, RTSObject target)
            : base(x, y, owner, target, RTSOBJECTIVETYPE.ot_repair)
        {
            state = 0;
        }
        /// <summary>
        /// Repair target copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public RepairTarget(RepairTarget obj)
            : base(obj)
        {

        }
        /// <summary>
        /// Repair target update method 
        /// </summary>
        public override void update()
        {
            switch (state)
            {
                case 0:
                    //caluculate path
                    owner.Route = owner.Owner.World.findPath(owner.Position, position);
                    state++;
                    break;
                case 1:
                    owner.fallowPath(owner.Owner.World.MG, owner.Speed);
                    if (Point2D.calculateDistance(target.Position, Owner.Position) <= owner.Sight)
                    {
                        state++;
                    }
                    else if (owner.RouteIndex == 0 || owner.RouteIndex == owner.Route.Count)
                    {
                        state++;
                    }
                    break;
                case 2:
                    if (target.HP < target.MaxHP)
                    {
                        string temp = owner.getAbilityOfType(RTSACTIONTYPE.at_repair);
                        owner.callAbility(temp);
                    }
                    else if (Point2D.calculateDistance(target.Position, owner.Position) > owner.Sight)
                    {
                        state = 0;
                    }
                    else
                    {
                        state++;
                    }
                    break;
                case 3:
                    state = -1;
                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

                    break;
                case -1:

                    break;
            }
        }
    }
    public class HealTarget : RTSObjective
    {
        //protected

        //public
        /// <summary>
        /// Heal target
        /// </summary>
        /// <param name="x">Target x position</param>
        /// <param name="y">Target y position</param>
        /// <param name="owner">Owner of objective</param>
        /// <param name="target">Target object to attack</param>
        public HealTarget(int x, int y, RTSObject owner, RTSObject target)
            : base(x, y, owner, target, RTSOBJECTIVETYPE.ot_heal)
        {
            state = 0;
        }
        /// <summary>
        /// Heal target copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public HealTarget(HealTarget obj)
            : base(obj)
        {

        }
        /// <summary>
        /// Heal target update method 
        /// </summary>
        public override void update()
        {
            switch (state)
            {
                case 0:
                    //caluculate path
                    owner.Route = owner.Owner.World.findPath(owner.Position, position);
                    state++;
                    break;
                case 1:
                    owner.fallowPath(owner.Owner.World.MG, owner.Speed);
                    if (Point2D.calculateDistance(target.Position, Owner.Position) <= owner.Sight)
                    {
                        state++;
                    }
                    else if (owner.RouteIndex == 0 || owner.RouteIndex == owner.Route.Count)
                    {
                        state++;
                    }
                    break;
                case 2:
                    if (target.HP < target.MaxHP)
                    {
                        string temp = owner.getAbilityOfType(RTSACTIONTYPE.at_heal);
                        owner.callAbility(temp);
                    }
                    else if (Point2D.calculateDistance(target.Position, owner.Position) > owner.Sight)
                    {
                        state = 0;
                    }
                    else
                    {
                        state++;
                    }
                    break;
                case 3:
                    state = -1;
                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

                    break;
                case -1:

                    break;
            }
        }
    }
    public class BuildTarget : RTSObjective
    {
        //protected

        //public
        /// <summary>
        /// Build target
        /// </summary>
        /// <param name="x">Target x position</param>
        /// <param name="y">Target y position</param>
        /// <param name="owner">Owner of objective</param>
        /// <param name="target">Target object to attack</param>
        public BuildTarget(int x, int y, RTSObject owner, RTSObject target)
            : base(x, y, owner, target, RTSOBJECTIVETYPE.ot_build)
        {
            state = 0;
        }
        /// <summary>
        /// Build target copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public BuildTarget(BuildTarget obj)
            : base(obj)
        {

        }
        /// <summary>
        /// Build target update method 
        /// </summary>
        public override void update()
        {
            switch (state)
            {
                case 0:
                    //caluculate path
                    owner.Route = owner.Owner.World.findPath(owner.Position, position);
                    state++;
                    break;
                case 1:
                    owner.fallowPath(owner.Owner.World.MG, owner.Speed);
                    if (Point2D.calculateDistance(target.Position, Owner.Position) <= owner.Sight)
                    {
                        state++;
                    }
                    else if (owner.RouteIndex == 0 || owner.RouteIndex == owner.Route.Count)
                    {
                        state++;
                    }
                    break;
                case 2:
                    if (target.HP < target.MaxHP)
                    {
                        string temp = owner.getAbilityOfType(RTSACTIONTYPE.at_build);
                        owner.callAbility(temp);
                    }
                    else if (Point2D.calculateDistance(target.Position, owner.Position) > owner.Sight)
                    {
                        state = 0;
                    }
                    else
                    {
                        state++;
                    }
                    break;
                case 3:
                    state = -1;
                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

                    break;
                case -1:

                    break;
            }
        }
    }
    public class BuffTarget : RTSObjective
    {
        //protected

        //public
        /// <summary>
        /// Buff target
        /// </summary>
        /// <param name="x">Target x position</param>
        /// <param name="y">Target y position</param>
        /// <param name="owner">Owner of objective</param>
        /// <param name="target">Target object to attack</param>
        public BuffTarget(int x, int y, RTSObject owner, RTSObject target)
            : base(x, y, owner, target, RTSOBJECTIVETYPE.ot_buff)
        {
            state = 0;
        }
        /// <summary>
        /// Buff target copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public BuffTarget(BuffTarget obj)
            : base(obj)
        {

        }
        /// <summary>
        /// Buff target update method 
        /// </summary>
        public override void update()
        {
            switch (state)
            {
                case 0:
                    //caluculate path
                    owner.Route = owner.Owner.World.findPath(owner.Position, position);
                    state++;
                    break;
                case 1:
                    owner.fallowPath(owner.Owner.World.MG, owner.Speed);
                    if (Point2D.calculateDistance(target.Position, Owner.Position) <= owner.Sight)
                    {
                        state++;
                    }
                    else if (owner.RouteIndex == 0 || owner.RouteIndex == owner.Route.Count)
                    {
                        state++;
                    }
                    break;
                case 2:
                    if (target.HP > 0)
                    {
                        string temp = owner.getAbilityOfType(RTSACTIONTYPE.at_buff);
                        owner.callAbility(temp);
                        state++;
                    }
                    else if (Point2D.calculateDistance(target.Position, owner.Position) > owner.Sight)
                    {
                        state = 0;
                    }
                    else
                    {
                        state++;
                    }
                    break;
                case 3:
                    state = -1;
                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

                    break;
                case -1:

                    break;
            }
        }
    }
    public class DebuffTarget : RTSObjective
    {
        //protected

        //public
        /// <summary>
        /// Debuff target
        /// </summary>
        /// <param name="x">Target x position</param>
        /// <param name="y">Target y position</param>
        /// <param name="owner">Owner of objective</param>
        /// <param name="target">Target object to attack</param>
        public DebuffTarget(int x, int y, RTSObject owner, RTSObject target)
            : base(x, y, owner, target, RTSOBJECTIVETYPE.ot_debuff)
        {
            state = 0;
        }
        /// <summary>
        /// Debuff target copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public DebuffTarget(DebuffTarget obj)
            : base(obj)
        {

        }
        /// <summary>
        /// Debuff target update method 
        /// </summary>
        public override void update()
        {
            switch (state)
            {
                case 0:
                    //caluculate path
                    owner.Route = owner.Owner.World.findPath(owner.Position, position);
                    state++;
                    break;
                case 1:
                    owner.fallowPath(owner.Owner.World.MG, owner.Speed);
                    if (Point2D.calculateDistance(target.Position, Owner.Position) <= owner.Sight)
                    {
                        state++;
                    }
                    else if (owner.RouteIndex == 0 || owner.RouteIndex == owner.Route.Count)
                    {
                        state++;
                    }
                    break;
                case 2:
                    if (target.HP > 0)
                    {
                        string temp = owner.getAbilityOfType(RTSACTIONTYPE.at_buff);
                        owner.callAbility(temp);
                        state++;
                    }
                    else if (Point2D.calculateDistance(target.Position, owner.Position) > owner.Sight)
                    {
                        state = 0;
                    }
                    else
                    {
                        state++;
                    }
                    break;
                case 3:
                    state = -1;
                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

                    break;
                case -1:

                    break;
            }
        }
    }
    public class FleeToTarget : RTSObjective
    {
        //protected

        //public
        /// <summary>
        /// Flee to target objective
        /// </summary>
        /// <param name="x">Objective x position</param>
        /// <param name="y">Objective y position</param>
        /// <param name="owner">Objective owner</param>
        /// <param name="target">Target of objective, set to null for moving</param>
        public FleeToTarget(int x, int y, RTSObject owner, RTSObject target)
            : base(x, y, owner, target, RTSOBJECTIVETYPE.ot_flee)
        {
            state = 0;
        }
        /// <summary>
        /// Flee to target copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public FleeToTarget(FleeToTarget obj)
            : base(obj)
        {

        }
        /// <summary>
        /// Flee to target update method
        /// </summary>
        public override void update()
        {
            switch (state)
            {
                case 0:
                    //caluculate path
                    owner.Route = owner.Owner.World.findPath(owner.Position, position);
                    state++;
                    break;
                case 1:
                    owner.fallowPath(owner.Owner.World.MG, owner.Speed);
                    if (owner.RouteIndex == 0 || owner.RouteIndex == owner.Route.Count)
                    {
                        state++;
                    }
                    break;
                case 2:
                    state = -1;
                    break;
                case 3:

                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

                    break;
                case -1:

                    break;
            }
        }
    }
    public class HarvestTarget : RTSObjective
    {
        //protected

        //public
        /// <summary>
        /// Harvest target
        /// </summary>
        /// <param name="x">Target x position</param>
        /// <param name="y">Target y position</param>
        /// <param name="owner">Owner of objective</param>
        /// <param name="target">Target object to attack</param>
        public HarvestTarget(int x, int y, RTSObject owner, RTSObject target)
            : base(x, y, owner, target, RTSOBJECTIVETYPE.ot_harvest)
        {
            state = 0;
        }
        /// <summary>
        /// Harvest target copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public HarvestTarget(HarvestTarget obj)
            : base(obj)
        {

        }
        /// <summary>
        /// Harvest target update method 
        /// </summary>
        public override void update()
        {
            switch (state)
            {
                case 0:
                    //caluculate path
                    owner.Route = owner.Owner.World.findPath(owner.Position, position);
                    state++;
                    break;
                case 1:
                    owner.fallowPath(owner.Owner.World.MG, owner.Speed);
                    if (Point2D.calculateDistance(position, owner.Position) <= owner.Sight)
                    {
                        state++;
                    }
                    else if (owner.RouteIndex == 0 || owner.RouteIndex == owner.Route.Count)
                    {
                        state++;
                    }
                    break;
                case 2:
                    if (((RTSUnit)owner).Tank < ((RTSUnit)owner).TankSize)
                    {
                        string temp = owner.getAbilityOfType(RTSACTIONTYPE.at_harvest);
                        owner.callAbility(temp);
                    }
                    else if (Point2D.calculateDistance(position, owner.Position) > owner.Sight)
                    {
                        state = 0;
                    }
                    else
                    {
                        state++;
                    }
                    break;
                case 3:
                    state = -1;
                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

                    break;
                case -1:

                    break;
            }
        }
    }
    public class RefineTarget : RTSObjective
    {
        //protected

        //public
        /// <summary>
        /// Refine target
        /// </summary>
        /// <param name="x">Target x position</param>
        /// <param name="y">Target y position</param>
        /// <param name="owner">Owner of objective</param>
        /// <param name="target">Target object to attack</param>
        public RefineTarget(int x, int y, RTSObject owner, RTSObject target)
            : base(x, y, owner, target, RTSOBJECTIVETYPE.ot_refine)
        {
            state = 0;
        }
        /// <summary>
        /// Refine target copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public RefineTarget(RefineTarget obj)
            : base(obj)
        {

        }
        /// <summary>
        /// Refine target update method 
        /// </summary>
        public override void update()
        {
            switch (state)
            {
                case 0:
                    //caluculate path
                    owner.Route = owner.Owner.World.findPath(owner.Position, position);
                    state++;
                    break;
                case 1:
                    owner.fallowPath(owner.Owner.World.MG, owner.Speed);
                    if (Point2D.calculateDistance(target.Position, owner.Position) <= owner.Sight)
                    {
                        state++;
                    }
                    else if (owner.RouteIndex == 0 || owner.RouteIndex == owner.Route.Count)
                    {
                        state++;
                    }
                    break;
                case 2:
                    if (((RTSUnit)owner).Tank > 0)
                    {
                        string temp = owner.getAbilityOfType(RTSACTIONTYPE.at_refine);
                        owner.callAbility(temp);
                    }
                    else if (Point2D.calculateDistance(target.Position, owner.Position) > owner.Sight)
                    {
                        state = 0;
                    }
                    else
                    {
                        state++;
                    }
                    break;
                case 3:
                    state = -1;
                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

                    break;
                case -1:

                    break;
            }
        }
    }
    public class SpecialTarget : RTSObjective
    {
        //protected

        //public
        /// <summary>
        /// Special target
        /// </summary>
        /// <param name="x">Target x position</param>
        /// <param name="y">Target y position</param>
        /// <param name="owner">Owner of objective</param>
        /// <param name="target">Target object to attack</param>
        public SpecialTarget(int x, int y, RTSObject owner, RTSObject target)
            : base(x, y, owner, target, RTSOBJECTIVETYPE.ot_special)
        {
            state = 0;
        }
        /// <summary>
        /// Attack target copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public SpecialTarget(SpecialTarget obj)
            : base(obj)
        {

        }
        /// <summary>
        /// Special target update method 
        /// </summary>
        public override void update()
        {
            switch (state)
            {
                case 0:
                    //caluculate path
                    owner.Route = owner.Owner.World.findPath(owner.Position, position);
                    state++;
                    break;
                case 1:
                    owner.fallowPath(owner.Owner.World.MG, owner.Speed);
                    if (Point2D.calculateDistance(position, owner.Position) <= owner.Sight)
                    {
                        state++;
                    }
                    else if (owner.RouteIndex == 0 || owner.RouteIndex == owner.Route.Count)
                    {
                        state++;
                    }
                    break;
                case 2:
                    if (target.HP > 0)
                    {
                        string temp = owner.getAbilityOfType(RTSACTIONTYPE.at_special);
                        owner.callAbility(temp);
                    }
                    else if (Point2D.calculateDistance(target.Position, owner.Position) > owner.Sight)
                    {
                        state = 0;
                    }
                    else
                    {
                        state++;
                    }
                    break;
                case 3:
                    state = -1;
                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

                    break;
                case -1:

                    break;
            }
        }
    }
    public class BuildingAttack : RTSObjective
    {
        //protected

        //public
        /// <summary>
        /// Building Attack
        /// </summary>
        /// <param name="x">Target x position</param>
        /// <param name="y">Target y position</param>
        /// <param name="owner">Owner of objective</param>
        /// <param name="target">Target object to attack</param>
        public BuildingAttack(int x, int y, RTSObject owner, RTSObject target)
            : base(x, y, owner, target, RTSOBJECTIVETYPE.ot_attack)
        {
            state = 0;
        }
        /// <summary>
        /// Building Attack copy constructor
        /// </summary>
        /// <param name="obj"></param>
        public BuildingAttack(BuildingAttack obj)
            : base(obj)
        {

        }
        /// <summary>
        /// Building Attack update method 
        /// </summary>
        public override void update()
        {
            switch (state)
            {
                case 0:
                    if (target.HP > 0)
                    {
                        owner.callAttack(target);
                    }
                    else if (Point2D.calculateDistance(target.Position, owner.Position) > owner.Sight)
                    {
                        state++;
                    }
                    else
                    {
                        state++;
                    }
                    break;
                case 1:
                    state = -1;
                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

                    break;
                case -1:

                    break;
            }
        }
    }
}
