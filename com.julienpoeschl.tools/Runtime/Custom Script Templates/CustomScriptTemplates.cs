using UnityEditor;
using System.IO;
using UnityEngine;
using UnityEditor.ProjectWindowCallback;
using System;

namespace Tools
{

    public static class CustomScriptTemplates
    {

        private static string root = "Packages";
        private static string packageRuntimeDir = Path.Combine("com.julienpoeschl.tools", "Runtime");
        private static string defaultAssetName = $"NewScript{templateAssetSuffix}";
        private static string templateDir = Path.Combine(root, packageRuntimeDir, "Custom Script Templates");
        private const string templateAssetSuffix = "Template.asset";

        [MenuItem("Tools/Custom Scripts/Create Script Template", priority = 50)]
        public static void CreateScriptTemplate()
        {
            string path = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(templateDir, defaultAssetName));

            CreateAsset action = ScriptableObject.CreateInstance<CreateAsset>();
            action.Init(typeof(ScriptTemplate));

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
            0,
            action,
            path,
            EditorGUIUtility.IconContent("ScriptableObject Icon").image as Texture2D,
            null
            );
        }

        private static void CreateScript(string templatePath)
        {
            if (!File.Exists(templatePath))
            {
                Debug.LogError($"The template at {templatePath} doesn't exist.");
                return;
            }

            ScriptTemplate template = AssetDatabase.LoadAssetAtPath<ScriptTemplate>(templatePath);

            if (template == null)
            {
                Debug.LogError("Script template not found at path: " + templatePath);
                return;
            }

            var action = ScriptableObject.CreateInstance<CreateScriptFromTemplateAction>();
            action.Init(template);

            string defaultName = template.DefaultName;
            if (!defaultName.EndsWith(".cs"))
                defaultName += ".cs";

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,
                action,
                Path.Combine(GetSelectedPathOrFallback(), defaultName),
                EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D,
                null
            );   
        }

        #region MonoBehaviour

        private static string monoBehaviourTemplatePath = Path.Combine(templateDir, $"MonoBehaviour{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/MonoBehaviour", false, 0)]
        public static void CreateMonoBehaviourScript()
        {
            CreateScript(monoBehaviourTemplatePath);
        }

        private static string abstractMonoBehaviourTemplatePath = Path.Combine(templateDir, $"AbstractMonoBehaviour{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/Abstract/MonoBehaviour", false, 0)]
        public static void CreateAbstractMonoBehaviourScript()
        {
            CreateScript(abstractMonoBehaviourTemplatePath);
        }

        #endregion

        #region Manager

        private static string managerTemplatePath = Path.Combine(templateDir, $"Manager{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/Manager", false, 1)]
        public static void CreateManagerScript()
        {
            CreateScript(managerTemplatePath);
        }

        #endregion

        #region Scriptable Object

        private static string scriptableObjectTemplatePath = Path.Combine(templateDir, $"ScriptableObject{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/Scriptable Object", false, 3)]
        public static void CreateScriptableObjectScript()
        {
            CreateScript(scriptableObjectTemplatePath);
        }

        private static string abstractScriptableObjectTemplatePath = Path.Combine(templateDir, $"AbstractScriptableObject{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/Abstract/Scriptable Object", false, 3)]
        public static void CreateAbstractScriptableObjectScript()
        {
            CreateScript(abstractScriptableObjectTemplatePath);
        }

        #endregion

        #region Class
        private static string classTemplatePath = Path.Combine(templateDir, $"Class{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/Class", false, 4)]
        public static void CreateClassScript()
        {
            CreateScript(classTemplatePath);
        }

        private static string abstractClassTemplatePath = Path.Combine(templateDir, $"AbstractClass{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/Abstract/Class", false, 4)]
        public static void CreateAbstractClassScript()
        {
            CreateScript(abstractClassTemplatePath);
        }

        #endregion

        #region Interface

        private static string interfaceTemplatePath = Path.Combine(templateDir, $"Interface{templateAssetSuffix}");
        private const string menuItem = "";
        [MenuItem("Assets/Create/Scripts/Interface", false, 4)]
        public static void CreateInterfaceScript()
        {
            CreateScript(interfaceTemplatePath);
        }

        #endregion
        
        #region Enum

        private static string enumTemplatePath = Path.Combine(templateDir, $"Enum{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/Enum", false, 4)]
        public static void CreateEnumScript()
        {
            CreateScript(enumTemplatePath);
        }

        #endregion



        private static string GetSelectedPathOrFallback()
        {
            string path = root;
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (File.Exists(path)) { path = Path.GetDirectoryName(path); }
                break;
            }
            return path;
        }

    }

    class CreateAsset : EndNameEditAction
    {
        private Type type;

        public void Init(Type type)
        {
            if (type == null)
            {
                Debug.LogError("Type was null.");
                return;
            }

            this.type = type;
        }

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            ScriptableObject asset = CreateInstance(type);
            AssetDatabase.CreateAsset(asset, pathName);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }

    public class CreateScriptFromTemplateAction : EndNameEditAction
    {
        private ScriptTemplate scriptTemplate;
        private const string templateNamePlaceholder = "FileName";
        private const string extension = ".cs";

        public void Init(ScriptTemplate template)
        {
            this.scriptTemplate = template;
        }

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            if (pathName == string.Empty) pathName = scriptTemplate.DefaultName;

            string name = Path.GetFileNameWithoutExtension(pathName);
            string code = scriptTemplate.Template.Replace(templateNamePlaceholder, name);

            File.WriteAllText(pathName, code);
            AssetDatabase.Refresh();

            if (string.IsNullOrEmpty(Path.GetExtension(pathName))) pathName += extension;

            MonoScript asset = AssetDatabase.LoadAssetAtPath<MonoScript>(pathName);
            ProjectWindowUtil.ShowCreatedAsset(asset);
        }
    }
}