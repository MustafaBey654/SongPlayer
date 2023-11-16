using SongPlayer.Models;
using SQLite;

namespace SongPlayer.Dal.EfCore
{
    public class MusicDatabase
    {

        public SQLiteAsyncConnection Database;

        public MusicDatabase()
        {

        }

        public async Task Init()
        {
            if (Database is not null)
            {
                return;
            }

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            await Database.CreateTableAsync<MusicClass>();
            await Console.Out.WriteLineAsync("İşlem başarılı MusicClass");
            await Database.CreateTableAsync<FavoriteClass>();
            await Console.Out.WriteLineAsync("İşlem başarılı FavoriteClass");

            await Database.CreateTableAsync<ImageClass>();
            await Console.Out.WriteLineAsync("Image Veri tabanı oluşturuldu.");


        }


    }
}
