using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using APIMASHLib;
using APIMASH_CNorrisLib;


namespace APIMASH_CNorris_Universal
{
    public sealed partial class MainPage : Page
    {
        readonly APIMASHInvoke apiInvoke;

        public MainPage()
        {
            this.InitializeComponent();
            apiInvoke = new APIMASHInvoke();
            apiInvoke.OnResponse += apiInvoke_OnResponse;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Invoke();
        }
        
        ////////////////////////////////////////////////////////////////////////////////////
        // Update this routine to build the URI to invoke the API 
        // determine how you want to build the API call: 
        //     a) using user input
        //     b) hard coded values
        //     c) all of the above
        ///////////////////////////////////////////////////////////////////////////////////
        private void Invoke()
        {
            const string apiCall = @"http://api.icndb.com/jokes/random?exclude=[explicit]";
            apiInvoke.Invoke<CNorrisJoke>(apiCall);
        }

        async private void apiInvoke_OnResponse(object sender, APIMASHEvent e)
        {
            var response = (CNorrisJoke)e.Object;

            if (e.Status == APIMASHStatus.SUCCESS)
            {
                var s = response.Value.Joke;
                s = s.Replace("&quot;", "'");
                Joke.Text = s;
            }
            else
            {
                var md = new MessageDialog(e.Message, "Error");
                bool? result = null;
                md.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler((cmd) => result = true)));
                await md.ShowAsync();
            }
        }

        private void HitMeButtonClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Invoke();
        }
    }
}
