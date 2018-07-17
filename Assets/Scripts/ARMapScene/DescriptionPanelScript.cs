using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class DescriptionPanelScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private RawImage _headerBackgroundImage;
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _detailsText;
    [SerializeField] private Text _tagText;
    [SerializeField] private Text _descriptionText;
    [SerializeField] private RawImage _speakerPhotoImage;
    [SerializeField] private Text _speakerNameText;
    [SerializeField] private Text _speakerCompanyCountryText;
    [SerializeField] private RectTransform _speakerInfoGroup;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      LOGO_BASE_PATH = Application.persistentDataPath + "Graphics/Logo";
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape)) SetActive(false);
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public bool Active { get; private set; }

    public void SetActive(bool value)
    {
      gameObject.SetActive(value);
      Active = value;
    }

    public void SetData(ScheduleItemDescriptionUiModel model)
    {
      if (model.Speaker != null)
      {
        LoadImage(model.Speaker.PhotoUrl, _speakerPhotoImage);
        _speakerNameText.text = model.Speaker.Name;
        _speakerCompanyCountryText.text = model.Speaker.Company + ", " + model.Speaker.Country;
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
      _tagText.text = model.Tag;
      SetBackgroundColor(model.TagColor);
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private string LOGO_BASE_PATH;

    private void ShowSpeakerData(bool value)
    {
      _speakerInfoGroup.gameObject.SetActive(value);
    }
    
    private void SetBackgroundColor(string color)
    {
      Color newColor;

      if (ColorUtility.TryParseHtmlString(color, out newColor))
        _headerBackgroundImage.color = newColor;
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
      string details = date + " / " + startTime + " - " + endTime + " / " + hall;

      if (language != "") details += " / " + language;
      if(complexity != "") details += " / " + complexity;

      return details;
    }
  }
}