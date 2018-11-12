﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core.DataAccess;
using Warehouse.Entities;

namespace Warehouse.Data.Interface
{
    public interface INewsDal : IEntityRepository<News>
    {
        List<News> GetNews(int take = 8);
    }
}
