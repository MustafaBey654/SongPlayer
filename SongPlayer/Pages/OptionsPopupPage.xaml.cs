
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

    // Geri dönüþ iþlemi gerçekleþtiðinde bu event'i tetikleyin.
    private void OnReturnButtonClicked(object sender, EventArgs e)
    {
        ReturnedFromOptionsPage?.Invoke();
        // ... Diðer iþlemler
    }



    [Obsolete]
    private async void OnSelectedOptionName(object sender, EventArgs e)
    {

        var label = (Label)sender;
        var optionName = (OptionClass)label.BindingContext;// YourItemType, koleksiyonunuzdaki öðelerin tipi olmalýdýr.


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
                    await Toast.Make("Favoriye baþarýlý bir þekilde eklendi").Show();
                }
                else
                {

                    await CloseAsync();
                    await Toast.Make("zaten önceden eklenmiþ").Show();
                }

            }

        }
        else if (id == 2)
        {
            try
            {
                var song = await viewModel.GetMusicById(musicId);

                await SendShareFile(song.FileName, song.FilePath);

                await Toast.Make("Gönderme iþlemi baþarýlý").Show();
                await CloseAsync();
            }
            catch (Exception ex)
            {


                await Toast.Make($"Gönderme iþlemi baþarýsýz {ex.Message}").Show();
                await CloseAsync();
            }
        }
        else
        {

            await viewModel.RemoveMusic(musicId);
            OnReturnButtonClicked(sender, e); // Bu satýrý ekleyin.
            await viewModel.LoadMusicList();
            await Toast.Make("Silme iþlemi baþarýlý").Show();
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

            await Toast.Make($"Dosya bulunamadý.! ").Show();
            await CloseAsync();
        }
    }



}
