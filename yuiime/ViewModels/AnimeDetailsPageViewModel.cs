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
using Xamarin.Essentials;
using yuiime.Models;

namespace yuiime.ViewModels
{
    public class AnimeDetailsPageViewModel : ViewModelBase
    {
        private Jikan jikan;
        private string l_Title, l_Description, l_Episodes, l_Rated, l_Score, l_ImgPath;
        private long l_Id;
        private int l_Completed, l_Dropped, l_OnHold, l_PlanToWatch, l_Watching, l_Total;

        private AnimeFromModels anime;
        private StaffFromModels tempStaff;
        private NewsFromModels tempNews;

        public ObservableCollection<StaffFromModels> AnimeStaff { get; }
        public ObservableCollection<NewsFromModels> AnimeNews { get; }

        public AnimeDetailsPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "Anime";
            jikan = new Jikan(true);

            AnimeStaff = new ObservableCollection<StaffFromModels>();
            AnimeNews = new ObservableCollection<NewsFromModels>();
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("anime"))
            {
                anime = parameters.GetValue<AnimeFromModels>("anime");

                GetAnime(anime);              
                GetStaff(l_Id);
                GetStats(l_Id);
                GetNews(l_Id);
            }  
        }

        // --------------FUNCTIONS--------------
        public async void GetAnime(AnimeFromModels anime)
        {
            L_Id = anime.L_Id;
            L_Title = anime.L_Name;
            L_Description = anime.L_Description;
            L_Episodes = anime.L_Episodes;
            L_Rated = anime.L_Rated;
            L_Score = anime.L_Score;

            AnimePictures pictures = await jikan.GetAnimePictures(l_Id);
            L_ImgPath = pictures.Pictures.First().Large;
        }
        public async void GetStaff(long id)
        {
            AnimeCharactersStaff charactersStaff = await jikan.GetAnimeCharactersStaff(id);
            foreach (StaffPositionEntry staffMember in charactersStaff.Staff)
            {
                tempStaff = new StaffFromModels();
                tempStaff.L_StaffImg = staffMember.ImageURL;
                tempStaff.L_StaffName = staffMember.Name;
                tempStaff.L_StaffRole = staffMember.Role.First();

                AnimeStaff.Add(tempStaff);
            }
        }
        public async void GetStats(long id)
        {
            AnimeStats stats = await jikan.GetAnimeStatistics(id);
            L_Completed = (int)stats.Completed;
            L_Dropped = (int)stats.Dropped;
            L_OnHold = (int)stats.OnHold;
            L_PlanToWatch = (int)stats.PlanToWatch;
            L_Watching = (int)stats.Watching;
            L_Total = l_Completed + l_Dropped + l_OnHold + l_PlanToWatch + l_Watching;
        }
        public async void GetNews(long id)
        {
            AnimeNews news = await jikan.GetAnimeNews(id);
            foreach (News newsEntry in news.News)
            {
                tempNews = new NewsFromModels();
                tempNews.L_ImgUrl = newsEntry.ImageURL;
                tempNews.L_Title = newsEntry.Title;
                tempNews.L_Author = newsEntry.Author;
                tempNews.L_Date = (DateTime)newsEntry.Date;

                AnimeNews.Add(tempNews);
            }
        }
        // --------------FUNCTIONS--------------

        // --------------PROPERTIES--------------
        // anime
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
            get { return l_ImgPath; }
            set { SetProperty(ref l_ImgPath, value); }
        }
        public long L_Id
        {
            get { return l_Id; }
            set { SetProperty(ref l_Id, value); }
        }
        // END anime
        // stats
        public int L_Completed
        {
            get { return l_Completed; }
            set { SetProperty(ref l_Completed, value); }
        }
        public int L_Dropped
        {
            get { return l_Dropped; }
            set { SetProperty(ref l_Dropped, value); }
        }
        public int L_OnHold
        {
            get { return l_OnHold; }
            set { SetProperty(ref l_OnHold, value); }
        }
        public int L_PlanToWatch
        {
            get { return l_PlanToWatch; }
            set { SetProperty(ref l_PlanToWatch, value); }
        }
        public int L_Watching
        {
            get { return l_Watching; }
            set { SetProperty(ref l_Watching, value); }
        }
        public int L_Total
        {
            get { return l_Total; }
            set { SetProperty(ref l_Total, value); }
        }
        // END stats
        // --------------PROPERTIES--------------
    }
}
