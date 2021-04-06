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
        private AnimeFromModels tempAnime;

        private IPageDialogService pageDialogService;

        public AnimePageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "Anime Page";
            jikan = new Jikan(true);

            Animes = new ObservableCollection<AnimeFromModels>();

            this.pageDialogService = pageDialogService;
        }

        public ICommand PerformSearch => new Command<string>(async (string query) =>
        {
            if (query != "")
            {
                IsBusy = true;

                try
                {
                    Animes.Clear();

                    AnimeSearchResult animeSearchResult = await jikan.SearchAnime(query, 1);
                    foreach (var item in animeSearchResult.Results)
                    {
                        tempAnime = new AnimeFromModels();
                        tempAnime.L_ImgUrl = item.ImageURL;
                        tempAnime.L_Name = item.Title;
                        tempAnime.L_Score = Convert.ToString(item.Score);
                        tempAnime.L_Episodes = Convert.ToString(item.Episodes);
                        tempAnime.L_Description = item.Description;
                        tempAnime.L_Rated = item.Rated;

                        Animes.Add(tempAnime);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    IsBusy = false;
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
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
    }
}
