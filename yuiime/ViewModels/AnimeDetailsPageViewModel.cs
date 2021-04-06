using JikanDotNet;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace yuiime.ViewModels
{
    public class AnimeDetailsPageViewModel : ViewModelBase
    {
        private Jikan jikan;
        private string inputText, l_Title, l_Description, l_Episodes, l_Rated, l_Score, l_imgPath;

        private IPageDialogService pageDialogService;

        public AnimeDetailsPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "Anime";
            jikan = new Jikan(true);

            this.pageDialogService = pageDialogService;
        }

        //public ICommand PerformSearch => new Command<string>(async (string query) =>
        //{
        //    if (query != "")
        //    {
        //        AnimeSearchResult animeSearchResult = await jikan.SearchAnime(query);

        //        L_Title = animeSearchResult.Results.First().Title;
        //        L_Description = animeSearchResult.Results.First().Description;
        //        L_Episodes = Convert.ToString(animeSearchResult.Results.First().Episodes);
        //        L_Rated = animeSearchResult.Results.First().Rated;
        //        L_Score = Convert.ToString(animeSearchResult.Results.First().Score);
        //        L_ImgPath = animeSearchResult.Results.First().ImageURL;

        //        Console.WriteLine(L_ImgPath);
        //    }
        //    else
        //    {
        //        await pageDialogService.DisplayAlertAsync("Oops", "Hmmm... Een leeg input?", "try again?");

        //        L_Title = String.Empty;
        //        L_Description = String.Empty;
        //        L_Episodes = String.Empty;
        //        L_Rated = String.Empty;
        //        L_Score = String.Empty;
        //        L_ImgPath = String.Empty;
        //    }
        //});


        public string InputText
        {
            get { return inputText; }
            set { SetProperty(ref inputText, value); }
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
