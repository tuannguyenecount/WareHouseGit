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
                return context.Set<News>().Include(n => n.AspNetUser).Where(n => n.Deleted == false).Where(filter).ToList();
            }
        }

        public override News GetFirst(Expression<Func<News, bool>> filter)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<News>().Include(n=>n.AspNetUser).Where(n => n.Deleted == false).FirstOrDefault()
                    : context.Set<News>().Include(n => n.AspNetUser).Where(n => n.Deleted == false).FirstOrDefault(filter);
            }
        }
        public override News GetSingle(Expression<Func<News, bool>> filter)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<News>().Include(n => n.AspNetUser).Where(n => n.Deleted == false).SingleOrDefault()
                    : context.Set<News>().Include(n => n.AspNetUser).Where(n => n.Deleted == false).SingleOrDefault(filter);
            }
        }

        public override int Count(Expression<Func<News, bool>> filter)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<News>().Where(n => n.Deleted == false).Count()
                    : context.Set<News>().Where(n => n.Deleted == false).Count(filter);
            }
        }

        public List<News> GetNews(int take = 8)
        {
            using (var context = new WarehouseContext())
            {
                return context.Set<News>().Include(n => n.AspNetUser).Where(n=>n.Status == true && n.Deleted == false).OrderByDescending(n => n.Id).Take(8).ToList();
            }
        }

        public override void Delete(News entity)
        {
            using (var context = new WarehouseContext()) {
                entity.Deleted = true;
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
