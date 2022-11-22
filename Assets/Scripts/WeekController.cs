using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeekController : MonoBehaviour
{
    [SerializeField] TMP_Text monthYearText;
    [SerializeField] GameObject daysOfTheWeekContainer;

    DateTime localTime = DateTime.Now;

    string[] months = {"Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"};

    void Start() {
        defineMonth(localTime);
        defineDaysOfTheWeek(localTime);
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
    }

    public void previousWeek() {
        localTime = localTime.AddDays(-7);
        defineMonth(localTime);
        defineDaysOfTheWeek(localTime);
    }
}
