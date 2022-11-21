using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;

public class DayMannager : MonoBehaviour, IPointerClickHandler
{
    public List<TaskData> tasks;
    [SerializeField] int year;
    [SerializeField] int day;
    [SerializeField] int month;
    private bool ready;
    Dictionary<string, int> months = new Dictionary<string, int> { { "jan", 1 },    { "fev", 2 },   { "mar", 3 },
                                                                   { "abr", 4 },    { "mai", 5 },   { "jun", 6 },
                                                                   { "jul", 7 },    { "ago", 8 },   { "set", 9 },
                                                                   { "out", 10 },   { "nov", 11 },  { "dez", 12 } };
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        DateTime date = new DateTime(year, month, day);
        FindObjectOfType<DailyTasksVisualization>().Visualize(tasks, date);
    }

    // Seta o componente Image caso o dia tenha alguma task, ou seja, deixa o dia no calendario com um tipo de highlight
    void SetUI()
    {
        // Filho do botao do dia possui uma imagem se mostra se existe tasks ou nao
        var color = Color.white;
        color.a = 0.25f;
        transform.GetChild(1).GetComponent<Image>().color = color;
    }
    public void isReady() => ready = true;
    public IEnumerator UpdateDay(string Day, string Mes, string Ano)
    {
        year = int.Parse(Ano);
        month = months[Mes];
        day = int.Parse(Day);

        string _day = day < 10 ? "0" + day.ToString() : day.ToString();
        string _month = month < 10 ? "0" + month.ToString() : month.ToString();
        string _year = year.ToString();
        string input = _day + "/" + _month + "/" + _year;

        StartCoroutine(Query.SearchForExistentTasks(input, "Data", this));

        yield return new WaitUntil(() => ready);
        ready = false;
        print(tasks.Count);
        if (tasks.Count != 0) SetUI();
    }
}
