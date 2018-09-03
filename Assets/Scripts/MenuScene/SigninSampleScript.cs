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
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private RawImage _userAvatar;
    [SerializeField] private ImageLoader _loader;

    public string webClientId = "634686754515-vtkaddac36pof0anm089grndrqckh4q2.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;
    private FirebaseAuth _auth;

    void Awake()
    {
      configuration = new GoogleSignInConfiguration
      {
        WebClientId = webClientId,
        RequestIdToken = true
      };

      _auth = FirebaseAuth.DefaultInstance;
    }

    public void OnSignIn()
    {
      GoogleSignIn.Configuration = configuration;
      GoogleSignIn.Configuration.UseGameSignIn = false;
      GoogleSignIn.Configuration.RequestIdToken = true;

      GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
        OnAuthenticationFinished);
    }


    public void OnSignOut()
    {
      GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
      GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
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
          }
        }
      }
      else
      {

        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();

        Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
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
           _loader.LoadImage(authTask.Result.PhotoUrl.AbsolutePath, _userAvatar);
            SceneManager.LoadScene("MenuScene");
          }
        });
      }
    }

    private List<string> messages = new List<string>();
  }
}