
using CommunityToolkit.Maui.Alerts;
using SongPlayer.Dal;
using SongPlayer.Dal.Concreate;
using SongPlayer.Models;
using System.Collections.ObjectModel;

namespace SongPlayer.Pages;

public partial class FavoriPage : ContentPage
{

    private FavoriPlayPage currentPlayPage; // FavoriPlayPage örneðini saklamak için bir alan

    // FavoriPage içinde, sýnýf seviyesinde bir deðiþken tanýmlayýn
    private bool isMusicPlaying = false;
    private FavoriViewModel viewModel;

    private MusicClass currentPlayingMusic; // Eklenen satýr

    public ObservableCollection<MusicClass> FavoriteList { get; private set; }

 
    public FavoriPage()
    {

        InitializeComponent();

        viewModel = new FavoriViewModel(new FavoriteRepository());
        BindingContext = viewModel;



      
         this.Appearing += async (s, e) =>
        {
            var favoriList = await viewModel.GetFavoriteMusicList();


            if (favoriList is not null)
            {
                FavoriteList = new ObservableCollection<MusicClass>(favoriList);
                listView.ItemsSource = FavoriteList;
            }
        };


    }


    [Obsolete]
    private void OnSelectedMusic(object sender, EventArgs e)
    {

        var label = (Label)sender;
        var item = (MusicClass)label.BindingContext;

        if (currentPlayingMusic != null)
        {
            currentPlayingMusic.IsPlaying = true; // Mevcut þarkýyý durdur
        }

        int id = item.Id;

        if (currentPlayPage is null)
        {
            // Mevcut sayfa zaten varsa, burada bir iþlem yapmaya gerek yok.
            currentPlayPage = new FavoriPlayPage(id, item);
            Navigation.PushAsync(currentPlayPage);

        }
        else
        {

            if (currentPlayPage.Parent != null)
            {
                Navigation.RemovePage(currentPlayPage);
            }

            currentPlayPage = new FavoriPlayPage(id, item);
                                                            
            Navigation.PushAsync(currentPlayPage);
        }

        // Çalýnan þarkýyý kaydet
        currentPlayingMusic = item;
        currentPlayingMusic.IsPlaying = true;



    }

    private async void deleteBtn_Clicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;
        var parentGrid = (Grid)button.Parent;
        var parentFrame = (Frame)parentGrid.Parent;
        var innerGrid = (Grid)parentFrame.Content;
        var label = innerGrid.Children.FirstOrDefault(c => c is Label) as Label;

        if (label != null && label.BindingContext is MusicClass music)
        {
            int musicId = music.Id;
            await viewModel.RemoveFavorite(musicId);
            listView.ItemsSource = await viewModel.GetFavoriteMusicList();

            await Toast.Make("Silme iþlemi baþarýlý").Show();

        }

    }
}
