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
using Cafe7.Service.BackUp.Implementation;
using Cafe7.Service.BackUp.Messaging.Request;
using Cafe7.Service.CustomerModule.Implementation;
using Cafe7.Service.CustomerModule.Messaging.Request;
using Cafe7.Service.InvoiceItemModule.Implementation;
using Cafe7.Service.InvoiceItemModule.Messaging.Request;
using Cafe7.Service.InvoiceModule.Implementation;
using Cafe7.Service.InvoiceModule.Messaging.Request;
using Cafe7.Ui.Windows.Customer;
using Cafe7.Ui.Windows.Invoice;
using Cafe7.Ui.Windows.ProductWindow;
using MaterialDesignThemes.Wpf;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Cafe7.Ui.Windows.MainWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly InvoiceService _invoiceService;
        private readonly InvoiceItemService _invoiceItemService;
        private BackUpService _backUpService;
        private Notifier _notifier;

        public MainWindow()
        {
            InitializeComponent();
            _backUpService=new BackUpService();
            _invoiceService = new InvoiceService();
            var customerService = new CustomerService();
            _invoiceItemService = new InvoiceItemService();
            var x = customerService.MaxRegistrationNumber();
            ChipCustomerMaxRegistrationNumber.Content = x.MaxRegistrationNumber;
            var response = customerService.GetAll(new CustomerGetAllRequest());
            var lst = response.Entity.ToList();
            GridCustomerScore.ItemsSource = lst;
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

        private void MenuItemInsert_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new CustomerInsertWindow()
            {
                Owner = this
            };
            win.ShowDialog();
        }

        private void MenuItemUpdateDelete_OnClick(object sender, RoutedEventArgs e)
        {

            var win = new CustomerUpdateDeleteWindow() { Owner = this };
            win.ShowDialog();
        }

        private void MenuItemProductInsert_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new ProductInsertWindow();
            win.ShowDialog();
        }

        private void MenuItemDeleteInsertProduct_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new ProductUpdateDeleteWindow();
            win.ShowDialog();
        }

        private void ButtonAddInvoice_OnClick(object sender, RoutedEventArgs e)
        {

            var win = new InvoiceInsertWindow
            {
                Owner = this
            };
            win.Show();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {


            switch (e.Key)
            {
                case Key.F5:
                    {
                        var win = new InvoiceInsertWindow
                        {
                            Owner = this
                        };
                        win.ShowDialog();
                        break;

                    }
                case Key.F6:
                    {
                        var win = new InvoiceDeleteWindow();
                        win.ShowDialog();
                        break;
                    }

            }
        }



        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new InvoiceReportWindow();
            win.ShowDialog();
        }

        private void MenuItemInvoiceDelete_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new InvoiceDeleteWindow{Owner = this};
            win.ShowDialog();
        }

        private void ButtonSearchInvoice_OnClick(object sender, RoutedEventArgs e)
        {
            if (TextBoxTableNumber.Text == "")
            {
                _notifier.ShowWarning("لطفا شماره میز را وارد کنید");
                return;
            }
            var response = _invoiceService.GetByTableNumber(new InvoiceGetByTableNumberRequest
            {
                TableNumber = int.Parse(TextBoxTableNumber.Text)
            });
            if (!response.IsSuccess)
            {
                _notifier.ShowError(response.Message);
                return;
            }
            var invoiceItemViewModel = _invoiceItemService.GetByInvoiceId(new InvoiceItemGetByInvoiceIdRequest
            {
                InvoiceId = response.Entity.Id
            });

            TextBlockTotal.Text = decimal.Parse(response.Entity.Total).ToString("####.00");
            GridInvoiceItem.ItemsSource = invoiceItemViewModel.Entity.ToList();
            LabelDate.Content=response.Entity.Date;
        }

        private void MenuItemScore_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new CustomerScoreWindow() { Owner = this };
            win.ShowDialog();
        }

        private void MenuItemBirthday_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new CustomerBirthdayWindow();
            win.ShowDialog();
        }

        private void MenuItemCustomerProfile_OnClick(object sender, RoutedEventArgs e)
        {
           var win=new CustomerProfileWindow();
            win.ShowDialog();
        }

        private void MenuItemSetPoint_OnClick(object sender, RoutedEventArgs e)
        {
          var win=new CustomerSetPointWindow();
            win.ShowDialog();
        }

        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            var response = _backUpService.Backup(new BackupRequest());
            MessageBox.Show(response.Message);
        }

        private void ButtonRestore_OnClick(object sender, RoutedEventArgs e)
        {
            var response = _backUpService.Restore(new RestoreRequest());
            MessageBox.Show(response.Message);
        }

    }
}
