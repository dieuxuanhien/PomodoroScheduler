using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroScheduler.ViewModels
{
    public class Task : INotifyPropertyChanged
{
    private string _taskName;
    private int _cyclesleft;

    public string TaskName
    {
        get => _taskName;
        set
        {
            _taskName = value;
            OnPropertyChanged(nameof(TaskName));
        }
    }

    public int CyclesLeft
    {
        get => _cyclesleft;
        set
        {
            if (_cyclesleft != value)
            {
                _cyclesleft = value;
                OnPropertyChanged(nameof(CyclesLeft));  // Notify when CyclesLeft changes
            }
        }
    }

    public Task(string taskName, int cyclesleft)
    {
        _taskName = taskName;
        _cyclesleft = cyclesleft;
    }

    public void CycleDecrease()
    {
        CyclesLeft--;  // Decrease CyclesLeft, which triggers PropertyChanged
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}


    public class TaskViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Task> _taskList;
        public ObservableCollection<Task> TaskList
        {
            get => _taskList;
            set
            {
                if (_taskList != value)
                {
                    if (_taskList != null)
                    {
                        _taskList.CollectionChanged -= OnTaskListChanged;
                    }

                    _taskList = value;

                    if (_taskList != null)
                    {
                        _taskList.CollectionChanged += OnTaskListChanged;
                    }

                    OnPropertyChanged(nameof(TaskList));
                    OnPropertyChanged(nameof(CurrentTask)); // Notify CurrentTask changes since it's derived from TaskList
                }
            }
        }

        public Task CurrentTask
        {
            get => TaskList.First();
        }


        public TaskViewModel()
        {
            TaskList=new ObservableCollection<Task>();
        }


        public void DecrementCurrentTaskCycle()
        {
            if (TaskList.Count>0)
            {
                CurrentTask.CycleDecrease();

                // Remove the task if CyclesLeft reaches 0
                if (CurrentTask.CyclesLeft <= 0)
                {
                    TaskList.Remove(CurrentTask);
                   
                }
                    // Notify that CurrentTask has changed
                OnPropertyChanged(nameof(CurrentTask));

              
            }
        }

        private void OnTaskListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Notify that the CurrentTask may have changed if the first item was added, removed, or replaced
            if (e.Action == NotifyCollectionChangedAction.Add ||
                e.Action == NotifyCollectionChangedAction.Remove ||
                e.Action == NotifyCollectionChangedAction.Replace)
            {
                OnPropertyChanged(nameof(CurrentTask));
            }
        }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
