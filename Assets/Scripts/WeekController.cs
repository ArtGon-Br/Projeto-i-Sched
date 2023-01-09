using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Auth;
using TMPro;

public class WeekController : MonoBehaviour
{
    [SerializeField] TMP_Text monthYearText;
    [SerializeField] GameObject daysOfTheWeekContainer;
    [SerializeField] Transform taskContainer;
    [SerializeField] Transform taskPrefab;

    DateTime localTime = DateTime.Now;

    string[] months = {"Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"};

    List<Transform> clones = new List<Transform>();

    void Start() {
        defineMonth(localTime);
        defineDaysOfTheWeek(localTime);
        searchAndInstantiateTasks(localTime);
    }

    private void defineMonth(DateTime date) {
        int monthIndex = date.Month;
        int year = date.Year;
        monthYearText.text = $"{months[monthIndex - 1]} {year}";
    }

    private void defineDaysOfTheWeek(DateTime date) {
        date = date.AddDays(-(int)date.DayOfWeek);

        for (int i = 0; i < daysOfTheWeekContainer.transform.childCount; i++) {
            GameObject dayObj = daysOfTheWeekContainer.transform.GetChild(i).gameObject;
            TMP_Text dayNumber = dayObj.transform.Find("Número").gameObject.GetComponent<TMP_Text>();

            dayNumber.text = date.AddDays(i).Day.ToString();
        }
    }

    public void nextWeek() {
        localTime = localTime.AddDays(7);
        defineMonth(localTime);
        defineDaysOfTheWeek(localTime);
        searchAndInstantiateTasks(localTime);
    }

    public void previousWeek() {
        localTime = localTime.AddDays(-7);
        defineMonth(localTime);
        defineDaysOfTheWeek(localTime);
        searchAndInstantiateTasks(localTime);
    }

    private void searchAndInstantiateTasks(DateTime date) {
        var auth = FirebaseAuth.DefaultInstance;
        var Db = FirebaseFirestore.DefaultInstance;

        deleteClones();

        // first day of the week
        date = date.AddDays(-(int)date.DayOfWeek);
        string day = date.Day.ToString();
        string month = date.Month.ToString();
        string year = date.Year.ToString();

        CollectionReference trfRef = Db.Collection(path: "users_sheet").Document(path: auth.CurrentUser.UserId.ToString()).Collection(path: "Tasks");

        Firebase.Firestore.Query query = trfRef;
        query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
        {
            if (querySnapshotTask.IsCanceled) return;

            for (int i =0 ; i < 7; i++) {
                List<DocumentSnapshot> tasksFound;
                tasksFound = querySnapshotTask.Result.Documents.Where(t => t.ToDictionary()["Day"].ToString() == day)
                                                                .Where(t => t.ToDictionary()["Month"].ToString() == month)
                                                                .Where(t => t.ToDictionary()["Year"].ToString() == year).ToList();

                print($"{tasksFound.Count}");

                foreach(DocumentSnapshot task in tasksFound) {
                    InstantiateTasks(task, i);
                }

                date = date.AddDays(1);
                day = date.Day.ToString();
                month = date.Month.ToString();
                year = date.Year.ToString();
            }
        });
    }

    private void InstantiateTasks(DocumentSnapshot task, int dayIndex) {
        Dictionary<string, object> details = task.ToDictionary();
        print($"{details["Name"].ToString()}");

        int hours = int.Parse(details["Hour"].ToString());
        int hourDuration = int.Parse(details["HourDuration"].ToString());
        double minutes = int.Parse(details["Min"].ToString()) * (60.0 / 70.0);
        double minutesDuration = int.Parse(details["MinutesDuration"].ToString()) * (60.0 / 70.0);

        Transform taskTransform = Instantiate(taskPrefab, taskContainer);

        taskTransform.Find("Texto").GetComponent<TMP_Text>().text = details["Name"].ToString();
        taskTransform.localPosition -= new Vector3(-120 * dayIndex, 70 * hours + (float)minutes, 0);
        taskTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 70 * hourDuration + (float)minutesDuration);
        taskTransform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(120, 70 * hourDuration + (float)minutesDuration);
        taskTransform.Find("Texto").GetComponent<RectTransform>().sizeDelta = new Vector2(120, 70 * hourDuration + (float)minutesDuration);
        taskTransform.gameObject.SetActive(true);

        clones.Add(taskTransform);
    }

    private void deleteClones()
    {
        foreach (Transform clone in clones) {
            if (clone != taskContainer) {
                Destroy(clone.gameObject);
            }
        }
        clones.Clear();
    }
}
