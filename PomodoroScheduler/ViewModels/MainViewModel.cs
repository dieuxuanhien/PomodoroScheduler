using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroScheduler.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Sub-ViewModels
        private TimerViewModel _timerViewModel;
        public TimerViewModel TimerViewModel
        {
            get => _timerViewModel;
            set
            {
                if (_timerViewModel != value)
                {
                    _timerViewModel = value;
                    OnPropertyChanged(nameof(TimerViewModel));  // Notify that TimerViewModel has changed
                }
            }
        }

        private TaskViewModel _taskViewModel;
        public TaskViewModel TaskViewModel
        {
            get => _taskViewModel;
            set
            {
                if (_taskViewModel != value)
                {
                    _taskViewModel = value;
                    OnPropertyChanged(nameof(TaskViewModel));  // Notify that TaskViewModel has changed
                }
            }
        }

        public MainViewModel()
        {
            _timerViewModel = new TimerViewModel();
            _taskViewModel = new TaskViewModel();

            TimerViewModel.CycleCompleted += OnCycleCompleted;
            // Subscribe to the task list changes in TaskViewModel
            TaskViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(TaskViewModel.CurrentTask))
                {
                    OnPropertyChanged(nameof(TaskNameDisplay)); // Notify UI for TaskName change
                    OnPropertyChanged(nameof(CyclesLeftDisplay)); // Notify UI for CyclesLeft change
                }
            };

        }


        public string TaskNameDisplay 
        { 
            get => $"< {TaskViewModel.CurrentTask.TaskName} >"; 
           
        }
        public string CyclesLeftDisplay 
        { 
            get => $"< Cycles left: {TaskViewModel.CurrentTask.CyclesLeft} >"; 
            
        }

        private void OnCycleCompleted()
        {
            TaskViewModel.DecrementCurrentTaskCycle();
            OnPropertyChanged(nameof(TaskViewModel));
           
            
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
