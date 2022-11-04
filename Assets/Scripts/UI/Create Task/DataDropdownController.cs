using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using System;
using UnityEngine.UI;

public class DataDropdownController : MonoBehaviour
{
    [SerializeField] CustomDropdown dayDropdown;
    [SerializeField] CustomDropdown monthDropdown;
    [SerializeField] CustomDropdown yearDropdown;

    // Start is called before the first frame update
    void Start()
    {
        FillDay();
        FillYear();

        dayDropdown.selectedText.text = "DIA";
        monthDropdown.selectedText.text = "MÊS";
        yearDropdown.selectedText.text = "ANO";
    }

    void FillDay()
    {
        dayDropdown.dropdownItems.Clear();
        for (int i = 1; i <= 31; i++)
        {
            dayDropdown.CreateNewItem(i.ToString("00"), null);
        }
    }

    void FillYear()
    {
        yearDropdown.dropdownItems.Clear();

        int currentYear = DateTime.Now.Year;

        for(int i = currentYear; i <= currentYear+30; i++)
        {
            yearDropdown.CreateNewItem(i.ToString(), null);
        }

        yearDropdown.index = 0;
        yearDropdown.scrollbar.GetComponent<Scrollbar>().value = 1;
    }
}
