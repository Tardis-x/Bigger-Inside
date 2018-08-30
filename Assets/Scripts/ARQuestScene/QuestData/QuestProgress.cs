using System;
using System.Collections.Generic;

public class QuestProgress
{
	public QuestProgress()
	{
		PhotoData = new QuestPhotoData();
		VrGameData = new QuestVrGameData();
		RiddlesData = new Dictionary<string, QuestRiddleData>();
	}

	public QuestPhotoData PhotoData { get; set; }
	public QuestVrGameData VrGameData { get; set; }
	
	public Dictionary<string, QuestRiddleData> RiddlesData { get; set; }
	public int globalScore;
	public bool isGoogleColorsCompleted;
	public bool allRiddlesCompleted;
	public Uri userPhotoUrl;
}
