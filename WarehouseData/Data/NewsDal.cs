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
    public class NewsDal : EntityRepositoryBase<News, WarehouseContext>, INewsDal
    {
        public List<News> GetNews()
        {
            using (var context = new WarehouseContext())
            {
                return context.Set<News>().Include(n => n.AspNetUser).OrderByDescending(n => n.Id).Take(8).ToList();
            }
        }
    }
}
