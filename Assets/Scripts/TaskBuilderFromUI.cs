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

    [Header("Fixed task")]
    [SerializeField] private CustomDropdown _dayFromField;
    [SerializeField] private CustomDropdown _monthFromField;
    [SerializeField] private CustomDropdown _yearFromField;
    [SerializeField] private CustomDropdown _dayToField;
    [SerializeField] private CustomDropdown _monthToField;
    [SerializeField] private CustomDropdown _yearToField;

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
            HourDuration = int.Parse(_durationHField.inputText.text),
            MinutesDuration = int.Parse(_durationMinField.inputText.text),
            Priority = _priorityField.index
        };

        return newTask;
    }
}
