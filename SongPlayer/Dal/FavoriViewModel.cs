using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SongPlayer.Dal.Concreate;
using SongPlayer.Dal.Contract;
using SongPlayer.Models;
using System.Collections.ObjectModel;

namespace SongPlayer.Dal;


public partial class FavoriViewModel : ObservableObject
{


    private readonly IFavoriteRepository _favoriteRepository;
    private readonly MusicRepository musicRepository;

    [ObservableProperty]
    private ObservableCollection<MusicClass> _musicList;


    [ObservableProperty]
    private ObservableCollection<FavoriteClass> _favoriteList;





    public FavoriViewModel(IFavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
        musicRepository = new MusicRepository();
        FavoriteList = new ObservableCollection<FavoriteClass>();
        MusicList = new ObservableCollection<MusicClass>();
    }
    [RelayCommand]
    public async Task<ObservableCollection<FavoriteClass>> LoadFavoriteList()
    {
        var favoriList = await _favoriteRepository.GetFavoriteList();
        if (favoriList != null && favoriList.Count > 0)
        {
            return new ObservableCollection<FavoriteClass>(favoriList);
        }
        else
        {
            return new ObservableCollection<FavoriteClass>();
        }
    }
    [RelayCommand]
    public async Task<ObservableCollection<MusicClass>> GetFavoriteMusicList()
    {
        var favoriList = await _favoriteRepository.GetFavoriteList();
        var musicList = new List<MusicClass>();

        foreach (var favorite in favoriList)
        {
            var music = await musicRepository.GetMusicById(favorite.MusicId);
            if (music != null)
            {
                musicList.Add(music);
            }
        }

        return new ObservableCollection<MusicClass>(musicList);
    }


    [RelayCommand]
    public async Task<FavoriteClass> AddFavorite(FavoriteClass favorite)
    {
        if (favorite is not null)
        {
            await _favoriteRepository.AddFavorite(favorite);
            var favoriteList = await LoadFavoriteList();
            if (FavoriteList == null)
            {
                FavoriteList = new ObservableCollection<FavoriteClass>();
            }

            FavoriteList.Add(favorite);

            var music = await musicRepository.GetMusicById(favorite.MusicId);
            // Önce MusicList'i başlatıp null olup olmadığını kontrol ediyoruz.
            if (MusicList == null)
            {
                MusicList = new ObservableCollection<MusicClass>();
            }

            MusicList.Add(music);
            return favorite;
        }
        return null;
    }
    [RelayCommand]
    public async Task<FavoriteClass> GetFavoriteById(int id)
    {
        var favorite = await _favoriteRepository.GetFavoriteById(id);
        if (favorite is not null)
        {
            return favorite;
        }
        return null;
    }

    [RelayCommand]
    public async Task RemoveFavorite(int id)
    {

        var favorite = await _favoriteRepository.GetFavoriteById(id);
        if (favorite is not null)
        {
            await _favoriteRepository.RemoveFavorite(id);
            FavoriteList.Remove(favorite);
            var music = await musicRepository.GetMusicById(favorite.MusicId);
            if (music is not null)
            {
                MusicList.Remove(music);
            }

        }

    }


}
