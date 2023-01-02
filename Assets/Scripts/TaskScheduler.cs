using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class TaskScheduler : MonoBehaviour
{
    [SerializeField] TaskRegisterer _registerer;
    [SerializeField] TaskBuilderFromUI _taskBuilder;

    [Header("Messages")]
    [SerializeField] GameObject createdTask;
    [SerializeField] GameObject dayCommomTask;
   
    public void AllocateNewTaskOnServer()
    {
        var task = _taskBuilder.BuildTaskFromUI();
        StartCoroutine(ProccessAllocation(task));
    }

    IEnumerator ProccessAllocation(TaskData task)
    {
        List<TaskData> conflictedTasks = new List<TaskData>();

        yield return StartCoroutine(_registerer.GetConflictedTaks(task, x => conflictedTasks = x));

        if (conflictedTasks.Count == 0)
        {
            if (task.Index == -1)
            {
                AllocateTask(task);
                print("Tarefa criada no servidor");
                Instantiate(createdTask);
            }
            else
            {
                UpdateTask(task);
                print($"Tarefa [{task.Name}] foi realocada para: {task.StartTime}");
            }
        }
        else
        {
            if (HasTaskWithHigherPriority(task, conflictedTasks))
            {
                TaskData taskWithFurthestTime = GetFurthestTask(conflictedTasks);

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
                foreach(TaskData realocTask in conflictedTasks)
                {
                    yield return StartCoroutine(ProccessAllocation(realocTask));
                }
            }
        }
    }

    private bool HasTaskWithHigherPriority(TaskData task, List<TaskData> conflictedTasks)
    {
        foreach (TaskData confTask in conflictedTasks)
        {
            if (confTask.Priority >= task.Priority)
            {
                return true;
            }
        }

        return false;
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
