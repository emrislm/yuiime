using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace yuiime.ViewModels
{
    public class AnimePageViewModel : ViewModelBase
    {
        public AnimePageViewModel(INavigationService navigationService): base(navigationService)
        {
            Title = "Anime";
        }
    }
}
