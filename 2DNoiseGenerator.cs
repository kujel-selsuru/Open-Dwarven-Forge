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
    public static class D2NoiseGenerator
    {
        private static Random rand = new Random((int) System.DateTime.Today.Ticks);
        private static int[] permutations;
        private static Point2D[] gradiants;

        static D2NoiseGenerator()
        {
            calculatePermuations(out permutations);
            calculateGradiants(out gradiants);
        }

        private static void calculatePermuations(out int[] p)
        {
            p = Enumerable.Range(0, 512).ToArray();

            for(int i = 0; i < p.Length; i++)
            {
                int source = rand.Next(p.Length);

                int t = p[i];
                p[i] = p[source];
                p[source] = t;
            }
        }

        public static void reseed()
        {
            calculatePermuations(out permutations);
        }

        private static void calculateGradiants(out Point2D[] grad)
        {
            grad = new Point2D[512];

            for(int i = 0; i < grad.Length; i++)
            {
                Point2D gradiant;

                do
                {
                    gradiant = new Point2D((float)rand.NextDouble() * 2 - 1, (float)rand.NextDouble() * 2 - 1);
                }while(gradiant.lengthSquared() >= 1);

                gradiant.normalize();

                grad[i] = gradiant;
            }
        }

        private static float drop(float t)
        {
            t = Math.Abs(t);
            return 1f - t * t * t * (t *(t * 6 - 15) + 10);
        }

        private static float Q(float u, float v)
        {
            return drop(u) * drop(v);
        }

        public static float noise(float x, float y)
        {
            Point2D cell = new Point2D((float)Math.Floor(x), (float)Math.Floor(y));
            float total = 0f;
            Point2D[] corners = new Point2D[] { new Point2D(0, 0), new Point2D(0, 1), new Point2D(1, 0), new Point2D(1, 1) };

            foreach(Point2D n in corners)
            {
                Point2D ij = new Point2D(cell.X + n.X, cell.Y + n.Y);
                Point2D uv = new Point2D(x - ij.X, y - ij.Y);

                int index = permutations[(int)ij.X % permutations.Length];
                index = permutations[(index + (int)ij.Y) % permutations.Length];

                Point2D grad = gradiants[index % gradiants.Length];

                total += Q(uv.X, uv.Y) * Point2D.dotProduct(grad, uv);
            }
            return Math.Max(Math.Min(total, 1f), -1f);
        }
        /// <summary>
        /// Generates a 2D array of floats containing noise data
        /// </summary>
        /// <param name="w">Width of grid</param>
        /// <param name="h">Height of grid</param>
        /// <param name="octaves">Number of octaves</param>
        /// <returns>Float[]</returns>
        public static float[] generateNoiseGrid(int w, int h, int octaves)
        {
            float[] data = new float[w * h];
            float min = float.MaxValue;
            float max = float.MinValue;

            reseed();

            float frequency = 0.5f;
            float amplitude = 1f;
            //float persistence = 0.25f;

            for(int octave = 0; octave < octaves; octave++)
            {
                Parallel.For(0,
                    w * h,
                    (offset) =>
                    {
                        float i = offset % w;
                        float j = offset / w;
                        float noise = D2NoiseGenerator.noise(i * frequency * 1f / w, j * frequency * 1f / h);
                        noise = data[(int)j * w + 1] += noise * amplitude;

                        min = Math.Min(min, noise);
                        max = Math.Max(max, noise);
                    }
                );

                frequency *= 2;
                amplitude /= 2;
            }

            return data;
        }
    }
}
