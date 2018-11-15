using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;

namespace Warehouse.Entities
{
    public partial class Article: IEntity
    {
        public int Id { get; set; }

        [StringLength(256)]
        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public Nullable<DateTime> DateCreated { get; set; }

        public Nullable<int> OrderNum { get; set; }

        [DefaultValue(true)]
        public Nullable<bool> Display { get; set; }

        [StringLength(256)]
        [Required]
        public string Alias { get; set; }
    }
}
