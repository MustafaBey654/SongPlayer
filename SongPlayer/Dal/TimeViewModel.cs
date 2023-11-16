
using CommunityToolkit.Mvvm.ComponentModel;
using SongPlayer.Dal.Contract;


namespace SongPlayer.Dal
{
    public partial class TimeViewModel : ObservableObject, ITimeViewModel
    {
        private static TimeViewModel instance = null;
        private static readonly object padlock = new();

        public static TimeViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new TimeViewModel();
                    }
                    return instance;
                }
            }
        }


        [ObservableProperty]
        private System.Timers.Timer _ttimer;

        [ObservableProperty]
        private TimeSpan _musicPlayerTime;


        // Event to notify when the timer has finished

        private event Action TimerFinisHed;

        [ObservableProperty]
        private bool isTimerSet = true;



        [ObservableProperty]
        private string _countdownText;



        public void OnResetCountdown()
        {
            //zamanlayıcıyı durdur ve süreyi sıfırla
            if (Ttimer != null)
            {
                Ttimer.Stop();
                Ttimer = null;
            }
            IsTimerSet = false;
            MusicPlayerTime = new TimeSpan();
            MusicPlayerTime = TimeSpan.Zero;
            CountdownText = "Geri sayım sıfırlandı";
        }



        public void OnSetTimer(TimeSpan timeSpan)
        {
            // Eğer saat 12'den büyükse, bu değerden 12 çıkar
            if (timeSpan.Hours >= 12)
            {
                timeSpan = new TimeSpan(timeSpan.Hours - 12, timeSpan.Minutes, timeSpan.Seconds);
            }

            MusicPlayerTime = timeSpan;
            Ttimer = new System.Timers.Timer(1000); // 1000 milisaniye = 1 saniye
            Ttimer.Elapsed += (sender, e) => UpdateCountdown();
            Ttimer.Start();

            UpdateCountdown();
        }

        public void UpdateCountdown()
        {
            if (MusicPlayerTime.TotalSeconds > 0)
            {
                IsTimerSet = true;
                //Geri sayımı azalt ve metni güncelle
                MusicPlayerTime = MusicPlayerTime.Add(TimeSpan.FromSeconds(-1));
                CountdownText = "Geri sayım: " + MusicPlayerTime.ToString(@"hh\:mm\:ss");
            }
            else
            {

                //Geri sayım bittiğinde zamanlayıcıyı durdur.
                Ttimer.Stop();
                IsTimerSet = false;
                TimerFinisHed?.Invoke();
                CountdownText = "Geri sayım bitti.";

            }

        }
    }

}
