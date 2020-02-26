using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Model.Product;
using Cafe7.Service.ProductModule.ViewModel;

namespace Cafe7.Service.ProductModule.Mapper
{
   public static class ProductMapper
    {
        public static ProductViewModel ToViewModel(this ProductModel entity)
        {
          return  AutoMapper.Mapper.Map<ProductModel, ProductViewModel>(entity);
        }
        public static ProductModel ToModel(this ProductViewModel entity)
        {
            return AutoMapper.Mapper.Map<ProductViewModel, ProductModel>(entity);
        }

        public static IEnumerable<ProductViewModel> ToViewModel(this IEnumerable<ProductModel> entity)
        {
            return AutoMapper.Mapper.Map<IEnumerable<ProductModel>, IEnumerable<ProductViewModel>>(entity);
        }
        public static IEnumerable<ProductModel> ToModel(this IEnumerable<ProductViewModel> entity)
        {
            return AutoMapper.Mapper.Map<IEnumerable<ProductViewModel>, IEnumerable<ProductModel>>(entity);
        }

    }
}
