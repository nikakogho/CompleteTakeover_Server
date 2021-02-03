using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteTakeover.Domain
{
    public class BaseData
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public bool IsHome { get; set; }
        [Required]
        public DateTime LastVisited { get; set; }
        [Required]
        public virtual PlayerData Player { get; set; }
        [Required]
        public int Builders { get; set; }
        //[ForeignKey("BaseDataID")]
        public virtual ICollection<BuildingInstanceData> Buildings { get; set; }
        public virtual ICollection<ConstructionData> Constructions { get; set; }
        public virtual ICollection<SquadData> Squads { get; set; }
        public virtual ICollection<DefenseReport> AttackedByAIReports { get; set; }
        public virtual ICollection<AttackReport> AttackedByPlayerReports { get; set; }

        public bool IsDestroyed => Buildings.Where(b => !b.Destroyed).Count() == 0;
    }
}
