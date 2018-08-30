using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class SpeechScript : InteractableObject
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
      if(_model.Speaker != null) LoadImage(_model.Speaker.PhotoUrl, _speakerPhoto);
      if(_model.ImageUrl != null) LoadImage(_model.ImageUrl, _tagImage);
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public SpeechScript GetInstance(ScheduleItemUiModel model)
    {
      SpeechScript instance = Instantiate(this);
      instance.SetName(model.Title);
      instance.SetStartTime(model.StartTime);
      instance.SetTimespanText(GetTimespanText(model.StartTime, model.EndTime));
      
      if (model.Speaker != null) instance.SetSpeakerData(model.Speaker);
      
      instance._description = model.Description;
      instance.SetTag(model.Tag);
      instance._model = model;
      return instance;
    }
    
    public override void Interact()
    {
      PanelManager.Instance.SpeechDescriptionPanel.SetActive(true);
      PanelManager.Instance.SpeechDescriptionPanel.SetData(_description);
    }

    public override void Disable()
    {
      Destroy(_tagImage);
      Destroy(_speakerPhoto);
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
    private ScheduleItemDescriptionUiModel _description;
    private ScheduleItemUiModel _model;
    

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
      //SetTagImageColor(tag);
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
          if (ColorUtility.TryParseHtmlString(ANDROID_TAG_COLOR, out newColor))
            _tagImage.color = newColor;
          break;
        case "Cloud":
          _description.TagColor = CLOUD_TAG_COLOR;
          if (ColorUtility.TryParseHtmlString(CLOUD_TAG_COLOR, out newColor))
            _tagImage.color = newColor;
          break;
        case "Web":
          _description.TagColor = WEB_TAG_COLOR;
          if (ColorUtility.TryParseHtmlString(WEB_TAG_COLOR, out newColor))
            _tagImage.color = newColor;
          break;
        case "Firebase":
          _description.TagColor = FIREBASE_TAG_COLOR;
          ColorUtility.TryParseHtmlString(FIREBASE_TAG_COLOR, out newColor);
          _tagImage.color = newColor;
          break;
        case "Design":
          _description.TagColor = DESIGN_TAG_COLOR;
          ColorUtility.TryParseHtmlString(DESIGN_TAG_COLOR, out newColor);
          _tagImage.color = newColor;
          break;
        default:
          _description.TagColor = GENERAL_TAG_COLOR;
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