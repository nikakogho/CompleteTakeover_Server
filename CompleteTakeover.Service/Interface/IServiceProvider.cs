using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompleteTakeover.Service
{
    using Domain;
    using Buildings;

    public interface IServiceProvider : IDisposable
    {
        #region Services

        IConstructionDataService ConstructionDataService { get; }
        ISquadDataService SquadDataService { get; }

        #region Buildings

        IArmyHolderService ArmyHolderService { get; }
        IBunkerService BunkerService { get; }
        IDecorationService DecorationService { get; }
        ILabService LabService { get; }
        IMainHallService MainHallService { get; }
        IMineService MineService { get; }
        IStorageService StorageService { get; }
        ITrainingZoneService TrainingZoneService { get; }
        ITurretService TurretService { get; }
        ITrapService TrapService { get; }
        IWallService WallService { get; }

        #endregion

        IBaseDataService BaseDataService { get; }
        IPlayerDataService PlayerDataService { get; }
        IDefenseReportService DefenseReportService { get; }
        IAttackReportService AttackReportService { get; }

        #endregion

        #region Transactions

        bool IsTransactionActive { get; }

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();

        #endregion

        #region Functionality

        bool Register(string username, string password, string faction);
        void AddBuilding(BuildingInstanceData building);
        BuildingInstanceData GetBuilding(int id);

        #endregion

        void SaveChanges();
    }
}
