using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core.DataAccess;
using Warehouse.Entities;

namespace Warehouse.Data.Interface
{
    public interface ISlideDal : IEntityRepository<Slide>
    {
        void CreateTranslation(SlideTranslation slideTranslation);
        void EditTranslation(SlideTranslation slideTranslation);
        void DeleteTranslation(int SlideId, string LanguageId);
    }
}
