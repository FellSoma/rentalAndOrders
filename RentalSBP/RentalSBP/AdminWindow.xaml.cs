using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace RentalSBP
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        Entities.RentalSBPEntities db = new Entities.RentalSBPEntities();
        public AdminWindow()
        {
            InitializeComponent();
            DataGridListHisory.ItemsSource = db.LoginHistories.ToList();
        }
        
        private void SearchPhone_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (SearchPhone.Text != "")
            {
                var _itemSourceList = new CollectionViewSource() { Source = db.LoginHistories.ToList() };

                //now we add our Filter
                _itemSourceList.Filter += new FilterEventHandler(Filter);

                // ICollectionView the View/UI part 
                ICollectionView Itemlist = _itemSourceList.View;

                DataGridListHisory.ItemsSource = Itemlist;
            }
            else
            {
                DataGridListHisory.ItemsSource = db.LoginHistories.ToList();
            }
        }

        public void Filter(object o, FilterEventArgs e)
        {
            string filterText = SearchPhone.Text;

            var obj = e.Item as Entities.LoginHistory;
            if (obj != null)
            {
                if (obj.Login.Contains(filterText))
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
