using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Cafe7.Model;
using Cafe7.Service;

namespace Cafe7.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var culture = new CultureInfo("fa-IR")
            {
                NumberFormat =
                {
                    NumberDecimalSeparator = ".",
                    CurrencyDecimalSeparator = ".",
                  
                }
            };
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            AutoMapperConfiguration.Configure();
            var db = new DbCafe7Managment();
            db.Customers.Find(0);
        }
    }
}
