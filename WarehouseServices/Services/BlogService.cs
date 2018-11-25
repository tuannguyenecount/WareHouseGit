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
    public class BlogService : IBlogService
    {
        IBlogDal _blogDal;

        public BlogService(IBlogDal blogDal)
        {
            _blogDal = blogDal;
        }

        public List<Blog> GetAll()
        {
            return _blogDal.GetList();
        }

        public List<Blog> GetListByDisplay(bool Display)
        {
            return _blogDal.GetList(b => b.Display == Display);
        }

        public Blog GetByAlias(string alias)
        {
            return _blogDal.GetSingle(b=>b.Alias == alias);
        }

        public Blog GetById(int Id)
        {
            return _blogDal.GetSingle(b => b.Id == Id);
        }

        public bool CheckUniqueTitle(string Title)
        {
            return _blogDal.GetFirst(b => b.Title == Title) == null;
        }

        public bool CheckUniqueAlias(string Alias)
        {
            return _blogDal.GetFirst(b => b.Alias == Alias) == null;
        }

        public void Add(Blog blog)
        {
            _blogDal.Add(blog);
        }

        public void Update(Blog blog)
        {
            _blogDal.Update(blog);
        }

        public void Delete(int Id)
        {
            _blogDal.Delete(new Blog() { Id = Id });
        }
    }
}
