using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayController : MonoBehaviour
{
    [SerializeField] TMP_Text monthYearText;
    [SerializeField] TMP_Text dayName;
    [SerializeField] TMP_Text dayNumber;

    DateTime localTime = DateTime.Now;

    string[] months = {"Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"};
    string[] daysOfTheWeek = {"DOMINGO", "SEGUNDA", "TERÇA", "QUARTA", "QUINTA", "SEXTA", "SÁBADO"};

    void Start()
    {
        defineMonth(localTime);
        defineDay(localTime);
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
    }
    public void previousDay() {
        localTime = localTime.AddDays(-1);
        defineMonth(localTime);
        defineDay(localTime);
    }
}
