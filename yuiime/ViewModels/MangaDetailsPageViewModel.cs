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
using yuiime.Models.Manga;
using yuiime.Models;

namespace yuiime.ViewModels
{
    public class MangaDetailsPageViewModel : ViewModelBase
    {
        private Jikan jikan;
        private string l_Title, l_Description, l_Chapters, l_Volumes, l_Score, l_ImgPath;
        private long l_Id;
        private int l_Completed, l_Dropped, l_OnHold, l_PlanToRead, l_Reading, l_Total;

        private MangaFromModels manga;
        private CharactersFromModels tempCharacter;
        private NewsFromModels tempNews;

        public ObservableCollection<CharactersFromModels> MangaCharacters { get; }
        public ObservableCollection<NewsFromModels> MangaNews { get; }

        public MangaDetailsPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "Manga";
            jikan = new Jikan(true);

            MangaCharacters = new ObservableCollection<CharactersFromModels>();
            MangaNews = new ObservableCollection<NewsFromModels>();
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("manga"))
            {
                manga = parameters.GetValue<MangaFromModels>("manga");

                GetManga(manga);
                GetCharacters(l_Id);
                GetStats(l_Id);
                GetNews(l_Id);
            }
        }

        // --------------FUNCTIONS--------------
        public async void GetManga(MangaFromModels manga)
        {
            L_Id = manga.L_Id;
            L_Title = manga.L_Name;
            L_Description = manga.L_Description;
            L_Chapters = manga.L_Chapters;
            L_Volumes = manga.L_Volumes;
            L_Score = manga.L_Score;

            MangaPictures pictures = await jikan.GetMangaPictures(l_Id);
            L_ImgPath = pictures.Pictures.First().Large;
        }
        public async void GetCharacters(long id)
        {
            MangaCharacters mangaCharacters = await jikan.GetMangaCharacters(id);
            foreach (CharacterEntry mangaCharacter in mangaCharacters.Characters)
            {
                tempCharacter = new CharactersFromModels();
                tempCharacter.L_CharacterImg = mangaCharacter.ImageURL;
                tempCharacter.L_CharacterName = mangaCharacter.Name;
                tempCharacter.L_CharacterRole = mangaCharacter.Role;

                MangaCharacters.Add(tempCharacter);
            }
        }
        public async void GetStats(long id)
        {
            MangaStats stats = await jikan.GetMangaStatistics(id);
            L_Completed = (int)stats.Completed;
            L_Dropped = (int)stats.Dropped;
            L_OnHold = (int)stats.OnHold;
            L_PlanToRead = (int)stats.PlanToRead;
            L_Reading = (int)stats.Reading;
            L_Total = l_Completed + l_Dropped + l_OnHold + L_PlanToRead + l_Reading;
        }
        public async void GetNews(long id)
        {
            MangaNews news = await jikan.GetMangaNews(id);
            foreach (News newsEntry in news.News)
            {
                tempNews = new NewsFromModels();
                tempNews.L_ImgUrl = newsEntry.ImageURL;
                tempNews.L_Title = newsEntry.Title;
                tempNews.L_Author = newsEntry.Author;
                tempNews.L_Date = (DateTime)newsEntry.Date;

                MangaNews.Add(tempNews);
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
        public string L_Chapters
        {
            get { return l_Chapters; }
            set { SetProperty(ref l_Chapters, value); }
        }
        public string L_Volumes
        {
            get { return l_Volumes; }
            set { SetProperty(ref l_Volumes, value); }
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
        public int L_PlanToRead
        {
            get { return l_PlanToRead; }
            set { SetProperty(ref l_PlanToRead, value); }
        }
        public int L_Reading
        {
            get { return l_Reading; }
            set { SetProperty(ref l_Reading, value); }
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
