using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCircle : MonoBehaviour {

	//---------------------------------------------------------------------
	// Editor
	//---------------------------------------------------------------------
	
	[SerializeField]
	private float _rotateSpeed = 200f;

	[SerializeField] private GameObject _parent;
	
	//---------------------------------------------------------------------
	// Internal
	//---------------------------------------------------------------------
	
	private RectTransform _rectComponent;
	
	//---------------------------------------------------------------------
	// Messages
	//---------------------------------------------------------------------
	
	// Use this for initialization
	void Start () 
	{
		_rectComponent = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update()
	{
		_rectComponent.Rotate(0f, 0f, -_rotateSpeed * Time.deltaTime);
	}
	
	//---------------------------------------------------------------------
	// Helpers
	//---------------------------------------------------------------------

	public void Show()
	{
		_parent.SetActive(true);
	}
}
