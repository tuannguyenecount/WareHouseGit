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

namespace Warehouse.Data.Data
{
    public class CategoryDal : EntityRepositoryBase<Category, WarehouseContext>, ICategoryDal
    {
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
