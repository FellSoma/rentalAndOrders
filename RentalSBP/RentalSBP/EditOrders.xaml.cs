using RentalSBP.Entities;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RentalSBP
{
    /// <summary>
    /// Логика взаимодействия для EditOrders.xaml
    /// </summary>
    public partial class EditOrders : Window
    {
        Entities.RentalSBPEntities db = new Entities.RentalSBPEntities();
        public EditOrders()
        {
            InitializeComponent();
            DataGridOrders.ItemsSource = db.Orders.ToList();
            Statucbx.ItemsSource = db.OrderStatus.ToList();
        }

        private void SomeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            OrderStatu orderStatu = comboBox.SelectedItem as OrderStatu;
            string nameNow = orderStatu.Name;
            switch (orderStatu.Name)
            {
                case "Закрыта":
                    {
                        var nowOrder = DataGridOrders.SelectedItem as Entities.Order;
                        using (Entities.RentalSBPEntities context = new Entities.RentalSBPEntities())
                        {
                            nowOrder.DateClosing = DateTime.Today.Date;
                            nowOrder.Id_OrderStatus = orderStatu.Id;
                            context.Orders.AddOrUpdate(nowOrder);
                            context.SaveChanges();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

       private void Edit(object sender, RoutedEventArgs e)
       {
            DataGridOrders.IsReadOnly = false;
            saveEdit.IsEnabled = true;
       }

        private void Save(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Статус изменён");
            DataGridOrders.IsReadOnly = true;
            saveEdit.IsEnabled = false;

        }
    }
}
