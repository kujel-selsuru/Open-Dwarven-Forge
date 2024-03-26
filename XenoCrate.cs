//====================================================
//Written by Kujel Selsuru
//Last Updated 26/03/24
//====================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenoLib
{
    /// <summary>
    /// A storage class for games
    /// </summary>
    public class XenoCrate
    {
        //protected
        protected string name;
        protected GenericBank<XenoObject> objs;

        //public
        /// <summary>
        /// XenoCreate constructor
        /// </summary>
        /// <param name="name">XenoCrate name</param>
        public XenoCrate(string name = "Untitled Crate")
        {
            this.name = name;
            objs = new GenericBank<XenoObject>();
        }
        /// <summary>
        /// XenoCrate from file constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public XenoCrate(System.IO.StreamReader sr)
        {
            sr.ReadLine();
            objs = new GenericBank<XenoObject>();
            int num = Convert.ToInt32(sr.ReadLine());
            string buffer = "";
            XenoObject tmp = null;
            for(int i = 0; i < num; i++)
            {
                buffer = sr.ReadLine();
                tmp = new XenoObject(sr);
                objs.addData(buffer, tmp);
            }
        }
        /// <summary>
        /// XenoCrate copy constructor
        /// </summary>
        /// <param name="obj">XenoCrate reference</param>
        public XenoCrate(XenoCrate obj)
        {
            name = obj.Name;
            objs = new GenericBank<XenoObject>();
            for (int i = 0; i < obj.objs.Index; i++)
            {
                objs.addData(obj.objs.Names[i], obj.objs.getData(i));
            }
        }
        /// <summary>
        /// Saves XenoCrate data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======XenoCrate Data======");
            sw.WriteLine(objs.Index);
            for(int i = 0; i < objs.Index; i++)
            {
                sw.WriteLine(objs.Names[i]);
                objs.getData(i).saveData(sw);
            }
        }
        /// <summary>
        /// Adds a XenoObject object with a corripsonding key and returns true if successful 
        /// else returns false
        /// </summary>
        /// <param name="objName">Object key</param>
        /// <param name="obj">Object reference</param>
        /// <returns>Boolean</returns>
        public bool addObj(string objName, XenoObject obj)
        {
            if(objs.containsKey(objName) == true)
            {
                return false;
            }
            objs.addData(objName, obj);
            return true;
        }
        /// <summary>
        /// Returns a XenoSprite reference or null provided an object name
        /// </summary>
        /// <param name="objName">Object key</param>
        /// <returns>XenoSprite</returns>
        public XenoObject getObj(string objName)
        {
            XenoObject tmp = null;
            if(objs.containsKey(objName) == true)
            {
                tmp = objs.getData(objName);
            }
            return tmp;
        }
        /// <summary>
        /// Checks if an object key is present
        /// </summary>
        /// <param name="objName">Object key</param>
        /// <returns>Boolean</returns>
        public bool containsObj(string objName)
        {
            if(objs.containsKey(objName) == true)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Name property
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
