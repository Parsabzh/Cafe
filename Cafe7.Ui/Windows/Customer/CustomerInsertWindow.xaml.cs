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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Arash;
using Cafe7.Service.CustomerModule.Implementation;
using Cafe7.Service.CustomerModule.Messaging.Request;
using Cafe7.Service.CustomerModule.ViewModel;
using Cafe7.Service.Sms;
using MaterialDesignThemes.Wpf;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Cafe7.Ui.Windows.Customer
{
    /// <summary>
    /// Interaction logic for CustomerInsertWindow.xaml
    /// </summary>
    public partial class CustomerInsertWindow : Window
    {
        private readonly CustomerService _customerService;
        private readonly Notifier _notifier;
        public CustomerInsertWindow()
        {
            InitializeComponent();
            _customerService = new CustomerService();
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var ctl in Grid.Children)
            {
                if (ctl.GetType() != typeof(TextBox)) continue;
                if ((((TextBox)ctl).Text)!="") continue;
                if (TextBoxAddress.Text=="" || TextBoxAddress.Text!="") continue;
                _notifier.ShowWarning("لطفا مقادیر خواسته شده را وارد نمایید");
                return;
            }

            if (PersianDatePicker.Text == "")
            {
                _notifier.ShowWarning("لطفا تاریخ تولد را وارد نمایید");
                return;
            }

            var response = _customerService.Insert(new CustomerInsertRequest
            {
                Entity = new CustomerViewModel
                {
                    Name = TextBoxName.Text.Trim(),
                    Family = TextBoxFamily.Text.Trim(),
                    NationalId = TextBoxNationalId.Text,
                    RegistrationNumber = int.Parse(TextBoxRegistrationNum.Text.Trim()),
                    Address = TextBoxAddress.Text.Trim(),
                    Mobile = TextBoxMobile.Text.Trim(),
                    Date = PersianDate.Today.ToDateTime(),
                    Birthday = PersianDatePicker.SelectedDate.ToString(),
                    Day = PersianDatePicker.SelectedDate.Day,
                    Month = PersianDatePicker.SelectedDate.Month
                }
        });

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "پیغام", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);
                return;
            }


            _notifier.ShowSuccess(response.Message);
            if (Owner is MainWindow.MainWindow win)
            {
                var items = _customerService.GetAll(new CustomerGetAllRequest());
                win.GridCustomerScore.ItemsSource = items.Entity.ToList();
                win.GridCustomerScore.Items.Refresh();
                win.ChipCustomerMaxRegistrationNumber.Content = response.Entity.RegistrationNumber;
            }
            
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Button_Click(sender, e);
        }
    }
}
