using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public static class FirestoreCache
  {

    private static string _fileName = "schedule.txt";

    public static void SaveSchedule(Schedule schedule)
    {
      var fileStream = File.Create(Application.persistentDataPath + "/" + _fileName);
      var binaryFormatter = new BinaryFormatter();
      
      binaryFormatter.Serialize(fileStream, schedule);
      fileStream.Close();
    }

    public static Schedule GetSchedule()
    {
      var filePath = Application.persistentDataPath + "/" + _fileName;
      
      if (!File.Exists(filePath)) return null;
      
      try
      {        
        var fileStream = File.Open(filePath, FileMode.Open);
        var binaryFormatter = new BinaryFormatter();

        var schedule = binaryFormatter.Deserialize(fileStream) as Schedule;
                
        return schedule;
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}