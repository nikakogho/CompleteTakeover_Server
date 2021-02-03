using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteTakeover.Domain
{
    public class ConstructionData
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public virtual BaseData Base { get; set; }
        public string BuildingDataName { get; set; }
        public virtual BuildingInstanceData Upgrading { get; set; }
        [Required]
        public int Hours { get; set; }
        [Required]
        public int Minutes { get; set; }
        [Required]
        public int Seconds { get; set; }
        [Required]
        public int TileX { get; set; }
        [Required]
        public int TileY { get; set; }
        public bool Over { get; set; }
    }
}
