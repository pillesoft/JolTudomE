using JolTudomE_WP.Common;
using JolTudomE_WP.View;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using System;
using Windows.Storage;
using Windows.Security.Credentials;
using System.Threading.Tasks;

// The Hub Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace JolTudomE_WP {
  
  /// <summary>
  /// Provides application-specific behavior to supplement the default Application class.
  /// </summary>
  public sealed partial class App : Application {
    private TransitionCollection transitions;
    private bool IsDialogOpen;
    private ApplicationDataContainer _LocalSettings = null;

    private const string _CredLockerResource = "JolTudomE";

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App() {
      this.InitializeComponent();
      this.Suspending += this.OnSuspending;
      this.Resuming += this.OnResuming;

      _LocalSettings = ApplicationData.Current.LocalSettings;
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used when the application is launched to open a specific file, to display
    /// search results, and so forth.
    /// </summary>
    /// <param name="e">Details about the launch request and process.</param>
    protected override async void OnLaunched(LaunchActivatedEventArgs e) {
#if DEBUG
      if (System.Diagnostics.Debugger.IsAttached) {
        this.DebugSettings.EnableFrameRateCounter = true;
      }
#endif

      Frame rootFrame = Window.Current.Content as Frame;

      // Do not repeat app initialization when the Window already has content,
      // just ensure that the window is active.
      if (rootFrame == null) {
        // Create a Frame to act as the navigation context and navigate to the first page.
        rootFrame = new Frame();

        // Associate the frame with a SuspensionManager key.
        SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

        // TODO: Change this value to a cache size that is appropriate for your application.
        rootFrame.CacheSize = 1;

        if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {

          // try to authenticate the user, who was in when the termination is occured
          PasswordCredential cred = GetCredential();
          if (cred != null) {
            cred.RetrievePassword();
            try {
              await DataSource.MakeLogin(cred.UserName, cred.Password, () => { });
            }
            catch { }
          }

          if (DataSource.IsAuthenticated) {
            // Restore the saved session state only when appropriate.
            // and if the previous login attempt succeeded
            try {
              await SuspensionManager.RestoreAsync();
            }
            catch (SuspensionManagerException) {
              // Something went wrong restoring state.
              // Assume there is no state and continue.
            }
          }
        }

        // Place the frame in the current Window.
        Window.Current.Content = rootFrame;
      }

      if (rootFrame.Content == null) {
        // Removes the turnstile navigation for startup.
        if (rootFrame.ContentTransitions != null) {
          this.transitions = new TransitionCollection();
          foreach (var c in rootFrame.ContentTransitions) {
            this.transitions.Add(c);
          }
        }

        rootFrame.ContentTransitions = null;
        rootFrame.Navigated += this.RootFrame_FirstNavigated;

        // When the navigation stack isn't restored navigate to the first page,
        // configuring the new page by passing required information as a navigation
        // parameter.

        Authenticate();

      }

      // Ensure the current window is active.
      Window.Current.Activate();
    }

    private async void Authenticate() {
      PasswordCredential cred = GetCredential();
      if (cred == null) {
        NavigationService.NavigateTo(PageEnum.Login);
      }
      else {
        cred.RetrievePassword();
        try {
          await DataSource.MakeLogin(cred.UserName, cred.Password,
            () => {
              if (DataSource.LoggedInInfo.PersonID == DataSource.SelectedUserInfo.PersonID) {
                NavigationService.NavigateTo(PageEnum.SelectedUser);
              }
              else {
                NavigationService.NavigateTo(PageEnum.LoggedInUser);
              }
            });

        }
        catch (UnauthorizedException) {
          ShowDialog("Bejelentkezési Hiba", "A letárolt Felhasználó név/Jelszó már nem megfelelő!");
          NavigationService.NavigateTo(PageEnum.Login);
        }
        catch (Exception exc) {
          ShowDialog("Bejelentkezési Hiba", exc.Message);
          NavigationService.NavigateTo(PageEnum.Login);
        }

      }

    }

    /// <summary>
    /// Restores the content transitions after the app has launched.
    /// </summary>
    private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e) {
      var rootFrame = sender as Frame;
      rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
      rootFrame.Navigated -= this.RootFrame_FirstNavigated;
    }

    /// <summary>
    /// Invoked when application execution is being suspended.  Application state is saved
    /// without knowing whether the application will be terminated or resumed with the contents
    /// of memory still intact.
    /// </summary>
    /// <param name="sender">The source of the suspend request.</param>
    /// <param name="e">Details about the suspend request.</param>
    private async void OnSuspending(object sender, SuspendingEventArgs e) {
      var deferral = e.SuspendingOperation.GetDeferral();



      await SuspensionManager.SaveAsync();
      deferral.Complete();
    }

    void OnResuming(object sender, object e) {
      //var deferral = e.SuspendingOperation.GetDeferral();
      //await SuspensionManager.RestoreAsync();
      //deferral.Complete();
    }

    public async void SessionExpired() {
      if (!IsDialogOpen) {
        IsDialogOpen = true;

        ContentDialog dialog = new ContentDialog() {
          Title = "Lejárt a Session!",
          Content = "Az éppen aktuális munkamenet lejárt!\nÚjra be kell jelentkezni ...",
          PrimaryButtonText = "Ok",
        };

        await dialog.ShowAsync();
        Frame rootFrame = Window.Current.Content as Frame;
        rootFrame.BackStack.Clear();
        IsDialogOpen = false;

        Authenticate();
      }
    }

    public async void ShowDialog(string title, string msg) {
      TextBlock msgbox = new TextBlock();
      msgbox.Text = msg;
      msgbox.TextWrapping = TextWrapping.WrapWholeWords;

      ContentDialog errordialog = new ContentDialog() {
        Title = title,
        Content = msgbox,
        PrimaryButtonText = "Ok"
      };

      await errordialog.ShowAsync();
    }

    public void SaveCredential(string username, string password) {
      PasswordVault vault = new PasswordVault();
      
      // remove the already saved credentials
      var credcoll = vault.FindAllByResource(_CredLockerResource);
      foreach (var item in credcoll) {
        vault.Remove(item);
      }

      // save the new credential
      PasswordCredential cred = new PasswordCredential(_CredLockerResource, username, password);
      vault.Add(cred);
    }

    private PasswordCredential GetCredential() {
      PasswordVault vault = new PasswordVault();
      PasswordCredential cred = null;
      try {
        var credcoll = vault.FindAllByResource(_CredLockerResource);
        cred = credcoll[0];
      }
      catch (Exception) { }
      return cred;
    }
  }
}
