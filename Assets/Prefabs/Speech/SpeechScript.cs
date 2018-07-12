using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Image _tagImage;
    [SerializeField] private RawImage _speakerPhoto;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public RectTransform GetInstance()
    {
      return Instantiate(GetComponent<RectTransform>());
    }

    public RectTransform GetInstance(string startTime, string endTime, string name, string tag, Speaker speaker)
    {
      SpeechScript instance = Instantiate(this);
      instance.SetName(name);
      instance.SetStartTime(startTime);
      
      if (speaker != null)
      {
        instance.SetSpeakerName(speaker.Name);
        instance.SetSpeakerCompanyCountry(speaker.Company, speaker.Country);
        instance.SetSpeakerPhoto(speaker.Photo);
      }
      
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
          break;
      }
    }
  }
}