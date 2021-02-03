using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteTakeover.Domain.Buildings
{
    [Table("Labs")]
    public class LabData : BuildingInstanceData
    {
        public string WorkingOnUnit { get; set; }
        public int HoursLeft { get; set; }
        public int MinutesLeft { get; set; }
        public int SecondsLeft { get; set; }

        public LabData()
        {
            BuildingType = "Lab";
        }
    }
}
