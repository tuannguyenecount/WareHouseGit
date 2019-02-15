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
    public class ArticleTranslationDal : EntityRepositoryBase<ArticleTranslation, WarehouseContext>, IArticleTranslationDal
    {

    }
}
