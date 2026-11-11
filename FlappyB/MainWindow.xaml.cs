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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += Timer_Tick;
            this.PreviewKeyDown += MainWindow_KeyDown;
        }
        private double sebesseg = 0;
        private double gravitacio = 0.5;

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                sebesseg = -5;
                e.Handled = true;
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Movement(bt);
        }

        private void Movement(Button bt)
        {
            double aktualispozicio = Canvas.GetTop(bt);

            sebesseg += gravitacio;

            double ujpozicio = aktualispozicio + sebesseg;

            Canvas.SetTop(bt, ujpozicio);
        }

        private void bt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(bt, 40);
            Canvas.SetTop(bt, 100);

            sebesseg = 0;

            timer.Start();

            this.Focus();
        }
    }
}