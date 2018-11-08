using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;

namespace Warehouse.Entities
{
    public partial class ImagesProduct: IEntity
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [StringLength(310)]
        public string Image { get; set; } 

        public Nullable<int> OrderNum { get; set; }

        public virtual Product Product { get; set; }
    }
}
