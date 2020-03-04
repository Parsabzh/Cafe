using System;
using System.Collections.Generic;
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
using Cafe7.Service.ProductModule.Implementation;
using Cafe7.Service.ProductModule.Messaging.Request;
using Cafe7.Service.ProductModule.ViewModel;

namespace Cafe7.Ui.Windows.ProductWindow
{
    /// <summary>
    /// Interaction logic for ProductUpdateDeleteWindow.xaml
    /// </summary>
    public partial class ProductUpdateDeleteWindow : Window
    {
        private readonly ProductService _productService;
        private int _id = 0;
        private string _type;
        public ProductUpdateDeleteWindow()
        {
            InitializeComponent();
            _productService = new ProductService();
        }
        private void ButtonSearch_OnClick(object sender, RoutedEventArgs e)
        {
            var response = _productService.Search(new ProductSearchRequest
            {
                Entity = new ProductViewModel()
                {
                    Name = TextBoxNameSearch.Text.Trim()
                }
            });
            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "اخطار", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);

                return;
            }

            _id = response.Entity.Id;
            TextBoxName.Text = response.Entity.Name;
            TextBoxPrice.Text = response.Entity.Price.ToString("#####.000000");
            _type = response.Entity.Type;
            MessageBox.Show(response.Message, "پیغام", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RightAlign);
            ButtonDelete.IsEnabled = true;
            ButtonUpdate.IsEnabled = true;
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            var message = MessageBox.Show(" آیا مطمئن هستید؟", "اخطار", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.RightAlign);
            if (message == MessageBoxResult.No) return;
            var response = _productService.Delete(new ProductDeleteRequest
            {
                Id = _id
            });
            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "اخطار", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);

                return;
            }
            foreach (var ctl in Grid.Children)
            {
                if (ctl.GetType() == typeof(TextBox))
                    ((TextBox)ctl).Text = string.Empty;
            }
            MessageBox.Show(response.Message, "پیغام", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RightAlign);
            ButtonDelete.IsEnabled = false;
            ButtonUpdate.IsEnabled = false;
        }

        private void ButtonUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            var regex = new Regex(@"^[1-9]\d*(\.\d+)?$");

            if (!regex.IsMatch(TextBoxPrice.Text))
            {
                MessageBox.Show("لطفا در جاهای تعیین شده،عدد وارد کنید", "اخطار", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);
                return;
            }
            var response = _productService.Update(new ProductUpdateRequest
            {
                Entity = new ProductViewModel()
                {
                    Id = _id,
                    Name = TextBoxName.Text.Trim(),
                    Price = decimal.Parse(TextBoxPrice.Text.Trim()),
                    Type = _type
                }
            });
            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "اخطار", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);

                return;
            }
            foreach (var ctl in Grid.Children)
            {
                if (ctl.GetType() == typeof(TextBox))
                    ((TextBox)ctl).Text = string.Empty;
            }
            MessageBox.Show(response.Message, "پیغام", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RightAlign);
            ButtonDelete.IsEnabled = false;
            ButtonUpdate.IsEnabled = false;
        }
    }
}
