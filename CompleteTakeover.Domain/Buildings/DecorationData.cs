using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteTakeover.Domain.Buildings
{
    [Table("Decorations")]
    public class DecorationData : BuildingInstanceData
    {
        public DecorationData()
        {
            BuildingType = "Decoration";
        }
    }
}
