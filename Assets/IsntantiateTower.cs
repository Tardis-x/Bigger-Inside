using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class IsntantiateTower : MonoBehaviour
  {
    [SerializeField] private GameObject _activeSlot;
    [SerializeField] private GameObject _towerPrefab;

    public void OnClick()
    {
      var quadCentre = GetQuadCentre(_activeSlot);
      var tower = Instantiate(_towerPrefab, quadCentre, Quaternion.identity, _activeSlot.transform.parent.transform);
      Debug.Log("Instantiated tower");
      tower.GetComponent<CapsuleCollider>().enabled = true;
      tower.GetComponent<TowerScript>().Slot = _activeSlot;
      var aoeTower = tower.GetComponent<AOETowerScript>();
      if (aoeTower != null) aoeTower.SetAOEVisible(true);
      _activeSlot.SetActive(false);
    }
    
    private Vector3 GetQuadCentre(GameObject quad)
    {
      var offsetY = 0.05f;
      var meshVerts = quad.GetComponent<MeshFilter>().mesh.vertices;
      var vertRealWorldPositions = new Vector3[meshVerts.Length];

      for (int i = 0; i < meshVerts.Length; i++)
      {
        vertRealWorldPositions[i] = quad.transform.TransformPoint(meshVerts[i]);
      }

      var midPoint = Vector3.Slerp(vertRealWorldPositions[0], vertRealWorldPositions[1], 0.5f);
      midPoint.y = offsetY;
      
      return midPoint;
    }
  }
}