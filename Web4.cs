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
    /// Direction4 enumeration
    /// </summary>
    public enum direction4 { up = 0, right, down, left}

    /// <summary>
    /// Web4 class
    /// </summary>
    public class Web4
    {
        //protected
        protected Web4 up;
        protected Web4 right;
        protected Web4 down;
        protected Web4 left;

        /// <summary>
        /// Web4 constructor
        /// </summary>
        public Web4()
        {
            up = null;
            right = null;
            down = null;
            left = null;
        }
        /// <summary>
        /// Link a Web4 node with another Web4 node
        /// </summary>
        /// <param name="node">Web4 object</param>
        /// <param name="direct">Direction of connection</param>
        public void connect(Web4 node, direction4 direct)
        {
            switch (direct)
            {
                case direction4.up:
                    up = node;
                    node.Down = this;
                    break;
                case direction4.right:
                    right = node;
                    node.Left = this;
                    break;
                case direction4.down:
                    down = node;
                    node.Up = this;
                    break;
                case direction4.left:
                    left = node;
                    node.Right = this;
                    break;
            }
        }
        /// <summary>
        /// Up property
        /// </summary>
        public Web4 Up
        {
            get { return up; }
            set { up = value; }
        }
        /// <summary>
        /// Right property
        /// </summary>
        public Web4 Right
        {
            get { return right; }
            set { right = value; }
        }
        /// <summary>
        /// Down property
        /// </summary>
        public Web4 Down
        {
            get { return down; }
            set { down = value; }
        }
        /// <summary>
        /// Left property
        /// </summary>
        public Web4 Left
        {
            get { return left; }
            set { left = value; }
        }
    }
}
