using System.Collections.Generic;
using System.Linq;
using Cafe7.Model.Product;
using Cafe7.Service.ProductModule.ViewModel;

namespace Cafe7.Service.ProductModule.Mapper
{
    public static class ProductMapper
    {
        public static ProductViewModel ToViewModel(this ProductModel entity)
        {
            if (entity == null)
                return null;

            return new ProductViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                Qty = entity.Qty,
                Type = entity.Type
            };
        }

        public static ProductModel ToModel(this ProductViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new ProductModel();
            entity.CopyTo(model);
            return model;
        }

        public static void CopyTo(this ProductViewModel source, ProductModel target)
        {
            target.Id = source.Id;
            target.Name = source.Name;
            target.Price = source.Price;
            target.Qty = source.Qty;
            target.Type = source.Type;
        }

        public static IEnumerable<ProductViewModel> ToViewModel(this IEnumerable<ProductModel> entity)
        {
            return entity?.Select(item => item.ToViewModel()).ToList() ?? new List<ProductViewModel>();
        }

        public static IEnumerable<ProductModel> ToModel(this IEnumerable<ProductViewModel> entity)
        {
            return entity?.Select(item => item.ToModel()).ToList() ?? new List<ProductModel>();
        }
    }
}
