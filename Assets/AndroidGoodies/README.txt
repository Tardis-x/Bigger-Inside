Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki
Support: leskiv.taras@gmail.com

---

### CHANGELOG


## v1.4.2

+ ADDED method to check if user has camera app installed before picking photos
+ ADDED Altitude to the location object returned by GPS

---

## v1.4.1

This release introduces fixes and internal changes:

FIXED Notifications not working on Android O and above
FIXED Migration problem with Application.bundleIdentifier -> Application.identifier for older Unity versions
CHANGED Now minimim SDK version is 14 not 10!
CHANGED Replaced one support-v4 jar with modular new support libs (See Plugins/Android folder)
CHANGED Now all files picked images/photos/audio/camera etc. are stored under external storage app directory so these files will be ones that get deleted first when the device runs low on storage and there is no guarantee when these files will be deleted. Previously these files were stored directly in external storage public directory.

---

## v1.4.0

ADDED Fingerprint scanner functionality (See `AGFingerprintScanner.cs`)
FIXED Issue when references to demo scripts were sometimes lost in example scene
FIXED Tweeting not working due to Twitter renaming their activity all the time
FIXED Some dialogs not working properly when bytecode stripping is enabled

---

## v1.3.0

ADDED Method to make newly added image file to appear in the gallery 
ADDED Method to get Ethernet MAC address (useful for AndroidTV)
IMPROVED Added proguard configuration to avoid stripping plugin class when using proguard
FIXED Some methods crashing on older Android versions (`AGFileUtils.DataDir`)
FIXED Issue with dialogs dismissing

---

## v1.2.2

FIXED Cleaned up some files

---

## v1.2.1

ADDED Several methods to `AGFileUtils.cs` class to get app cache and other directories
ADDED `AGMediaRecorder.cs` class record audio files
ADDED `AGWallpaperManager.cs` class to set device wallpaper images
ADDED Method to copy text to system clipboard

---

## v1.2.0

IMPROVED When showing dialog in immersive mode the app stays in immersive mode
IMPROVED Adde more options for local notifications (color, small icon etc.)
IMPROVED Now local notifications can be scheduled in the future
ADDED Support for repeating local notifications
ADDED Picking audio files from device. See `AGFilePicker.cs`
ADDED Picking video files from device. See `AGFilePicker.cs`
ADDED Picking arbitrary files by specifying mime type. See `AGFilePicker.cs`
ADDED Recording video file from camera and receiving it. See `AGCamera.cs`

---

## v1.1.10

FIXED Incorrect orientation when receiving the taken photo
FIXED Sharing in Twitter not working after Twitter updated composer activity class
FIXED When saving screenshot image filename extension is added automatically now
IMPROVED Creating calendar event not working on some devices properly

---

## v1.1.9

BBREAKING CHANGES: Photo and image pickers have now different method signatures, please check the examples

ADDED Function to send MMS
ADDED Functions to get/set system screen brightness
ADDED Functions to open 'Can modify system settings' system screen where user can allow app to modify system settings
CHANGED Completely reworked pickers to make them more reliable

---

## v1.1.8

ADDED Contact picker to pick contacts from address book
ADDED Options to use Intent.ACTION_OPEN_DOCUMENT for image picker
ADDED Method to load image into `Texture2D` provided with string Android URI
ADDED Method to open Facebook and Twitter user profile by profile ID
ADDED Time Picker - now can choose if to show 24h or 12h format
FIXED Image display name not being correct when picking image
FIXED Scaling issues when picking image from gallery

---

## v1.1.7

ADDED options to send text/image directly via Facebook Messenger, WhatsApp, Telegram, Viber, SnapChat
ADDED Feature to open instagram profile in the native app
ADDED Feature to uninstall the app by package
ADDED Feature to intall the APK file from SD card
FIXED Minor bugs in the editor

---

## v1.1.6

FIXED not taking into consideration local time when creating calendar event

---

## v1.1.5

- ADDED support for native SharedPreferenced (get, set, clear all, get all)
- ADDED Method to get installed packages on device `AGDeviceInfo.GetInstalledPackages`
- ADDED Method to share/upload video to YouTube/Facebook etc. `AGShare.ShareVideo`
- FIXED Flashlight not always working on latest android versions

---

## v1.1.4

- Reworked how saving image to gallery works to refresh gallery after saving
- Fixed how sending SMS works

---

## v1.1.3

- Added `AGCamera` class to take photos and receive result as `Texture2D`.
- Added `AGPermissions.RequestPermissions` method to request runtime peermissions

**Breaking changes:**

- `ImagePickResult` and `ImageResultSize` classes are not inner any more. Change `AGGallery.ImagePickResult` and `AGGallery.ImageResultSize` to `ImagePickResult` and `ImageResultSize` respectively in your code.

---

## v1.1.2b HOTFIX RELEASE

---

- Fixed Android support lib v7 was packaged into `goodies-bridge-release.jar` causing conflicts if support lib v7 was already in project

---

## v1.1.2

- Added `Timestamp`, `HasSpeed`, `Speed`, `HasBearing`, `Bearing`, `IsFromMockProvider` properties to `Location` object when getting GPS updates.
- `AGGPS.DistanceBetween` and `Location.DistanceTo` to compute distance between locations
- `AGGPS.DeviceHasGPS` - convenience method to check if device has GPS module.

---

## v1.1.1

- Added cc and bcc recipients when sending email.
- Added `<uses-feature android:name="android.hardware.location.gps" android:required="false" />` to manifest and added GPS class reference docs.
- Added ability to manage [runtime permissions](https://developer.android.com/training/permissions/requesting.html).

---

## v1.1.0

- Added method to launch any installed app on device by providing package (`AGApps.OpenOtherAppOnDevice()`)
- Ability to pick photo from gallery and receive it as `Texture2D` (`AGGallery.PickImageFromGallery()`)

---

## v1.0.0 Initial PRO Version Release

- Create new note
- Sharing image (Texture2D)
- Sharing Screenshot
- Saving image to gallery
- Added progress bar and spinner loading dialogs
- Added theme choice for dialogs (Light/Dark)
- Check battery charge level
- Get application package
- Create calendar event with info
- Open calendar at date
- Get external storage directory and all almost all [`android.os.Environment`](https://developer.android.com/reference/android/os/Environment.html) methods.
  - Get standard directories (data dir, download cache dir, external storage dir etc.)
  - Get external storage state
  - Check if external storage removable
  - Check if external storage emulated
  - Etc. check `AndroidEnvironment.cs` API for full info
- Open settings or any required section of settings
- Open alarms/Set alarm.
- Set timer

## v0.1

+ Initial Release

Features:

- Different dialogs (message, radiobuttons, checkboxes)
- Time and Date picker
- Show Toast
- Open Map locations/addresses
- Native share text/tweet/send email/sms
- Checking if app is installed
- Enable immersive mode
- Get device info (model, manufacturer, android version etc.)

---
