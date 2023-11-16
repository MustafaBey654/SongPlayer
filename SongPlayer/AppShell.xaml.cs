using SongPlayer.Dal;
using SongPlayer.Dal.Concreate;
using System.ComponentModel;

namespace SongPlayer
{
    public partial class AppShell : Shell
    {
        private readonly ImageViewModel viewModel;
        public static readonly BindableProperty HeaderImageProperty =
       BindableProperty.Create(nameof(HeaderImage), typeof(ImageSource), typeof(AppShell), default(ImageSource));

        public static readonly BindableProperty FooterImageProperty =
            BindableProperty.Create(nameof(FooterImage),typeof(ImageSource),typeof(AppShell), default(ImageSource));
        
        
        public ImageSource FooterImage
        {
            get { return (ImageSource)GetValue(FooterImageProperty); }
            set { SetValue(FooterImageProperty, value); }
        }
        
        public ImageSource HeaderImage
        {
            get { return (ImageSource)GetValue(HeaderImageProperty); }
            set { SetValue(HeaderImageProperty, value); }
        }



        public AppShell()
        {
            InitializeComponent();
            viewModel = new ImageViewModel(new ImageRepository());
            BindingContext = viewModel;
        }

     

    }
}
