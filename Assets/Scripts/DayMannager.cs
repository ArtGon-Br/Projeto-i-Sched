using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;

public class DayMannager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] int year;
    [SerializeField][Range(1, 31)] int day;
    [SerializeField][Range(1, 12)] int month;

    List<Task> tasks;

    void Awake()
    {

    }

    void Start()
    {
        if (CheckTasks()) SetUI();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        DateTime date = new DateTime(year, month, day);
        FindObjectOfType<DailyTasksVisualization>().Visualize(tasks, date);
    }

    // Checa no banco de dados se existe alguma tarefa e adiciona cada uma na lista (tasks), caso nao tenha alguma tarefa retorna false
    bool CheckTasks()
    {
        var aux = false;

        return aux;
    }


    // Seta o componente Image caso o dia tenha alguma task, ou seja, deixa o dia no calendario com um tipo de highlight
    void SetUI()
    {
        // Filho do botao do dia possui uma imagem se mostra se existe tasks ou nao
        var color = Color.white;
        color.a = 0.25f;
        transform.GetChild(1).GetComponent<Image>().color = color;
    }
}
