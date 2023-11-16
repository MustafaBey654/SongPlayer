

using SongPlayer.Models;

namespace SongPlayer.Dal.Contract
{
    public interface IFavoriteRepository
    {
        public Task<List<FavoriteClass>> GetFavoriteList();
        public Task<FavoriteClass> GetFavoriteById(int id);
        public Task<FavoriteClass> AddFavorite(FavoriteClass favorite);
        Task RemoveFavorite(int id);
    }
}
