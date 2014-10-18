﻿using JolTudomE_WP.Common;
using JolTudomE_WP.Model;
using JolTudomE_WP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace JolTudomE_WP.View {
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page {
    private NavigationHelper navigationHelper;

    public MainPage() {
      this.InitializeComponent();

      this.navigationHelper = new NavigationHelper(this);
      this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
      this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
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

      // take out LoginPage from Backstack list
      var login = Frame.BackStack.FirstOrDefault(b => b.SourcePageType.Name == "LoginPage");
      if (login != null) {
        Frame.BackStack.Remove(login);
      }

      ((IViewModel)this.DataContext).LoadData(DataSource.LoggedInInfo.PersonID);

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

    private void lvStatistic_ItemClick(object sender, ItemClickEventArgs e) {
      int testid = ((JolTudomE_WP.Model.Statistic)e.ClickedItem).TestID;
      Frame.Navigate(typeof(TestDetailPage), testid);
    }

    private void cmdStartTest_Click(object sender, RoutedEventArgs e) {
      MainViewModel vm = (MainViewModel)this.DataContext;
      if (vm.SelectedTopics.Count > 0) {
        Frame.Navigate(typeof(TestExecutePage), new NewTestParam { NumberOfQuestions = vm.NumberQuestion, TopicIDs = vm.SelectedTopics });
      }
      else {
        vm.IsTopicErrorShown = true;
      }
    }

    private void lvTopic_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      MainViewModel vm = (MainViewModel)this.DataContext;
      vm.SelectedTopics = new List<int>();
      foreach(Topic t in lvTopic.SelectedItems) {
        vm.SelectedTopics.Add(t.TopicID);
      }
      vm.IsTopicErrorShown = vm.SelectedTopics.Count == 0;
    }


  }
}
