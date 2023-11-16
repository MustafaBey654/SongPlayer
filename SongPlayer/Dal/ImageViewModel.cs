using CommunityToolkit.Mvvm.ComponentModel;
using SongPlayer.Dal.Contract;
using SongPlayer.Models;
using SongPlayer.Pages;
using System.Collections.ObjectModel;


namespace SongPlayer.Dal
{
   
    public partial class ImageViewModel:ObservableObject
    {

        private readonly IimageRepository _imageRepository;

        [ObservableProperty]
        private ObservableCollection<ImageClass> _imageList;


        public ImageViewModel(IimageRepository repository)
        {
            _imageRepository = repository;
            ImageList = new ObservableCollection<ImageClass>();
        }


        public async Task<ObservableCollection<ImageClass>> LoadImageList()
        {
            var imageList = await _imageRepository.GetImageList();
            if(imageList != null && imageList.Count > 0)
            {
                return new ObservableCollection<ImageClass>(imageList);
            }
            else
            {
                return new ObservableCollection<ImageClass>();
            }
        }

        public async Task<ImageClass> AddImage(ImageClass image)
        {
            if(image is not null)
            {
                await _imageRepository.AddImage(image);
                var imageList = await LoadImageList();
                if(ImageList == null)
                {
                    ImageList = new ObservableCollection<ImageClass>();
                }
                ImageList.Add(image);
                return image;
            }
            return null;
        }

        [Obsolete]
        public void UpdateFlyoutHeaderImage(byte[] imageData)
        {
            if (Application.Current.MainPage is AppShell shell)
            {
                shell.HeaderImage = ImageFromByteArray(imageData);
                var imageSource = ImageFromByteArray(imageData);
                shell.HeaderImage = imageSource;

                // Resmi sakla
                if (imageData != null)
                {
                    // Byte dizisini bir Base64 string'e dönüştür
                    var imageString = Convert.ToBase64String(imageData);

                    // String'i sakla
                    Preferences.Set("headerImage", imageString);
                }
            }
        }

        [Obsolete]
        public void UpdateWhoAmIImage(byte[] imageData)
        {
            if (imageData == null)
            {
                Console.WriteLine("imageData null.");
                return;
            }

            var imageSource = ImageFromByteArray(imageData);
        

            if (imageSource is null)
            {
                Console.WriteLine("null");
            }

            // Resmi sakla
            if (imageData != null)
            {
                // Byte dizisini bir Base64 string'e dönüştür
                var imageString = Convert.ToBase64String(imageData);

                // String'i sakla
                Preferences.Set("whoAmIImage", imageString);
            }
        }


        [Obsolete]
        public void UpdateFlyoutFooterImage(byte[] imageData)
        {
            if(Application.Current.MainPage is AppShell shell)
            {
                shell.FooterImage = ImageFromByteArray(imageData);
                var imageSource = ImageFromByteArray(imageData);
                shell.FooterImage = imageSource;

                //Resmi Sakla
                if(imageData != null)
                {
                    //Byte dizisini bir Base64 string e dönüştür
                    var imageString = Convert.ToBase64String(imageData);
                    // String'i sakla
                    Preferences.Set("footerImage", imageString);
                }
            }
        }

        public ImageSource ImageFromByteArray(byte[] imageData)
        {
            if (imageData == null)
                return null;

            return ImageSource.FromStream(() => new MemoryStream(imageData));
        }

        [Obsolete]
        public void LoadFlyoutHeaderImage()
        {
            if (Preferences.ContainsKey("headerImage"))
            {
                // String i al
                var imageString = Preferences.Get("headerImage", string.Empty);

                // String i diziye dönüştür
                var imageData = Convert.FromBase64String(imageString);
                if (imageData != null)
                {
                    if (Application.Current.MainPage is AppShell shell)
                    {
                        shell.HeaderImage = ImageFromByteArray(imageData);
                    }
                }
            }

            if (Preferences.ContainsKey("footerImage"))
            {
                var imagefooterData = Preferences.Get("footerImage", string.Empty);

                var footerData = Convert.FromBase64String(imagefooterData);
                if(footerData != null)
                {
                    if(Application.Current.MainPage is AppShell shell)
                    {
                        shell.FooterImage = ImageFromByteArray(footerData); 
                    }
                }
            }
        }

        public ImageSource LoadWhoAmIImage()
        {
            if (Preferences.ContainsKey("whoAmIImage"))
            {
                var imageString = Preferences.Get("whoAmIImage", string.Empty);
                var imageData = Convert.FromBase64String(imageString);
                var image = ImageFromByteArray(imageData);
                if( image is not null)
                {
                    return image;

                }
                
            }
            return null;

        }

        public async Task<ImageClass> GetImageById(int id)
        {
            var myImage = await _imageRepository.GetImageById(id);
            if(myImage != null)
            {
                return myImage;
            }

            return null;
        }

        public async Task RemoveImage(int id)
        {
            var image = await _imageRepository.GetImageById(id);
            if(image != null)
            {
                await _imageRepository.RemoveImage(id);
                ImageList.Remove(image);
            }
        }



    }
}
