using System.Collections;
using System.Collections.Generic;
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

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      SetActive(false);
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------
    
    public void SetActive(bool value)
    {
      gameObject.SetActive(value);
    }

    public void SetData(SessionItem session, Texture speakerPhoto, string speakerName, string speakerCompanyCountry,
      string date, string startTime, string endTime, string hall, Texture tagImage)
    {
      _speakerPhotoImage.texture = speakerPhoto;
      _speakerNameText.text = speakerName;
      _speakerCompanyCountryText.text = speakerCompanyCountry;
      _titleText.text = session.Title;
      _descriptionText.text = session.Description;
      _detailsText.text = ComposeDetailsText(date, startTime, endTime, hall, session.Language, session.Complexity);
      _tagText.text = session.Tag;
      _headerBackgroundImage.texture = tagImage;
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
    
    private string ComposeDetailsText(string date, string startTime, string endTime, string hall, string language,
      string complexity)
    {
      return date + " / " + startTime + " - " + endTime + " / " + hall + " / " + language + " / " + complexity;
    }
  }
}