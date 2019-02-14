namespace Warehouse.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Warehouse.Core.Entities;

    [Table("Slide")]
    public partial class Slide : IEntity
    {
        public int Id { get; set; }

        [StringLength(128)]
        [Display(Name = "Hình")]
        public string Image { get; set; }

        [Display(Name = "Số thứ tự")]
        public byte? Order { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(256)]
        public string Title { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public virtual ICollection<SlideTranslation> SlideTranslations { get; set; }
    }
}
