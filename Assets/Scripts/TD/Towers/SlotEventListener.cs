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

		private void Awake()
		{
			_materialColor = _meshRenderer.material.color;
		}

		private void OnEnable()
		{
			_meshRenderer.material.color = new Color(_materialColor.r, _materialColor.g, _materialColor.b, 0);
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