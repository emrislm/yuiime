using Xamarin.Forms;

namespace yuiime.Views
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);

            InitializeComponent();
        }

        protected override bool OnBackButtonPressed() => true;
    }
}
