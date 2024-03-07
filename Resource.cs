using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chord
{
    internal class Resource
    {
        public int Hash { get; set; }
        public string Value { get; set; }
    
        public Resource(string value, int numberNodes) 
        {
                Value = value;
                Hash = GetHash(value,numberNodes);
        }

        public static int GetHash(string value, int numberNodes)
        {
            int h = 0;
            for (int i = 0; i < value.Length; i++)
            {
                h += value[i];
            }
            return ((h * 31) % numberNodes) + 1;
        }
    }
}
