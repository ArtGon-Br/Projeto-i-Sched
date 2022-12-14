using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RecentTasksVisualization : MonoBehaviour
{
    [SerializeField] int MaxTasks;
    [SerializeField] Transform parentTransformForInstantation;
    [SerializeField] TaskUI prefabTask;

    List<TaskData> recentTasks = new List<TaskData>();

    // Start is called before the first frame update
    void Start()
    {
        recentTasks = Query.SearchForExistentTasks(SetDateToString(), "Data");
        print($"Tasks no dia: {recentTasks.Count}");

        int days = 1;
        while(recentTasks.Count < MaxTasks && days < 7)
        {
            var arrayTasks = Query.SearchForExistentTasks(SetDateToString(days), "Data").ToArray();
            for(int i = 0; i < arrayTasks.Length; i++)
                recentTasks.Add(arrayTasks[i]);
            days++;
        }

        foreach(TaskData task in recentTasks)
        {
            var obj = Instantiate(prefabTask, parentTransformForInstantation);
            obj.SetTask(task, true);
        }
    }

    private string SetDateToString(int acrescimo = default)
    {
        int day = DateTime.Now.Day + acrescimo;
        int month = DateTime.Now.Month;
        int year = DateTime.Now.Year;

        var builder = new StringBuilder();
        if (day < 10) builder.Append("0");
        builder.Append($"{day}/");
        if (month < 10) builder.Append("0");
        builder.Append($"{month}/");
        builder.Append(year.ToString());
        
        return builder.ToString();
    }
}
