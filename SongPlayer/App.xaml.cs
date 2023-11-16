using SongPlayer.Dal;
using SongPlayer.Dal.Concreate;
using SongPlayer.Pages;

namespace SongPlayer
{
    public partial class App : Application
    {
        private readonly ImageViewModel viewModel;
        public WhoAmI WhoAmIPage { get; set; } = new WhoAmI();
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            viewModel = new ImageViewModel(new ImageRepository());
            BindingContext = viewModel;
        }

     
        protected override void OnStart()
        {
            base.OnStart();
            // Uygulama başladığında 
            if(Application.Current.MainPage is AppShell shell)
            {
                var viewmodel = shell.BindingContext as ImageViewModel;
                viewModel?.LoadFlyoutHeaderImage();
            }
        }
    }
}
