// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGLocalNotifications.cs
//

#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
    using System;
    using Internal;
    using JetBrains.Annotations;
    using UnityEngine;

    /// <summary>
    /// Class to show local notifications.
    /// </summary>
    public static class AGLocalNotifications
    {
        static string PackageName
        {
            get
            {
#if UNITY_5_6_OR_NEWER
                return Application.identifier;
#else
				return Application.bundleIdentifier;
#endif
            }
        }

        const string GoodiesNotificationManagerClass = "com.deadmosquitogames.notifications.GoodiesNotificationManager";

        /// <summary>
        /// Shows the local notification. Upon click it will open the game if the game is not in foreground.
        /// </summary>
        /// <param name="title">Set the title (first row) of the notification, in a standard notification.</param>
        /// <param name="text">Set the text (second row) of the notification, in a standard notification.</param>
        /// <param name="when">Set the time that the event occurred. Notifications in the panel are sorted by this time.</param>
        /// <param name="tickerText">Set the text that is displayed in the status bar when the notification first arrives.</param>
        /// <param name="iconName">Icon drawable name. Icon must be included in the app in res/drawable folder</param>
        [PublicAPI]
        public static void ShowNotification([NotNull] string title, string text,
            DateTime? when, string tickerText = null, string iconName = null)
        {
            if (AGUtils.IsNotAndroidCheck())
            {
                return;
            }

            if (when == null)
            {
                when = DateTime.Now;
            }

            using (var c = new AndroidJavaClass(GoodiesNotificationManagerClass))
            {
                Color32 bgColor = Color.black;
                c.CallStatic("setNotification",
                    AGUtils.Activity,
                    new System.Random().Next(),
                    when.Value.ToMillisSinceEpoch() - DateTime.Now.ToMillisSinceEpoch(),
                    title, text, text,
                    1,
                    1,
                    1,
                    "", "notify_icon_small",
                    bgColor.ToAndroidColor(), PackageName
                );
            }
        }

        /// <summary>
        /// Shows the local notification. Upon click it will open the game if the game is not in foreground.
        /// 
        /// To change the small notification icon replace 'notify_icon_small.png' in Android/Plugins/res/drawable folder with your image but maintaining the same icon name.
        /// </summary>
        /// <param name="id">Notification id. You can cancel notification by its id.</param>
        /// <param name="when">When notification is going to be shown.</param>
        /// <param name="title">Title text.</param>
        /// <param name="message">Notification message.</param>
        /// <param name="bgColor">Message background color.</param>
        /// <param name="sound">Whether to play sound.</param>
        /// <param name="vibrate">Whether to vibrate.</param>
        /// <param name="lights">Whether to show lights.</param>
        /// <param name="bigIcon">Icon drawable name. Icon must be included in the app in Android/Plugins/res/drawable folder</param>
        [PublicAPI]
        public static void ShowNotification(int id,
            DateTime when,
            string title, string message,
            Color32 bgColor,
            bool sound = true, bool vibrate = true, bool lights = true,
            string bigIcon = "")
        {
            if (AGUtils.IsNotAndroidCheck())
            {
                return;
            }

            using (var c = new AndroidJavaClass(GoodiesNotificationManagerClass))
            {
                c.CallStatic("setNotification",
                    AGUtils.Activity,
                    id,
                    when.ToMillisSinceEpoch() - DateTime.Now.ToMillisSinceEpoch(),
                    title, message, message,
                    sound ? 1 : 0,
                    vibrate ? 1 : 0,
                    lights ? 1 : 0,
                    bigIcon, "notify_icon_small",
                    bgColor.ToAndroidColor(), PackageName
                );
            }
        }

        /// <summary>
        /// Shows the repeating local notification. Upon click it will open the game if the game is not in foreground.
        /// 
        /// NOTE: as of API 19, all repeating alarms are inexact.  If your
        /// application needs precise delivery times then it must use one-time
        ///	exact alarms, rescheduling each time. Legacy applications
        ///	whose SDK version is earlier than API 19 will continue to have all
        ///	of their alarms, including repeating alarms, treated as exact.
        /// 
        /// </summary>
        /// <param name="id">Notification id. You can cancel notification by its id.</param>
        /// <param name="when">When notification is going to be shown.</param>
        /// <param name="intervalMillis">A period at which the alarm will automatically repeat.</param>
        /// <param name="title">Title text.</param>
        /// <param name="message">Notification message.</param>
        /// <param name="bgColor">Message background color.</param>
        /// <param name="sound">Whether to play sound.</param>
        /// <param name="vibrate">Whether to vibrate.</param>
        /// <param name="lights">Whether to show lights.</param>
        /// <param name="bigIcon">Icon drawable name. Icon must be included in the app in Android/Plugins/res/drawable folder</param>
        [PublicAPI]
        public static void ShowNotificationRepeating(int id,
            DateTime when,
            long intervalMillis,
            string title, string message,
            Color32 bgColor,
            bool sound = true, bool vibrate = true, bool lights = true,
            string bigIcon = "")
        {
            if (AGUtils.IsNotAndroidCheck())
            {
                return;
            }

            using (var c = new AndroidJavaClass(GoodiesNotificationManagerClass))
            {
                c.CallStatic("setRepeatingNotification",
                    AGUtils.Activity,
                    id,
                    when.ToMillisSinceEpoch() - DateTime.Now.ToMillisSinceEpoch(),
                    title, message, message,
                    intervalMillis,
                    sound ? 1 : 0,
                    vibrate ? 1 : 0,
                    lights ? 1 : 0,
                    bigIcon, "notify_icon_small",
                    bgColor.ToAndroidColor(), PackageName
                );
            }
        }

        /// <summary>
        /// Cancels future notification that was scheduled.
        /// </summary>
        /// <param name="notificationId">Notification identifier.</param>
        [PublicAPI]
        public static void CancelNotification(int notificationId)
        {
            if (AGUtils.IsNotAndroidCheck())
            {
                return;
            }

            using (var c = new AndroidJavaClass(GoodiesNotificationManagerClass))
            {
                c.CallStatic("cancelNotification", AGUtils.Activity, notificationId);
            }
        }
    }
}
#endif