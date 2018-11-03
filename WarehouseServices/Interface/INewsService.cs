using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warehouse.Entities;

namespace Warehouse.Services.Interface
{
    public interface INewsService
    {

        List<News> GetAll();

        List<News> GetNews();

        News GetById(int id);

        void Add(News news);

        void Update(News news);

        void Delete(int id);

    }
}
