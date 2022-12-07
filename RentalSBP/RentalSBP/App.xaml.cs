using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RentalSBP
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly Entities.RentalSBPEntities DataBase = new Entities.RentalSBPEntities();

        public static ConnectionWindow Emploee_id = new ConnectionWindow();

    }
}
