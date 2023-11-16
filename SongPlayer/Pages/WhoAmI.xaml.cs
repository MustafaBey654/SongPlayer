using Microsoft.Maui.Controls;
using SongPlayer.Dal;
using SongPlayer.Dal.Concreate;
using System;

namespace SongPlayer.Pages;

public partial class WhoAmI : ContentPage
{ 


    private readonly ImageViewModel viewModel;
    public WhoAmI()
    {
        InitializeComponent();
        viewModel = new ImageViewModel(new ImageRepository());
        BindingContext = viewModel;
    }

    protected override bool OnBackButtonPressed()
    {
        Dispatcher.DispatchAsync(async () =>
        {
            await Shell.Current.GoToAsync("///ListPage");
        });

        return true;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // ViewModel'den güncel resmi alýn
        var imageSource = viewModel.LoadWhoAmIImage();

      
        imageShow.Source = imageSource;
  
    }



}