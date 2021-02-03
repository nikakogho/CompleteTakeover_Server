using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain;

namespace CompleteTakeover.Repository
{
    public interface IBaseDataRepository : IRepository<BaseData, int>
    {
        BaseData CreateNewHomeBase(string faction);
    }
}
