using System.Collections.Generic;
using System.Linq;
using Cafe7.Model.Invoice;
using Cafe7.Service.InvoiceModule.ViewModels;

namespace Cafe7.Service.InvoiceModule.Mapper
{
    public static class InvoiceMapper
    {
        public static InvoiceViewModel ToViewModel(this InvoiceModel entity)
        {
            if (entity == null)
                return null;

            return new InvoiceViewModel
            {
                Id = entity.Id,
                Date = entity.Date,
                TableNumber = entity.TableNumber,
                Total = entity.Total,
                InvoiceNumber = entity.InvoiceNumber
            };
        }

        public static InvoiceModel ToModel(this InvoiceViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new InvoiceModel();
            entity.CopyTo(model);
            return model;
        }

        public static void CopyTo(this InvoiceViewModel source, InvoiceModel target)
        {
            target.Id = source.Id;
            target.Date = source.Date;
            target.TableNumber = source.TableNumber;
            target.Total = source.Total;
            target.InvoiceNumber = source.InvoiceNumber;
        }

        public static IEnumerable<InvoiceViewModel> ToViewModel(this IEnumerable<InvoiceModel> entity)
        {
            return entity?.Select(item => item.ToViewModel()).ToList() ?? new List<InvoiceViewModel>();
        }

        public static IEnumerable<InvoiceModel> ToModel(this IEnumerable<InvoiceViewModel> entity)
        {
            return entity?.Select(item => item.ToModel()).ToList() ?? new List<InvoiceModel>();
        }
    }
}
