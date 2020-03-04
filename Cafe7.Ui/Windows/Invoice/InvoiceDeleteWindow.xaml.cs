using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Cafe7.Service.CustomerModule.Implementation;
using Cafe7.Service.CustomerModule.Messaging.Request;
using Cafe7.Service.InvoiceItemModule.Implementation;
using Cafe7.Service.InvoiceItemModule.Messaging.Request;
using Cafe7.Service.InvoiceItemModule.ViewModel;
using Cafe7.Service.InvoiceModule.Implementation;
using Cafe7.Service.InvoiceModule.Messaging.Request;
using Cafe7.Service.InvoiceModule.ViewModels;
using Cafe7.Service.ProductModule.Implementation;
using Cafe7.Service.ProductModule.Messaging.Request;
using Cafe7.Service.ProductModule.ViewModel;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;


namespace Cafe7.Ui.Windows.Invoice
{
    /// <inheritdoc cref="InvoiceDeleteWindow" />
    /// <summary>
    /// Interaction logic for InvoiceDeleteWindow.xaml
    /// </summary>
    public partial class InvoiceDeleteWindow
    {
        private readonly InvoiceService _invoiceService;
        private readonly InvoiceItemService _invoiceItemService;
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;
        private List<InvoiceItemViewModel> _lst;
        private readonly Notifier _notifier;
        private decimal _y, _x = 0;

        public InvoiceDeleteWindow()
        {
            InitializeComponent();
            _invoiceService = new InvoiceService();
            _invoiceItemService = new InvoiceItemService();
            _productService = new ProductService();
            _customerService = new CustomerService();
            _lst = new List<InvoiceItemViewModel>();

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
            InputComboBox();
        }

        private void ButtonTableNumberSearch_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxTableNumberSearch.Text == "")
            {
                _notifier.ShowError("لطفا شماره میز را وارد نمایید");
                return;
            }
            var response = _invoiceService.GetByTableNumber(new InvoiceGetByTableNumberRequest
            {
                TableNumber = int.Parse(TextBoxTableNumberSearch.Text)
            });
            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "اخطار", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);
                return;
            }
            var invoiceItemViewModel = _invoiceItemService.GetByInvoiceId(new InvoiceItemGetByInvoiceIdRequest
            {
                InvoiceId = response.Entity.Id
            });
            LabelTotal.Content = decimal.Parse(response.Entity.Total).ToString("####.00");
            _lst = invoiceItemViewModel.Entity.ToList();
            GridInvoiceItem.ItemsSource = invoiceItemViewModel.Entity.ToList();
            GridInvoiceItem.Items.Refresh();
            ButtonUpdate.IsEnabled = true;
            ButtonDelete.IsEnabled = true;
            ButtonAddToInvoice.IsEnabled = true;
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (TextBoxTableNumberSearch.Text == "")
            {
                _notifier.ShowError("لطفا شماره میز را وارد نمایید");
                return;
            }
            var response = _invoiceService.GetByTableNumber(new InvoiceGetByTableNumberRequest
            {
                TableNumber = int.Parse(TextBoxTableNumberSearch.Text)
            });
            _invoiceItemService.Delete(new InvoiceItemDeleteRequest
            {
                Id = response.Entity.Id
            });

            var deleteInvoice = _invoiceService.Delete(new InvoiceDeleteRequest
            {
                Id = response.Entity.Id
            });

            _notifier.ShowWarning(deleteInvoice.Message);
            ButtonUpdate.IsEnabled = false;
            ButtonDelete.IsEnabled = false;
            ButtonAddToInvoice.IsEnabled = false;
            if (!(Owner is MainWindow.MainWindow win)) return;
            var items = _customerService.GetAll(new CustomerGetAllRequest());
            win.GridCustomerScore.ItemsSource = items.Entity.ToList();
            win.GridCustomerScore.Items.Refresh();

        }

        private void ButtonAddToInvoice_Click(object sender, RoutedEventArgs e)
        {

            decimal y = 0;
            var regex = new Regex("^[0-9\\s]{0,10}$");
            foreach (var ctl in Grid.Children)
            {
                if (ctl.GetType() != typeof(TextBox)) continue;
                if (regex.IsMatch(((TextBox)ctl).Text)) continue;
                MessageBox.Show("لطفا در جاهای تعیین شده،عدد وارد کنید", "اخطار", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);
                return;
            }
            if (TextBoxCustomerNum.Text == "")
            {
                MessageBox.Show("لطفا شماره اشتراک را وارد کنید", "اخطار", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);
                return;
            }
            var customerId = _customerService.GetByRegistartionNumber(new CustomerGetByRegistrationNumberRequest
            {
                RegistrationNumber = int.Parse(TextBoxCustomerNum.Text.Trim())
            });
            if (!customerId.IsSuccess)
            {
                MessageBox.Show(customerId.Message, "توجه", MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.None, MessageBoxOptions.RightAlign);
                return;
            }

      
            if (TextBoxCoffeeNum.Text != "" && ComboBoxCoffe.Text != "" && TextBoxCustomerNum.Text.Trim() != "")

            {

                if (_lst.Any(m => m.PName == ComboBoxCoffe.Text) && _lst.Any(m => m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim())))
                {
                    var c = _lst.SingleOrDefault(m => m.PName == ComboBoxCoffe.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim()));
                    var x = _lst.IndexOf(c);
                    var newInvoiceItem = new InvoiceItemViewModel
                    {
                        Registration = c.Registration,
                        Price = c.Price,
                        PName = c.PName,
                        Qty = c.Qty + int.Parse(TextBoxCoffeeNum.Text)
                    };
                    _lst[x] = newInvoiceItem;
                    TextBoxCoffeeNum.Clear();
                    ComboBoxCoffe.Text = "";

                }
                else
                {
                    _lst.Add(new InvoiceItemViewModel
                    {
                        PName = ComboBoxCoffe.Text,
                        Qty = int.Parse(TextBoxCoffeeNum.Text),
                        Price = decimal.Parse(_productService.Search(new ProductSearchRequest
                        {
                            Entity = new ProductViewModel
                            {
                                Name = ComboBoxCoffe.Text
                            }
                        }).Entity.Price.ToString("####.00000")),
                        Registration = int.Parse(TextBoxCustomerNum.Text.Trim())
                    });
                    TextBoxCoffeeNum.Clear();
                    ComboBoxCoffe.Text = "";
                }
            }

            if (TextBoxColdDrink.Text != "" && ComboBoxColdDrink.Text != "" && TextBoxCustomerNum.Text.Trim() != "")
            {
                if (_lst.Any(m => m.PName == ComboBoxColdDrink.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim())))
                {
                    var c = _lst.SingleOrDefault(m => m.PName == ComboBoxCoffe.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim()));
                    var x = _lst.IndexOf(c);
                    var newInvoiceItem = new InvoiceItemViewModel
                    {
                        Registration = c.Registration,
                        Price = c.Price,
                        PName = c.PName,
                        Qty = c.Qty + int.Parse(TextBoxColdDrink.Text)
                    };
                    _lst[x] = newInvoiceItem;
                    TextBoxColdDrink.Clear();
                    ComboBoxColdDrink.Text = "";

                }
                else
                {

                    _lst.Add(new InvoiceItemViewModel
                    {
                        PName = ComboBoxColdDrink.Text,
                        Qty = int.Parse(TextBoxColdDrink.Text),
                        Price = decimal.Parse(_productService.Search(new ProductSearchRequest
                        {
                            Entity = new ProductViewModel
                            {
                                Name = ComboBoxColdDrink.Text
                            }
                        }).Entity.Price.ToString("####.00")),
                        Registration = int.Parse(TextBoxCustomerNum.Text.Trim())
                    });
                    TextBoxColdDrink.Clear();
                    ComboBoxColdDrink.Text = "";
                }
            }

            if (TextBoxBreakFast.Text != "" && ComboBoxBreakFast.Text != "" && TextBoxCustomerNum.Text.Trim() != "")
            {
                if (_lst.Any(m => m.PName == ComboBoxBreakFast.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim())))
                {
                    var c = _lst.SingleOrDefault(m => m.PName == ComboBoxBreakFast.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim()));
                    var x = _lst.IndexOf(c);
                    var newInvoiceItem = new InvoiceItemViewModel
                    {
                        Price = c.Price,
                        PName = c.PName,
                        Qty = c.Qty + int.Parse(TextBoxBreakFast.Text),
                        Registration = c.Registration
                    };
                    _lst[x] = newInvoiceItem;
                    TextBoxBreakFast.Clear();
                    ComboBoxBreakFast.Text = "";

                }
                else
                {
                    _lst.Add(new InvoiceItemViewModel
                    {
                        PName = ComboBoxBreakFast.Text,
                        Qty = int.Parse(TextBoxBreakFast.Text),
                        Price = decimal.Parse(_productService.Search(new ProductSearchRequest
                        {
                            Entity = new ProductViewModel
                            {
                                Name = ComboBoxBreakFast.Text
                            }
                        }).Entity.Price.ToString("####.00")),
                        Registration = int.Parse(TextBoxCustomerNum.Text.Trim())
                    });
                    TextBoxBreakFast.Clear();
                    ComboBoxBreakFast.Text = "";
                }
            }

            if (TextBoxHotDrink.Text != "" && ComboBoxHotDrink.Text != "" && TextBoxCustomerNum.Text != "")
            {
                if (_lst.Any(m => m.PName == ComboBoxHotDrink.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim())))
                {
                    var c = _lst.SingleOrDefault(m => m.PName == ComboBoxHotDrink.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim()));
                    var x = _lst.IndexOf(c);
                    var newInvoiceItem = new InvoiceItemViewModel
                    {
                        Registration = c.Registration,
                        Price = c.Price,
                        PName = c.PName,
                        Qty = c.Qty + int.Parse(TextBoxHotDrink.Text)
                    };
                    _lst[x] = newInvoiceItem;
                    TextBoxHotDrink.Clear();
                    ComboBoxHotDrink.Text = "";

                }
                else
                {
                    _lst.Add(new InvoiceItemViewModel
                    {
                        PName = ComboBoxHotDrink.Text,
                        Qty = int.Parse(TextBoxHotDrink.Text),
                        Price = decimal.Parse(_productService.Search(new ProductSearchRequest
                        {
                            Entity = new ProductViewModel
                            {
                                Name = ComboBoxHotDrink.Text
                            }
                        }).Entity.Price.ToString("####.00")),
                        Registration = int.Parse(TextBoxCustomerNum.Text.Trim())
                    });
                    TextBoxHotDrink.Clear();
                    ComboBoxHotDrink.Text = "";
                }
            }

            if (TextBoxIce.Text != "" && ComboBoxIce.Text != "" && TextBoxCustomerNum.Text != "")
            {
                if (_lst.Any(m => m.PName == ComboBoxIce.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim())))
                {
                    var c = _lst.SingleOrDefault(m => m.PName == ComboBoxIce.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim()));
                    var x = _lst.IndexOf(c);
                    var newInvoiceItem = new InvoiceItemViewModel
                    {
                        Registration = c.Registration,
                        Price = c.Price,
                        PName = c.PName,
                        Qty = c.Qty + int.Parse(TextBoxIce.Text)
                    };
                    _lst[x] = newInvoiceItem;
                    TextBoxIce.Clear();
                    ComboBoxIce.Text = "";

                }
                else
                {
                    _lst.Add(new InvoiceItemViewModel
                    {
                        PName = ComboBoxIce.Text,
                        Qty = int.Parse(TextBoxIce.Text),
                        Price = decimal.Parse(_productService.Search(new ProductSearchRequest
                        {
                            Entity = new ProductViewModel
                            {
                                Name = ComboBoxIce.Text
                            }
                        }).Entity.Price.ToString("####.00")),
                        Registration = int.Parse(TextBoxCustomerNum.Text.Trim())
                    });
                    TextBoxIce.Clear();
                    ComboBoxIce.Text = "";
                }
            }

            if (TextBoxCoctel.Text != "" && ComboBoxCoctel.Text != "" && TextBoxCustomerNum.Text != "")
            {
                if (_lst.Any(m => m.PName == ComboBoxCoctel.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim())))
                {
                    var c = _lst.SingleOrDefault(m => m.PName == ComboBoxCoctel.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim()));
                    var x = _lst.IndexOf(c);
                    var newInvoiceItem = new InvoiceItemViewModel
                    {
                        Registration = c.Registration,
                        Price = c.Price,
                        PName = c.PName,
                        Qty = c.Qty + int.Parse(TextBoxCoctel.Text)
                    };
                    _lst[x] = newInvoiceItem;
                    TextBoxCoctel.Clear();
                    ComboBoxCoctel.Text = "";

                }
                else
                {
                    _lst.Add(new InvoiceItemViewModel
                    {
                        PName = ComboBoxCoctel.Text,
                        Qty = int.Parse(TextBoxCoctel.Text),
                        Price = decimal.Parse(_productService.Search(new ProductSearchRequest
                        {
                            Entity = new ProductViewModel
                            {
                                Name = ComboBoxCoctel.Text
                            }
                        }).Entity.Price.ToString("####.00")),
                        Registration = int.Parse(TextBoxCustomerNum.Text.Trim())
                    });
                    TextBoxCoctel.Clear();
                    ComboBoxCoctel.Text = "";
                }
            }

            if (TextBoxSmooti.Text != "" && ComboBoxSmooti.Text != "" && TextBoxCustomerNum.Text != "")
            {
                if (_lst.Any(m => m.PName == ComboBoxSmooti.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim())))
                {
                    var c = _lst.SingleOrDefault(m => m.PName == ComboBoxSmooti.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim()));
                    var x = _lst.IndexOf(c);
                    var newInvoiceItem = new InvoiceItemViewModel
                    {
                        Registration = c.Registration,
                        Price = c.Price,
                        PName = c.PName,
                        Qty = c.Qty + int.Parse(TextBoxSmooti.Text)
                    };
                    _lst[x] = newInvoiceItem;
                    TextBoxSmooti.Clear();
                    ComboBoxSmooti.Text = "";

                }
                else
                {
                    _lst.Add(new InvoiceItemViewModel
                    {
                        PName = ComboBoxSmooti.Text,
                        Qty = int.Parse(TextBoxSmooti.Text),
                        Price = decimal.Parse(_productService.Search(new ProductSearchRequest
                        {
                            Entity = new ProductViewModel
                            {
                                Name = ComboBoxSmooti.Text
                            }
                        }).Entity.Price.ToString("####.00")),
                        Registration = int.Parse(TextBoxCustomerNum.Text.Trim())
                    });
                    TextBoxSmooti.Clear();
                    ComboBoxSmooti.Text = "";
                }
            }

            if (TextBoxHearbalTea.Text != "" && ComboBoxHearbalTea.Text != "" && TextBoxCustomerNum.Text != "")
            {
                if (_lst.Any(m => m.PName == ComboBoxHearbalTea.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim())))
                {
                    var c = _lst.SingleOrDefault(m => m.PName == ComboBoxHearbalTea.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim()));
                    var x = _lst.IndexOf(c);
                    var newInvoiceItem = new InvoiceItemViewModel
                    {
                        Registration = c.Registration,
                        Price = c.Price,
                        PName = c.PName,
                        Qty = c.Qty + int.Parse(TextBoxHearbalTea.Text)
                    };
                    _lst[x] = newInvoiceItem;
                    TextBoxHearbalTea.Clear();
                    ComboBoxHearbalTea.Text = "";

                }
                else
                {
                    _lst.Add(new InvoiceItemViewModel
                    {
                        PName = ComboBoxHearbalTea.Text,
                        Qty = int.Parse(TextBoxHearbalTea.Text),
                        Price = decimal.Parse(_productService.Search(new ProductSearchRequest
                        {
                            Entity = new ProductViewModel
                            {
                                Name = ComboBoxHearbalTea.Text
                            }
                        }).Entity.Price.ToString("####.00")),
                        Registration = int.Parse(TextBoxCustomerNum.Text.Trim())
                    });

                    TextBoxHearbalTea.Clear();
                    ComboBoxHearbalTea.Text = "";
                }
            }

            if (TextBoxHerabalTeaSpecial.Text != "" && ComboBoxHerbalTeaSpecial.Text != "" && TextBoxCustomerNum.Text != "")
            {
                if (_lst.Any(m => m.PName == ComboBoxHerbalTeaSpecial.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim())))
                {
                    var c = _lst.SingleOrDefault(m => m.PName == ComboBoxHerbalTeaSpecial.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim()));
                    var x = _lst.IndexOf(c);
                    var newInvoiceItem = new InvoiceItemViewModel
                    {
                        Registration = c.Registration,
                        Price = c.Price,
                        PName = c.PName,
                        Qty = c.Qty + int.Parse(TextBoxHerabalTeaSpecial.Text)
                    };
                    _lst[x] = newInvoiceItem;
                    TextBoxHerabalTeaSpecial.Clear();
                    ComboBoxHerbalTeaSpecial.Text = "";

                }
                else
                {

                    _lst.Add(new InvoiceItemViewModel
                    {
                        PName = ComboBoxHerbalTeaSpecial.Text,
                        Qty = int.Parse(TextBoxHerabalTeaSpecial.Text),
                        Price = decimal.Parse(_productService.Search(new ProductSearchRequest
                        {
                            Entity = new ProductViewModel
                            {
                                Name = ComboBoxHerbalTeaSpecial.Text
                            }
                        }).Entity.Price.ToString("####.00")),
                        Registration = int.Parse(TextBoxCustomerNum.Text.Trim())
                    });

                    TextBoxHerabalTeaSpecial.Clear();
                    ComboBoxHerbalTeaSpecial.Text = "";
                }
            }

            if (TextBoxCake.Text != "" && ComboBoxCake.Text != "" && TextBoxCustomerNum.Text != "")
            {
                if (_lst.Any(m => m.PName == ComboBoxCake.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim())))
                {
                    var c = _lst.SingleOrDefault(m => m.PName == ComboBoxCake.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim()));
                    var x = _lst.IndexOf(c);
                    var newInvoiceItem = new InvoiceItemViewModel
                    {
                        Registration = c.Registration,
                        Price = c.Price,
                        PName = c.PName,
                        Qty = c.Qty + int.Parse(TextBoxCake.Text)
                    };
                    _lst[x] = newInvoiceItem;
                    TextBoxCake.Clear();
                    ComboBoxCake.Text = "";

                }
                else
                {
                    _lst.Add(new InvoiceItemViewModel
                    {
                        PName = ComboBoxCake.Text,
                        Qty = int.Parse(TextBoxCake.Text),
                        Price = decimal.Parse(_productService.Search(new ProductSearchRequest
                        {
                            Entity = new ProductViewModel
                            {
                                Name = ComboBoxCake.Text
                            }
                        }).Entity.Price.ToString("####.00")),
                        Registration = int.Parse(TextBoxCustomerNum.Text.Trim())
                    });

                    TextBoxCake.Clear();
                    ComboBoxCake.Text = "";
                }
            }

            if (TextBoxShake.Text != "" && ComboBoxShake.Text != "" && TextBoxCustomerNum.Text != "")
            {
                if (_lst.Any(m => m.PName == ComboBoxShake.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim())))
                {
                    var c = _lst.SingleOrDefault(m => m.PName == ComboBoxShake.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim()));
                    var x = _lst.IndexOf(c);
                    var newInvoiceItem = new InvoiceItemViewModel
                    {
                        Registration = c.Registration,
                        Price = c.Price,
                        PName = c.PName,
                        Qty = c.Qty + int.Parse(TextBoxShake.Text)
                    };
                    _lst[x] = newInvoiceItem;
                    TextBoxShake.Clear();
                    ComboBoxShake.Text = "";

                }
                else
                {
                    _lst.Add(new InvoiceItemViewModel
                    {
                        PName = ComboBoxShake.Text,
                        Qty = int.Parse(TextBoxShake.Text),
                        Price = decimal.Parse(_productService.Search(new ProductSearchRequest
                        {
                            Entity = new ProductViewModel
                            {
                                Name = ComboBoxShake.Text
                            }
                        }).Entity.Price.ToString("####.00")),
                        Registration = int.Parse(TextBoxCustomerNum.Text.Trim())
                    });

                    TextBoxShake.Clear();
                    ComboBoxShake.Text = "";
                }
            }

            if (TextBoxGlasse.Text != "" && ComboBoxGlasse.Text != "" && TextBoxCustomerNum.Text != "")
            {
                if (_lst.Any(m => m.PName == ComboBoxGlasse.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim())))
                {
                    var c = _lst.SingleOrDefault(m => m.PName == ComboBoxGlasse.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim()));
                    var x = _lst.IndexOf(c);
                    var newInvoiceItem = new InvoiceItemViewModel
                    {
                        Registration = c.Registration,
                        Price = c.Price,
                        PName = c.PName,
                        Qty = c.Qty + int.Parse(TextBoxGlasse.Text)
                    };
                    _lst[x] = newInvoiceItem;
                    TextBoxGlasse.Clear();
                    ComboBoxGlasse.Text = "";

                }
                else
                {
                    _lst.Add(new InvoiceItemViewModel
                    {
                        PName = ComboBoxGlasse.Text,
                        Qty = int.Parse(TextBoxGlasse.Text),
                        Price = decimal.Parse(_productService.Search(new ProductSearchRequest
                        {
                            Entity = new ProductViewModel
                            {
                                Name = ComboBoxGlasse.Text
                            }
                        }).Entity.Price.ToString("####.00"))
                   ,
                        Registration = int.Parse(TextBoxCustomerNum.Text.Trim())
                    });

                    TextBoxGlasse.Clear();
                    ComboBoxGlasse.Text = "";
                }
            }

            if (TextBoxFarapeh.Text != "" && ComboBoxFarapeh.Text != "" && TextBoxCustomerNum.Text != "")
            {
                if (_lst.Any(m => m.PName == ComboBoxFarapeh.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim())))
                {
                    var c = _lst.SingleOrDefault(m => m.PName == ComboBoxFarapeh.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim()));
                    var x = _lst.IndexOf(c);
                    var newInvoiceItem = new InvoiceItemViewModel
                    {
                        Registration = c.Registration,
                        Price = c.Price,
                        PName = c.PName,
                        Qty = c.Qty + int.Parse(TextBoxFarapeh.Text)
                    };
                    _lst[x] = newInvoiceItem;
                    TextBoxFarapeh.Clear();
                    ComboBoxFarapeh.Text = "";

                }
                else
                {
                    _lst.Add(new InvoiceItemViewModel
                    {
                        PName = ComboBoxFarapeh.Text,
                        Qty = int.Parse(TextBoxFarapeh.Text),
                        Price = decimal.Parse(_productService.Search(new ProductSearchRequest
                        {
                            Entity = new ProductViewModel
                            {
                                Name = ComboBoxFarapeh.Text
                            }
                        }).Entity.Price.ToString("####.00")),
                        Registration = int.Parse(TextBoxCustomerNum.Text.Trim())
                    });
                    TextBoxFarapeh.Clear();
                    ComboBoxFarapeh.Text = "";
                }
            }

            if (TextBoxFood.Text != "" && ComboBoxFood.Text != "" && TextBoxCustomerNum.Text != "")
            {
                if (_lst.Any(m => m.PName == ComboBoxFood.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim())))
                {
                    var c = _lst.SingleOrDefault(m => m.PName == ComboBoxFood.Text && m.Registration == int.Parse(TextBoxCustomerNum.Text.Trim()));
                    var x = _lst.IndexOf(c);
                    var newInvoiceItem = new InvoiceItemViewModel
                    {
                        Registration = c.Registration,
                        Price = c.Price,
                        PName = c.PName,
                        Qty = c.Qty + int.Parse(TextBoxFood.Text)
                    };
                    _lst[x] = newInvoiceItem;
                    TextBoxFood.Clear();
                    ComboBoxFood.Text = "";

                }
                else
                {
                    _lst.Add(new InvoiceItemViewModel
                    {
                        PName = ComboBoxFood.Text,
                        Qty = int.Parse(TextBoxFood.Text),
                        Price = decimal.Parse(_productService.Search(new ProductSearchRequest
                        {
                            Entity = new ProductViewModel
                            {
                                Name = ComboBoxFood.Text
                            }
                        }).Entity.Price.ToString("####.00")),
                        Registration = int.Parse(TextBoxCustomerNum.Text.Trim())
                    });
                    TextBoxFood.Clear();
                    ComboBoxFood.Text = "";

                }
            }

            GridInvoiceItem.ItemsSource = _lst;
            GridInvoiceItem.Items.Refresh();

            foreach (var item in _lst)
            {
                var x = item.Price * item.Qty;

                y = x + y;
            }

            if (TextBoxOff.Text != "") LabelTotal.Content = y - (y * (decimal.Parse(TextBoxOff?.Text) / 100));
            else
                LabelTotal.Content = y.ToString("####.00");
            ButtonUpdate.IsEnabled = true;
            ButtonDelete.IsEnabled = true;
            _notifier.ShowSuccess("محصول به لیست اضافه شد");

        }


        private void ButtonUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            if (TextBoxTableNumberSearch.Text == "")
            {
                _notifier.ShowError("لطفا شماره میز را وارد نمایید");
                return;
            }
            var regex = new Regex("^[0-9\\s]{0,10}$");
            foreach (var ctl in Grid.Children)
            {
                if (ctl.GetType() != typeof(TextBox)) continue;
                if (regex.IsMatch(((TextBox)ctl).Text)) continue;
                MessageBox.Show("لطفا در جاهای تعیین شده،عدد وارد کنید", "اخطار", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);
                return;
            }
            var response = _invoiceService.GetByTableNumber(new InvoiceGetByTableNumberRequest
            {
                TableNumber = int.Parse(TextBoxTableNumberSearch.Text)
            });

            var invoiveItemResponse = _invoiceItemService.Update(new InvoiceItemUpdateRequest
            {
                Entity = new InvoiceItemUpdateViewModel
                {
                    InvoiceItemViewModels = _lst,
                    InvoiceId = response.Entity.Id
                }
            });


            if (!invoiveItemResponse.IsSuccess)
            {
                _notifier.ShowError(invoiveItemResponse.Message);
                return;
            }

            var invoiceResponse = _invoiceService.Update(new InvoiceUpdateRequest
            {
                Entity = new InvoiceViewModel
                {
                    Id = response.Entity.Id,
                    Date = response.Entity.Date,
                    Total = decimal.Parse(LabelTotal.Content.ToString()).ToString("####.00"),
                    TableNumber = response.Entity.TableNumber
                }
            });
            if (!invoiceResponse.IsSuccess)
            {
                _notifier.ShowError(invoiveItemResponse.Message);
                return;
            }
            _notifier.ShowSuccess(invoiveItemResponse.Message);
            ButtonUpdate.IsEnabled = false;
            ButtonDelete.IsEnabled = false;
            ButtonAddToInvoice.IsEnabled = false;
            if (!(Owner is MainWindow.MainWindow win)) return;
            var items = _customerService.GetAll(new CustomerGetAllRequest());
            win.GridCustomerScore.ItemsSource = items.Entity.ToList();
            win.GridCustomerScore.Items.Refresh();

        }

        public void InputComboBox()
        {
            var response = _productService.GetByType(new ProductGetByTypeRequest
            {
                Type = "قهوه"
            });


            ComboBoxCoffe.ItemsSource = response.Entity;

            response = _productService.GetByType(new ProductGetByTypeRequest
            {
                Type = "کیک"
            });


            ComboBoxCake.ItemsSource = response.Entity;

            response = _productService.GetByType(new ProductGetByTypeRequest
            {
                Type = "نوشیدنی گرم"
            });


            ComboBoxHotDrink.ItemsSource = response.Entity;
            response = _productService.GetByType(new ProductGetByTypeRequest
            {
                Type = "نوشیدنی سرد"
            });


            ComboBoxColdDrink.ItemsSource = response.Entity;
            response = _productService.GetByType(new ProductGetByTypeRequest
            {
                Type = "آیس"
            });

            ComboBoxIce.ItemsSource = response.Entity;
            response = _productService.GetByType(new ProductGetByTypeRequest
            {
                Type = "کوکتل"
            });


            ComboBoxCoctel.ItemsSource = response.Entity;
            response = _productService.GetByType(new ProductGetByTypeRequest
            {
                Type = "اسموتی"
            });


            ComboBoxSmooti.ItemsSource = response.Entity;
            response = _productService.GetByType(new ProductGetByTypeRequest
            {
                Type = "دمنوش"
            });



            ComboBoxHearbalTea.ItemsSource = response.Entity;
            response = _productService.GetByType(new ProductGetByTypeRequest
            {
                Type = "دمنوش ویژه"
            });


            ComboBoxHerbalTeaSpecial.ItemsSource = response.Entity;
            response = _productService.GetByType(new ProductGetByTypeRequest
            {
                Type = "شیک"
            });


            ComboBoxShake.ItemsSource = response.Entity;
            response = _productService.GetByType(new ProductGetByTypeRequest
            {
                Type = "گلاسه"
            });



            ComboBoxGlasse.ItemsSource = response.Entity;
            response = _productService.GetByType(new ProductGetByTypeRequest
            {
                Type = "فراپه"
            });


            ComboBoxFarapeh.ItemsSource = response.Entity;
            response = _productService.GetByType(new ProductGetByTypeRequest
            {
                Type = "صبحانه"
            });


            ComboBoxBreakFast.ItemsSource = response.Entity;
            response = _productService.GetByType(new ProductGetByTypeRequest
            {
                Type = "غذاها"
            });


            ComboBoxFood.ItemsSource = response.Entity;
        }

        private void ButtonDataGridItemDelete_OnClick(object sender, RoutedEventArgs e)
        {

            var item = (InvoiceItemViewModel)GridInvoiceItem.SelectedItem;
            var index = _lst.FindIndex(m => m.PName == item.PName && m.Registration == item.Registration);
            _lst.RemoveAt(index);
            GridInvoiceItem.ItemsSource = _lst;
            GridInvoiceItem.Items.Refresh();

            foreach (var invoiceItem in _lst)
            {
                _x = invoiceItem.Price * invoiceItem.Qty;

                _y = _x + _y;
            }


            if (_y == 0)
                ButtonUpdate.IsEnabled = false;
            LabelTotal.Content = _y;
            _x = 0;
            _y = 0;
        }


    }
}
