using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.Core;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ua.org.gdg.devfest
{
    public class NotificationsManager : MonoBehaviour
    {
        private const string ClearedNotifications = "cleared_notifications";
        
        [SerializeField] private GameObject _listItemPrefab;
        [SerializeField] private GameObject _notificationsList;
        [SerializeField] private GameObject _emptyPlaceholder;
        [SerializeField] private GameObject _errorPlaceholder;
        [SerializeField] private Button _clearButton;
        
        [SerializeField] private GameEvent _showLoading;
        [SerializeField] private GameEvent _dismissLoading;

        private readonly HashSet<string> _notifications = new HashSet<string>();
        private readonly HashSet<string> _clearedNotifications = new HashSet<string>();

        #region lifecycle methods

        void OnEnable()
        {
            var clearedNotificationIdsStr = PlayerPrefs.GetString(ClearedNotifications, "");
            foreach (var id in clearedNotificationIdsStr.Split(','))
            {
                _clearedNotifications.Add(id);
            }
            
            UpdateNotificationsWindow();
        }

        void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
        }

        #endregion

        #region public methods

        public void ClearNotifications()
        {
            // call it before the API to make UI responsive
            InitUiElements(0, false);

            _clearedNotifications.UnionWith(_notifications);
            var serializedIds = _clearedNotifications.Aggregate((accumulated, next) => accumulated + "," + next);
            PlayerPrefs.SetString(ClearedNotifications, serializedIds);
        }

        #endregion

        #region private methods

        private void UpdateNotificationsWindow()
        {
            InitUiElements(-1, false);
            _showLoading.Raise();

            var query = NotificationsQuery.ReadAndUnread().OfTypes(
                Notification.NotificationTypes.Comment,
                Notification.NotificationTypes.Direct,
                Notification.NotificationTypes.MentionInActivity,
                Notification.NotificationTypes.MentionInComment,
                Notification.NotificationTypes.ReplyToComment,
                Notification.NotificationTypes.Targeting);
            
            GetSocial.User.GetNotifications(query,
                onSuccess: notifications =>
                {
//            var notifications = new List<Notification>();
//            for (int i = 0; i < 20; i++)
//            {
//                notifications.Add(NewItem("Title " + i, "text"));
//            }
            
                    var notificaiotnsToShow = notifications.FindAll(notification => !_clearedNotifications.Contains(notification.Id));
                        
                    _notifications.Clear();
                    _notifications.UnionWith(notificaiotnsToShow.Select(notification => notification.Id));

                    _dismissLoading.Raise();
                    InitUiElements(notificaiotnsToShow.Count, false);
                    PopulateList(notificaiotnsToShow);
                },
                onError: error =>
                {
                    Debug.LogError(error.Message);

                    _dismissLoading.Raise();
                    InitUiElements(0, true);
                });
        }

        private static Notification NewItem(string title, string text)
        {
            var createdAt = DateTime.Now.Subtract(TimeSpan.FromMinutes(Random.Range(0, 3600)));
            return new Notification()
            {
                Action = Notification.Type.Custom,
                ActionData = new Dictionary<string, string>(),
                CreatedAt = (Int32) (createdAt.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
                Id = "Id" + Random.Range(1, 1000),
                NotificationType = Notification.NotificationTypes.NewFriendship,
                Text = text,
                Title = title,
                WasRead = false
            };
        }

        private void ClearNotificationsList()
        {
            foreach (Transform child in _notificationsList.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void PopulateList(List<Notification> notifications)
        {
            for (int i = 0; i < notifications.Count; i++)
            {
                GameObject card = Instantiate(_listItemPrefab);
                card.transform.SetParent(_notificationsList.transform, false);

                card.GetComponent<NotificationListItem>().Init(notifications[i]);
            }
        }

        private void InitUiElements(int notificationsCount, bool isError)
        {
            _clearButton.gameObject.SetActive(!isError && (notificationsCount > 0));
            _emptyPlaceholder.SetActive(!isError && (notificationsCount == 0));
            _errorPlaceholder.SetActive(isError);

            if (isError || (notificationsCount <= 0))
            {
                ClearNotificationsList();
            }
        }

        #endregion
    }
}