using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using System.Linq;
using WebStore.ViewModels;
using System.Collections.Generic;

namespace WebStore.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BrandsViewComponent(IProductData ProductData) => _ProductData = ProductData;
        public IViewComponentResult Invoke() => View(GetBrands());
       public IEnumerable<BrandsViewModel> GetBrands() =>
            _ProductData.GetBrands()
            .OrderBy(b => b.Order).Select(b => new BrandsViewModel
            {
                Id = b.Id,
                Name = b.Name,
            });
    }
}
