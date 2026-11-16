using System.Collections.Generic;
using System.Linq;
using Cafe7.Model.Invoice;
using Cafe7.Service.CustomerModule.Mapper;
using Cafe7.Service.InvoiceItemModule.ViewModel;
using Cafe7.Service.InvoiceModule.Mapper;

namespace Cafe7.Service.InvoiceItemModule.Mapper
{
    public static class InvoiceItemMapper
    {
        public static InvoiceItemViewModel ToViewModel(this InvoiceItemModel entity)
        {
            if (entity == null)
                return null;

            return new InvoiceItemViewModel
            {
                Id = entity.Id,
                InvoiceId = entity.InvoiceId,
                CustomerId = entity.CustomerId,
                Price = entity.Price,
                Qty = entity.Qty,
                PName = entity.PName,
                Customer = entity.Customer.ToViewModel(),
                Invoice = entity.Invoice.ToViewModel()
            };
        }

        public static InvoiceItemModel ToModel(this InvoiceItemViewModel entity)
        {
            if (entity == null)
                return null;

            return new InvoiceItemModel
            {
                Id = entity.Id,
                InvoiceId = entity.InvoiceId,
                CustomerId = entity.CustomerId,
                Price = entity.Price,
                Qty = entity.Qty,
                PName = entity.PName,
                Customer = entity.Customer.ToModel(),
                Invoice = entity.Invoice.ToModel()
            };
        }

        public static IEnumerable<InvoiceItemViewModel> ToViewModel(this IEnumerable<InvoiceItemModel> entity)
        {
            return entity?.Select(item => item.ToViewModel()).ToList() ?? new List<InvoiceItemViewModel>();
        }

        public static IEnumerable<InvoiceItemModel> ToModel(this IEnumerable<InvoiceItemViewModel> entity)
        {
            return entity?.Select(item => item.ToModel()).ToList() ?? new List<InvoiceItemModel>();
        }
    }
}
