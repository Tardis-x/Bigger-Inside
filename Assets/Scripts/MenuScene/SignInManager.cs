using Facebook.Unity;
using System.Threading.Tasks;
using Google;
using UnityEngine;
using Firebase.Auth;

namespace ua.org.gdg.devfest
{
  public class SignInManager : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Events")]
    [SerializeField] private GameEvent _showMenu;
    [SerializeField] private GameEvent _signIn;
    [SerializeField] private GameEvent _signInFinished;
    
    [Header("SignIn")]
    [SerializeField] private string _webClientId;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
    
    private GoogleSignInConfiguration configuration;
    private FirebaseAuth _auth;
    private ProgressDialogSpinner _signInSpinner;
    
    //---------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------
    
    public bool UserSignedIn
    {
      get { return FirebaseAuth.DefaultInstance.CurrentUser != null; }
    }

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------
    
    private void Awake()
    {     
      ConfigureGoogleSignIn();

      _auth = FirebaseAuth.DefaultInstance;

      if (!FB.IsInitialized)
      {
        FB.Init();
      }
      else
      {
        FB.ActivateApp();
      }
    }
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------
    
    public void OnGoogleSignIn()
    {
      _signIn.Raise();
      
      GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticationFinished);
    }

    public void OnFacebookSignIn()
    {
      _signIn.Raise();
      FB.LogInWithReadPermissions(callback:OnFacebookLogin);
    }
    
    public void Logout()
    {
      if (FirebaseAuth.DefaultInstance.CurrentUser == null) return;
      
      FirebaseAuth.DefaultInstance.SignOut();
      
      if(FB.IsLoggedIn) FB.LogOut();
      else GoogleSignIn.DefaultInstance.SignOut();
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void ConfigureGoogleSignIn()
    {
      GoogleSignIn.Configuration = new GoogleSignInConfiguration
      {
        WebClientId = _webClientId,
        RequestIdToken = true,
        UseGameSignIn = false
      };
    }
    
    private void OnFacebookLogin(ILoginResult result)
    {
      if (FB.IsLoggedIn)
      {
        var token = AccessToken.CurrentAccessToken.TokenString;
        var credential = FacebookAuthProvider.GetCredential(token);
        _auth.SignInWithCredentialAsync(credential).ContinueWith(OnFacebookAuthenticationFinished);
      }
      else
      {
        Utils.ShowMessage("Facebook login failed");
      }
    }

    private void OnFacebookAuthenticationFinished(Task<FirebaseUser> task)
    {
      var signInCompleted = new TaskCompletionSource<FirebaseUser>();
      
      if (task.IsCanceled)
      {
        signInCompleted.SetCanceled();
      }
      else if (task.IsFaulted)
      {
        signInCompleted.SetException(task.Exception);
        Utils.ShowMessage("Firebase login failed");
      }
      else
      {
        signInCompleted.SetResult(task.Result);
      }
      
      SignInFinished();
    }

    private void OnGoogleAuthenticationFinished(Task<GoogleSignInUser> task)
    {
      if (task.IsFaulted)
      {
        Utils.ShowMessage("Google plus login failed");
        SignInFinished();
      }
      else if (task.IsCanceled)
      {
        SignInFinished();
      }
      else
      {
        var credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
        
        _signInSpinner = new ProgressDialogSpinner("Please wait", "Signing in...");
        _signInSpinner.Show();
        
        _auth.SignInWithCredentialAsync(credential).ContinueWith(OnSignInWithCredentialsFinished);
      }
    }

    private void OnSignInWithCredentialsFinished(Task<FirebaseUser> authTask)
    {
      var signInCompleted = new TaskCompletionSource<FirebaseUser>();

      _signInSpinner.Dismiss();
      SignInFinished();

      if (authTask.IsCanceled)
      {
        signInCompleted.SetCanceled();
      }
      else if (authTask.IsFaulted)
      {
        Utils.ShowMessage("Firebase login failed");
        signInCompleted.SetException(authTask.Exception);
      }
      else
      {
        signInCompleted.SetResult(authTask.Result);
      }
    }

    private void SignInFinished()
    {
      _signInFinished.Raise();
      _showMenu.Raise();
    }
  }
}