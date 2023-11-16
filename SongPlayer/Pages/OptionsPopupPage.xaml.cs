
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using SongPlayer.Dal;
using SongPlayer.Model;
using SongPlayer.Models;
using System.Collections.ObjectModel;


namespace SongPlayer.Pages;

public partial class OptionsPopupPage : Popup
{
    public event Action ReturnedFromOptionsPage;


    private readonly OptionClass Option;
    private readonly MusicViewModel viewModel;
    private readonly FavoriViewModel favoriViewModel;

    public ObservableCollection<MusicClass> MusicList { get; private set; }

    public OptionsPopupPage(MusicViewModel _viewModel, FavoriViewModel favoriView)
    {
        InitializeComponent();
        Option = new OptionClass();

        listView.ItemsSource = Option.GetOptionsList();


        viewModel = _viewModel;
        favoriViewModel = favoriView;

    }

    // Geri d�n�� i�lemi ger�ekle�ti�inde bu event'i tetikleyin.
    private void OnReturnButtonClicked(object sender, EventArgs e)
    {
        ReturnedFromOptionsPage?.Invoke();
        // ... Di�er i�lemler
    }



    [Obsolete]
    private async void OnSelectedOptionName(object sender, EventArgs e)
    {

        var label = (Label)sender;
        var optionName = (OptionClass)label.BindingContext;// YourItemType, koleksiyonunuzdaki ��elerin tipi olmal�d�r.


        SharedViewModel mySharedViewModel = SharedViewModel.Instance;


        int musicId = mySharedViewModel.MusicId;

        var favorimusic = await favoriViewModel.LoadFavoriteList();
        //Options
        int id = (int)optionName.Id;


        if (id == 1)
        {

            if (favorimusic is not null)
            {
                var music = favorimusic.Where(mId => mId.MusicId == musicId).FirstOrDefault();
                if (music is null)
                {
                    FavoriteClass favorite = new FavoriteClass()
                    {


                        MusicId = musicId   //music.Id
                    };

                    await favoriViewModel.AddFavorite(favorite);   //AddFavori(favorite);
                    await CloseAsync();
                    await Toast.Make("Favoriye ba�ar�l� bir �ekilde eklendi").Show();
                }
                else
                {

                    await CloseAsync();
                    await Toast.Make("zaten �nceden eklenmi�").Show();
                }

            }

        }
        else if (id == 2)
        {
            try
            {
                var song = await viewModel.GetMusicById(musicId);

                await SendShareFile(song.FileName, song.FilePath);

                await Toast.Make("G�nderme i�lemi ba�ar�l�").Show();
                await CloseAsync();
            }
            catch (Exception ex)
            {


                await Toast.Make($"G�nderme i�lemi ba�ar�s�z {ex.Message}").Show();
                await CloseAsync();
            }
        }
        else
        {

            await viewModel.RemoveMusic(musicId);
            OnReturnButtonClicked(sender, e); // Bu sat�r� ekleyin.
            await viewModel.LoadMusicList();
            await Toast.Make("Silme i�lemi ba�ar�l�").Show();
            await CloseAsync();

            //Application.Current.MainPage = new Shell();//ListPage(logger);
        }



    }


    public async Task SendShareFile(string fileName, string path)
    {
        string fn = fileName;
        string file = path;

        if (File.Exists(file))
        {
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Share MP3 file",
                File = new ShareFile(file)
            });
        }
        else
        {

            await Toast.Make($"Dosya bulunamad�.! ").Show();
            await CloseAsync();
        }
    }



}
