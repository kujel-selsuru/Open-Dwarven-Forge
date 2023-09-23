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
    /// ListNode class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListNode<T>
    {
        //protected
	    protected int priority;
	    protected ListNode<T> parent;
	    protected ListNode<T> child;
	    protected T data;

        //public
        /// <summary>
        /// ListNode constructor
        /// </summary>
        /// <param name="data">Data reference of type T</param>
        /// <param name="priority">Priority value</param>
        /// <param name="parent">Parent node</param>
        /// <param name="child">Child node</param>
	    public ListNode(T data = default(T), int priority = 100, ListNode<T> parent = null, ListNode<T> child = null)
	    {
		    this.data = data;
		    this.priority = priority;
		    this.parent = parent;
		    this.child = child;
	    }
        /// <summary>
        /// Priority property
        /// </summary>
	    public int Priority
	    {
		    get {return priority;}
	    }
        /// <summary>
        /// Data property
        /// </summary>
	    public T Data
	    {
		    get {return data;}
	    }
        /// <summary>
        /// Parent property
        /// </summary>
	    public ListNode<T> Parent
	    {
		    get {return parent;}
		    set {parent = value;}
	    }
        /// <summary>
        /// Child property
        /// </summary>
	    public ListNode<T> Child
	    {
		    get {return child;}
		    set {child = value;}
	    }
    }
    /// <summary>
    /// PriorityList class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityList<T>
    {
        //protected
	    protected int count;
	    protected ListNode<T> root;

        //public
        /// <summary>
        /// PriorityList constructor
        /// </summary>
	    public PriorityList()
	    {
		    count = 0;
		    root = new ListNode<T>();
	    }
        /// <summary>
        /// Add a data item to list
        /// </summary>
        /// <param name="data">Data reference of type T</param>
        /// <param name="priority">Priority value, lower value higher priority</param>
	    public void add(T data, int priority = 100)
	    {
		    ListNode<T> temp = root;
		    ListNode<T> temp2;
		    for(int i = 0; i < count; i++)
		    {
			    if(temp.Child != null)
			    {
				    if(temp.Child.Priority <= priority)
				    {
					    temp2 = new ListNode<T>(data, priority, temp, temp.Child);
					    temp.Child.Parent = temp2;
					    temp.Child = temp2;
					    count++;
					    return;
				    }
			    }
			    else
			    {
				    temp2 = new ListNode<T>(data, priority, temp);
				    count++;
				    return;
			    }
			    temp = temp.Child; 
		    }
            if (count == 0)
            {
                temp2 = new ListNode<T>(data, priority, root, null);
                root.Child = temp2;
                count++;
            }
	    }
        /// <summary>
        /// Pop the top item off list and return it
        /// </summary>
        /// <returns>Object of type T</returns>
	    public T pop()
	    {
            if (count > 0)
            {
                T temp = root.Child.Data;
                root.Child.Parent = root;
                root.Child = root.Child.Child;
                count--;
                return temp;
            }
            return default(T);
	    }
        /// <summary>
        /// Return a List of all data references in PriorityList
        /// </summary>
        /// <returns>List of objects of tpye T</returns>
        public List<T> getAllData()
        {
            List<T> temp = new List<T>();
            if (count <= 0)
            {
                return temp;
            }
            ListNode<T> curr = root;
            for (int i = 0; i < count; i++)
            {          
                if (curr.Child != null)
                {
                    curr = curr.Child;
                }
                temp.Add(curr.Data);
            }
            return temp;
        }
        /// <summary>
        /// Clear all elements in list
        /// </summary>
	    public void clear()
	    {
		    root = new ListNode<T>();
		    count = 0;
	    }
        /// <summary>
        /// Count property
        /// </summary>
	    public int Count
	    {
		    get {return count;}
	    }
    }

}
