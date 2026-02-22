using UnityEditor;
using System.IO;
using UnityEngine;
using UnityEditor.ProjectWindowCallback;
using System;

namespace Tools
{

    public static class ScriptTemplateMenuItem
    {

        private static readonly string root = "Packages";
        private static readonly string packageRuntimeDir = Path.Combine("com.julienpoeschl.tools", "Runtime");
        private static readonly string defaultAssetName = $"NewScript{templateAssetSuffix}";
        private static readonly string templateDir = Path.Combine(root, packageRuntimeDir, "Custom Script Templates");
        private const string templateAssetSuffix = "Template.asset";

        [MenuItem("Tools/Custom Scripts/Create Script Template", priority = 50)]
        public static void CreateScriptTemplate()
        {
            string path = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(templateDir, defaultAssetName));

            EndAssetAction action = CreateAction<EndAssetAction, Type>.Retreive(typeof(ScriptTemplate));

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

            EndScriptTemplateAction action = CreateAction<EndScriptTemplateAction, ScriptTemplate>.Retreive(template);

            string defaultName = template.DefaultName;
            if (!defaultName.EndsWith(template.Extension))
                defaultName += template.Extension;

            Texture2D icon = template.Icon != null ? template.Icon : EditorGUIUtility.IconContent(template.GetIconName()).image as Texture2D;

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,
                action,
                Path.Combine(GetSelectedPathOrFallback(), defaultName),
                icon,
                null
            );   
        }

        #region MonoBehaviour

        private static readonly string monoBehaviourTemplatePath = Path.Combine(templateDir, $"MonoBehaviour{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/MonoBehaviour", false, 0)]
        public static void CreateMonoBehaviourScript()
        {
            CreateScript(monoBehaviourTemplatePath);
        }

        private static readonly string abstractMonoBehaviourTemplatePath = Path.Combine(templateDir, $"AbstractMonoBehaviour{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/Abstract/MonoBehaviour", false, 0)]
        public static void CreateAbstractMonoBehaviourScript()
        {
            CreateScript(abstractMonoBehaviourTemplatePath);
        }

        #endregion

        #region Manager

        private static readonly string managerTemplatePath = Path.Combine(templateDir, $"Manager{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/Manager", false, 1)]
        public static void CreateManagerScript()
        {
            CreateScript(managerTemplatePath);
        }

        #endregion

        #region Scriptable Object

        private static readonly string scriptableObjectTemplatePath = Path.Combine(templateDir, $"ScriptableObject{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/Scriptable Object", false, 3)]
        public static void CreateScriptableObjectScript()
        {
            CreateScript(scriptableObjectTemplatePath);
        }

        private static readonly string abstractScriptableObjectTemplatePath = Path.Combine(templateDir, $"AbstractScriptableObject{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/Abstract/Scriptable Object", false, 3)]
        public static void CreateAbstractScriptableObjectScript()
        {
            CreateScript(abstractScriptableObjectTemplatePath);
        }

        #endregion

        #region Class
        private static readonly string classTemplatePath = Path.Combine(templateDir, $"Class{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/Class", false, 4)]
        public static void CreateClassScript()
        {
            CreateScript(classTemplatePath);
        }

        private static readonly string staticClassTemplatePath = Path.Combine(templateDir, $"StaticClass{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/Static Class", false, 4)]
        public static void CreateStaticClassScript()
        {
            CreateScript(staticClassTemplatePath);
        }

        private static readonly string abstractClassTemplatePath = Path.Combine(templateDir, $"AbstractClass{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/Abstract/Class", false, 4)]
        public static void CreateAbstractClassScript()
        {
            CreateScript(abstractClassTemplatePath);
        }

        #endregion

        #region Interface

        private static readonly string interfaceTemplatePath = Path.Combine(templateDir, $"Interface{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/Interface", false, 4)]
        public static void CreateInterfaceScript()
        {
            CreateScript(interfaceTemplatePath);
        }

        #endregion
        
        #region Enum

        private static readonly string enumTemplatePath = Path.Combine(templateDir, $"Enum{templateAssetSuffix}");
        [MenuItem("Assets/Create/Scripts/Enum", false, 4)]
        public static void CreateEnumScript()
        {
            CreateScript(enumTemplatePath);
        }

        #endregion

        #region Markdown

        [MenuItem("Assets/Create/Scripts/Markdown", false, 4)]
        public static void CreateMarkdownScript()
        {
            CreateScript(Path.Combine(templateDir, $"Markdown{templateAssetSuffix}"));
        }

        #endregion

        #region Editor

        [MenuItem("Assets/Create/Scripts/Custom Editor", false, 4)]
        public static void CreateCustomEditorScript()
        {
            CreateScript(Path.Combine(templateDir, $"CustomEditor{templateAssetSuffix}"));
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

    static class CreateAction<T, I> where T : ScriptableObject, IEndActionInit<I>
    {
        /// <summary>
        /// Wraps scriptable object instance creation and init call of end action into single method.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static T Retreive(I i)
        {
            T so = ScriptableObject.CreateInstance<T>();
            so.Init(i);

            return so;
        }
    }

    interface IEndActionInit<I>
    {
        void Init(I i);
    }

    class EndAssetAction : EndNameEditAction, IEndActionInit<Type>
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

    public class EndScriptTemplateAction : EndNameEditAction, IEndActionInit<ScriptTemplate>
    {
        private ScriptTemplate scriptTemplate;
        private const string templateNamePlaceholder = "FileName";

        public void Init(ScriptTemplate template)
        {
            scriptTemplate = template;
        }

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            if (pathName == string.Empty) pathName = scriptTemplate.DefaultName;

            string name = Path.GetFileNameWithoutExtension(pathName);
            string code = scriptTemplate.Template.Replace(templateNamePlaceholder, name);

            File.WriteAllText(pathName, code);
            AssetDatabase.Refresh();

            if (string.IsNullOrEmpty(Path.GetExtension(pathName))) pathName += scriptTemplate.Extension;

            #if UNITY_EDITOR
            // Load the MonoScript asset
            MonoScript asset = AssetDatabase.LoadAssetAtPath<MonoScript>(pathName);

            // Choose your icon
            Texture2D icon = scriptTemplate.Icon;
            // Or for TextAsset: EditorGUIUtility.ObjectContent(null, typeof(TextAsset)).image as Texture2D;

            // Set the icon
            if (asset != null && icon != null)
                EditorGUIUtility.SetIconForObject(asset, icon);

            // Show in Project window
            ProjectWindowUtil.ShowCreatedAsset(asset);
            #endif

        }
    }
}