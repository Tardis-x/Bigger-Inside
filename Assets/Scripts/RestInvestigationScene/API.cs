using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Newtonsoft.Json;
using UnityEngine.Experimental.PlayerLoop;

namespace ua.org.gdg.devfest
{
  public class API : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private Text _responseText;
    [SerializeField] private ScrollableListScript _listScript;
    [SerializeField] private ShowScript _showScript;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------
    
    public void Request()
    {
      WWW scheduleRequest = new WWW(SCHEDULE_URL);
      WWW sessionRequest = new WWW(SESSIONS_URL);
      
      WaitForSeconds w;
      while (!scheduleRequest.isDone || !sessionRequest.isDone)
        w = new WaitForSeconds(0.1f);
      
      JsonSchedule jSchedule = JsonConvert.DeserializeObject<JsonSchedule>(scheduleRequest.text);
      Schedule sch = FirestoreHelper.ParseSchedule(jSchedule);
      SessionTable st = JsonConvert.DeserializeObject<SessionTable>(sessionRequest.text);
      Dictionary<int, string> items = FirestoreHelper.ParseSessions(st);
      
      _listScript.AddContent(FirestoreHelper.ComposeScheduleForHall(Hall.Conference, 0, sch, items, _showScript));
      _listScript.Show();
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private const string SCHEDULE_URL =
      "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/schedule";
    
    private const string SESSIONS_URL = 
      "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/sessions?pageSize=40";
    
    private IEnumerator OnResponse(WWW req)
    {
      yield return req;

      JsonSchedule jSchedule = JsonConvert.DeserializeObject<JsonSchedule>(req.text);
      Schedule sch = FirestoreHelper.ParseSchedule(jSchedule);
      
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

      _responseText.text = txt;
    }
  }
}