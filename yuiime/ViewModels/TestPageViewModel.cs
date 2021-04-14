using JikanDotNet;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using yuiime.Views;
using yuiime.Models;
using yuiime.Repo;

namespace yuiime.ViewModels
{
    public class TestPageViewModel : ViewModelBase
    {
        private string username_UP, password_UP, confirmpassword_UP, username_IN, password_IN;

        private Users tempUser;
        private IUserRepo<Users> userRepo;
        public TestPageViewModel(INavigationService navigationService, IUserRepo<Users> userRepo) :base(navigationService)
        {
            this.userRepo = userRepo;
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
        public Command SignUpCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (Password_UP == ConfirmPassword_UP)
                        SignUp();
                    else
                        App.Current.MainPage.DisplayAlert("", "Password must be same as above!", "OK");
                });
            }
        }
        private async void SignIn()
        {
            if (string.IsNullOrEmpty(Username_IN) || string.IsNullOrEmpty(Password_IN))
            {
                await App.Current.MainPage.DisplayAlert("Empty Value(s)", "Please enter Email and Password", "OK");
            }
            else
            {
                tempUser = new Users { Username = Username_IN, Password = Password_IN };

                var user = await userRepo.GetUserAsync(username_IN);
                if (user != null)
                    if (Username_IN == user.Username && Password_IN == user.Password)
                    {
                        await App.Current.MainPage.DisplayAlert("Success!", "", "Ok");
                        await NavigationService.NavigateAsync("NavigationPage/MainTabbedPage?createTab=AnimePage&createTab=MangaPage");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Login Fail", "Please enter correct Username and Password", "OK");
                    }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Login Fail", "User not found", "OK");
                }
            }
        }
        private async void SignUp()
        {
            if (string.IsNullOrEmpty(Username_UP) || string.IsNullOrEmpty(Password_UP))
            {
                await App.Current.MainPage.DisplayAlert("Empty Value(s)", "Please enter Email and Password", "OK");
            }
            else
            {
                tempUser = new Users { Id = Guid.NewGuid().ToString(), Username = Username_UP, Password = Password_UP };

                var user = await userRepo.AddUserAsync(tempUser);
                if (user)
                {
                    await App.Current.MainPage.DisplayAlert("Success!", "", "Ok");
                    await NavigationService.NavigateAsync("NavigationPage/MainTabbedPage?createTab=AnimePage&createTab=MangaPage");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Signup Fail", "OK");
                }
            }
        }

        public string Username_IN
        {
            get { return username_IN; }
            set { SetProperty(ref username_IN, value); }
        }
        public string Password_IN
        {
            get { return password_IN; }
            set { SetProperty(ref password_IN, value); }
        }
        public string Username_UP
        {
            get { return username_UP; }
            set { SetProperty(ref username_UP, value); }
        }
        public string Password_UP
        {
            get { return password_UP; }
            set { SetProperty(ref password_UP, value); }
        }
        public string ConfirmPassword_UP
        {
            get { return confirmpassword_UP; }
            set { SetProperty(ref confirmpassword_UP, value); }
        }
    }
}
