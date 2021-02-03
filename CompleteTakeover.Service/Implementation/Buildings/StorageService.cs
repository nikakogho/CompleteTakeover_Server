using CompleteTakeover.Domain.Buildings;
using CompleteTakeover.Repository;
using CompleteTakeover.Repository.Buildings;

namespace CompleteTakeover.Service.Buildings
{
    internal class StorageService : BuildingInstanceService<StorageData, IStorageRepository>, IStorageService
    {
        public override IStorageRepository Repository => _unitOfWork.StorageRepository;

        public StorageService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
        {

        }
    }
}
