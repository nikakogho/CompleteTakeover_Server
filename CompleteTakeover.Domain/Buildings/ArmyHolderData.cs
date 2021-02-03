using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteTakeover.Domain.Buildings
{
    [Table("ArmyHolders")]
    public class ArmyHolderData : BuildingInstanceData
    {
        public ArmyHolderData()
        {
            BuildingType = "Army Holder";
        }
    }
}
