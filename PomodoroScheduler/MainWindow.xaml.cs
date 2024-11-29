using PomodoroScheduler.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PomodoroScheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
   
    public partial class MainWindow : Window
    {
        public MainViewModel _mainModel;
        public MainWindow()
        {
            _mainModel = new MainViewModel();
            DataContext = _mainModel;
            InitializeComponent();
            
           
        }

        private void StartPauseEvent(object sender, RoutedEventArgs e)
        {
            if (StartPauseButton.Content.ToString() == "Pause")
            {
                StartPauseButton.Content = "Resume";
                _mainModel.TimerViewModel.Pause();
            }
            else
            {
                StartPauseButton.Content = "Pause";
                _mainModel.TimerViewModel.Start();
            }
        }

        private void ResetEvent(object sender, RoutedEventArgs e)
        {
            _mainModel.TimerViewModel.Reset();
            StartPauseButton.Content = "Start";
            
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.FocusedElement is TextBox)
            {
                Keyboard.ClearFocus();
            }
        }
    }
}