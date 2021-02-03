using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteTakeover.Domain.Buildings
{
    [Table("MainHalls")]
    public class MainHallData : BuildingInstanceData
    {
        public int StoredGold { get; set; }
        public int StoredElixir { get; set; }
        public int GoldCapacity { get; set; }
        public int ElixirCapacity { get; set; }

        public MainHallData()
        {
            BuildingType = "Main Hall";
        }
    }
}
