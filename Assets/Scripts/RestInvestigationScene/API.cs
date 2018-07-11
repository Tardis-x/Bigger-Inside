using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Newtonsoft.Json;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Networking.NetworkSystem;

namespace ua.org.gdg.devfest
{
  public class API : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private ScrollableListScript _listScript;
    [SerializeField] private SpeechScript _speechScript;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      WWW scheduleRequest = new WWW(SCHEDULE_URL);
      WWW sessionRequest = new WWW(SESSIONS_URL);

      StartCoroutine(OnScheduleResponse(scheduleRequest));
      StartCoroutine(OnSessionResponse(sessionRequest));
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void OnClick()
    {
      Request(Hall.Expo, 0);
      _listScript.Show();
    }

    public void Request(Hall hall, int day)
    {
      _listScript.AddContent(FirestoreHelper.ComposeScheduleForHall(hall, day, _schedule, _items, _speechScript));
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private const string SCHEDULE_URL =
      "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/schedule";

    private const string SESSIONS_URL =
      "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/sessions?pageSize=40";
    
    private const string SPEAKERS_URL =
      "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/speakers";

    private Schedule _schedule;
    private List<SessionItem> _items;

    private IEnumerator OnScheduleResponse(WWW req)
    {
      yield return req;

      JsonSchedule jSchedule = JsonConvert.DeserializeObject<JsonSchedule>(req.text);
      _schedule = FirestoreHelper.ParseSchedule(jSchedule);
    }

    private IEnumerator OnSessionResponse(WWW req)
    {
      yield return req;

      SessionTable st = JsonConvert.DeserializeObject<SessionTable>(req.text);
      _items = FirestoreHelper.ParseSessions(st);
    }

    private List<string> messages = new List<string>();

    private void AddStatusText(string text)
    {
      messages.Add(text);
      string txt = "";

      foreach (string s in messages)
      {
        txt += "\n" + s;
      }
    }
  }
}