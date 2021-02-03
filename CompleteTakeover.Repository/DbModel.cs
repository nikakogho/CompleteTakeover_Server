using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using CompleteTakeover.Domain;
using CompleteTakeover.Domain.Buildings;

namespace CompleteTakeover.Repository
{
    public class DbModel : DbContext
    {
        #region Buildings 

        public virtual DbSet<BuildingInstanceData> Buildings { get; set; }

        public virtual DbSet<ArmyHolderData> ArmyHolders { get; set; }
        public virtual DbSet<BunkerData> Bunkers { get; set; }
        public virtual DbSet<DecorationData> Decorations { get; set; }
        public virtual DbSet<LabData> Labs { get; set; }
        public virtual DbSet<MainHallData> MainHalls { get; set; }
        public virtual DbSet<MineData> Mines { get; set; }
        public virtual DbSet<StorageData> Storages { get; set; }
        public virtual DbSet<TrainingZoneData> TrainingZones { get; set; }
        public virtual DbSet<TurretData> Turrets { get; set; }
        public virtual DbSet<TrapData> Traps { get; set; }
        public virtual DbSet<WallData> Walls { get; set; }

        #endregion

        public virtual DbSet<ConstructionData> ConstructionData { get; set; }
        public virtual DbSet<BaseData> BaseData { get; set; }
        public virtual DbSet<PlayerData> PlayerData { get; set; }
        public virtual DbSet<DefenseReport> DefenseReports { get; set; }
        public virtual DbSet<AttackReport> AttackReports { get; set; }

        public DbModel()
            : base(ConfigurationManager.ConnectionStrings["DbModel"].ConnectionString)
        {
            Database.CreateIfNotExists();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}