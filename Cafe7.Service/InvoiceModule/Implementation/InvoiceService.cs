using System;
using System.Collections.Generic;
using System.Linq;
using Cafe7.Infrastructure.ExceptionHelper;
using Cafe7.Infrastructure.Localize.InvoiceResource;
using Cafe7.Infrastructure.Messaging;
using Cafe7.Infrastructure.Service.RequsetResponseMessaging.Response;
using Cafe7.Model;
using Cafe7.Model.Invoice;
using Cafe7.Service.InvoiceModule.Mapper;
using Cafe7.Service.InvoiceModule.Messaging.Request;
using Cafe7.Service.InvoiceModule.Messaging.Response;

namespace Cafe7.Service.InvoiceModule.Implementation
{
    public class InvoiceService
    {
        private DbCafe7Managment _db;
        public InvoiceInsertResponse Insert(InvoiceInsertRequest request)
        {
            _db = new DbCafe7Managment();
            try
            {
                var model = request.Entity.ToModel();
                _db.Invoices.Add(model);
                _db.SaveChanges();
                return new InvoiceInsertResponse
                {
                    Entity = model.ToViewModel(),
                    Message = MessageResource.InsertSuccess,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new InvoiceInsertResponse
                {
                    Message = ex.GetMessages(),
                    IsSuccess = false
                };
            }
        }

        public InvoiceGetByTableNumberResponse GetByTableNumber(InvoiceGetByTableNumberRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                if (!_db.Invoices.Any(c => c.TableNumber == request.TableNumber))
                {

                    return new InvoiceGetByTableNumberResponse
                    {

                        IsSuccess = false,
                        Message =string.Format( MessageResource.NotFound,InvoiceResource.Invoice)
                    };
                }
                var invoice = _db.Invoices.Where(m => m.TableNumber == request.TableNumber).Select(m => m.Id).Max();
                var model = _db.Invoices.Find(invoice);
                return new InvoiceGetByTableNumberResponse
                {
                    Entity = model.ToViewModel(),
                    IsSuccess = true,
                    Message = MessageResource.SearchSuccess
                };
            }
            catch (Exception ex)
            {
                return new InvoiceGetByTableNumberResponse
                {
                    IsSuccess = false,
                    Message = ex.GetMessages()
                };
            }

        }

        public InvoiceDeleteResponse Delete(InvoiceDeleteRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                var model = _db.Invoices.Find(request.Id);

                if (model == null)
                    return new InvoiceDeleteResponse
                    {
                        Message =string.Format( MessageResource.NotFound,InvoiceResource.Invoice),
                        IsSuccess = false
                    };
                _db.Invoices.Remove(model);
                _db.SaveChanges();

                return new InvoiceDeleteResponse()
                {
                    Message = MessageResource.DeletSuccess,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new InvoiceDeleteResponse()
                {
                    IsSuccess = false,
                    Message = ex.GetMessages()
                };
            }
        }

        //public InvoiceGetByCustomerIdResponse GetByCustomerId(InvoiceGetByCustomerIdRequest request)
        //{
        //    try
        //    {
        //        _db = new DbCafe7Managment();
        //        if (!_db.InvoiceItems.Any(m => m.CustomerId == request.CustomerId))
        //            return new InvoiceGetByCustomerIdResponse
        //            {
        //                Message = string.Format(MessageResource.NotFound,InvoiceResource.Invoice),
        //                IsSuccess = false
        //            };
        //        var model = _db.InvoiceItems.Where(m => m.CustomerId == request.CustomerId);
        //        return new InvoiceGetByCustomerIdResponse
        //        {
        //            Entity = model.AsEnumerable().ToViewModel(),
        //            Message = MessageResource.SearchSuccess,
        //            IsSuccess = true
        //        };

        //    }
        //    catch (Exception ex)
        //    {
        //        return new InvoiceGetByCustomerIdResponse
        //        {
        //            Message = ex.GetMessages(),
        //            IsSuccess = false
        //        };
        //    }

        //}

        public InvoiceUpdateResponse Update(InvoiceUpdateRequest request)
        {
            try
            {
                _db=new DbCafe7Managment();
                var model = _db.Invoices.Find(request.Entity.Id);
                AutoMapper.Mapper.Map(request.Entity,model);
                _db.SaveChanges();
                return new InvoiceUpdateResponse
                {
                    Message = MessageResource.UpdateSuccess,
                    IsSuccess = true,
                    Entity = request.Entity
                };

            }
            catch (Exception ex)
            {
                return new InvoiceUpdateResponse
                {
                    Message = ex.GetMessages(),
                    IsSuccess = false
                };
            }
        }
    }
}
