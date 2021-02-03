using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompleteTakeover.Service
{
    using Buildings;
    using Domain;
    using Domain.Buildings;
    using Repository;

    public class ServiceProvider : IServiceProvider
    {
        private IUnitOfWork _unitOfWork;

        #region Services

        #region Lazy

        private Lazy<IConstructionDataService> _constructionDataService;
        private Lazy<ISquadDataService> _squadDataService;

        #region Buildings

        private Lazy<IArmyHolderService> _armyHolderService;
        private Lazy<IBunkerService> _bunkerService;
        private Lazy<IDecorationService> _decorationService;
        private Lazy<ILabService> _labService;
        private Lazy<IMainHallService> _mainHallService;
        private Lazy<IMineService> _mineService;
        private Lazy<IStorageService> _storageService;
        private Lazy<ITrainingZoneService> _trainingZoneService;
        private Lazy<ITurretService> _turretService;
        private Lazy<ITrapService> _trapService;
        private Lazy<IWallService> _wallService;

        #endregion

        private Lazy<IBaseDataService> _baseDataService;
        private Lazy<IPlayerDataService> _playerDataService;
        private Lazy<IDefenseReportService> _defenseReportService;
        private Lazy<IAttackReportService> _attackReportService;

        #endregion

        #region Property

        public IConstructionDataService ConstructionDataService => _constructionDataService.Value;
        public ISquadDataService SquadDataService => _squadDataService.Value;

        #region Buildings
        
        public IArmyHolderService ArmyHolderService => _armyHolderService.Value;
        public IBunkerService BunkerService => _bunkerService.Value;
        public IDecorationService DecorationService => _decorationService.Value;
        public ILabService LabService => _labService.Value;
        public IMainHallService MainHallService => _mainHallService.Value;
        public IMineService MineService => _mineService.Value;
        public IStorageService StorageService => _storageService.Value;
        public ITrainingZoneService TrainingZoneService => _trainingZoneService.Value;
        public ITurretService TurretService => _turretService.Value;
        public ITrapService TrapService => _trapService.Value;
        public IWallService WallService => _wallService.Value;

        #endregion

        public IBaseDataService BaseDataService => _baseDataService.Value;
        public IPlayerDataService PlayerDataService => _playerDataService.Value;
        public IDefenseReportService DefenseReportService => _defenseReportService.Value;
        public IAttackReportService AttackReportService => _attackReportService.Value;

        #endregion

        #endregion

        public ServiceProvider()
        {
            _unitOfWork = new UnitOfWork();

            _constructionDataService = new Lazy<IConstructionDataService>(() => new ConstructionDataService(_unitOfWork));
            _squadDataService = new Lazy<ISquadDataService>(() => new SquadDataService(_unitOfWork));

            #region Buildings

            _armyHolderService = new Lazy<IArmyHolderService>(() => new ArmyHolderService(_unitOfWork));
            _bunkerService = new Lazy<IBunkerService>(() => new BunkerService(_unitOfWork));
            _decorationService = new Lazy<IDecorationService>(() => new DecorationService(_unitOfWork));
            _labService = new Lazy<ILabService>(() => new LabService(_unitOfWork));
            _mainHallService = new Lazy<IMainHallService>(() => new MainHallService(_unitOfWork));
            _mineService = new Lazy<IMineService>(() => new MineService(_unitOfWork));
            _storageService = new Lazy<IStorageService>(() => new StorageService(_unitOfWork));
            _trainingZoneService = new Lazy<ITrainingZoneService>(() => new TrainingZoneService(_unitOfWork));
            _turretService = new Lazy<ITurretService>(() => new TurretService(_unitOfWork));
            _trapService = new Lazy<ITrapService>(() => new TrapService(_unitOfWork));
            _wallService = new Lazy<IWallService>(() => new WallService(_unitOfWork));

            #endregion

            _baseDataService = new Lazy<IBaseDataService>(() => new BaseDataService(_unitOfWork));
            _playerDataService = new Lazy<IPlayerDataService>(() => new PlayerDataService(_unitOfWork));
            _defenseReportService = new Lazy<IDefenseReportService>(() => new DefenseReportService(_unitOfWork));
            _attackReportService = new Lazy<IAttackReportService>(() => new AttackReportService(_unitOfWork));
        }

        #region Transactions

        public bool IsTransactionActive => _unitOfWork.IsTransactionActive;

        public void BeginTransaction()
        {
            _unitOfWork.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _unitOfWork.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _unitOfWork.RollbackTransaction();
        }

        #endregion

        #region Functionality

        public bool Register(string username, string password, string faction)
        {
            return _unitOfWork.Register(username, password, faction);
        }

        public void AddBuilding(BuildingInstanceData building)
        {
            switch (building.BuildingType)
            {
                case "Army Holder": ArmyHolderService.Add(building as ArmyHolderData); break;
                case "Bunker": BunkerService.Add(building as BunkerData); break;
                case "Lab": LabService.Add(building as LabData); break;
                case "Main Hall": MainHallService.Add(building as MainHallData); break;
                case "Mine": MineService.Add(building as MineData); break;
                case "Storage": StorageService.Add(building as StorageData); break;
                case "Training Zone": TrainingZoneService.Add(building as TrainingZoneData); break;
                case "Turret": TurretService.Add(building as TurretData); break;
            }
        }

        public BuildingInstanceData GetBuilding(int id)
        {
            return _unitOfWork.GetBuilding(id);
        }

        public List<BuildingInstanceData> GetAllBuildings()
        {
            return _unitOfWork.GetAllBuildings();
        }

        #endregion

        public void SaveChanges()
        {
            _unitOfWork.SaveChanges();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
