using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace KeyboardIdentify
{
    public class KeyboardTimeline
    {
        struct TimePoint
        {
            public DateTime time;
            public int key;

            public TimePoint(DateTime t, int k)
            {
                time = t;
                key = k;
            }
        }

        private List<TimePoint> DownLine;
        private List<TimePoint> UpLine;

        public KeyboardTimeline()
        {
            DownLine = new List<TimePoint>();
            UpLine = new List<TimePoint>();
        }

        public void MarkDown(int k)
        {
            DownLine.Add(new TimePoint(DateTime.Now, k));
        }

        public void MarkUp(int k)
        {
            UpLine.Add(new TimePoint(DateTime.Now, k));
        }


        public Vector ToVector()
        {
            List<double> v = new List<double>();

            DateTime now = DateTime.Now;
            for (int i = 0; i < DownLine.Count; ++i)
            {
                for (int j = 0; j < UpLine.Count; ++j)
                {
                    if (DownLine[i].key.Equals(UpLine[j].key))
                    {
                        v.Add((UpLine[j].time - DownLine[i].time).TotalMilliseconds);
                        now = UpLine[j].time;
                        UpLine.RemoveAt(j);
                    }
                }
                if (i + 1 < DownLine.Count)
                {
                    v.Add((DownLine[i + 1].time - now).TotalMilliseconds);
                }
            }

            return new Vector(v);
        }

        public static bool IsAvailableKey(int keyValue)
        {
            return !(keyValue >= (int) Keys.A && keyValue <= (int) Keys.Z ||
                     keyValue >= (int) Keys.D0 && keyValue <= (int) Keys.D9 ||
                     keyValue >= (int) Keys.NumPad0 && keyValue <= (int)Keys.NumPad9);
        }
    }
}
