using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Domain.Entities
{
    public class Section : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
        public int? ParentId { get; set; }

        //[ForeignKey(nameof(ParentId))]
        //public Section ParentId { get; set; }
        //public ICollection<Product> Products { get; set; }
        //public object Parent { get; set; }
    }
}
