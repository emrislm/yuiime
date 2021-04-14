﻿using Xamarin.Forms;

namespace yuiime.Views
{
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);

            InitializeComponent();
        }

        protected override bool OnBackButtonPressed() => true;
    }
}
