using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteTakeover.Domain
{
    public class DefenseReport
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string InvasionName { get; set; }
        [Required]
        public virtual BaseData Base { get; set; }
        [Required]
        public int LostGold { get; set; }
        [Required]
        public int LostElixir { get; set; }
        public bool Processed { get; set; }
        public virtual ICollection<BuildingInstanceData> DestroyedBuildings { get; set; }
        public virtual ICollection<SquadData> DeployedArmy { get; set; }
    }
}
