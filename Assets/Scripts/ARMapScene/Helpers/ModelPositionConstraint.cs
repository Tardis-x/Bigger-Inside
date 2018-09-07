using UnityEngine;

public class ModelPositionConstraint : MonoBehaviour {

	//---------------------------------------------------------------------
	// Editor
	//---------------------------------------------------------------------

	[Header("Offset")] 
	[SerializeField] private Vector3 _offset;
	
	//---------------------------------------------------------------------
	// Internal
	//---------------------------------------------------------------------

	private Vector3 _anchor;
	
	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------

	private void Awake ()
	{
		_anchor = transform.position;
	} 
	
	private void FixedUpdate ()
	{
		var _scale = transform.localScale.x / .3f;
		
		var pos = transform.position;
		pos.x = Mathf.Clamp(pos.x, (_anchor.x - _offset.x) * _scale, (_anchor.x + _offset.x) * _scale);
		pos.y = _anchor.y;
		pos.z = Mathf.Clamp(pos.z, (_anchor.z - _offset.z) * _scale, (_anchor.z + _offset.z) * _scale);

		transform.position = pos;
	}
}
