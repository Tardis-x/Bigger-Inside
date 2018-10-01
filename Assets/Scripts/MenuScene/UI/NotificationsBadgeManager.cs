﻿using GetSocialSdk.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
    public class NotificationsBadgeManager : MonoBehaviour
    {
        [SerializeField] private Text _countText;
        [SerializeField] private Image _unreadBackgoundImage;

        void OnEnable()
        {
            _unreadBackgoundImage.gameObject.SetActive(false);

            GetSocial.User.GetNotificationsCount(
                query: NotificationsCountQuery.Unread(),
                onSuccess: count =>
                {
                    _countText.text = count > 99 ? "99+" : count.ToString();
                    _unreadBackgoundImage.gameObject.SetActive(count > 0);
                },
                onError: error =>
                {
                    Debug.LogErrorFormat("Failed to retrieve notifications count, error: {0}", error.Message);
                });
        }
    }
}