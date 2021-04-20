using Prism;
using Prism.Behaviors;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using yuiime.Models;
using yuiime.Repo;
using yuiime.ViewModels;
using yuiime.Views;

namespace yuiime
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/SignInPage");
            //await NavigationService.NavigateAsync("NavigationPage/MainTabbedPage?createTab=AnimePage&createTab=MangaPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterSingleton<IUserRepo<Users>, FirebaseRepo>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<TestPage, TestPageViewModel>();
            containerRegistry.RegisterForNavigation<MainTabbedPage, MainTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<AnimePage, AnimePageViewModel>();
            containerRegistry.RegisterForNavigation<MangaPage, MangaPageViewModel>();
            containerRegistry.RegisterForNavigation<AnimeDetailsPage, AnimeDetailsPageViewModel>();
            containerRegistry.RegisterForNavigation<MangaDetailsPage, MangaDetailsPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpPageViewModel>();
            containerRegistry.RegisterForNavigation<SignInPage, SignInPageViewModel>();
        }
    }
}
