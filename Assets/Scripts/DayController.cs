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

public class DayController : MonoBehaviour
{
    [SerializeField] TMP_Text monthYearText;
    [SerializeField] TMP_Text dayName;
    [SerializeField] TMP_Text dayNumber;
    [SerializeField] Transform viewport;
    [SerializeField] Transform taskPrefab;

    DateTime localTime = DateTime.Now;

    static string[] months = {"Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"};
    static string[] daysOfTheWeek = {"DOMINGO", "SEGUNDA", "TERÇA", "QUARTA", "QUINTA", "SEXTA", "SÁBADO"};

    List<Transform> clones = new List<Transform>();

    void Start()
    {
        defineMonth(localTime);
        defineDay(localTime);
        taskPrefab.gameObject.SetActive(false);
        searchAndInstantiateTasks(localTime.Day.ToString(), localTime.Month.ToString(), localTime.Year.ToString());
    }

    private void defineMonth(DateTime date) {
        int monthIndex = date.Month;
        int year = date.Year;
        monthYearText.text = $"{months[monthIndex - 1]} {year}";
    }

    private void defineDay(DateTime date) {
        int day = date.Day;
        int dayIndex = (int)date.DayOfWeek;

        dayName.text = daysOfTheWeek[dayIndex];
        dayNumber.text = day.ToString();
    }

    public void nextDay() {
        localTime = localTime.AddDays(1);
        defineMonth(localTime);
        defineDay(localTime);
        searchAndInstantiateTasks(localTime.Day.ToString(), localTime.Month.ToString(), localTime.Year.ToString());
    }
    public void previousDay() {
        localTime = localTime.AddDays(-1);
        defineMonth(localTime);
        defineDay(localTime);
        searchAndInstantiateTasks(localTime.Day.ToString(), localTime.Month.ToString(), localTime.Year.ToString());
    }
    private void searchAndInstantiateTasks(string day, string month, string year) {
        var auth = FirebaseAuth.DefaultInstance;
        var Db = FirebaseFirestore.DefaultInstance;

        CollectionReference trfRef = Db.Collection(path: "users_sheet").Document(path: auth.CurrentUser.UserId.ToString()).Collection(path: "Tasks");

        Firebase.Firestore.Query query = trfRef;

        deleteClones();

        query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
        {
            if (querySnapshotTask.IsCanceled) return;

            List<DocumentSnapshot> tasksFound;
            tasksFound = querySnapshotTask.Result.Documents.Where(t => t.ToDictionary()["Day"].ToString() == day)
                                                            .Where(t => t.ToDictionary()["Month"].ToString() == month)
                                                            .Where(t => t.ToDictionary()["Year"].ToString() == year).ToList();

            print($"{tasksFound.Count}");

            foreach(DocumentSnapshot task in tasksFound) {
                InstantiateTasks(task);
            }
        });
    }
    private void InstantiateTasks(DocumentSnapshot task) {
        Dictionary<string, object> details = task.ToDictionary();
        print($"{details["Name"].ToString()}");

        int hours = int.Parse(details["Hour"].ToString());
        int hourDuration = int.Parse(details["HourDuration"].ToString());
        double minutes = int.Parse(details["Min"].ToString()) * (60.0 / 70.0);
        double minutesDuration = int.Parse(details["MinutesDuration"].ToString()) * (60.0 / 70.0);

        Transform taskTransform = Instantiate(taskPrefab, viewport);

        taskTransform.Find("Texto").GetComponent<TMP_Text>().text = details["Name"].ToString();
        taskTransform.localPosition -= new Vector3(0, 70 * hours + (float)minutes, 0);
        taskTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(840, 70 * hourDuration + (float)minutesDuration);
        taskTransform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(840, 70 * hourDuration + (float)minutesDuration);
        taskTransform.Find("Texto").GetComponent<RectTransform>().sizeDelta = new Vector2(840, 70 * hourDuration + (float)minutesDuration);
        taskTransform.gameObject.SetActive(true);

        clones.Add(taskTransform);
    }
    private void deleteClones()
    {
        foreach (Transform clone in clones) {
            if (clone != viewport) {
                Destroy(clone.gameObject);
            }
        }
        clones.Clear();
    }
}
