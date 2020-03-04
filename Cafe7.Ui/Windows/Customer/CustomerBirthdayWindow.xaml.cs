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
using Cafe7.Infrastructure.DateTimeHelper;
using Cafe7.Infrastructure.Messaging;
using Cafe7.Infrastructure.PeopleResource;
using Cafe7.Service.CustomerModule.Implementation;
using Cafe7.Service.CustomerModule.Messaging.Request;
using Cafe7.Service.CustomerModule.Messaging.Request.ScoreRequest;
using Cafe7.Service.CustomerModule.Messaging.Response;
using Cafe7.Service.CustomerModule.Messaging.Response.ScoreResponse;
using Cafe7.Service.CustomerModule.ViewModel;
using Cafe7.Service.Sms;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Cafe7.Ui.Windows.Customer
{
    /// <summary>
    /// Interaction logic for CustomerBirthdayWindow.xaml
    /// </summary>
    public partial class CustomerBirthdayWindow : Window
    {
        private readonly CustomerService _customerService;
        private readonly ScoreService _scoreService;
        private List<SmsViewModel> _list;
        private List<CustomerViewModel> _listCustomer;
        private readonly Notifier _notifier;
        public CustomerBirthdayWindow()
        {
            InitializeComponent();
            TextBlockDate.Text = PersianDate.Today.ToString();
            _customerService = new CustomerService();
            _scoreService = new ScoreService();
            _list = new List<SmsViewModel>();
            _listCustomer = new List<CustomerViewModel>();
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



        private void ButtonCustomerSearchBirthday_Click(object sender, RoutedEventArgs e)
        {
            var response = _customerService.Birthday(new CustomerBirthdayRequest
            {
                Month = PersianDate.Today.Month,
                Day = PersianDate.Today.Day
            });
            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message);
                return;
            }

            GridCustomer.ItemsSource = response.Entity.ToList();
            _notifier.ShowSuccess(response.Message);
            ButtonSendSms.IsEnabled = true;
            TextBoxOff.IsEnabled = true;
        }

        private void ButtonSendSms_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxOff.Text == "")
            {
                _notifier.ShowWarning("لطفا مقدار تخفیف را وارد کنید");
                return;
            }
            var response = _customerService.Birthday(new CustomerBirthdayRequest
            {
                Month = PersianDate.Today.Month,
                Day = PersianDate.Today.Day
            });
            _list = response.Entity.Select(item => new SmsViewModel
            {
                Number = item.Mobile,
                Sms = $"{item.Name} {item.Family}  عزیز تولدتون مبارک. ماامروز برای شما {TextBoxOff.Text} درصد تخفیف در نظر گرفته ایم،به امید دیدار شما \n کافه هفت"
            })
                .ToList();
            var smsResponse = Sms.SendSmsToMany(_list);
            if (!smsResponse.IsSuccess)
            {
                MessageBox.Show(smsResponse.Message);
                return;
            }

            _notifier.ShowSuccess(response.Message);
            Close();
        }

        private void ButtonCustomerSearchScore_OnClick(object sender, RoutedEventArgs e)
        {
            var entity = new ScoreMaxMinViewModel
            {
                Min = TextBoxMinScore.Text,
                Max = TextBoxMaxScore.Text
            };
            var response = _scoreService.GetByMaxMin(new ScoreGetByMaxMinRequest
            {
                Entity = entity
            });
            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message);
                GridCustomer.ItemsSource = "";
                GridCustomer.Items.Refresh();
                return;
            }

            _notifier.ShowSuccess(response.Message);
            ButtonSendSmsEvent.IsEnabled = true;
            TextBoxOffEvent.IsEnabled = true;
            GridCustomer.ItemsSource = response.Entity.ToList();
            _listCustomer = response.Entity.ToList();
            GridCustomer.Items.Refresh();
        }

        private void ButtonSendSmsEvent_OnClick(object sender, RoutedEventArgs e)
        {
            if (TextBoxOffEvent.Text == "")
            {
                _notifier.ShowWarning("لطفا مقدار تخفیف را وارد کنید");
                return;
            }
            switch (ComboBoxHotSmsType.SelectedIndex)
            {
                case 0:
                    foreach (var customerViewModel in _listCustomer)
                    {
                        _list.Add(new SmsViewModel
                        {
                            Number = customerViewModel.Mobile,
                            Sms = $"{customerViewModel.Name} {customerViewModel.Family} :) عزیز این عید فرخنده را به شما تبریک میگوییم\n ما برای شما {TextBoxOffEvent.Text} تخفیف در نظر گرفته ایم.به امید دیدار شما \n کافه هفت"
                        });

                    }
                    var s = Sms.SendSmsToMany(_list);
                    _notifier.ShowInformation(s.Message);
                    break;
                case 1:
                    foreach (var customerViewModel in _listCustomer)
                    {
                        _list.Add(new SmsViewModel
                        {
                            Number = customerViewModel.Mobile,
                            Sms = $"{customerViewModel.Name} {customerViewModel.Family} :) عزیز این روز زیبا را به شما تبریک میگوییم\n ما برای شما {TextBoxOffEvent.Text} تخفیف در نظر گرفته ایم.به امید دیدار شما \n کافه هفت"
                        });

                    }
                    s = Sms.SendSmsToMany(_list);
                    _notifier.ShowInformation(s.Message);
                    break;
                default:
                    MessageBox.Show("لطفا نوع مناسبت را اننخاب کنید");
                    return;
            }

            {

            }
        }
    }
}
