using System.Collections.Generic;
using Cafe7.Model.Invoice;
using Cafe7.Service.InvoiceItemModule.ViewModel;
using Cafe7.Service.InvoiceModule.ViewModels;

namespace Cafe7.Service.InvoiceItemModule.Mapper
{
   public static class InvoiceItemMapper
    {
        //public static InvoiceItemItemModel ToModel(this InvoiceItemItemViewModel entity)
        //{
        //    return AutoMapper.Mapper.Map<InvoiceItemItemViewModel, InvoiceItemItemModel>(entity);
        //}

        //public static InvoiceItemItemViewModel ToViewModel(this InvoiceItemItemModel entity)
        //{
        //    return AutoMapper.Mapper.Map<InvoiceItemItemModel, InvoiceItemItemViewModel>(entity);
        //}

        public static InvoiceItemViewModel ToViewModel(this InvoiceItemModel entity)
        {
          return  AutoMapper.Mapper.Map<InvoiceItemModel, InvoiceItemViewModel>(entity);
        }
        public static InvoiceItemModel ToModel(this InvoiceItemViewModel entity)
        {
            return AutoMapper.Mapper.Map<InvoiceItemViewModel, InvoiceItemModel>(entity);
        }

        public static IEnumerable<InvoiceItemViewModel> ToViewModel(this IEnumerable<InvoiceItemModel> entity)
        {
            return AutoMapper.Mapper.Map<IEnumerable<InvoiceItemModel>, IEnumerable<InvoiceItemViewModel>>(entity);
        }
        public static IEnumerable<InvoiceItemModel> ToModel(this IEnumerable<InvoiceItemViewModel> entity)
        {
            return AutoMapper.Mapper.Map<IEnumerable<InvoiceItemViewModel>, IEnumerable<InvoiceItemModel>>(entity);
        }

        //public static IEnumerable<InvoiceItemItemViewModel> ToViewModel(this IEnumerable<InvoiceItemItemModel> entity)
        //{
        //    return AutoMapper.Mapper.Map<IEnumerable<InvoiceItemItemModel>, IEnumerable<InvoiceItemItemViewModel>>(entity);
        //}

        //public static IEnumerable<InvoiceItemItemModel> ToModel(this IEnumerable<InvoiceItemItemViewModel> entity)
        //{
        //    return AutoMapper.Mapper.Map<IEnumerable<InvoiceItemItemViewModel>, IEnumerable<InvoiceItemItemModel>>(entity);
        //}

    }
}
