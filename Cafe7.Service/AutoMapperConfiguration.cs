using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cafe7.Model.Customer;
using Cafe7.Model.Invoice;
using Cafe7.Model.Product;
using Cafe7.Service.CustomerModule.ViewModel;
using Cafe7.Service.InvoiceItemModule.ViewModel;
using Cafe7.Service.InvoiceModule.ViewModels;
using Cafe7.Service.ProductModule.ViewModel;

namespace Cafe7.Service
{
   public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ProductModel, ProductViewModel>();
                cfg.CreateMap<ProductViewModel, ProductModel>();
                cfg.CreateMap<CustomerModel, CustomerViewModel>();
                cfg.CreateMap<CustomerViewModel, CustomerModel>();
                cfg.CreateMap<InvoiceModel, InvoiceViewModel>();
                cfg.CreateMap<InvoiceViewModel, InvoiceModel>();
                cfg.CreateMap<InvoiceItemModel, InvoiceItemViewModel>();
                cfg.CreateMap<InvoiceItemViewModel, InvoiceItemModel>();
                cfg.CreateMap<ScoreModel, ScoreViewModel>();
                cfg.CreateMap<ScoreViewModel, ScoreModel>();
            });
        }
    }
}
