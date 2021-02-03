using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteTakeover.Domain.Buildings
{
    [Table("Turrets")]
    public class TurretData : BuildingInstanceData
    {
        public TurretData()
        {
            BuildingType = "Turret";
        }
    }
}
