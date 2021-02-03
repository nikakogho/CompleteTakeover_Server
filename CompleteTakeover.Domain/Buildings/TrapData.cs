using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteTakeover.Domain.Buildings
{
    [Table("Traps")]
    public class TrapData : BuildingInstanceData
    {
        public TrapData()
        {
            BuildingType = "Trap";
        }
    }
}
