using System;
using Cafe7.Infrastructure.Service;

namespace Cafe7.Service.PeopleModule.ViewModel
{
    public abstract class PeopleViewModel : ServiceBase<int>
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string NationalId { get; set; }
        public string Mobile { get; set; }
        public string Birthday { get; set; }
        public DateTime Date { get; set; }
        public string Address { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
    }
}
