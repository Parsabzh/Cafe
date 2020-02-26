using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cafe7.Model.People;

namespace Cafe7.Model.Customer
{
    [Table("Score",Schema = "PeopleInfo")]
   public class ScoreModel
    {
        [Key]
        public int CustomerId { get; set; }
        public double LastScore { get; set; }
        public double Total { get; set; }

        [ForeignKey("CustomerId")]
        public CustomerModel Customer { get; set; }
    }
}
