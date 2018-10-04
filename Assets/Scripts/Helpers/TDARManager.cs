using UnityEngine;
using UnityEngine.UI;
using Vuforia;

namespace ua.org.gdg.devfest
{
	public class TDARManager : MonoBehaviour
	{
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Path to resources")]
    [SerializeField] private string _prefabPath;
    
    [Space]
    [Header("Targets")]
    [SerializeField] private GameObject _imageTarget;
	  [SerializeField] private GameObject _groundPlaneTarget;
    [SerializeField] private GameObject _planeFinder;

    [Space] 
    [Header("UI")] 
    [SerializeField] private Text _hint;
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private GameObject _environmentPrefab;

    private bool _arCoreSupport;
    
    //---------------------------------------------------------------------
    // Events
    //---------------------------------------------------------------------

    public void OnTrackingLost()
    {
      ShowHint(_arCoreSupport);
      ShowHint("Aim camera at horizontal surface");
      _planeFinder.SetActive(_arCoreSupport);
    }

    public void OnTrackingFound()
    {
      _planeFinder.SetActive(false);
      ShowHint(false);
    }
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      _arCoreSupport = ARCoreHelper.CheckArCoreSupport();
      LoadResources();
    }

    private void Start()
    {
      PrepareScene(_arCoreSupport);
    }
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void OnContentPlaced(GameObject anchor)
    {
      _planeFinder.SetActive(false);
      ShowHint(false);
    }

    public void OnAutomaticHitTest(HitTestResult hitTestResult)
    {
      if (hitTestResult == null)
      {
        ShowHint("Aim camera at horizontal surface");
        return;
      }
      
      ShowHint("Tap anywhere on surface");
    }
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
    
    private void LoadResources()
    {
      _environmentPrefab = Resources.Load<GameObject>(_prefabPath);
    }
    
    private void PrepareScene(bool arCoreSupport)
    {
      _planeFinder.gameObject.SetActive(arCoreSupport);
      _groundPlaneTarget.gameObject.SetActive(arCoreSupport);
      _imageTarget.SetActive(!arCoreSupport);
      
      ShowHint(arCoreSupport);

      Instantiate(_environmentPrefab, arCoreSupport ? _groundPlaneTarget.transform : _imageTarget.transform);
    }
    
    private void ShowHint(bool value)
    {
      _hint.gameObject.SetActive(value);
    }

	  private void ShowHint(string hintText)
	  {
	    _hint.gameObject.SetActive(true);
	    _hint.text = hintText;
	  }
	}
}