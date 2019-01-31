using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Pomoductive.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtendedSplash : Page
    {
        internal Rect splashImageRect; // Rect to store splash screen image coordinates.
        private AppShell shell;
        private SplashScreen splashScreen; // Variable to hold the splash screen object.'
        private IActivatedEventArgs activatedEventArgs;

        public ExtendedSplash(IActivatedEventArgs e)
        {
            this.InitializeComponent();

            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);

            splashScreen = e.SplashScreen;
            this.activatedEventArgs = e;

            if (splashScreen != null)
            {
                // Retrieve the window coordinates of the splash screen image.
                splashImageRect = splashScreen.ImageLocation;
            }
            Resize();

            // Prepare the app shell and window content.
            shell = Window.Current.Content as AppShell ?? new AppShell();
            shell.Language = ApplicationLanguages.Languages[0];

            LoadDataAsync(this.activatedEventArgs);
        }

        private async void LoadDataAsync(IActivatedEventArgs e)
        {
            await App.AppStatisticDataViewModel.ConfigureStatisticsDatas();
            
            Window.Current.Content = shell;
            Window.Current.Activate();

        }

        // Position the extended splash screen image in the same location as the system splash screen image.
        private void Resize()
        {
            if (this.splashScreen == null) return;

            // The splash image's not always perfectly centered. Therefore we need to set our image's position 
            // to match the original one to obtain a clean transition between both splash screens.

            this.splashImage.Height = this.splashScreen.ImageLocation.Height;
            this.splashImage.Width = this.splashScreen.ImageLocation.Width;

            this.splashImage.SetValue(Canvas.TopProperty, this.splashScreen.ImageLocation.Top);
            this.splashImage.SetValue(Canvas.LeftProperty, this.splashScreen.ImageLocation.Left);

            this.progressRing.SetValue(Canvas.TopProperty, this.splashScreen.ImageLocation.Top + this.splashScreen.ImageLocation.Height + 50);
            this.progressRing.SetValue(Canvas.LeftProperty, this.splashScreen.ImageLocation.Left + this.splashScreen.ImageLocation.Width / 2 - this.progressRing.Width / 2);
        }

        void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            // Safely update the extended splash screen image coordinates. This function will be fired in response to snapping, unsnapping, rotation, etc...
            if (splashScreen != null)
            {
                // Update the coordinates of the splash screen image.
                splashImageRect = splashScreen.ImageLocation;
                Resize();
            }
        }
    }
}
