using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Model.Customer;
using Cafe7.Service.CustomerModule.ViewModel;

namespace Cafe7.Service.CustomerModule.Mapper
{
   public static class CustomerMapper
    {
        public static CustomerViewModel ToViewModel(this CustomerModel entity)
        {
          return  AutoMapper.Mapper.Map<CustomerModel, CustomerViewModel>(entity);
        }
        public static CustomerModel ToModel(this CustomerViewModel entity)
        {
            return AutoMapper.Mapper.Map<CustomerViewModel, CustomerModel>(entity);
        }

        public static IEnumerable<CustomerViewModel> ToViewModel(this IEnumerable<CustomerModel> entity)
        {
            return AutoMapper.Mapper.Map<IEnumerable<CustomerModel>, IEnumerable<CustomerViewModel>>(entity);
        }
        public static IEnumerable<CustomerModel> ToModel(this IEnumerable<CustomerViewModel> entity)
        {
            return AutoMapper.Mapper.Map<IEnumerable<CustomerViewModel>, IEnumerable<CustomerModel>>(entity);
        }

        public static ScoreViewModel ToViewModel(this ScoreModel entity)
        {
            return AutoMapper.Mapper.Map<ScoreModel, ScoreViewModel>(entity);
        }

        public static ScoreModel ToModel(this ScoreViewModel entity)
        {
            return AutoMapper.Mapper.Map<ScoreViewModel, ScoreModel>(entity);
        }

        public static IEnumerable<ScoreViewModel> ToViewModel(this IEnumerable<ScoreModel> entity)
        {
            return AutoMapper.Mapper.Map<IEnumerable<ScoreModel>, IEnumerable<ScoreViewModel>>(entity);
        }
        public static IEnumerable<ScoreModel> ToModel(this IEnumerable<ScoreViewModel> entity)
        {
            return AutoMapper.Mapper.Map<IEnumerable<ScoreViewModel>, IEnumerable<ScoreModel>>(entity);
        }

    }
}
