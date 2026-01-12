using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Skill_Base))]
public class Skill_BaseEditor : Editor
{
    SerializedProperty skillTypeProp;
    SerializedProperty upgradeTypeProp;

    void OnEnable()
    {
        skillTypeProp = serializedObject.FindProperty("skillType");
        upgradeTypeProp = serializedObject.FindProperty("upgradeType");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(skillTypeProp);

        // Build filtered list of upgrade types based on selected skill type
        var enumNames = System.Enum.GetNames(typeof(SkillUpgradeType));

        string prefix = GetPrefixForSkillType((SkillType)skillTypeProp.enumValueIndex);

        List<string> displayNames = new List<string>();
        List<int> enumIndices = new List<int>();

        for (int i = 0; i < enumNames.Length; i++)
        {
            string name = enumNames[i];

            if (name == "None" || (!string.IsNullOrEmpty(prefix) && name.StartsWith(prefix)))
            {
                displayNames.Add(name);
                enumIndices.Add(i); // enum index, not underlying value
            }
        }

        // Ensure we always have at least the `None` option
        if (displayNames.Count == 0)
        {
            displayNames.Add("None");
            enumIndices.Add(0);
        }

        int currentEnumIndex = upgradeTypeProp.enumValueIndex;
        int selectedIndex = enumIndices.IndexOf(currentEnumIndex);
        if (selectedIndex < 0) selectedIndex = 0;

        selectedIndex = EditorGUILayout.Popup("Upgrade Type", selectedIndex, displayNames.ToArray());
        upgradeTypeProp.enumValueIndex = enumIndices[selectedIndex];

        // Draw the rest of the properties (except skillType & upgradeType which we've handled)
        DrawPropertiesExcluding(serializedObject, "skillType", "upgradeType");

        serializedObject.ApplyModifiedProperties();
    }

    string GetPrefixForSkillType(SkillType st)
    {
        switch (st)
        {
            case SkillType.Dash: return "Dash";
            case SkillType.TimeEcho: return "TimeEcho";
            case SkillType.TimeShard: return "Shard"; // enum uses "Shard" for TimeShard
            case SkillType.SwordThrow: return "SwordThrow";
            default: return string.Empty;
        }
    }
}
