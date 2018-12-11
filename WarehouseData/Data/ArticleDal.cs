using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.DataAccess.EntityFramework;
using Warehouse.Data.Interface;
using Warehouse.Entities;
using System.Data.Entity;
namespace Warehouse.Data.Data
{
    public class ArticleDal : EntityRepositoryBase<Article, WarehouseContext>, IArticleDal
    {
        public override List<Article> GetList(Expression<Func<Article, bool>> filter = null)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<Article>().Where(a => a.Deleted == false).ToList()
                    : context.Set<Article>().Where(a => a.Deleted == false).Where(filter).ToList();
            }
        }
        public override Article GetSingle(Expression<Func<Article, bool>> filter)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<Article>().Where(a => a.Deleted == false).SingleOrDefault()
                    : context.Set<Article>().Where(a => a.Deleted == false).SingleOrDefault(filter);
            }
        }

        public override Article GetFirst(Expression<Func<Article, bool>> filter)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<Article>().Where(a => a.Deleted == false).FirstOrDefault()
                    : context.Set<Article>().Where(a => a.Deleted == false).FirstOrDefault(filter);
            }
        }
        public override int Count(Expression<Func<Article, bool>> filter)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<Article>().Where(a => a.Deleted == false).Count()
                    : context.Set<Article>().Where(a => a.Deleted == false).Count(filter);
            }
        } 

        public override void Delete(Article entity)
        {
            using (var context = new WarehouseContext()) {
                entity.Deleted = true;
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
