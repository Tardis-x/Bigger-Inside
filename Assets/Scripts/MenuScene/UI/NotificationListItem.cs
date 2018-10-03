using System.Linq;
using GetSocialSdk.Core;
using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
    public class NotificationListItem : MonoBehaviour
    {
        [SerializeField] private Text _time;
        [SerializeField] private Text _text;
        [SerializeField] private Image _readMarker;

        private string _id;

        public void Init(Notification notification)
        {
            _id = notification.Id;
            
            var displayText = string.IsNullOrEmpty(notification.Title) ? string.Empty : (notification.Title + "\n");
            displayText += notification.Text;
            
            _text.text = displayText;
            _time.text = TimeUtils.GetPrettyDate(TimeUtils.UnixTimeStampToDateTime(notification.CreatedAt));

            float colorA = notification.WasRead ? 0.5f : 1f;
            _readMarker.color = new Color(_readMarker.color.r, _readMarker.color.g, _readMarker.color.b, colorA);
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, colorA);
            _time.color = new Color(_time.color.r, _time.color.g, _time.color.b, colorA);
        }

        public void MarkNotificationAsRead()
        {
            float colorA = 0.5f;
            _readMarker.color = new Color(_readMarker.color.r, _readMarker.color.g, _readMarker.color.b, colorA);
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, colorA);
            _time.color = new Color(_time.color.r, _time.color.g, _time.color.b, colorA);
            
            GetSocial.User.SetNotificationsRead(new string[]{_id}.ToList(), true, () =>
            {
                Debug.Log("Marked notification as read");
            }, error =>
            {
                Debug.LogError("Failed to mark notification as read, error: " + error.Message);
            });
        }
    }
}