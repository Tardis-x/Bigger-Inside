using UnityEngine;

namespace ua.org.gdg.devfest
{
    public class ARCorePanelScript : MonoBehaviour
    {

        //---------------------------------------------------------------------
        // Editor
        //---------------------------------------------------------------------

        [Header("Events")] 
        [SerializeField] private GameEvent _onCountdownStart;
        [SerializeField] private GameEvent _onHit;
        [SerializeField] private GameEvent _onAnswer;


        //---------------------------------------------------------------------
        // Public
        //---------------------------------------------------------------------

        public void OnButtonPlayClick()
        {
            Debug.Log("BUTTON PLAY PRESSED");
            _onCountdownStart.Raise();
        }

        public void OnButtonHitClick()
        {
            Debug.Log("BUTTON HIT PRESSED");
            _onHit.Raise();
        }

        public void OnButtonAnswerClick()
        {
            Debug.Log("BUTTON ANSWER PRESSED");
            _onAnswer.Raise();
        }
    }
}