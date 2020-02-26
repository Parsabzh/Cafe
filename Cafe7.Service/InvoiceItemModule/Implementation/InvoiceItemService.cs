using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml;
using Cafe7.Infrastructure.ExceptionHelper;
using Cafe7.Infrastructure.Localize.InvoiceResource;
using Cafe7.Infrastructure.Messaging;
using Cafe7.Infrastructure.PeopleResource;
using Cafe7.Infrastructure.Service.RequsetResponseMessaging.Response;
using Cafe7.Model;
using Cafe7.Model.Invoice;
using Cafe7.Model.Product;
using Cafe7.Service.CustomerModule.Implementation;
using Cafe7.Service.CustomerModule.Messaging.Request.ScoreRequest;
using Cafe7.Service.CustomerModule.ViewModel;
using Cafe7.Service.InvoiceItemModule.Mapper;
using Cafe7.Service.InvoiceItemModule.Messaging.Request;
using Cafe7.Service.InvoiceItemModule.Messaging.Response;
using Cafe7.Service.InvoiceItemModule.ViewModel;
using Microsoft.SqlServer.Server;
using static System.String;

namespace Cafe7.Service.InvoiceItemModule.Implementation
{
    public class InvoiceItemService
    {

        private DbCafe7Managment _db;
        private readonly ScoreService _scoreService;

        private decimal _point;
        public InvoiceItemService()
        {
            _scoreService = new ScoreService();
            _point = _scoreService.SearchPoint(new PointSearchRequest()).Entity.Point;
        }

        public InvoiceItemInsertResponse Insert(InvoiceItemInsertRequest request)
        {
            _db = new DbCafe7Managment();
            //var point = _scoreService.SearchPoint(new PointSearchRequest());

            try
            {

                var lst = new List<InvoiceItemModel>();
                foreach (var invoiceItemModel in request.Entity)
                {

                    var customer = _db.Customers.SingleOrDefault(m => m.RegistrationNumber == invoiceItemModel.Registration);
                    if (customer == null)
                    {
                        return new InvoiceItemInsertResponse
                        {
                            Message = Format(MessageResource.NotFound, PeopleResource.Customer)
                            ,
                            IsSuccess = false
                        };
                    }

                    var scoreVm = new ScoreViewModel
                    {
                        CustomerId = customer.Id,
                        LastScore = (double)(_point * invoiceItemModel.Price * invoiceItemModel.Qty),
                    };
                    var response = _scoreService.Update(new ScoreUpdateRequest { Entity = scoreVm });
                    if (!response.IsSuccess)
                        return new InvoiceItemInsertResponse
                        {
                            Message = response.Message,
                            IsSuccess = response.IsSuccess
                        };
                    var invoiceItem = new InvoiceItemModel
                    {
                        InvoiceId = request.Invoice,
                        PName = invoiceItemModel.PName,
                        Price = decimal.Parse(invoiceItemModel.Price.ToString("####.00")),
                        Qty = invoiceItemModel.Qty,
                        CustomerId = customer.Id
                    };
                    _db.InvoiceItems.Add(invoiceItem);
                    _db.SaveChanges();

                    lst.Add(invoiceItem);
                }

                return new InvoiceItemInsertResponse
                {
                    Entity = lst.ToViewModel(),
                    Message = MessageResource.InsertSuccess,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new InvoiceItemInsertResponse
                {
                    Message = ex.GetMessages(),
                    IsSuccess = false
                };
            }
        }

        public InvoiceItemGetByInvoiceIdResponse GetByInvoiceId(InvoiceItemGetByInvoiceIdRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();

                var model = _db.InvoiceItems.Where(m => m.InvoiceId == request.InvoiceId);

                var newModel = new List<InvoiceItemViewModel>();
                foreach (var invoiceItemModel in model)
                {
                    _db = new DbCafe7Managment();
                    var registration = _db.Customers.SingleOrDefault(m => m.Id == invoiceItemModel.CustomerId).RegistrationNumber;
                    newModel.Add(new InvoiceItemViewModel()
                    {
                        PName = invoiceItemModel.PName,
                        Price = decimal.Parse(invoiceItemModel.Price.ToString("####.00")),
                        InvoiceId = invoiceItemModel.InvoiceId,
                        Id = invoiceItemModel.Id,
                        Qty = invoiceItemModel.Qty,
                        Registration = registration
                    });
                }


                return new InvoiceItemGetByInvoiceIdResponse
                {
                    Entity = newModel.AsEnumerable(),
                    Message = MessageResource.SearchSuccess,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                return new InvoiceItemGetByInvoiceIdResponse
                {
                    IsSuccess = false,
                    Message = ex.GetMessages()
                };
            }
        }

        public InvoiceItemDeleteResponse Delete(InvoiceItemDeleteRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                var model = _db.InvoiceItems.Where(m => m.InvoiceId == request.Id);

                foreach (var invoiceItemModel in model)
                {
                    var responseScore = _scoreService.Delete(new ScoreDeleteRequest
                    {
                        CustomerId = invoiceItemModel.CustomerId,
                        ScoreDecrease = (invoiceItemModel.Price * _point * invoiceItemModel.Qty).ToString("N0")
                    });

                    _db = new DbCafe7Managment();
                    _db.InvoiceItems.Remove(invoiceItemModel);
                    _db.SaveChanges();

                }

                return new InvoiceItemDeleteResponse()
                {

                    Message = MessageResource.DeletSuccess,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                return new InvoiceItemDeleteResponse()
                {
                    IsSuccess = false,
                    Message = ex.GetMessages()
                };
            }
        }

        public InvoiceItemGetByDateResponse GetByDate(InvoiceItemGetByDateRequest request)
        {
            try
            {
                var x = 0;
                decimal total = 0;
                var lstProduct = new List<InvoiceItemReportViewModel>();
                _db = new DbCafe7Managment();
                if (!_db.Invoices.Any(m => m.Date == request.Date))
                    return new InvoiceItemGetByDateResponse
                    {
                        Message = Format(MessageResource.NotFound, InvoiceResource.Invoice),
                        IsSuccess = false
                    };


                var productModel = _db.Products;
                foreach (var product in productModel)
                {
                    _db = new DbCafe7Managment();
                    var invoiceModel = _db.Invoices.Where(d => d.Date == request.Date);
                    foreach (var invoice in invoiceModel)
                    {
                        _db = new DbCafe7Managment();
                        var qty = 0;
                        if (_db.InvoiceItems.Any(m => m.InvoiceId == invoice.Id))
                        {
                            if (!_db.InvoiceItems.Where(m => m.InvoiceId == invoice.Id).Any(c => c.PName == product.Name)) continue;
                           var qtyList = _db.InvoiceItems.Where(m => m.InvoiceId == invoice.Id && m.PName == product.Name);
                            var q = 0;
                            foreach (var qtyItem in qtyList)
                            {
                                q = q + qtyItem.Qty;
                            }

                            qty = q;
                        }
                        x = qty + x;

                    }

                    if (x == 0) continue;

                    lstProduct.Add(new InvoiceItemReportViewModel
                    {
                        PName = product.Name,
                        Price = decimal.Parse(product.Price.ToString("####.00")),
                        Qty = x,

                    });
                    x = 0;
                }
                _db = new DbCafe7Managment();
                var invoiceTotal = _db.Invoices.Where(d => d.Date == request.Date);
                foreach (var item in invoiceTotal)
                {
                    total = decimal.Parse(item.Total) + total;
                }
                return new InvoiceItemGetByDateResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.SearchSuccess,
                    Entity = lstProduct.ToList(),
                    TotalOff = total.ToString("####.00")
                };
            }
            catch (Exception ex)
            {
                return new InvoiceItemGetByDateResponse
                {
                    Message = ex.GetMessages(),
                    IsSuccess = false
                };
            }

        }

        //baraye hazf customer
        public InvoiceItemDeleteByCustomerIdResponse DeleteByCustomerId(InvoiceItemDeleteByCustomerIdRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                var invoiceItemModels = _db.InvoiceItems.Where(m => m.CustomerId == request.Id);
                foreach (var invoiceItemModel in invoiceItemModels)
                {
                    _db.InvoiceItems.Remove(invoiceItemModel);

                }
                _db.SaveChanges();
                return new InvoiceItemDeleteByCustomerIdResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.DeletSuccess
                };

            }
            catch (Exception ex)
            {
                return new InvoiceItemDeleteByCustomerIdResponse
                {
                    IsSuccess = false,
                    Message = ex.GetMessages()
                };
            }
        }

        public InvoiceItemUpdateResponse Update(InvoiceItemUpdateRequest request)
        {
            var list = new List<InvoiceItemModel>();
            try
            {
                _db = new DbCafe7Managment();
                var model = _db.InvoiceItems.Where(m => m.InvoiceId == request.Entity.InvoiceId);

                foreach (var invoiceItemModel in model)
                {
                    _db = new DbCafe7Managment();
                    var newModel = _db.InvoiceItems.Find(invoiceItemModel.Id);
                    if (newModel == null) return new InvoiceItemUpdateResponse
                    {
                        Message = Format(MessageResource.NotFound, InvoiceResource.Invoice),
                        IsSuccess = false
                    };
                    _scoreService.Delete(new ScoreDeleteRequest
                    {
                        CustomerId = invoiceItemModel.CustomerId,
                        ScoreDecrease = (_point * invoiceItemModel.Price * invoiceItemModel.Qty).ToString("N0")
                    });
                    _db.InvoiceItems.Remove(newModel);
                    _db.SaveChanges();
                }
                foreach (var item in request.Entity.InvoiceItemViewModels)
                {
                    var customer = _db.Customers.SingleOrDefault(m => m.RegistrationNumber == item.Registration);
                    if (customer == null)
                        return new InvoiceItemUpdateResponse
                        {
                            Message = Format(MessageResource.NotFound, PeopleResource.Customer),
                            IsSuccess = false
                        };
                    var invoiceItemModel = new InvoiceItemModel
                    {
                        InvoiceId = request.Entity.InvoiceId,
                        PName = item.PName,
                        Price = decimal.Parse(item.Price.ToString("####.00")),
                        Qty = item.Qty,
                        CustomerId = customer.Id

                    };
                    _db = new DbCafe7Managment();
                    _db.InvoiceItems.Add(invoiceItemModel);
                    _db.SaveChanges();
                    var response = _scoreService.Update(new ScoreUpdateRequest
                    {
                        Entity = new ScoreViewModel
                        {
                            CustomerId = customer.Id,
                            LastScore = (double)(_point * item.Price * item.Qty)
                        }
                    });
                    if (!response.IsSuccess)
                        return new InvoiceItemUpdateResponse
                        {
                            IsSuccess = false,
                            Message = response.Message
                        };
                    list.Add(invoiceItemModel);
                }
                return new InvoiceItemUpdateResponse
                {
                    Message = MessageResource.UpdateSuccess,
                    Entity = list.ToViewModel(),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new InvoiceItemUpdateResponse
                {
                    Message = ex.GetMessages(),
                    IsSuccess = false
                };
            }
        }

        public InvoiceItemGetForProfileResponse GetForProfile(InvoiceItemGetForProfileRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                var lst = new List<InvoiceItemForProfileViewModel>();
                var customer = _db.Customers.SingleOrDefault(m => m.RegistrationNumber == request.Registartion);
                if (customer == null)
                    return new InvoiceItemGetForProfileResponse
                    {
                        Message = Format(MessageResource.NotFound, PeopleResource.Customer),
                        IsSuccess = false
                    };
                var customerId = _db.InvoiceItems.Any(m => m.CustomerId == customer.Id);
                if (customerId == false)
                    return new InvoiceItemGetForProfileResponse
                    {
                        Message = Format(MessageResource.NotFound, InvoiceResource.Invoice),
                        IsSuccess = false
                    };
                var invoiceItemModels = _db.InvoiceItems.Where(m => m.CustomerId == customer.Id);
                foreach (var invoiceItemModel in invoiceItemModels)
                {
                    _db = new DbCafe7Managment();
                    var invoice = _db.Invoices.Find(invoiceItemModel.InvoiceId);
                    if (invoice == null)
                        return new InvoiceItemGetForProfileResponse
                        {
                            Message = Format(MessageResource.NotFound, InvoiceResource.Invoice),
                            IsSuccess = false
                        };

                    lst.Add(new InvoiceItemForProfileViewModel
                    {
                        Date = invoice.Date,
                        ProductName = invoiceItemModel.PName,
                        Qty = invoiceItemModel.Qty
                    });
                }
                return new InvoiceItemGetForProfileResponse
                {
                    Entity = lst.AsEnumerable(),
                    IsSuccess = true,
                    Message = MessageResource.SearchSuccess
                };
            }
            catch (Exception ex)
            {
                return new InvoiceItemGetForProfileResponse
                {
                    Message = ex.GetMessages(),
                    IsSuccess = false
                };
            }
        }
    }
}
