using JikanDotNet;
using Prism.AppModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using yuiime.Models;
using yuiime.Views;

namespace yuiime.ViewModels
{
    public class AnimePageViewModel : ViewModelBase
    {
        private Jikan jikan;
        public ObservableCollection<AnimeFromModels> Animes { get; }
        public ObservableCollection<AnimeFromModels> LatestAnimes { get; }
        private AnimeFromModels tempAnime;
        private AnimeFromModels tempSeasonalAnime;

        private IPageDialogService pageDialogService;

        public AnimePageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "Anime";
            jikan = new Jikan(true);

            Animes = new ObservableCollection<AnimeFromModels>();
            LatestAnimes = new ObservableCollection<AnimeFromModels>();

            this.pageDialogService = pageDialogService;
        }

        public void OnAppearing()
        {
            IsBusy = false;
            IsBusy2 = false;

            SelectedAnime = null;
            SelectedSeasonalAnime = null;
        }

        private async void LatestAnimesInit()
        {
            Season season = await jikan.GetSeason();
            foreach (var seasonEntry in season.SeasonEntries)
            {
                tempSeasonalAnime = new AnimeFromModels();
                tempSeasonalAnime.L_Id = seasonEntry.MalId;
                tempSeasonalAnime.L_ImgUrl = seasonEntry.ImageURL;
                tempSeasonalAnime.L_Name = seasonEntry.Title;
                if (seasonEntry.Score == null)
                {
                    tempSeasonalAnime.L_Score = "--";
                }
                else
                {
                    tempSeasonalAnime.L_Score = Convert.ToString(seasonEntry.Score);
                }
                if (seasonEntry.Episodes == null)
                {
                    tempSeasonalAnime.L_Episodes = "--";
                }
                tempSeasonalAnime.L_Description = seasonEntry.Synopsis;
                tempSeasonalAnime.L_Rated = seasonEntry.Type;
                tempSeasonalAnime.L_BigPicture = "";

                LatestAnimes.Add(tempSeasonalAnime);

                if (seasonEntry.Score >= 8)
                {
                    tempSeasonalAnime.L_ScoreTextColor = "LawnGreen";
                }
                else if (seasonEntry.Score >= 5)
                {
                    tempSeasonalAnime.L_ScoreTextColor = "Orange";
                }
                else
                {
                    tempSeasonalAnime.L_ScoreTextColor = "Red";
                }
            }
            SeasonLabel = "Latest";
        }

        public ICommand PerformSearch => new Command<string>(async (string query) =>
        {
            if (query != "")
            {
                IsBusy = true;
                IsBusy2 = true;

                try
                {
                    Animes.Clear();

                    AnimeSearchResult animeSearchResult = await jikan.SearchAnime(query, 1);
                    foreach (var item in animeSearchResult.Results)
                    {
                        tempAnime = new AnimeFromModels();
                        tempAnime.L_Id = item.MalId;
                        tempAnime.L_ImgUrl = item.ImageURL;
                        tempAnime.L_Name = item.Title;
                        tempAnime.L_Score = Convert.ToString(item.Score);
                        tempAnime.L_Episodes = Convert.ToString(item.Episodes);
                        tempAnime.L_Description = item.Description;
                        tempAnime.L_Rated = item.Rated;
                        tempAnime.L_BigPicture = "";

                        Animes.Add(tempAnime);

                        if (item.Score >= 8)
                        {
                            tempAnime.L_ScoreTextColor = "LawnGreen";
                        }
                        else if (item.Score >= 5)
                        {
                            tempAnime.L_ScoreTextColor = "Orange";
                        }
                        else
                        {
                            tempAnime.L_ScoreTextColor = "Red";
                        }
                    }
                    ResultsLabel = "Results";

                    LatestAnimesInit();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    IsBusy = false;
                    IsBusy2 = false;
                }
            }
            else
            {
                await pageDialogService.DisplayAlertAsync("Oops", "Hmmm... Een leeg input?", "try again?");
            }
        });

        private async void OnAnimeSelected(AnimeFromModels anime)
        {
            if (anime == null)
            {
                return;
            }

            var p = new NavigationParameters();
            p.Add("anime", anime);

            await NavigationService.NavigateAsync(nameof(AnimeDetailsPage), p);
        }

        private AnimeFromModels selectedAnime;
        public AnimeFromModels SelectedAnime
        {
            get { return selectedAnime; }
            set { SetProperty(ref selectedAnime, value); OnAnimeSelected(value); }
        }
        private AnimeFromModels selectedSeasonalAnime;
        public AnimeFromModels SelectedSeasonalAnime
        {
            get { return selectedSeasonalAnime; }
            set { SetProperty(ref selectedSeasonalAnime, value); OnAnimeSelected(value); }
        }
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        private bool isBusy2;
        public bool IsBusy2
        {
            get { return isBusy2; }
            set { SetProperty(ref isBusy2, value); }
        }

        private string resultsLabel;
        public string ResultsLabel
        {
            get { return resultsLabel; }
            set { SetProperty(ref resultsLabel, value); }
        }
        private string seasonLabel;
        public string SeasonLabel
        {
            get { return seasonLabel; }
            set { SetProperty(ref seasonLabel, value); }
        }
    }
}
