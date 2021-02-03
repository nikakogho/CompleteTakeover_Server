using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteTakeover.Domain.Buildings
{
    [Table("Bunkers")]
    public class BunkerData : BuildingInstanceData
    {
        public virtual ICollection<SquadData> StoredUnits { get; set; }

        public BunkerData()
        {
            BuildingType = "Bunker";
        }
    }
}
