using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Infrastructure.ExceptionHelper;
using Cafe7.Infrastructure.Localize.ProductResource;
using Cafe7.Infrastructure.Messaging;
using Cafe7.Infrastructure.Service.RequsetResponseMessaging.Response;
using Cafe7.Model;
using Cafe7.Model.Product;
using Cafe7.Service.ProductModule.Mapper;
using Cafe7.Service.ProductModule.Messaging.Request;
using Cafe7.Service.ProductModule.Messaging.Response;

namespace Cafe7.Service.ProductModule.Implementation
{
   public class ProductService
    {
        private DbCafe7Managment _db;
        public ProductInsertResponse Insert(ProductInsertRequest request)
        {
            try
            {

                _db = new DbCafe7Managment();
                var model = request.Entity.ToModel();
                
                if (_db.Products.Any(m => m.Name == request.Entity.Name))
                {
                    return new ProductInsertResponse
                    {
                        Entity = request.Entity,
                        Message = string.Format(MessageResource.AlreadyExist,ProductResource.Name),
                        IsSuccess = false,
                        Result = ResultType.None
                    };
                }

                _db.Products.Add(model);
                _db.SaveChanges();
                return new ProductInsertResponse
                {
                    Entity = model.ToViewModel(),
                    Message = MessageResource.InsertSuccess,
                    IsSuccess = true,
                    Result = ResultType.Success
                };
            }
            catch (Exception e)
            {
                return new ProductInsertResponse
                {
                    Message = e.GetMessages(),
                    IsSuccess = false
                };
            }
        }

        public ProductDeleteResponse Delete(ProductDeleteRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                var entity = _db.Products.Find(request.Id);
                if (entity != null)
                {
                    _db.Products.Remove(entity);
                    _db.SaveChanges();
                    return new ProductDeleteResponse
                    {
                        Message = MessageResource.DeletSuccess,
                        IsSuccess = true,
                        Result = ResultType.Success
                    };
                }
                else
                {
                    return new ProductDeleteResponse
                    {
                        Message = string.Format(MessageResource.NotFound,ProductResource.Name)
                        ,
                        IsSuccess = false
                    };
                }

            }
            catch (Exception e)
            {
                return new ProductDeleteResponse
                {
                    Message = e.GetMessages(),
                    IsSuccess = false
                };
            }
        }

        public ProductUpdateResponse Update(ProductUpdateRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                var model = request.Entity.ToModel();
                var entity = _db.Products.Find(model.Id);
                if (entity == null)
                    return new ProductUpdateResponse
                    {
                        Message =string.Format( MessageResource.NotFound,ProductResource.Name),
                        IsSuccess = false
                    };
                if (entity.Name == request.Entity.Name)
                {
                    AutoMapper.Mapper.Map(request.Entity, entity);
                    _db.SaveChanges();
                    return new ProductUpdateResponse
                    {
                        Entity = model.ToViewModel(),
                        Message = MessageResource.UpdateSuccess,
                        IsSuccess = true,
                        Result = ResultType.Success
                    };
                }
                else
                {
                    if (_db.Products.Any(m => m.Name == request.Entity.Name))
                    {
                        return new ProductUpdateResponse
                        {
                            Entity = request.Entity,
                            Message =string.Format( MessageResource.AlreadyExist,ProductResource.Name),
                            IsSuccess = false
                        };
                    }
                }

                AutoMapper.Mapper.Map(request.Entity, entity);
                _db.SaveChanges();
                return new ProductUpdateResponse
                {
                    Entity = model.ToViewModel(),
                    Message = MessageResource.UpdateSuccess,
                    IsSuccess = true,
                    Result = ResultType.Success
                };
            }
            catch (Exception e)
            {
                return new ProductUpdateResponse
                {
                    Entity = request.Entity,
                    Message = e.GetMessages(),
                    IsSuccess = false,
                    Result = ResultType.None
                };
            }
        }

        public ProductSearchResponse Search(ProductSearchRequest request)
        {
            try
            {
                _db = new DbCafe7Managment();
                var model = request.Entity.ToModel();

                if (model.Name == "")
                    return new ProductSearchResponse
                    {
                        Message =string.Format( MessageResource.NotFound,ProductResource.Name),
                        IsSuccess = false,
                        Result = ResultType.None
                    };

                var entity = _db.Products.SingleOrDefault(m => m.Name == model.Name);
                if (entity == null)
                    return new ProductSearchResponse
                    {
                        Message =string.Format( MessageResource.NotFound,ProductResource.Name),
                        IsSuccess = false
                    };
                return new ProductSearchResponse
                {
                    Entity = entity.ToViewModel(),
                    Message = MessageResource.SearchSuccess,
                    IsSuccess = true,
                    Result = ResultType.Success
                };

            }
            catch (Exception e)
            {
                return new ProductSearchResponse
                {
                    Message = e.GetMessages(),
                    IsSuccess = false,
                    Result = ResultType.Information
                };
            }
        }

        public ProductGetByTypeResponse GetByType(ProductGetByTypeRequest request)
        {
            try
            {


                _db = new DbCafe7Managment();
                if (!_db.Products.Any(c => c.Type == request.Type))
                {
                    IEnumerable<ProductModel> model = new List<ProductModel>
                 {
                   new ProductModel
                   {
                       Name = ProductResource.NoProduct
                   }
                 };
            
                }


                var entity = _db.Products.Where(m => m.Type == request.Type).Select(n => n.Name).ToList();
                return new ProductGetByTypeResponse
                {
                    Entity = entity,
                    Message = MessageResource.InsertSuccess,
                    IsSuccess = true
                };


            }
            catch (Exception e)
            {
                return new ProductGetByTypeResponse
                {
                    Message = e.GetMessages(),
                    IsSuccess = false
                };
            }
        }
    }
}
