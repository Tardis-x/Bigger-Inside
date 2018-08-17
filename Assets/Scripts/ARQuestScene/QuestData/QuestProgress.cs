using System;
using System.Collections.Generic;

public class QuestProgress
{
	public QuestProgress()
	{
		photoData = new QuestPhotoData();
		vrGameData = new QuestVrGameData();
		riddlesData = new Dictionary<string, QuestRiddleData>();
	}

	public QuestPhotoData photoData { get; set; }
	public QuestVrGameData vrGameData { get; set; }
	
	public Dictionary<string, QuestRiddleData> riddlesData { get; set; }
	public int globalScore;
	public bool isGoogleColorsCompleted;
	public bool allRiddlesCompleted;
	public Uri userPhotoUrl;
}
