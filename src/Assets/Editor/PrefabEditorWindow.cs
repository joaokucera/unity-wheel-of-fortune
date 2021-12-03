using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Editor
{
    public class PrefabEditorWindow : EditorWindow
    {
        [MenuItem("Window/Open PrefabEditorWindow")]
        public static void Init()
        {
            Texture icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Content/EditorTools/Images/ace.png");

            PrefabEditorWindow window = GetWindow<PrefabEditorWindow>();
            window.titleContent = new GUIContent("Prefab Editor Window", icon);
            window.autoRepaintOnSceneChange = true;
            window.Show();
        }

        private Object _searchDirectory;
        private Object _providedFileObject;
        private PrefabConfig[] _prefabConfigs;
        private string _assetFolder;
        private string _warningText;

        private void OnEnable()
        {
            _searchDirectory = null;
            _providedFileObject = null;
            _prefabConfigs = null;
            _assetFolder = null;
            _warningText = null;
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            ShowSearchPrefabsWithTextComponent();
            EditorGUILayout.Space();
            ShowProvidedFileObject();
            EditorGUILayout.Space();
            ShowApplyConfigToSelectedPrefabsButton();
            EditorGUILayout.Space();
            ShowCreatePrefabsButton();
            EditorGUILayout.Space();
            ShowWarningText();

            EditorGUILayout.EndVertical();
        }

        private void ShowSearchPrefabsWithTextComponent()
        {
            EditorGUILayout.BeginHorizontal();

            GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
                {alignment = TextAnchor.MiddleLeft, fontSize = 14, normal = {textColor = Color.white}};
            GUILayout.Label("Directory to search:", labelStyle);
            _searchDirectory = EditorGUILayout.ObjectField(_searchDirectory, typeof(Object), true);

            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.yellow;

            if (GUILayout.Button("Search Prefabs with Text Component", new GUIStyle(GUI.skin.button) {fontSize = 14}))
            {
                SearchAndSelectPrefabsWithTextComponent();
            }

            GUI.backgroundColor = Color.white;
        }

        private void SearchAndSelectPrefabsWithTextComponent()
        {
            List<GameObject> allAssetsWithTextComponent = new List<GameObject>();

            string directoryPath = AssetDatabase.GetAssetPath(_searchDirectory);
            string[] guids = AssetDatabase.FindAssets("t:Prefab",
                new[] {Directory.Exists(directoryPath) ? directoryPath : "Assets"});

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Object[] allAssetsAtPath = AssetDatabase.LoadAllAssetsAtPath(assetPath);

                PrefabConfigUtils.GetAllPrefabsWithTextComponent(allAssetsAtPath, ref allAssetsWithTextComponent);
            }

            Selection.objects = allAssetsWithTextComponent.ToArray();
        }

        private void ShowProvidedFileObject()
        {
            EditorGUILayout.BeginHorizontal();

            GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
                {alignment = TextAnchor.MiddleLeft, fontSize = 14, normal = {textColor = Color.white}};
            GUILayout.Label("Drag&drop a file to create prefabs:", labelStyle);
            _providedFileObject = EditorGUILayout.ObjectField(_providedFileObject, typeof(Object), true);

            EditorGUILayout.EndHorizontal();

            if (_providedFileObject == null)
            {
                return;
            }

            string assetPath = AssetDatabase.GetAssetPath(_providedFileObject);
            ValidateProvidedFileObject(assetPath, _providedFileObject);

            _providedFileObject = null;
        }

        private void ValidateProvidedFileObject(string assetPath, Object assetAtPath)
        {
            try
            {
                _warningText = null;
                _prefabConfigs = null;
                _assetFolder = Path.GetDirectoryName(assetPath);

                if (assetAtPath is TextAsset textAsset)
                {
                    string jsonString = JsonHelper.FixJson(textAsset.text);
                    _prefabConfigs = JsonHelper.FromJson<PrefabConfig>(jsonString);

                    foreach (PrefabConfig prefabConfig in _prefabConfigs)
                    {
                        if (prefabConfig.Validate(_assetFolder))
                        {
                            continue;
                        }

                        if (string.IsNullOrEmpty(_warningText))
                        {
                            _warningText =
                                $"Provided file {assetAtPath.name} | {assetAtPath.GetType()} has invalid content!";
                        }

                        _warningText += $"\nPrefabConfig: {prefabConfig}!";
                    }
                }
                else
                {
                    _warningText = $"Provided file {assetAtPath.name} | {assetAtPath.GetType()} is not a TextAsset!";
                }
            }
            catch (Exception exception)
            {
                _warningText =
                    $"Provided file {assetAtPath.name} | {assetAtPath.GetType()} is not properly formatted!\n{exception}";
            }

            if (!string.IsNullOrEmpty(_warningText))
            {
                _prefabConfigs = null;
            }
        }

        private void ShowApplyConfigToSelectedPrefabsButton()
        {
            if (_prefabConfigs == null || Selection.objects == null || Selection.objects.Length == 0)
            {
                return;
            }

            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("Apply Config To Selected Prefabs", new GUIStyle(GUI.skin.button) {fontSize = 14}))
            {
                ApplyConfigToSelectedPrefabs();
            }

            GUI.backgroundColor = Color.white;
        }

        private void ApplyConfigToSelectedPrefabs()
        {
            if (Selection.objects == null || Selection.objects.Length == 0)
            {
                return;
            }

            List<GameObject> allAssetsWithTextComponent = new List<GameObject>();
            PrefabConfigUtils.GetAllPrefabsWithTextComponent(Selection.objects, ref allAssetsWithTextComponent);

            foreach (GameObject go in allAssetsWithTextComponent)
            {
                foreach (PrefabConfig prefabConfig in _prefabConfigs)
                {
                    if (go.name != prefabConfig.Text)
                    {
                        continue;
                    }

                    PrefabConfigUtils.ApplyConfigToGameObjectWithTextComponent(prefabConfig, go);
                    Undo.RecordObject(go, "Apply Config To Selected Prefab Undo");

                    break;
                }
            }
        }

        private void ShowCreatePrefabsButton()
        {
            if (_prefabConfigs == null)
            {
                return;
            }

            GUI.backgroundColor = Color.cyan;

            if (GUILayout.Button("Create Prefabs", new GUIStyle(GUI.skin.button) {fontSize = 14}))
            {
                CreatePrefabs();
            }

            GUI.backgroundColor = Color.white;
        }

        private void CreatePrefabs()
        {
            if (_prefabConfigs == null || string.IsNullOrEmpty(_assetFolder))
            {
                return;
            }

            string prefabsFolder = $"{_assetFolder}/Prefabs";
            if (!Directory.Exists(prefabsFolder))
            {
                Directory.CreateDirectory(prefabsFolder);
            }

            foreach (PrefabConfig prefabConfig in _prefabConfigs)
            {
                string prefabConfigPath = $"{prefabsFolder}/{prefabConfig.Text}.prefab";
                prefabConfigPath = AssetDatabase.GenerateUniqueAssetPath(prefabConfigPath);

                GameObject go = new GameObject(prefabConfig.Text, typeof(Text), typeof(SpriteRenderer));
                PrefabConfigUtils.ApplyConfigToGameObjectWithTextComponent(prefabConfig, go);

                GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(go, prefabConfigPath, InteractionMode.UserAction);
                
                Undo.RegisterCreatedObjectUndo(prefab, "Create Prefab");
                Undo.DestroyObjectImmediate(go);
            }
        }

        private void ShowWarningText()
        {
            if (!string.IsNullOrEmpty(_warningText))
            {
                GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
                    {alignment = TextAnchor.MiddleCenter, fontSize = 14, normal = {textColor = Color.yellow}};
                GUILayout.Label(_warningText, labelStyle);
            }
        }
    }
}