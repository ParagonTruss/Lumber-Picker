using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumber_Picker
{
    
    class Rack
    {
        public Rack(string number, double length, string grade, string species, string treatment)
        {
            Number = number;
            Length = length;
            Grade = grade;
            Species = species;
            Treatment = treatment;

        }
        public string Number { get; }
        public double Length { get; set; }
        public string Grade { get; set; }
        public string Species { get; set; }
        public string Treatment { get; set; }
    }
}
