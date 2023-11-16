using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SongPlayer.Dal;
using SongPlayer.Dal.Concreate;
using SongPlayer.Dal.Contract;
using SongPlayer.Dal.EfCore;
using SongPlayer.Models;
using SongPlayer.Pages;
using System.Collections.ObjectModel;

namespace SongPlayer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitMediaElement()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<ObservableCollection<MusicClass>>();
            builder.Services.AddSingleton<ListPage>();
            builder.Services.AddSingleton<WhoAmI>();
            builder.Services.AddSingleton<FavoriPage>();
            builder.Services.AddSingleton<ImageViewModel>();
            builder.Services.AddSingleton<ListPicturesPage>();
            builder.Services.AddSingleton<ITimeViewModel, TimeViewModel>();
            builder.Services.AddSingleton<MusicViewModel>();
            builder.Services.AddSingleton<FavoriViewModel>();

            builder.Services.AddSingleton<MusicDatabase>();
            builder.Services.AddSingleton<IMusicRepository, MusicRepository>();
            builder.Services.AddSingleton<IFavoriteRepository, FavoriteRepository>();




            builder.Services.AddSingleton<SharedViewModel>();


            return builder.Build();
        }
    }
}
