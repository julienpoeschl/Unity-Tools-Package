using UnityEditor;

namespace Tools
{
    [CustomEditor(typeof(ScriptTemplate))]
    public class ScriptTemplateEditor : Editor
    {

        private ScriptTemplate scriptTemplate;

        void OnEnable()
        {
            scriptTemplate = (ScriptTemplate)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty defaultNameProp = serializedObject.FindProperty("DefaultName");
            SerializedProperty extensionProp = serializedObject.FindProperty("extension");
            SerializedProperty iconNameProp = serializedObject.FindProperty("iconName");
            SerializedProperty iconProp = serializedObject.FindProperty("icon");
            SerializedProperty templateProp = serializedObject.FindProperty("Template");
 
            EditorGUILayout.PropertyField(defaultNameProp);
            EditorGUILayout.PropertyField(extensionProp);
            EditorGUILayout.PropertyField(iconNameProp);
 
            if (iconNameProp.enumValueIndex == (int)ScriptTemplate.UnityIconName.Custom)
                EditorGUILayout.PropertyField(iconProp);
 
            EditorGUILayout.PropertyField(templateProp);

            serializedObject.ApplyModifiedProperties();
        }
    }

}
