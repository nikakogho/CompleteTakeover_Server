using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompleteTakeover.Domain
{
    public class AttackReport
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public virtual BaseData Base { get; set; }
        [Required]
        public virtual PlayerData Attacker { get; set; }
        [Required]
        public int AmassedGold { get; set; }
        [Required]
        public int AmassedElixir { get; set; }
        public bool WasRevenge { get; set; }
        public bool Avenged { get; set; }
        public bool ProcessedOnAttackersSide { get; set; }
        public bool ProcessedOnDefendersSide { get; set; }
        public virtual ICollection<BuildingInstanceData> DestroyedBuildings { get; set; }
        public virtual ICollection<SquadData> DeployedArmy { get; set; }
    }
}
