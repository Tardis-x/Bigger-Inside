using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [SerializeField]
    private Text _titleText;

    [SerializeField]
    private Text _description;

    [SerializeField]
    private GameObject _photoControlsPanel;
    [SerializeField]
    private GameObject _arControlsPanel;
    [SerializeField]
    private GameObject _simpleControlsPanel;

    private QuestManager _questManager;

    private void Awake()
    {
        Debug.Log("QuestUI.Awake()");
        
        // obtain reference to object that represents quest manager
        QuestManagerReferenceInitialization();
    }

    private void QuestManagerReferenceInitialization()
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

    public void UpdateQuestUi(QuestStepData questStepData)
    {
        Debug.Log("QuestUI.UpdateQuestUI()");
        
        // update quest step title
        _titleText.text = String.Format("{0}. {1}", questStepData.number, questStepData.name);

        // update quest step description
        _description.text = questStepData.description;

        // show controls appropriate only for current quest step type
        _photoControlsPanel.SetActive(false);
        _arControlsPanel.SetActive(false);
        _simpleControlsPanel.SetActive(false);
        
        // TODO: consider better option than using switch-statement
        switch (questStepData.type)
        {
            case QuestStepType.Welcome:
            case QuestStepType.Final:
                _simpleControlsPanel.SetActive(true);
                break;
            case QuestStepType.Photo:
                _photoControlsPanel.SetActive(true);
                break;
            case QuestStepType.ArObject:
                _arControlsPanel.SetActive(true);
                break;
        }
    }

    public void OnMenuButtonClicked()
    {
        Debug.Log("QuestUI.OnProgressButtonClicked");
        
        SceneManager.LoadScene("MenuScene");
    }

    public void OnNextButtonClicked()
    {
        Debug.Log("QuestUI.OnNextButtonClicked");
        
        _questManager.CompleteCurrentStep();
    }

    public void OnTakePhotoButtonClicked()
    {
        Debug.Log("QuestUI.OnTakePhotoButtonClicked");
        
        // TODO: Add implementation
    }

    public void OnUploadPhotoButtonClicked()
    {
        Debug.Log("QuestUI.OnUploadPhotoButtonClicked");
        
        // TODO: Add implementation
    }

    public void OnScanButtonClicked()
    {
        Debug.Log("QuestUI.OnScanButtonClicked");
        
        // TODO: Add implementation
    }
}
