
using SQLite;

namespace SongPlayer.Models
{
    public class ImageClass:IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public byte[] Image { get; set; }
    }
}
