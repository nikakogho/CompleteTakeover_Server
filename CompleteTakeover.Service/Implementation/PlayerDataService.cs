using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompleteTakeover.Service
{
    using Domain;
    using Repository;

    internal class PlayerDataService : Service<PlayerData, string, IPlayerDataRepository>, IPlayerDataService
    {
        public override IPlayerDataRepository Repository => _unitOfWork.PlayerDataRepository;

        public PlayerDataService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
        {

        }

        public PlayerData Login(string username, byte[] password)
        {
            return Repository.Login(username, password);
        }
    }
}
