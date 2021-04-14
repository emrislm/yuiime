using Xamarin.Forms;

namespace yuiime.Views
{
    public partial class SignInPage : ContentPage
    {
        public SignInPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);

            InitializeComponent();
        }

        protected override bool OnBackButtonPressed() => true;
    }
}
