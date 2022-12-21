using RentalSBP.Entities;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RentalSBP
{
    /// <summary>
    /// Логика взаимодействия для newOrders.xaml
    /// </summary>
    public partial class newOrders : Window
    {
        Entities.RentalSBPEntities db = new Entities.RentalSBPEntities();
        Entities.Emploee nowEmploee;
        public newOrders(ref Entities.Emploee authEmploe)
        {
            InitializeComponent();
            DataGridClients.ItemsSource = db.Clients.ToList();
            cbService.ItemsSource = db.Services.ToList();
            nowEmploee = authEmploe;
            massComboBoxs[0] = cbService;
        }
        ComboBox[] massComboBoxs = new ComboBox[100];
        int indexNameComboBoxs = 1;
        private void AddServeses(object sender, RoutedEventArgs e)
        {
            if (indexNameComboBoxs < 9)
            {
                
                ComboBox comboBox = new ComboBox();
                comboBox.Name = "cbService" + indexNameComboBoxs;
                indexNameComboBoxs++;
                comboBox.DisplayMemberPath = "Name";
                comboBox.Margin = new Thickness(10, 0, 10, 5);
                comboBox.ItemsSource = db.Services.ToList();
                comboBox.SelectedIndex = 0;
                massComboBoxs[indexNameComboBoxs - 1] = comboBox;
                spServices.Children.Add(comboBox);
            }
            else
            {
                MessageBox.Show("Вы достигли лимита выбора услуг.");
            }
        }

        private void SearchPhone_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (SearchPhone.Text != "")
            {
                var _itemSourceList = new CollectionViewSource() { Source = db.Clients.ToList() };

                //now we add our Filter
                _itemSourceList.Filter += new FilterEventHandler(Filter);

                // ICollectionView the View/UI part 
                ICollectionView Itemlist = _itemSourceList.View;

                DataGridClients.ItemsSource = Itemlist;
            }
            else
            {
                DataGridClients.ItemsSource = db.Clients.ToList();
            }
        }

        public void Filter(object o, FilterEventArgs e)
        {
            string filterText = SearchPhone.Text;

            var obj = e.Item as Entities.Client;
            if (obj != null)
            {
                if (obj.Phone.Contains(filterText))
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }

        private void AddClient(object sender, RoutedEventArgs e)
        {
            Window g = new NewClient();
            g.ShowDialog();
        }

        private void AddOrder(object sender, RoutedEventArgs e)
        {
            var client = DataGridClients.SelectedItem as Entities.Client;
            if (client != null)
            {
                DateTime dateTime = DateTime.Now;
                Entities.Order order = new Entities.Order()
                {
                    DateCreate = DateTime.Today.Date,
                    TimeOrder = dateTime.TimeOfDay,
                    Id_Client = client.Id,
                    CodeOrder = client.Id + "/" + dateTime.Date.ToString("dd.MM.yyyy"),
                    Id_OrderStatus = 1,
                    TimeRental = Convert.ToDouble(timeRentHour.Text),
                    Id_Creator_Employee = nowEmploee.Id
                };
                Service serviceNow = null;
                string nameService;

                using (Entities.RentalSBPEntities context = new Entities.RentalSBPEntities())
                {
                    context.Orders.Add(order);
                    context.SaveChanges();

                    for (int i = 0; i < indexNameComboBoxs ; i++)
                    {
                        nameService = massComboBoxs[i].Text;
                        serviceNow = context.Services.Where(b => b.Name == nameService).FirstOrDefault();
                        Entities.ServicesOrder servicesOrder = new Entities.ServicesOrder()
                        {
                            Id_Order = order.Id,
                            Id_Service = serviceNow.Id,
                        };
                        context.ServicesOrders.Add(servicesOrder);
                        context.SaveChanges();
                    }
                    MessageBox.Show("Заказ создан");
                }
            }
        }
    }
}
