using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
    public class ScenesPanelScript : MonoBehaviour
    {

        //---------------------------------------------------------------------
        // Editor
        //---------------------------------------------------------------------

        [Header("UI")] 
        [SerializeField] private Button _openScenesMenuButton;
        [SerializeField] private Button _closeScenesMenuButton;
        [SerializeField] private Button _saveSpeakerSceneButton;
        [SerializeField] private Button _towerDefenceSceneButton;

        //---------------------------------------------------------------------
        // Public
        //---------------------------------------------------------------------

        public void OpenScenesMenu()
        {
            ShowOpenMenuButton(false);
            ShowMenu(true);
        }

        public void CloseScenesMenu()
        {
            ShowMenu(false);
            ShowOpenMenuButton(true);
        }

        public void OpenSaveSpeakerScene()
        {
            SceneManager.LoadScene(Scenes.SCENE_SSFSQ);
        }

        public void OpenTowerDefenceScene()
        {
            SceneManager.LoadScene(Scenes.SCENE_TD);
        }

        //---------------------------------------------------------------------
        // Helpers
        //---------------------------------------------------------------------

        private void ShowOpenMenuButton(bool value)
        {
            _openScenesMenuButton.gameObject.SetActive(value);
        }

        private void ShowMenu(bool value)
        {
            _closeScenesMenuButton.gameObject.SetActive(value);
            _saveSpeakerSceneButton.gameObject.SetActive(value);
            _towerDefenceSceneButton.gameObject.SetActive(value);
        }
    }
}