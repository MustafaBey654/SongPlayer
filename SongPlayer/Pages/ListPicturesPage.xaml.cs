
using CommunityToolkit.Maui.Views;
using SongPlayer.Dal;
using SongPlayer.Dal.Concreate;
using SongPlayer.Models;
using System.Collections.ObjectModel;

namespace SongPlayer.Pages;

public partial class ListPicturesPage : ContentPage
{
    private readonly ImageViewModel viewModel;
    private ObservableCollection<ImageClass> ImageList;

    private OptionPicturePage optionsPopup; // OptionsPopupPage inst

    [Obsolete]
    public ListPicturesPage()
	{
		InitializeComponent();
        viewModel = new ImageViewModel(new ImageRepository());
        ImageList = new ObservableCollection<ImageClass>();


        BindingContext = new MusicViewModel(new MusicRepository());

        optionsPopup = new OptionPicturePage(viewModel);
        optionsPopup.ReturnedFromOptionsPage += OnOptionsPopupReturned;

        imagesListView.ItemsSource = ImageList;

        this.Appearing += async (s, e) =>
        {
            await Task.Run(async () =>
            {
                ImageList = await viewModel.LoadImageList();
                imagesListView.ItemsSource = ImageList;
            });


        };


    }

    [Obsolete]
    private async void OnAddImageButtonClicked(object sender, EventArgs e)
	{
        var fileResult = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Resim Seçimi",
            FileTypes = FilePickerFileType.Images
        });

        if(fileResult != null)
        {
            var stream = await fileResult.OpenReadAsync();
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();

            var image = new ImageClass { Image = imageBytes };
            await viewModel.AddImage(image);
            // Resim eklendikten sonra listeyi güncelle
            CheckMusicListUpdate();
        }
    }


    [Obsolete]
    private void OptionsButton_Clicked(object sender, EventArgs e)
    {


        var button = (ImageButton)sender;
        var parentGrid = (Grid)button.Parent;
        var parentFrame = (Frame)parentGrid.Parent;
        var innerGrid = (Grid)parentFrame.Content;
        var label = innerGrid.Children.FirstOrDefault(c => c is Label) as Label;


        if (label != null && label.BindingContext is ImageClass image)
        {
            int imageId = image.Id;
          
            ImageSharedViewModel imageSharedViewModel = ImageSharedViewModel.Instance;
            imageSharedViewModel.ImageId = imageId;

            if (optionsPopup != null)
            {
                optionsPopup.ReturnedFromOptionsPage -= OnOptionsPopupReturned; // Önceki dinlemeyi kaldýr
            }

            optionsPopup.ReturnedFromOptionsPage -= OnOptionsPopupReturned; // Önceki dinlemeyi kaldýr
            optionsPopup = new OptionPicturePage(viewModel);
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
            var imageList = await viewModel.LoadImageList();
            imagesListView.ItemsSource = imageList;
        });

    }


    
}