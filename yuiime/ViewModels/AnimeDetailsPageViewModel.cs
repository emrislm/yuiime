using JikanDotNet;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using yuiime.Models;

namespace yuiime.ViewModels
{
    public class AnimeDetailsPageViewModel : ViewModelBase
    {
        private Jikan jikan;
        private string l_Title, l_Description, l_Episodes, l_Rated, l_Score, l_imgPath;

        private AnimeFromModels anime;

        private IPageDialogService pageDialogService;

        public AnimeDetailsPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "Anime";
            jikan = new Jikan(true);

            this.pageDialogService = pageDialogService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("anime"))
            {
                anime = parameters.GetValue<AnimeFromModels>("anime");

                L_Title = anime.L_Name;
                L_Description = anime.L_Description;
                L_Episodes = anime.L_Episodes;
                L_Rated = anime.L_Rated;
                L_Score = anime.L_Score;
                L_ImgPath = anime.L_ImgUrl;
            }
        }

        public string L_Title
        {
            get { return l_Title; }
            set { SetProperty(ref l_Title, value); }
        }
        public string L_Description
        {
            get { return l_Description; }
            set { SetProperty(ref l_Description, value); }
        }
        public string L_Episodes
        {
            get { return l_Episodes; }
            set { SetProperty(ref l_Episodes, value); }
        }
        public string L_Rated
        {
            get { return l_Rated; }
            set { SetProperty(ref l_Rated, value); }
        }
        public string L_Score
        {
            get { return l_Score; }
            set { SetProperty(ref l_Score, value); }
        }
        public string L_ImgPath
        {
            get { return l_imgPath; }
            set { SetProperty(ref l_imgPath, value); }
        }
    }
}
