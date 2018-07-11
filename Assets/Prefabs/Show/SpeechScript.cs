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

    [Header("Value References")] [SerializeField]
    private Text _startTimeText;

    [SerializeField] private Text _endTimeText;
    [SerializeField] private Text _nameText;
    [SerializeField] private Image _tagImage;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public RectTransform GetInstance()
    {
      return Instantiate(GetComponent<RectTransform>());
    }

    public RectTransform GetInstance(string startTime, string endTime, string name, string tag)
    {
      SpeechScript instance = Instantiate(this);
      instance.SetName(name);
      instance.SetStartTime(startTime);
      instance.SetEndTime(endTime);
      instance.SetTagImageColor(tag);
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

    private void SetStartTime(string startTimeText)
    {
      _startTimeText.text = startTimeText;
    }

    private void SetEndTime(string endTimeText)
    {
      _endTimeText.text = endTimeText;
    }

    private void SetName(string nameText)
    {
      _nameText.text = nameText;
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