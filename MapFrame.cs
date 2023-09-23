//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

namespace XenoLib
{
    /// <summary>
    /// FrameNode class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FrameNode<T>
    {
        //protected
        protected List<FrameNode<T>> nodes;
        protected T data;
        protected bool remove;

        /// <summary>
        /// FrameNode constructor
        /// </summary>
        /// <param name="data">Data type T reference</param>
        public FrameNode(T data)
        {
            nodes = new List<FrameNode<T>>();
            this.data = data;
            remove = false;
        }
        /// <summary>
        /// Nodes property
        /// </summary>
        public List<FrameNode<T>> Nodes
        {
            get { return nodes; }
        }
        /// <summary>
        /// Data property
        /// </summary>
        public T Data
        {
            get { return data; }
            set { data = value; }
        }
        /// <summary>
        /// Remove property
        /// </summary>
        public bool Remove
        {
            get { return remove; }
            set { remove = value; }
        }
    }
    /// <summary>
    /// MapFrame class
    /// </summary>
    /// <typeparam name="T">Data type T reference</typeparam>
    public class MapFrame<T>
    {
        //protected
        protected List<FrameNode<T>> nodes;

        //public
        /// <summary>
        /// MapFrame constructor
        /// </summary>
        public MapFrame()
        {
            nodes = new List<FrameNode<T>>();
        }
        /// <summary>
        /// Add a new data point to list
        /// </summary>
        /// <param name="newData">New data reference</param>
        /// <param name="curr">Current node reference</param>
        public void addNode(T newData, FrameNode<T> curr = null)
        {
            FrameNode<T> temp = new FrameNode<T>(newData);
            if (curr == null)
            {
                nodes.Add(temp);
            }
            else
            {
                nodes.Add(temp);
                curr.Nodes.Add(temp);
                temp.Nodes.Add(curr);
            }
        }
        /// <summary>
        /// Remove a node from the list
        /// </summary>
        /// <param name="curr">Node to remove from the list</param>
        public void removeNode(FrameNode<T> curr)
        {
            if (curr != null)
            {
                if (curr.Remove == true)
                {
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        for (int k = 0; k < nodes[i].Nodes.Count; k++)
                        {
                            if(nodes[i].Nodes[k].Remove)
                            {
                                nodes[i].Nodes.RemoveAt(k);
                            }
                        }
                        if (nodes[i].Remove)
                        {
                            nodes.RemoveAt(i);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Incomplete function, never worked correctly
        /// </summary>
        /// <param name="Renderer">Renderer referecne</param>
        public void draw(IntPtr Renderer)
        {
            //assumes data contains loaction data
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int k = 0; k < nodes[i].Nodes.Count; k++)
                {
                    //commented out to compile for now
                    //Line.Draw(sb, pixel, nodes[i].Center, nodes[i].Nodes[k].Center, Color.LightGreen, 2);
                }
            }
        }
        /// <summary>
        /// Nodes property
        /// </summary>
        public List<FrameNode<T>> Nodes
        {
            get { return nodes; }
        }
    }
}
