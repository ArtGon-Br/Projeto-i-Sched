using UnityEngine;
using Michsky.UI.ModernUIPack;
using System;
using Unity.Notifications.Android;

public class TaskBuilderFromUI : MonoBehaviour
{
    [SerializeField] private CustomInputField _nameField;
    [SerializeField] private CustomInputField _descriptionFIeld;
    [SerializeField] private CustomInputField _hourDurationField;
    [SerializeField] private CustomInputField _minDurationField;
    [SerializeField] private HorizontalSelector _priorityField;

    public TaskData BuildTaskFromUI()
    {
        return BuildDynamicTask();
    }

    private TaskData BuildDynamicTask()
    {
        DateTime endTime = DateTime.UtcNow;
        endTime = endTime.AddHours(int.Parse(_hourDurationField.inputText.text));
        endTime = endTime.AddMinutes(int.Parse(_minDurationField.inputText.text));
        var notificationController = FindObjectOfType<NotificationController>();
        int id = 0;
        if (notificationController != null)
        {
            var notification = notificationController.CreateNotification(DateTime.UtcNow, _nameField.inputText.text, "Sua tarefa esta começando");
            id = AndroidNotificationCenter.SendNotification(notification, "idChannel");
        }

        TaskData newTask = new TaskData
        {
            Name = _nameField.inputText.text,
            Description = _descriptionFIeld.inputText.text,
            Priority = _priorityField.index,
            StartTime = DateTime.UtcNow,
            EndTime = endTime,
            Index = -1,
            MyNotificationID = id
        };

        return newTask;
    }
}
