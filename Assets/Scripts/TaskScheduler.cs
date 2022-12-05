using System.Collections;
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
        StartCoroutine(ProccessAllocation());
    }

    IEnumerator ProccessAllocation()
    {
        var task = _taskBuilder.BuildTaskFromUI();
        int count = 0;

        yield return StartCoroutine(_registerer.GetConflictedTaks(task, x => count = x));

        if (count == 0)
        {
            AllocateTask(task);
            print("Tarefa salva no servidor");

            Instantiate(createdTask);
        }
        else
        {
            print("Já tem uma tarefa nesse mesmo dia!");

            Instantiate(dayCommomTask);
        }
    }

    private void AllocateTask(TaskData task)
    {
        _registerer.RegisterTask(task);
    }
}
