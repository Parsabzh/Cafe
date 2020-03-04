using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Cafe7.Service.ProductModule.Implementation;
using Cafe7.Service.ProductModule.Messaging.Request;
using Cafe7.Service.ProductModule.ViewModel;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Cafe7.Ui.Windows.ProductWindow
{
    /// <summary>
    /// Interaction logic for ProductInsertWindow.xaml
    /// </summary>
    public partial class ProductInsertWindow : Window
    {
        private readonly ProductService _productService;
        private string _type;
        private Notifier _notifier;
        public ProductInsertWindow()
        {
            InitializeComponent();
            var culture = new CultureInfo("fa-IR")
            {
                NumberFormat =
                {
                    NumberDecimalSeparator = ".",
                    CurrencyDecimalSeparator = "."
                }
            };
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            _productService = new ProductService();
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

        private void ButtonInsert_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var ctrl in Grid.Children)
            {
                if (ctrl.GetType() != typeof(RadioButton)) continue;
                if (((RadioButton)ctrl).IsChecked == true)
                {
                    _type = ((RadioButton)ctrl).Content.ToString();
                }
            }

            if (TextBoxName.Text == "" || TextBoxPrice.Text == "")
            {
                _notifier.ShowWarning("لطفا قسمت نام و قیمت را پر نمیاید");
                return;
            }
            var regex = new Regex(@"^[1-9]\d*(\.\d+)?$");
         
                if (!regex.IsMatch(TextBoxPrice.Text)) { 
                MessageBox.Show("لطفا در جاهای تعیین شده،عدد وارد کنید", "اخطار", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);
                return; }
            
            var response = _productService.Insert(new ProductInsertRequest
            {
                Entity = new ProductViewModel()
                {
                    Name = TextBoxName.Text.Trim(),
                    Price = decimal.Parse(TextBoxPrice.Text.Trim()),
                    Type = _type,

                }
            });
            if (!response.IsSuccess)
            {
                _notifier.ShowError(response.Message);
                return;
            }

            _notifier.ShowSuccess(response.Message);
            TextBoxPrice.Text = "";
            TextBoxName.Text = "";
            TextBoxName.Focus();


        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonInsert_OnClick(sender, e);
            }
        }
    }
}

