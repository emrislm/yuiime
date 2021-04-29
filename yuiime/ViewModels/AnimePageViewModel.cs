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
        private AnimeFromModels selectedAnime, selectedSeasonalAnime, selectedTopAnime;
        private string resultsLabel, seasonLabel, topAnimeLabel;
        private bool isBusy, isBusy2;

        private Jikan jikan;

        private AnimeFromModels tempAnime;
        public ObservableCollection<AnimeFromModels> Animes { get; }
        public ObservableCollection<AnimeFromModels> LatestAnimes { get; }
        public ObservableCollection<AnimeFromModels> TopAnimes { get; }

        private IPageDialogService pageDialogService;

        public AnimePageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "Anime";
            jikan = new Jikan(true);

            Animes = new ObservableCollection<AnimeFromModels>();
            LatestAnimes = new ObservableCollection<AnimeFromModels>();
            TopAnimes = new ObservableCollection<AnimeFromModels>();

            this.pageDialogService = pageDialogService;

            LatestAnime();
            TopAnime();
        }

        public void OnAppearing()
        {
            IsBusy = false;
            IsBusy2 = false;

            SelectedAnime = null;
            SelectedSeasonalAnime = null;
            SelectedTopAnime = null;
        }

        private async void LatestAnime()
        {
            IsBusy2 = true;

            Season season = await jikan.GetSeason();
            foreach (var seasonEntry in season.SeasonEntries)
            {
                tempAnime = new AnimeFromModels();
                tempAnime.L_Id = seasonEntry.MalId;
                tempAnime.L_ImgUrl = seasonEntry.ImageURL;
                tempAnime.L_Name = seasonEntry.Title;
                if (seasonEntry.Score == null)
                {
                    tempAnime.L_Score = "--";
                }
                else
                {
                    tempAnime.L_Score = Convert.ToString(seasonEntry.Score);
                }
                if (seasonEntry.Episodes == null)
                {
                    tempAnime.L_Episodes = "--";
                }
                tempAnime.L_Description = seasonEntry.Synopsis;
                tempAnime.L_Rated = seasonEntry.Type;

                LatestAnimes.Add(tempAnime);

                if (seasonEntry.Score >= 8)
                {
                    tempAnime.L_ScoreTextColor = "LawnGreen";
                }
                else if (seasonEntry.Score >= 5)
                {
                    tempAnime.L_ScoreTextColor = "Orange";
                }
                else
                {
                    tempAnime.L_ScoreTextColor = "Red";
                }
            }
            SeasonLabel = "Latest";

            IsBusy2 = false;
        }
        private async void TopAnime()
        {
            IsBusy2 = true;

            AnimeTop topAnimeList = await jikan.GetAnimeTop();
            foreach (var listEntry in topAnimeList.Top)
            {
                tempAnime = new AnimeFromModels();
                tempAnime.L_Id = listEntry.MalId;
                tempAnime.L_ImgUrl = listEntry.ImageURL;
                tempAnime.L_Name = listEntry.Title;
                if (listEntry.Score == null)
                {
                    tempAnime.L_Score = "No";
                }
                else
                {
                    tempAnime.L_Score = Convert.ToString(listEntry.Score);
                }
                if (listEntry.Episodes == null)
                {
                    tempAnime.L_Episodes = "No";
                }
                else
                {
                    tempAnime.L_Episodes = Convert.ToString(listEntry.Episodes);
                }
                tempAnime.L_Description = "No discription :/";
                tempAnime.L_Rated = "No Rating :/";

                TopAnimes.Add(tempAnime);

                if (listEntry.Score >= 8)
                {
                    tempAnime.L_ScoreTextColor = "LawnGreen";
                }
                else if (listEntry.Score >= 5)
                {
                    tempAnime.L_ScoreTextColor = "Orange";
                }
                else
                {
                    tempAnime.L_ScoreTextColor = "Red";
                }
            }
            TopAnimeLabel = "Best of the Best";

            IsBusy2 = false;
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

                    ScrollToPosition.End;
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

            await NavigationService.NavigateAsync(nameof(AnimeDetailsPage), p, true, true);
        }

        public AnimeFromModels SelectedTopAnime
        {
            get { return selectedTopAnime; }
            set { SetProperty(ref selectedTopAnime, value); OnAnimeSelected(value); }
        }
        public AnimeFromModels SelectedSeasonalAnime
        {
            get { return selectedSeasonalAnime; }
            set { SetProperty(ref selectedSeasonalAnime, value); OnAnimeSelected(value); }
        }
        public AnimeFromModels SelectedAnime
        {
            get { return selectedAnime; }
            set { SetProperty(ref selectedAnime, value); OnAnimeSelected(value); }
        }

        public bool IsBusy2
        {
            get { return isBusy2; }
            set { SetProperty(ref isBusy2, value); }
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public string TopAnimeLabel
        {
            get { return topAnimeLabel; }
            set { SetProperty(ref topAnimeLabel, value); }
        }
        public string SeasonLabel
        {
            get { return seasonLabel; }
            set { SetProperty(ref seasonLabel, value); }
        }
        public string ResultsLabel
        {
            get { return resultsLabel; }
            set { SetProperty(ref resultsLabel, value); }
        }
    }
}
