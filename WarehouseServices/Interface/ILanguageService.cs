using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warehouse.Entities;

namespace Warehouse.Services.Interface
{
    public interface ILanguageService
    {
        List<Language> GetAll();

        Language GetById(string Id);

    }
}
