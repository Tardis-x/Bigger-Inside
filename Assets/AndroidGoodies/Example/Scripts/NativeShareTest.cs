namespace AndroidGoodiesExamples
{
	using System.IO;
	using DeadMosquito.AndroidGoodies;
	using JetBrains.Annotations;
	using UnityEngine;

	public class NativeShareTest : MonoBehaviour
	{
		public string message = "Android Native Goodies PRO by Dead Mosquito Games http://u3d.as/xf8 #AssetStore";
		public Texture2D image;

		public bool withChooser = true;

		public string subject;
		public string text;

#if UNITY_ANDROID
		[UsedImplicitly]
		public void OnShareClick()
		{
			AGShare.ShareTextWithImage(subject, text, image);
		}

		[UsedImplicitly]
		public void OnSendEmailClick()
		{
			var recipients = new[] {"x@gmail.com", "hello@gmail.com"};
			var ccRecipients = new[] {"cc@gmail.com"};
			var bccRecipients = new[] {"bcc@gmail.com", "bcc-guys@gmail.com"};
			AGShare.SendEmail(recipients, "subj", "body", image, withChooser, cc: ccRecipients, bcc: bccRecipients);
		}

		[UsedImplicitly]
		public void OnSendSmsClick()
		{
			AGShare.SendSms("123123123", "Hello", withChooser, "Custom Chooser Title");
		}

		[UsedImplicitly]
		public void OnSendMmsClick()
		{
			AGShare.SendMms("777-888-999", "Hello my friend", image, false, "MMS!");
		}

		[UsedImplicitly]
		public void OnTweetClick()
		{
			AGShare.Tweet(message, image);
		}

		[UsedImplicitly]
		public void OnShareScreenshot()
		{
			AGShare.ShareScreenshot();
		}

		// FB Messenger
		[UsedImplicitly]
		public void OnSendFacebookMessageText()
		{
			AGShare.SendFacebookMessageText(message);
		}

		[UsedImplicitly]
		public void OnSendFacebookMessageImage()
		{
			AGShare.SendFacebookMessageImage(image);
		}

		// WhatsApp
		[UsedImplicitly]
		public void OnSendWhatsAppTextMessage()
		{
			AGShare.SendWhatsAppTextMessage(message);
		}

		[UsedImplicitly]
		public void OnSendWhatsAppImageMessage()
		{
			AGShare.SendWhatsAppImageMessage(image);
		}

		// Instagram
		[UsedImplicitly]
		public void OnSendInstagramImage()
		{
			AGShare.ShareInstagramPhoto(image);
		}

		// Telegram
		[UsedImplicitly]
		public void OnSendTelegramTextMessage()
		{
			AGShare.SendTelegramTextMessage(message);
		}

		[UsedImplicitly]
		public void OnSendTelegramImageMessage()
		{
			AGShare.SendTelegramImageMessage(image);
		}

		// Viber
		[UsedImplicitly]
		public void OnSendViberTextMessage()
		{
			AGShare.SendViberTextMessage(message);
		}

		[UsedImplicitly]
		public void OnSendViberImageMessage()
		{
			AGShare.SendViberImageMessage(image);
		}

		// SnapChat
		[UsedImplicitly]
		public void OnSendSnapChatTextMessage()
		{
			AGShare.SendSnapChatTextMessage(message);
		}

		[UsedImplicitly]
		public void OnSendSnapChatImageMessage()
		{
			AGShare.SendSnapChatImageMessage(image);
		}

		[UsedImplicitly]
		public void OnShareVideo()
		{
			// NOTE: In order to test this method video file must exist on file system.
			// To test this method you put 'xxx.mov' video into your downloads folder
			const string videoFileName = "/xxx.mov";
			var filePath = Path.Combine(AGEnvironment.ExternalStorageDirectoryPath, AGEnvironment.DirectoryDownloads) + videoFileName;
			Debug.Log("Sharing video: " + filePath + ", file exists?: " + File.Exists(filePath));
			AGShare.ShareVideo(filePath, "My Video Title", "Upload Video");
		}
#endif
	}
}
