/*=============================================================================
CLASSES
=============================================================================*/
using System;
using System.Collections.Generic;
namespace PythonBackup
{
    public class GaussDistr
    {
        //Initializing like this is the same as doing it in the constructor 
        private static Random rand = new Random();

        #region Fields
        private Sensus parent;
        private int mu;
        private float sig;
        public List<int> collection;

        #endregion

        public GaussDistr(Sensus parent, int mu, float sig)
        {
            this.parent = parent;
            this.mu = mu;
            this.sig = sig;
            this.collection = new List<int>();
        }

        public List<double> CreateDistr(int nN)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < nN; i++)
            {
                result.Add(DrawRand());
            }
            return result;
        }

        //This is not a real gaussian distribution just an approximation
        public double DrawRand(double? min = null)
        {
            if (min != null)
            {
                min = (min.Value - mu) / sig;
            }
            return mu + DrawRandomNormalLaw(min) * sig;
        }

        /// <summary>
        /// Draws a random number from a normal law
        /// </summary>
        /// <param name="min">Minimum returned value</param>
        /// <returns>Random value</returns>
        public static double DrawRandomNormalLaw(double? min = null)
        {
            double K = Math.Sin(2 * Math.PI * rand.NextDouble());
            //Creates a random numbers (uniform distribution) in (0;1]
            //It's the same as rand.NextDouble() but be don't want 0 (1 is accepted) because of the ln (noted Log)
            double r = 1 - rand.NextDouble();
            bool opposeResult = false;
            if (min != null)
            {
                //1 side of the bell is truncated
                double minValue = min.Value;
                if (K < 0)
                {
                    K = -K;
                }
                double xMinTruncAbs, xMaxTruncAbs;
                bool pickInTruncatedSide = true;
                if (minValue < 0)
                {
                    xMinTruncAbs = GetLimitX1(K, -minValue);
                    xMaxTruncAbs = 1;
                    pickInTruncatedSide = (rand.NextDouble() > 1 / (2 - xMinTruncAbs));
                    opposeResult = pickInTruncatedSide;
                }
                else
                {
                    xMinTruncAbs = 0;
                    xMaxTruncAbs = GetLimitX1(K, minValue);
                }
                if (pickInTruncatedSide)
                {
                    r = xMinTruncAbs + r * (xMaxTruncAbs - xMinTruncAbs);
                }
            }
            double absoluteResult = Math.Sqrt(-2 * Math.Log(r)) * K;
            return opposeResult ? -absoluteResult : absoluteResult;
        }

        /// <summary>
        /// Gets the limit value of x1 L so that L * coeffX2 is the limit acceptable value 
        /// </summary>
        /// <param name="coeffX2">sin(Pi * x2) with x2 in [0;1[</param>
        /// <returns></returns>
        private static double GetLimitX1(double coeffX2, double limit)
        {
            if (coeffX2 == 0)
            {
                return 0;
            }
            limit /= coeffX2;
            return Math.Exp(-(limit * limit) / 2);
        }

        /// <summary>
        /// Returns a random boolean
        /// </summary>
        /// <returns>Random boolean</returns>
        private static bool GetRandomBoolean()
        {
            return rand.Next() % 2 == 1;
        }
    }
}