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
    /// PossibleAssignment class
    /// </summary>
    public class PossibleAssignment
    {
        //protected
        protected int distance;
        protected int score;
        protected SimpleSprite possibleTaskDoer;
        protected Task task;

        //public
        /// <summary>
        /// PossibleAssginment constructor
        /// </summary>
        /// <param name="distance">Distence in pixels to task</param>
        /// <param name="score">Value of possibility of doing task</param>
        /// <param name="possibleTaskDoer">SimpleSprite reference</param>
        /// <param name="task">Task that needs doing</param>
        public PossibleAssignment(int distance, int score, SimpleSprite possibleTaskDoer, Task task)
        {
            this.distance = distance;
            this.score = score;
            this.possibleTaskDoer = possibleTaskDoer;
            this.task = task;
        }
        /// <summary>
        /// Distence property
        /// </summary>
        public int Distance
        {
            get { return distance; }
        }
        /// <summary>
        /// Score property
        /// </summary>
        public int Score
        {
            get { return score; }
        }
        /// <summary>
        /// TargetTask property
        /// </summary>
        public Task TargetTask
        {
            get { return task; }
        }
        /// <summary>
        /// PossibleTaskDoer property
        /// </summary>
        public SimpleSprite PossibleTaskDoer
        {
            get { return possibleTaskDoer; }
        }
    }

}
