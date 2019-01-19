//  ---------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//  The MIT License (MIT)
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  ---------------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Pomoductive.ViewModels;
using Pomoductive.Views;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Pomoductive
{
    /// <summary>
    /// The "chrome" layer of the app that provides top-level navigation with
    /// proper keyboarding navigation.
    /// </summary>
    public sealed partial class AppShell : Page
    {
        /// <summary>
        /// Initializes a new instance of the AppShell, sets the static 'Current' reference,
        /// adds callbacks for Back requests and changes in the SplitView's DisplayMode, and
        /// provide the nav menu list with the data to display.
        /// </summary>
        public AppShell()
        {
            InitializeComponent();

            AppFrame.Navigated += NavigationService_Navigated;
            PopulateNavItems();

            //SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;

            // Hide default title bar.
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            UpdateTitleBarLayout(coreTitleBar);
            // Set AppTitleBar element as a draggable region.
            Window.Current.SetTitleBar(AppTitleBar);

            // Register a handler for when the size of the overlaid caption control changes.
            // TODO: For example, when the app moves to a screen with a different DPI.
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
        }

        /// <summary>
        /// Gets the navigation frame instance.
        /// </summary>
        public Frame AppFrame => frame;
        private ObservableCollection<ShellNavigationItem> _navigationItems = App.AppViewModel.NavigationItems;


        /// <summary>
        /// Default keyboard focus movement for any unhandled keyboarding
        /// </summary>
        private void AppShell_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            FocusNavigationDirection direction = FocusNavigationDirection.None;
            switch (e.Key)
            {
                case VirtualKey.Left:
                case VirtualKey.GamepadDPadLeft:
                case VirtualKey.GamepadLeftThumbstickLeft:
                case VirtualKey.NavigationLeft:
                    direction = FocusNavigationDirection.Left;
                    break;
                case VirtualKey.Right:
                case VirtualKey.GamepadDPadRight:
                case VirtualKey.GamepadLeftThumbstickRight:
                case VirtualKey.NavigationRight:
                    direction = FocusNavigationDirection.Right;
                    break;

                case VirtualKey.Up:
                case VirtualKey.GamepadDPadUp:
                case VirtualKey.GamepadLeftThumbstickUp:
                case VirtualKey.NavigationUp:
                    direction = FocusNavigationDirection.Up;
                    break;

                case VirtualKey.Down:
                case VirtualKey.GamepadDPadDown:
                case VirtualKey.GamepadLeftThumbstickDown:
                case VirtualKey.NavigationDown:
                    direction = FocusNavigationDirection.Down;
                    break;
            }

            if (direction != FocusNavigationDirection.None &&
                FocusManager.FindNextFocusableElement(direction) is Control control)
            {
                control.Focus(FocusState.Keyboard);
                e.Handled = true;
            }
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            LeftPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayLeftInset);
            RightPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayRightInset);
            AppTitleBar.Height = coreTitleBar.Height;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }




        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///NAVIGATION
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void PopulateNavItems()
        {
            _navigationItems.Clear();

            _navigationItems.Add(ShellNavigationItem.FromType<MainPage>("DashBoard", Symbol.Admin));
            _navigationItems.Add(ShellNavigationItem.FromType<StatisticsPage>("Statistics", Symbol.Home));
            _navigationItems.Add(ShellNavigationItem.FromType<TodoManagementPage>("To-Do Management", Symbol.Calendar));
            _navigationItems.Add(ShellNavigationItem.FromType<JournalPage>("Journal", Symbol.People));
            _navigationItems.Add(ShellNavigationItem.FromType<ChallengePage>("Challenge", Symbol.Map));
        }

        public bool Navigate<T>(object parameter = null, NavigationTransitionInfo infoOverride = null) where T : Page => Navigate(typeof(T), parameter, infoOverride);

        public bool Navigate(Type pageType, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            // Don't open the same page multiple times
            if (AppFrame.CurrentSourcePageType != pageType)
            {
                return AppFrame.Navigate(pageType, parameter, infoOverride);
            }
            else
            {
                return false;
            }
        }

        private void NavList_Loaded(object sender, RoutedEventArgs e)
        {
            Navigate(typeof(MainPage));
        }

        private void MoveNavIndicator(int index)
        {
            NavIndicatorOffset.X = index * NavIndicator.X2;
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            NavList.Visibility = Visibility.Visible;
            NavIndicator.Visibility = Visibility.Visible;

            var spt = e.SourcePageType;

            if (typeof(Views.MainPage).Equals(spt))
            {
                MoveNavIndicator(0);
            }
            else if (typeof(Views.StatisticsPage).Equals(spt))
            {
                MoveNavIndicator(1);
            }
            else if (typeof(Views.TodoManagementPage).Equals(spt))
            {
                MoveNavIndicator(2);
            }
            else if (typeof(JournalPage).Equals(spt))
            {
                MoveNavIndicator(3);
            }
            else if (typeof(ChallengePage).Equals(spt))
            {
                MoveNavIndicator(4);
            }
            else
            {
                NavList.SelectedIndex = -1;
                NavList.Visibility = Visibility.Collapsed;
                NavIndicator.Visibility = Visibility.Collapsed;
            }
        }

        private void NavList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var navigationItem = e.ClickedItem as ShellNavigationItem;
            if (navigationItem != null)
            {
                Navigate(navigationItem.PageType);
            }
        }

        /// <summary>
        /// Navigates the frame to the previous page.
        /// </summary>
        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (AppFrame.CanGoBack)
            {
                AppFrame.GoBack();
            }
        }



    }
}
