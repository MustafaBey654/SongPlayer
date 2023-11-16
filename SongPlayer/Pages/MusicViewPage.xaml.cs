using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using SongPlayer.Dal;
using SongPlayer.Dal.Concreate;
using SongPlayer.Models;
using System.Collections.ObjectModel;
using System.Timers;

namespace SongPlayer.Pages;

public partial class MusicViewPage : ContentPage
{
    private bool isFirstClick = true;
    // Bu kodu sayfa y�klendi�inde veya uygulama ba�lad���nda kullanabilirsiniz.
    ImageSource shuffleImageSource = ImageSource.FromFile("shuffle.png");
    ImageSource repeatImageSource = ImageSource.FromFile("repeat.png");

    private bool isRepeating = false;

    public bool _isTimer = true;

    private TimeViewModel timeViewModel;
    private MusicViewModel viewModel;
    private ObservableCollection<MusicClass> shuffledMusicList;

    private int currentShuffleIndex = 0;


    private int SelectedId = 0;

    private bool IsSeeking = false;
    private double newPosition = 0;
    private System.Timers.Timer timer;


    private bool isShuffleMode = false;
    private int index;



    [Obsolete]
    public MusicViewPage(int id)
    {
        InitializeComponent();

        timeViewModel = TimeViewModel.Instance;


        // Ba�lang��ta "Kar���k �alma" modu se�ili olarak ba�las�n
        shuffleButton.ImageSource = ImageSource.FromFile("shuffle.png");
        //context = new MusicDbContext(new DbContextOptions<MusicDbContext>());

        viewModel = new MusicViewModel(new MusicRepository());
        OnTimerElapsed(null, null);
        ShuffleButton_Clicked(null, null);

        timer = new System.Timers.Timer(1000);
        timer.Elapsed += OnTimerElapsed;
        // Zamanlay�c�y� ba�lat
        Device.StartTimer(TimeSpan.FromSeconds(1), OnTimerElapsed);

        // Slider'�n ValueChanged olay�n� dinleyin
        progressSlider.ValueChanged += Slider_ValueChanged;
        mediaElementLbl.Play();

        SelectedId = id;


        Dispatcher.Dispatch(async () =>
        {
            await viewModel.LoadMusicList();


            try
            {
                var musicListesi = await viewModel.LoadMusicList();

                shuffledMusicList = musicListesi;  //GetMusicList().ToList();
                var musicData = await viewModel.GetMusicById(SelectedId);
                int index = shuffledMusicList.IndexOf(musicData);
                currentShuffleIndex = index;

            }
            catch (Exception ex)
            {

                Console.WriteLine($"index hatas� {ex.Message}");
                Console.WriteLine($"index hatas� {ex.Message}");
                // Console.WriteLine($"index hatas� {ex.InnerException.Message}");
            }

            if (currentShuffleIndex >= 0 && currentShuffleIndex < shuffledMusicList.Count)
            {
                var music = shuffledMusicList[currentShuffleIndex].FilePath;

                currentShuffleIndex = index;
                mediaElementLbl.Source = MediaSource.FromFile(music);
                mediaElementLbl.ShouldAutoPlay = true;
                OnTimerFinished();

                mediaElementLbl.MediaOpened += (sender, e) =>
                {
                    albumArtImage.IsVisible = true;
                    timer.Start();

                };
            }
            else
            {
                Console.WriteLine("  // currentShuffleIndex ge�erli bir indeks de�il");
            }


        });




    }




    private async void OnTimerFinished()
    {
        _isTimer = timeViewModel.IsTimerSet;
        if (_isTimer)
        {
            mediaElementLbl.Play();
        }
        else
        {
            mediaElementLbl.Pause();
            var result = await DisplayAlert("M�zik �alma S�resi", "M�zik �alma s�reniz sona erdi. Tekrar �almak ister misiniz?", "Evet", "Hay�r");
            if (result)
            {
                TimeViewModel.Instance.IsTimerSet = true;
                mediaElementLbl.Play();
            }
            else
            {
                await Shell.Current.GoToAsync("//ListPage");
            }
        }
    }

    private void Next_Clicked(object sende, EventArgs e)
    {

        if (currentShuffleIndex >= 0 && currentShuffleIndex < shuffledMusicList.Count)
        {
            var music = shuffledMusicList[currentShuffleIndex].FilePath;
            // Daha fazla i�lemler yapabilirsiniz.
            mediaElementLbl.Source = MediaSource.FromFile(music);
            mediaElementLbl.ShouldAutoPlay = true;
            // mediaElementLbl.Play();
            OnTimerFinished();
            if (currentShuffleIndex == shuffledMusicList.Count)
            {
                currentShuffleIndex = 0;
            }

            currentShuffleIndex++;
        }





    }

    private void Previous_Clicked(object sende, EventArgs e)
    {
        if (currentShuffleIndex < shuffledMusicList.Count)
        {
            var music = shuffledMusicList[currentShuffleIndex].FilePath;
            mediaElementLbl.Source = MediaSource.FromFile(music);
            mediaElementLbl.ShouldAutoPlay = true;
            //mediaElementLbl.Play();
            OnTimerFinished();
            if (currentShuffleIndex == 0)
                currentShuffleIndex++;
            currentShuffleIndex--;
        }
    }

    [Obsolete]
    private async void ShuffleButton_Clicked(object sender, EventArgs e)
    {
        // Kar���k modu ba�lang��ta etkinle�tirin.
        if (!isShuffleMode)
        {
            isShuffleMode = true;
            shuffleButton.ImageSource = shuffleImageSource;

            shuffledMusicList = await viewModel.LoadMusicList();
            currentShuffleIndex = 0;

            isRepeating = false;
            //mediaElementLbl.Play();
            OnTimerFinished();

            mediaElementLbl.MediaEnded += (s, args) =>
            {
                OnMediaEnded(isRepeating);
            };
        }
        else
        {
            // Kar���k moddayken "Shuffle" butonuna bas�ld���nda, tekrar modunu etkinle�tirin.
            isShuffleMode = false;
            shuffleButton.ImageSource = repeatImageSource;

            isRepeating = true;

            mediaElementLbl.MediaEnded += (s, args) =>
            {
                OnMediaEnded(isRepeating);
            };
        }


    }

    private void OnMediaEnded(bool isRepeating)
    {
        if (isRepeating)
        {

            mediaElementLbl.SeekTo(TimeSpan.Zero); // �ark�n�n ba��na geri d�n
            mediaElementLbl.Stop();
            // mediaElementLbl.Play();
            OnTimerFinished();


        }
        else
        {
            PlayNextRandomSong();
        }
    }

    // Rastgele bir sonraki �ark�y� �alacak metot
    private void PlayNextRandomSong()
    {
        if (currentShuffleIndex < shuffledMusicList.Count)
        {
            var music = shuffledMusicList[currentShuffleIndex].FilePath;
            mediaElementLbl.Source = MediaSource.FromFile(music);
            mediaElementLbl.ShouldAutoPlay = true;
            //mediaElementLbl.Play();
            OnTimerFinished();
            currentShuffleIndex++;
        }
        else
        {
            // �ark�lar bittiyse ilk �ark�ya d�n
            currentShuffleIndex = 0;
            mediaElementLbl.MediaEnded -= (sender, e) => OnMediaEnded(isRepeating);
        }
    }

    // Timer olay�
    [Obsolete]
    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {

        if (mediaElementLbl.CurrentState == MediaElementState.Stopped && !IsSeeking)
        {
            currentShuffleIndex++;
            if (currentShuffleIndex < shuffledMusicList.Count)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    mediaElementLbl.Source = MediaSource.FromFile(shuffledMusicList[currentShuffleIndex].FilePath);
                    //mediaElementLbl.Play();
                    OnTimerFinished();
                });
            }
        }

        if (mediaElementLbl.CurrentState == MediaElementState.Playing)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                lblMusicTime.Text = FormatTime(mediaElementLbl.Position);
            });
        }
    }


    private string FormatTime(TimeSpan time)
    {
        return $"{(int)time.TotalMinutes}:{time.Seconds:D2}";
    }

    [Obsolete]
    private async void PlayPauseButton_Clicked(object sender, EventArgs e)
    {




        if (mediaElementLbl.CurrentState == MediaElementState.Playing)
        {
            mediaElementLbl.Pause();
        }
        else if (mediaElementLbl.CurrentState == MediaElementState.Paused)
        {
            //mediaElementLbl.Play();
            OnTimerFinished();
        }
        else if (mediaElementLbl.CurrentState == MediaElementState.Stopped || mediaElementLbl.Source == null)
        {
            var media = mediaElementLbl.Source;
            if (mediaElementLbl.Source == null)
            {

                shuffledMusicList = await viewModel.LoadMusicList();  //MusicList.ToList();  //GetMusicList().ToList();
                var musicData = await viewModel.GetMusicById(SelectedId);  //GetMusic(SelectedId);
                if (musicData is not null)
                {
                    int index = shuffledMusicList.IndexOf(musicData);
                    currentShuffleIndex = index;
                    var music = shuffledMusicList[currentShuffleIndex].FilePath;


                    currentShuffleIndex = index;
                    mediaElementLbl.Source = MediaSource.FromFile(music);
                    mediaElementLbl.ShouldAutoPlay = true;
                    //mediaElementLbl.Play();
                    OnTimerFinished();

                }
                else
                {
                    Console.WriteLine("musisc data null");
                }





            }
            else if (currentShuffleIndex < shuffledMusicList.Count)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    mediaElementLbl.Source = MediaSource.FromFile(shuffledMusicList[currentShuffleIndex].FilePath);
                    mediaElementLbl.ShouldAutoPlay = true;
                    //mediaElementLbl.Play();
                    OnTimerFinished();
                });
            }
        }
        // E�er duraklat�ld�ysa, s�radaki �ark�n�n otomatik ba�lamamas�n� sa�la
        else if (mediaElementLbl.CurrentState == MediaElementState.Paused)
        {
            mediaElementLbl.ShouldAutoPlay = false;
        }


    }

    private void SkipForwardButton_Clicked(object sender, EventArgs e)
    {


        // 15 saniye ileri sar
        TimeSpan newPosition = mediaElementLbl.Position.Add(TimeSpan.FromSeconds(15));
        mediaElementLbl.SeekTo(newPosition);
    }


    private void SkipBackwardButton_Clicked(object sender, EventArgs e)
    {

        // 15 saniye geri sar
        TimeSpan time = mediaElementLbl.Position.Subtract(TimeSpan.FromSeconds(15));
        mediaElementLbl.SeekTo(time);
    }

    [Obsolete]
    private void StopButton_Clicked(Object sender, EventArgs e)
    {

        if (mediaElementLbl.CurrentState == MediaElementState.Playing)
        {
            // E�er �ark� �al�yorsa, �ark�y� durdur
            mediaElementLbl.Stop();
            if (mediaElementLbl.CurrentState == MediaElementState.Stopped)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    lblMusicTime.Text = FormatTime(mediaElementLbl.Position);
                });
            }
        }


        mediaElementLbl.Source = null;
        mediaElementLbl.ShouldAutoPlay = false;
    }



    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {


        double progress = e.NewValue;
        double newPosition = (progress / 100) * mediaElementLbl.Duration.TotalSeconds;
        mediaElementLbl.Position.Add(TimeSpan.FromSeconds(newPosition));
        IsSeeking = false; // Kullan�c� s�r�kleme i�lemi bitti�inde bayra�� false yap



    }



    [Obsolete]
    private bool OnTimerElapsed()
    {

        if (!IsSeeking)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (mediaElementLbl.Duration.TotalSeconds > 0)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        double progress = (mediaElementLbl.Position.TotalSeconds / mediaElementLbl.Duration.TotalSeconds) * 100;
                        progressSlider.Value = progress;
                        lblMusicTime.Text = FormatTime(mediaElementLbl.Position);
                    });
                }

                if (newPosition > 0)
                {
                    double newPositionInSeconds = (newPosition / 100) * mediaElementLbl.Duration.TotalSeconds;
                    mediaElementLbl.SeekTo(TimeSpan.FromSeconds(newPositionInSeconds));
                    newPosition = 0;
                }
            });
        }

        return true;



    }


    void ContentPage_Unloaded(object sender, EventArgs e)
    {

        mediaElementLbl.Handler?.DisconnectHandler();
    }

}