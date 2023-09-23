//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XenoLib
{
    /// <summary>
    /// PriorityQueue class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T>
    {
        /// <summary>
        /// Internal Node class
        /// </summary>
        public class Node
        {
            //private
            int priority;
            //Node prev;
            Node next;
            T data;
            long time;

            //public
            /// <summary>
            /// Node constructor
            /// </summary>
            /// <param name="t">Data reference</param>
            /// <param name="p">Priority value</param>
            /// <param name="time">Time value</param>
            /// <param name="next">Next Node reference</param>
            public Node(T t, int p, long time, Node next = null)
            {
                data = t;
                priority = p;
                //this.prev = prev;
                this.next = next;
                this.time = time;
            }
            /// <summary>
            /// Next property
            /// </summary>
            public Node Next
            {
                get { return next; }
                set { next = value; }
            }
            /*
            public Node Prev
            {
                get { return prev; }
                set { prev = value; }
            }
            */ 
            /// <summary>
            /// Priority property
            /// </summary>
            public int Priority
            {
                get { return priority; }
            }
            /// <summary>
            /// Data property
            /// </summary>
            public T Data
            {
                get { return data; }
            }
            /// <summary>
            /// Time property
            /// </summary>
            public long Time
            {
                get { return time; }
            }
        }
        //private
        Node start;
        Node end;
        Node itorater;
        int count;

        //public
        /// <summary>
        /// PriorityQueue constructor
        /// </summary>
        public PriorityQueue()
        {
            start = null;
            end = null;
            count = 0;
        }
        /// <summary>
        /// Checks if PriorityQueue is empty
        /// </summary>
        /// <returns></returns>
        public bool isEmpty()
        {
            if (start == null)
            {
                if (count > 0)
                {
                    count = 0;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Adds a new data object to PriorityQueue (higher value == lower priority)
        /// </summary>
        /// <param name="t">Data object reference</param>
        /// <param name="p">Priority value</param>
        public void enqueue(T t, int p)
        {
            count++;
            Node temp;

            //queue is empty so add first node
            if (start == null && end == null)
            {
                start = new Node(t, p, System.DateTime.Now.Ticks);
                itorater = start;
                start.Next = null;
                end = start;
            }//queue has one node in the list
            else if (start != null && start.Time == end.Time)
            {
                //new data is lower priority so add at end
                if (start.Priority < p)
                {
                    end = new Node(t, p, System.DateTime.Now.Ticks);
                    start.Next = end;
                    end.Next = null;
                }
                else //new data is higher priority so add at front
                {
                    temp = new Node(t, p, System.DateTime.Now.Ticks);
                    temp.Next = start;
                    start = temp;
                }
            }
            else
            {
                //queue has more then 1 node so find the appropriate place and add it.
                temp = start;
                Node node = new Node(t, p, System.DateTime.Now.Ticks);
                while (temp.Next != null && (p > temp.Priority && p != temp.Priority))
                {
                    temp = temp.Next;
                }
                if (temp.Next == null)//added to end of priority queue
                {
                    end = node;
                }
                node.Next = temp.Next;
                temp.Next = node;
            }
        }
        /// <summary>
        /// Returns next item in PriorityQueue
        /// </summary>
        /// <returns>T data object reference</returns>
        public T dequeue()
        {
            if(isEmpty())
            {
                return default(T);
            }
            count--;
            Node temp = start;
            start = start.Next;
            //only 1 node left in queue
            if (start == null)
            {
                end = null;
            }
            return temp.Data;
        }
        /// <summary>
        /// Itorates internal itorator one item at a time in list
        /// </summary>
        /// <returns>Boolean</returns>
        public bool itorate()
        {
            if (!isEmpty())
            {
                if (itorater.Next == null)
                {
                    return false;
                }
                else
                {
                    itorater = itorater.Next;
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Returns item at internal itorator
        /// </summary>
        /// <returns></returns>
        public T seeItorater()
        {
            if (!isEmpty())
            {
                return itorater.Data;
            }
            else
            {
                return default(T);
            }
        }
        /// <summary>
        /// Returns a reference to first item in PriorityQueue
        /// </summary>
        /// <returns></returns>
        public T peek()
        {
            if (!isEmpty())
            {
                return start.Data;
            }
            else
            {
                return default(T);
            }
        }
        /// <summary>
        /// Clears PriorityQueue
        /// </summary>
        public void clear()
        {
            while (count > 0)
            {
                if (start != null)
                {
                    Node temp = start;
                    start = start.Next;
                    count--;
                    if (start == null)
                    {
                        end = null;
                    }
                }
                else
                {
                    count = 0;
                }
            }
        }
        /// <summary>
        /// Returns a list of all items in PriorityQueue
        /// </summary>
        /// <returns></returns>
        public List<T> getAllData()
        {
            List<T> temp = new List<T>();
            if (isEmpty())
            {
                return temp;
            }
            Node curr = start;
            for (int i = 0; i < count; i++)
            {
                temp.Add(curr.Data);
                if (curr.Next != null)
                {
                    curr = curr.Next;
                }
            }
            return temp;
        }
        /// <summary>
        /// Resets internal itorator to front of PriorityQueue
        /// </summary>
        public void resetItorater()
        {
            itorater = start;
        }
        /// <summary>
        /// Count property
        /// </summary>
        public int Count
        {
            get { return count;}
        }
        /// <summary>
        /// Start/front property
        /// </summary>
        public Node Start
        {
            get { return start; }
        }
        /// <summary>
        /// End/last property
        /// </summary>
        public Node End
        {
            get { return end; }
        }
    }
}
