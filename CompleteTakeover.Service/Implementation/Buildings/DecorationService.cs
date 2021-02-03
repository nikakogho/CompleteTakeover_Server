using CompleteTakeover.Domain.Buildings;
using CompleteTakeover.Repository;
using CompleteTakeover.Repository.Buildings;

namespace CompleteTakeover.Service.Buildings
{
    internal class DecorationService : 
        BuildingInstanceService<DecorationData, IDecorationRepository>, IDecorationService
    {
        public override IDecorationRepository Repository => _unitOfWork.DecorationRepository;

        public DecorationService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
        {

        }
    }
}
