using UnityEngine;
using Michsky.UI.ModernUIPack;

public class TaskBuilderFromUI : MonoBehaviour
{
    [System.Serializable]
    class RepeatDays
    {
        [SerializeField] private CustomToggle _monday;
        [SerializeField] private CustomToggle _tuesday;
        [SerializeField] private CustomToggle _wednesday;
        [SerializeField] private CustomToggle _thursday;
        [SerializeField] private CustomToggle _friday;
        [SerializeField] private CustomToggle _saturday;
        [SerializeField] private CustomToggle _sunday;

        public string GetRepeatedDays()
        {
            string result = "";

            result += _monday.toggleObject.isOn ? "1" : "0";
            result += _tuesday.toggleObject.isOn ? "1" : "0";
            result += _wednesday.toggleObject.isOn ? "1" : "0";
            result += _thursday.toggleObject.isOn ? "1" : "0";
            result += _friday.toggleObject.isOn ? "1" : "0";
            result += _saturday.toggleObject.isOn ? "1" : "0";
            result += _sunday.toggleObject.isOn ? "1" : "0";

            return result;
        }
    }

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
    [SerializeField] private CustomDropdown _fixedTaskHourField;
    [SerializeField] private CustomDropdown _fixedTaskMinField;
    [SerializeField] private CustomInputField _fixedTaskHourDurationField;
    [SerializeField] private CustomInputField _fixedTaskMinDurationField;
    [SerializeField] private RepeatDays _repeatedDays;

    public TaskData BuildTaskFromUI()
    {
        if(_isFixField.toggleObject.isOn)
        {
            return BuildFixedTask();
        }
        else
        {
            return BuildDynamicTask();
        }
    }

    private TaskData BuildDynamicTask()
    {
        TaskData newTask = new TaskData
        {
            Name = _nameField.inputText.text,
            Description = _descriptionFIeld.inputText.text,
            isFix = false,
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

    private TaskData BuildFixedTask()
    {
        TaskData newTask = new TaskData
        {
            Name = _nameField.inputText.text,
            Description = _descriptionFIeld.inputText.text,
            isFix = true,
            Day = (_dayFromField.selectedItemIndex + 1).ToString(),
            Month = (_monthFromField.selectedItemIndex + 1).ToString(),
            Year = _yearFromField.dropdownItems[_yearFromField.selectedItemIndex].itemName,
            DayTo = (_dayToField.selectedItemIndex + 1).ToString(),
            MonthTo = (_monthToField.selectedItemIndex + 1).ToString(),
            YearTo = _yearToField.dropdownItems[_yearToField.selectedItemIndex].itemName,
            Hour = _fixedTaskHourField.selectedItemIndex,
            Min = _fixedTaskMinField.selectedItemIndex,
            HourDuration = int.Parse(_fixedTaskHourDurationField.inputText.text),
            MinutesDuration = int.Parse(_fixedTaskMinDurationField.inputText.text),
            RepeatDays = _repeatedDays.GetRepeatedDays()
        };

        return newTask;
    }
}
