namespace Warehouse.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Warehouse.Core.Entities;

    [Table("InfoShop")]
    public partial class InfoShop : IEntity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string ShopName { get; set; }

        [StringLength(256)]
        public string Logo { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Zalo { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        public string Address { get; set; }

        [StringLength(256)]
        public string Fanpage { get; set; }

        public string Google_Map { get; set; }

        public string GoogleAnalytics { get; set; }

    }
}
