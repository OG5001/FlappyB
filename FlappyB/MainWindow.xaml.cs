using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FlappyB
{
    public class Akadaly
    {
        public Rectangle Felso { get; set; }
        public Rectangle Also { get; set; }
        public bool Pontozva { get; set; }

        public Akadaly(Rectangle felso, Rectangle also)
        {
            Felso = felso;
            Also = also;
            Pontozva = false;
        }
    }
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;

        private double sebesseg = 0;
        private double gravitacio = 0.5;
        private double akadalySebesseg = 3;
        int currentScore = 0;

        private List<Akadaly> akadalyok = new List<Akadaly>();

        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += Timer_Tick;
            point.Text = "Pontszám: 0";

            PreviewKeyDown += MainWindow_KeyDown;

            Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AkadalyokLetrehozasa();
        }

        private void AkadalyokLetrehozasa()
        {
            for (int i = 0; i < 10; i++)
            {
                Rectangle felso = new Rectangle();
                Rectangle also = new Rectangle();

                felso.Width = 52;
                also.Width = 52;

                ImageBrush akadalyKep = new ImageBrush();

                akadalyKep.ImageSource = new BitmapImage(
                    new Uri("pack://application:,,,/Kepek/akadaly.jpg")
                );

                akadalyKep.Stretch = Stretch.Fill;

                felso.Fill = akadalyKep;
                also.Fill = akadalyKep;

                double x = palya.ActualWidth + i * 260;

                double res = 120;
                double felsoMagassag = random.Next(50, 140);

                felso.Height = felsoMagassag;

                double alsoTop = felsoMagassag + res;

                also.Height = palya.ActualHeight - alsoTop;

                Canvas.SetLeft(felso, x);
                Canvas.SetTop(felso, 0);

                Canvas.SetLeft(also, x);
                Canvas.SetTop(also, alsoTop);

                palya.Children.Add(felso);
                palya.Children.Add(also);

                akadalyok.Add(new Akadaly(felso, also));
            }
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && timer.IsEnabled)
            {
                sebesseg = -5;
                e.Handled = true;
            }
        }
        private void Movement()
        {
            double aktualispozicio = Canvas.GetTop(bt);

            sebesseg += gravitacio;

            double ujpozicio = aktualispozicio + sebesseg;

            Canvas.SetTop(bt, ujpozicio);
        }
        private void AkadalyMovement()
        {
            foreach (Akadaly akadaly in akadalyok)
            {
                double x = Canvas.GetLeft(akadaly.Felso);

                x -= akadalySebesseg;

                if (x + akadaly.Felso.Width < 0)
                {
                    x = palya.ActualWidth + 100;

                    double felsoMagassag = random.Next(50, 140);
                    double res = 120;

                    akadaly.Felso.Height = felsoMagassag;

                    double alsoTop = felsoMagassag + res;

                    akadaly.Also.Height = palya.ActualHeight - alsoTop;

                    Canvas.SetTop(akadaly.Also, alsoTop);
                    akadaly.Pontozva = false;
                }
                if (!akadaly.Pontozva && x + akadaly.Felso.Width <= Canvas.GetLeft(bt))
                {
                    currentScore++;
                    point.Text = $"Pontszám: {currentScore}";
                    akadaly.Pontozva = true;

                }
                Canvas.SetLeft(akadaly.Felso, x);
                Canvas.SetLeft(akadaly.Also, x);
            }
        }

        private void UtkozesVizsgalat()
        {
            double x = Canvas.GetLeft(bt);
            double y = Canvas.GetTop(bt);

            Rect panda = new Rect(
                x,
                y,
                bt.ActualWidth,
                bt.ActualHeight
            );
            if (y <= 0 ||
                y + bt.ActualHeight >= palya.ActualHeight)
            {
                JatekVege();
                return;
            }
            foreach (Akadaly akadaly in akadalyok)
            {
                Rect felso = new Rect(
                    Canvas.GetLeft(akadaly.Felso),
                    Canvas.GetTop(akadaly.Felso),
                    akadaly.Felso.Width,
                    akadaly.Felso.Height
                );

                Rect also = new Rect(
                    Canvas.GetLeft(akadaly.Also),
                    Canvas.GetTop(akadaly.Also),
                    akadaly.Also.Width,
                    akadaly.Also.Height
                );

                if (panda.IntersectsWith(felso) ||
                    panda.IntersectsWith(also))
                {
                    JatekVege();
                    return;
                }
            }
        }
        private void JatekVege()
        {
            timer.Stop();

            MessageBox.Show("Vesztettél!");
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Movement();

            AkadalyMovement();

            UtkozesVizsgalat();
        }
        private void gameStart()
        {
            point.Text = "Pontszám: 0";
            timer.Stop();

            Canvas.SetLeft(bt, 40);
            Canvas.SetTop(bt, 100);

            sebesseg = 0;

            for (int i = 0; i < akadalyok.Count; i++)
            {
                double x = palya.ActualWidth + i * 260;

                double felsoMagassag = random.Next(50, 140);
                double res = 120;

                akadalyok[i].Felso.Height = felsoMagassag;

                double alsoTop = felsoMagassag + res;

                akadalyok[i].Also.Height =
                    palya.ActualHeight - alsoTop;

                Canvas.SetLeft(akadalyok[i].Felso, x);
                Canvas.SetTop(akadalyok[i].Felso, 0);

                Canvas.SetLeft(akadalyok[i].Also, x);
                Canvas.SetTop(akadalyok[i].Also, alsoTop);
            }

            timer.Start();

            this.Focus();
            


        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gameStart();
        }

        private void bt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            gameStart();


        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            gameStart();
            Rectangle kod = new Rectangle();
            kod.Width = 1000;
            kod.Height = 1000;
            Canvas.SetLeft(kod, 0);
            kod.Fill = Brushes.LightGray;
            kod.Opacity = 0.9;
            palya.Children.Add(kod);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            gameStart();
        }
    }
}