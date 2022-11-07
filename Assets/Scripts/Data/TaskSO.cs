using System;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu]
public class TaskSO:ScriptableObject
{
    [Range(0,23)][SerializeField] int hour;
    [Range(0,59)][SerializeField] int minute;

    [SerializeField] string taskName;
    [TextArea] [SerializeField] string taskDescription;

    public string GetName() => taskName;
    public void setName(string name) => taskName = name;
    public string GetTaskDescription() => taskDescription;

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
    public void setHour(string _hour)
    {
        string[] temp = _hour.Split(':');
        hour = int.Parse(temp[0]);
        minute = int.Parse(temp[1]);
    }
}
