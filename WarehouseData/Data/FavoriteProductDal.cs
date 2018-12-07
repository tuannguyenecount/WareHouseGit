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
    public class FavoriteProductDal : EntityRepositoryBase<FavoriteProduct, WarehouseContext>, IFavoriteProductDal
    {
        public override List<FavoriteProduct> GetList(Expression<Func<FavoriteProduct, bool>> filter = null)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<FavoriteProduct>().Include(p => p.Product).Include(p => p.AspNetUser).ToList()
                    : context.Set<FavoriteProduct>().Include(p => p.Product).Include(p => p.AspNetUser).Where(filter).ToList();
            }
        }
    }
}
