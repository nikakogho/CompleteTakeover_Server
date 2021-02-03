using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain;

namespace CompleteTakeover.Repository
{
    internal class PlayerDataRepository : Repository<PlayerData, string>, IPlayerDataRepository
    {
        public PlayerDataRepository(DbModel context)
        :base(context)
        {

        }

        public PlayerData Login(string username, byte[] password)
        {
            var player = Get(username);
            if (player == null) return null;
            if (!player.Password.SequenceEqual(password)) return null;
            return player;
        }
    }
}
