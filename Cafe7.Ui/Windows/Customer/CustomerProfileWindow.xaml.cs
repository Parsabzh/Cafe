using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Cafe7.Service.Class;
using Cafe7.Service.CustomerModule.Implementation;
using Cafe7.Service.CustomerModule.Messaging.Request;
using Cafe7.Service.CustomerModule.Messaging.Request.ScoreRequest;
using Cafe7.Service.CustomerModule.ViewModel;
using Cafe7.Service.InvoiceItemModule.Implementation;
using Cafe7.Service.InvoiceItemModule.Messaging.Request;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Cafe7.Ui.Windows.Customer
{
    /// <summary>
    /// Interaction logic for CustomerProfileWindow.xaml
    /// </summary>
    public partial class CustomerProfileWindow : Window
    {
        private readonly CustomerService _customerService;
        private readonly InvoiceItemService _invoiceItemService;
        private readonly Notifier _notifier;
        private readonly ScoreService _scoreService;

        public CustomerProfileWindow()
        {
            InitializeComponent();
            _scoreService=new ScoreService();
            _customerService = new CustomerService();
            _invoiceItemService = new InvoiceItemService();
            _notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    this,
                    Corner.BottomRight,
                    30,
                    10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    TimeSpan.FromSeconds(3),
                    MaximumNotificationCount.FromCount(3));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });
        }

        private void ButtonSearchCustomer_Click(object sender, RoutedEventArgs e)
        {
            var responseCustomer = _customerService.Search(new CustomerSearchRequest
            {
                Entity = new CustomerViewModel { RegistrationNumber = int.Parse(TextBoxRegistartionNumber.Text.Trim()) }
            });
            if (!responseCustomer.IsSuccess)
            {
                _notifier.ShowError(responseCustomer.Message);
                return;
            }

            LabelName.Content = responseCustomer.Entity.Name;
            LabelFamily.Content = responseCustomer.Entity.Family;
            _notifier.ShowSuccess(responseCustomer.Message);

            var responseInvoice = _invoiceItemService.GetForProfile(new InvoiceItemGetForProfileRequest { Registartion = int.Parse(TextBoxRegistartionNumber.Text.Trim()) });

            if (!responseInvoice.IsSuccess)
            {
                _notifier.ShowError(responseInvoice.Message);
                return;
            }
            GridCustomer.ItemsSource = responseInvoice.Entity.ToList();
            GridCustomer.Items.Refresh();
            _notifier.ShowSuccess(responseInvoice.Message);
            var responseScore = _scoreService.GetByCustomerId(new ScoreGetByCustomerIdRequest{CustomerId = responseCustomer.Entity.Id});
            if (!responseScore.IsSuccess)
                _notifier.ShowError(responseScore.Message);
            LabelScore.Content = responseScore.Entity.Total.ToString("N0");
            var x = responseInvoice.Entity.Count();
            var y = ((100 * x) / 365)+1;
            if (y>100)
            {
                ProgressBar.SetPercent(100);
                return;
            }
                ProgressBar.SetPercent(y); 
        }
       
    }
}
