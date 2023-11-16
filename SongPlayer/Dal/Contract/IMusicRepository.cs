

using SongPlayer.Models;

namespace SongPlayer.Dal.Contract
{
    public interface IMusicRepository
    {
        public Task<List<MusicClass>> GetSongList();
        public Task<List<string>> GetFileNamesAsync();

        public Task<MusicClass> GetMusicById(int id);

        public Task RemoveMusic(int id);

        public Task<MusicClass> AddMusic(MusicClass music);

        public Task<FavoriteClass> FavoriteAdd(FavoriteClass favorite);



    }
}
