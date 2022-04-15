using ParagonApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumber_Picker
{
    public class Chord
    {
        public Chord(Member member, string trussName)
        {
            Name = member.Name;
            TrussName = trussName;
        }
        public string Name { get; set; }
        public string TrussName { get; set; }
    }
}
