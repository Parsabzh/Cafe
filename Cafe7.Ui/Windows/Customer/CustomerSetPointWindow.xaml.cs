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
using Cafe7.Service.CustomerModule.Implementation;
using Cafe7.Service.CustomerModule.Messaging.Request.ScoreRequest;
using Cafe7.Service.CustomerModule.ViewModel;

namespace Cafe7.Ui.Windows.Customer
{
    /// <summary>
    /// Interaction logic for CustomerSetPointWindow.xaml
    /// </summary>
    public partial class CustomerSetPointWindow : Window
    {
        private readonly ScoreService _scoreService;
        public CustomerSetPointWindow()
        {
            InitializeComponent();
            _scoreService=new ScoreService();

        }

        private void ButtonSetPoint_Click(object sender, RoutedEventArgs e)
        {
            var response = _scoreService.InsertPoint(new PointInsertRequest
            {
                Entity = new PointViewModel
                {
                    Point = decimal.Parse(TextBoxPoint.Text.Trim())
                }
            });
            if (response.IsSuccess)
            {
                this.Close();
                return;
            }

            MessageBox.Show(response.Message);
        }
    }
}
