using SongPlayer.Models;


namespace SongPlayer.Dal.Contract
{
    public interface IimageRepository
    {
        public Task<List<ImageClass>> GetImageList();
        public Task<ImageClass> GetImageById(int id);
        public Task<ImageClass> AddImage(ImageClass image);
        Task RemoveImage(int id);
    }
}
