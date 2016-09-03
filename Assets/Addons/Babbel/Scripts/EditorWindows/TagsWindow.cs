using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Babbel {

	public class TagsWindow : EditorWindow {

        public static string defaultTagLocation = "Assets/Data/Babbel/Tags";

        public static string filterFieldName = "babbel:tags_window:filter_field";

        enum DisplayMode { All, Ranked, Used, UsedActive, NotUsed};

        Tag editTag;
        bool editTagNameChange = false;
        string editTagNewName;
        string editTagNewDescription;
        bool editGiveFocus = true;

        string filterQuery;
        DisplayMode displayMode = DisplayMode.All;

        [SerializeField]
        BabbelEditorTheme theme;       
        
        [MenuItem("Window/Babbel/Tags...")]
		public static void ShowWindow() {

			EditorWindow window = EditorWindow.GetWindow (typeof(TagsWindow));
			window.titleContent = new GUIContent ("Babbel:Tags");

            window.Focus();
		}

        public void SetTheme(BabbelEditorTheme theme=null)
        {
            if (theme == null)
            {
                theme = AssetTools.LoadByQuery<BabbelEditorTheme>("t:BabbelEditorTheme");
            }
            this.theme = theme;
        }

		void OnGUI() {

            if (theme == null)
            {
                SetTheme();
            }

            AddModalButtons();

            EditorGUILayout.Space();

            AddEditArea();

            EditorGUILayout.Space();

            AddFilterField();
            AddTags();

            GUILayout.FlexibleSpace();


        }

        void AddEditArea()
        {
            if (editTag == null)
            {
                if (GUILayout.Button(theme.Add, theme.UpStateToggle))
                {
                    EnsureFolder(defaultTagLocation);
                    editTag = AssetTools.CreateAsset<Tag>(defaultTagLocation); ;
                }
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                editTagNewName = EditorGUILayout.TextField(string.IsNullOrEmpty(editTagNewName) ? editTag.name : editTagNewName, theme.Title);
                if (EditorGUI.EndChangeCheck())
                {
                    editTagNameChange = true;
                }
                editTagNewDescription = EditorGUILayout.TextField(string.IsNullOrEmpty(editTagNewDescription) ? editTag.description : editTagNewDescription, theme.Text);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(theme.SaveClose, theme.UpStateToggle))
                {
                    if (editTagNewDescription != null) {
                        editTag.description = editTagNewDescription;
                    }
                    if (editTagNameChange)
                    {
                        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(editTag), editTagNewName);
                    }
                    AssetDatabase.SaveAssets();
                    ClearEditTagEdit();
                }
                else if (GUILayout.Button(theme.Save, theme.UpStateToggle))
                {
                    if (editTagNameChange)
                    {
                        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(editTag), editTagNewName);
                    }
                    AssetDatabase.SaveAssets();
                    ClearEditTagEdit(true);
                }
                else if (GUILayout.Button(theme.Close, theme.UpStateToggle))
                {
                    ClearEditTagEdit();
                }
                else if (GUILayout.Button(theme.Delete, theme.UpStateToggle))
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(editTag));
                    ClearEditTagEdit();
                }

                EditorGUILayout.EndHorizontal();
            }

        }

        void ClearEditTagEdit(bool keepTag=false)
        {
            if (!keepTag)
            {
                editTag = null;
            }

            editTagNameChange = false;
            editTagNewDescription = null;
            editTagNewName = null;
            editGiveFocus = true;          
        }

        void AddFilterField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(theme.Filter, theme.Title);
            GUI.SetNextControlName(filterFieldName);
            filterQuery = GUILayout.TextField(string.IsNullOrEmpty(filterQuery) ? "" : filterQuery, theme.IconAligningInput);
            if (editGiveFocus)
            {
                GUI.FocusControl(filterFieldName);
                editGiveFocus = false;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        void AddModalButtons()
        {
            //Modal buttons
            GUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            LayOutModal(DisplayMode.All, theme.TagModeAll);
            LayOutModal(DisplayMode.Ranked, theme.TagModeRanked);
            LayOutModal(DisplayMode.Used, theme.TagModeUsed);
            LayOutModal(DisplayMode.UsedActive, theme.TagModeUsedActive);
            LayOutModal(DisplayMode.NotUsed, theme.TagModeNotUsed);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        void LayOutModal(DisplayMode mode, GUIContent content)
        {
            if (mode == displayMode)
            {
                GUILayout.Label(content, theme.DownStateToggle);
            } else
            {
                if (GUILayout.Button(content, theme.UpStateToggle))
                {
                    displayMode = mode;
                }
            }
        }

        void AddTags()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            Vector2 pos = Vector2.zero;
            pos = GUILayout.BeginScrollView(pos, false, true);
            if (displayMode == DisplayMode.All)
            {
                string[] guids = AssetDatabase.FindAssets("t:Babbel.Tag");
                foreach (string guid in guids) {
                    Tag tag = AssetDatabase.LoadAssetAtPath<Tag>(AssetDatabase.GUIDToAssetPath(guid));
                    if (!string.IsNullOrEmpty(filterQuery)) {
                        if (tag.name.Contains(filterQuery) || tag.description.Contains(filterQuery))
                        {
                            AddTag(tag);
                        }
                    } else {
                        AddTag(tag);
                    }
                }

            } else
            {
                EditorGUILayout.HelpBox("Feature not yet implemented", MessageType.Error);
            }
            GUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
        }

        void AddTag(Tag tag)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(tag.name, theme.Title))
            {
                ClearEditTagEdit();
                editTag = tag;
            }
            GUILayout.Label(tag.description, theme.Text);
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