using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class SlotEventListener : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private Color _materialColor;
		
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private MeshRenderer _meshRenderer;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Start()
		{
			_materialColor = _meshRenderer.material.color;
		}
		
		//---------------------------------------------------------------------
		// Events
		//---------------------------------------------------------------------

		public void OnBeginDragTower()
		{
			_meshRenderer.material.color = new Color(_materialColor.r, _materialColor.g, _materialColor.b, 1);
		}

		public void OnEndDragTower()
		{
			_meshRenderer.material.color = new Color(_materialColor.r, _materialColor.g, _materialColor.b, 0);
		}
	}
}