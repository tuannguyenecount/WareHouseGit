using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data.Interface;
using Warehouse.Entities;
using Warehouse.Services.Interface;

namespace Warehouse.Services.Services
{
    public class ArticleService : IArticleService
    {
        readonly IArticleDal _articleDal;

        public ArticleService(IArticleDal articleDal)
        {
            _articleDal = articleDal;
        }

        public List<Article> GetAll()
        {
            return _articleDal.GetList();
        }

        public List<Article> GetListByDisplay(bool? Display = null)
        {
            return _articleDal.GetList(a => a.Display == Display);
        }

        public Article GetByAlias(string alias)
        {
            return _articleDal.Get(a=>a.Alias == alias);
        }

        public Article GetById(int Id)
        {
            return _articleDal.Get(a => a.Id == Id);
        }

    }
}
