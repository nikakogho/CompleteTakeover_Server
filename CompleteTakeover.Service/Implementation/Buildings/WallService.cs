using CompleteTakeover.Domain.Buildings;
using CompleteTakeover.Repository;
using CompleteTakeover.Repository.Buildings;

namespace CompleteTakeover.Service.Buildings
{
    internal class WallService : BuildingInstanceService<WallData, IWallRepository>, IWallService
    {
        public override IWallRepository Repository => _unitOfWork.WallRepository;

        public WallService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
        {

        }
    }
}
