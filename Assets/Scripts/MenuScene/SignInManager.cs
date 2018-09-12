using System;
using System.Linq;
using System.Security.Cryptography;
using Facebook.Unity;
using System.Threading.Tasks;
using Google;
using UnityEngine;
using Firebase.Auth;
using GetSocialSdk.Core;

namespace ua.org.gdg.devfest
{
  public class SignInManager : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Events")] [SerializeField] private GameEvent _showMenu;
    [SerializeField] private GameEvent _signIn;
    [SerializeField] private GameEvent _signInFinished;

    [Header("SignIn")] [SerializeField] private string _webClientId;

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
      
      SetGetSocialNameAndAvatar();
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
      FB.LogInWithReadPermissions(callback: OnFacebookLogin);
    }

    public void Logout()
    {
      if (_auth.CurrentUser == null) return;

      GetSocial.User.Reset(
        () => { Debug.Log("AUTH RESET SUCCESSFULL"); },
        error => { Debug.Log("AUTH RESET ERROR: " + error.Message);});

      _auth.SignOut();

      if (FB.IsLoggedIn) FB.LogOut();
      else GoogleSignIn.DefaultInstance.SignOut();
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void ConfigureGoogleSignIn()
    {
      if (GoogleSignIn.Configuration != null) return;

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

        _signInSpinner = new ProgressDialogSpinner("Please wait", "Signing in...");
        _signInSpinner.Show();

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
      _signInSpinner.Dismiss();

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
        GetSocialSignIn();
      }

      SignInFinished();
    }

    private void OnGoogleAuthenticationFinished(Task<GoogleSignInUser> task)
    {
      if (task.IsFaulted)
      {
        Utils.ShowMessage("Google plus login failed");
      }
      else if (task.IsCanceled)
      {
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
        GetSocialSignIn();
      }

      SignInFinished();
    }

    private void GetSocialSignIn()
    {
      GetSocial.WhenInitialized(OnGetSocialInitialized);
    }

    private void OnGetSocialInitialized()
    {
      var user = FirebaseAuth.DefaultInstance.CurrentUser;
      
      HMACSHA256 cypher = new HMACSHA256(System.Text.Encoding.Default.GetBytes("SECURITY"));
      string token = System.Text.Encoding.Default.GetString(cypher.ComputeHash(Convert.FromBase64String(user.UserId)));
      
      var authIdentity = AuthIdentity.CreateCustomIdentity(user.ProviderId, user.UserId, token);

      GetSocial.User.AddAuthIdentity(authIdentity, SetGetSocialNameAndAvatar,
        error => { Debug.Log("AUTH ADD ERROR: " + error.Message); },
        conflict =>
        {
          Debug.Log("CONFLICT");
          
          GetSocial.User.SwitchUser(authIdentity, () =>
          {
            Debug.Log("SWITCH SUCCESS");
          }, error =>
          {
            Debug.Log("SWTRCH ERROR: " + error.Message);
          });
        });
      
      Debug.Log("PROVIDER ID: " + user.ProviderId);
      Debug.Log("USER ID: " + user.UserId);
      Debug.Log("GET SOCIAL USER ID: " + GetSocial.User.Id);
      Debug.Log("TOKEN: " + token);
      SetGetSocialNameAndAvatar();
    }

    private void OnUserTokenFetched(Task<string> task)
    {
      if (task.IsCanceled)
      {
        Utils.ShowMessage("TokenAsync was canceled.");
        return;
      }

      if (task.IsFaulted)
      {
        Utils.ShowMessage("TokenAsync encountered an error: " + task.Exception);
        return;
      }

      string idToken = task.Result;
      var user = FirebaseAuth.DefaultInstance.CurrentUser;
      var authIdentity = AuthIdentity.CreateCustomIdentity(user.ProviderId, user.UserId, idToken);

      GetSocial.User.AddAuthIdentity(authIdentity, SetGetSocialNameAndAvatar,
        error => { Debug.Log("AUTH ADD ERROR: " + error.Message); },
        conflict => { Debug.Log("CONFLICT"); });
      
      Debug.Log("PROVIDER ID: " + user.ProviderId);
      Debug.Log("USER ID: " + user.UserId);
      Debug.Log("GET SOCIAL USER ID: " + GetSocial.User.Id);
      Debug.Log("TOKEN: " + idToken);
      SetGetSocialNameAndAvatar();
    }

    private void SetGetSocialNameAndAvatar()
    {
      var user = FirebaseAuth.DefaultInstance.CurrentUser;
      SetGetSocialUsername(user.DisplayName);
      SetGetSocialAvatar(GetHigherResProfilePic(user.PhotoUrl.OriginalString));
    }

    private void SetGetSocialUsername(string name)
    {
      GetSocial.User.SetDisplayName(name,
        () => { Utils.ShowMessage("Name successfully changed: " + GetSocial.User.DisplayName); },
        error => { Utils.ShowMessage("Name change failed, error: " + error); });
    }

    private void SetGetSocialAvatar(string url)
    {
      GetSocial.User.SetAvatarUrl(url,
        () => { Utils.ShowMessage("Avatar successfully changed: " + GetSocial.User.AvatarUrl); },
        error => { Utils.ShowMessage("Avatar update failed, error: " + error); });
    }

    private void SignInFinished()
    {
      _signInFinished.Raise();
      _showMenu.Raise();
    }

    private string GetHigherResProfilePic(string photoUrl)
    {
      string result = photoUrl;

      if (photoUrl.Contains("s96-c"))
      {
        result = photoUrl.Replace("s96-c", "s400-c");
      }
      else
      {
        result = photoUrl + "?type=large";
      }

      return result;
    }
  }
}