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
        var imageList = await viewModel.LoadImageList();
        //Options
        int id = (int)optionName.Id;

        ImageSharedViewModel imageSharedViewModel = ImageSharedViewModel.Instance;
        int imageId = imageSharedViewModel.ImageId;

        // 1 -> Üst
        // 2-> Alt
        // 3-> Ayarlar
        // 4->Sil

        if (id == 1)
        {

            if (imageList is not null)
            {
                var image = imageList.Where(i => i.Id == imageId).FirstOrDefault();

                // Üst Alana Eklendi
                if(image is not null)
                {
                        ((App.Current.MainPage as AppShell)?.BindingContext as ImageViewModel)?.UpdateFlyoutHeaderImage(image.Image);

                        await CloseAsync();
                        await Toast.Make("Üst Alana Eklendi").Show();
                    
                   
                }
               
            }
            else
            {

                await CloseAsync();
                await Toast.Make("zaten önceden eklenmiþ").Show();
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


                await Toast.Make($"Ekleme  baþarýsýz {ex.Message}").Show();
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
                    Console.WriteLine("BindingContext ImageViewModel türünde deðil.");
                    return;
                }
                myViewModel.UpdateWhoAmIImage(image.Image);
                await Toast.Make("Ayarlar bölümüne resim eklendi.").Show();
                await CloseAsync();
            }
            catch (Exception ex)
            {
                await Toast.Make($"Ekleme  baþarýsýz {ex.Message}").Show();
                await CloseAsync();
            }
        }
        else
        {

            await viewModel.RemoveImage(imageId);
            OnReturnButtonClicked(sender, e); // Bu satýrý ekleyin.
            await viewModel.LoadImageList();
            await Toast.Make("Silme iþlemi baþarýlý").Show();
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