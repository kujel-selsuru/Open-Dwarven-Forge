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
    public static class Shuffler<T>
    {
        public static void shuffleList(List<T> obj)
        {
            Random rand = new Random((int)System.DateTime.Today.Ticks);
            int num = obj.Count * 2;
            int i1 = 0;
            int i2 = 0;
            T ob = default(T);
            for (int n = 0; n < num; n++)
            {
                i1 = rand.Next(0, obj.Count - 1);
                i2 = rand.Next(0, obj.Count - 1);
                ob = obj[i1];
                obj[i1] = obj[i2];
                obj[i2] = ob;
            }
        }
    }
}
