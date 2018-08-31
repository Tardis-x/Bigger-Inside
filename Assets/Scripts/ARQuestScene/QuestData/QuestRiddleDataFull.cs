using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRiddleDataFull 
{
    public readonly string description;
    public bool isCompleted;
    public int score;
    public bool isText;
    public Sprite image;
    public QuestRiddleDataFull(bool istext, Sprite sprite)
    {
        isText = istext;
        image = sprite;
    }
    
    public QuestRiddleDataFull(bool istext, string text)
    {
        isText = istext;
        description = text;
    }

}
