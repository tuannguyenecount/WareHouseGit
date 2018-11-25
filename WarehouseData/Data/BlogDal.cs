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
                    ? context.Set<Blog>().Include(p => p.User).ToList()
                    : context.Set<Blog>().Include(p => p.User).Where(filter).ToList();
            }
        }
        public override Blog GetSingle(Expression<Func<Blog, bool>> filter)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<Blog>().Include(p => p.User).SingleOrDefault()
                    : context.Set<Blog>().Include(p => p.User).SingleOrDefault(filter);
            }
        }

        public override Blog GetFirst(Expression<Func<Blog, bool>> filter)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<Blog>().Include(p => p.User).FirstOrDefault()
                    : context.Set<Blog>().Include(p => p.User).FirstOrDefault(filter);
            }
        }
    }
}
