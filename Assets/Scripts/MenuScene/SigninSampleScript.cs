// <copyright file="SigninSampleScript.cs" company="Google Inc.">
// Copyright (C) 2017 Google Inc. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations

using Facebook.Unity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

namespace ua.org.gdg.devfest
{
  public class SigninSampleScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private GameEvent _showMenu;
    [SerializeField] private GameEvent _signedIn;
    
    public Text statusText;
    public string webClientId;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
    
    private GoogleSignInConfiguration configuration;
    private FirebaseAuth _auth;
    private List<string> _messages = new List<string>();


    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
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
      
      GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
        OnGoogleAuthenticationFinished);
    }

    public void OnFacebookSignIn()
    {
      AddStatusText("Calling FB SignIn");
      FB.LogInWithReadPermissions(callback:OnFacebookLogin);
    }
    
    public void OnSignOut()
    {
      AddStatusText("Calling SignOut");
      GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
      AddStatusText("Calling Disconnect");
      GoogleSignIn.DefaultInstance.Disconnect();
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void ConfigureGoogleSignIn()
    {
      GoogleSignIn.Configuration = new GoogleSignInConfiguration
      {
        WebClientId = webClientId,
        RequestIdToken = true,
        UseGameSignIn = false
      };
    }
    
    private void OnFacebookLogin(ILoginResult result)
    {
      if (FB.IsLoggedIn)
      {
        AddStatusText("FB is LoggedIn");
        var token = AccessToken.CurrentAccessToken.TokenString;
        var credential = FacebookAuthProvider.GetCredential(token);
        
        AddStatusText("Calling Firebase LogIn");
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
        AddStatusText("Canceled");
      }
      else if (task.IsFaulted)
      {
        signInCompleted.SetException(task.Exception);
        
        Utils.ShowMessage("Firebase login failed");
      }
      else
      {
        signInCompleted.SetResult(task.Result);
        AddStatusText("Welcome: " + task.Result.DisplayName + "!");
        _signedIn.Raise();
        _showMenu.Raise();
      }
    }

    private void OnGoogleAuthenticationFinished(Task<GoogleSignInUser> task)
    {
      if (task.IsFaulted)
      {
        Utils.ShowMessage("Google plus login failed");
      }
      else if (task.IsCanceled)
      {
        AddStatusText("Canceled");
      }
      else
      {
        AddStatusText("Welcome: " + task.Result.DisplayName + "!");
        
        var signInCompleted = new TaskCompletionSource<FirebaseUser>();

        var credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
        _auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
        {
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
            AddStatusText("Firebase user: " + authTask.Result.DisplayName);
            _signedIn.Raise();
           _showMenu.Raise();
          }
        });
      }
    }

    private void AddStatusText(string text)
    {
      if (_messages.Count == 5)
      {
        _messages.RemoveAt(0);
      }

      _messages.Add(text);
      var txt = "";
      foreach (string s in _messages)
      {
        txt += "\n" + s;
      }

      statusText.text = txt;
    }
  }
}