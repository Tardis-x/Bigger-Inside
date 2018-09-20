// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGAlertDialog.cs
//



#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using UnityEngine;

	/// <summary>
	/// Android alert dialog. Contains static methods to show different dialogs.
	/// </summary>
	public class AGAlertDialog
	{
		readonly AndroidJavaObject _dialog;

		AGAlertDialog(Builder builder)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				throw new InvalidOperationException("AndroidJavaObject can be created only on Android");
			}

			var javaBuilder = CreateDialogBuilder(builder._theme);
			if (!string.IsNullOrEmpty(builder._title))
			{
				javaBuilder.CallAJO("setTitle", builder._title);
			}
			if (!string.IsNullOrEmpty(builder._message))
			{
				javaBuilder.CallAJO("setMessage", builder._message);
			}

			// Buttons
			if (builder._isPositiveButtonSet)
			{
				javaBuilder.CallAJO("setPositiveButton", builder._positiveButtonText, new DialogOnClickListenerProxy(builder._positiveButtonCallback));
			}
			if (builder._isNegativeButtonSet)
			{
				javaBuilder.CallAJO("setNegativeButton", builder._negativeButtonText, new DialogOnClickListenerProxy(builder._negativeButtonCallback));
			}
			if (builder._isNeutralButtonSet)
			{
				javaBuilder.CallAJO("setNeutralButton", builder._neutralButtonText, new DialogOnClickListenerProxy(builder._neutralButtonCallback));
			}

			// Items
			if (builder._areItemsSet)
			{
				javaBuilder.CallAJO("setItems", builder._items, new DialogOnClickListenerProxy(builder._itemClickCallback, true));
			}
			if (builder._areSingleChoiceItemsSet)
			{
				javaBuilder.CallAJO("setSingleChoiceItems",
					builder._singleChoiceItems,
					builder._singleChoiceCheckedItem,
					new DialogOnClickListenerProxy(builder._singleChoiceItemClickCallback));
			}
			if (builder._areMultiChoiceItemsSet)
			{
				javaBuilder.CallAJO("setMultiChoiceItems",
					builder._multiChoiceItems,
					builder._multiChoiceCheckedItems,
					new DialogOnMultiChoiceClickListenerProxy(builder._multiChoiceItemClickCallback));
			}

			// Cancel / Dismiss
			if (builder._onCancelCallback != null)
			{
				javaBuilder.CallAJO("setOnCancelListener", new DialogOnCancelListenerPoxy(builder._onCancelCallback));
			}
			if (builder._onDismissCallback != null)
			{
				javaBuilder.CallAJO("setOnDismissListener", new DialogOnDismissListenerProxy(builder._onDismissCallback));
			}

			_dialog = javaBuilder.CallAJO("create");
		}

		const string AlertDialogBuilderClass = "android.app.AlertDialog$Builder";

		static AndroidJavaObject CreateDialogBuilder(AGDialogTheme theme)
		{
			int themeResource = AndroidDialogUtils.GetDialogTheme(theme);
			return AndroidDialogUtils.IsDefault(themeResource)
				? new AndroidJavaObject(AlertDialogBuilderClass, AGUtils.Activity)
				: new AndroidJavaObject(AlertDialogBuilderClass, AGUtils.Activity, themeResource);
		}

		class Builder
		{
			internal string _title;
			internal string _message;
			internal AGDialogTheme _theme = AGDialogTheme.Light;

			// buttons
			internal bool _isPositiveButtonSet;

			internal string _positiveButtonText;
			internal Action _positiveButtonCallback;

			internal bool _isNegativeButtonSet;
			internal string _negativeButtonText;
			internal Action _negativeButtonCallback;

			internal bool _isNeutralButtonSet;
			internal string _neutralButtonText;
			internal Action _neutralButtonCallback;

			// items
			internal bool _areItemsSet;

			internal string[] _items;
			internal Action<int> _itemClickCallback;

			// single choice
			internal bool _areSingleChoiceItemsSet;

			internal string[] _singleChoiceItems;
			internal int _singleChoiceCheckedItem;
			internal Action<int> _singleChoiceItemClickCallback;

			// multi choice
			internal bool _areMultiChoiceItemsSet;

			internal string[] _multiChoiceItems;
			internal bool[] _multiChoiceCheckedItems;
			internal Action<int, bool> _multiChoiceItemClickCallback;

			internal Action _onCancelCallback;
			internal Action _onDismissCallback;

			public Builder SetTitle(string title)
			{
				_title = title;
				return this;
			}

			public Builder SetMessage(string message)
			{
				_message = message;
				return this;
			}

			public Builder SetTheme(AGDialogTheme theme)
			{
				_theme = theme;
				return this;
			}

			public Builder SetPositiveButton(string buttonText, Action callback)
			{
				_isPositiveButtonSet = true;
				_positiveButtonText = buttonText;
				_positiveButtonCallback = callback;
				return this;
			}

			public Builder SetNegativeButton(string buttonText, Action callback)
			{
				_isNegativeButtonSet = true;
				_negativeButtonText = buttonText;
				_negativeButtonCallback = callback;
				return this;
			}

			public Builder SetNeutralButton(string buttonText, Action callback)
			{
				_isNeutralButtonSet = true;
				_neutralButtonText = buttonText;
				_neutralButtonCallback = callback;
				return this;
			}

			public Builder SetItems(string[] items, Action<int> onItemClick)
			{
				_areItemsSet = true;
				_items = items;
				_itemClickCallback = onItemClick;
				return this;
			}

			public Builder SetMultiChoiceItems(string[] items, bool[] checkedItems, Action<int, bool> onItemClick)
			{
				_areMultiChoiceItemsSet = true;
				_multiChoiceItems = items;
				_multiChoiceCheckedItems = checkedItems;
				_multiChoiceItemClickCallback = onItemClick;
				return this;
			}

			public Builder SetSingleChoiceItems(string[] items, int checkedItem, Action<int> onItemClick)
			{
				_areSingleChoiceItemsSet = true;
				_singleChoiceItems = items;
				_singleChoiceCheckedItem = checkedItem;
				_singleChoiceItemClickCallback = onItemClick;
				return this;
			}

			/// <summary>
			/// Sets the on cancel listener.
			/// </summary>
			/// <returns>The on cancel listener.</returns>
			/// <param name="onCancel">On cancel.</param>
			public Builder SetOnCancelListener(Action onCancel)
			{
				_onCancelCallback = onCancel;
				return this;
			}

			/// <summary>
			/// Sets the on dismiss listener.
			/// </summary>
			/// <returns>The on dismiss listener.</returns>
			/// <param name="onDismiss">On dismiss.</param>
			public Builder SetOnDismissListener(Action onDismiss)
			{
				_onDismissCallback = onDismiss;
				return this;
			}

			/// <summary>
			/// Creates an AGAlertDialog with the arguments supplied to this builder.
			/// </summary>
			public AGAlertDialog Create()
			{
				return new AGAlertDialog(this);
			}
		}

		void Show()
		{
			AndroidDialogUtils.ShowWithImmersiveModeWorkaround(_dialog);
		}

		#region show_dialog

		/// <summary>
		/// Displays message dialog with positive button only
		/// </summary>
		/// <param name="title">Dialog title</param>
		/// <param name="message">Dialog message</param>
		/// <param name="positiveButtonText">Text for positive button</param>
		/// <param name="onPositiveButtonClick">Positive button callback</param>
		/// <param name="onDismiss">On dismiss callback</param>
		/// <param name="theme">Dialog theme</param>
		public static void ShowMessageDialog(string title, string message, string positiveButtonText,
			Action onPositiveButtonClick,
			Action onDismiss = null,
			AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (onPositiveButtonClick == null)
			{
				throw new ArgumentNullException("onPositiveButtonClick", "Button callback cannot be null");
			}

			AGUtils.RunOnUiThread(() =>
			{
				var builder = CreateMessageDialogBuilder(title, message, positiveButtonText, onPositiveButtonClick, onDismiss).SetTheme(theme);
				var dialog = builder.Create();
				dialog.Show();
			});
		}


		/// <summary>
		/// Displays message dialog with positive and negative buttons
		/// </summary>
		/// <param name="title">Dialog title</param>
		/// <param name="message">Dialog message</param>
		/// <param name="positiveButtonText">Text for positive button</param>
		/// <param name="onPositiveButtonClick">Positive button callback</param>
		/// <param name="negativeButtonText">Text for negative button</param>
		/// <param name="onNegativeButtonClick">Negative button callback</param>
		/// <param name="onDismiss">On dismiss callback</param>
		/// <param name="theme">Dialog theme</param>
		public static void ShowMessageDialog(string title, string message,
			string positiveButtonText, Action onPositiveButtonClick,
			string negativeButtonText, Action onNegativeButtonClick,
			Action onDismiss = null,
			AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (onPositiveButtonClick == null)
			{
				throw new ArgumentNullException("onPositiveButtonClick", "Button callback cannot be null");
			}
			if (onNegativeButtonClick == null)
			{
				throw new ArgumentNullException("onNegativeButtonClick", "Button callback cannot be null");
			}

			AGUtils.RunOnUiThread(() =>
			{
				var builder = CreateMessageDialogBuilder(title, message, positiveButtonText, onPositiveButtonClick, onDismiss).SetTheme(theme);
				builder.SetNegativeButton(negativeButtonText, onNegativeButtonClick);
				var dialog = builder.Create();
				dialog.Show();
			});
		}

		/// <summary>
		/// Displays message dialog with positive, negative buttons and neutral buttons
		/// </summary>
		/// <param name="title">Dialog title</param>
		/// <param name="message">Dialog message</param>
		/// <param name="positiveButtonText">Text for positive button</param>
		/// <param name="onPositiveButtonClick">Positive button callback</param>
		/// <param name="negativeButtonText">Text for negative button</param>
		/// <param name="onNegativeButtonClick">Negative button callback</param>
		/// <param name="neutralButtonText">Text for neutral button</param>
		/// <param name="onNeutralButtonClick">Neutral button callback</param>
		/// <param name="onDismiss">On dismiss callback</param>
		/// <param name="theme">Dialog theme</param>
		public static void ShowMessageDialog(string title, string message,
			string positiveButtonText, Action onPositiveButtonClick,
			string negativeButtonText, Action onNegativeButtonClick,
			string neutralButtonText, Action onNeutralButtonClick,
			Action onDismiss = null,
			AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (onPositiveButtonClick == null)
			{
				throw new ArgumentNullException("onPositiveButtonClick", "Button callback cannot be null");
			}
			if (onNegativeButtonClick == null)
			{
				throw new ArgumentNullException("onNegativeButtonClick", "Button callback cannot be null");
			}
			if (onNeutralButtonClick == null)
			{
				throw new ArgumentNullException("onNeutralButtonClick", "Button callback cannot be null");
			}

			AGUtils.RunOnUiThread(() =>
			{
				var builder = CreateMessageDialogBuilder(title, message, positiveButtonText, onPositiveButtonClick, onDismiss).SetTheme(theme);
				builder.SetNegativeButton(negativeButtonText, onNegativeButtonClick);
				builder.SetNeutralButton(neutralButtonText, onNeutralButtonClick);
				var dialog = builder.Create();
				dialog.Show();
			});
		}

		static Builder CreateMessageDialogBuilder(string title, string message,
			string positiveButtonText, Action onPositiveButtonClick, Action onDismiss)
		{
			var builder = new Builder().SetTitle(title).SetMessage(message);
			builder.SetPositiveButton(positiveButtonText, onPositiveButtonClick);
			if (onDismiss != null)
			{
				builder.SetOnDismissListener(onDismiss);
			}
			return builder;
		}

		/// <summary>
		/// Shows the chooser dialog to choose one item from the list. Dialog is dismissed on item click.
		/// </summary>
		/// <param name="title">Dialog title</param>
		/// <param name="items">Itmes to choose from</param>
		/// <param name="onClickCallback">Invoked on item click</param>
		/// <param name="theme">Dialog theme</param>
		public static void ShowChooserDialog(string title, string[] items, Action<int> onClickCallback,
			AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			AGUtils.RunOnUiThread(() =>
			{
				var builder = new Builder().SetTitle(title).SetTheme(theme);
				builder.SetItems(items, onClickCallback);
				var dialog = builder.Create();
				dialog.Show();
			});
		}

		/// <summary>
		/// Shows the single item choice dialog.
		/// </summary>
		/// <returns>The single item choice dialog.</returns>
		/// <param name="title">Title.</param>
		/// <param name="items">Items.</param>
		/// <param name="checkedItem">Checked item.</param>
		/// <param name="onItemClicked">On item clicked.</param>
		/// <param name="positiveButtonText">Positive button text.</param>
		/// <param name="onPositiveButtonClick">On positive button click.</param>
		/// <param name="theme">Dialog theme</param>
		public static void ShowSingleItemChoiceDialog(string title, string[] items, int checkedItem, Action<int> onItemClicked,
			string positiveButtonText, Action onPositiveButtonClick,
			AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (items == null)
			{
				throw new ArgumentNullException("items", "Items can't be null");
			}
			if (onItemClicked == null)
			{
				throw new ArgumentNullException("onItemClicked", "Item click callback cannot be null");
			}
			if (onPositiveButtonClick == null)
			{
				throw new ArgumentNullException("onPositiveButtonClick", "Button callback cannot be null");
			}

			AGUtils.RunOnUiThread(() =>
			{
				var builder = CreateSingleChoiceDialogBuilder(title, items, checkedItem, onItemClicked).SetTheme(theme);
				builder.SetPositiveButton(positiveButtonText, onPositiveButtonClick);
				var dialog = builder.Create();
				dialog.Show();
			});
		}

		static Builder CreateSingleChoiceDialogBuilder(string title, string[] items, int checkedItem, Action<int> onItemClicked)
		{
			var builder = new Builder().SetTitle(title);
			builder.SetSingleChoiceItems(items, checkedItem, onItemClicked);
			return builder;
		}

		/// <summary>
		/// Shows the multi item choice dialog.
		/// </summary>
		/// <returns>The multi item choice dialog.</returns>
		/// <param name="title">Title.</param>
		/// <param name="items">Items.</param>
		/// <param name="checkedItems">Checked items.</param>
		/// <param name="onItemClicked">On item clicked.</param>
		/// <param name="positiveButtonText">Positive button text.</param>
		/// <param name="onPositiveButtonClick">On positive button click.</param>
		/// <param name="theme">Dialog theme</param>
		public static void ShowMultiItemChoiceDialog(string title, string[] items, bool[] checkedItems, Action<int, bool> onItemClicked,
			string positiveButtonText, Action onPositiveButtonClick,
			AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (items == null)
			{
				throw new ArgumentNullException("items", "Items can't be null");
			}
			if (checkedItems == null)
			{
				throw new ArgumentNullException("checkedItems", "Checked items can't be null");
			}
			if (onItemClicked == null)
			{
				throw new ArgumentNullException("onItemClicked", "Item click callback can't be null");
			}
			if (onPositiveButtonClick == null)
			{
				throw new ArgumentNullException("onPositiveButtonClick", "Button click callback can;t be null");
			}

			AGUtils.RunOnUiThread(() =>
			{
				var builder = CreateMultiChoiceDialogBuilder(title, items, checkedItems, onItemClicked).SetTheme(theme);
				builder.SetPositiveButton(positiveButtonText, onPositiveButtonClick);
				var dialog = builder.Create();
				dialog.Show();
			});
		}

		static Builder CreateMultiChoiceDialogBuilder(string title, string[] items, bool[] checkedItems, Action<int, bool> onItemClicked)
		{
			var builder = new Builder().SetTitle(title);
			builder.SetMultiChoiceItems(items, checkedItems, onItemClicked);
			return builder;
		}

		#endregion

	}
}
#endif