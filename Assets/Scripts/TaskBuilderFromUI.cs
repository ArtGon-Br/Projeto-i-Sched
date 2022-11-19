using UnityEngine;
using Michsky.UI.ModernUIPack;

public class TaskBuilderFromUI : MonoBehaviour
{
    [SerializeField] private CustomInputField _nameField;
    [SerializeField] private CustomInputField _descriptionFIeld;
    [SerializeField] private CustomToggle _isFixField;
    [SerializeField] private CustomDropdown _dayField;
    [SerializeField] private CustomDropdown _monthField;
    [SerializeField] private CustomDropdown _yearField;
    [SerializeField] private CustomDropdown _hourField;
    [SerializeField] private CustomDropdown _minField;
    [SerializeField] private CustomInputField _durationHField;
    [SerializeField] private CustomInputField _durationMinField;
    [SerializeField] private HorizontalSelector _priorityField;

    public TaskData BuildTaskFromUI()
    {
        TaskData newTask = new TaskData
        {
            Name = _nameField.inputText.text,
            Description = _descriptionFIeld.inputText.text,
            isFix = _isFixField.toggleObject.isOn,
            Day = (_dayField.selectedItemIndex + 1).ToString(),
            Month = (_monthField.selectedItemIndex + 1).ToString(),
            Year = _yearField.dropdownItems[_yearField.selectedItemIndex].itemName,
            Hour = _hourField.selectedItemIndex,
            Min = _minField.selectedItemIndex,
            DurationH = _durationHField.inputText.text,
            DurationMin = _durationMinField.inputText.text,
            Priority = _priorityField.index
        };

        return newTask;
    }
}
