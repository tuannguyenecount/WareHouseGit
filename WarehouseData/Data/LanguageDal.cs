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
    public class LanguageDal : EntityRepositoryBase<Language, WarehouseContext>, ILanguageDal
    {
        public override List<Language> GetList(Expression<Func<Language, bool>> filter = null)
        {
            using (var context = new WarehouseContext())
            {
                return filter == null
                    ? context.Set<Language>().Include(p => p.ProductTranslations).Include(p => p.CategoryTranslations).Include(p => p.BlogTranslations).Include(p => p.ArticleTranslations).Include(p=>p.SlideTranslations).ToList()
                    : context.Set<Language>().Include(p => p.ProductTranslations).Include(p => p.CategoryTranslations).Include(p => p.BlogTranslations).Include(p => p.ArticleTranslations).Include(p => p.SlideTranslations).Where(filter).ToList();
            }
        }
    }
}
