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
using System.Windows.Threading;

namespace RentalSBP
{
    /// <summary>
    /// Логика взаимодействия для AdminProfile.xaml
    /// </summary>
    public partial class AdminProfile : Window
    {
        Entities.RentalSBPEntities db = new Entities.RentalSBPEntities();
        DispatcherTimer timer = new DispatcherTimer();
        int s, m, h;

        Entities.Emploee refEmploee;
        public AdminProfile(ref Entities.Emploee authEmploe)
        {
            InitializeComponent();
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            s++;
            if (s > 59)
            {
                if (m == 5)
                {
                    messageOut.Visibility = Visibility.Visible;
                }
                if (m == 10)
                {
                    s = 0;
                    m = 0;
                    timer.Stop();
                    OutTimer w = new OutTimer();
                    w.Show();
                    this.Close();
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

        private void LoginGistory(object sender, RoutedEventArgs e)
        {
            Window g = new AdminWindow();
            g.ShowDialog();
        }
    }
}
