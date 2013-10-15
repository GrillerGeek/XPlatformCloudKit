/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XPlatformCloudKit.Common;
using XPlatformCloudKit.Helpers;
using XPlatformCloudKit.Models;
using XPlatformCloudKit.ViewModels;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace XPlatformCloudKit.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ItemDescriptionView : LayoutAwarePage
    {

        public new ItemDescriptionViewModel ViewModel
        {
            get { return (ItemDescriptionViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public ItemDescriptionView()
        {
            this.InitializeComponent();
            Loaded += ItemDescriptionView_Loaded;
            //DataContext = new ItemDescriptionViewModel(); MVVMCross does not need to set DataContext!
        }

        void ItemDescriptionView_Loaded(object sender, RoutedEventArgs e)
        {
            flipView.SelectedItem = AppState.SelectedItem;
        }

        private WebView browser;
        private void WireUpWebBrowser(object sender, RoutedEventArgs e)
        {
            browser = sender as WebView;
            flipView.SelectionChanged += flipView_SelectionChanged;
            LoadWebContent();
        }

        private void LoadWebContent()
        {
            var selectedItem = flipView.SelectedItem as Item;

            var bc = AppSettings.BackgroundColorOfDescription[0] == '#' ? AppSettings.BackgroundColorOfDescription : FetchBackgroundColor();

            var fc = AppSettings.FontColorOfDescription[0] == '#' ? AppSettings.FontColorOfDescription : FetchFontColor();

            string scriptOptions = string.Empty;
            string disableHyperLinksJS = "<script type='text/javascript'>window.onload = function() {   var anchors = document.getElementsByTagName(\"a\"); for (var i = 0; i < anchors.length; i++) { anchors[i].onclick = function() {return(false);}; }};</script>";

            if (AppSettings.DisableHyperLinksInItemDescriptionView)
                scriptOptions = scriptOptions + disableHyperLinksJS;

            var webcontent = "<HTML>" +
            "<HEAD>" +
            "<meta name=\"viewport\" content=\"width=320, user-scrollable=no\" />"
            +
                scriptOptions
            +
            "<style type='text/css'>a img {border: 0;}</style>" +
            "</HEAD>" +
            "<BODY style=\"background-color:" + bc + ";color:" + fc + "\">" +
            selectedItem.Description +
            "</BODY>" +
            "</HTML>";

            try
            {
                browser.NavigateToString(webcontent);
            }
            catch { };
        }

        private void flipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadWebContent();
        }

        private string FetchFontColor()
        {
            return IsBackgroundBlack() ? "#fff;" : "#000";
        }

        private static bool IsBackgroundBlack()
        {
            return FetchBackgroundColor()[1] != 'F';
        }

        private static string FetchBackgroundColor()
        {
            SolidColorBrush mc = (SolidColorBrush)Application.Current.Resources["ApplicationPageBackgroundThemeBrush"];
            string color = mc.Color.ToString();
            return color.Remove(1, 2);
        }


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        //protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        //{
        //}

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        //protected override void SaveState(Dictionary<String, Object> pageState)
        //{
        //}
    }
}
