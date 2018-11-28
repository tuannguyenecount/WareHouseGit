using System;
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
        public override Order GetSingle(Expression<Func<Order, bool>> filter)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<Order>().Include(p => p.OrderDetails).SingleOrDefault()
                    : context.Set<Order>().Include(p => p.OrderDetails).SingleOrDefault(filter);
            }
        }
        public override List<Order> GetList(Expression<Func<Order, bool>> filter = null)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<Order>().Include(p => p.OrderDetails).ToList()
                    : context.Set<Order>().Include(p => p.OrderDetails).Where(filter).ToList();
            }
        }
       

    }
}
