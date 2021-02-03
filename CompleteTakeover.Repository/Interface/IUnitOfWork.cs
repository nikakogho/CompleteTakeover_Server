using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompleteTakeover.Repository
{
    using Buildings;
    using CompleteTakeover.Domain;

    public interface IUnitOfWork : IDisposable
    {
        #region Repositories

        IConstructionDataRepository ConstructionDataRepository { get; }
        ISquadDataRepository SquadDataRepository { get; }

        #region Buildings

        IArmyHolderRepository ArmyHolderRepository { get; }
        IBunkerRepository BunkerRepository { get; }
        IDecorationRepository DecorationRepository { get; }
        ILabRepository LabRepository { get; }
        IMainHallRepository MainHallRepository { get; }
        IMineRepository MineRepository { get; }
        IStorageRepository StorageRepository { get; }
        ITrainingZoneRepository TrainingZoneRepository { get; }
        ITurretRepository TurretRepository { get; }
        ITrapRepository TrapRepository { get; }
        IWallRepository WallRepository { get; }

        #endregion

        IBaseDataRepository BaseDataRepository { get; }
        IPlayerDataRepository PlayerDataRepository { get; }
        IDefenseReportRepository DefenseReportRepository { get; }
        IAttackReportRepository AttackReportRepository { get; }

        #endregion

        #region Transactions

        bool IsTransactionActive { get; }

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();

        #endregion

        #region Functionality
        
        bool Register(string username, string password, string faction);
        BuildingInstanceData GetBuilding(int id);
        List<BuildingInstanceData> GetAllBuildings();

        #endregion

        void SaveChanges();
    }
}
