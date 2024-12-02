using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PomodoroScheduler.ViewModels
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Action CycleCompleted;
        
        private DispatcherTimer _timer;
        private TimeSpan _timeLeft;

        // Properties for session, short break, and long break durations
        private int _sessionTime;
        private int _shortBreakTime;
        public int SessionTime
        {
            get => _sessionTime;
            set
            {
                _sessionTime = Math.Clamp(value, 1, 599);
                OnPropertyChanged(nameof(SessionTime));
            }
        }
        public int ShortBreakTime
        {
            get => _shortBreakTime;
            set
            {
                _shortBreakTime = Math.Clamp(value,1,599);
                OnPropertyChanged(nameof(ShortBreakTime));
            }
        }
        public int LongBreakTime => ShortBreakTime * 3;

        private int _cycleCount;
        public int CycleCount
        {
            get => _cycleCount;
            private set
            {
                _cycleCount = value;
                OnPropertyChanged(nameof(CycleCount));
            }
        }

        private string _phase;
        public string Phase
        {
            get => _phase.ToUpper();
            private set
            {
                _phase = value;
                OnPropertyChanged(nameof(Phase));
            }
        }

        public string TimeLeft
        {
            get =>_timeLeft.Hours>0 ? _timeLeft.ToString(@"h\:mm\:ss") : _timeLeft.ToString(@"mm\:ss");
            private set
            {
                OnPropertyChanged(nameof(TimeLeft));
            }
        }

        public TimerViewModel()
        {
            // Default times (modifiable via properties)
            SessionTime = 25;  // 25 minutes
            ShortBreakTime = 5; // 5 minutes
            
            Phase = "Session";

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

            Reset();
        }

        // Timer Controls
        public void Start()
        {
            _timer.Start();
        }

        public void Pause()
        {
            _timer.Stop();
        }

        public void Reset()
        {
            _timer.Stop();
            _timeLeft = TimeSpan.FromMinutes(SessionTime);
            Phase = "Session";
            OnPropertyChanged(nameof(TimeLeft));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_timeLeft.TotalSeconds > 0)
            {
                _timeLeft -= TimeSpan.FromSeconds(1);
                OnPropertyChanged(nameof(TimeLeft));
            }
            else
            {
                _timer.Stop();
                PlayNotificationSound();
                HandlePhaseChange();
                _timer.Start();
            }
        }

        private void HandlePhaseChange()
        {
            if (_phase == "Session")
            {
                
                CycleComplete();
                if (CycleCount % 4 == 0)
                {
                    _phase = "Long Break";
                    _timeLeft = TimeSpan.FromMinutes(LongBreakTime);
                }
                else
                {
                    _phase = "Short Break";
                    _timeLeft = TimeSpan.FromMinutes(ShortBreakTime);
                }
            }
            else
            {
                _phase = "Session";
                _timeLeft = TimeSpan.FromMinutes(SessionTime);
            }

            OnPropertyChanged(nameof(Phase));
            OnPropertyChanged(nameof(TimeLeft));
        }

        protected void CycleComplete()
        {
            CycleCount++;
            CycleCompleted?.Invoke();

        }

        private void PlayNotificationSound()
        {
            try
            {
               
                SoundPlayer player = new SoundPlayer();
                player.SoundLocation = "Sounds/notification.wav";


                player.Play();
               
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Audio file error: " + ex.Message);
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
