using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using yuiime.Models;
using yuiime.Repo;
using yuiime.Views;

namespace yuiime.ViewModels
{
    public class SignInPageViewModel : ViewModelBase
    {
        private string username, password;

        private Users tempUser;
        private IUserRepo<Users> userRepo;

        public DelegateCommand GoToSignUpCommand { get; }
        private IPageDialogService pageDialogService;
        public SignInPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IUserRepo<Users> userRepo): base(navigationService)
        {
            this.userRepo = userRepo;

            this.pageDialogService = pageDialogService;
            GoToSignUpCommand = new DelegateCommand(GoToSignUp);
        }

        public async void GoToSignUp()
        {
            await App.Current.MainPage.Navigation.PushAsync(new SignUpPage(), true);
        }

        public Command SignInCommand
        {
            get
            {
                return new Command(() =>
                {
                    SignIn();
                });
            }
        }
        private async void SignIn()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                await pageDialogService.DisplayAlertAsync("Empty Value(s)", "Please enter Email and Password", "OK");
            }
            else
            {
                tempUser = new Users { Username = Username, Password = Password };

                var user = await userRepo.GetUserAsync(username);
                if (user != null)
                    if (Username == user.Username && Password == user.Password)
                    {
                        await pageDialogService.DisplayAlertAsync("Success!", "", "Ok");

                        Username = "";
                        Password = "";

                        await NavigationService.NavigateAsync("NavigationPage/MainTabbedPage?createTab=AnimePage&createTab=MangaPage");
                    }
                    else
                    {
                        await pageDialogService.DisplayAlertAsync("Login Fail", "Please enter correct Username and Password", "OK");
                    }
                else
                {
                    await pageDialogService.DisplayAlertAsync("Login Fail", "User not found", "OK");
                }
            }
        }

        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }
    }
}
