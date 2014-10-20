﻿using JolTudomE_WP.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace JolTudomE_WP.View {
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class LoginPage : Page {
    private NavigationHelper navigationHelper;

    public LoginPage() {
      this.InitializeComponent();

      this.navigationHelper = new NavigationHelper(this);
      this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
      this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

      txtUserName.Text = "admin2";
      txtPassword.Password = "12345678";

    }

    /// <summary>
    /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
    /// </summary>
    public NavigationHelper NavigationHelper {
      get { return this.navigationHelper; }
    }

    /// <summary>
    /// Populates the page with content passed during navigation.  Any saved state is also
    /// provided when recreating a page from a prior session.
    /// </summary>
    /// <param name="sender">
    /// The source of the event; typically <see cref="NavigationHelper"/>
    /// </param>
    /// <param name="e">Event data that provides both the navigation parameter passed to
    /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
    /// a dictionary of state preserved by this page during an earlier
    /// session.  The state will be null the first time a page is visited.</param>
    private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e) {
      // take out Register from Backstack list
      var register = Frame.BackStack.FirstOrDefault(b => b.SourcePageType.Name == "Register");
      if (register != null) {
        Frame.BackStack.Remove(register);
      }

    }

    /// <summary>
    /// Preserves state associated with this page in case the application is suspended or the
    /// page is discarded from the navigation cache.  Values must conform to the serialization
    /// requirements of <see cref="SuspensionManager.SessionState"/>.
    /// </summary>
    /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
    /// <param name="e">Event data that provides an empty dictionary to be populated with
    /// serializable state.</param>
    private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e) {
    }

    #region NavigationHelper registration

    /// <summary>
    /// The methods provided in this section are simply used to allow
    /// NavigationHelper to respond to the page's navigation methods.
    /// <para>
    /// Page specific logic should be placed in event handlers for the  
    /// <see cref="NavigationHelper.LoadState"/>
    /// and <see cref="NavigationHelper.SaveState"/>.
    /// The navigation parameter is available in the LoadState method 
    /// in addition to page state preserved during an earlier session.
    /// </para>
    /// </summary>
    /// <param name="e">Provides data for navigation methods and event
    /// handlers that cannot cancel the navigation request.</param>
    protected override void OnNavigatedTo(NavigationEventArgs e) {
      this.navigationHelper.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e) {
      this.navigationHelper.OnNavigatedFrom(e);
    }

    #endregion

    private async void cmdLogin_Click(object sender, RoutedEventArgs e) {
      prgBar.Visibility = Windows.UI.Xaml.Visibility.Visible;
      try {
        bool loginres = await DataSource.MakeLogin(txtUserName.Text, txtPassword.Password);
        if (DataSource.LoggedInInfo.RoleID == DataSource.GetRoleStudent().RoleID) {
          DataSource.SelectedUserInfo = DataSource.LoggedInInfo;
          Frame.Navigate(typeof(SelectedUserPage));
        }
        else {
          Frame.Navigate(typeof(LoggedInUserPage));
        }
      }
      catch (UnauthorizedException) {
        prgBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        ShowLoginError("Rossz Felhasználó név vagy Jelszó!");
      }
      catch (Exception exc) {
        prgBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        ShowLoginError(exc.Message);
      }
    }

    private async void ShowLoginError(string msg) {
      ContentDialog errordialog = new ContentDialog() {
        Title = "Bejelentkezési Hiba",
        Content = msg,
        PrimaryButtonText = "Ok"
      };

      await errordialog.ShowAsync();
    }

    private void cmdRegister_Click(object sender, RoutedEventArgs e) {
      Frame.Navigate(typeof(RegisterPage));
    }

  }
}
