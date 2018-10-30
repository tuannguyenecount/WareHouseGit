namespace Warehouse.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Slide")]
    public partial class Slide
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string Image { get; set; }

        public byte? Order { get; set; }

        [StringLength(256)]
        public string Title { get; set; }

        public bool Status { get; set; }
    }
}
