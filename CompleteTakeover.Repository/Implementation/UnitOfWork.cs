using System;
using System.Collections.Generic;
using System.Data.Entity;
using CompleteTakeover.Domain;
using System.Linq;

namespace CompleteTakeover.Repository
{
    using Helpers;
    using Buildings;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbModel _context;

        #region Repositories

        #region Lazy

        private Lazy<IConstructionDataRepository> _constructionDataRepository;
        private Lazy<ISquadDataRepository> _squadDataRepository;

        #region Buildings

        private Lazy<IArmyHolderRepository> _armyHolderRepository;
        private Lazy<IBunkerRepository> _bunkerRepository;
        private Lazy<IDecorationRepository> _decorationRepository;
        private Lazy<ILabRepository> _labRepository;
        private Lazy<IMainHallRepository> _mainHallRepository;
        private Lazy<IMineRepository> _mineRepository;
        private Lazy<IStorageRepository> _storageRepository;
        private Lazy<ITrainingZoneRepository> _trainingZoneRepository;
        private Lazy<ITurretRepository> _turretRepository;

        #endregion

        private Lazy<IBaseDataRepository> _baseDataRepository;
        private Lazy<IPlayerDataRepository> _playerDataRepository;
        private Lazy<IDefenseReportRepository> _defenseRepository;
        private Lazy<IAttackReportRepository> _attackRepository;
        private Lazy<ITrapRepository> _trapRepository;
        private Lazy<IWallRepository> _wallRepository;

        #endregion

        #region Property

        public IConstructionDataRepository ConstructionDataRepository => _constructionDataRepository.Value;
        public ISquadDataRepository SquadDataRepository => _squadDataRepository.Value;

        #region Buildings

        public IArmyHolderRepository ArmyHolderRepository => _armyHolderRepository.Value;
        public IBunkerRepository BunkerRepository => _bunkerRepository.Value;
        public IDecorationRepository DecorationRepository => _decorationRepository.Value;
        public ILabRepository LabRepository => _labRepository.Value;
        public IMainHallRepository MainHallRepository => _mainHallRepository.Value;
        public IMineRepository MineRepository => _mineRepository.Value;
        public IStorageRepository StorageRepository => _storageRepository.Value;
        public ITrainingZoneRepository TrainingZoneRepository => _trainingZoneRepository.Value;
        public ITurretRepository TurretRepository => _turretRepository.Value;
        public ITrapRepository TrapRepository => _trapRepository.Value;
        public IWallRepository WallRepository => _wallRepository.Value;

        #endregion

        public IBaseDataRepository BaseDataRepository => _baseDataRepository.Value;
        public IPlayerDataRepository PlayerDataRepository => _playerDataRepository.Value;
        public IDefenseReportRepository DefenseReportRepository => _defenseRepository.Value;
        public IAttackReportRepository AttackReportRepository => _attackRepository.Value;

        #endregion

        #endregion

        private DbContextTransaction _transaction;

        public UnitOfWork()
        {
            _context = new DbModel();

            _constructionDataRepository = new Lazy<IConstructionDataRepository>(() => new ConstructionDataRepository(_context));
            _squadDataRepository = new Lazy<ISquadDataRepository>(() => new SquadDataRepository(_context));

            _armyHolderRepository = new Lazy<IArmyHolderRepository>(() => new ArmyHolderRepository(_context));
            _bunkerRepository = new Lazy<IBunkerRepository>(() => new BunkerRepository(_context));
            _decorationRepository = new Lazy<IDecorationRepository>(() => new DecorationRepository(_context));
            _labRepository = new Lazy<ILabRepository>(() => new LabRepository(_context));
            _mainHallRepository = new Lazy<IMainHallRepository>(() => new MainHallRepository(_context));
            _mineRepository = new Lazy<IMineRepository>(() => new MineRepository(_context));
            _storageRepository = new Lazy<IStorageRepository>(() => new StorageRepository(_context));
            _trainingZoneRepository = new Lazy<ITrainingZoneRepository>(() => new TrainingZoneRepository(_context));
            _turretRepository = new Lazy<ITurretRepository>(() => new TurretRepository(_context));
            _trapRepository = new Lazy<ITrapRepository>(() => new TrapRepository(_context));
            _wallRepository = new Lazy<IWallRepository>(() => new WallRepository(_context));

            _baseDataRepository = new Lazy<IBaseDataRepository>(() => new BaseDataRepository(_context));
            _playerDataRepository = new Lazy<IPlayerDataRepository>(() => new PlayerDataRepository(_context));
            _defenseRepository = new Lazy<IDefenseReportRepository>(() => new DefenseReportRepository(_context));
            _attackRepository = new Lazy<IAttackReportRepository>(() => new AttackReportRepository(_context));
        }

        #region Transactions

        public bool IsTransactionActive => _transaction != null;

        public void BeginTransaction()
        {
            if (IsTransactionActive) throw new Exception("Transaction already active!");
            else _transaction = _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (IsTransactionActive) _transaction.Commit();
            else throw new Exception("Transaction not active!");
        }

        public void RollbackTransaction()
        {
            if (IsTransactionActive) _transaction.Rollback();
            else throw new Exception("Transaction not active!");
        }

        #endregion

        #region Functionality

        public bool Register(string username, string password, string faction)
        {
            if (username.Length < 2 || username.Length > 100 || password.Length < 6 || password.Length > 100) return false;
            if (PlayerDataRepository.Get(username) != null) return false;

            try
            {
                var home = BaseDataRepository.CreateNewHomeBase(faction);

                if (home == null) throw new Exception("Home is null for some reason!");

                var player = new PlayerData()
                {
                    Username = username,
                    Password = Hasher.Hash(password),
                    Faction = faction,
                    Gems = 100,
                    IconPath = faction + " Player Default",
                    LastOnline = DateTime.Now
                };
                player.Colonies = new List<BaseData> { home };
                PlayerDataRepository.Add(player);
                SaveChanges();
            }
            catch//(Exception e)
            {
                //to do log
                throw;
                //return false;
            }

            return true;
        }

        public BuildingInstanceData GetBuilding(int id)
        {
            return _context.Buildings.Find(id);
        }
        
        public List<BuildingInstanceData> GetAllBuildings()
        {
            return _context.Buildings.ToList();
        }

        #endregion

        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch// (Exception exception) may do some kind of log
            {
                //DataLogger.Log(exception.Message);
                throw;
            }
            finally
            {
                //Dispose();
            }
        }

        public void Dispose()
        {
            if (IsTransactionActive) RollbackTransaction();
            _context.Dispose();
        }
    }
}