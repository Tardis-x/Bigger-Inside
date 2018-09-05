﻿using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class SpeechItemScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Value References")] 
    [SerializeField] private Text _speakerCompanyCountryText;
    [SerializeField] private Text _speakerNameText;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _tagText;
    [SerializeField] private Image _tagBorder;
    [SerializeField] private RawImage _speakerPhoto;
    [SerializeField] private Text _timespanText;
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      LOGO_BASE_PATH = Application.persistentDataPath + "Graphics/Logo";
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public bool General { get; private set; }
    
    public SpeechItemScript GetInstance(SpeechItemModel model)
    {
      SpeechItemScript instance = Instantiate(this);
      instance._model = model;
      instance.SetName(model.Title);
      instance._timespanText.text = model.Timespan;
      
      if (model.Speaker != null) instance.SetSpeakerData(model.Speaker);
      
      instance._description = model.Description;
      instance.SetTag(model.Tag);
      
      return instance;
    }

    public ScheduleItemDescriptionUiModel GetDescription()
    {
      return _description;
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
    
    private const string ANDROID_TAG_COLOR = "#3CC23E";
    private const string WEB_TAG_COLOR = "#2196F3";
    private const string CLOUD_TAG_COLOR = "#3F51B5";
    private const string FIREBASE_TAG_COLOR = "#FFA827";
    private const string DESIGN_TAG_COLOR = "#EC407B";
    private const string GENERAL_TAG_COLOR = "#9E9E9E";
    private string LOGO_BASE_PATH;
    private const int DESCRIPTION_HEIGHT_W_O_SPEAKER = 330;
    private const int DESCRIPTION_HEIGHT_W_SPEAKER = 510;
    private ScheduleItemDescriptionUiModel _description;
    private SpeechItemModel _model;

    private void SetGeneralSpeechItem(bool value)
    {
      var rect = GetComponent<RectTransform>();
     rect.sizeDelta = 
        new Vector2(0, value ? DESCRIPTION_HEIGHT_W_O_SPEAKER : DESCRIPTION_HEIGHT_W_SPEAKER);
      _tagBorder.gameObject.SetActive(!value);
    }

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

    private void SetSpeakerData(Speaker speaker)
    {
      
      SetSpeakerCompanyCountry(speaker.Company, speaker.Country);
      SetSpeakerName(speaker.Name);
      LoadImage(_model.Speaker.PhotoUrl, _speakerPhoto);
    }

    private void SetSpeakerCompanyCountry(string company, string country)
    {
      _speakerCompanyCountryText.text = company + ", " + country;
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
      SetGeneralSpeechItem(tag == "General");
      General = tag == "General";
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
          _description.TagColor = ANDROID_TAG_COLOR;
          SetTagTextAndBorderColor(ANDROID_TAG_COLOR);
          break;
        case "Cloud":
          _description.TagColor = CLOUD_TAG_COLOR;
          SetTagTextAndBorderColor(CLOUD_TAG_COLOR);
          break;
        case "Web":
          _description.TagColor = WEB_TAG_COLOR;
          SetTagTextAndBorderColor(WEB_TAG_COLOR);
          break;
        case "Firebase":
          _description.TagColor = FIREBASE_TAG_COLOR;
          SetTagTextAndBorderColor(FIREBASE_TAG_COLOR);
          break;
        case "Design":
          _description.TagColor = DESIGN_TAG_COLOR;
          SetTagTextAndBorderColor(DESIGN_TAG_COLOR);
          break;
        default:
          _description.TagColor = GENERAL_TAG_COLOR;
          SetTagTextAndBorderColor(GENERAL_TAG_COLOR);
          SetSpeakerImageVisible(false);
          break;
      }
    }

    private void SetTagTextAndBorderColor(string color)
    {
      Color newColor;
      
      if (ColorUtility.TryParseHtmlString(color, out newColor))
      {
        _tagBorder.color = newColor;
        _tagText.color = newColor;
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