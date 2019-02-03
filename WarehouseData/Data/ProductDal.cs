using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Warehouse.Core.DataAccess.EntityFramework;
using Warehouse.Data.Interface;
using Warehouse.Entities;
using System.Linq.Expressions;
using Warehouse.Common;
using System.Globalization;

namespace Warehouse.Data.Data
{
    public class ProductDal : EntityRepositoryBase<Product, WarehouseContext>, IProductDal
    {
        public override Product GetFirst(Expression<Func<Product, bool>> filter)
        {
            using (var context = new WarehouseContext())
            {
                return context.Set<Product>().Include(p => p.Category).Include(p => p.ImagesProducts).Include(p => p.ProductTranslations).Where(p => p.Deleted == false).FirstOrDefault(filter);
            }
        }
        public override Product GetSingle(Expression<Func<Product, bool>> filter)
        {
            using (var context = new WarehouseContext())
            {
                return context.Set<Product>().Include(p => p.Category).Include(p => p.ImagesProducts).Include(p => p.ProductTranslations).Where(p => p.Deleted == false).SingleOrDefault(filter);
            }
        }
        public override List<Product> GetList(Expression<Func<Product, bool>> filter = null)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<Product>().Include(p => p.OrderDetails).Include(p => p.Category).Include(p => p.ImagesProducts).Include(p => p.ProductTranslations).Where(p => p.Deleted == false).ToList()
                    : context.Set<Product>().Include(p => p.OrderDetails).Include(p => p.Category).Include(p => p.ImagesProducts).Include(p => p.ProductTranslations).Where(p => p.Deleted == false).Where(filter).ToList();
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

        public override int Count(Expression<Func<Product, bool>> filter)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<Product>().Where(p => p.Deleted == false).Count()
                    : context.Set<Product>().Where(p => p.Deleted == false).Count(filter);
            }
        }

        public override void Delete(Product entity)
        {
            using (var context = new WarehouseContext()) {
                entity.Deleted = true;
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void CreateTranslation(ProductTranslation productTranslation)
        {
            using (var context = new WarehouseContext())
            {
                context.ProductTranslations.Add(productTranslation);
                context.SaveChanges();
            }
        }

        public void EditTranslation(ProductTranslation productTranslation)
        {
            using (var context = new WarehouseContext())
            {
                context.Entry(productTranslation).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
