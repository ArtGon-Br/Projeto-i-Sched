using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatedTaskItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _taskName;
    [SerializeField] private TextMeshProUGUI _taskDateTime;
    [SerializeField] private Image _background;
    [SerializeField] private Color _createdColor;
    [SerializeField] private Color _reallocatedColor;

    public void SetUI(TaskData task, bool created)
    {
        _background.color = created ? _createdColor : _reallocatedColor;
        _taskName.text = task.Name;
        string reallocated = created ? string.Empty : "Realocada para ->";
        _taskDateTime.text = $"{reallocated} {task.StartTime.ToLongDateString()} às {task.StartTime.ToShortTimeString()}";
    }
}
