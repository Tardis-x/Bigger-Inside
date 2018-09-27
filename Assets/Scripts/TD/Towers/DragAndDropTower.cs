using UnityEngine;
using UnityEngine.EventSystems;

namespace ua.org.gdg.devfest
{
  public class DragAndDropTower : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
  {
    private const int MAX_OFFSET = 200;
    private const int MIN_OFFSET = 150;
  
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private GameObject _towerPrefab;
    [SerializeField] private float _ghostTowerScaleFactor;

    [Space] 
    [Header("Events")] 
    [SerializeField] private GameEvent _onBeginDrag;
    [SerializeField] private GameEvent _onEndDrag;
    [SerializeField] private IntGameEvent _moneyChangedForAmount;
    [SerializeField] private IntGameEvent _audioGameEvent;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private GameObject _hoverPrefab;
    private GameObject _activeSlot;
    private float _offset = 200;
    private float _raycastDistance = 10;

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

      _onBeginDrag.Raise();
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (!Interactable) return;

      var ray = Camera.main.ScreenPointToRay(GetMousePositionWithOffset());
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
        var quadCentre = GetQuadCentre(_activeSlot);
        var tower = Instantiate(_towerPrefab, quadCentre, Quaternion.identity, _activeSlot.transform.parent.transform);
        tower.GetComponent<CapsuleCollider>().enabled = true;
        tower.GetComponent<TowerScript>().Slot = _activeSlot;
        var aoeTower = tower.GetComponent<AOETowerScript>();
        if (aoeTower != null) aoeTower.SetAOEVisible(true);
        _activeSlot.SetActive(false);
        _moneyChangedForAmount.Raise(-_towerPrefab.GetComponent<TowerScript>().Cost);
        _audioGameEvent.Raise((int) Sound.BuyTower);
      }

      _hoverPrefab.SetActive(false);
      _onEndDrag.Raise();
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private Vector3 GetMousePositionWithOffset()
    {
      var offset = Mathf.Clamp(_offset / _raycastDistance, MIN_OFFSET, MAX_OFFSET);
      return Input.mousePosition + new Vector3(0, offset, 0);
    }

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
        _raycastDistance = hits[terrainCollderQuadIndex].distance;
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
        if (hits[i].collider.gameObject.name.StartsWith("Environment"))
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
      var meshVerts = quad.GetComponent<MeshFilter>().mesh.vertices;
      var vertRealWorldPositions = new Vector3[meshVerts.Length];

      for (int i = 0; i < meshVerts.Length; i++)
      {
        vertRealWorldPositions[i] = quad.transform.TransformPoint(meshVerts[i]);
      }

      var midPoint = Vector3.Slerp(vertRealWorldPositions[0], vertRealWorldPositions[1], 0.5f);
      midPoint.y += GetOffset();

      return midPoint;
    }

    private float GetOffset()
    {
      return -0.005f;
    }

    //---------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------

    public bool Interactable { get; set; }
  }
}