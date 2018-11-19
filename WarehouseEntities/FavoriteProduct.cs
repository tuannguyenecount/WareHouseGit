using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;

namespace Warehouse.Entities
{
    public class FavoriteProduct: IEntity
    {
        [Key]
        [Column(Order = 1)]
        public int ProductId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string AspNetUserId { get; set; }

        public Nullable<DateTime> FavoriteDate { get; set; }

        public virtual Product Product { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}
