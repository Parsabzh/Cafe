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
using Cafe7.Service.InvoiceItemModule.Implementation;
using Cafe7.Service.InvoiceItemModule.Messaging.Request;
using Cafe7.Service.InvoiceItemModule.ViewModel;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Cafe7.Ui.Windows.Invoice
{
    /// <summary>
    /// Interaction logic for InvoiceReportWindow.xaml
    /// </summary>
    public partial class InvoiceReportWindow : Window
    {
        private readonly InvoiceItemService _invoiceItemService;
        private readonly Notifier _notifier;
        public InvoiceReportWindow()
        {
            InitializeComponent();
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

        private void ButtonInvoiceDateSearch_OnClick(object sender, RoutedEventArgs e)
        {
            var response = _invoiceItemService.GetByDate(new InvoiceItemGetByDateRequest
            {
                Date = PersianDatePicker.SelectedDate.ToString()
            });
            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "اخطار", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);
                return;
            }
            _notifier.ShowSuccess(response.Message);
            GridInvoiceItem.ItemsSource = response.Entity.ToList();
            GridInvoiceItem.Items.Refresh();
            var row = response.Entity.Aggregate<InvoiceItemReportViewModel, decimal>(0, (current, item) => (item.Price * item.Qty) + current);
            LabelTotal.Content = row.ToString("N0");
            LabelTotalWithOff.Content = response.TotalOff;
        }
    }
}
