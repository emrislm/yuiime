﻿using Xamarin.Forms;

namespace yuiime.Views
{
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed() => true;
    }
}
