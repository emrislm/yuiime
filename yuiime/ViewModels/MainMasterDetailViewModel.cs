using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace yuiime.ViewModels
{
    public class MainMasterDetailViewModel : ViewModelBase
    {
        public MainMasterDetailViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigateCommand = new DelegateCommand<string>(NavigateCommandExecuted);
        }

        public DelegateCommand<string> NavigateCommand { get; }

        private async void NavigateCommandExecuted(string path)
        {
            await NavigationService.NavigateAsync($"NavigationPage/{path}");
        }
    }
}
