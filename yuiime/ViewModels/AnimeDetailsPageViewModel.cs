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
using yuiime.Models;

namespace yuiime.ViewModels
{
    public class AnimeDetailsPageViewModel : ViewModelBase
    {
        private Jikan jikan;
        private string l_Title, l_Description, l_Episodes, l_Rated, l_Score, l_ImgPath, l_BigPicture;
        private long l_Id;

        private AnimeFromModels anime;
        private StaffFromModels tempStaff;
        public ObservableCollection<StaffFromModels> AnimeStaff { get; }

        private IPageDialogService pageDialogService;

        public AnimeDetailsPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "Anime";
            jikan = new Jikan(true);

            AnimeStaff = new ObservableCollection<StaffFromModels>();

            this.pageDialogService = pageDialogService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("anime"))
            {
                anime = parameters.GetValue<AnimeFromModels>("anime");

                L_Id = anime.L_Id;
                L_Title = anime.L_Name;
                L_Description = anime.L_Description;
                L_Episodes = anime.L_Episodes;
                L_Rated = anime.L_Rated;
                L_Score = anime.L_Score;
                L_ImgPath = anime.L_ImgUrl;
                L_BigPicture = "";

            }  
        }

        public void AddStaff(long id)
        {
            AnimeCharactersStaff charactersStaff = jikan.GetAnimeCharactersStaff(id).Result;
            if (charactersStaff != null)
            {
                foreach (var staffMember in charactersStaff.Staff)
                {
                    tempStaff = new StaffFromModels();
                    tempStaff.L_StaffImg = "";
                    tempStaff.L_StaffName = staffMember.Name;
                    AnimeStaff.Add(tempStaff);
                }
            }
            else
            {
                Debug.WriteLine("PIC IS NULLLLLLLLLLLL");
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
            get { return l_ImgPath; }
            set { SetProperty(ref l_ImgPath, value); }
        }
        public string L_BigPicture
        {
            get { return l_BigPicture; }
            set { SetProperty(ref l_BigPicture, value); }
        }
        public long L_Id
        {
            get { return l_Id; }
            set { SetProperty(ref l_Id, value); }
        }
    }
}
