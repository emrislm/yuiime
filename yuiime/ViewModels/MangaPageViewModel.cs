using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace yuiime.ViewModels
{
    public class MangaPageViewModel : ViewModelBase
    {
        public MangaPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Manga";
        }
    }
}
