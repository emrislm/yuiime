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
    public class SignUpPageViewModel : ViewModelBase
    {
        private string username, password, confirmPassword;

        private Users tempUser;
        private IUserRepo<Users> userRepo;

        public DelegateCommand GoToSignInCommand { get; }
        private IPageDialogService pageDialogService;
        public SignUpPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IUserRepo<Users> userRepo) : base(navigationService)
        {
            this.userRepo = userRepo;

            this.pageDialogService = pageDialogService;
            GoToSignInCommand = new DelegateCommand(GoToSignIn);
        }

        public async void GoToSignIn()
        {
            await App.Current.MainPage.Navigation.PushAsync(new SignInPage(), true);
        }

        public Command SignUpCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (Password == ConfirmPassword)
                        SignUp();
                    else
                        pageDialogService.DisplayAlertAsync("", "Password must be same as above!", "OK");
                });
            }
        }
        private async void SignUp()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                await pageDialogService.DisplayAlertAsync("Empty Value(s)", "Please enter Email and Password", "OK");
            }
            else
            {
                tempUser = new Users { Id = Guid.NewGuid().ToString(), Username = Username, Password = Password };

                var user = await userRepo.AddUserAsync(tempUser);
                if (user)
                {
                    await pageDialogService.DisplayAlertAsync("Success!", "", "Ok");

                    Username = "";
                    Password = "";

                    await NavigationService.NavigateAsync("NavigationPage/MainTabbedPage?createTab=AnimePage&createTab=MangaPage");
                }
                else
                {
                    await pageDialogService.DisplayAlertAsync("Error", "Signup Fail", "OK");
                }
            }
        }

        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { SetProperty(ref confirmPassword, value); }
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
