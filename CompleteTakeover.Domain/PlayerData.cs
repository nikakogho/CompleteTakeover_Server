using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompleteTakeover.Domain
{
    public class PlayerData
    {
        [Key]
        public string Username { get; set; }
        [Required]
        public byte[] Password { get; set; }
        [Required]
        public string IconPath { get; set; }
        [Required]
        public string Faction { get; set; }
        [Required]
        public int Gems { get; set; }
        [Required]
        public DateTime LastOnline { get; set; }
        public virtual ICollection<BaseData> Colonies { get; set; }
        public virtual ICollection<AttackReport> Attacks { get; set; }
    }
}
