using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(SubQuest))]
public class SubQuestDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        EditorGUI.BeginProperty(position, label, property);

        //Rect labelRec = new Rect(position.x, position.y, position.width, position.height);
        //position = EditorGUI.PrefixLabel(labelRec, GUIUtility.GetControlID(FocusType.Passive), label);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;

        Rect fullRect = new Rect(position.x, position.y + 5, position.width, position.height - 5);

        EditorGUI.DrawRect(fullRect, new Color(0.25f, 0.25f, 0.25f, 1));

        Rect typeRec = new Rect(position.x, position.y + 10, position.width, position.height * .10f);
        EditorGUI.PropertyField(typeRec, property.FindPropertyRelative(nameof(SubQuest.hasCustomDescription)));
        bool hasCustomDesc = property.FindPropertyRelative(nameof(SubQuest.hasCustomDescription)).boolValue;
        if (hasCustomDesc) 
        {
            Rect typeRec2 = new Rect(position.x, position.y + 35, position.width, position.height * .15f);
            EditorGUI.PropertyField(typeRec2, property.FindPropertyRelative(nameof(SubQuest.customDescription)));
        }

        SubQuest.SubQuestType taskTypeEnum = (SubQuest.SubQuestType)property.FindPropertyRelative(nameof(SubQuest.type)).enumValueIndex;
        Rect typeRec3 = new Rect(position.x, position.y + 60, position.width, position.height * .25f);
        EditorGUI.PropertyField(typeRec3, property.FindPropertyRelative(nameof(SubQuest.type)));

        Rect property1Rec = new Rect(position.x, position.y + 85, position.width, position.height * .25f);
        Rect property2Rec = new Rect(position.x, position.y + 115, position.width, position.height * .15f);
        Rect propertyItemRec = new Rect(position.x, position.y + 85, position.width - 4, position.height * .15f);

        switch (taskTypeEnum)
        {
            case SubQuest.SubQuestType.KILL:
                GUI.backgroundColor = Color.red;
                EditorGUI.PropertyField(property1Rec, property.FindPropertyRelative(nameof(SubQuest.dinosaursToKill)));
                EditorGUI.PropertyField(property2Rec, property.FindPropertyRelative(nameof(SubQuest.killAmount)));
                //EditorGUI.PropertyField(property3Rec, property.FindPropertyRelative(nameof(Task.completed)));
                break;
            case SubQuest.SubQuestType.PICKUP:
                GUI.backgroundColor = Color.yellow;
                EditorGUI.PropertyField(propertyItemRec, property.FindPropertyRelative(nameof(SubQuest.itemToPickUp)));
                EditorGUI.PropertyField(property2Rec, property.FindPropertyRelative(nameof(SubQuest.pickUpAmount)));
                //EditorGUI.PropertyField(property3Rec, property.FindPropertyRelative(nameof(Task.completed)));
                break;
            case SubQuest.SubQuestType.CRAFT:
                GUI.backgroundColor = Color.green;
                EditorGUI.PropertyField(propertyItemRec, property.FindPropertyRelative(nameof(SubQuest.itemToCraft)));
                EditorGUI.PropertyField(property2Rec, property.FindPropertyRelative(nameof(SubQuest.craftAmount)));
                //EditorGUI.PropertyField(property3Rec, property.FindPropertyRelative(nameof(Task.completed)));
                break;
            case SubQuest.SubQuestType.REPAIR:
                GUI.backgroundColor = Color.blue;
                EditorGUI.PropertyField(property1Rec, property.FindPropertyRelative(nameof(SubQuest.locationToRepair)));
                //EditorGUI.PropertyField(property3Rec, property.FindPropertyRelative(nameof(Task.completed)));
                break;
            default:
                break;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SubQuest.SubQuestType taskTypeEnum = (SubQuest.SubQuestType)property.FindPropertyRelative(nameof(SubQuest.type)).enumValueIndex;
        if(taskTypeEnum == SubQuest.SubQuestType.REPAIR) 
        {
            return 120f;
        }
        else 
        {
            return 150f;
        }
    }
}
