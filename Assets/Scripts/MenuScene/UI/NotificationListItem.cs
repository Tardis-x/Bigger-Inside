using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
    public class NotificationListItem : MonoBehaviour
    {
        [SerializeField] private Text _time;
        [SerializeField] private Text _text;

        public void Init(string title, string text, long createdAt)
        {
            var displayText = string.IsNullOrEmpty(title) ? string.Empty : (title + "\n");
            displayText += text;
            
            _text.text = displayText;

            _time.text = TimeUtils.GetPrettyDate(TimeUtils.UnixTimeStampToDateTime(createdAt));
        }
    }
}