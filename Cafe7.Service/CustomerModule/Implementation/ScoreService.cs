using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.ExceptionHelper;
using Cafe7.Infrastructure.Messaging;
using Cafe7.Infrastructure.PeopleResource;
using Cafe7.Infrastructure.SmsResource;
using Cafe7.Model;
using Cafe7.Model.Customer;
using Cafe7.Service.CustomerModule.Mapper;
using Cafe7.Service.CustomerModule.Messaging.Request.ScoreRequest;
using Cafe7.Service.CustomerModule.Messaging.Response.ScoreResponse;
using Cafe7.Service.CustomerModule.ViewModel;
using static System.String;

namespace Cafe7.Service.CustomerModule.Implementation
{
    public class ScoreService
    {
        private DbCafe7Managment _db;
        public ScoreUpdateResponse Update(ScoreUpdateRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();

                var model = _db.Scores.Find(request.Entity.CustomerId);
                var customerModel = _db.Customers.Find(request.Entity.CustomerId);
                if (model == null)
                {
                    var scoreModelm = new ScoreModel
                    {
                        CustomerId = request.Entity.CustomerId,
                        LastScore = request.Entity.LastScore,
                        Total = request.Entity.LastScore
                    };
                    _db.Scores.Add(scoreModelm);
                    _db.SaveChanges();

                    var s = Sms.Sms.SendSms(customerModel?.Mobile, $"{customerModel?.Name} {customerModel?.Family} عزیز شما اولین امتیاز خود را کسب کردید :) \n امتیاز شما = {request.Entity.LastScore:N0} \n کافه هفت");


                    if (s.IsSuccess)
                        return new ScoreUpdateResponse()
                        {
                            Entity = model.ToViewModel(),
                            Message = MessageResource.AddScore,
                            IsSuccess = true,

                        };
                    _db = new DbCafe7Managment();
                    var x = _db.Scores.Find(request.Entity.CustomerId);
                    if (x != null) _db.Scores.Remove(x);
                    _db.SaveChanges();
                    return new ScoreUpdateResponse
                    {
                        Message = s.Message,
                        IsSuccess = false
                    };
           
                }

                var total = request.Entity.LastScore + model.Total;
                var scoreVm = new ScoreViewModel
                {
                    CustomerId = request.Entity.CustomerId,
                    LastScore = request.Entity.LastScore,
                    Total = double.Parse(total.ToString("N0"))
                };
                var send = Sms.Sms.SendSms(customerModel?.Mobile,
                    $"{customerModel?.Name} {customerModel?.Family} عزیز امتیاز شما افزوده شد :) \n امتیاز کسب شده = {request.Entity.LastScore} \n امتیاز کل شما = {total:N0} \n کافه هفت ");
                if (!send.IsSuccess)
                {
                    return new ScoreUpdateResponse
                    {
                        Message = send.Message,
                        IsSuccess = false
                    };
                }
                _db.Scores.Remove(model);
                _db.Scores.Add(scoreVm.ToModel());
                _db.SaveChanges();



                return new ScoreUpdateResponse
                {
                    Entity = scoreVm,
                    Message = MessageResource.AddScore,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ScoreUpdateResponse
                {
                    Message = ex.GetMessages(),
                    IsSuccess = false
                };
            }
        }

        public ScoreDeleteResponse Delete(ScoreDeleteRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                var scoreModel = _db.Scores.Find(request.CustomerId);

                if (scoreModel == null)
                    return new ScoreDeleteResponse
                    {
                        Message = Format(MessageResource.NotFound, PeopleResource.Score),
                        IsSuccess = false
                    };
                var scoreDecrease = double.Parse(request.ScoreDecrease);
                if (scoreDecrease < scoreModel.Total)
                {
                    var totalVm = scoreModel.Total - scoreDecrease;
                    scoreModel.Total = totalVm;
                    var customer = _db.Customers.Find(request.CustomerId);
                    var s = Sms.Sms.SendSms(customer?.Mobile,
                          $"{customer?.Name} {customer?.Family} عزیز به میزان {request.ScoreDecrease} از امتیاز شما کسر شد \n امتیاز فعلی شما = {totalVm:N0} \n کافه هفت");
                    if (!s.IsSuccess)
                    {
                        return new ScoreDeleteResponse
                        {
                            Message = s.Message,
                            IsSuccess = false
                        };
                    }
                    //_db.Scores.Remove(scoreModel);
                    //_db.Scores.Add(scoreModel);
                    _db.SaveChanges();

                    return new ScoreDeleteResponse
                    {
                        Entity = scoreModel.ToViewModel(),
                        Message = string.Format(MessageResource.DecreaseSuccess, PeopleResource.Score),
                        IsSuccess = true
                    };
                }
                else if (scoreDecrease > scoreModel.Total)
                {
                    return new ScoreDeleteResponse
                    {
                        Entity = scoreModel.ToViewModel(),
                        Message = string.Format(MessageResource.DercreaseUnSuccessFull, PeopleResource.Score),
                        IsSuccess = false
                    };
                }

                var customerModel = _db.Customers.Find(request.CustomerId);
                var send = Sms.Sms.SendSms(customerModel?.Mobile,
                    $"{customerModel?.Name} {customerModel?.Family} عزیز شما از کل امتیازات خود استفاده کردید \n کافه هفت");
                if (!send.IsSuccess)
                {
                    return new ScoreDeleteResponse
                    {
                        Message = send.Message,
                        IsSuccess = false
                    };
                }
                _db.Scores.Remove(scoreModel);
                _db.SaveChanges();

                return new ScoreDeleteResponse
                {
                    Message = string.Format(MessageResource.DecreaseSuccess, PeopleResource.Score),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ScoreDeleteResponse
                {
                    Message = ex.GetMessages(),
                    IsSuccess = false
                };
            }
        }

        public ScoreGetByCustomerIdResponse GetByCustomerId(ScoreGetByCustomerIdRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                var model = _db.Scores.Find(request.CustomerId);
                if (model != null)
                    return new ScoreGetByCustomerIdResponse
                    {
                        Entity = model.ToViewModel(),
                        Message = MessageResource.SearchSuccess,
                        IsSuccess = true
                    };
                return new ScoreGetByCustomerIdResponse
                {
                    Entity = new ScoreViewModel
                    {
                        Total = 0
                    }
                    ,
                    Message = string.Format(MessageResource.NotFound, PeopleResource.Score),
                    IsSuccess = false
                };

            }
            catch (Exception ex)
            {
                return new ScoreGetByCustomerIdResponse
                {
                    Message = ex.GetMessages(),
                    IsSuccess = false
                };
            }
        }

        public ScoreGetByMaxMinResponse GetByMaxMin(ScoreGetByMaxMinRequest request)
        {

            var _list = new List<CustomerViewModel>();
            try
            {
                if (request.Entity.Max == "" && request.Entity.Min == "")
                    return new ScoreGetByMaxMinResponse
                    {
                        Message = string.Format(MessageResource.FillTheBlank, PeopleResource.Score),
                        IsSuccess = false
                    };
                _db = new DbCafe7Managment();
                if (request.Entity.Max != "")
                {

                    var max = double.Parse(request.Entity.Max);


                    if (!_db.Scores.Any(m => m.Total >= max))
                    {
                        return new ScoreGetByMaxMinResponse
                        {
                            Message = string.Format(MessageResource.NotFound, PeopleResource.Score),
                            IsSuccess = false,
                            Entity = null
                        };
                    }

                    var scoreModel = _db.Scores.Where(m => m.Total >= max);
                    foreach (var item in scoreModel)
                    {
                        _db = new DbCafe7Managment();
                        var customerModel = _db.Customers.Find(item.CustomerId);
                        if (customerModel == null)
                        {
                            return new ScoreGetByMaxMinResponse
                            {
                                Message = string.Format(MessageResource.NotFound, PeopleResource.Customer),
                                IsSuccess = false
                                ,
                                Entity = null
                            };
                        }

                        _list.Add(customerModel.ToViewModel());
                    }
                }
                else
                {
                    var min = double.Parse(request.Entity.Min);
                    if (!_db.Scores.Any(m => m.Total <= min))
                    {
                        return new ScoreGetByMaxMinResponse
                        {
                            Message = string.Format(MessageResource.NotFound, PeopleResource.Score),
                            IsSuccess = false
                        };
                    }

                    var scoreModel = _db.Scores.Where(m => m.Total <= min);
                    foreach (var item in scoreModel)
                    {
                        _db = new DbCafe7Managment();
                        var customerModel = _db.Customers.Find(item.CustomerId);
                        if (customerModel == null)
                        {
                            return new ScoreGetByMaxMinResponse
                            {
                                Message = string.Format(MessageResource.NotFound, PeopleResource.Customer),
                                IsSuccess = false
                            };
                        }

                        _list.Add(customerModel.ToViewModel());
                    }
                }

                return new ScoreGetByMaxMinResponse
                {
                    Entity = _list,
                    Message = MessageResource.SearchSuccess,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {

                return new ScoreGetByMaxMinResponse
                {
                    Message = ex.GetMessages(),
                    IsSuccess = false
                };
            }
        }

        public PointInsertResponse InsertPoint(PointInsertRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                var model = new PointModel
                {
                    Point = request.Entity.Point
                };
                if (!_db.Points.Any())
                {
                    _db.Points.Add(model);
                    _db.SaveChanges();
                    return new PointInsertResponse
                    {
                        Entity = request.Entity,
                        Message = MessageResource.InsertSuccess,
                        IsSuccess = true
                    };
                }

                var firstItem = _db.Points.First();
                _db.Points.Remove(firstItem);
                _db.Points.Add(model);
                _db.SaveChanges();
                return new PointInsertResponse
                {
                    Entity = new PointViewModel
                    {
                        Id = model.Id,
                        Point = model.Point
                    },
                    Message = MessageResource.InsertSuccess,
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new PointInsertResponse
                {
                    Message = e.GetMessages(),
                    IsSuccess = false
                };
            }
        }

        public PointSearchResponse SearchPoint(PointSearchRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                if (!_db.Points.Any())
                {
                    return new PointSearchResponse
                    {
                        IsSuccess = false,
                        Entity = new PointViewModel
                        {
                            Point = 0
                        }
                    };
                }

                var model = _db.Points.First();
                if (model == null)
                    return new PointSearchResponse
                    {
                        Message = Format(MessageResource.NotFound, PeopleResource.Score),
                        IsSuccess = false
                    };
                return new PointSearchResponse
                {
                    Entity = new PointViewModel
                    {
                        Id = model.Id,
                        Point = model.Point
                    },
                    Message = MessageResource.SearchSuccess,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {

                return new PointSearchResponse
                {
                    Message = ex.GetMessages(),
                    IsSuccess = false
                };
            }
        }
    }
}
