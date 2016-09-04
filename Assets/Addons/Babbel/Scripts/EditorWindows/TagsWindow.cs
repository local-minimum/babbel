using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Babbel {

	public class TagsWindow : AbstractBabbelWindow {

        public static string defaultTagLocation = "Assets/Data/Babbel/Tags";

        public static string filterFieldName = "babbel:tags_window:filter_field";

        enum DisplayMode { All, Ranked, Used, UsedActive, NotUsed};

        Tag editTag;
        bool editTagNameChange = false;
        string editTagNewName;
        string editTagNewDescription;
        bool editGiveFocus = true;
        Vector2 tagScrollPosition = Vector2.zero;

        string filterQuery;
        DisplayMode displayMode = DisplayMode.All;     
        
        [MenuItem("Window/Babbel/Tags", priority = 1)]
		public static void ShowWindow() {

			EditorWindow window = EditorWindow.GetWindow (typeof(TagsWindow));	
            window.Focus();
		}

        protected override void OnGUI()
        {
            base.OnGUI();
            
            AddModalButtons();

            EditorGUILayout.Space();

            AddEditArea();

            EditorGUILayout.Space();

            AddFilterField();
            AddTags();

        }

        void AddEditArea()
        {
            if (editTag == null)
            {
                if (GUILayout.Button(theme.Add, theme.UpStateToggle))
                {
                    AssetTools.EnsureFolder(defaultTagLocation);
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
                    SaveEditTag();
                    ClearEditTagEdit();
                }
                else if (GUILayout.Button(theme.Save, theme.UpStateToggle))
                {
                    SaveEditTag(); 
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

        void SaveEditTag()
        {
            if (editTagNewDescription != null)
            {
                editTag.description = editTagNewDescription;
            }
            if (editTagNameChange)
            {
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(editTag), editTagNewName);
            }
            AssetDatabase.SaveAssets();
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
            tagScrollPosition = GUILayout.BeginScrollView(tagScrollPosition, false, false);
            List<Tag> tags = GetTags();
            if (tags != null)
            {
                foreach (Tag tag in tags) {
                    
                    if (tag == null)
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(filterQuery)) {
                        if (tag.Matches(filterQuery))
                        {
                            AddTag(tag);
                        }
                    } else {
                        AddTag(tag);
                    }
                }

            } else
            {
                if (displayMode == DisplayMode.UsedActive)
                {
                    EditorGUILayout.HelpBox("The Babbel:Story window doesn't have anything selected", MessageType.Info);
                }
                else {
                    EditorGUILayout.HelpBox("Feature not yet implemented", MessageType.Error);
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
        }

        List<Tag> GetTags()
        {

            List<Tag> tags = new List<Tag>();
                                  
            if (displayMode == DisplayMode.All)
            {
                foreach (string guid in GetGUIDs())
                {                
                    tags.Add(AssetDatabase.LoadAssetAtPath<Tag>(AssetDatabase.GUIDToAssetPath(guid)));
                }
            }
            else if (displayMode == DisplayMode.Used)
            {
                tags.AddRange(StoryBoardWindow.Story.All<Tag>());

            } else if (displayMode == DisplayMode.NotUsed)
            {
                foreach (string guid in GetGUIDs())
                {
                    Tag tag = AssetDatabase.LoadAssetAtPath<Tag>(AssetDatabase.GUIDToAssetPath(guid));
                    if (!StoryBoardWindow.Story.Contains(tag))
                    {                        
                        tags.Add(tag);
                    }
                }
            }
            else if (displayMode == DisplayMode.UsedActive)
            {
                if (StoryBoardWindow.ActiveBoard == null)
                {
                    return null;
                } else {
                    tags.AddRange(StoryBoardWindow.ActiveBoard.All<Tag>());
                }
            }
            else
            {
                return null;
            }

            return tags;
        }

        string[] GetGUIDs()
        {
            return  AssetDatabase.FindAssets("t:Babbel.Tag");
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

	}

}