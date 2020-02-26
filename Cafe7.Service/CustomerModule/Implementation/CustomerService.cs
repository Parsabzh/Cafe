using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.ExceptionHelper;

using Cafe7.Infrastructure.Messaging;
using Cafe7.Infrastructure.PeopleResource;
using Cafe7.Infrastructure.Service.RequsetResponseMessaging.Response;
using Cafe7.Model;
using Cafe7.Model.Customer;
using Cafe7.Service.CustomerModule.Mapper;
using Cafe7.Service.CustomerModule.Messaging.Request;
using Cafe7.Service.CustomerModule.Messaging.Response;
using Cafe7.Service.CustomerModule.ViewModel;
using Cafe7.Service.InvoiceItemModule.Implementation;
using Cafe7.Service.InvoiceItemModule.Messaging.Request;
using Cafe7.Service.InvoiceModule.Mapper;
using static System.String;


namespace Cafe7.Service.CustomerModule.Implementation
{
    public class CustomerService
    {
        private DbCafe7Managment _db;
        public CustomerInsertResponse Insert(CustomerInsertRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                var model = request.Entity.ToModel();
                if (request.Entity.RegistrationNumber == 0 && request.Entity.NationalId == "" && request.Entity.NationalId == "" && request.Entity.Mobile == "")
                    return new CustomerInsertResponse
                    {
                        IsSuccess = false,
                        Message = string.Format(MessageResource.NotFound, PeopleResource.Customer)
                    };
                if (_db.Customers.Any(m => m.RegistrationNumber == request.Entity.RegistrationNumber))
                {
                    return new CustomerInsertResponse
                    {
                        Entity = request.Entity,
                        Message = Format(MessageResource.AlreadyExist, PeopleResource.RegistrationNumber),
                        IsSuccess = false,
                        Result = ResultType.None
                    };
                }

                else if (_db.Customers.Any(m => m.NationalId == request.Entity.NationalId))
                {
                    return new CustomerInsertResponse
                    {
                        Entity = request.Entity,
                        Message = Format(MessageResource.AlreadyExist, PeopleResource.NationalId),
                        IsSuccess = false,
                        Result = ResultType.None
                    };
                }
                else if (_db.Customers.Any(m => m.Mobile == request.Entity.Mobile))
                {
                    return new CustomerInsertResponse
                    {
                        Entity = request.Entity,
                        Message = string.Format(MessageResource.AlreadyExist, PeopleResource.Mobile),
                        IsSuccess = false,
                        Result = ResultType.None
                    };
                }
                _db.Customers.Add(model);
                _db.SaveChanges();
                var s = Sms.Sms.SendSms(model.Mobile, $"{model.Name} {model.Family} عزیز به باشگاه مشتریان کافه هفت خوش آمدید.به امید دیدار شما همراه گرامی \n کد اشتراک شما = {model.RegistrationNumber}");
                if (s.IsSuccess)
                    return new CustomerInsertResponse
                    {
                        Entity = model.ToViewModel(),
                        Message = MessageResource.InsertSuccess,
                        IsSuccess = true,
                        Result = ResultType.Success
                    };
                _db = new DbCafe7Managment();
                var x = _db.Customers.Find(model.Id);
                if (x != null) _db.Customers.Remove(x);
                _db.SaveChanges();
                return new CustomerInsertResponse
                {
                    Message = s.Message,
                    IsSuccess = false
                };
        
            }
            catch (Exception e)
            {
                return new CustomerInsertResponse
                {
                    Message = e.GetMessages(),
                    IsSuccess = false
                };
            }
        }

        public CustomerDeleteResponse Delete(CustomerDeleteRequest request)
        {
            try
            {


                _db = new DbCafe7Managment();
                var entity = _db.Customers.Find(request.Id);
                if (entity != null)
                {

                    var scoreModel = _db.Scores.Find(entity.Id);
                    if (scoreModel != null)
                    {
                        _db.Scores.Remove(scoreModel);
                        _db.SaveChanges();
                    }

                    _db.Customers.Remove(entity);
                    _db.SaveChanges();
                    return new CustomerDeleteResponse
                    {
                        LastRegistrationNumber = _db.Customers.Max(m => m.RegistrationNumber).ToString(),
                        Message = MessageResource.DeletSuccess,
                        IsSuccess = true,
                        Result = ResultType.Success
                    };
                }
                else
                {
                    return new CustomerDeleteResponse
                    {
                        Message = string.Format(MessageResource.NotFound, PeopleResource.Customer)
                        ,
                        IsSuccess = false
                    };
                }

            }
            catch (Exception e)
            {
                return new CustomerDeleteResponse
                {
                    Message = e.GetMessages(),
                    IsSuccess = false
                };
            }
        }

        public CustomerUpdateResponse Update(CustomerUpdateRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                var model = request.Entity.ToModel();
                var entity = _db.Customers.Find(model.Id);
                if (entity == null)
                    return new CustomerUpdateResponse
                    {
                        Message = Format(MessageResource.NotFound, PeopleResource.Customer),
                        IsSuccess = false
                    };
                if (entity.Mobile != request.Entity.Mobile)
                {
                    if (_db.Customers.Any(m => m.Mobile == request.Entity.Mobile))
                    {
                        return new CustomerUpdateResponse
                        {
                            Message = Format(MessageResource.AlreadyExist, PeopleResource.Mobile),
                            IsSuccess = false
                        };
                    }
                }
                else if (entity.RegistrationNumber != request.Entity.RegistrationNumber)
                {
                    if (_db.Customers.Any(m => m.RegistrationNumber == request.Entity.RegistrationNumber))
                    {
                        return new CustomerUpdateResponse
                        {
                            Message = Format(MessageResource.AlreadyExist, PeopleResource.RegistrationNumber),
                            IsSuccess = false
                        };
                    }
                }
                else if (entity.NationalId != request.Entity.NationalId)
                {
                    if (_db.Customers.Any(m => m.NationalId == request.Entity.NationalId))
                    {
                        return new CustomerUpdateResponse
                        {
                            Message = Format(MessageResource.AlreadyExist, PeopleResource.NationalId),
                            IsSuccess = false
                        };
                    }
                }


                AutoMapper.Mapper.Map(request.Entity, entity);
                _db.SaveChanges();
                return new CustomerUpdateResponse
                {
                    Entity = model.ToViewModel(),
                    Message = MessageResource.UpdateSuccess,
                    IsSuccess = true,
                    Result = ResultType.Success
                };
            }
            catch (Exception e)
            {
                return new CustomerUpdateResponse
                {
                    Entity = request.Entity,
                    Message = e.GetMessages(),
                    IsSuccess = false,
                    Result = ResultType.None
                };
            }
        }

        public CustomerSearchResponse Search(CustomerSearchRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                var model = request.Entity.ToModel();

                if (model.RegistrationNumber == 0 && model.Mobile == "" && model.NationalId == "")
                    return new CustomerSearchResponse
                    {
                        Message = string.Format(MessageResource.NotFound, PeopleResource.Customer),
                        IsSuccess = false,
                        Result = ResultType.None
                    };
                if (model.RegistrationNumber != 0)
                {
                    var registrationNumber = _db.Customers.SingleOrDefault(m => m.RegistrationNumber == model.RegistrationNumber);
                    if (registrationNumber == null)
                        return new CustomerSearchResponse
                        {
                            Message = string.Format(MessageResource.NotFound, PeopleResource.RegistrationNumber),
                            IsSuccess = false
                        };
                    return new CustomerSearchResponse
                    {
                        Entity = registrationNumber.ToViewModel(),
                        Message = MessageResource.SearchSuccess,
                        IsSuccess = true,
                        Result = ResultType.Success
                    };
                }
                else if (model.Mobile != "")
                {
                    {
                        var mobile = _db.Customers.SingleOrDefault(m => m.Mobile == model.Mobile);
                        if (mobile == null)
                            return new CustomerSearchResponse
                            {
                                Message = string.Format(MessageResource.NotFound, PeopleResource.Mobile),
                                IsSuccess = false
                            };
                        return new CustomerSearchResponse
                        {
                            Entity = mobile.ToViewModel(),
                            Message = MessageResource.SearchSuccess,
                            IsSuccess = true,
                            Result = ResultType.Success
                        };
                    }
                }
                var nationalid = _db.Customers.SingleOrDefault(m => m.NationalId == model.NationalId);
                if (nationalid == null)
                    return new CustomerSearchResponse
                    {
                        Message = string.Format(MessageResource.NotFound, PeopleResource.NationalId),
                        IsSuccess = false
                    };
                return new CustomerSearchResponse
                {
                    Entity = nationalid.ToViewModel(),
                    Message = MessageResource.SearchSuccess,
                    IsSuccess = true,
                    Result = ResultType.Success
                };
            }
            catch (Exception e)
            {
                return new CustomerSearchResponse
                {
                    Message = e.GetMessages(),
                    IsSuccess = false,
                    Result = ResultType.Information
                };
            }
        }

        public CustomerGetByIdResponse GetById(CustomerGetByIdRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();

                if (!_db.Customers.Any(m => m.Id == request.Id))
                {
                    return new CustomerGetByIdResponse
                    {
                        Message = MessageResource.NotFound,
                        IsSuccess = false
                    };
                }
                var model = _db.Customers.Find(request.Id);
                return new CustomerGetByIdResponse
                {
                    IsSuccess = true,
                    Entity = model.ToViewModel(),
                    Message = MessageResource.SearchSuccess
                };
            }
            catch (Exception ex)
            {
                return new CustomerGetByIdResponse
                {
                    Message = ex.GetMessages(),
                    IsSuccess = false
                };
            }
        }

        public CustomerGetByRegistartionNumberResponse GetByRegistartionNumber(CustomerGetByRegistrationNumberRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                if (!_db.Customers.Any(m => m.RegistrationNumber == request.RegistrationNumber))
                    return new CustomerGetByRegistartionNumberResponse
                    {
                        IsSuccess = false,
                        Message = string.Format(MessageResource.NotFound, PeopleResource.RegistrationNumber),
                    };

                var model = _db.Customers.SingleOrDefault(m => m.RegistrationNumber == request.RegistrationNumber);
                return new CustomerGetByRegistartionNumberResponse
                {
                    Entity = model.ToViewModel(),
                    Message = MessageResource.SearchSuccess,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                return new CustomerGetByRegistartionNumberResponse
                {
                    Message = ex.GetMessages(),
                    IsSuccess = false
                };
            }
        }

        public CustomerMaxRegistrationNumberResponse MaxRegistrationNumber()
        {
            _db = new DbCafe7Managment();
            var maxId = _db.Customers.Max(m => (m != null ? m.Id : (int?) null));
            if (maxId==null)return new CustomerMaxRegistrationNumberResponse
            {
                MaxRegistrationNumber = 0
            };

            var registrartionNumber = _db.Customers.SingleOrDefault(m => m.Id == maxId).RegistrationNumber;
            return new CustomerMaxRegistrationNumberResponse
            {
                MaxRegistrationNumber = registrartionNumber
            };
        }

        public CustomerBirthdayResponse Birthday(CustomerBirthdayRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                if (!_db.Customers.Any(m => m.Month == request.Month))
                    return new CustomerBirthdayResponse
                    {
                        Message = string.Format(MessageResource.NotFound, PeopleResource.Customer),
                        IsSuccess = false
                    };

                var month = _db.Customers.Where(m => m.Month == request.Month);
                if (!month.Any(d => d.Day == request.Day))
                {
                    return new CustomerBirthdayResponse
                    {
                        Message = string.Format(MessageResource.NotFound, PeopleResource.Customer),
                        IsSuccess = false
                    };
                }
                var getByMonth = month.Where(c => c.Day == request.Day);

                return new CustomerBirthdayResponse
                {
                    Entity = getByMonth.AsEnumerable().ToViewModel(),
                    IsSuccess = true,
                    Message = MessageResource.SearchSuccess
                };
            }
            catch (Exception e)
            {
                return new CustomerBirthdayResponse
                {
                    Message = e.GetMessages(),
                    IsSuccess = false
                };
            }
        }

        public CustomerGetAllResponse GetAll(CustomerGetAllRequest request)
        {
            try
            {
                var lst = new List<CustomerGetAllScoreViewModel>();
                _db = new DbCafe7Managment();
                var model = _db.Customers;
                foreach (var customerModel in model)
                {
                    if (customerModel == null) continue;
                    _db = new DbCafe7Managment();
                    var scoreModel = _db.Scores.SingleOrDefault(m => m.CustomerId == customerModel.Id);
                    if (scoreModel == null)
                    {
                       lst.Add(new CustomerGetAllScoreViewModel
                        {
                            Name = customerModel.Name,
                            Family = customerModel.Family,
                            RegistrationNumber = customerModel.RegistrationNumber,
                            Total = 0
                        });
                       continue;
                    }
                    var total = scoreModel.Total;
                    lst.Add(new CustomerGetAllScoreViewModel
                    {
                        Name = customerModel.Name,
                        Family = customerModel.Family,
                        RegistrationNumber = customerModel.RegistrationNumber,
                        Total = total
                    });
                }

                var list = lst.OrderByDescending(m => m.Total);
                return new CustomerGetAllResponse
                {
                    Entity = list,
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {

                return new CustomerGetAllResponse
                {
                    Message = e.GetMessages(),
                    IsSuccess = false
                };
            }
        }
    }
}
