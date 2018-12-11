using System;
using System.Collections.Generic;
using Warehouse.Entities;
using Warehouse.Services.Interface;
using Warehouse.Data.Interface;
using System.Linq;
namespace Warehouse.Services.Services
{
    public class NewsService : INewsService
    {
        private INewsDal _newsDal;

        public NewsService(INewsDal newsDal)
        {
            _newsDal = newsDal;
        }

        public List<News> GetAll()
        {
            return _newsDal.GetList();
        }

        public News GetById(int id)
        {
            return _newsDal.GetSingle(p => p.Id == id);
        }

        public void Update(News news)
        {
            _newsDal.Update(news);
        }

        public void Add(News news)
        {
            _newsDal.Add(news);
        }

        public void Delete(int Id)
        {
            News news = GetById(Id);
            if (news != null)
                _newsDal.Delete(news);
        }

        public List<News> GetNews(int take)
        {
            return _newsDal.GetNews(take);
        }
    }
}
