using System.Collections.Generic;

public class QuestProgress
{
	public QuestProgress()
	{
		photoData = new QuestPhotoData();
		vrGameData = new QuestVrGameData();
		riddlesData = new Dictionary<string, bool>();
	}

	public QuestPhotoData photoData { get; set; }
	public QuestVrGameData vrGameData { get; set; }
	
	public Dictionary<string, bool> riddlesData { get; set; }
}
