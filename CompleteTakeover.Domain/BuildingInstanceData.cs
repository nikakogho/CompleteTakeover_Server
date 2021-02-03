using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteTakeover.Domain
{
    public abstract class BuildingInstanceData
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string DataName { get; set; }
        public string BuildingType { get; set; }
        //[Required]
        //[ForeignKey("Base")]
        public int BaseDataID { get; set; }
        public virtual BaseData Base { get; set; }
        [Required]
        public int Level { get; set; }
        [Required]
        public int TileX { get; set; }
        [Required]
        public int TileY { get; set; }
        [Required]
        public bool Destroyed { get; set; }
        public virtual ICollection<DefenseReport> DestroyedInAIAttacks { get; set; }
        public virtual ICollection<AttackReport> DestroyedInPlayerAttacks { get; set; }
    }
}
