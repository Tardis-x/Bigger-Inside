using UnityEngine;
using UnityEngine.EventSystems;

namespace ua.org.gdg.devfest
{
  public class DragAndDropTower : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private GameObject _towerPrefab;
    [SerializeField] private int _ghostTowerScaleFactor;

    [Space]
    [Header("Events")] 
    [SerializeField] private GameEvent _onBeginDrag;
    [SerializeField] private GameEvent _onEndDrag;
    [SerializeField] private IntGameEvent _moneyChangedForAmount;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private GameObject _hoverPrefab;
    private GameObject _activeSlot;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      _hoverPrefab = Instantiate(_towerPrefab);
      _hoverPrefab.transform.localScale = Vector3.one * _ghostTowerScaleFactor;
      _hoverPrefab.SetActive(false);
    }


    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
      if (!Interactable) return;
      
      Debug.Log("OnBeginDrag raised");
      _onBeginDrag.Raise();
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (!Interactable) return;
      
      var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      var hits = Physics.RaycastAll(ray, 50f);

      if (hits != null && hits.Length > 0)
      {
        MaybeShowHoverPrefab(hits);

        var slotIndex = GetSlotIndex(hits);
        if (slotIndex != -1)
        {
          var slotQuad = hits[slotIndex].collider.gameObject;
          _activeSlot = slotQuad;
          ChangeMaterialColor(true);
        }
        else
        {
          ChangeMaterialColor(false);
          _activeSlot = null;
        }
      }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      if (!Interactable) return;
      
      if (_activeSlot != null)
      {
        Debug.Log(string.Format("Active slot pos: x - {0}, y - {1}, z - {2}", _activeSlot.transform.position.x, 
          _activeSlot.transform.position.y, _activeSlot.transform.position.z));
        var quadCentre = GetQuadCentre(_activeSlot);
        Debug.Log(string.Format("Quad center pos: x - {0}, y - {1}, z - {2}", quadCentre.x, 
          quadCentre.y, quadCentre.z));
        var tower = Instantiate(_towerPrefab, quadCentre, Quaternion.identity, _activeSlot.transform.parent.transform);
        Debug.Log("Instantiated tower");
        tower.GetComponent<CapsuleCollider>().enabled = true;
        tower.GetComponent<TowerScript>().Slot = _activeSlot;
        var aoeTower = tower.GetComponent<AOETowerScript>();
        if(aoeTower != null) aoeTower.SetAOEVisible(true);
        _activeSlot.SetActive(false);
        // _moneyChangedForAmount.Raise(-_towerPrefab.GetComponent<TowerScript>().Cost);
      }

      _hoverPrefab.SetActive(false);
      Debug.Log("OnEndDrag raised");
      _onEndDrag.Raise();
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------


    private void ChangeMaterialColor(bool value)
    {
      var meshRenderers = _hoverPrefab.GetComponentsInChildren<MeshRenderer>();
      
      for (var i = 0; i < meshRenderers.Length; i++)
      {
        meshRenderers[i].material.color = value ? Color.green : Color.red;
      }
    }

    private void MaybeShowHoverPrefab(RaycastHit[] hits)
    {
      var terrainCollderQuadIndex = GetTerrainColliderQuadIndex(hits);
      if (terrainCollderQuadIndex != -1)
      {
        _hoverPrefab.transform.position = hits[terrainCollderQuadIndex].point;
        _hoverPrefab.SetActive(true);
      }
      else
      {
        _hoverPrefab.SetActive(false);
      }
    }

    private int GetTerrainColliderQuadIndex(RaycastHit[] hits)
    {
      for (var i = 0; i < hits.Length; i++)
      {
        if (hits[i].collider.gameObject.name.Equals("Environment"))
        {
          return i;
        }
      }

      return -1;
    }

    private int GetSlotIndex(RaycastHit[] hits)
    {
      for (var i = 0; i < hits.Length; i++)
      {
        if (hits[i].collider.gameObject.name.StartsWith("Slot"))
        {
          return i;
        }
      }

      return -1;
    }


    private Vector3 GetQuadCentre(GameObject quad)
    {
      var offsetY = -0.05f;
      var meshVerts = quad.GetComponent<MeshFilter>().mesh.vertices;
      var vertRealWorldPositions = new Vector3[meshVerts.Length];

      for (int i = 0; i < meshVerts.Length; i++)
      {
        vertRealWorldPositions[i] = quad.transform.TransformPoint(meshVerts[i]);
      }

      var midPoint = Vector3.Slerp(vertRealWorldPositions[0], vertRealWorldPositions[1], 0.5f);
      Debug.Log(string.Format("Midpoint y: {0}", midPoint.y));
      midPoint.y += offsetY;
      
      return midPoint;
    }
    
    //---------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------
    
    public bool Interactable { get; set; }
  }
}