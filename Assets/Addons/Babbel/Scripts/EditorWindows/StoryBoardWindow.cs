using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Babbel
{
    public class StoryBoardWindow : AbstractBabbelWindow
    {

        bool expandedMenu = false;
        Rect menuPosition = new Rect(4, 4, 16, 16);
        Rect expandedMenuPosition = new Rect(22, 4, 0, 120);


        [MenuItem("Window/Babbel/Storyboard", priority = 0)]
        public static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow(typeof(StoryBoardWindow));           
            window.Focus();
        }


        protected override void OnGUI()
        {
            base.OnGUI();
            AddMenu();
        }

        void AddMenu()
        {
            
            if (GUI.Button(menuPosition, theme.Menu, theme.Title)) {
                expandedMenu = !expandedMenu;
            }

            if (expandedMenu)
            {
                expandedMenuPosition.width = maxSize.x - (4 + expandedMenuPosition.x);

                GUILayout.BeginArea(expandedMenuPosition);

                //ACTS

                GUILayout.BeginHorizontal();
                GUILayout.Label(theme.StoryActs, theme.Title);

                //TODO: List Acts

                if (GUILayout.Button(theme.Add, theme.UpStateToggle))
                {

                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                //SCENES

                GUILayout.BeginHorizontal();
                GUILayout.Label(theme.StoryScenes, theme.Title);

                //TODO: List Scenes

                if (GUILayout.Button(theme.Add, theme.UpStateToggle))
                {

                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.EndArea();
            }
        }

    }

}
