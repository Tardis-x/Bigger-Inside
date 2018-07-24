using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DeadMosquito.AndroidGoodies;

public class QuestUI : MonoBehaviour
{
    [SerializeField]
    GameObject _menuPanel;

    [SerializeField]
    GameObject _photoPanel;
    [SerializeField]
    GameObject _vrGamePanel;
    [SerializeField]
    GameObject _riddlesPanel;

    GameObject _activePanel;

    QuestManager _questManager;
    
    void Awake()
    {
        Debug.Log("QuestUI.Awake()");
        
        // obtain reference to object that represents quest manager
        QuestManagerReferenceInitialization();
        
        // show quest menu screen
        _menuPanel.SetActive(true);

        _activePanel = _menuPanel;
        
        // hide quest screens with tasks
        _photoPanel.SetActive(false);
        _vrGamePanel.SetActive(false);
        _riddlesPanel.SetActive(false);
    }

    void QuestManagerReferenceInitialization()
    {
        GameObject questManagerTemp = GameObject.Find("QuestManager");

        if (questManagerTemp != null)
        {
            _questManager = questManagerTemp.GetComponent<QuestManager>();

            if (_questManager == null)
            {
                Debug.LogError("Could not locate QuestManager component on " + questManagerTemp.name);
            }
        }
        else
        {
            Debug.LogError("Could not locate quest manager object in current scene!");
        }
    }

    public void OnMainMenuButtonClicked()
    {
        Debug.Log("QuestUI.OnMainMenuButtonClicked");
        
        SceneManager.LoadScene("MenuScene");
    }

    public void OnPhotoButtonClicked()
    {
        Debug.Log("QuestUI.OnPhotoButtonClicked");
        
        _menuPanel.SetActive(false);
        _activePanel = _photoPanel;
        _photoPanel.SetActive(true);
    }
    
    public void OnVrGameButtonClicked()
    {
        Debug.Log("QuestUI.OnVrGameButtonClicked");
        
        _menuPanel.SetActive(false);
        _activePanel = _vrGamePanel;
        _vrGamePanel.SetActive(true);
    }
    
    public void OnRiddlesButtonClicked()
    {
        Debug.Log("QuestUI.OnRiddlesButtonClicked");
        
        _menuPanel.SetActive(false);
        _activePanel = _riddlesPanel;
        _riddlesPanel.SetActive(true);
    }

    public void OnBackButtonClicked()
    {
        Debug.Log("QuestUI.OnBackButtonClicked");
        
        _activePanel.SetActive(false);
        _activePanel = _menuPanel;
        _menuPanel.SetActive(true);
    }
}
