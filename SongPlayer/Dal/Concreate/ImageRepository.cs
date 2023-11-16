using SongPlayer.Dal.Contract;
using SongPlayer.Dal.EfCore;
using SongPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongPlayer.Dal.Concreate
{
    public class ImageRepository : IimageRepository
    {
        private readonly MusicDatabase database;


        public ImageRepository()
        {
            database = new MusicDatabase();
        }
        public async Task<ImageClass> AddImage(ImageClass image)
        {
            if(image is not null)
            {
                await database.Init();
                var newImage = new ImageClass()
                {
                    Image = image.Image
                };

                await database.Database.InsertAsync(newImage);
                return image;

            }

            return null;

        }

        public async Task<ImageClass> GetImageById(int id)
        {
            await database.Init();
            var myImage = await database.Database.Table<ImageClass>().Where(i=>i.Id==id).FirstOrDefaultAsync();
            if(myImage is not null)
            {
                return myImage;
            }
            return null;
        }

        public async Task<List<ImageClass>> GetImageList()
        {
            await database.Init();
            return await database.Database.Table<ImageClass>().ToListAsync();
        }

        public async Task RemoveImage(int id)
        {
            await database.Init();
            var myImages = await database.Database.Table<ImageClass>().Where(i=>i.Id== id).FirstOrDefaultAsync();
            if(myImages is not null)
            {
                await database.Database.DeleteAsync(myImages);
            }
            
        }
    }
}
