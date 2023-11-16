
using CommunityToolkit.Maui.Alerts;
using SongPlayer.Dal;
using SongPlayer.Dal.Concreate;
using SongPlayer.Models;
using System.Collections.ObjectModel;

namespace SongPlayer.Pages;

public partial class AddSongPages : ContentPage
{
    private readonly MusicViewModel viewModel;
    private ObservableCollection<MusicClass> MusicList;
    public AddSongPages()
    {

        InitializeComponent();

        viewModel = new MusicViewModel(new MusicRepository());

        MusicList = viewModel.MusicList;
    }

    [Obsolete]
    private void OnAddSongButtonClicked(object sender, EventArgs e)
    {



        Dispatcher.Dispatch(async () =>
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {


               // var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                var status = await Permissions.CheckStatusAsync<Permissions.Media>();

                if (status != PermissionStatus.Granted)
                {
                    // Eðer izin verilmemiþse, kullanýcýdan izin iste
                   // status = await Permissions.RequestAsync<Permissions.StorageWrite>();
                    status = await Permissions.RequestAsync<Permissions.Media>();

                }

                //status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                status = await Permissions.CheckStatusAsync<Permissions.Media>();

                if (status != PermissionStatus.Granted)
                {
                   // status = await Permissions.RequestAsync<Permissions.StorageRead>();
                    status = await Permissions.RequestAsync<Permissions.Media>();
                }

                if (status == PermissionStatus.Granted)
                {
                    var musicFiles = await FilePicker.PickMultipleAsync(new PickOptions
                    {
                        FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    {DevicePlatform.Android,new[] {"audio/*"} },//Android için ses dosyalarý
                    {DevicePlatform.iOS,new[]{"public.audio"} },//iOS için ses dosyalarý
                }),
                    });

                    if (musicFiles != null && musicFiles.Count() > 0)
                    {
                        try
                        {
                            try
                            {
                                foreach (var file in musicFiles)
                                {
                                    var music = new MusicClass
                                    {
                                        FileName = Path.GetFileName(file.FileName),
                                        FilePath = file.FullPath
                                    };
                                    await viewModel.AddMusic(music);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Hata: {ex.Message}");
                                if (ex.InnerException != null)
                                {

                                    await DisplayAlert("Inner Exception:", $"{ex.InnerException.Message} ekleme baþarýsýz", "OK");
                                }
                            }


                        }
                        catch (Exception ex)
                        {

                            await DisplayAlert("Veri Tabaný Kayýt Sorunu", $"{ex.Message} kayýt baþarýsýz", "OK");
                        }

                        await Toast.Make("Ýþlem Tamamlandý").Show();
                        await Shell.Current.GoToAsync("//ListPage");
                    }


                }
            });
        });


    }





}





