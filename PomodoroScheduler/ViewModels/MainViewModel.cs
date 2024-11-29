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
        public TimerViewModel TimerViewModel { get; set; }
        public TaskViewModel TaskViewModel { get; set; }

        public MainViewModel()
        {
            TimerViewModel = new TimerViewModel();
            TaskViewModel = new TaskViewModel();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
