using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex1
{
    class DataFileReader
    {
        public DataFileReader()
        {

        }

        public List<List<float>> parseDataFromFile(String fileName)
        {
            List<List<float>> list = new List<List<float>>();

            string[] lines = System.IO.File.ReadAllLines(fileName);
            string line = lines[0];
            string[] words = line.Split(',');

            foreach (String word in words)
            {
                list.Add(new List<float>());
            }

            for (int i = 0; i < lines.Length; i++)
            {
                line = lines[i];
                for (int j = 0; j < words.Length; j++)
                {
                    words = line.Split(',');
                    list[j].Add(float.Parse(words[j]));
                }
            }
            return list;
        }
    }
}
