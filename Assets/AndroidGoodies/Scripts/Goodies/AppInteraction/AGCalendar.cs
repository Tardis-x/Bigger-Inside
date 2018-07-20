// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGCalendar.cs
//



#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	public static class AGCalendar
	{
		/// <summary>
		/// If this event counts as busy time or is still free time that can be scheduled over.
		/// </summary>
		[SuppressMessage("ReSharper", "UnusedMember.Global")]
		public enum EventAvailability
		{
			Default = -1,

			/// <summary>
			/// Indicates that this event takes up time and will conflict with other events.
			/// </summary>
			Busy = 0x00000000,

			/// <summary>
			/// Indicates that this event is free time and will not conflict with other events.
			/// </summary>
			Free = 0x00000001,

			/// <summary>
			/// Indicates that the owner's availability may change, but should be considered busy time that will conflict.
			/// </summary>
			Tentative = 0x00000002
		}

		/// <summary>
		/// Defines how the event shows up for others when the calendar is shared.
		/// </summary>
		[SuppressMessage("ReSharper", "UnusedMember.Global")]
		public enum EventAccessLevel
		{
			/// <summary>
			/// Default access is controlled by the server and will be treated as public on the device.
			/// </summary>
			Default = 0x00000000,

			/// <summary>
			/// Private shares the event as a free/busy slot with no details.
			/// </summary>
			Private = 0x00000002,

			/// <summary>
			/// Public makes the contents visible to anyone with access to the calendar.
			/// </summary>
			Public = 0x00000003
		}

		/// <summary>
		/// android.provider.CalendarContract.Events
		/// </summary>
		static class Events
		{
			public const string TITLE = "title";
			public const string DESCRIPTION = "description";
			public const string EVENT_LOCATION = "eventLocation";
			public const string RRULE = "rrule";
			public const string AVAILABILITY = "availability";
			public const string ACCESS_LEVEL = "accessLevel";

			public static AndroidJavaObject CONTENT_URI
			{
				get
				{
					using (var eventsClass = new AndroidJavaClass(C.AndroidProviderCalendarContractEvents))
					{
						return eventsClass.GetStatic<AndroidJavaObject>("CONTENT_URI");
					}
				}
			}
		}

		static class CalendarContract
		{
			public const string EXTRA_EVENT_BEGIN_TIME = "beginTime";
			public const string EXTRA_EVENT_END_TIME = "endTime";
			public const string EXTRA_EVENT_ALL_DAY = "allDay";
		}

		/// <summary>
		/// Class to construct calendar event
		/// </summary>
		public class EventBuilder
		{
			readonly string _title;
			readonly long _beginTime;
			long _endTime;
			bool _allDay;
			string _location;
			string _description;
			string _emails;
			string _rrule;
			EventAvailability _availability = EventAvailability.Default;
			EventAccessLevel _accessLevel = EventAccessLevel.Default;

			/// <summary>
			/// Initializes a new instance of the <see cref="AGCalendar.EventBuilder"/> class.
			/// </summary>
			/// <param name="eventName">Event name.</param>
			/// <param name="beginTime">Event begin time.</param>
			[PublicAPI]
			public EventBuilder(string eventName, DateTime beginTime)
			{
				_title = eventName;
				_beginTime = beginTime.ToMillisSinceEpoch();
			}

			/// <summary>
			/// Sets the event end time.
			/// </summary>
			/// <returns>The event end time.</returns>
			/// <param name="endTime">End time of the event.</param>
			[PublicAPI]
			public EventBuilder SetEndTime(DateTime endTime)
			{
				_endTime = endTime.ToMillisSinceEpoch();
				return this;
			}

			/// <summary>
			/// Sets if the event the is all day event.
			/// </summary>
			/// <returns>If the event the is all day event.</returns>
			/// <param name="allDay">If set to <c>true</c> event is all day.</param>
			[PublicAPI]
			public EventBuilder SetIsAllDay(bool allDay)
			{
				_allDay = allDay;
				return this;
			}

			/// <summary>
			/// Sets the location.
			/// </summary>
			/// <returns>The location.</returns>
			/// <param name="location">Location.</param>
			[PublicAPI]
			public EventBuilder SetLocation(string location)
			{
				_location = location;
				return this;
			}

			/// <summary>
			/// Sets the description.
			/// </summary>
			/// <returns>The description.</returns>
			/// <param name="description">Description.</param>
			[PublicAPI]
			public EventBuilder SetDescription(string description)
			{
				_description = description;
				return this;
			}

			/// <summary>
			/// Sets the emails to invite people.
			/// </summary>
			/// <returns>The emails to invite people.</returns>
			/// <param name="emails">Emails to invite people.</param>
			[PublicAPI]
			public EventBuilder SetEmails(string[] emails)
			{
				_emails = string.Join(",", emails);
				return this;
			}

			/// <summary>
			/// Sets the Rrule.
			/// </summary>
			/// <returns>The Rrule.</returns>
			/// <param name="rrule">Rrule.</param>
			[PublicAPI]
			public EventBuilder SetRRule(string rrule)
			{
				_rrule = rrule;
				return this;
			}

			/// <summary>
			/// Sets the availability.
			/// </summary>
			/// <returns>The availability.</returns>
			/// <param name="availability">Availability.</param>
			[PublicAPI]
			public EventBuilder SetAvailability(EventAvailability availability)
			{
				_availability = availability;
				return this;
			}

			/// <summary>
			/// Sets the event access level.
			/// </summary>
			/// <returns>The event access level.</returns>
			/// <param name="accessLevel">Access level.</param>
			[PublicAPI]
			public EventBuilder SetAccessLevel(EventAccessLevel accessLevel)
			{
				_accessLevel = accessLevel;
				return this;
			}

			/// <summary>
			/// Builds the and shows the android calendar with constructed event.
			/// </summary>
			[PublicAPI]
			public void BuildAndShow()
			{
				if (AGUtils.IsNotAndroidCheck())
				{
					return;
				}

				var intent = new AndroidIntent(AndroidIntent.ACTION_EDIT).SetData(Events.CONTENT_URI);
				intent.SetType("vnd.android.cursor.item/event");
				intent.PutExtra(Events.TITLE, _title);
				intent.PutExtra(CalendarContract.EXTRA_EVENT_BEGIN_TIME, _beginTime);
				intent.PutExtra(CalendarContract.EXTRA_EVENT_ALL_DAY, _allDay);
				if (_endTime != 0)
				{
					intent.PutExtra(CalendarContract.EXTRA_EVENT_END_TIME, _endTime);
				}
				if (!string.IsNullOrEmpty(_location))
				{
					intent.PutExtra(Events.EVENT_LOCATION, _location);
				}
				if (!string.IsNullOrEmpty(_description))
				{
					intent.PutExtra(Events.DESCRIPTION, _description);
				}
				if (!string.IsNullOrEmpty(_rrule))
				{
					intent.PutExtra(Events.RRULE, _rrule);
				}
				if (!string.IsNullOrEmpty(_emails))
				{
					intent.PutExtra(AndroidIntent.EXTRA_EMAIL, _emails);
				}
				if (_availability != EventAvailability.Default)
				{
					intent.PutExtra(Events.AVAILABILITY, (int) _availability);
				}
				intent.PutExtra(Events.ACCESS_LEVEL, (int) _accessLevel);

				AGUtils.StartActivity(intent.AJO);
			}
		}

		#region API

		/// <summary>
		/// Indicates whether the calendar app is installed.
		/// </summary>
		/// <returns><c>true</c>, if the calendar app is installed <c>false</c> otherwise.</returns>
		[PublicAPI]
		public static bool UserHasCalendarApp()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			// dummy intent to resolve activity
			using (var intent = new AndroidIntent(AndroidIntent.ACTION_INSERT).SetData(Events.CONTENT_URI))
			{
				intent.PutExtra(Events.TITLE, "dummy_title");
				intent.PutExtra(CalendarContract.EXTRA_EVENT_BEGIN_TIME, (long) 123);
				return intent.ResolveActivity();
			}
		}

		/// <summary>
		/// Opens the calendar for the particular date.
		/// </summary>
		/// <param name="dateTime">Date time.</param>
		[PublicAPI]
		public static void OpenCalendarForDate(DateTime dateTime)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			// https://developer.android.com/guide/topics/providers/calendar-provider.html#intents
			long millis = dateTime.ToMillisSinceEpoch();
			using (var calendarContractClass = new AndroidJavaClass(C.AndroidProviderCalendarContract))
			{
				using (var contentUri = calendarContractClass.GetStatic<AndroidJavaObject>("CONTENT_URI"))
				{
					using (var contentUriBuilder = contentUri.CallAJO("buildUpon"))
					{
						contentUriBuilder.CallAJO("appendPath", "time");
						using (var conentUrisClass = new AndroidJavaClass(C.AndroidContentContentUris))
						{
							conentUrisClass.CallStaticAJO("appendId", contentUriBuilder, millis);
						}
						var uri = contentUriBuilder.CallAJO("build");
						var intent = new AndroidIntent(AndroidIntent.ACTION_VIEW).SetData(uri);
						AGUtils.StartActivity(intent.AJO);
					}
				}
			}
		}

		#endregion
	}
}
#endif