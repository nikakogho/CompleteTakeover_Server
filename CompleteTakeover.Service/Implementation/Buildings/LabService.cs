using CompleteTakeover.Domain.Buildings;
using CompleteTakeover.Repository;
using CompleteTakeover.Repository.Buildings;

namespace CompleteTakeover.Service.Buildings
{
    internal class LabService : BuildingInstanceService<LabData, ILabRepository>, ILabService
    {
        public override ILabRepository Repository => _unitOfWork.LabRepository;

        public LabService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
        {

        }
    }
}
