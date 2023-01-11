using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using UnityEngine;
using UnityEditor.PackageManager.UI;
using Michsky.UI.ModernUIPack;

public class RecentTasksVisualization : MonoBehaviour
{
    private int days = 1;
    private DateTime Now;
    List<TaskData> recentTasks;

    [SerializeField] int MaxTasks;
    [SerializeField] GameObject waiting;
    [SerializeField] Transform parentTransformForInstantation;
    [SerializeField] TaskUI prefabTask;

    [SerializeField] WindowManager window;


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Initialize());
    }

    public void Refresh()
    {
        if (window.currentWindowIndex != 0) return;
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        parentTransformForInstantation.DeleteChildren();
        waiting.SetActive(true);

        days = 1;
        Now = DateTime.Now;
        Query.SearchForExistentTasksThroughDate(Now.Date);

        yield return new WaitUntil(() => Query.searchingEnd);
        
        recentTasks = Query.GetTasksFounded();

        if (recentTasks.Count >= MaxTasks) InstantiateTasks();
        else StartCoroutine(SearchForMoreDays(Now.AddDays(1)));
    }

    IEnumerator SearchForMoreDays(DateTime day)
    {
        Query.SearchForExistentTasksThroughDate(day);

        yield return new WaitUntil(() => Query.searchingEnd);
        var arrayTasks = Query.GetTasksFounded();

        int lenght = arrayTasks.Count;
        for (int i = 0; i < lenght; i++)
        {
            recentTasks.Add(arrayTasks[i]);
            if (recentTasks.Count >= MaxTasks) break;
        }

        day = day.AddDays(1);
        if (days > 7 || recentTasks.Count >= MaxTasks) InstantiateTasks();
        else StartCoroutine(SearchForMoreDays(day));
    }

    private void InstantiateTasks()
    {
        waiting.SetActive(false);
        print(recentTasks.Count);
        foreach (TaskData task in recentTasks)
        {
            var obj = Instantiate(prefabTask, parentTransformForInstantation);
            obj.SetTask(task, true);
        }
    }
}
