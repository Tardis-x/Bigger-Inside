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
            Debug.Log("OnBeginDrag raised");
            _onBeginDrag.Raise();
        }

        public void OnDrag(PointerEventData eventData)
        {
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
            if (_activeSlot != null)
            {
                var quadCentre = GetQuadCentre(_activeSlot);
                Instantiate(_towerPrefab, quadCentre, Quaternion.identity, _activeSlot.transform.parent.transform);
                _activeSlot.SetActive(false);
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
            var meshVerts = quad.GetComponent<MeshFilter>().mesh.vertices;
            var vertRealWorldPositions = new Vector3[meshVerts.Length];

            for (int i = 0; i < meshVerts.Length; i++)
            {
                vertRealWorldPositions[i] = quad.transform.TransformPoint(meshVerts[i]);
            }

            var midPoint = Vector3.Slerp(vertRealWorldPositions[0], vertRealWorldPositions[1], 0.5f);
            return midPoint;
        }
    }
}