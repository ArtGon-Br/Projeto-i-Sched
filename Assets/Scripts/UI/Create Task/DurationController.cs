using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DurationController : MonoBehaviour
{
    [SerializeField] TMP_InputField hourInput;
    [SerializeField] TMP_InputField minInput;

    private void OnEnable()
    {
        hourInput.SetTextWithoutNotify("00");
        minInput.SetTextWithoutNotify("00");

        hourInput.onEndEdit.AddListener(x =>
        {
            int value = int.Parse(x);
            if (value < 0) value = 0;

            hourInput.SetTextWithoutNotify(value.ToString("00"));
        });

        minInput.onEndEdit.AddListener(x =>
        {
            int value = int.Parse(x);
            if (value > 59)
            {
                int hoursToAdd = value / 60;
                value = value % 60;
                minInput.SetTextWithoutNotify(value.ToString("00"));

                int currentHours = int.Parse(hourInput.text);
                currentHours += hoursToAdd;
                hourInput.SetTextWithoutNotify(currentHours.ToString("00"));
            }

            if (value < 0) value = 0;
            minInput.SetTextWithoutNotify(value.ToString("00"));
        });
    }
}
