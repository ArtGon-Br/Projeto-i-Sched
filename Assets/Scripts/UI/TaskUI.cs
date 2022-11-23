using UnityEngine;
using TMPro;
using System.Text;
using System.Linq;

public class TaskUI : MonoBehaviour
{
    [SerializeField] TMP_Text hourField;
    [SerializeField] TMP_Text taskNameField;

    //TODO: remover esse field do inspector
    [SerializeField] TaskData currentTask;

    private void Start()
    {
        UpdateLabels();
    }

    // Essa função deve ser chamada toda vez que um Task UI for instanciado
    public void SetTask(TaskData task)
    {
        currentTask = task;
        UpdateLabelsInDailyVisualization();
    }

    void UpdateLabels()
    {
        
    }

    void UpdateLabelsInDailyVisualization()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(currentTask.Hour.ToString());
        builder.Append(":");
        if(currentTask.Min < 10)
        {
            builder.Append("0");
        }
        builder.Append(currentTask.Min.ToString());
        
        hourField.text = builder.ToString();
        builder.Clear();

        builder.Append(currentTask.Name[0].ToString().ToUpper());
        builder.Append(currentTask.Name.Substring(1, currentTask.Name.Length - 1));
        taskNameField.text = builder.ToString();
    }
}
