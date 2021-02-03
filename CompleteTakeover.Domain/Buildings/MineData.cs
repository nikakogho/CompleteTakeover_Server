using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteTakeover.Domain.Buildings
{
    [Table("Mines")]
    public class MineData : BuildingInstanceData
    {
        public int Stored { get; set; }
        public bool StoresGold { get; set; }
        //public int Capacity { get; set; }

        public MineData()
        {
            BuildingType = "Mine";
        }
    }
}
