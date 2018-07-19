﻿
public class QuestRiddleData
{
	public QuestRiddleData(int number, string name, string description, RiddleMarkerType markerType)
	{
		this.number = number;
		this.name = name;
		this.Description = description;
		this.markerType = markerType;
		
		State = false;
	}
	
	int number;
	string name;
	string description;

	public bool State { get; set; }

	public string Description { get; }

	RiddleMarkerType markerType;
}