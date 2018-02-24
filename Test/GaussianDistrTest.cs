using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PythonBackup;

namespace Test
{
    public class GaussianDistrTest
    {
        private const int ValueCount = 10000;
        private static readonly double? MinValue = -3f / 8;//= null;
        private static double Step = 1f / 8;

        public static void Test()
        {
            List<double> randomNumbers = GetRandomNumbers().Where(x => !double.IsInfinity(x)).OrderBy(x => x).ToList();
            if (MinValue != null && randomNumbers.First() < MinValue.Value)
            {
                //throw new Exception("Unexpected value");
            }

            List<Tuple<string, int>> groups = new List<Tuple<string, int>>();
            int minGroupIndex = (int)Math.Floor(randomNumbers.First() / Step);
            double minRange = minGroupIndex * Step;
            int currentIndex = 0;
            while (currentIndex < randomNumbers.Count)
            {
                int count = 0;
                double maxRange = minRange + Step;
                while (currentIndex < randomNumbers.Count && randomNumbers[currentIndex] < maxRange)
                {
                    count++;
                    currentIndex++;
                }
                groups.Add(new Tuple<string, int>(string.Format("[{0};{1}[", minRange, maxRange), count));
                minRange = maxRange;
            }
            foreach (Tuple<string, int> group in groups)
            {
                Console.WriteLine(string.Format("{0} : {1}", group.Item1, group.Item2));
            }
        }

        private static List<double> GetRandomNumbers()
        {
            List<double> randomNumbers = new List<double>();
            for (int i = 0; i < ValueCount; i++)
            {
                randomNumbers.Add(GaussDistr.DrawRandomNormalLaw(MinValue));
            }
            return randomNumbers;
        }


    }
}