using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Arash.PersianDateControls;
using Cafe7.Service.CustomerModule.Implementation;
using Cafe7.Service.CustomerModule.Messaging.Request;
using Cafe7.Service.CustomerModule.Messaging.Request.ScoreRequest;
using Cafe7.Service.CustomerModule.ViewModel;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Cafe7.Ui.Windows.Customer
{
    /// <summary>
    /// Interaction logic for CustomerScoreWindow.xaml
    /// </summary>
    public partial class CustomerScoreWindow : Window
    {
        private readonly CustomerService _customerService;
        private int _id;
        private readonly ScoreService _scoreService;
        private Notifier _notifier;
       
        public CustomerScoreWindow()
        {
            InitializeComponent();
            _customerService=new CustomerService();
            _scoreService=new ScoreService();
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

        private void ButtonSearch_OnClick(object sender, RoutedEventArgs e)
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
                MessageBox.Show(response.Message);
                return;
            }
            foreach (var ctl in Grid.Children)
            {
                if (ctl.GetType() == typeof(TextBox))
                    ((TextBox)ctl).Text = string.Empty;
            }

            
            _id = response.Entity.Id;
            TextBoxName.Text = response.Entity.Name.Trim();
            TextBoxFamily.Text = response.Entity.Family.Trim();              
            TextBoxMobile.Text = response.Entity.Mobile.Trim();
            TextBoxRegistrationNum.Text = response.Entity.RegistrationNumber.ToString();
          
           var scoreResponse=_scoreService.GetByCustomerId(new ScoreGetByCustomerIdRequest
            {
                CustomerId =_id
            });
           if (!scoreResponse.IsSuccess)
           {_notifier.ShowError(scoreResponse.Message);
               LabelTotalScoreShow.Content = scoreResponse.Entity.Total.ToString("N0");
               ButtonUpdateScore.IsEnabled = true;
               ButtonDeleteScore.IsEnabled = true; return;}
            _notifier.ShowSuccess(scoreResponse.Message);
            LabelTotalScoreShow.Content = scoreResponse.Entity.Total.ToString("N0");
            ButtonUpdateScore.IsEnabled = true;
            ButtonDeleteScore.IsEnabled = true;
        
        }

        private void ButtonUpdateScore_OnClick(object sender, RoutedEventArgs e)
        {
            if (TextBoxScore.Text == "")
            {
                _notifier.ShowWarning("لطفا مقدار امتیاز را وارد نمایید");
                return;
            }

            var response = _scoreService.Update(new ScoreUpdateRequest
            {
                Entity = new ScoreViewModel
                {
                    CustomerId = _id,
                    LastScore = double.Parse(TextBoxScore.Text),
                   
                }
            });
            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message);
                return;             
            }

            _notifier.ShowSuccess(response.Message);
            ButtonUpdateScore.IsEnabled = false;
            ButtonDeleteScore.IsEnabled = false;
            if (Owner is MainWindow.MainWindow win)
            {
                var items = _customerService.GetAll(new CustomerGetAllRequest());
                win.GridCustomerScore.ItemsSource = items.Entity.ToList();
                win.GridCustomerScore.Items.Refresh();
            };
        }

        private void ButtonDeleteScore_OnClick(object sender, RoutedEventArgs e)
        {
            var response = _scoreService.Delete(new ScoreDeleteRequest
            {
                CustomerId = _id,
                ScoreDecrease = TextBoxScoreDecrease.Text
            });
            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message);
                return;
            }

            _notifier.ShowSuccess(response.Message);
            ButtonUpdateScore.IsEnabled = false;
            ButtonDeleteScore.IsEnabled = false;
            if (Owner is MainWindow.MainWindow win)
            {
                var items = _customerService.GetAll(new CustomerGetAllRequest());
                win.GridCustomerScore.ItemsSource = items.Entity.ToList();
                win.GridCustomerScore.Items.Refresh();
            };
        }


        private void TextBoxPercent_OnTextChanged(object sender, TextChangedEventArgs e)
        {           
            if (TextBoxInvoice.Text == "" || TextBoxPercent.Text == "")
            {
                TextBoxScore.Text = "0";
                return;
            }
            var z = decimal.Parse(TextBoxInvoice.Text);
            var y = decimal.Parse(TextBoxPercent.Text);
            var x = z * y;
            TextBoxScore.Text = ((double)x).ToString("N0");
        }

        private void TextBoxInvoice_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxInvoice.Text != "" && TextBoxPercent.Text != "") return;
            TextBoxScore.Text = "0";  
        }
    }
}
