namespace SongPlayer.Pages;

public partial class FeedBackPage : ContentPage
{
    public FeedBackPage()
    {
        InitializeComponent();
    }


    private async void OnSendClicked(object sender, EventArgs e)
    {
        var feedback = FeedbackEditor.Text;

        if (string.IsNullOrEmpty(feedback) || feedback.Length < 30)
        {
            // Geri bildirim boþ veya 30 karakterden az ise bir uyarý göster
            await DisplayAlert("Uyarý", "Lütfen en az 30 karakterlik bir geri bildirim girin.", "Tamam");
            return;
        }
        var email = "mustafa.er2375@gmail.com";
        var subject = "Geri Bildirim";
        var body = feedback;

        try
        {
            var mailto = $"mailto:{email}?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}";
            await Launcher.OpenAsync(new Uri(mailto));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        await Shell.Current.GoToAsync("//ListPage");
    }



    protected override bool OnBackButtonPressed()
    {
        Dispatcher.DispatchAsync(async () =>
        {
            await Shell.Current.GoToAsync("///ListPage");
        });

        return true;
    }
}