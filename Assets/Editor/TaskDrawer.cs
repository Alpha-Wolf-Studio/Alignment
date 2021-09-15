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

        Rect labelRec = new Rect(position.x, position.y, position.width, position.height);
        //position = EditorGUI.PrefixLabel(labelRec, GUIUtility.GetControlID(FocusType.Passive), label);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;

        Task.TaskType taskTypeEnum = (Task.TaskType)property.FindPropertyRelative("type").enumValueIndex;
        Rect typeRec = new Rect(position.x, position.y + 10, position.width, position.height * .25f);
        EditorGUI.PropertyField(typeRec, property.FindPropertyRelative("type"));

        Rect property1Rec = new Rect(position.x, position.y + 35, position.width, position.height * .25f);
        Rect property2Rec = new Rect(position.x, position.y + 60, position.width, position.height * .2f);
        Rect propertyItemRec = new Rect(position.x, position.y + 35, position.width - 4, position.height * .2f);

        switch (taskTypeEnum)
        {
            case Task.TaskType.KILL:
                GUI.backgroundColor = Color.red;
                EditorGUI.PropertyField(property1Rec, property.FindPropertyRelative("dinosaursToKill"));
                EditorGUI.PropertyField(property2Rec, property.FindPropertyRelative("killAmount"));
                break;
            case Task.TaskType.PICKUP:
                GUI.backgroundColor = Color.yellow;
                EditorGUI.PropertyField(propertyItemRec, property.FindPropertyRelative("itemToPickUp"));
                EditorGUI.PropertyField(property2Rec, property.FindPropertyRelative("pickUpAmount"));
                break;
            case Task.TaskType.CRAFT:
                GUI.backgroundColor = Color.green;
                EditorGUI.PropertyField(propertyItemRec, property.FindPropertyRelative("itemToCraft"));
                EditorGUI.PropertyField(property2Rec, property.FindPropertyRelative("craftAmount"));
                break;
            case Task.TaskType.REPAIR:
                GUI.backgroundColor = Color.blue;
                EditorGUI.PropertyField(property1Rec, property.FindPropertyRelative("locationToRepair"));
                break;
            default:
                break;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 100f;
    }

}
