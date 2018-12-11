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
    public class BlogDal : EntityRepositoryBase<Blog, WarehouseContext>, IBlogDal
    {
        public override List<Blog> GetList(Expression<Func<Blog, bool>> filter = null)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<Blog>().Include(b => b.User).Where(b => b.Deleted == false).ToList()
                    : context.Set<Blog>().Include(b => b.User).Where(b => b.Deleted == false).Where(filter).ToList();
            }
        }
        public override Blog GetSingle(Expression<Func<Blog, bool>> filter)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<Blog>().Include(b => b.User).Where(b => b.Deleted == false).SingleOrDefault()
                    : context.Set<Blog>().Include(b => b.User).Where(b => b.Deleted == false).SingleOrDefault(filter);
            }
        }

        public override Blog GetFirst(Expression<Func<Blog, bool>> filter)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<Blog>().Include(b => b.User).Where(b => b.Deleted == false).FirstOrDefault()
                    : context.Set<Blog>().Include(b => b.User).Where(b => b.Deleted == false).FirstOrDefault(filter);
            }
        }

        public override int Count(Expression<Func<Blog, bool>> filter)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<Blog>().Where(b => b.Deleted == false).Count()
                    : context.Set<Blog>().Where(b => b.Deleted == false).Count(filter);
            }
        }

        public override void Delete(Blog entity)
        {
            using (var context = new WarehouseContext()) {
                entity.Deleted = true;
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
