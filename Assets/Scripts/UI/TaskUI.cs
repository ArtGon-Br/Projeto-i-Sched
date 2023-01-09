using UnityEngine;
using TMPro;

public class TaskUI : MonoBehaviour
{
    [SerializeField] TMP_Text dateField;
    [SerializeField] TMP_Text hourField;
    [SerializeField] TMP_Text taskNameField;
        
    TaskData currentTask;

    // Essa função deve ser chamada toda vez que um Task UI for instanciado
    public void SetTask(TaskData task, bool showDate = default)
    {
        currentTask = task;
        UpdateLabels(showDate);
    }

    void UpdateLabels(bool showDate)
    {
        hourField.text = $"{currentTask.StartTime.Hour.ToString("00")} : {currentTask.StartTime.Minute.ToString("00")}";
        taskNameField.text = currentTask.Name;

        if (showDate)
        {
            dateField.text = $"{currentTask.StartTime.Day}/{currentTask.StartTime.Month}";
            dateField.gameObject.SetActive(true);
        }
        else dateField.gameObject.SetActive(false);
    }

}
