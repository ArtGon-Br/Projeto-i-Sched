using System.Collections.Generic;
using UnityEngine;

public class CreateTaskMessage : MonoBehaviour
{
    [SerializeField] private GameObject _createdTaskMessage;
    [SerializeField] private GameObject _reallocTasksMessage;
    [SerializeField] private CreatedTaskItemUI _taskItemPrefab;

    public void UpdateUI(TaskData newTask, List<TaskData> reallocTasks)
    {
        UpdateCreatedTask(newTask);
        UpdateReallocTasks(reallocTasks);
    }

    private void UpdateCreatedTask(TaskData newTask)
    {
        CreatedTaskItemUI item = Instantiate(_taskItemPrefab, _createdTaskMessage.transform);
        item.SetUI(newTask, true);
    }

    private void UpdateReallocTasks(List<TaskData> reallocTasks)
    {
        if(reallocTasks.Count == 0)
        {
            _reallocTasksMessage.gameObject.SetActive(false);
            return;
        }

        foreach(TaskData task in reallocTasks)
        {
            CreatedTaskItemUI item = Instantiate(_taskItemPrefab, _reallocTasksMessage.transform);
            item.SetUI(task, false);
        }
    }
}
