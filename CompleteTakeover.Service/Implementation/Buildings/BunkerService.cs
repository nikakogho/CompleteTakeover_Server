using CompleteTakeover.Domain.Buildings;
using CompleteTakeover.Repository;
using CompleteTakeover.Repository.Buildings;

namespace CompleteTakeover.Service.Buildings
{
    internal class BunkerService : BuildingInstanceService<BunkerData, IBunkerRepository>, IBunkerService
    {
        public override IBunkerRepository Repository => _unitOfWork.BunkerRepository;

        public BunkerService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
        {

        }
    }
}
