using CompleteTakeover.Domain.Buildings;
using CompleteTakeover.Repository;
using CompleteTakeover.Repository.Buildings;

namespace CompleteTakeover.Service.Buildings
{
    internal class MainHallService : BuildingInstanceService<MainHallData, IMainHallRepository>, IMainHallService
    {
        public override IMainHallRepository Repository => _unitOfWork.MainHallRepository;

        public MainHallService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
        {
            
        }
    }
}
