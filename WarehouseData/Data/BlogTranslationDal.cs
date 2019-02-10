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
    public class BlogTranslationDal : EntityRepositoryBase<BlogTranslation, WarehouseContext>, IBlogTranslationDal
    {
        public override List<BlogTranslation> GetList(Expression<Func<BlogTranslation, bool>> filter = null)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<BlogTranslation>().Include(b => b.Blog).Where(b => b.Blog != null && b.Blog.Deleted == false).ToList()
                    : context.Set<BlogTranslation>().Include(b => b.Blog).Where(b => b.Blog != null && b.Blog.Deleted == false).Where(filter).ToList();
            }
        }
        public override BlogTranslation GetSingle(Expression<Func<BlogTranslation, bool>> filter)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<BlogTranslation>().Include(b => b.Blog).Where(b => b.Blog != null && b.Blog.Deleted == false).SingleOrDefault()
                    : context.Set<BlogTranslation>().Include(b => b.Blog).Where(b => b.Blog != null && b.Blog.Deleted == false).SingleOrDefault(filter);
            }
        }

        public override BlogTranslation GetFirst(Expression<Func<BlogTranslation, bool>> filter)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<BlogTranslation>().Include(b => b.Blog).Where(b => b.Blog != null && b.Blog.Deleted == false).FirstOrDefault()
                    : context.Set<BlogTranslation>().Include(b => b.Blog).Where(b => b.Blog != null && b.Blog.Deleted == false).FirstOrDefault(filter);
            }
        }
    }
}
