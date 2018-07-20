namespace AndroidGoodiesExamples
{
	using System;
	using DeadMosquito.AndroidGoodies;
	using JetBrains.Annotations;
	using UnityEngine;

	public class CalendarTest : MonoBehaviour
	{
#if UNITY_ANDROID
		[UsedImplicitly]
		public void OnCreateEventClick()
		{
			var beginTime = DateTime.Now;
			var endTime = beginTime.AddHours(2);
			var eventBuilder = new AGCalendar.EventBuilder("Lunch with someone special", beginTime);
			eventBuilder.SetEndTime(endTime);
			eventBuilder.SetIsAllDay(false);
			eventBuilder.SetLocation("Miami beach");
			eventBuilder.SetDescription("Amazing lunch with a beautiful lady");
			eventBuilder.SetEmails(new[] {"lol@gmail.com", "test@gmail.com"});
			eventBuilder.SetRRule("FREQ=DAILY");
			// This feature crashes on SAMSUNG calendar app, use with caution!
			// eventBuilder.SetAccessLevel(AGCalendar.EventAccessLevel.Public);
			eventBuilder.SetAvailability(AGCalendar.EventAvailability.Free);
			eventBuilder.BuildAndShow();
		}

		[UsedImplicitly]
		public void OnOpenCalendarDateClick()
		{
			// Open calendar on a date a week ahead
			AGCalendar.OpenCalendarForDate(DateTime.Now.AddDays(7));
		}
#endif
	}
}
