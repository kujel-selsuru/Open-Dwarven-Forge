//====================================================
//Written by Kujel Selsuru
//Last Updated 23/09/23
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace XenoLib
{
    /// <summary>
    /// SwitchNode class
    /// </summary>
    public class SwitchNode
    {
        //public
        public bool val;
        public string name;

        /// <summary>
        /// SwitchNode constructor
        /// </summary>
        /// <param name="name">SwitchNode name</param>
        /// <param name="val">Switch value</param>
        public SwitchNode(string name = "blank", bool val = false)
        {
            this.name = name;
            this.val = val;
        }
        /// <summary>
        /// Save data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void save(StreamWriter sw)
        {
            sw.WriteLine(val.ToString());
            sw.WriteLine(name);
        }
        /// <summary>
        /// Name property
        /// </summary>
        public string Name
        {
            get { return name; }
        }
        /// <summary>
        /// Val property
        /// </summary>
        public bool Val
        {
            get { return val; }
            set { val = value; }
        }
    }
    /// <summary>
    /// SwitchBoard class
    /// </summary>
    public class SwitchBoard
    {
        //protected
        protected List<String> keys;
        protected List<bool> switches;

        //public
        /// <summary>
        /// SwitchBoard from file constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public SwitchBoard(StreamReader sr = null)
        {
            keys = new List<string>();
            switches = new List<bool>();
            if (sr == null)
            {
                //do nothing else here
            }
            else
            {
                int temp = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < temp; i++)
                {
                    keys.Add(sr.ReadLine());
                    switches.Add(Convert.ToBoolean(sr.ReadLine()));
                }
            }
        }
        /// <summary>
        /// Creates and returns a copy of specified SwitchNode
        /// </summary>
        /// <param name="key">SwitchNode name/key</param>
        /// <param name="node">reference for copy of SwitchNode</param>
        public void seeSwitch(string key, out SwitchNode node)
        {
            int temp = checkKey(key);
            if(temp == -1)
            {
                //return false;
                node = new SwitchNode();
            }
            else
            {
                //return true;
                node = new SwitchNode(key, switches[temp]);
            }
        }
        /// <summary>
        /// Sets the value of a specified SwitchNode
        /// </summary>
        /// <param name="key">Name/key</param>
        /// <param name="newVal">Value to set to</param>
        /// <returns>Boolean</returns>
        public bool setSwitch(string key, bool newVal)
        {
            int temp = checkKey(key);
            if(temp == -1)
            {
                return false;
            }
            else
            {
                switches[temp] = newVal;
                return true;
            }
        }
        /// <summary>
        /// Adds a new SwtichNode to SwitchBoard
        /// </summary>
        /// <param name="key">Name/key</param>
        /// <param name="val">Initial state of new SwtichNode</param>
        /// <returns>Boolean</returns>
        public bool addSwitch(string key, bool val)
        {
            int temp = checkKey(key);
            if (temp == -1)
            {
                return false;
            }
            else
            {
                keys.Add(key);
                switches.Add(val);
                return true;
            }
        }
        /// <summary>
        /// Checks if a SwtichNode of provided key exists
        /// </summary>
        /// <param name="key">Name/key</param>
        /// <returns>Int</returns>
        public int checkKey(string key)
        {
            for(int i = 0; i < keys.Count; i++)
            {
                if (keys[i] == key)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// Saves data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveData(StreamWriter sw)
        {
            sw.WriteLine(keys.Count.ToString());
            for (int i = 0; i < keys.Count; i++)
            {
                sw.WriteLine(keys[i]);
                sw.WriteLine(switches[i].ToString());
            }
        }
        /// <summary>
        /// Returns state of swtich
        /// </summary>
        /// <param name="key">Name/key</param>
        /// <returns>Boolean</returns>
        public bool viewSwitch(string key)
        {
            if(keys.Contains(key))
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    if (keys[i] == key)
                    {
                        return switches[i];
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Keys property
        /// </summary>
        public List<String> Keys
        {
            get { return keys; }
        }
    }     
    /*
    public class SwitchBoard
    {
        protected Dictionary<string, SwitchNode> board;
        protected List<string> keys;
        public SwitchBoard()
        {
            board = new Dictionary<string, SwitchNode>();
            keys = new List<string>();
        }
        public SwitchBoard(StreamReader sr)
        {
            board = new Dictionary<string, SwitchNode>();
            keys = new List<string>();
            int tempNum = Convert.ToInt32(sr.ReadLine());
            string val1 = "";
            string val2 = "";
            for (int i = 0; i < tempNum; i++)
            {
                val1 = sr.ReadLine();
                val2 = sr.ReadLine();
                board.Add(val1, new SwitchNode(val1, Convert.ToBoolean(val2)));
                keys.Add(val1);
            }
        }
        public bool addSwitch(string name, SwitchNode node)
        {
            if (board.ContainsKey(name))
            {
                return false;
            }
            else
            {
                board.Add(name, node);
                keys.Add(name);
                return true;
            }
        }
        public bool seeSwitch(string name, out SwitchNode val)
        {
            if (board.ContainsKey(name))
            {
                board.TryGetValue(name, out val);
                return true;
            }
            else
            {
                val = default(SwitchNode);
                return false;
            }
        }
        public bool setSwitch(string name, bool newVal)
        {
            SwitchNode node;
            seeSwitch(name, out node);
            bool oldVal = node.val;
            node.val = newVal;
            if (oldVal = node.val)
            {
                return false;
            }
            else
            {
                return true;
            }
            /*
            board.Remove(name);
            SwitchNode node;
            node.name = name;
            node.val = newVal;
            board.Add(name, node);
            keys.Add(name);
            
            //board.TryGetValue(name, out val);
            //val.val = newVal;
             
            if (node.name == "blank")
            {
                return false;
            }
            else
            {
                return true;
            }
            */
    /*
        }
        public void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine(keys.Count.ToString());
            for (int i = 0; i < keys.Count; i++)
            {
                
                SwitchNode node;
                board.TryGetValue(keys[i], out node);
                sw.WriteLine(node.name);
                sw.WriteLine(node.val.ToString());
            }
        }
        public List<string> Keys
        {
            get { return keys; }
        }
    }
    */
}
