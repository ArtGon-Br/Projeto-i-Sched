using UnityEngine;
using TMPro;

public class TaskUI : MonoBehaviour
{
    [SerializeField] TMP_Text hourField;
    [SerializeField] TMP_Text taskNameField;

    //TODO: remover esse field do inspector
    [SerializeField] TaskSO currentTask;

    private void Start()
    {
        UpdateLabels();
    }

    // Essa função deve ser chamada toda vez que um Task UI for instanciado
    public void SetTask(TaskSO task)
    {
        currentTask = task;
        UpdateLabels();
    }

    void UpdateLabels()
    {
        if (currentTask == null) return;

        hourField.text = currentTask.GetHour();
        taskNameField.text = currentTask.GetName();
    }
}
