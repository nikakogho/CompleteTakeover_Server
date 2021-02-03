using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteTakeover.Domain.Buildings
{
    [Table("Walls")]
    public class WallData : BuildingInstanceData
    {
        public WallData()
        {
            BuildingType = "Wall";
        }
    }
}
