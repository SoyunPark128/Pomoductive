using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.EntityFrameworkCore;
using Pomoductive.ViewModels;
using Pomoductive.Views;
using Windows.Globalization;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using Pomoductive.Repository;
using Pomoductive.Repository.Sql;

namespace Pomoductive
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// Gets the app-wide MainViewModel and AppStopwatch singleton instances.
        public static ApplicationViewModel AppViewModel { get; } = new ApplicationViewModel();
        

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }


        /// <summary>s
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // Load the database.
            UseSqlite();

            // Prepare the app shell and window content.
            AppShell shell = Window.Current.Content as AppShell ?? new AppShell();
            shell.Language = ApplicationLanguages.Languages[0];


            Window.Current.Content = shell;

            if (shell.AppFrame.Content == null)
            {
                // When the navigation stack isn't restored, navigate to the first page
                // suppressing the initial entrance animation.
                shell.AppFrame.Navigate(typeof(MainPage), null,
                    new SuppressNavigationTransitionInfo());
            }

            Window.Current.Activate();

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            // Set active window colors
            titleBar.ForegroundColor = Colors.White;
            titleBar.BackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.ButtonHoverForegroundColor = Colors.White;
            titleBar.ButtonPressedForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonHoverBackgroundColor = Colors.DimGray;
            titleBar.ButtonPressedBackgroundColor = Colors.Gray;

            // Set inactive window colors
            titleBar.InactiveForegroundColor = Color.FromArgb(218, 255, 255, 255);
            titleBar.InactiveBackgroundColor = Color.FromArgb(255, 255, 255, 255);
            titleBar.ButtonInactiveForegroundColor = Color.FromArgb(218, 255, 255, 255);
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }


        /// <summary>
        /// Pipeline for interacting with backend service or database.
        /// </summary>
        public static IPomoductiveRepository Repository { get; private set; }

        /// <summary>
        /// Configures the app to use the Sqlite data source. If no existing Sqlite database exists, 
        /// loads a demo database filled with fake data so the app has content.
        /// </summary>
        public static void UseSqlite()
        {
            String databasePath = ApplicationData.Current.LocalFolder.Path + @"\Pomoductive.db";

            var dbOptions = new DbContextOptionsBuilder<PomoductiveContext>().UseSqlite(
                "Data Source=" + databasePath);
            Repository = new SqlPomoductiveRepository(dbOptions);

        }



    }
}
