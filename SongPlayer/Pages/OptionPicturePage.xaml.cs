using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using SongPlayer.Dal;
using SongPlayer.Model;
using SongPlayer.Models;
using System.Collections.ObjectModel;

namespace SongPlayer.Pages;

public partial class OptionPicturePage : Popup
{
    public event Action ReturnedFromOptionsPage;
    private readonly OptionPictureClass Option;
    private readonly ImageViewModel viewModel;
    public ObservableCollection<ImageClass> ImageList { get; private set; }
    public OptionPicturePage(ImageViewModel _viewmodel)
    {
        InitializeComponent();
        Option = new OptionPictureClass();
        listView.ItemsSource = Option.GetOptionsList();
        viewModel = _viewmodel;

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
        var imageList = await viewModel.LoadImageList();
        //Options
        int id = (int)optionName.Id;

        ImageSharedViewModel imageSharedViewModel = ImageSharedViewModel.Instance;
        int imageId = imageSharedViewModel.ImageId;

        // 1 -> �st
        // 2-> Alt
        // 3-> Ayarlar
        // 4->Sil

        if (id == 1)
        {

            if (imageList is not null)
            {
                var image = imageList.Where(i => i.Id == imageId).FirstOrDefault();

                // �st Alana Eklendi
                if(image is not null)
                {
                        ((App.Current.MainPage as AppShell)?.BindingContext as ImageViewModel)?.UpdateFlyoutHeaderImage(image.Image);

                        await CloseAsync();
                        await Toast.Make("�st Alana Eklendi").Show();
                    
                   
                }
               
            }
            else
            {

                await CloseAsync();
                await Toast.Make("zaten �nceden eklenmi�").Show();
            }

        }


        else if (id == 2)
        {
            try
            {
                var image = imageList.Where(i => i.Id == imageId).FirstOrDefault();  //await viewModel.GetImageById(id);

                // Alt alana resim eklendi
                ((App.Current.MainPage as AppShell)?.BindingContext as ImageViewModel)?.UpdateFlyoutFooterImage(image.Image);


                await Toast.Make("Alt alana resim eklendi.").Show();
                await CloseAsync();
            }
            catch (Exception ex)
            {


                await Toast.Make($"Ekleme  ba�ar�s�z {ex.Message}").Show();
                await CloseAsync();
            }
        }
        else if (id == 3)
        {
            try
            {
                var image = imageList.Where(i => i.Id == imageId).FirstOrDefault();

                var myViewModel = ((App.Current as App).WhoAmIPage.BindingContext as ImageViewModel);
                if (myViewModel == null)
                {
                    Console.WriteLine("BindingContext ImageViewModel t�r�nde de�il.");
                    return;
                }
                myViewModel.UpdateWhoAmIImage(image.Image);
                await Toast.Make("Ayarlar b�l�m�ne resim eklendi.").Show();
                await CloseAsync();
            }
            catch (Exception ex)
            {
                await Toast.Make($"Ekleme  ba�ar�s�z {ex.Message}").Show();
                await CloseAsync();
            }
        }
        else
        {

            await viewModel.RemoveImage(imageId);
            OnReturnButtonClicked(sender, e); // Bu sat�r� ekleyin.
            await viewModel.LoadImageList();
            await Toast.Make("Silme i�lemi ba�ar�l�").Show();
            await CloseAsync();

            
        }



    }

    public ImageSource ImageFromByteArray(byte[] imageData)
    {
        if (imageData == null)
            return null;

        return ImageSource.FromStream(() => new MemoryStream(imageData));
    }

}