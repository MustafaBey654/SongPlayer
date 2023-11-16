using CommunityToolkit.Maui.Views;
using SongPlayer.Dal;
using SongPlayer.Dal.Concreate;
using SongPlayer.Models;
using System.Collections.ObjectModel;




namespace SongPlayer.Pages;

public partial class ListPage : ContentPage
{
    private OptionsPopupPage optionsPopup; // OptionsPopupPage inst

    private readonly MusicViewModel viewModel;
    private readonly FavoriViewModel favoriViewModel;





    public ObservableCollection<MusicClass> MusicList { get; set; }

    [Obsolete]
    public ListPage()
    {

        InitializeComponent();



        viewModel = new MusicViewModel(new MusicRepository());
        favoriViewModel = new FavoriViewModel(new FavoriteRepository());



        MusicList = new ObservableCollection<MusicClass>();

        BindingContext = new MusicViewModel(new MusicRepository());

        optionsPopup = new OptionsPopupPage(viewModel, favoriViewModel);
        optionsPopup.ReturnedFromOptionsPage += OnOptionsPopupReturned;

        listView.ItemsSource = MusicList;


        this.Appearing += async (s, e) =>
        {
            await Task.Run(async () =>
            {
                MusicList = await viewModel.LoadMusicList();
                listView.ItemsSource = MusicList;
            });


        };


    }

 



    [Obsolete]
    private void OptionsButton_Clicked(object sender, EventArgs e)
    {


        var button = (ImageButton)sender;
        var parentGrid = (Grid)button.Parent;
        var parentFrame = (Frame)parentGrid.Parent;
        var innerGrid = (Grid)parentFrame.Content;
        var label = innerGrid.Children.FirstOrDefault(c => c is Label) as Label;

        if (label != null && label.BindingContext is MusicClass music)
        {
            int musicId = music.Id;
            SharedViewModel mySharedViewModel = SharedViewModel.Instance;
            mySharedViewModel.MusicId = musicId;

            if (optionsPopup != null)
            {
                optionsPopup.ReturnedFromOptionsPage -= OnOptionsPopupReturned; // Önceki dinlemeyi kaldýr
            }

            optionsPopup.ReturnedFromOptionsPage -= OnOptionsPopupReturned; // Önceki dinlemeyi kaldýr
            optionsPopup = new OptionsPopupPage(viewModel, favoriViewModel);
            optionsPopup.ReturnedFromOptionsPage += OnOptionsPopupReturned; // Yeni instance için dinlemeyi ekle

            this.ShowPopup(optionsPopup);
        }

    }

    [Obsolete]
    private void OnOptionsPopupReturned()
    {
        CheckMusicListUpdate();
    }

    [Obsolete]
    private void CheckMusicListUpdate()
    {
        Device.BeginInvokeOnMainThread(async () =>
        {
            var musicListe = await viewModel.LoadMusicList();
            listView.ItemsSource = musicListe;
        });

    }




    [Obsolete]
    private async void OnSelectedMusic(object sender, EventArgs e)
    {

        var label = (Label)sender;
        var item = (MusicClass)label.BindingContext;// YourItemType, koleksiyonunuzdaki öðelerin tipi olmalýdýr.

        int id = item.Id;

        await Navigation.PushAsync(new MusicViewPage(id));
        //await Shell.Current.GoToAsync($"musicviewpage?id={id}");
    }





}




