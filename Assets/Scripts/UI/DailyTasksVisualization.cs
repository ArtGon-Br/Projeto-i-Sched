using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DailyTasksVisualization : MonoBehaviour
{
    Dictionary<string, string> weekDays = new Dictionary<string, string> 
                                            { { "Monday", "Segunda-Feira"}, { "Tuesday", "Terça-Feira" }, { "Wednesday", "Quarta-Feira" },
                                              { "Thursday", "Quinta-Feira"}, { "Friday", "Sexta-Feira"}, { "Saturday", "Sábado"}, { "Sunday", "Domingo"} };


    [SerializeField] GameObject taskPrefab;
    [SerializeField] Transform tasksTrasform;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TextMeshProUGUI dayText, dateText;

    public void InstantiateTasks(List<Task> listTasks)
    {
        // Instanciar todas as taks presentes naquele dia
        foreach (Task task in listTasks)
        {
            var temp = Instantiate(taskPrefab, tasksTrasform);
            temp.GetComponent<TaskUI>().SetTask(task);
        }
    }
    
    public void Visualize(List<Task> _listTasks, DateTime date)
    {
        InstantiateTasks(_listTasks);
        dayText.text = weekDays[date.DayOfWeek.ToString()];

        string day, month;

        if (date.Day < 10) day = "0" + date.Day.ToString();
        else day = date.Day.ToString();

        if (date.Month < 10) month = "0" + date.Month.ToString();
        else month = date.Month.ToString();

        dateText.text = day + " / " + month + " / " + date.Year.ToString();

        //Responsavel por Tornar o canvasgroup visivel, logo todas as tarefas daquele dia
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void Disable()
    {
        //Responsavel por Desativar o canvasgroup
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
