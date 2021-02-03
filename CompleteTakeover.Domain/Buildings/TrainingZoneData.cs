using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteTakeover.Domain.Buildings
{
    [Table("TrainingZones")]
    public class TrainingZoneData : BuildingInstanceData
    {
        public string CurrentUnitName { get; set; }
        public int HoursLeft { get; set; }
        public int MinutesLeft { get; set; }
        public int SecondsLeft { get; set; }
        public virtual ICollection<SquadData> Slots { get; set; }

        public TrainingZoneData()
        {
            BuildingType = "Training Zone";
        }
    }
}
