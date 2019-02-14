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
    public class SlideDal : EntityRepositoryBase<Slide, WarehouseContext>, ISlideDal
    {
        public override int Count(Expression<Func<Slide, bool>> filter)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<Slide>().Where(s => s.Deleted == false).Count()
                    : context.Set<Slide>().Where(s => s.Deleted == false).Count(filter);
            }
        }
        public override List<Slide> GetList(Expression<Func<Slide, bool>> filter = null)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<Slide>().Include(x => x.SlideTranslations).Where(s => s.Deleted == false).ToList()
                    : context.Set<Slide>().Include(x => x.SlideTranslations).Where(s => s.Deleted == false).Where(filter).ToList();
            }
        }
        public override Slide GetFirst(Expression<Func<Slide, bool>> filter)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<Slide>().Include(x => x.SlideTranslations).Where(p => p.Deleted == false).FirstOrDefault()
                    : context.Set<Slide>().Include(x => x.SlideTranslations).Where(s => s.Deleted == false).FirstOrDefault(filter);
            }
        }
        public override Slide GetSingle(Expression<Func<Slide, bool>> filter)
        {
            using (var context = new WarehouseContext()) {
                return filter == null
                    ? context.Set<Slide>().Include(x => x.SlideTranslations).Where(p => p.Deleted == false).SingleOrDefault()
                    : context.Set<Slide>().Include(x => x.SlideTranslations).Where(s => s.Deleted == false).SingleOrDefault(filter);
            }
        }
        public override void Delete(Slide entity)
        {
            using (var context = new WarehouseContext()) {
                entity.Deleted = true;
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void CreateTranslation(SlideTranslation slideTranslation)
        {
            using (var context = new WarehouseContext())
            {
                context.SlideTranslations.Add(slideTranslation);
                context.SaveChanges();
            }
        }

        public void EditTranslation(SlideTranslation slideTranslation)
        {
            using (var context = new WarehouseContext())
            {
                context.Entry(slideTranslation).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteTranslation(int SlideId, string LanguageId)
        {
            using (var context = new WarehouseContext())
            {
                SlideTranslation slideTranslation = context.SlideTranslations.Find(SlideId, LanguageId);
                context.SlideTranslations.Remove(slideTranslation);
                context.SaveChanges();
            }
        }
    }
}
