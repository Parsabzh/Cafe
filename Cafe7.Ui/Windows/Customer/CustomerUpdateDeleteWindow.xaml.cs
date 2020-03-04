using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Cafe7.Infrastructure.DateTimeHelper;
using Cafe7.Service.CustomerModule.Implementation;
using Cafe7.Service.CustomerModule.Messaging.Request;
using Cafe7.Service.CustomerModule.ViewModel;
using Cafe7.Service.InvoiceItemModule.Implementation;
using Cafe7.Service.InvoiceItemModule.Messaging.Request;
using Cafe7.Service.InvoiceModule.Implementation;
using Cafe7.Service.InvoiceModule.Messaging.Request;
using Cafe7.Service.InvoiceModule.ViewModels;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Cafe7.Ui.Windows.Customer
{
    /// <summary>
    /// Interaction logic for CustomerUpdateDeleteWindow.xaml
    /// </summary>
    public partial class CustomerUpdateDeleteWindow : Window
    {
        private readonly CustomerService _customerService;
        private readonly InvoiceService _invoiceService;
        private readonly InvoiceItemService _invoiceItemService;
        private int _id = 0;
        private DateTime _date;
        private readonly Notifier _notifier;
        public CustomerUpdateDeleteWindow()
        {

            InitializeComponent();
            _customerService = new CustomerService();
            _invoiceService = new InvoiceService();
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
        private void ButtonUpdate_OnClick(object sender, RoutedEventArgs e)
        {

            var entity = new CustomerViewModel
            {
                Id = _id,
                Name = TextBoxName.Text.Trim(),
                Family = TextBoxFamily.Text.Trim(),
                RegistrationNumber = int.Parse(TextBoxRegistrationNum.Text.Trim()),
                Address = TextBoxAddress.Text.Trim(),
                Mobile = TextBoxMobile.Text.Trim(),
                NationalId = TextBoxNationalId.Text.Trim(),
                Birthday = PersianDatePicker.SelectedDate.ToString(),
                Date = _date,
                Day = PersianDatePicker.SelectedDate.Day,
                Month = PersianDatePicker.SelectedDate.Month
            };

            var response = _customerService.Update(new CustomerUpdateRequest
            {
                Entity = entity
            });
            if (response.IsSuccess)
                _notifier.ShowSuccess(response.Message);
            _notifier.ShowError(response.Message);
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            var message = MessageBox.Show("آیا مطمِئن هستید؟", "اخطار", MessageBoxButton.YesNo, MessageBoxImage.Asterisk, MessageBoxResult.None, MessageBoxOptions.RightAlign);
            if (message != MessageBoxResult.Yes) return;
            //gereftan list invoice baraye b dast avardane id invoice ha baraye inke invoice item ha ro pak konam
            var deleteInvoiceItemResponse = _invoiceItemService.DeleteByCustomerId(new InvoiceItemDeleteByCustomerIdRequest
            {
                Id = _id
            });


            if (!deleteInvoiceItemResponse.IsSuccess)
            {
                _notifier.ShowError(deleteInvoiceItemResponse.Message);
                return;
            }

            var response = _customerService.Delete(new CustomerDeleteRequest
            {
                Id = _id
            });

            if (!response.IsSuccess)
            {
                _notifier.ShowError(response.Message);
                return;
            }
            foreach (var ctl in Grid.Children)
            {
                if (ctl.GetType() == typeof(TextBox))
                    ((TextBox)ctl).Text = string.Empty;

            }
            PersianDatePicker.Text = "";
            if (Owner is MainWindow.MainWindow win)
            {
                win.ChipCustomerMaxRegistrationNumber.Content = response.LastRegistrationNumber;
                win.GridCustomerScore.ItemsSource = _customerService.GetAll(new CustomerGetAllRequest()).Entity.ToList();
                win.GridCustomerScore.Items.Refresh();
            }
            _notifier.ShowSuccess(response.Message);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var entity = new CustomerViewModel()
            {
                Mobile = TextBoxSearchMobile.Text,
                RegistrationNumber = int.Parse(TextBoxRegistrationNumSearch.Text),
                NationalId = TextBoxNationalIdSearch.Text
            };
            var response = _customerService.Search(new CustomerSearchRequest
            {
                Entity = entity
            });
            if (!response.IsSuccess)
            {
                _notifier.ShowError(response.Message);
                return;
            }
            _notifier.ShowSuccess(response.Message);
            foreach (var ctl in Grid.Children)
            {
                if (ctl.GetType() == typeof(TextBox))
                    ((TextBox)ctl).Text = string.Empty;
            }
            _date = response.Entity.Date;
            _id = response.Entity.Id;
            TextBoxName.Text = response.Entity.Name.Trim();
            TextBoxFamily.Text = response.Entity.Family.Trim();
            TextBoxAddress.Text = response.Entity.Address.Trim();
            TextBoxMobile.Text = response.Entity.Mobile.Trim();
            TextBoxRegistrationNum.Text = response.Entity.RegistrationNumber.ToString();
            TextBoxNationalId.Text = response.Entity.NationalId.Trim();
            PersianDatePicker.Text = response.Entity.Birthday.ToString(CultureInfo.CurrentUICulture);
        }
    }
}
