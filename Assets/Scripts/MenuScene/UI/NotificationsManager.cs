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
        [SerializeField] private GameObject _listItemPrefab;
        [SerializeField] private GameObject _notificationsList;
        [SerializeField] private GameObject _emptyPlaceholder;
        [SerializeField] private GameObject _errorPlaceholder;
        [SerializeField] private Button _clearButton;

        private readonly List<Notification> _notifications = new List<Notification>();

        #region lifecycle methods

        void OnEnable()
        {
            UpdateNotificationsWindow();

//            var notifications = new List<Notification>();
//
//            for (int i = 0; i < 30; i++)
//            {
//                notifications.Add(NewItem("Title " + i, "Text Text Text Text\nText Text Text Text \nText Text Text Text \nText Text Text Text ".Substring(0, Random.RandomRange(10, 75))));
//            }
//
//            if (notifications.Count == 0)
//            {
//                DisableClearButton();
//                ShowEmptyListPlaceholder();
//            }
//
//            PopulateList(notifications);
        }

        void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
        }

        #endregion

        #region public methods

        public void ClearNotifications()
        {
            var ids = _notifications.Select(notification => notification.Id).ToList();

            // call it before the API to make UI responsive
            InitUiElements(0, false);

            GetSocial.User.SetNotificationsRead(ids, true,
                onSuccess: () => { Debug.Log("Cleared notifications"); },
                onError: error => { Debug.LogError(error.Message); });
        }

        #endregion

        #region private methods

        private void UpdateNotificationsWindow()
        {
            InitUiElements(0, false);

            NotificationsQuery query = NotificationsQuery.Unread();
            GetSocial.User.GetNotifications(query,
                onSuccess: notifications =>
                {
                    _notifications.Clear();
                    _notifications.AddRange(notifications);

                    InitUiElements(notifications.Count, false);
                    PopulateList(notifications);
                },
                onError: error =>
                {
                    Debug.LogError(error.Message);

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

                card.GetComponent<NotificationListItem>().Init(notifications[i].Title, notifications[i].Text,
                    notifications[i].CreatedAt);
            }
        }

        private void InitUiElements(int notificationsCount, bool isError)
        {
            _clearButton.gameObject.SetActive(!isError && (notificationsCount > 0));
            _emptyPlaceholder.SetActive(!isError && (notificationsCount <= 0));
            _errorPlaceholder.SetActive(isError);

            if (isError || (notificationsCount <= 0))
            {
                ClearNotificationsList();
            }
        }

        #endregion
    }
}