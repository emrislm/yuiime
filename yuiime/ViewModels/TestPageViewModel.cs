using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace yuiime.ViewModels
{
    public class TestPageViewModel : ViewModelBase
    {
        public DelegateCommand SaveCommand { get; }

        public TestPageViewModel(INavigationService navigationService) :base(navigationService)
        {
            Title = "Test Page hahahah";

            SaveCommand = new DelegateCommand(OnSave);
        }

        private void OnSave()
        {
            LabelText = InputText;
        }

        private string labelText;
        public string LabelText
        {
            get { return labelText; }
            set { SetProperty(ref labelText, value); }
        }
        private string inputText;
        public string InputText
        {
            get { return inputText; }
            set { SetProperty(ref inputText, value); }
        }


    }
}
