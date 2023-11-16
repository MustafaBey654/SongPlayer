using System.ComponentModel;

namespace SongPlayer.Dal
{
    public class SharedViewModel : INotifyPropertyChanged
    {
        private static SharedViewModel instance;

        public event PropertyChangedEventHandler PropertyChanged;

        private int musicId = 0;

        public int MusicId
        {
            get { return musicId; }
            set
            {
                musicId = value;
                OnPropertyChanged(nameof(MusicId));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Dışarıdan erişilebilir bir nokta sağla
        public static SharedViewModel Instance
        {
            get
            {
                // Eğer instance null ise, bir örnek oluştur
                if (instance == null)
                {
                    instance = new SharedViewModel();
                }
                return instance;
            }
        }
    }
}
