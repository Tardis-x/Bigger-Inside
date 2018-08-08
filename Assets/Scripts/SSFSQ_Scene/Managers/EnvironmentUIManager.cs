using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
    public class EnvironmentUIManager : MonoBehaviour
    {
        private Coroutine _countdownCoroutine;
        
        //---------------------------------------------------------------------
        // Editor
        //---------------------------------------------------------------------
        [Header("Variables")]
        [SerializeField] private IntVariable _score;
        [SerializeField] private IntVariable _timeLeft;

        [Space]
        [Header("UI")]
        [SerializeField] private GameOverPanelScript _gameOverPanel;
        [SerializeField] private Text _screenQuestionText;
        [SerializeField] private Text _getReadyText;

        [Space]
        [Header("Events")]
        [SerializeField] private GameEvent _onGameStart;
        
        //---------------------------------------------------------------------
        // Events
        //---------------------------------------------------------------------

        public void OnCountdownStart()
        {
            Debug.Log("Environment: OnCountDownStart");
            GetReadyTextSetActive(true);
            ScreenQuestionTextSetActive(false);
            _gameOverPanel.HidePanel();
            _timeLeft.ResetValue();
            _countdownCoroutine = StartCoroutine(GetReadyCountDown());
        }

        //---------------------------------------------------------------------
        // Internal
        //---------------------------------------------------------------------

        private void GetReadyTextSetActive(bool value)
        {
            _getReadyText.gameObject.SetActive(value);
        }
    
        private void ScreenQuestionTextSetActive(bool value)
        {
            _screenQuestionText.gameObject.SetActive(value);
        }
        
        private IEnumerator GetReadyCountDown()
        {
            while (_timeLeft.RuntimeValue >= -1)
            {
                if (_timeLeft.RuntimeValue > 0)
                {
                    _getReadyText.text = "GET READY!\n" + _timeLeft.RuntimeValue;
                }
                else if (_timeLeft.RuntimeValue == 0)
                {
                    _getReadyText.text = "GET READY!\nGO!";
                }
                else
                {
                    EndGetReadyCountdown();
                }
        
                yield return new WaitForSeconds(1);
                _timeLeft.RuntimeValue--;
            }
        }
        
        private void EndGetReadyCountdown()
        {
            ScreenQuestionTextSetActive(true);
            GetReadyTextSetActive(false);
            StopCoroutine(_countdownCoroutine);
            Debug.Log("RAISING OnGameStart");
            _onGameStart.Raise();
        }
    }
}
