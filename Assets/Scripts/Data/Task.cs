using System;
using UnityEngine;

[CreateAssetMenu]
public class Task:ScriptableObject
{
    [Range(0,23)][SerializeField] int hour;
    [Range(0,59)][SerializeField] int minute;

    [SerializeField] string taskName;
    [TextArea] [SerializeField] string taskDescription;

    public string GetName()
    {
        return taskName;
    }

    public string GetTaskDescription()
    {
        return taskDescription;
    }

    public DateTime GetDate()
    {
        return new DateTime(2022, DateTime.Now.Month, DateTime.Now.Day, hour, minute, 0);
    }

    public string GetHour()
    {
        string hour = GetDate().Hour.ToString();
        string minutes = GetDate().Minute.ToString();

        return hour + ":" + minutes;
    }
}
