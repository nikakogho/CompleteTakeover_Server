using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CompleteTakeover.Domain.Buildings;

namespace CompleteTakeover.Domain
{
    public class SquadData
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string UnitName { get; set; }
        [Required]
        public int Amount { get; set; }
        public virtual BaseData InArmyOfBase { get; set; }
        public virtual BunkerData StoredAtBunker { get; set; }
        public virtual TrainingZoneData TrainingSlotAt { get; set; }
        public virtual AttackReport DeployedInAttack { get; set; }
        public virtual DefenseReport DeployedInDefense { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
    }
}
