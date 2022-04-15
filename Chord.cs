using ParagonApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Globalization;

namespace Lumber_Picker
{
    public class Chord
    {
        public Chord(Member member, string trussName)
        {
            Name = member.Name;
            TrussName = trussName;
            Length = member.OverallLength;
            Grade = member.Lumber.Grade;
            Species = member.Lumber.Species;
            Treatment = member.Lumber.TreatmentType;

        }
        public string Name { get; }
        public string TrussName { get; }
        //[DisplayMember("chord Length")]
        public double Length { get; }
        public string Grade { get; }
        public string Species { get; }
        public string Treatment { get; }
        public string StockItem { get; set; }
    }
}
