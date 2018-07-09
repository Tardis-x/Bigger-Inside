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

    public void Request()
    {
      string scheduleUrl =
        "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/schedule";

      WWW scheduleRequest = new WWW(scheduleUrl);
      StartCoroutine(OnResponse(scheduleRequest));
    }

    private IEnumerator OnResponse(WWW req)
    {
      yield return req;

      JsonSchedule jSchedule = JsonConvert.DeserializeObject<JsonSchedule>(req.text);
      Schedule sch = FirestoreHelper.ParseSchedule(jSchedule);
      var expoItems = sch.Days[0].Timeslots.Select(x => x.Sessions.Where(s => s.Hall == Hall.Expo));
      
      foreach (var sessions in expoItems)
      {
        foreach (var s in sessions)
        {
          RequestItemName(s.Items[0]);
        }
      }
      
      //RequestItemName(sch.Days[0].Timeslots[7].Sessions[0].Items[0]);
      // AddStatusText("S");
    } 
    
    private void RequestItemName(int item)
         {
           string sessionUrl =
             "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/sessions/"
             + item;
           WWW sessionRequest = new WWW(sessionUrl);
           StartCoroutine(OnSessionResponse(sessionRequest));
         }
    
    private IEnumerator OnSessionResponse(WWW req)
    {
      yield return req;

      string item = JsonConvert.DeserializeObject<JsonSession>(req.text).fields.title.stringValue;
      AddStatusText(item);
    }

    private List<string> messages = new List<string>();

    private void AddStatusText(string text)
    {
//      if (messages.Count == 5)
//      {
//        messages.RemoveAt(0);
//      }

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