using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskScheduler : MonoBehaviour
{
    [SerializeField] TaskRegisterer _registerer;
    [SerializeField] TaskBuilderFromUI _taskBuilder;
    [SerializeField] private GameObject _allocatingUI;
    [SerializeField] private GameObject _normalView;
    [SerializeField] private UIElement _uiElement;

    [Header("Messages")]
    [SerializeField] CreateTaskMessage createdTask;
    [SerializeField] GameObject dayCommomTask;

    private List<TaskData> _reallocatedTasks = new List<TaskData>();

    public void AllocateNewTaskOnServer()
    {
        _reallocatedTasks.Clear();

        var task = _taskBuilder.BuildTaskFromUI();
        ChangeUIToAllocating();
        StartCoroutine(ProccessAllocation(task, () => HandleFinishAllocation(task)));
    }

    private void ChangeUIToAllocating()
    {
        _allocatingUI.SetActive(true);
        _normalView.SetActive(false);
    }

    private void HandleFinishAllocation(TaskData allocatedTask)
    {
        CreateTaskMessage newTask = Instantiate(createdTask);
        newTask.UpdateUI(allocatedTask, _reallocatedTasks);
        _uiElement.Hide();
        if (_reallocatedTasks.Count == 0)
        {
            print("Tarefa criada no servidor");
        }
        else
        {
            // TODO: Aqui fazer instanciar uma mensagem que mostre
            // 1. A tarefa alocada e a data que ela foi alocada
            // 2. A lista de todas as tarefas que foram realocadas, mostrando o hor�rio novo delas

            foreach (TaskData realloc in _reallocatedTasks)
            {
                print($"Tarefa [{realloc.Name}] foi realocada para: {realloc.StartTime}");
                FindObjectOfType<NotificationController>().UpdateNotification(realloc);
            }
        }
        _uiElement.Hide();
    }

    IEnumerator ProccessAllocation(TaskData task, Action OnFinishAllocation = null)
    {
        List<TaskData> conflictedTasks = new List<TaskData>();

        yield return StartCoroutine(_registerer.GetConflictedTaks(task, x => conflictedTasks = x));

        if (conflictedTasks.Count == 0)
        {
            if (task.Index == -1)
            {
                AllocateTask(task);
            }
            else
            {
                UpdateTask(task);
                _reallocatedTasks.Add(task);
            }
        }
        else
        {
            List<TaskData> higherPriorityTasks = GetTasksWithHigherPriority(task, conflictedTasks).ToList();
            if (higherPriorityTasks.Count > 0)
            {
                TaskData taskWithFurthestTime = GetFurthestTask(higherPriorityTasks);

                TimeSpan timeSpan = task.EndTime - task.StartTime;
                task.StartTime = taskWithFurthestTime.EndTime.AddMinutes(30);
                task.EndTime = task.StartTime.Add(timeSpan);

                yield return StartCoroutine(ProccessAllocation(task));
            }
            else
            {
                // Allocate the target task, because it has higher priority
                AllocateTask(task);

                // Reallocate all other tasks that was before allocated
                foreach (TaskData realocTask in conflictedTasks)
                {
                    yield return StartCoroutine(ProccessAllocation(realocTask));
                }
            }
        }

        OnFinishAllocation?.Invoke();
    }

    private IEnumerable<TaskData> GetTasksWithHigherPriority(TaskData task, List<TaskData> conflictedTasks)
    {
        foreach (TaskData confTask in conflictedTasks)
        {
            if (confTask.Priority >= task.Priority)
            {
                yield return confTask;
            }
        }
    }

    private TaskData GetFurthestTask(List<TaskData> conflictedTasks)
    {
        TaskData taskWithFurthestTime = conflictedTasks[0];
        foreach (TaskData confTask in conflictedTasks)
        {
            if (confTask.EndTime > taskWithFurthestTime.EndTime)
            {
                taskWithFurthestTime = confTask;
            }
        }

        return taskWithFurthestTime;
    }

    private void AllocateTask(TaskData task)
    {
        _registerer.RegisterTask(task);
    }

    private void UpdateTask(TaskData task)
    {
        _registerer.UpdateTask(task);
    }
}
