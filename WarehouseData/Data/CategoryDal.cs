using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Data.Data;
using Warehouse.Core.DataAccess.EntityFramework;
using Warehouse.Data.Interface;
using Warehouse.Entities;
using Warehouse.Common;
using System.Linq.Expressions;
using System.Data.Entity;

namespace Warehouse.Data.Data
{
    public class CategoryDal : EntityRepositoryBase<Category, WarehouseContext>, ICategoryDal
    {
        public override List<Category> GetList(Expression<Func<Category, bool>> filter = null)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<Category>().Include(c => c.Category1).Include(c => c.Category2).Where(c=> c.Deleted == false).ToList()
                    : context.Set<Category>().Include(c => c.Category1).Include(c => c.Category2).Where(c => c.Deleted == false).Where(filter).ToList();
            }
        }
        public override Category GetFirst(Expression<Func<Category, bool>> filter)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<Category>().Include(c => c.Category1).Include(c => c.Category2).Where(c => c.Deleted == false).FirstOrDefault()
                    : context.Set<Category>().Include(c => c.Category1).Include(c => c.Category2).Where(c => c.Deleted == false).FirstOrDefault(filter);
            }
        }
        public override Category GetSingle(Expression<Func<Category, bool>> filter)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<Category>().Include(c => c.Category1).Include(c => c.Category2).Where(p => p.Deleted == false).SingleOrDefault()
                    : context.Set<Category>().Include(c => c.Category1).Include(c => c.Category2).Where(c => c.Deleted == false).SingleOrDefault(filter);
            }
        }

        public override int Count(Expression<Func<Category, bool>> filter)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<Category>().Where(c => c.Deleted == false).Count()
                    : context.Set<Category>().Where(c => c.Deleted == false).Count(filter);
            }
        }

        public override void Delete(Category entity)
        {
            using (var context = new WarehouseContext()) {
                entity.Deleted = true;
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
