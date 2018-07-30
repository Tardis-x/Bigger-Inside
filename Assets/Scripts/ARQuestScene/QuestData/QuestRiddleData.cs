using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRiddleData
{
	public readonly string description;
	public bool isCompleted;
	public int score;

	public QuestRiddleData(string des)
	{
		description = des;
	}
}
