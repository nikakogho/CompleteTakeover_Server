﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain;
using CompleteTakeover.Repository;

namespace CompleteTakeover.Service
{
    public interface IConstructionDataService : IService<ConstructionData, int, IConstructionDataRepository>
    {

    }
}
