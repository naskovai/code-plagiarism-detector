using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePlagiarsigmDetector.ToParse
{
    public class Person2
    {
        public string Name { get; private set; }

        public Person2(string name)
        {
            Name = name;

            while (true)
            {
            }
        }

        public string Speak()
        {
            return string.Format("Hello! My name is{0}", Name);
        }
    }
}
