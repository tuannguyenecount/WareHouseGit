using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Warehouse.Entities;

namespace Warehouse.Models
{
    public class NewsListViewModel
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Image { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Title { get; set; }
        public string Introduce { get; set; }
    }
}