using GetSocialSdk.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
    public class NotificationsBadgeManager : MonoBehaviour
    {
        //---------------------------------------------------------------------
        // Editor
        //---------------------------------------------------------------------
        
        [SerializeField] private Text _countText;
        [SerializeField] private Image _unreadBackgroundImage;
        
        //---------------------------------------------------------------------
        // Events
        //---------------------------------------------------------------------

        public void OnSignOutRequest()
        {
            ShowCount(false);
        }
        
        //---------------------------------------------------------------------
        // Messages
        //---------------------------------------------------------------------

        private void OnEnable()
        {
            UpdateNotificationsCount();
        }
        
        //---------------------------------------------------------------------
        // Public
        //---------------------------------------------------------------------
       
        public void OnSignInFinished()
        {
            UpdateNotificationsCount();
        }
        
        //---------------------------------------------------------------------
        // Internal
        //---------------------------------------------------------------------

        private void UpdateNotificationsCount()
        {
            ShowCount(false);

            var notificationsCountQuery = NotificationsCountQuery.Unread().OfTypes(
                Notification.NotificationTypes.Comment,
                Notification.NotificationTypes.Direct,
                Notification.NotificationTypes.MentionInActivity,
                Notification.NotificationTypes.MentionInComment,
                Notification.NotificationTypes.ReplyToComment,
                Notification.NotificationTypes.Targeting);
            
            Debug.Log("Username: " + GetSocial.User.DisplayName);
            GetSocial.User.GetNotificationsCount(
                query: notificationsCountQuery,
                onSuccess: count =>
                {
                    _countText.text = count > 99 ? "99+" : count.ToString();
                    Debug.Log("Notifications count: " + count);
                    ShowCount(count > 0);
                },
                onError: error =>
                {
                    Debug.LogErrorFormat("Failed to retrieve notifications count, error: {0}", error.Message);
                });
        }

        private void ShowCount(bool value)
        {
            _unreadBackgroundImage.gameObject.SetActive(value);
        }
    }
}