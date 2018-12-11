using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Entities;

namespace Warehouse.Services.Interface
{
    public interface IArticleService
    {
        List<Article> GetAll();
        List<Article> GetListByDisplay(bool? Display);
        Article GetById(int Id);
        Article GetByAlias(string alias);
        bool CheckUniqueTitle(string Title);
        bool CheckUniqueAlias(string Alias);
        void Add(Article blog);
        void Update(Article blog);
        void Delete(int Id);
    }
}
