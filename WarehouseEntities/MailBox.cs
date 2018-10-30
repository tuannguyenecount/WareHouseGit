namespace Warehouse.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MailBox")]
    public partial class MailBox
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(500)]
        public string Subject { get; set; }

        public string Content { get; set; }

        public DateTime DateSend { get; set; }
    }
}
