using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
    public class Environment : MonoBehaviour
    {
        //---------------------------------------------------------------------
        // Editor
        //---------------------------------------------------------------------

        [Header("UI")] 
        [SerializeField] private GameOverPanelScript _gameOverPanel;
        [SerializeField] private Text _screenQuestionText;
        [SerializeField] private Text _getReadyText;

        [Space] 
        [Header("Animation")] 
        [SerializeField] private CrowdControlScript _crowdControl;
        [SerializeField] private SpeakerAnimationScript _speakerAnimation;
        [SerializeField] private BoxingGloveScript _boxingGlove;
        [SerializeField] private ParticleSystem _tomatoes;

        [Space]
        [Header("Events")]
        [SerializeField] private GameEvent _onInstantiated;

        //---------------------------------------------------------------------
        // Property
        //---------------------------------------------------------------------

        public GameOverPanelScript GameOverPanel
        {
            get { return _gameOverPanel; }
        }

        public Text ScreenQuestionText
        {
            get { return _screenQuestionText; }
        }

        public Text GetReadyText
        {
            get { return _getReadyText; }
        }

        public CrowdControlScript CrowdControl
        {
            get { return _crowdControl; }
        }

        public SpeakerAnimationScript SpeakerAnimation
        {
            get { return _speakerAnimation; }
        }

        public BoxingGloveScript BoxingGlove
        {
            get { return _boxingGlove; }
        }

        public ParticleSystem Tomatoes
        {
            get { return _tomatoes; }
        }

        //---------------------------------------------------------------------
        // Messages
        //---------------------------------------------------------------------

        private void Start()
        {
            _onInstantiated.Raise();
        }
    }
}
