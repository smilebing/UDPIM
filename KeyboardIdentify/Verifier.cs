using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyboardIdentify;

namespace KeyboardIdentify
{
    public class Verifier
    {
        public static bool Verify(Vector v, ICollection<Vector> vectors)
        {
            List<double> distance = new List<double>();
            double threshold = CaculateThresholdDistance(vectors);
            int first20Percent = 0;
            int last20Percent = 0;
            int count = 0;

            //求出距离
            foreach (Vector vec in vectors)
            {
                distance.Add(Vector.DistanceBetween(v, vec));
            }
            //计算前后20%的index并移除。
            //先移除后面的避免index发生变化
            first20Percent = (int)(distance.Count * 0.2);
            last20Percent = (int) (distance.Count * 0.8);
            distance.RemoveRange(last20Percent, distance.Count-last20Percent);
            distance.RemoveRange(0, first20Percent);

            foreach (double d in distance)
            {
                if (Math.Abs(d / threshold - 1) < 0.15)
                    count++;
            }

            return (double)count/(double)distance.Count > 0.8;
        }

        public static double CaculateThresholdDistance(ICollection<Vector> vectors)
        {
            List<double> distances = new List<double>();
            //List<Vector> VectorsList = new List<Vector>(vectors);
            foreach (Vector v1 in vectors)
            {
                foreach (Vector v2 in vectors)
                {
                    if (!ReferenceEquals(v1, v2))
                    {
                        distances.Add(Vector.DistanceBetween(v1, v2));
                    }
                }
            }
            //去掉前20%和后20%，求平均值，即为阈值
            List<double> TrimedDistances = new List<double>();
            for (int i = (int)(distances.Count * 0.2); i < distances.Count * 0.8; ++i)
            {
                TrimedDistances.Add(distances[i]);
            }
            return TrimedDistances.Average();
        }
    }
}
