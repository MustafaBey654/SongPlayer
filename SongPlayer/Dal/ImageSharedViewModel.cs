using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongPlayer.Dal
{
    public class ImageSharedViewModel:INotifyPropertyChanged
    {
        private static ImageSharedViewModel instance;
        public event PropertyChangedEventHandler PropertyChanged;

        private int imageId = 0;
        public int ImageId
        {
            get { return imageId; }
            set { 
                imageId = value;
                OnPropertychanged(nameof(ImageId));
            }
        }

        protected virtual void OnPropertychanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Dışarıdan erişebilir bir nokta sağla

        public static ImageSharedViewModel Instance
        {
            get
            {
                //Eğer instance null ise, bir örnek oluştur
                if (instance == null)
                {
                    instance = new ImageSharedViewModel();
                }
                return instance;
            }
        }
    }
}
