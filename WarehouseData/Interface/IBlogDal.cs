using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.DataAccess;
using Warehouse.Entities;

namespace Warehouse.Data.Interface
{
    public interface IBlogDal : IEntityRepository<Blog>
    {
        void CreateTranslation(BlogTranslation blogTranslation);
        void EditTranslation(BlogTranslation blogTranslation);
        void DeleteTranslation(int BlogId, string LanguageId);
    }
}
