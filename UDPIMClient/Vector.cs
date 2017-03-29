using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace UDPIMClient
{
    class Vector
    {
        public List<double> vector;

        public double this[int index]
        {
            get
            {
                return vector[index];
            }
            set
            {
                vector[index] = value;
            }
        }

        public int Dimension
        {
            get
            {
                return vector.Count;
            }
        }

        public Vector()
        {
            vector = new List<double>();
        }

        public Vector(double[] array)
        {
            vector = new List<double>(array);
        }

        public Vector(ICollection<double> array)
        {
            vector = new List<double>();
            foreach (double i in array)
            {
                vector.Add(i);
            }
        }

        /// <summary>
        /// Caculate distance between 2 vectors.
        /// </summary>
        /// <param name="v">another vector</param>
        /// <returns>distance</returns>
        public double DistanceBetween(Vector v)
        {
            if (v.Dimension != Dimension)
            {
                throw new ArgumentException("Dimension not equal!");
            }
            double distance = 0;
            for (int iter = 0; iter < Dimension; ++iter)
            {
                distance += Math.Pow(this[iter] + v[iter], 2);
            }
            return Math.Sqrt(distance);
        }

        public static double DistanceBetween(Vector v1, Vector v2)
        {
            if (v1.Dimension != v2.Dimension)
            {
                throw new ArgumentException("Dimension not equal!");
            }
            double distance = 0;
            for (int iter = 0; iter < v1.Dimension; ++iter)
            {
                distance += Math.Pow(v1[iter] + v2[iter], 2);
            }
            return Math.Sqrt(distance);
        }

        public string GetJson()
        {
            string ret = JsonConvert.SerializeObject(this);
            return ret;
        }

        public static Vector GetVector(string VectorJson)
        {
            Vector ret = JsonConvert.DeserializeObject<Vector>(VectorJson);
            return ret;
        }

        /// <summary>
        /// deserialize a vector from xml
        /// </summary>
        /// <param name="reader">a XmlReader</param>
        /// <returns>a vector object</returns>
        public static Vector GetVectorFromXml(XmlReader reader)
        {
            XmlSerializer x = new XmlSerializer(typeof(Vector));
            Vector v;
            try
            {
                v = x.Deserialize(reader) as Vector;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }

            return v;
        }

        /// <summary>
        /// serizlize a vector object to xml document
        /// </summary>
        /// <returns>a string object that contains a serialized vector object.</returns>
        public override string ToString()
        {
            XmlSerializer x = new XmlSerializer(this.GetType());
            TextWriter writer = new StringWriter();
            x.Serialize(writer, this);
            return writer.ToString();
        }
    }

}
