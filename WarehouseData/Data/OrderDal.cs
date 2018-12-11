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
                    ? context.Set<Order>().Include(o => o.OrderDetails).Include(o => o.District).Include(o => o.Ward).Include(o => o.Province).Where(o => o.Deleted == false).SingleOrDefault()
                    : context.Set<Order>().Include(o => o.OrderDetails).Include(o => o.District).Include(o => o.Ward).Include(o => o.Province).Where(o => o.Deleted == false).SingleOrDefault(filter);
            }
        }
        public override Order GetFirst(Expression<Func<Order, bool>> filter)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<Order>().Include(o => o.OrderDetails).Include(o => o.District).Include(o => o.Ward).Include(o => o.Province).Where(o => o.Deleted == false).FirstOrDefault()
                    : context.Set<Order>().Include(o => o.OrderDetails).Include(o => o.District).Include(o => o.Ward).Include(o => o.Province).Where(o => o.Deleted == false).FirstOrDefault(filter);
            }
        }
        public override List<Order> GetList(Expression<Func<Order, bool>> filter = null)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<Order>().Include(o => o.OrderDetails).Include(o => o.District).Include(o => o.Ward).Include(o => o.Province).Where(o => o.Deleted == false).ToList()
                    : context.Set<Order>().Include(o => o.OrderDetails).Include(o => o.District).Include(o => o.Ward).Include(o => o.Province).Where(o => o.Deleted == false).Where(filter).ToList();
            }
        }

        public override int Count(Expression<Func<Order, bool>> filter)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<Order>().Where(o => o.Deleted == false).Count()
                    : context.Set<Order>().Where(o => o.Deleted == false).Count(filter);
            }
        }
        public override void Delete(Order entity)
        {
            using (var context = new WarehouseContext()) {
                entity.Deleted = true;
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

    }
}
