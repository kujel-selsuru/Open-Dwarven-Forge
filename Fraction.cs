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
    /// Fraction class
    /// </summary>
    public class Fraction
    {
        //protected
        protected int num;
        protected int dem;

        //public
        /// <summary>
        /// Fraction constructor
        /// </summary>
        /// <param name="num">Numorator</param>
        /// <param name="dem">Denominator</param>
        public Fraction(int num, int dem)
        {
            this.num = num;
            this.dem = dem;
        }
        /// <summary>
        /// Reduces fraction to lowest form
        /// </summary>
        public void reduce()
        {
            int t = 2;
            while (t < dem)
            {
                if (dem % t == 0)
                {
                    if (num % t == 0)
                    {
                        num /= t;
                        dem /= t;
                        t = 2;
                    }
                }
                t++;
            }
        }
        /// <summary>
        /// Add another Fraction to this instance
        /// </summary>
        /// <param name="other"></param>
        public void add(Fraction other)
        {
            int oNum = other.Num;
            int oDem = other.Dem;
            if (dem != oDem)
            {
                oNum *= dem;
                oDem *= num;
                num *= other.Dem;
                dem *= other.Num;
            }
            num += oNum;
        }
        /// <summary>
        /// Subtract another Fraction from this instance
        /// </summary>
        /// <param name="other"></param>
        public void subtract(Fraction other)
        {
            int oNum = other.Num;
            int oDem = other.Dem;
            if (dem != oDem)
            {
                oNum *= dem;
                oDem *= num;
                num *= other.Dem;
                dem *= other.Num;
            }
            num -= oNum;
        }
        /// <summary>
        /// Multiply this instance by another Fraction
        /// </summary>
        /// <param name="other"></param>
        public void multiply(Fraction other)
        {
            num *= other.Num;
            dem *= other.Dem;
        }
        /// <summary>
        /// Divide this instance by another Fraction
        /// </summary>
        /// <param name="other"></param>
        public void devide(Fraction other)
        {
            num *= other.Dem;
            dem *= other.Num;
        }
        /// <summary>
        /// Numerator property
        /// </summary>
        public int Num
        {
            get { return num; }
            set { num = value; }
        }
        /// <summary>
        /// Denominator property
        /// </summary>
        public int Dem
        {
            get { return dem; }
            set { dem = value; }
        }
    }
}
