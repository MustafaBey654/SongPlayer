using SQLite;

namespace SongPlayer.Models
{
    public partial class MusicClass : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value.Length > 30 ? value.Substring(0, 30) + ".mp3" : value;

            }
        }
        public string FilePath { get; set; }

        //public List<FavoriteClass> Favorites { get; set; }  
        //public List<PlayListSong> PlayListSongs { get; set; }

        public bool IsPlaying { get; set; } // Şarkının çalıp çalmadığını belirten özellik


        public override string ToString()
        {
            return FileName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is MusicClass))
            {
                return false;
            }

            MusicClass other = (MusicClass)obj;

            // Karşılaştırılacak özellikleri buraya ekleyin
            return this.Id == other.Id; // Örneğin, Id özelliği üzerinden karşılaştırma yapıyorsanız.
        }

        public override int GetHashCode()
        {
            // Id özelliği kullanılarak bir hash değeri oluşturun
            return this.Id.GetHashCode();
        }
    }

}

