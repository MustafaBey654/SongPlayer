namespace SongPlayer.Extensions
{
    public static class ExtensService
    {
        public static void ConfigureInitializeDbcontext(this IServiceCollection services, string connectionString)
        {
            //var optionsBuilder = new DbContextOptionsBuilder<MusicDbContext>();
            //optionsBuilder.UseSqlite(connectionString); // Örnek olarak SQL Server kullanılıyor.

            //using (var scope = services.BuildServiceProvider().CreateScope())
            //{
            //    MusicDbInitializer.Initializer(new MusicDbContext(optionsBuilder.Options));
            //}

        }
    }
}
