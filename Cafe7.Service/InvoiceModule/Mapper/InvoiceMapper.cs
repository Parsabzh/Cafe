using System.Collections.Generic;
using Cafe7.Model.Invoice;
using Cafe7.Service.InvoiceModule.ViewModels;

namespace Cafe7.Service.InvoiceModule.Mapper
{
   public static class InvoiceMapper
    {
        //public static InvoiceItemModel ToModel(this InvoiceItemViewModel entity)
        //{
        //    return AutoMapper.Mapper.Map<InvoiceItemViewModel, InvoiceItemModel>(entity);
        //}

        //public static InvoiceItemViewModel ToViewModel(this InvoiceItemModel entity)
        //{
        //    return AutoMapper.Mapper.Map<InvoiceItemModel, InvoiceItemViewModel>(entity);
        //}

        public static InvoiceViewModel ToViewModel(this InvoiceModel entity)
        {
          return  AutoMapper.Mapper.Map<InvoiceModel, InvoiceViewModel>(entity);
        }
        public static InvoiceModel ToModel(this InvoiceViewModel entity)
        {
            return AutoMapper.Mapper.Map<InvoiceViewModel, InvoiceModel>(entity);
        }

        public static IEnumerable<InvoiceViewModel> ToViewModel(this IEnumerable<InvoiceModel> entity)
        {
            return AutoMapper.Mapper.Map<IEnumerable<InvoiceModel>, IEnumerable<InvoiceViewModel>>(entity);
        }
        public static IEnumerable<InvoiceModel> ToModel(this IEnumerable<InvoiceViewModel> entity)
        {
            return AutoMapper.Mapper.Map<IEnumerable<InvoiceViewModel>, IEnumerable<InvoiceModel>>(entity);
        }

        //public static IEnumerable<InvoiceItemViewModel> ToViewModel(this IEnumerable<InvoiceItemModel> entity)
        //{
        //    return AutoMapper.Mapper.Map<IEnumerable<InvoiceItemModel>, IEnumerable<InvoiceItemViewModel>>(entity);
        //}

        //public static IEnumerable<InvoiceItemModel> ToModel(this IEnumerable<InvoiceItemViewModel> entity)
        //{
        //    return AutoMapper.Mapper.Map<IEnumerable<InvoiceItemViewModel>, IEnumerable<InvoiceItemModel>>(entity);
        //}

    }
}
