using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Michsky.UI.ModernUIPack;
using Firebase.Auth;


public class TaskMannager : MonoBehaviour
{

    [SerializeField] private  CustomInputField _nameField;
    [SerializeField] private  CustomInputField _descriptionFIeld;
    [SerializeField] private  CustomToggle _isFixField;
    [SerializeField] private  CustomDropdown _dayField;
    [SerializeField] private  CustomDropdown _monthField;
    [SerializeField] private  CustomDropdown _yearField;
    [SerializeField] private  CustomDropdown _hourField;
    [SerializeField] private  CustomDropdown _minField;
    [SerializeField] private  CustomInputField _durationHField;
    [SerializeField] private  CustomInputField _durationMinField;
    [SerializeField] private  HorizontalSelector _priorityField;
    [SerializeField] private  ButtonManager _addButton;
    
    Firebase.Auth.FirebaseAuth auth;
    FirebaseFirestore _db;
    ListenerRegistration _listenerRegistration;



    private void Start() {
        int tasks = 0, h, m;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        _db = FirebaseFirestore.DefaultInstance;
        _listenerRegistration = _db.Collection(path:"users_sheet").Document(path:auth.CurrentUser.UserId.ToString()).Listen(snapshot =>
        {
            UserData data = snapshot.ConvertTo<UserData>();
            tasks = data.Tasks;
        });
        if(_durationHField.inputText.text[0] == '0'){
            h = int.Parse(_durationHField.inputText.text[1].ToString());
        }else{
            h = int.Parse(_durationHField.inputText.text);
        } 
        if(_durationMinField.inputText.text[0] == '0'){
            m = int.Parse(_durationMinField.inputText.text[1].ToString());
        }else{
            m = int.Parse(_durationMinField.inputText.text);
        }
        _addButton.clickEvent.AddListener(() => {
            var _taskController = new TaskController{
                Name = _nameField.inputText.text,
                Description = _descriptionFIeld.inputText.text,
                isFix = _isFixField.toggleObject.isOn,
                Day = _dayField.selectedItemIndex + 1,
                Month = _monthField.selectedItemIndex + 1,
                Year = int.Parse(_yearField.dropdownItems[_yearField.selectedItemIndex].itemName),
                Hour = _hourField.selectedItemIndex,
                Min = _minField.selectedItemIndex, 
                DurationH = h,
                DurationMin = m,
                Priority = _priorityField.index
            };
            var _userData = new UserData{
                Tasks =  tasks + 1
            };
            var Firestore = FirebaseFirestore.DefaultInstance;
            Firestore.Collection(path:"users_sheet").Document(path:auth.CurrentUser.UserId.ToString()).SetAsync(_userData);
            Firestore.Collection(path:"users_sheet").Document(path:auth.CurrentUser.UserId.ToString()).Collection(path:"Tasks").Document(path:tasks.ToString()).SetAsync(_taskController);
        });
    }
    private void OnDestroy() {
        _listenerRegistration.Stop();
    }
}
