using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace RentalSBP
{
    /// <summary>
    /// Логика взаимодействия для HomeSeniorSeller.xaml
    /// </summary>
    public partial class HomeSeniorSeller : Window
    {
        Entities.RentalSBPEntities db = new Entities.RentalSBPEntities();
        DispatcherTimer timer = new DispatcherTimer();
        int s, m, h;

        Entities.Emploee refEmploee;
        public HomeSeniorSeller(ref Entities.Emploee authEmploe)
        {
            ConnectionWindow connection = new ConnectionWindow();
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
            int id = authEmploe.Id;
            refEmploee = authEmploe;
            ListViewEmploee.ItemsSource = db.Emploees.ToList().Where(b => b.Id == id);
        }

        private void Orders(object sender, RoutedEventArgs e)
        {
            EditOrders g = new EditOrders();
            g.ShowDialog();
        }

        private void NewOrder(object sender, RoutedEventArgs e)
        {
            newOrders g = new newOrders(ref refEmploee);
            g.ShowDialog();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (m >= 10)
            {
                s = 0;
                m = 0;
                timer.Stop();
                OutTimer w = new OutTimer();
                w.Show();
                Close();
            }
            s++;
            if (s > 59)
            {
                if (m == 5)
                {
                    messageOut.Visibility = Visibility.Visible;
                }
                m++;
                s = 0;
                if (m > 59)
                {
                    m = 0;
                    h++;
                }
            }
            timerHome.Text = $"{h}:{m}:{s}";
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            this.Close();
        }
    }
}
