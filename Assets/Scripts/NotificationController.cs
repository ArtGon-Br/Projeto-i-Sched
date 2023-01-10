using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
public class NotificationController : MonoBehaviour
{
    void Start() 
    {
        var channel = new AndroidNotificationChannel()
        {
        Id = "idChannel",
        Name = "Notification Center",
        Importance = Importance.Default,
        Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);   
    }

    public AndroidNotification CreateNotification(System.DateTime _time,string _title, string _msg)
    {
        var _myNotification = new AndroidNotification();
        _myNotification.Title = _title;
        _myNotification.Text = _msg;
        _myNotification.FireTime = _time;
        return _myNotification;

    }

    public void UpdateNotification(TaskData realloc)
    {
        var notification = FindObjectOfType<NotificationController>().CreateNotification(realloc.StartTime, realloc.Name, "Sua tarefa esta começando");
        AndroidNotificationCenter.UpdateScheduledNotification(realloc.MyNotificationID, notification, "channel_id");
    }
}
