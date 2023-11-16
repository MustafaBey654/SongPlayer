
using SongPlayer.Dal.Contract;
using SongPlayer.Dal.EfCore;
using SongPlayer.Models;

namespace SongPlayer.Dal.Concreate;

public class MusicRepository : IMusicRepository
{
    MusicDatabase database;

    public MusicRepository()
    {
        database = new MusicDatabase();
        // database.Init();
    }

    public async Task<MusicClass> AddMusic(MusicClass music)
    {

        if (music is not null)
        {
            await database.Init();
            await database.Database.InsertAsync(music);
            return music;
        }
        return null;
    }

    public async Task<FavoriteClass> FavoriteAdd(FavoriteClass favorite)
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

    public Task<List<string>> GetFileNamesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<MusicClass> GetMusicById(int id)
    {
        await database.Init();
        var music = await database.Database.Table<MusicClass>().Where(Id => Id.Id == id).FirstOrDefaultAsync();
        if (music is not null)
        {
            return music;
        }
        return null;
    }

    public async Task<List<MusicClass>> GetSongList()
    {
        await database.Init();
        return await database.Database.Table<MusicClass>().ToListAsync();
    }

    public async Task RemoveMusic(int id)
    {
        await database.Init();
        var music = await database.Database.Table<MusicClass>().Where(md => md.Id == id).FirstOrDefaultAsync();
        if (music is not null)
        {
            await database.Database.DeleteAsync(music);
        }

    }
}


