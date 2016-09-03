using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Babbel {

	public class TagsWindow : EditorWindow {

        public static string defaultTagLocation = "Assets/Data/Babbel/Tags";

        enum DisplayMode { All, Ranked, Used, UsedActive, NotUsed};

        Tag editTag;
        bool editTagNameChange = false;
        string editTagNewName;

        string filterQuery;
        DisplayMode displayMode = DisplayMode.All;

        [SerializeField]
        GUIStyle inactiveToggleButtons = null;
        [SerializeField]
        GUIStyle activeToggleButtons = null;

        GUIContent allMode = new GUIContent("All", "All registered tags");
        GUIContent rankedMode = new GUIContent("Ranked", "Sort tags based on usage");
        GUIContent usedMode = new GUIContent("Used", "Only tags in use");
        GUIContent usedActiveMode = new GUIContent("Used Active", "Only tags in active Babbel:Storyboard");
        GUIContent notUsedMode = new GUIContent("Not Used", "Only tags never used");

        
        [MenuItem("Window/Babbel/Tags...")]
		public static void ShowWindow() {

			EditorWindow window = EditorWindow.GetWindow (typeof(TagsWindow));
			window.titleContent = new GUIContent ("Babbel:Tags");

            window.Focus();
		}

        public void AssignStyles()
        {
            GUISkin skin = AssetTools.LoadByQuery<GUISkin>("BabbelSkin t:GUISkin");
            activeToggleButtons = skin.button;
            inactiveToggleButtons = skin.customStyles[0];

        }

		void OnGUI() {

            if (true || activeToggleButtons == null)
            {
                AssignStyles();
            }

            AddModalButtons();
            EditorGUILayout.Space();

            AddFilterField();
            EditorGUILayout.Space();

            if (editTag == null)
            {
                if (GUILayout.Button("New Tag"))
                {
                    EnsureFolder(defaultTagLocation);
                    editTag = AssetTools.CreateAsset<Tag>(defaultTagLocation); ;                    
                }
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                editTagNewName = EditorGUILayout.TextField("Name", string.IsNullOrEmpty(editTagNewName) ? editTag.name : editTagNewName);                
                if (EditorGUI.EndChangeCheck())
                {
                    editTagNameChange = true;                    
                }
                editTag.description = EditorGUILayout.TextArea(editTag.description);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Save and/or Close"))
                {
                    if (editTagNameChange)
                    {
                        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(editTag), editTagNewName);
                    }
                    AssetDatabase.SaveAssets();
                    editTag = null;
                    editTagNameChange = false;
                    editTagNewName = null;
                }
                else if (GUILayout.Button("Save and Keep"))
                {
                    if (editTagNameChange)
                    {
                        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(editTag), editTagNewName);
                    }
                    AssetDatabase.SaveAssets();
                    editTagNameChange = false;
                    editTagNameChange = false;
                    editTagNewName = null;
                }
                else if (GUILayout.Button("Delete"))
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(editTag));
                    editTag = null;
                    editTagNameChange = false;
                    editTagNewName = null;
                }

                EditorGUILayout.EndHorizontal();
            }

            AddTags();

		}

        void AddFilterField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Filter:");
            filterQuery = GUILayout.TextField(string.IsNullOrEmpty(filterQuery) ? "" : filterQuery, GUILayout.ExpandWidth(false), GUILayout.MinWidth(120));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        void AddModalButtons()
        {
            //Modal buttons
            GUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            LayOutModal(DisplayMode.All, allMode);
            LayOutModal(DisplayMode.Ranked, rankedMode);
            LayOutModal(DisplayMode.Used, usedMode);
            LayOutModal(DisplayMode.UsedActive, usedActiveMode);
            LayOutModal(DisplayMode.NotUsed, notUsedMode);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        void LayOutModal(DisplayMode mode, GUIContent content)
        {
            if (mode == displayMode)
            {
                GUILayout.Label(content, inactiveToggleButtons);
            } else
            {
                if (GUILayout.Button(content, activeToggleButtons))
                {
                    displayMode = mode;
                }
            }
        }

        void AddTags()
        {
            if (displayMode == DisplayMode.All)
            {
                string[] guids = AssetDatabase.FindAssets("t:Babbel.Tag");
                foreach (string guid in guids) {
                    AddTag(AssetDatabase.LoadAssetAtPath<Tag>(AssetDatabase.GUIDToAssetPath(guid)));
                }

            } else
            {
                EditorGUILayout.HelpBox("Feature not yet implemented", MessageType.Error);
            }
        }

        void AddTag(Tag tag)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(tag.name);
            GUILayout.Label(tag.description);
            GUILayout.EndHorizontal();
        }

        void EnsureFolder(string path)
        {
            string parent = null;
            foreach(string child in path.Split('/'))
            {
                if (string.IsNullOrEmpty(parent))
                {
                    parent = child;
                    continue;
                }

                string curPath = string.Join("/", new string[] { parent, child });

                if (AssetDatabase.IsValidFolder(curPath))
                {
                    parent = curPath;
                }
                else
                {
                    string guid = AssetDatabase.CreateFolder(parent, child);
                    parent = AssetDatabase.GUIDToAssetPath(guid);
                } 
            }
        }
	}

}