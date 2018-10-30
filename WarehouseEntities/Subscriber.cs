namespace Warehouse.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Subscriber")]
    public partial class Subscriber
    {
        [Key]
        [StringLength(256)]
        public string Email { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateSubscriber { get; set; }
    }
}
