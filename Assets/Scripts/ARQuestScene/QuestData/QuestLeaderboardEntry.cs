using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLeaderboardEntry
{
    public string userName;
    public int globalScore;

    public QuestLeaderboardEntry(string s, int score)
    {
        userName = s;
        globalScore = score;
    }
}
