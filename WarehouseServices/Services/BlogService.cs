using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Warehouse.Data.Interface;
using Warehouse.Entities;
using Warehouse.Services.Interface;

namespace Warehouse.Services.Services
{
    public class BlogService : IBlogService
    {
        IBlogDal _blogDal;
        IBlogTranslationDal _blogTranslationDal;

        public BlogService(IBlogDal blogDal, IBlogTranslationDal blogTranslationDal)
        {
            _blogDal = blogDal;
            _blogTranslationDal = blogTranslationDal;
        }

        public string LanguageId
        {
            get
            {
                return HttpContext.Current.Request.Cookies["lang"].Value;
            }
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
            if (LanguageId == "vi")
                return _blogDal.GetSingle(b => b.Alias == alias);
            else
            {
                int BlogId = _blogTranslationDal.GetSingle(x => x.LanguageId == LanguageId && x.Alias == alias).BlogId;
                return this.GetById(BlogId);      
            }
        }

        public Blog GetById(int Id)
        {
             return _blogDal.GetSingle(b => b.Id == Id);
        }

        public bool CheckUniqueTitle(string Title)
        {
            return _blogDal.GetFirst(b => b.Title == Title) == null && _blogTranslationDal.GetFirst(b => b.Title == Title) == null;
        }

        public bool CheckUniqueAlias(string Alias)
        {
            return _blogDal.GetFirst(b => b.Alias == Alias) == null && _blogTranslationDal.GetFirst(b => b.Alias == Alias) == null;
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
            Blog blog = GetById(Id);
            if (blog != null)
                _blogDal.Delete(blog);
        }
        
        public int CountDisplay()
        {
            return _blogDal.Count(b => b.Display == true);
        }

        public int CountHide()
        {
            return _blogDal.Count(b => b.Display == false);
        }

        public void CreateTranslation(BlogTranslation blogTranslation)
        {
            try
            {
                _blogDal.CreateTranslation(blogTranslation);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void EditTranslation(BlogTranslation blogTranslation)
        {
            try
            {
                _blogDal.EditTranslation(blogTranslation);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteTranslation(int BlogId, string LanguageId)
        {
            try
            {
                _blogDal.DeleteTranslation(BlogId, LanguageId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int CountByTitle(string Title)
        {
            return _blogDal.Count(x => x.Title == Title) + _blogTranslationDal.Count(x => x.Title == Title);
        }
    }
}
