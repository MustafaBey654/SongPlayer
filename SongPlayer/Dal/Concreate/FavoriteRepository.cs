using SongPlayer.Dal.Contract;
using SongPlayer.Dal.EfCore;
using SongPlayer.Models;

namespace SongPlayer.Dal.Concreate
{
    public class FavoriteRepository : IFavoriteRepository
    {
        MusicDatabase database;

        public FavoriteRepository()
        {
            database = new MusicDatabase();
        }

        public async Task<FavoriteClass> AddFavorite(FavoriteClass favorite)
        {
            if (favorite is not null)
            {
                await database.Init();
                var favoriMusic = new FavoriteClass()
                {
                    MusicId = favorite.MusicId
                };

                await database.Database.InsertAsync(favoriMusic);
                return favorite;
            }
            return null;
        }

        public async Task<FavoriteClass> GetFavoriteById(int id)
        {
            await database.Init();
            var fabıriteList = await database.Database.Table<FavoriteClass>().ToListAsync();
            var favorite = await database.Database.Table<FavoriteClass>().Where(fd => fd.MusicId == id).FirstOrDefaultAsync();
            if (favorite is not null)
            {
                return favorite;
            }
            return null;
        }

        public async Task<List<FavoriteClass>> GetFavoriteList()
        {
            await database.Init();
            return await database.Database.Table<FavoriteClass>().ToListAsync();
        }

        public async Task RemoveFavorite(int id)
        {
            var favorite = await database.Database.Table<FavoriteClass>().Where(fd => fd.MusicId == id).FirstOrDefaultAsync();
            if (favorite is not null)
            {
                await database.Database.DeleteAsync(favorite);
            }
        }
    }
}
