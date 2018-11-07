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
                    ? context.Set<Category>().Include(p => p.Category1).Include(p=>p.Category2).ToList()
                    : context.Set<Category>().Include(p => p.Category1).Include(p => p.Category2).Where(filter).ToList();
            }
        }

        public List<Category> SortList(List<Category> entities, ENUM.SORT_TYPE sortType = ENUM.SORT_TYPE.Descending)
        {
            using (var context = new WarehouseContext())
            {
                return sortType == ENUM.SORT_TYPE.Ascending
                    ? entities.OrderBy(c=>c.OrderNum).ToList()
                    : entities.OrderByDescending(c=>c.OrderNum).ToList();
            }
        }
      
    }
}
