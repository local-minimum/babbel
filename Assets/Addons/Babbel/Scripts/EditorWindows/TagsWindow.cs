using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Babbel {

	public class TagsWindow : EditorWindow {

        enum DisplayMode { All, Ranked, Used, UsedActive, NotUsed};

        string filterQuery;
        DisplayMode displayMode = DisplayMode.All;

        public static GUIStyle inactiveToggleButtons = new GUIStyle();
        public static GUIStyle activeToggleButtons = new GUIStyle();

        GUIContent allMode = new GUIContent("All", "All registered tags");
        GUIContent rankedMode = new GUIContent("Ranked", "Sort tags based on usage");
        GUIContent usedMode = new GUIContent("Used", "Only tags in use");
        GUIContent usedActiveMode = new GUIContent("Used Active", "Only tags in active Babbel:Storyboard");
        GUIContent notUsedMode = new GUIContent("Not Used", "Only tags never used");

        [MenuItem("Window/Babbel/Tags...")]
		public static void ShowWindow() {

			EditorWindow window = EditorWindow.GetWindow (typeof(TagsWindow));
			window.titleContent = new GUIContent ("Babbel:Tags");
		}

		void OnGUI() {

            AddModalButtons();
            EditorGUILayout.Space();

            AddFilterField();
            EditorGUILayout.Space();

            AddTags();

		}

        void AddFilterField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Filter:");
            filterQuery = GUILayout.TextField(filterQuery, GUILayout.ExpandWidth(false), GUILayout.MinWidth(120));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        void AddModalButtons()
        {
            //Modal buttons
            GUILayout.BeginHorizontal();
            LayOutModal(DisplayMode.All, allMode);
            LayOutModal(DisplayMode.Ranked, rankedMode);
            LayOutModal(DisplayMode.Used, usedMode);
            LayOutModal(DisplayMode.UsedActive, usedActiveMode);
            LayOutModal(DisplayMode.NotUsed, notUsedMode);
            GUILayout.EndHorizontal();
        }

        void LayOutModal(DisplayMode mode, GUIContent content)
        {
            if (mode == displayMode)
            {
                GUILayout.Label(content);
            } else
            {
                if (GUILayout.Button(content))
                {
                    displayMode = mode;
                }
            }
        }

        void AddTags()
        {
            if (displayMode == DisplayMode.All)
            {

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
	}

}