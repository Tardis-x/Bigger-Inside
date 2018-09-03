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
using UnityEngine.SceneManagement;
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
    public Text statusText;
    public string webClientId = "634686754515-vtkaddac36pof0anm089grndrqckh4q2.apps.googleusercontent.com";

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
      configuration = new GoogleSignInConfiguration
      {
        WebClientId = webClientId,
        RequestIdToken = true
      };

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
    
    public void OnSignIn()
    {
      GoogleSignIn.Configuration = configuration;
      GoogleSignIn.Configuration.UseGameSignIn = false;
      GoogleSignIn.Configuration.RequestIdToken = true;

      AddStatusText("Calling SignIn");
      GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
        OnAuthenticationFinished);
    }

    public void OnFacebookSignIn()
    {
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
    
    private void OnFacebookLogin(ILoginResult result)
    {
      Debug.Log("FB LOGIN: " + FB.IsLoggedIn);
      if (FB.IsLoggedIn)
      {
        var token = AccessToken.CurrentAccessToken.TokenString;
        var credential = FacebookAuthProvider.GetCredential(token);
        
        _auth.SignInWithCredentialAsync(credential).ContinueWith(OnFacebookAuthenticationFinished);
      }
      else
      {
        AddStatusText("Facebook login failed");
      }
    }

    private void OnFacebookAuthenticationFinished(Task<FirebaseUser> task)
    {
      var signInCompleted = new TaskCompletionSource<FirebaseUser>();
      Debug.Log("FB FIREBASE CALLBACK");
      
      if (task.IsCanceled)
      {
        Debug.Log("TASK IS CANCELED");
        signInCompleted.SetCanceled();
      }
      else if (task.IsFaulted)
      {
        Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
        signInCompleted.SetException(task.Exception);
      }
      else
      {
        Debug.Log("TASK IS SUCCESSFUL");
        Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        signInCompleted.SetResult(task.Result);
        AddStatusText("Firebase user: " + task.Result.DisplayName);
            
        SceneManager.LoadScene("MenuScene");
      }
    }

    private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
      if (task.IsFaulted)
      {
        using (IEnumerator<System.Exception> enumerator =
          task.Exception.InnerExceptions.GetEnumerator())
        {
          if (enumerator.MoveNext())
          {
            GoogleSignIn.SignInException error =
              (GoogleSignIn.SignInException) enumerator.Current;
            AddStatusText("Got Error: " + error.Status + " " + error.Message);
          }
          else
          {
            AddStatusText("Got Unexpected Exception?!?" + task.Exception);
          }
        }
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
            signInCompleted.SetException(authTask.Exception);
          }
          else
          {
            Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
            signInCompleted.SetResult(authTask.Result);
            AddStatusText("Firebase user: " + authTask.Result.DisplayName);
            
            SceneManager.LoadScene("MenuScene");
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