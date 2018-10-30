namespace Warehouse.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class News
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [StringLength(300)]
        public string Image { get; set; }

        [StringLength(500)]
        public string Introduce { get; set; }

        public string Content { get; set; }

        [StringLength(128)]
        public string Poster { get; set; }

        [Required]
        [StringLength(256)]
        public string Alias_SEO { get; set; }

        public int View { get; set; }

        public DateTime? DateSubmitted { get; set; }

        public bool Status { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}
