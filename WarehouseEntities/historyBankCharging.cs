namespace Warehouse.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Warehouse.Core.Entities;

    [Table("historyBankCharging")]
    public partial class historyBankCharging : IEntity
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string fullname { get; set; }

        [StringLength(15)]
        public string phone { get; set; }

        [StringLength(256)]
        public string email { get; set; }

        public string transaction_info { get; set; }

        [StringLength(50)]
        public string order_code { get; set; }

        public int? price { get; set; }

        public string payment_id { get; set; }

        [StringLength(10)]
        public string payment_type { get; set; }

        public string error_text { get; set; }

        public string secure_code { get; set; }

        public DateTime date_trans { get; set; }
    }
}
