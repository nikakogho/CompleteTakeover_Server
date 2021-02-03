using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain;

namespace CompleteTakeover.Repository
{
    public interface IPlayerDataRepository : IRepository<PlayerData, string>
    {
        PlayerData Login(string username, byte[] password);
    }
}
