using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompleteTakeover.Service
{
    using Domain;
    using Repository;

    public interface IPlayerDataService : IService<PlayerData, string, IPlayerDataRepository>
    {
        PlayerData Login(string username, byte[] password);
    }
}
