using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class SpeechScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Value References")] 
    [SerializeField] private Text _speakerCompanyCountryText;
    [SerializeField] private Text _speakerNameText;
    [SerializeField] private Text _startTimeHoursText;
    [SerializeField] private Text _startTimeMinutesText;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _tagText;
    [SerializeField] private RawImage _tagImage;
    [SerializeField] private RawImage _speakerPhoto;
    [SerializeField] private Text _timespanText;
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      LOGO_BASE_PATH = Application.persistentDataPath + "Graphics/Logo";
    }

    private void Start()
    {
      if(_photoUrl != null) LoadImage(_photoUrl, _speakerPhoto);
      if(_tagImageUrl != null) LoadImage(_tagImageUrl, _tagImage);
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public RectTransform GetInstance(string startTime, string endTime, string name, string tag, Speaker speaker,
      string sessionImageUrl)
    {
      SpeechScript instance = Instantiate(this);
      instance.SetName(name);
      instance.SetStartTime(startTime);
      instance.SetTimespanText(GetTimespanText(startTime, endTime));
      
      if (speaker != null)
      {
        instance.SetSpeakerName(speaker.Name);
        instance.SetSpeakerCompanyCountry(speaker.Company, speaker.Country);
        instance._photoUrl = speaker.PhotoUrl;
      }

      if (sessionImageUrl != "") instance._tagImageUrl = sessionImageUrl;
      
      instance.SetTag(tag);
      return instance.GetComponent<RectTransform>();
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private const string ANDROID_TAG_COLOR = "#8DCB71";
    private const string WEB_TAG_COLOR = "#43A6F5";
    private const string CLOUD_TAG_COLOR = "#5C6CC0";
    private const string FIREBASE_TAG_COLOR = "#FFA827";
    private const string DESIGN_TAG_COLOR = "#EC407B";
    private const string GENERAL_TAG_COLOR = "#9E9E9E";
    private string LOGO_BASE_PATH;
    private string _photoUrl;
    private string _tagImageUrl;

    private void SetTimespanText(string timespanText)
    {
      _timespanText.text = timespanText;
    }
    
    private string GetTimespanText(string startTime, string endTime)
    {
      int startHour = Convert.ToInt32(startTime.Split(':')[0]);
      int endHour = Convert.ToInt32(endTime.Split(':')[0]);
      int startMinute = Convert.ToInt32(startTime.Split(':')[1]);
      int endMinute = Convert.ToInt32(endTime.Split(':')[1]);

      int hourSpan = endHour - startHour;
      int minuteSpan = endMinute - startMinute;

      if (minuteSpan < 0)
      {
        hourSpan--;
        minuteSpan = 60 + minuteSpan;
      }

      string timespanText = "";

      if (hourSpan == 1) timespanText += "1 hour";
      if (hourSpan > 1) timespanText += hourSpan + " hours";
      if (timespanText != "") timespanText += " ";
      if(minuteSpan > 0) timespanText += minuteSpan + " mins";

      return timespanText;
    }
    
    private void SetSpeakerPhoto(Texture2D photo)
    {
      _speakerPhoto.texture = photo;
    }
    
    private void SetStartTime(string startTimeText)
    {
      _startTimeHoursText.text = startTimeText.Split(':')[0];
      _startTimeMinutesText.text = startTimeText.Split(':')[1];
    }

    private void SetSpeakerCompanyCountry(string company, string country)
    {
      _speakerCompanyCountryText.text = company + " / " + country;
    }

    private void SetSpeakerName(string speakerNameText)
    {
      _speakerNameText.text = speakerNameText;
    }

    private void SetName(string nameText)
    {
      _nameText.text = nameText;
    }

    private void SetTag(string tag)
    {
      SetTagText(tag);
      SetTagImageColor(tag);
    }
    
    private void SetTagText(string tag)
    {
      _tagText.text = tag;
    }

    private void SetSpeakerImageVisible(bool visible)
    {
      _speakerPhoto.gameObject.SetActive(visible);
    }
    
    private void SetTagImageColor(string tag)
    {
      Color newColor;

      switch (tag)
      {
        case "Android":
          if (ColorUtility.TryParseHtmlString(ANDROID_TAG_COLOR, out newColor))
            _tagImage.color = newColor;
          break;
        case "Cloud":
          if (ColorUtility.TryParseHtmlString(CLOUD_TAG_COLOR, out newColor))
            _tagImage.color = newColor;
          break;
        case "Web":
          if (ColorUtility.TryParseHtmlString(WEB_TAG_COLOR, out newColor))
            _tagImage.color = newColor;
          break;
        case "Firebase":
          ColorUtility.TryParseHtmlString(FIREBASE_TAG_COLOR, out newColor);
          _tagImage.color = newColor;
          break;
        case "Design":
          ColorUtility.TryParseHtmlString(DESIGN_TAG_COLOR, out newColor);
          _tagImage.color = newColor;
          break;
        default:
          ColorUtility.TryParseHtmlString(GENERAL_TAG_COLOR, out newColor);
          _tagImage.color = newColor;
          SetSpeakerImageVisible(false);
          break;
      }
    }
    
    private void LoadImage(string logoUrl, RawImage image)
    {
      string filePath = GetFilePathFromUrl(logoUrl);
      
      if (LoadFromFile(filePath, image)) return;
      
      WWW req = new WWW(logoUrl);
      StartCoroutine(OnResponse(req, filePath, image));
    }

    private IEnumerator OnResponse(WWW req, string filePath, RawImage image)
    {
      yield return req;

      image.texture = req.texture;
      //SetImageTexture(image, req.bytes);
      SaveLogoToFile(filePath, req.bytes);
    }
    
    private bool LoadFromFile(string fileName, RawImage image)
    {
      var filePath = LOGO_BASE_PATH + fileName;
      
      if (!File.Exists(filePath)) return false;
      
      var fileData = File.ReadAllBytes(filePath);
      SetImageTexture(image, fileData);
      return true;
    }

    private void SetImageTexture(RawImage image, byte[] data)
    {
      var texture2D = new Texture2D(0, 0, TextureFormat.BGRA32, false);
      texture2D.LoadImage(data);
      image.texture = texture2D;
    }
    
    private void SaveLogoToFile(string fileName, byte[] logoBytes)
    {
      var filePath = LOGO_BASE_PATH + fileName;
      var directoryName = Path.GetDirectoryName(filePath);

      if (directoryName == null) return;
      
      if (!Directory.Exists(directoryName))
      {
        Directory.CreateDirectory(directoryName);
      }
      
      File.WriteAllBytes(filePath, logoBytes);
    }

    private string GetFilePathFromUrl(string url)
    {
      if(!url.Contains('%')) return url.Split('/').Last();

      return url.Split('%').First(x => x.Contains("?")).Split('?').First(y => y.Contains(".jpg"));
    }
  }
}