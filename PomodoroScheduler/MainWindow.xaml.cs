using PomodoroScheduler.ViewModels;
using System.ComponentModel;
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

            _mainModel.TaskViewModel.TaskList.Add(new ViewModels.Task("IT008888888", 2));
            _mainModel.TaskViewModel.TaskList.Add(new ViewModels.Task("IT33", 2));
            _mainModel.TaskViewModel.TaskList.Add(new ViewModels.Task("IT008888888", 2));

            _mainModel.TimerViewModel.PropertyChanged += TimerViewModel_PropertyChanged;
            InitializeComponent();
            
           
        }
        





        //TIMER TAB EVENTS AND FUNCTIONS
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
        private void NumericTextBoxCorrectionEvent(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
           
            if (int.TryParse(textBox.Text, out int value))
            {
               
            }
            else
            {
                textBox.Text = "1";
            }
            textBox.SelectionStart= textBox.Text.Length;
        }
        private void SessionTimeIncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            _mainModel.TimerViewModel.SessionTime += 5;
        }

        private void SessionTimeDecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            _mainModel.TimerViewModel.SessionTime -= 5;
        }

        private void ShortBreakTimeIncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            _mainModel.TimerViewModel.ShortBreakTime += 5;
        }

        private void ShortBreakTimeDecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            _mainModel.TimerViewModel.ShortBreakTime -= 5;
        }
     

        //SCHEDULER TAB EVENTS

        private void AddTaskEvent(object sender, RoutedEventArgs e)
        {
            string newTaskName = TaskNameBox.Text;
            int newCyclesNum= 0;
            int.TryParse(CyclesLeftBox.Text, out newCyclesNum);
            if (newTaskName != string.Empty && newCyclesNum >0)
            {
                _mainModel.TaskViewModel.TaskList.Add(new ViewModels.Task(newTaskName, newCyclesNum));
            }


        }
        private void RemoveTaskEvent(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ViewModels.Task taskToRemove)
            {
                // Access the TaskViewModel and remove the task
                _mainModel.TaskViewModel.TaskList.Remove(taskToRemove);
            }
        }
        private void TaskNameBoxCorrectionEvent(object sender, TextChangedEventArgs e)
        {
            if (TaskNameBox.Text.Length >25) TaskNameBox.Text= TaskNameBox.Text.Substring(0,25);
        }

        private void CyclesLeftBoxCorrectionEvent(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == string.Empty) return;
            if (int.TryParse(textBox.Text, out int value))
            {
                if (value < 1) value = 1;
                else if (value > 99) value = 99;
                textBox.Text = value.ToString();
            } 
            else
            {
                textBox.Text = "1"; 
            }
        }


        //WINDOW RANGE EVENTS
        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.FocusedElement is TextBox)
            {
                Keyboard.ClearFocus();
                
            }
        }
        private void TimerViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.TimerViewModel.Phase))
            {
                this.Dispatcher.Invoke(() =>
                {
                    if (this.WindowState == WindowState.Minimized)
                    {
                        this.WindowState = WindowState.Normal; // Restore the window from minimized state
                    }

                    // Bring the window to the foreground
                    this.Activate();  // Set focus to the window
                    this.Topmost = true;  // Temporarily set the window as topmost
                    this.Topmost = false; // Revert topmost status to avoid permanent topmost
                });

            }
        }

    }
}