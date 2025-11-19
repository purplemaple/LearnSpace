using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _1._1_ProgressForEvent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>  
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                button.IsEnabled = false;

                Progress<double> progress = new(value => progressBar.Value = value);
                await DoJobAsync(progress);
                button.IsEnabled = true;
            }
            catch (Exception)
            {

            }
        }

        async Task DoJobAsync(IProgress<double> progress)
        {
            for (int i = 1; i <= 100; i++)
            {
                await Task.Delay(50).ConfigureAwait(false);
                progressBar.Value = i;
                //progress.Report(i);
            }
        }
    }
}