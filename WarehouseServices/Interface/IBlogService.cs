using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Entities;

namespace Warehouse.Services.Interface
{
    public interface IBlogService
    {
        List<Blog> GetAll();
        List<Blog> GetListByDisplay(bool Display);
        Blog GetById(int Id);
        Blog GetByAlias(string alias);
        bool CheckUniqueTitle(string Title);
        bool CheckUniqueAlias(string Alias);
        void Add(Blog blog);
        void Update(Blog blog);
        void Delete(int Id);
    }
}
