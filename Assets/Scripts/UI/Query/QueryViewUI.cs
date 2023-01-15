using System;
using TMPro;
using UnityEngine;

public class QueryViewUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _date;
    [SerializeField] private TMP_Text _hour;
    [SerializeField] private TMP_Text _name;

    public void UpdateText(DateTime date, string taskName)
    {
        _date.text = date.ToShortDateString();
        _hour.text = date.ToShortTimeString();
        _name.text = taskName;

        Debug.Log("Text updated");
    }
}
