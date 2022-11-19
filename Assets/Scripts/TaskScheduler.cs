using System.Collections;
using UnityEngine;

public class TaskScheduler : MonoBehaviour
{
    [SerializeField] TaskRegisterer _registerer;
    [SerializeField] TaskBuilderFromUI _taskBuilder;
   
    public void AllocateNewTaskOnServer()
    {
        StartCoroutine(ProccessAllocation());
    }

    IEnumerator ProccessAllocation()
    {
        var task = _taskBuilder.BuildTaskFromUI();
        int count = 0;

        yield return StartCoroutine(_registerer.GetConflictedTaks(task, x => count = x));

        if(count == 0)
        {
            AllocateTask(task);
            print("Tarefa salva no servidor");
        }else
        {
            print("Já tem uma tarefa nesse mesmo dia!");
        }
    }

    private void AllocateTask(TaskData task)
    {
        _registerer.RegisterTask(task);
    }


}
