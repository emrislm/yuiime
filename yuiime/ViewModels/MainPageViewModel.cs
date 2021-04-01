using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yuiime.Views;

namespace yuiime.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public DelegateCommand NavigateToTestpageCommand { get; }
        public MainPageViewModel(INavigationService navigationService): base(navigationService)
        {
            Title = "Main";

            NavigateToTestpageCommand = new DelegateCommand(NavigateToTestpage);
        }

        private async void NavigateToTestpage()
        {
            await NavigationService.NavigateAsync(nameof(TestPage));
        }
    }
}
