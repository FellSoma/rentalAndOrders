using RentalSBP.Entities;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brushes = System.Drawing.Brushes;
using Point = System.Drawing.Point;

namespace RentalSBP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Entities.RentalSBPEntities db = new Entities.RentalSBPEntities();
        public MainWindow()
        {
            InitializeComponent();
            //   int i=1;
            //   foreach (var emploee in db.Emploees)
            //   {
            //       string path = @"D:\\Emploees\" + i + ".JPEG"; // Путь к картинкам + их названия
            //       if(i==6)
            //       {
            //            path = @"D:\\Emploees\" + i + ".JPG"; // Путь к картинкам + их названия
            //       }
            //       byte[] imageInBytes = System.IO.File.ReadAllBytes(path); // в массив байт
            //       emploee.Photo = imageInBytes; // массив байт в таблицу
            //       i++;
            //   }
            //   db.SaveChanges();
            //  i = 0;
        }
        private string text = String.Empty;
        private Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();

            //Создадим изображение
            Bitmap result = new Bitmap(Width, Height);

            //Вычислим позицию текста
            int Xpos = rnd.Next(0, Width - 95);
            int Ypos = rnd.Next(15, Height - 25);

            //Добавим различные цвета
            System.Drawing.Brush[] colors = { Brushes.Black,
            Brushes.Red,
            Brushes.RoyalBlue,
            Brushes.Green,
            Brushes.DarkBlue};

            //Укажем где рисовать
            Graphics g = Graphics.FromImage((System.Drawing.Image)result);

            //Пусть фон картинки будет серым
            g.Clear(System.Drawing.Color.Gray);

            //Сгенерируем текст
            text = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 5; ++i)
                text += ALF[rnd.Next(ALF.Length)];

            //Сгенерируем формат текста
            string[] st =
            {
                "Arial",
                "Times New Roman",
                "Lucida Calligraphy"
                };
            //Нарисуем сгенирируемый текст
            g.DrawString(text,
            new Font(st[rnd.Next(st.Length)], 15),
            colors[rnd.Next(colors.Length)],
            new PointF(Xpos, Ypos));

            //Добавим немного помех
            /////Линии из углов
            g.DrawLine(Pens.Black,
            new Point(0, 0),
            new Point(Width - 1, Height - 1));
            g.DrawLine(Pens.Black,
            new Point(0, Height - 1),
            new Point(Width - 1, 0));
            ////Белые точки
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, System.Drawing.Color.White);

            return result;
        }

        //If you get 'dllimport unknown'-, then add 'using System.Runtime.InteropServices;'
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        private void CaphBtn_Click(object sender, RoutedEventArgs e)
        {
            if (text == capchaTB.Text)
            {
                autorizationBtn.IsEnabled = true;
                ImCaph.Visibility = Visibility.Hidden;
                capchaTB.Visibility = Visibility.Hidden;
                CaphBtn.Visibility = Visibility.Hidden;
                capchaTB.Text = null;
                i = 0;
            }
            else
            {
                ImCaph.Source = ImageSourceFromBitmap(CreateImage(200, 50));
            }
        }

        int i = 0;



        private void Sing(object sender, RoutedEventArgs e)
        {
            //Создание локальной переменной
            Emploee authUser = null;

            //Поиск пользователей по E-mail
            authUser = db.Emploees.Where(b => b.E_mail == MailBx.Text).FirstOrDefault();

            if (authUser != null)
            {
                if (authUser.Password == passwordBx.Password)
                {
                    ConnectionWindow connection = new ConnectionWindow();
                    App.Emploee_id.id = authUser.Id;
                    //Открытие окон по Должности
                    switch (authUser.id_JobTitle)
                    {
                        case 1:
                            //создание локальной перременной и её заполнение
                            Entities.LoginHistory loginHistory = new Entities.LoginHistory()
                            {
                                LoginDate = DateTime.Now,
                                Id_Employee = authUser.Id,
                                SuccessfulLogin = true,
                                Login = authUser.E_mail
                            };
                            Entities.RentalSBPEntities context = new RentalSBPEntities();
                            context.LoginHistories.Add(loginHistory);
                            context.SaveChanges();
                            Window g = new Home(ref authUser);
                            g.Show();
                            break;
                        case 2:
                            Entities.LoginHistory loginHistory2 = new Entities.LoginHistory()
                            {
                                LoginDate = DateTime.Now,
                                Id_Employee = authUser.Id,
                                SuccessfulLogin = true,
                                Login = authUser.E_mail
                            };
                            Entities.RentalSBPEntities context2 = new RentalSBPEntities();
                            context2.LoginHistories.Add(loginHistory2);
                            context2.SaveChanges();
                            Window s = new HomeSeniorSeller(ref authUser);
                            s.Show();
                            break;
                        case 3:
                            Entities.LoginHistory loginHistory3 = new Entities.LoginHistory()
                            {
                                LoginDate = DateTime.Now,
                                Id_Employee = authUser.Id,
                                SuccessfulLogin = true,
                                Login = authUser.E_mail
                            };
                            Entities.RentalSBPEntities context3 = new RentalSBPEntities();
                            context3.LoginHistories.Add(loginHistory3);
                            context3.SaveChanges();
                            Window a = new AdminProfile(ref authUser);
                            a.Show();
                            break;
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный пароль");

                    i = i + 1;
                    if (i == 3)
                    {
                        ImCaph.Visibility = Visibility.Visible;
                        ImCaph.Source = ImageSourceFromBitmap(CreateImage(200, 50));
                        capchaTB.Visibility = Visibility.Visible;
                        CaphBtn.Visibility = Visibility.Visible;
                        autorizationBtn.IsEnabled = false;
                    }

                    Entities.LoginHistory loginHistory = new Entities.LoginHistory()
                    {
                        LoginDate = DateTime.Now,
                        Id_Employee = authUser.Id,
                        Login = authUser.E_mail,
                        SuccessfulLogin = false
                    };
                    Entities.RentalSBPEntities context = new RentalSBPEntities();
                    context.LoginHistories.Add(loginHistory);
                    context.LoginHistories.Add(loginHistory);
                    context.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
                i = i + 1;
                if (i == 3)
                {
                    ImCaph.Visibility = Visibility.Visible;
                    ImCaph.Source = ImageSourceFromBitmap(CreateImage(200, 50));
                    capchaTB.Visibility = Visibility.Visible;
                    CaphBtn.Visibility = Visibility.Visible;
                    autorizationBtn.IsEnabled = false;
                }
            }
        }

        private void UnWiewPasswordOrange(object sender, RoutedEventArgs e)
        {
            passwordView.Text = "";
            passwordView.Visibility = Visibility.Hidden;
            passwordBx.Visibility = Visibility.Visible;

        }
        private void viewPasswordOrange(object sender, RoutedEventArgs e)
        {
            passwordView.Text = passwordBx.Password;
            passwordView.Visibility = Visibility.Visible;
            passwordBx.Visibility = Visibility.Hidden;

        }
    }
}
