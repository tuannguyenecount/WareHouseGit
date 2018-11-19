using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Common;
using Warehouse.Entities;


namespace Warehouse.Services.Interface
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        Category GetById(int Id);
        Category GetByAlias(string alias);
        int CountAll();
        List<Category> GetParents();
        List<Category> GetChilds(int Id);
        bool CheckExistName(string Name);
        bool CheckExistAlias(string Alias);
        void Add(Category category);
        void Update(Category category);
        void Delete(Category category);

    }
}
