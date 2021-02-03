using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain.Buildings;
using CompleteTakeover.Repository.Buildings;

namespace CompleteTakeover.Service.Buildings
{
    public interface IMainHallService : IBuildingInstanceService<MainHallData, IMainHallRepository>
    {
    }
}
