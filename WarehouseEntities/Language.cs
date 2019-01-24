using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;

namespace Warehouse.Entities
{
    [Table("Language")]
    public partial class Language: IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Use { get; set; }

        public virtual ICollection<ProductTranslation> ProductTranslations { get; set; }
        public virtual ICollection<CategoryTranslation> CategoryTranslations { get; set; }
        public virtual ICollection<ArticleTranslation> ArticleTranslations { get; set; }
        public virtual ICollection<BlogTranslation> BlogTranslations { get; set; }
        public virtual ICollection<SlideTranslation> SlideTranslations { get; set; }

    }
}
