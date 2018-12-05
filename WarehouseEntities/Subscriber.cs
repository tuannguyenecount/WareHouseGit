namespace Warehouse.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Warehouse.Core.Entities;

    [Table("Subscriber")]
    public partial class Subscriber : IEntity
    {
        [Key]
        [StringLength(256)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Ngày đăng ký")]
        public DateTime DateSubscriber { get; set; }
    }
}
