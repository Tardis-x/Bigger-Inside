using UnityEngine;

namespace ua.org.gdg.devfest
{
    public class ARCorePanelScript : MonoBehaviour
    {

        //---------------------------------------------------------------------
        // Editor
        //---------------------------------------------------------------------

        [Header("UI")]
        [SerializeField] private GameObject _buttonPlay;
        [SerializeField] private GameObject _buttonAnswer;
        [SerializeField] private GameObject _buttonHit;


        //---------------------------------------------------------------------
        // Public
        //---------------------------------------------------------------------

        public void ShowPlayButton(bool value)
        {
            _buttonPlay.SetActive(value);
        }

        public void ShowAnswerButton(bool value)
        {
            _buttonAnswer.SetActive(value);
        }
        
        public void ShowHitButton(bool value)
        {
            _buttonHit.SetActive(value);
        }
    }
}