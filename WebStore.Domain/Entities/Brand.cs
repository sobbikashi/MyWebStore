using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    //[Table("Brands")] так можно назначить имя таблице
    public class Brand: NamedEntity, IOrderedEntity //Brands
    {
        //[Column("BrandOrder")] назначение колонки таблицы ручками
        public int Order { get; set; }   
        public ICollection<Product> Products { get; set; } //навигационное свойство
    }    
}
