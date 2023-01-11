using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Linq;

public class DayMannager : MonoBehaviour
{
    private List<TaskData> tasks;
    [SerializeField] int year;
    [SerializeField] int day;
    [SerializeField] int month;
    [SerializeField] int taskCount;
    Dictionary<string, int> months = new Dictionary<string, int> { { "jan", 1 },    { "fev", 2 },   { "mar", 3 },
                                                                   { "abr", 4 },    { "mai", 5 },   { "jun", 6 },
                                                                   { "jul", 7 },    { "ago", 8 },   { "set", 9 },
                                                                   { "out", 10 },   { "nov", 11 },  { "dez", 12 } };

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (tasks.Count == 0) return;
            DateTime date = new DateTime(year, month, day);
            FindObjectOfType<DailyTasksVisualization>().Visualize(tasks, date);
        });
    }

    // Seta o componente Image caso o dia tenha alguma task, ou seja, deixa o dia no calendario com um tipo de highlight
    void SetUI(bool hasTasks, bool dayPassed)
    {
        // Filho do botao do dia possui uma imagem se mostra se existe tasks ou nao
        Color color;
        ColorUtility.TryParseHtmlString("#00FA27", out color);
        color.a = hasTasks ? 0.25f: 0;
        transform.GetChild(1).GetComponent<Image>().color = color;

        color = Color.white;
        color.a = dayPassed ? 0.95f : 1f;
        GetComponent<Image>().color = color;
        transform.GetChild(0).GetComponent<Text>().color = color;
        GetComponent<Button>().interactable = !dayPassed;
    }

    public void setDate(string Day, string Mes, string Ano)
    {
        int.TryParse(Ano, out year);
        month = months[Mes];
        int.TryParse(Day, out day);

        StartCoroutine(UpdateDay());
    }
    
    private IEnumerator UpdateDay()
    {
        var date = new DateTime(year, month, day);

        Query.SearchForExistentTasksThroughDate(date);
        yield return new WaitUntil(() => Query.searchingEnd);

        tasks = new List<TaskData>(Query.GetTasksFounded());
        CheckTasksInSpeficDate();
        taskCount = tasks.Count;

        var temp = DateTime.Now.Date <= date;
        SetUI(tasks.Count > 0 && temp, temp);
    }

    private void CheckTasksInSpeficDate()
    {
        var correctTasks = tasks.Where(t => t.StartTime.Day == day)
                              .Where(t => t.StartTime.Month == month)
                              .Where(t => t.StartTime.Year == year).ToList();

        tasks = correctTasks;
    }
}
