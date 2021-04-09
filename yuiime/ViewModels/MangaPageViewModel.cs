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
using yuiime.Models.Manga;
using yuiime.Views;

namespace yuiime.ViewModels
{
    public class MangaPageViewModel : ViewModelBase
    {
        private Jikan jikan;

        private MangaFromModels tempManga;
        public ObservableCollection<MangaFromModels> Mangas { get; }
        public ObservableCollection<MangaFromModels> TopMangas { get; }

        private IPageDialogService pageDialogService;
        public MangaPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "Manga";
            jikan = new Jikan(true);

            Mangas = new ObservableCollection<MangaFromModels>();
            TopMangas = new ObservableCollection<MangaFromModels>();

            this.pageDialogService = pageDialogService;
        }

        public void OnAppearing()
        {
            IsBusy = false;

            SelectedManga = null;
            SelectedTopManga = null;
        }

        private async void TopMangasInit()
        {
            MangaTop topMangaList = await jikan.GetMangaTop();
            foreach (var listEntry in topMangaList.Top)
            {
                tempManga = new MangaFromModels();
                tempManga.L_Id = listEntry.MalId;
                tempManga.L_ImgUrl = listEntry.ImageURL;
                tempManga.L_Name = listEntry.Title;
                if (listEntry.Score == null)
                {
                    tempManga.L_Score = "--";
                }
                else
                {
                    tempManga.L_Score = Convert.ToString(listEntry.Score);
                }
                if (listEntry.Volumes == null)
                {
                    tempManga.L_Volumes = "No";
                }
                else
                {
                    tempManga.L_Volumes = Convert.ToString(listEntry.Volumes);
                }
                tempManga.L_Chapters = "No";
                tempManga.L_Description = "No discription :/";

                TopMangas.Add(tempManga);

                if (listEntry.Score >= 8)
                {
                    tempManga.L_ScoreTextColor = "LawnGreen";
                }
                else if (listEntry.Score >= 5)
                {
                    tempManga.L_ScoreTextColor = "Orange";
                }
                else
                {
                    tempManga.L_ScoreTextColor = "Red";
                }
            }
            TopMangaLabel = "Best of the Best";
        }

        public ICommand PerformSearch => new Command<string>(async (string query) =>
        {
            if (query != "")
            {
                IsBusy = true;

                try
                {
                    Mangas.Clear();

                    MangaSearchResult mangaSearchResult = await jikan.SearchManga(query, 1);
                    foreach (var item in mangaSearchResult.Results)
                    {
                        tempManga = new MangaFromModels();
                        tempManga.L_Id = item.MalId;
                        tempManga.L_ImgUrl = item.ImageURL;
                        tempManga.L_Name = item.Title;
                        tempManga.L_Score = Convert.ToString(item.Score);
                        tempManga.L_Chapters = Convert.ToString(item.Chapters);
                        tempManga.L_Volumes = Convert.ToString(item.Volumes);
                        tempManga.L_Description = item.Description;

                        Mangas.Add(tempManga);

                        if (item.Score >= 8)
                        {
                            tempManga.L_ScoreTextColor = "LawnGreen";
                        }
                        else if (item.Score >= 5)
                        {
                            tempManga.L_ScoreTextColor = "Orange";
                        }
                        else
                        {
                            tempManga.L_ScoreTextColor = "Red";
                        }
                    }
                    ResultsLabel = "Results";

                    TopMangasInit();
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

        private async void OnMangaSelected(MangaFromModels manga)
        {
            if (manga == null)
            {
                return;
            }

            var p = new NavigationParameters();
            p.Add("manga", manga);

            await NavigationService.NavigateAsync(nameof(MangaDetailsPage), p);
        }

        private MangaFromModels selectedTopManga;
        public MangaFromModels SelectedTopManga
        {
            get { return selectedTopManga; }
            set { SetProperty(ref selectedTopManga, value); OnMangaSelected(value); }
        }
        private MangaFromModels selectedManga;
        public MangaFromModels SelectedManga
        {
            get { return selectedManga; }
            set { SetProperty(ref selectedManga, value); OnMangaSelected(value); }
        }
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private string resultsLabel;
        public string ResultsLabel
        {
            get { return resultsLabel; }
            set { SetProperty(ref resultsLabel, value); }
        }
        private string topMangaLabel;
        public string TopMangaLabel
        {
            get { return topMangaLabel; }
            set { SetProperty(ref topMangaLabel, value); }
        }
    }
}
