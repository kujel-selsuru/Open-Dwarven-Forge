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
    /// stores the names of the sectors for a sector graph 256
    /// </summary>
    public enum sector256 {a1 = 0, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16,
                            b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15, b16,
                            c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12, c13, c14, c15, c16,
                            d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12, d13, d14, d15, d16,
                            e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, e16,
                            f1, f2, f3, f4, f5, f6, f7, f8, f9, f10, f11, f12, f13, f14, f15, f16,
                            g1, g2, g3, g4, g5, g6, g7, g8, g9, g10, g11, g12, g13, g14, g15, g16,
                            h1, h2, h3, h4, h5, h6, h7, h8, h9, h10, h11, h12, h13, h14, h15, h16,
                            i1, i2, i3, i4, i5, i6, i7, i8, i9, i10, i11, i12, i13, i14, i15, i16,
                            j1, j2, j3, j4, j5, j6, j7, j8, j9, j10, j11, j12, j13, j14, j15, j16,
                            k1, k2, k3, k4, k5, k6, k7, k8, k9, k10, k11, k12, k13, k14, k15, k16,
                            l1, l2, l3, l4, l5, l6, l7, l8, l9, l10, l11, l12, l13, l14, l15, l16,
                            m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12, m13, m14, m15, m16,
                            n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, n11, n12, n13, n14, n15, n16,
                            o1, o2, o3, o4, o5, o6, o7, o8, o9, o10, o11, o12, o13, o14, o15, o16,
                            p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16,
                            none
                            };
    /// <summary>
    /// A sector graph with 256 sectors
    /// by defualt scale is set for a 9600 x 9600 pixel area
    /// </summary>
    public class SectorGraph256
    {
        //protected
        protected sector256[,] grid;
        protected int scalex;
        protected int scaley;

        //public
        /// <summary>
        /// Sector256Graph constructor
        /// </summary>
        /// <param name="width">Width of region</param>
        /// <param name="height">Height of region</param>
        public SectorGraph256(int width = 9600, int height = 9600)
        {
            setScale(width, height);
            grid = new sector256[16, 16];
            int val = 0;
            for (int i = 0; i < 16; i++)
            {
                for (int k = 0; k < 16; k++)
                {
                    grid[i, k] = (sector256)val;
                    val++;
                }
            }
        }
        /// <summary>
        /// sets the scale for the sector graph
        /// </summary>
        /// <param name="width">area width</param>
        /// <param name="height">area height</param>
        public void setScale(int width, int height)
        {
            scalex = width / 16;
            scaley = height / 16;
        }
        /// <summary>
        /// given a point return the sector to which it belongs
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>Sector256 enumeration</returns>
        public sector256 findSector(int x, int y)
        {
            return grid[x / scalex, y / scaley];
        }
    }
}
