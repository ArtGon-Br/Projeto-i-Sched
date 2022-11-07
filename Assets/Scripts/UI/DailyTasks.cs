using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DailyTasks : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] int year;
    [SerializeField] [Range(1, 31)] int day;
    [SerializeField] [Range(1, 12)] int month;

    List<TaskSO> tasks;

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
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
