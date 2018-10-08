using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class SpeechItemScript : MonoBehaviour
  {
    private const string ANDROID_TAG_COLOR = "#3CC23E";
    private const string WEB_TAG_COLOR = "#2196F3";
    private const string CLOUD_TAG_COLOR = "#3F51B5";
    private const string FIREBASE_TAG_COLOR = "#FFA827";
    private const string DESIGN_TAG_COLOR = "#EC407B";
    private const string GENERAL_TAG_COLOR = "#9E9E9E";
    private string LOGO_BASE_PATH;

    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("UI")]
    [SerializeField] private Text _speakerCompanyCountryText;
    [SerializeField] private Text _speakerNameText;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _tagText;
    [SerializeField] private Image _tagBorder;
    [SerializeField] private RawImage _speakerPhoto;
    [SerializeField] private Image _speakerPhotoCircle;
    [SerializeField] private Text _timespanText;
    [SerializeField] private Text _complexityText;
    
    [Space]
    [Header("Speaker 2")]
    [SerializeField] private GameObject _speakerData2;
    [SerializeField] private Text _speakerCompanyCountryText2;
    [SerializeField] private Text _speakerNameText2;
    [SerializeField] private RawImage _speakerPhoto2;
    
    [Space]
    [Header("Tag 2")]
    [SerializeField] private GameObject _tag2;

    //---------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------

    public bool General { get; private set; }
    
    public string MainTag
    {
      get { return _model.MainTag; }
    }

    public string[] Tags
    {
      get { return _model.Tags; }
    }

    public bool ImageLoaded
    {
      get
      {
        if(_model.Speakers == null) return false;
        return _model.Speakers.Length < 2 ? _image1Loaded : _image1Loaded && _image2Loaded;
      }
      private set
      {
        if (!_image1Loaded)
          _image1Loaded = value;
        else _image2Loaded = value;
      }
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private bool _image1Loaded, _image2Loaded;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      LOGO_BASE_PATH = Application.persistentDataPath + "Graphics/";
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public SpeechItemScript GetInstance(SpeechItemModel model)
    {
      var instance = Instantiate(this);
      instance._model = model;
      instance.SetName(model.Title);
      instance._timespanText.text = model.Timespan;

      if (model.Speakers != null) instance.SetSpeakerData(model.Speakers);

      instance._description = model.Description;
      instance.SetComplexityText(model.Description.Complexity ?? "");
      instance.SetTag(model.MainTag);
      if(model.Tags != null) instance._tag2.SetActive(model.Tags.Length > 1);
      instance.gameObject.SetActive(false);
      
      return instance;
    }

    public ScheduleItemDescriptionUiModel GetDescription()
    {
      return _description;
    }

    public void LoadSpeakerPhoto()
    {
      if(_model.Speakers == null) return;
      
      LoadImage(_model.Speakers[0].PhotoUrl, _speakerPhoto);
      
      if(_model.Speakers.Length < 2) return;
      
      LoadImage(_model.Speakers[1].PhotoUrl, _speakerPhoto2);
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private ScheduleItemDescriptionUiModel _description;
    private SpeechItemModel _model;

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void SetGeneralSpeechItem(bool value)
    {
      _tagBorder.gameObject.SetActive(!value);
    }

    private void SetSpeakerData(Speaker[] speakers)
    {
      if (speakers.Length < 2)
      {
        SetSpeakerCompanyCountry(speakers[0].Company, speakers[0].Country);
        SetSpeakerName(speakers[0].Name);
      }
      else
      {
        _speakerData2.SetActive(true);
        SetSpeakerCompanyCountry(speakers[0].Company, speakers[0].Country, speakers[1].Company, speakers[1].Country);
        SetSpeakerName(speakers[0].Name, speakers[1].Name);
      }
    }

    private void SetComplexityText(string complexity)
    {
      if(_complexityText == null) return;
      
      _complexityText.text = complexity;
    }

    private void SetSpeakerCompanyCountry(string company, string country)
    {
      _speakerCompanyCountryText.text = company + " / " + country;
    }
    
    private void SetSpeakerCompanyCountry(string company, string country, string company2, string country2)
    {
      _speakerCompanyCountryText.text = company + " / " + country;
      _speakerCompanyCountryText2.text = company2 + " / " + country2;
    }

    private void SetSpeakerName(string speakerNameText)
    {
      _speakerNameText.text = speakerNameText;
    }
    
    private void SetSpeakerName(string speakerNameText, string speakerNameText2)
    {
      _speakerNameText.text = speakerNameText;
      _speakerNameText2.text = speakerNameText2;
    }

    private void SetName(string nameText)
    {
      _nameText.text = nameText + "\n";
    }

    private void SetTag(string speechTag)
    {
      if(_tagText == null) return;
      
      SetTagText(speechTag);
      SetTagImageColor(speechTag);
      SetGeneralSpeechItem(speechTag == "General");
      General = speechTag == "General";
    }

    private void SetTagText(string speechTag)
    {
      _tagText.text = speechTag;
    }

    private void SetSpeakerImageVisible(bool visible)
    {
      _speakerPhoto.gameObject.SetActive(visible);
      _speakerPhotoCircle.gameObject.SetActive(visible);
    }

    private void SetTagImageColor(string speechTag)
    {
      switch (speechTag)
      {
        case "Mobile":
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
        case "General":
          _description.TagColor = GENERAL_TAG_COLOR;
          SetTagTextAndBorderColor(GENERAL_TAG_COLOR);
          SetSpeakerImageVisible(false);
          break;
        default:
          _description.TagColor = "#FFFFFF";
          SetTagTextAndBorderColor("#FFFFFF");
          _model.MainTag = "Other";
          break;
      }
    }

    private void SetTagTextAndBorderColor(string color)
    {
      Color newColor;

      if (!ColorUtility.TryParseHtmlString(color, out newColor)) return;

      _tagBorder.color = newColor;
      _tagText.color = newColor;
    }

    private void LoadImage(string logoUrl, RawImage image)
    {
      var filePath = GetFilePathFromUrl(logoUrl);

      if (LoadFromFile(filePath, image))
      {
        ImageLoaded = true;
        return;
      }

      var req = new WWW(logoUrl);
      if(gameObject.activeSelf) StartCoroutine(OnResponse(req, filePath, image));
    }

    private IEnumerator OnResponse(WWW req, string filePath, RawImage image)
    {
      yield return req;
      
      SetImageTexture(image, req.bytes);
      SaveLogoToFile(filePath, req.bytes);
      ImageLoaded = true;
    }

    private bool LoadFromFile(string fileName, RawImage image)
    {
      var filePath = LOGO_BASE_PATH + fileName;

      if (!File.Exists(filePath)) return false;

     if(new FileInfo(filePath).Length < 1) return false;
      
      var fileData = File.ReadAllBytes(filePath);
      SetImageTexture(image, fileData);
      return true;
    }

    private static void SetImageTexture(RawImage image, byte[] data)
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

    private static string GetFilePathFromUrl(string url)
    {
      return !url.Contains('%')
        ? url.Split('/').Last()
        : url.Split('%').First(x => x.Contains("?")).Split('?').First(y => y.Contains(".jpg"));
    }
  }
}