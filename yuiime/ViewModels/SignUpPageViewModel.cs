using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using yuiime.Models;
using yuiime.Repo;
using yuiime.Views;

namespace yuiime.ViewModels
{
    public class SignUpPageViewModel : ViewModelBase
    {
        private string username, password, confirmPassword, hashedPassword;

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
            await NavigationService.NavigateAsync(nameof(SignInPage), null, true, true);
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
                await pageDialogService.DisplayAlertAsync("Empty Value(s)", "Please enter Username and Password", "OK");
            }
            else
            {
                HashedPassword = CreateMD5(password);
                tempUser = new Users { Id = Guid.NewGuid().ToString(), Username = Username, Password = HashedPassword };

                var user = await userRepo.AddUserAsync(tempUser);
                if (user)
                {
                    await pageDialogService.DisplayAlertAsync("Success!", "", "Ok");

                    Debug.WriteLine($"Password: {Password} -------------------- HashedPass: {HashedPassword}");

                    Username = "";
                    Password = "";
                    ConfirmPassword = "";

                    await NavigationService.NavigateAsync("NavigationPage/MainTabbedPage?createTab=AnimePage&createTab=MangaPage");
                }
                else
                {
                    await pageDialogService.DisplayAlertAsync("Error", "Signup Fail", "OK");
                }
            }
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
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
