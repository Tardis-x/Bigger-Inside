using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class DescriptionPanelScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private Text _titleText;
    [SerializeField] private Text _detailsText;
    [SerializeField] private Text _tagText;
    [SerializeField] private Image _tagBorder;
    [SerializeField] private Text _descriptionText;
    [SerializeField] private RawImage _speakerPhotoImage;
    [SerializeField] private Text _speakerNameText;
    [SerializeField] private Text _speakerCompanyCountryText;
    [SerializeField] private RectTransform _speakerInfoGroup;

    [Space] 
    [Header("Speaker 2")]
    [SerializeField] private GameObject _speaker2;
    [SerializeField] private Text _speakerNameText2;
    [SerializeField] private Text _speakerCompanyCountryText2;
    [SerializeField] private RawImage _speakerPhotoImage2;
    
    [Space] 
    [Header("Tag 2")]
    [SerializeField] private GameObject _tag2;
    //---------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------
    
    public bool Active { get; private set; }

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      LOGO_BASE_PATH = Application.persistentDataPath + "Graphics/";
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape)) SetActive(false);
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void SetActive(bool value)
    {
      GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1;
      gameObject.SetActive(value);
      Active = value;
    }

    public void DisablePanel()
    {
      GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1;
      gameObject.SetActive(false);
      Active = false;
    }

    public void SetData(ScheduleItemDescriptionUiModel model)
    {
      if (model.Speakers != null)
      {
        SetSpeakerData(model.Speakers);
        ShowSpeakerData(true);
      }
      else
      {
        ShowSpeakerData(false);
      }
      
      _titleText.text = model.Title;
      _descriptionText.text = model.Description;
      _detailsText.text = ComposeDetailsText(model.DateReadable, model.StartTime, model.EndTime, model.Hall,
        model.Language, model.Complexity);
      _tagText.text = model.MainTag;
      SetTagColor(model.TagColor);
      ShowTag(model.MainTag != "General", model.Tags != null && model.Tags.Length > 1);
    }

    public void OnBackButtonClick()
    {
      SetActive(false);
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private string LOGO_BASE_PATH;

    private void ShowSpeakerData(bool value)
    {
      _speakerInfoGroup.gameObject.SetActive(value);
    }

    private void SetSpeakerData(Speaker[] speakers)
    {
      LoadImage(speakers[0].PhotoUrl, _speakerPhotoImage);
      _speakerNameText.text = speakers[0].Name;
      _speakerCompanyCountryText.text = speakers[0].Company + " / " + speakers[0].Country;

      if (speakers.Length < 2) return;
      
      _speaker2.SetActive(true);
      LoadImage(speakers[1].PhotoUrl, _speakerPhotoImage2);
      _speakerNameText2.text = speakers[1].Name;
      _speakerCompanyCountryText2.text = speakers[1].Company + " / " + speakers[1].Country;
    }

    private void ShowTag(bool tag1, bool tag2)
    {
      _tag2.SetActive(tag2);
      _tagBorder.gameObject.SetActive(tag1);
    }

    private void SetTagColor(string color)
    {
      Color newColor;

      if (ColorUtility.TryParseHtmlString(color, out newColor))
      {
        _tagText.color = newColor;
        _tagBorder.color = newColor;
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
      if (!url.Contains('%')) return url.Split('/').Last();

      return url.Split('%').First(x => x.Contains("?")).Split('?').First(y => y.Contains(".jpg"));
    }

    private string ComposeDetailsText(string date, string startTime, string endTime, string hall, string language,
      string complexity)
    {
      string details = date + ", " + startTime + " - " + endTime + "\n" + hall;

      if(!string.IsNullOrEmpty(complexity)) details += "\nContent level: " + complexity;

      return details;
    }
  }
}