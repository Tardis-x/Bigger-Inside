using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRiddleDataFull 
{
    public readonly string description;
    public bool isCompleted;
    public int score;
    public bool isText;
    public Texture2D texture;
    public QuestRiddleDataFull(bool istext, Texture2D tex)
    {
        isText = istext;
        texture = tex;
    }
    
    public QuestRiddleDataFull(bool istext, string text)
    {
        isText = istext;
        description = text;
    }

}
