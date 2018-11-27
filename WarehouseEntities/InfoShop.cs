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

        [Display(Name = "Tên shop")]
        public string ShopName { get; set; }

        public string Logo { get; set; }

        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }

        public string Zalo { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        public string Fanpage { get; set; }

        [Display(Name = "Bản đồ google map")]
        public string Google_Map { get; set; }

        [Display(Name = "Google Analytics")]
        public string GoogleAnalytics { get; set; }

    }
}
