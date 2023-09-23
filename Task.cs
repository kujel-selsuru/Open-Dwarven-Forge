//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;

namespace XenoLib
{
    /// <summary>
    /// Task class
    /// </summary>
    public class Task
    {
        //protected
        protected int priority;
        protected int priorityModifier;
        protected Objective objective;
        protected SimpleSprite taskDoer;
        protected bool assign;

        //public
        /// <summary>
        /// Task constructor
        /// </summary>
        /// <param name="priority">Priority value</param>
        /// <param name="priorityModifier">Priority modifier value</param>
        /// <param name="objective">Objective reference</param>
        /// <param name="taskDoer">SimpleSprite reference</param>
        public Task(int priority, int priorityModifier, Objective objective, SimpleSprite taskDoer)
	    {
		    this.priority = priority;
		    this. priorityModifier = priorityModifier;
		    this.objective = objective;
            this.taskDoer = taskDoer;
	    }
        /// <summary>
        /// Virtual function for assigning a doer of this task
        /// </summary>
        /// <param name="taskDoer">SimpleSprite reference</param>
        public virtual void assignTask(SimpleSprite taskDoer)
        {

        }
        /// <summary>
        /// Priority property
        /// </summary>
        public int Priority
        {
            get { return priority; }
        }
        /// <summary>
        /// PriorityModifier property
        /// </summary>
        public int PriorityModifier
        {
            get { return priorityModifier; }
        }
        /// <summary>
        /// Objective property
        /// </summary>
        public Objective TargetObjective
        {
            get { return objective; }
        }
        /// <summary>
        /// TaskDoer property
        /// </summary>
        public SimpleSprite TaskDoer
        {
            get { return taskDoer; }
        }
        /// <summary>
        /// Assigned property
        /// </summary>
        public bool Assigned
        {
            get { return assign; }
            set { assign = value; }
        }
    }

}
