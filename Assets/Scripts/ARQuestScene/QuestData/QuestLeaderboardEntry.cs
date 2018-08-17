using System;

public class QuestLeaderboardEntry
{
	public Uri userPhotoUrl;
	public int globalScore;

	public QuestLeaderboardEntry(Uri url, int score)
	{
		userPhotoUrl = url;
		globalScore = score;
	}
}
