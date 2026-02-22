using UnityEngine;

namespace Tools
{
    public class ScriptTemplate : ScriptableObject
    {
        [Tooltip("The default name assigned to the script on creation.")]
        public string DefaultName;

        [Tooltip("The extension of the script.")]
        [SerializeField] private string extension = ".cs";
        public string Extension => extension;

        public enum UnityIconName
        {
            CSharpScript,
            ScriptableObject,
            Text,
            Custom
        }

        [Tooltip("The icon name that will decide the icon used in the asset folder.")]
        [SerializeField] private UnityIconName iconName = UnityIconName.CSharpScript;
        public string GetIconName()
        {
            return iconName switch
            {
                UnityIconName.CSharpScript => "cs Script Icon",
                UnityIconName.ScriptableObject => "ScriptableObject Icon",
                UnityIconName.Text => "TextAsset Icon",
                _ => string.Empty,
            };
        }

        [Tooltip("Custom icon used in the asset folder. Will be ignored if kept empty.")]
        [SerializeField] private Texture2D icon;
        public Texture2D Icon => icon;


        [Tooltip("The code template you want to load into a newly created script. Use placeholders <{placeholder}> for the script name (also class name).")]
        [TextArea(40, 40)]
        public string Template;
    }    
}

