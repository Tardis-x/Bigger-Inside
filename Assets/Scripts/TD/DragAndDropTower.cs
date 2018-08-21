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
        [SerializeField] private GameObject _environment;
        [SerializeField] private int _ghostTowerScaleFactor;
        
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
            // Debug.Log("Beginning drag");
        }

        public void OnDrag(PointerEventData eventData)
        {
            RaycastHit[] hits;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray, 50f);
            if (hits != null && hits.Length > 0)
            {
                MaybeShowHoverPrefab(hits);

                int slotIndex = GetSlotIndex(hits);
                if (slotIndex != -1)
                {
                    GameObject slotQuad = hits[slotIndex].collider.gameObject;
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
                Vector3 quadCentre = GetQuadCentre(_activeSlot);
                var tower = Instantiate(_towerPrefab, quadCentre, Quaternion.identity, _environment.transform);
                var towerPosition = tower.transform.position;
                tower.transform.position = new Vector3(towerPosition.x - .35f, towerPosition.y, towerPosition.z - 0.35f);
                _activeSlot.SetActive(false);
            }

            _hoverPrefab.SetActive(false);
        }
        
        //---------------------------------------------------------------------
        // Helpers
        //---------------------------------------------------------------------
        
        
        private void ChangeMaterialColor(bool value)
        {
            MeshRenderer[] meshRenderers = _hoverPrefab.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material.color = value ? Color.green : Color.red;
            }
        }

        private void MaybeShowHoverPrefab(RaycastHit[] hits)
        {
            int terrainCollderQuadIndex = GetTerrainColliderQuadIndex(hits);
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
            for (int i = 0; i < hits.Length; i++)
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
            for (int i = 0; i < hits.Length; i++)
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
            Vector3[] meshVerts = quad.GetComponent<MeshFilter>().mesh.vertices;
            Vector3[] vertRealWorldPositions = new Vector3[meshVerts.Length];

            for (int i = 0; i < meshVerts.Length; i++)
            {
                vertRealWorldPositions[i] = quad.transform.TransformPoint(meshVerts[i]);
            }

            Vector3 midPoint = Vector3.Slerp(vertRealWorldPositions[0], vertRealWorldPositions[1], 0.1f);
            return midPoint;
        }
    }
}