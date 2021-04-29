using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using yuiime.Models;
using yuiime.Repo;
using yuiime.Views;

namespace yuiime.ViewModels
{
    public class SignInPageViewModel : ViewModelBase
    {
        private string username, password, hashedPassword;

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
            await NavigationService.NavigateAsync(nameof(SignUpPage), null, true, true);
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
                HashedPassword = CreateMD5(password);

                var user = await userRepo.GetUserAsync(username);
                if (user != null)
                    if (Username == user.Username && HashedPassword == user.Password)
                    {
                        await pageDialogService.DisplayAlertAsync("Success!", "", "Ok");

                        Username = "";
                        Password = "";

                        await NavigationService.NavigateAsync("NavigationPage/MainTabbedPage?createTab=AnimePage&createTab=MangaPage", null, true, true);
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

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public string HashedPassword
        {
            get { return hashedPassword; }
            set { SetProperty(ref hashedPassword, value); }
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
