using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Babbel
{
    public class StoryBoardWindow : AbstractBabbelWindow
    {

        bool expandedMenu = false;
        Rect menuPosition = new Rect(4, 4, 19, 19);
        Rect expandedMenuPosition = new Rect(25, 4, 0, 120);

        ScriptableObject scene = null;
        ScriptableObject act = null;

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

            if (GUI.Button(menuPosition, theme.Menu, expandedMenu ? theme.TitleActive : theme.Title)) {
                expandedMenu = !expandedMenu;
            }

            expandedMenuPosition.width = maxSize.x - (4 + expandedMenuPosition.x);

            GUILayout.BeginArea(expandedMenuPosition);


            if (expandedMenu)
            {

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

                if (GUILayout.Button(theme.Add, act == null ? theme.DownStateToggle : theme.UpStateToggle))
                {
                    if (act == null)
                    {

                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            } else
            {
                GUILayout.BeginHorizontal();
                if (act == null)
                {
                    GUILayout.Label(theme.NoAct, theme.Title);
                } else
                {
                    GUILayout.Label(act.name, theme.Title);

                    if (scene == null)
                    {
                        GUILayout.Label(theme.NoScene, theme.Text);
                    } else
                    {
                        GUILayout.Label(scene.name, theme.Text);
                    }
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndArea();

        }

    }

}
