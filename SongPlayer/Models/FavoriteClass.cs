using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace SongPlayer.Models;

public class FavoriteClass : IEntity //INotifyPropertyChanged
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [ForeignKey(nameof(MusicId))]
    public int MusicId { get; set; }


}
