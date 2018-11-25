using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Warehouse.Models
{
    public class DetailsBlogViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string DateCreated { get; set; }
        public int LikeCount { get; set; }
        public int ViewCount { get; set; }
        public string Alias { get; set; }
    }
}