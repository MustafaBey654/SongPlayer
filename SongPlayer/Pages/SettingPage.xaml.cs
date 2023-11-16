
using CommunityToolkit.Maui.Alerts;
using SongPlayer.Dal;


namespace SongPlayer.Pages;

public partial class SettingPage : ContentPage
{
    private TimeViewModel tmViewModel;

    public SettingPage()
    {
        InitializeComponent();

        tmViewModel = TimeViewModel.Instance;
        BindingContext = tmViewModel;



    }


    private async void OnEditPagePicture(object sender,EventArgs e)
    {
        

       await Navigation.PushAsync(new ListPicturesPage());

    }



    [Obsolete]
    private async void OnSetTimerButtonClicked(object sender, EventArgs e)
    {
        try
        {
            tmViewModel.OnSetTimer(MusicPlayerTimer.Time);


        }
        catch (Exception ex)
        {

            await DisplayAlert($"Hata oluþtu", ex.Message.ToString(), "Ok");
        }


    }

    private async void OnResetCountdownButtonClicked(object sender, EventArgs e)
    {

        tmViewModel.OnResetCountdown();
        MusicPlayerTimer.Time = new TimeSpan();

        await Toast.Make("Sýfýrlama yapýldý.").Show();
        await Shell.Current.GoToAsync("//ListPage");
    }


    protected override bool OnBackButtonPressed()
    {
        Dispatcher.Dispatch(async () =>
        {
            await Shell.Current.GoToAsync("//ListPage");
        });
        return true; ;

    }



}





