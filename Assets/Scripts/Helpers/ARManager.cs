using UnityEngine;
using UnityEngine.UI;
using Vuforia;

namespace ua.org.gdg.devfest
{
  public class ARManager : MonoBehaviour
  {

    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Path to resources")]
    [SerializeField] private string _arCorePrefabPath;
    [SerializeField] private string _imageTargetPrefabPath;
    
    [Space]
    [Header("Targets")]
    [SerializeField] private GameObject _imageTarget;
    [SerializeField] private GameObject _planeFinder;

    [Space] 
    [Header("UI")] 
    [SerializeField] private Text _hint;
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private GameObject _imageTargetEnvironment;

    private bool _arCoreSupport;
    
    //---------------------------------------------------------------------
    // Events
    //---------------------------------------------------------------------

    public void OnTrackingLost()
    {
      ShowHint(_arCoreSupport);
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
      LoadResources(_arCoreSupport);
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
        _hint.gameObject.SetActive(true);
      }
      
      _hint.gameObject.SetActive(false);
    }
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
    
    private void LoadResources(bool arCoreSupport)
    {
      if (arCoreSupport)
      {
        _planeFinder.GetComponent<ContentPositioningBehaviour>().AnchorStage =
          Resources.Load<AnchorBehaviour>(_arCorePrefabPath);
      }
      else
      {
        _imageTargetEnvironment = Resources.Load<GameObject>(_imageTargetPrefabPath);
      }
    }
    
    private void PrepareScene(bool arCoreSupport)
    {
      _planeFinder.gameObject.SetActive(arCoreSupport);
      _imageTarget.SetActive(!arCoreSupport);
      
      ShowHint(arCoreSupport);
      
      if (!arCoreSupport)
      {
        Instantiate(_imageTargetEnvironment, _imageTarget.transform);
      }
    }
    
    private void ShowHint(bool value)
    {
      _hint.gameObject.SetActive(value);
    }
  }
}