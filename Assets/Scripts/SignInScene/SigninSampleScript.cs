﻿// <copyright file="SigninSampleScript.cs" company="Google Inc.">
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

namespace ua.org.gdg.devfest
{
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using Google;
  using UnityEngine;
  using UnityEngine.UI;
  using Firebase.Auth;

  public class SigninSampleScript : MonoBehaviour
  {
    public Text statusText;

    public string webClientId = "634686754515-vtkaddac36pof0anm089grndrqckh4q2.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;
    private FirebaseAuth _auth;
    private FirebaseUser _user;

    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
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

      AddStatusText("Calling SignIn");
      GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
        OnAuthenticationFinished);
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
            signInCompleted.SetResult(authTask.Result);
            _user = authTask.Result;
            AddStatusText("Firebase user: " + authTask.Result.DisplayName);
           
            
            SceneManager.LoadScene("RestInvestigationScene");
          }
        });
      }
    }

    public void OnSignInSilently()
    {
      GoogleSignIn.Configuration = configuration;
      GoogleSignIn.Configuration.UseGameSignIn = false;
      GoogleSignIn.Configuration.RequestIdToken = true;
      AddStatusText("Calling SignIn Silently");

      GoogleSignIn.DefaultInstance.SignInSilently()
        .ContinueWith(OnAuthenticationFinished);
    }


    public void OnGamesSignIn()
    {
      GoogleSignIn.Configuration = configuration;
      GoogleSignIn.Configuration.UseGameSignIn = true;
      GoogleSignIn.Configuration.RequestIdToken = false;

      AddStatusText("Calling Games SignIn");

      GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
        OnAuthenticationFinished);
    }

    private List<string> messages = new List<string>();

    void AddStatusText(string text)
    {
      if (messages.Count == 5)
      {
        messages.RemoveAt(0);
      }

      messages.Add(text);
      string txt = "";
      foreach (string s in messages)
      {
        txt += "\n" + s;
      }

      statusText.text = txt;
    }
  }
}