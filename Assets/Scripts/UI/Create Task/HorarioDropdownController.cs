using Michsky.UI.ModernUIPack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorarioDropdownController : MonoBehaviour
{
    [SerializeField] CustomDropdown hourDropdown;
    [SerializeField] CustomDropdown minutesDropdown;

    // Start is called before the first frame update
    void Start()
    {
        FillHour();
        FillMinutes();

        hourDropdown.selectedText.text = "HORA";
        minutesDropdown.selectedText.text = "MINUTO";
    }

    void FillHour()
    {
        hourDropdown.dropdownItems.Clear();
        for (int i = 0; i <= 23; i++)
        {
            hourDropdown.CreateNewItem(i.ToString("00"), null);
        }
    }

    void FillMinutes()
    {
        minutesDropdown.dropdownItems.Clear();

        for (int i = 0; i <= 59; i++)
        {
            minutesDropdown.CreateNewItem(i.ToString("00"), null);
        }

        minutesDropdown.index = 0;
    }
}
