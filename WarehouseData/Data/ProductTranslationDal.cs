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
    public class ProductTranslationDal : EntityRepositoryBase<ProductTranslation, WarehouseContext>, IProductTranslationDal
    {
        public override ProductTranslation GetFirst(Expression<Func<ProductTranslation, bool>> filter)
        {
            using (var context = new WarehouseContext())
            {
                return context.Set<ProductTranslation>().FirstOrDefault(filter);
            }
        }
        public override ProductTranslation GetSingle(Expression<Func<ProductTranslation, bool>> filter)
        {
            using (var context = new WarehouseContext())
            {
                return context.Set<ProductTranslation>().SingleOrDefault(filter);
            }
        }
        public override List<ProductTranslation> GetList(Expression<Func<ProductTranslation, bool>> filter = null)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<ProductTranslation>().ToList()
                    : context.Set<ProductTranslation>().Where(filter).ToList();
            }
        }

    }
}
