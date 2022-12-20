using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
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
        StringBuilder builder = new StringBuilder();
        builder.Append(day.ToString());
        builder.Append("/");
        builder.Append(month.ToString());
        builder.Append("/");
        builder.Append(year.ToString());

        Query.SearchForExistentTasks(builder.ToString(), "Data");
        yield return new WaitUntil(() => Query.searchingEnd);

        tasks = new List<TaskData>(Query.GetTasksFounded());
        CheckTasksInSpeficDate();
        taskCount = tasks.Count;

        SetUI(tasks.Count > 0 && !isDayPassed(), isDayPassed());
    }

    private bool isDayPassed()
    {
        int _year = DateTime.Now.Year;
        int _month = DateTime.Now.Month;
        int _day = DateTime.Now.Day;

        if (year < _year) return true;
        if (month < _month && year == _year) return true;
        if (day < _day && month == _month && year == _year) return true;

        return false;
    }
    private void CheckTasksInSpeficDate()
    {
        var correctTasks = tasks.Where(t => t.Day == day.ToString())
                              .Where(t => t.Month == month.ToString())
                              .Where(t => t.Year == year.ToString()).ToList();

        tasks = correctTasks;
    }
}
