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
        public override List<News> GetList(Expression<Func<News, bool>> filter = null)
        {
            using (var context = new WarehouseContext())
            {
                return context.Set<News>().Include(n => n.AspNetUser).Where(filter).ToList();
            }
        }

        public List<News> GetNews(int take = 8)
        {
            using (var context = new WarehouseContext())
            {
                return context.Set<News>().Include(n => n.AspNetUser).Where(n=>n.Status== true).OrderByDescending(n => n.Id).Take(8).ToList();
            }
        }
    }
}
