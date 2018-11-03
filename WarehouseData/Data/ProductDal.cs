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
    public class ProductDal : EntityRepositoryBase<Product, WarehouseContext>, IProductDal
    {
        public override List<Product> GetList(Expression<Func<Product, bool>> filter = null)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<Product>().Include(p=>p.Category).ToList()
                    : context.Set<Product>().Include(p => p.Category).Where(filter).ToList();
            }
        }

        public List<Product> GetNewProducts()
        {
            using (var context = new WarehouseContext())
            {
                return context.Set<Product>().Include(p => p.Category).OrderByDescending(p=>p.Id).Take(8).ToList();
            }
        }

        public IQueryable<Product> SortList(IQueryable<Product> entities, Expression<Func<Product, dynamic>> sorting = null, ENUM.SORT_TYPE sortType = ENUM.SORT_TYPE.Descending)
        {
            using (var context = new WarehouseContext())
            {
                return sortType == ENUM.SORT_TYPE.Ascending
                    ? entities.OrderBy(sorting)
                    : entities.OrderByDescending(sorting);
            }
        }


    }
}
