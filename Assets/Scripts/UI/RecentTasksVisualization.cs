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
        Query.SearchForExistentTasks(SetDateToString(), "Data");

        yield return new WaitUntil(() => Query.searchingEnd);
        
        recentTasks = Query.GetTasksFounded();

        if (recentTasks.Count >= MaxTasks) InstantiateTasks();
        else StartCoroutine(SearchForMoreDays());
    }

    IEnumerator SearchForMoreDays()
    {
        Query.SearchForExistentTasks(SetDateToString(days), "Data");

        yield return new WaitUntil(() => Query.searchingEnd);
        var arrayTasks = Query.GetTasksFounded();

        int lenght = arrayTasks.Count;
        for (int i = 0; i < lenght; i++)
        {
            recentTasks.Add(arrayTasks[i]);
            if (recentTasks.Count >= MaxTasks) break;
        }

        days++;
        if (days > 7 || recentTasks.Count >= MaxTasks) InstantiateTasks();
        else StartCoroutine(SearchForMoreDays());
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

    private string SetDateToString(int acrescimo = default)
    {
        int day = DateTime.Now.Day + acrescimo;
        int month = DateTime.Now.Month;
        int year = DateTime.Now.Year;

        DayCorrection(ref day, ref month, ref year);

        var builder = new StringBuilder();
        if (day < 10) builder.Append("0");
        builder.Append($"{day}/");
        if (month < 10) builder.Append("0");
        builder.Append($"{month}/");
        builder.Append(year.ToString());
        
        return builder.ToString();
    }

    private void DayCorrection(ref int day, ref int month, ref int year)
    {
        if(day > 28 && month == 2)
        {
            day = day - 28;
            month++;
        }
        else if(day > 30 && month % 2 == 0)
        {
            day = day - 30;
            month++;
        }
        else if(day > 31 && month % 2 == 1)
        {
            day = day - 31;
            month++;
        }

        if (month > 12)
        {
            month = month - 12;
            year++;
        }
    }
}
