using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CalendarController : MonoBehaviour
{
    int _firstDayOfMonth;                                                           //Primeiro dia da semana do mes
    int _currentShowType                = 0;
    bool _changeMonth                   = false;
    System.DateTime day                 = System.DateTime.UtcNow.ToLocalTime();     //Dia atual
    [SerializeField] Button[] Days;                                                 //Texto dos dias
    [SerializeField] GameObject[] _showTypes;
    [SerializeField] Text _ano, _mes;                                               // Texto do ano e do mes

    [SerializeField] WindowManager window;

    private void Start()
    {

    }

    public void Initialize()
    {
        if (window.currentWindowIndex != 1) return;
        while (day.Day != 1)
        {
            day = day.AddDays(-1);
        }
        _ano.text = day.Year.ToString();
        _mes.text = day.ToString("MMM");
        switch (day.DayOfWeek.ToString())
        {
            case "Monday":
                _firstDayOfMonth = 1;
                break;
            case "Tuesday":
                _firstDayOfMonth = 2;
                break;
            case "Wednesday":
                _firstDayOfMonth = 3;
                break;
            case "Thursday":
                _firstDayOfMonth = 4;
                break;
            case "Friday":
                _firstDayOfMonth = 5;
                break;
            case "Saturday":
                _firstDayOfMonth = 6;
                break;
            case "Sunday":
                _firstDayOfMonth = 0;
                break;
        }
        for (int i = 0; i < 42; i++)
        {
            var temp = Days[i].GetComponentInChildren<Text>();
            temp.text = day.AddDays(i - _firstDayOfMonth).Day.ToString();

            if (day.AddDays(i - _firstDayOfMonth).Month != day.Month)
            {
                temp.color = new Color(255f, 255f, 255f, 0.4f);
            }
            else
            {
                temp.color = new Color(255f, 255f, 255f, 1f);
                Days[i].GetComponent<DayMannager>().setDate(temp.text, _mes.text, _ano.text);
            }
        }
    }

    public void ChangeMonth(int p){
        day = day.AddMonths(p);
        while(day.Day != 1){
            day = day.AddDays(-1);
        }
        _ano.text = day.Year.ToString();
        _mes.text = day.ToString("MMM");
        switch (day.DayOfWeek.ToString())
        {
            case "Monday":
                _firstDayOfMonth = 1;
            break;
            case "Tuesday":
                _firstDayOfMonth = 2;
            break;
            case "Wednesday":
                _firstDayOfMonth = 3;
            break;
            case "Thursday":
                _firstDayOfMonth = 4;
            break;
            case "Friday":
                _firstDayOfMonth = 5;
            break;
            case "Saturday":
                _firstDayOfMonth = 6;
            break;
            case "Sunday":
                _firstDayOfMonth = 0;
            break;
        }     
        for (int i = 0; i < 42; i++)
        {
            var temp = Days[i].GetComponentInChildren<Text>();
            temp.text = day.AddDays(i-_firstDayOfMonth).Day.ToString();
            if(day.AddDays(i-_firstDayOfMonth).Month != day.Month){
                temp.color = new Color(255f,255f,255f,0.4f);
            }else{
                temp.color = new Color(255f,255f,255f,1f);
                Days[i].GetComponent<DayMannager>().setDate(temp.text, _mes.text, _ano.text);
            }
        }
    }


    public void ChangeShowType(int _case){
        _showTypes[_currentShowType].SetActive(false);
        switch (_case%10)
        {
            case 1:
                _currentShowType = 1;
                _showTypes[_currentShowType].SetActive(true);
            break;
            case 2:
                _currentShowType = 0;
                switch (Mathf.Abs(_case/10))
                {
                    case 0:
                    break;
                    case 1:
                        ChangeMonth(-(day.Month - 1));     
                    break;
                    case 2:
                        ChangeMonth(-(day.Month - 2)); 
                    break;
                    case 3:
                        ChangeMonth(-(day.Month - 3)); 
                    break;
                    case 4:
                        ChangeMonth(-(day.Month - 4)); 
                    break;
                    case 5:
                        ChangeMonth(-(day.Month - 5)); 
                    break;
                    case 6:
                        ChangeMonth(-(day.Month - 6)); 
                    break;
                    case 7:
                        ChangeMonth(-(day.Month - 7)); 
                    break;
                    case 8:
                        ChangeMonth(-(day.Month - 8)); 
                    break;
                    case 9:
                        ChangeMonth(-(day.Month - 9)); 
                    break;
                    case 10:
                        ChangeMonth(-(day.Month - 10)); 
                    break;
                    case 11:
                        ChangeMonth(-(day.Month - 11)); 
                    break;
                    case 12:
                        ChangeMonth(-(day.Month - 12)); 
                    break;
                }
                _showTypes[_currentShowType].SetActive(true);
            break;
        }
    }
}
