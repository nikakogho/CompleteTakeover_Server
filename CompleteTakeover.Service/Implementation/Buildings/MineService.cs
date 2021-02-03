using CompleteTakeover.Domain.Buildings;
using CompleteTakeover.Repository;
using CompleteTakeover.Repository.Buildings;

namespace CompleteTakeover.Service.Buildings
{
    internal class MineService : BuildingInstanceService<MineData, IMineRepository>, IMineService
    {
        public override IMineRepository Repository => _unitOfWork.MineRepository;

        public MineService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
        {
            
        }
    }
}
