using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

[assembly: ExportFont("Montserrat-Black.ttf")]
[assembly: ExportFont("Montserrat-BlackItalic.ttf")]
[assembly: ExportFont("Montserrat-Bold.ttf")]
[assembly: ExportFont("Montserrat-BoldItalic.ttf")]
[assembly: ExportFont("Montserrat-ExtraBold.ttf")]
[assembly: ExportFont("Montserrat-ExtraBoldItalic.ttf")]
[assembly: ExportFont("Montserrat-ExtraLight.ttf")]
[assembly: ExportFont("Montserrat-ExtraLightItalic.ttf")]
[assembly: ExportFont("Montserrat-Italic.ttf")]
[assembly: ExportFont("Montserrat-Light.ttf")]
[assembly: ExportFont("Montserrat-LightItalic.ttf")]
[assembly: ExportFont("Montserrat-Medium.ttf")]
[assembly: ExportFont("Montserrat-MediumItalic.ttf")]
[assembly: ExportFont("Montserrat-Regular.ttf")]
[assembly: ExportFont("Montserrat-SemiBold.ttf")]
[assembly: ExportFont("Montserrat-SemiBoldItalic.ttf")]
[assembly: ExportFont("Montserrat-Thin.ttf")]
[assembly: ExportFont("Montserrat-ThinItalic.ttf")]

[assembly: ExportFont("Roboto-Black.ttf")]
[assembly: ExportFont("Roboto-BlackItalic.ttf")]
[assembly: ExportFont("Roboto-Bold.ttf")]
[assembly: ExportFont("Roboto-BoldItalic.ttf")]
[assembly: ExportFont("Roboto-Italic.ttf")]
[assembly: ExportFont("Roboto-Light.ttf")]
[assembly: ExportFont("Roboto-LightItalic.ttf")]
[assembly: ExportFont("Roboto-Medium.ttf")]
[assembly: ExportFont("Roboto-MediumItalic.ttf")]
[assembly: ExportFont("Roboto-Regular.ttf")]
[assembly: ExportFont("Roboto-Thin.ttf")]
[assembly: ExportFont("Roboto-ThinItalic.ttf")]


namespace yuiime.ViewModels
{
    public class MainTabbedPageViewModel : ViewModelBase
    {
        public MainTabbedPageViewModel(INavigationService navigationService): base(navigationService)
        {
            Title = "Main tab";
        }

    }
}
