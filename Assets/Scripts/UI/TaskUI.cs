using UnityEngine;
using TMPro;
using System.Text;
using System.Linq;
using System;

public class TaskUI : MonoBehaviour
{
    [SerializeField] TMP_Text dateField;
    [SerializeField] TMP_Text hourField;
    [SerializeField] TMP_Text taskNameField;

    //TODO: remover esse field do inspector
    [SerializeField] TaskData currentTask;

    // Essa função deve ser chamada toda vez que um Task UI for instanciado
    public void SetTask(TaskData task, bool showDate = default)
    {
        currentTask = task;
        UpdateLabels(showDate);
    }

    void UpdateLabels(bool showDate)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(currentTask.Hour.ToString());
        builder.Append(":");
        if (currentTask.Min < 10)
        {
            builder.Append("0");
        }
        builder.Append(currentTask.Min.ToString());

        hourField.text = builder.ToString();
        builder.Clear();

        builder.Append(currentTask.Name[0].ToString().ToUpper());
        builder.Append(currentTask.Name.Substring(1, currentTask.Name.Length - 1));
        taskNameField.text = builder.ToString();

        if (showDate)
        {
            dateField.text = $"{currentTask.Day}/{currentTask.Month}";
            dateField.gameObject.SetActive(true);
        }
        else dateField.gameObject.SetActive(false);
    }

}
