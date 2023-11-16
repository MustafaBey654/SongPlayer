using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SongPlayer.Dal.Concreate;
using SongPlayer.Dal.Contract;
using SongPlayer.Models;
using System.Collections.ObjectModel;



namespace SongPlayer.Dal;

public partial class MusicViewModel : ObservableObject
{


    private readonly IMusicRepository _musicRepository;

    private readonly FavoriteRepository favoriteRepository;

    [ObservableProperty]
    private ObservableCollection<MusicClass> musicList;

    [ObservableProperty]
    private MusicClass musicClass;


    public MusicViewModel(IMusicRepository musicRepository)
    {
        _musicRepository = musicRepository;
        favoriteRepository = new FavoriteRepository();
        MusicList = new ObservableCollection<MusicClass>(); // Burada başlatılıyor
        MusicClass = new MusicClass();
    }

    [RelayCommand]
    public async Task<ObservableCollection<MusicClass>> LoadMusicList()
    {
        try
        {
            var songList = await _musicRepository.GetSongList();

            if (songList != null)
            {
                return new ObservableCollection<MusicClass>(songList);
            }
            else
            {
                // Handle case when no music is available
                return new ObservableCollection<MusicClass>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Hata  mesajı dikkat => {ex.Message}  {ex.InnerException?.Message}");
            return new ObservableCollection<MusicClass>();
        }
    }

    [RelayCommand]
    public async Task<MusicClass> AddMusic(MusicClass music)
    {
        if (music is not null)
        {
            await _musicRepository.AddMusic(music);
            var musicList = await LoadMusicList(); // Yeni müziği ekledikten sonra listeyi güncelle  
            MusicList = musicList;
            return music;

        }

        return null;

    }

    [RelayCommand]
    public async Task<MusicClass> GetMusicById(int id)
    {
        var music = await _musicRepository.GetMusicById(id);
        if (music is not null)
        {
            return music; //MusicList[id];

        }
        return null;
    }


    [RelayCommand]
    public async Task RemoveMusic(int id)
    {

        var music = await _musicRepository.GetMusicById(id);
        var liste = await LoadMusicList();

        if (music is not null)
        {
            await _musicRepository.RemoveMusic(id);
            if (MusicList != null) // MusicList null değilse işlemi gerçekleştir
            {
                MusicList.Remove(music);
            }
        }


    }
}









































//        private static MusicViewModel instance;

//        public event PropertyChangedEventHandler PropertyChanged;


//        private ObservableCollection<string> _musicFileNames;
//        public ObservableCollection<string> MusicFileNames
//        {
//            get { return _musicFileNames; }
//            set
//            {
//                if (_musicFileNames != value)
//                {
//                    _musicFileNames = value;
//                    OnPropertyChanged(nameof(MusicFileNames));
//                }
//            }
//        }



//        private ObservableCollection<FavoriteClass> _favoriteList;

//        public ObservableCollection<FavoriteClass> FavoriteList
//        {
//            get { return _favoriteList; }
//            set
//            {
//                if( _favoriteList != value )
//                {
//                    _favoriteList = value;
//                    OnPropertyChanged(nameof(FavoriteList));
//                }

//            }
//        }

//        private ObservableCollection<MusicClass> _musicList;
//        public ObservableCollection<MusicClass> MusicList
//        {
//            get { return _musicList; }
//            set
//            {
//                _musicList = value;
//                OnPropertyChanged(nameof(MusicList));
//            }
//        }
//        protected virtual void OnPropertyChanged(string propertyName)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

//        }


//        public MusicViewModel(MusicDbContext context)
//        {
//            dbContext = context;
//            FavoriteList = new ObservableCollection<FavoriteClass>(); // Burada başlatılıyor.
//        }


//        public MusicClass GetMusic(int id)
//        {
//            var musicFile = dbContext.MusicClasses.Where(mId=>mId.Id==id).FirstOrDefault();

//            return musicFile;
//        }

//        public FavoriteClass AddFavori(FavoriteClass favorite)
//        {
//            if (favorite is not null)
//            {
//                // favorite nesnesinin Id'si 0'dan büyükse (yani daha önce veritabanında kaydedilmişse)
//                if (favorite.Id > 0)
//                {
//                    if (!dbContext.FavoriteClassesFavorites.Any(f => f.Id == favorite.Id))
//                    {
//                        dbContext.FavoriteClassesFavorites.Add(favorite);
//                        dbContext.SaveChanges();
//                        FavoriteList.Add(favorite);
//                        return favorite;
//                    }
//                }
//                else
//                {
//                    dbContext.FavoriteClassesFavorites.Add(favorite);
//                    dbContext.SaveChanges();
//                    FavoriteList.Add(favorite);
//                    return favorite;
//                }
//            }

//            return null;

//            //if(favorite is not null && dbContext.FavoriteClassesFavorites.Contains(favorite) == false)
//            //{
//            //    dbContext.FavoriteClassesFavorites.Add(favorite);
//            //    dbContext.SaveChanges();
//            //    FavoriteList.Add(favorite);
//            //    return favorite;
//            //}
//            //return null;
//        }

//        public ObservableCollection<MusicClass> GetMusicList()
//        {
//            if (_musicList == null)
//            {
//                _musicList = new ObservableCollection<MusicClass>();
//            }

//            List<MusicClass> musicList = dbContext.MusicClasses.ToList();
//            ObservableCollection<MusicClass> observableMusicList = new ObservableCollection<MusicClass>(musicList);
//            MusicList = observableMusicList;
//            // OnPropertyChanged(nameof(MusicList)); // OnPropertyChanged metoduyla UI'ya bildiriliyor.
//            return observableMusicList;

//        }



//        public ObservableCollection<string> GetMusicFileNames()
//        {
//            ObservableCollection<string> fileNames = new ObservableCollection<string>(
//                dbContext.FavoriteClassesFavorites
//                    .Where(f => f.MusicClass != null && f.MusicClass.FileName != null)
//                    .Select(f => f.MusicClass.FileName)
//                    .ToList()
//            );

//            ObservableCollection<string> observableMusicFileName = new ObservableCollection<string>(fileNames);
//            MusicFileNames = observableMusicFileName;
//            return observableMusicFileName;
//        } 





//        public ObservableCollection<FavoriteClass> GetFavoriteListName()
//        {
//            List<FavoriteClass> favoriteList = dbContext.FavoriteClassesFavorites.ToList();
//            var listFileName = favoriteList.Where(f => f.MusicClass != null && f.MusicClass.FileName != null)
//                                            .Select(f => new FavoriteClass { MusicClass = new MusicClass { FileName = f.MusicClass.FileName } })
//                                            .ToList();
//            ObservableCollection<FavoriteClass> observableFileNameList = new ObservableCollection<FavoriteClass>(listFileName);
//            FavoriteList = observableFileNameList;
//            return observableFileNameList;
//        }

//        public ObservableCollection<FavoriteClass> GetFavorites()
//        {
//            List<FavoriteClass> favoriteList= dbContext.FavoriteClassesFavorites.ToList();
//            ObservableCollection<FavoriteClass> observableFavoriteList = new ObservableCollection<FavoriteClass>(favoriteList);
//            FavoriteList = observableFavoriteList;
//            return observableFavoriteList;
//        }

//        public void RemoveFavori(FavoriteClass favorite)
//        {
//            dbContext.FavoriteClassesFavorites.Remove(favorite);
//            dbContext.SaveChanges();
//            FavoriteList.Remove(favorite);


//        }

//        public List<PlayListClass> GetPlayList()
//        {
//            return dbContext.PlayListClassSongs.ToList();
//        }

//        public PlayListClass AddPlayList(PlayListClass playlistMusic)
//        {
//            if(playlistMusic is not null)
//            {
//                dbContext.PlayListClassSongs.Add(playlistMusic);
//                dbContext.SaveChanges();
//                return playlistMusic;
//            }
//            return null;

//        }

//        public void RemovePlayList(PlayListClass playlistMusic)
//        {
//            dbContext.PlayListClassSongs.Remove(playlistMusic);
//            dbContext.SaveChanges();
//        }

//        public void AddMusic(MusicClass music)
//        {

//            dbContext.MusicClasses.Add(music);
//            dbContext.SaveChanges();
//            MusicList.Add(music);
//        }

//        public void RemoveMusic(MusicClass music)
//        {

//            dbContext.MusicClasses.Remove(music);
//            dbContext.SaveChanges();
//            MusicList.Remove(music);
//        }
//    }
//}
