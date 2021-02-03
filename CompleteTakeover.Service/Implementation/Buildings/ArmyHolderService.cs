using CompleteTakeover.Domain.Buildings;
using CompleteTakeover.Repository;
using CompleteTakeover.Repository.Buildings;

namespace CompleteTakeover.Service.Buildings
{
    internal class ArmyHolderService : BuildingInstanceService<ArmyHolderData, IArmyHolderRepository>, IArmyHolderService
    {
        public override IArmyHolderRepository Repository => _unitOfWork.ArmyHolderRepository;

        public ArmyHolderService(IUnitOfWork unitOfWork)
        :base(unitOfWork)
        {

        }
    }
}
