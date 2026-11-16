using System.Collections.Generic;
using System.Linq;
using Cafe7.Model.Customer;
using Cafe7.Service.CustomerModule.ViewModel;

namespace Cafe7.Service.CustomerModule.Mapper
{
    public static class CustomerMapper
    {
        public static CustomerViewModel ToViewModel(this CustomerModel entity)
        {
            if (entity == null)
                return null;

            return new CustomerViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Family = entity.Family,
                NationalId = entity.NationalId,
                Mobile = entity.Mobile,
                Birthday = entity.Birthday,
                Date = entity.Date,
                Address = entity.Address,
                Day = entity.Day,
                Month = entity.Month,
                RegistrationNumber = entity.RegistrationNumber
            };
        }

        public static CustomerModel ToModel(this CustomerViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new CustomerModel();
            entity.CopyTo(model);
            return model;
        }

        public static void CopyTo(this CustomerViewModel source, CustomerModel target)
        {
            target.Id = source.Id;
            target.Name = source.Name;
            target.Family = source.Family;
            target.NationalId = source.NationalId;
            target.Mobile = source.Mobile;
            target.Birthday = source.Birthday;
            target.Date = source.Date;
            target.Address = source.Address;
            target.Day = source.Day;
            target.Month = source.Month;
            target.RegistrationNumber = source.RegistrationNumber;
        }

        public static IEnumerable<CustomerViewModel> ToViewModel(this IEnumerable<CustomerModel> entity)
        {
            return entity?.Select(item => item.ToViewModel()).ToList() ?? new List<CustomerViewModel>();
        }

        public static IEnumerable<CustomerModel> ToModel(this IEnumerable<CustomerViewModel> entity)
        {
            return entity?.Select(item => item.ToModel()).ToList() ?? new List<CustomerModel>();
        }

        public static ScoreViewModel ToViewModel(this ScoreModel entity)
        {
            if (entity == null)
                return null;

            return new ScoreViewModel
            {
                CustomerId = entity.CustomerId,
                LastScore = entity.LastScore,
                Total = entity.Total,
                Customer = entity.Customer
            };
        }

        public static ScoreModel ToModel(this ScoreViewModel entity)
        {
            if (entity == null)
                return null;

            return new ScoreModel
            {
                CustomerId = entity.CustomerId,
                LastScore = entity.LastScore,
                Total = entity.Total,
                Customer = entity.Customer
            };
        }

        public static IEnumerable<ScoreViewModel> ToViewModel(this IEnumerable<ScoreModel> entity)
        {
            return entity?.Select(item => item.ToViewModel()).ToList() ?? new List<ScoreViewModel>();
        }

        public static IEnumerable<ScoreModel> ToModel(this IEnumerable<ScoreViewModel> entity)
        {
            return entity?.Select(item => item.ToModel()).ToList() ?? new List<ScoreModel>();
        }
    }
}
