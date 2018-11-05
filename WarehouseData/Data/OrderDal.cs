﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Warehouse.Core.DataAccess.EntityFramework;
using Warehouse.Data.Interface;
using Warehouse.Entities;
using System.Linq.Expressions;

namespace Warehouse.Data.Data
{
    public class OrderDal : EntityRepositoryBase<Order, WarehouseContext>, IOrderDal
    {

    }
}
