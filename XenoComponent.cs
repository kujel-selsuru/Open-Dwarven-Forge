using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenoLib
{
    /// <summary>
    /// XenoComponent types
    /// </summary>
    public enum COMPONENTS { INT = 0, DECIMAL, STRING, LIST};
    public class XenoComponent
    {
        //protected
        protected string name;
        protected COMPONENTS compType;
        protected List<XenoComponent> container;
        protected string val;
        //public
        /// <summary>
        /// XenoComponent constructor
        /// </summary>
        /// <param name="name">Component name</param>
        /// <param name="compType">COMPONENTS value</param>
        public XenoComponent(string name, COMPONENTS compType)
        {
            this.name = name;
            this.compType = compType;
            container = new List<XenoComponent>();
            val = "";
        }
        /// <summary>
        /// XenoComponent from file constructor
        /// </summary>
        /// <param name="sr">StreamReader reference</param>
        public XenoComponent(System.IO.StreamReader sr)
        {
            sr.ReadLine();
            name = sr.ReadLine();
            compType = (COMPONENTS)(Convert.ToInt32(sr.ReadLine()));
            int num = Convert.ToInt32(sr.ReadLine());
            container = new List<XenoComponent>();
            for(int i = 0; i < num - 1; i++)
            {
                container.Add(new XenoComponent(sr));
            }
            val = sr.ReadLine();
        }
        /// <summary>
        /// XenoComponent copy constructor
        /// </summary>
        /// <param name="obj">XenoComponent reference</param>
        public XenoComponent(XenoComponent obj)
        {
            name = obj.Name;
            container = new List<XenoComponent>();
            for(int i = 0; i < obj.Container.Count - 1; i++)
            {
                container.Add(obj.Container[i]);
            }
            val = obj.Val;
        }
        /// <summary>
        /// Saves XenoComponent data
        /// </summary>
        /// <param name="sw">StreamWriter reference</param>
        public void saveData(System.IO.StreamWriter sw)
        {
            sw.WriteLine("======XenoComponent Data======");
            sw.WriteLine(name);
            sw.WriteLine((int)compType);
            sw.WriteLine(container.Count);
            for(int i = 0; i < container.Count - 1; i++)
            {
                container[i].saveData(sw);
            }
            sw.WriteLine(val);
        }
        /// <summary>
        /// Name property
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// CompType property
        /// </summary>
        public COMPONENTS CompType
        {
            get { return compType; }
            set { compType = value; }
        }
        /// <summary>
        /// Container property
        /// </summary>
        public List<XenoComponent> Container
        {
            get { return container; }
        }
        /// <summary>
        /// Val property
        /// </summary>
        public string Val
        {
            get { return val; }
            set { val = value; }
        }
    }
}
