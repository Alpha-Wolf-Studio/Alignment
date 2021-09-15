using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Task))]
public class TaskDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        EditorGUI.BeginProperty(position, label, property);

        //Rect labelRec = new Rect(position.x, position.y, position.width, position.height);
        //position = EditorGUI.PrefixLabel(labelRec, GUIUtility.GetControlID(FocusType.Passive), label);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;

        Task.TaskType taskTypeEnum = (Task.TaskType)property.FindPropertyRelative(nameof(Task.type)).enumValueIndex;
        Rect typeRec = new Rect(position.x, position.y + 10, position.width, position.height * .25f);
        EditorGUI.PropertyField(typeRec, property.FindPropertyRelative(nameof(Task.type)));

        Rect property1Rec = new Rect(position.x, position.y + 35, position.width, position.height * .25f);
        Rect property2Rec = new Rect(position.x, position.y + 60, position.width, position.height * .2f);
        Rect propertyItemRec = new Rect(position.x, position.y + 35, position.width - 4, position.height * .2f);

        switch (taskTypeEnum)
        {
            case Task.TaskType.KILL:
                GUI.backgroundColor = Color.red;
                EditorGUI.PropertyField(property1Rec, property.FindPropertyRelative(nameof(Task.dinosaursToKill)));
                EditorGUI.PropertyField(property2Rec, property.FindPropertyRelative(nameof(Task.killAmount)));
                break;
            case Task.TaskType.PICKUP:
                GUI.backgroundColor = Color.yellow;
                EditorGUI.PropertyField(propertyItemRec, property.FindPropertyRelative(nameof(Task.itemToPickUp)));
                EditorGUI.PropertyField(property2Rec, property.FindPropertyRelative(nameof(Task.pickUpAmount)));
                break;
            case Task.TaskType.CRAFT:
                GUI.backgroundColor = Color.green;
                EditorGUI.PropertyField(propertyItemRec, property.FindPropertyRelative(nameof(Task.itemToCraft)));
                EditorGUI.PropertyField(property2Rec, property.FindPropertyRelative(nameof(Task.craftAmount)));
                break;
            case Task.TaskType.REPAIR:
                GUI.backgroundColor = Color.blue;
                EditorGUI.PropertyField(property1Rec, property.FindPropertyRelative(nameof(Task.locationToRepair)));
                break;
            default:
                break;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        Task.TaskType taskTypeEnum = (Task.TaskType)property.FindPropertyRelative(nameof(Task.type)).enumValueIndex;
        if(taskTypeEnum == Task.TaskType.REPAIR) 
        {
            return 70f;
        }
        else 
        {
            return 100f;
        }
    }
}
