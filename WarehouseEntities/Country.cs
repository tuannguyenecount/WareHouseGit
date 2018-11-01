namespace Warehouse.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Warehouse.Core.Entities;

    [Table("Country")]
    public partial class Country : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Country()
        {
            Provinces = new HashSet<Province>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CountryCode { get; set; }

        [StringLength(100)]
        public string CommonName { get; set; }

        [StringLength(100)]
        public string FormalName { get; set; }

        [StringLength(100)]
        public string CountryType { get; set; }

        [StringLength(100)]
        public string CountrySubType { get; set; }

        [StringLength(100)]
        public string Sovereignty { get; set; }

        [StringLength(100)]
        public string Capital { get; set; }

        [StringLength(100)]
        public string CurrencyCode { get; set; }

        [StringLength(100)]
        public string CurrencyName { get; set; }

        [StringLength(100)]
        public string TelephoneCode { get; set; }

        [StringLength(100)]
        public string CountryCode3 { get; set; }

        [StringLength(100)]
        public string CountryNumber { get; set; }

        [StringLength(100)]
        public string InternetCountryCode { get; set; }

        public int? SortOrder { get; set; }

        public bool? IsPublished { get; set; }

        [StringLength(50)]
        public string Flags { get; set; }

        public bool? IsDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Province> Provinces { get; set; }
    }
}
