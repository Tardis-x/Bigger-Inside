using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HallInterraction : InterractibleObject
{
	[SerializeField]
	private Text _hallNameText;

	public override void Interract()
	{
		_hallNameText.text = name;
	}
}
